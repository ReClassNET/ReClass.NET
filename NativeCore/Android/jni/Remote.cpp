#include "Remote.h"
#include "LinuxSocketWrapper.h"
#include "API.h"
#include <unordered_map>
#include <Packet.h>
#include "Utils.h"
#include "dirent.h"
#include "LinuxProcess.h"
#include <fstream>
#include "ReClassNET_Plugin.hpp"
#include <SnapshotBase.h>

enum {
    SNAPSHOT_PROCESSES,
    SNAPSHOT_MODULES,
    SNAPSHOT_SECTIONS
};

struct OpenedProcessInfo;
struct SnapshotProcesses;
struct SnapshotSections;
struct SnapshotModules;

std::unordered_map<ClientSocketLinux*, OpenedProcessInfo*> gProcs;    
std::unordered_map<ClientSocketLinux*, std::vector<SnapshotSections*>> gSnapSecs; 
std::unordered_map<ClientSocketLinux*, std::vector<SnapshotProcesses*>> gSnapProcs;
std::unordered_map<ClientSocketLinux*, std::vector<SnapshotModules*>> gSnapMods;

struct SnapshotProcesses : SnapshotBase<ProcessInfo>{

    SnapshotProcesses()
    {
        type = SNAPSHOT_PROCESSES;

        DIR* pProcDir = opendir("/proc/");

        if(pProcDir)
        {
            struct dirent* pDirEnt = nullptr;
            
            while((pDirEnt = readdir(pProcDir)))
            {
                uint32_t currPid = atoi(pDirEnt->d_name);

                if(currPid)
                {
                    char currCmdLinePath[256] {};

                    sprintf(currCmdLinePath, "/proc/%d/cmdline", int(currPid));

                    FILE* fCmdLine = fopen(currCmdLinePath, "r");

                    if(fCmdLine)
                    {
                        char cmdLineBuff[MAX_PROC_NAME * sizeof(char)];
                        std::string cmdLineStr = "";

                        while (fgets(cmdLineBuff, sizeof(cmdLineBuff), fCmdLine)) 
                            cmdLineStr += std::string(cmdLineBuff);

                        ProcessInfo pi;

                        pi.mProcessId = currPid;
                        strcpy(pi.mProcessName, cmdLineStr.c_str());

                        //printf("%s %d\n", pi.mProcessName, pi.mProcessId);

                        mArray.push_back(pi);

                        fclose(fCmdLine);
                    }
                }
            }

            closedir(pProcDir);
        }
    }

    static std::unordered_map<ClientSocketLinux*, std::vector<SnapshotProcesses*>>& getSnaps() {return gSnapProcs;}
};

bool getMapsSegments(uint32_t pid, std::vector<SectionInfo>& outSeg)
{
    char mapsPath[260] {};
    sprintf(mapsPath, "/proc/%d/maps", int(pid));

    printf("Opening %s\n", mapsPath);

    FILE *fp = fopen(mapsPath,"r");

    if (fp)
    {
        char* line =(char*) malloc(4096);
        //printf("%s Opened\n", mapsPath);

        if(line)
        {
            while (fgets(line,4096, fp)) {
                SectionInfo seg{};

                //printf("%s\n", line);

                uint64_t offset, unk;
                char r;
                char w;
                char x;
                char m;

                sscanf(line,"%lx-%lx %c%c%c%c %lx %lx:%lx %lx %260[^\n\t]", &seg.mStart, &seg.mEnd, &r, &w, &x, &m, &offset, &unk, &unk, &unk, seg.mPath);

                //printf("%lx-%lx %s\n", seg.mStart, seg.mEnd, seg.mPath);

                if(r == 'r')
                    seg.mProt |= SectionProtection::Read;

                if(w == 'w')
                    seg.mProt |= SectionProtection::Write;

                if(x == 'x')
                    seg.mProt |= SectionProtection::Execute;

                if(m == 'p')
                {
                    seg.mType = SectionType::Private;
                    seg.mProt |= SectionProtection::CopyOnWrite;
                }

                if(offset != 0)
                {
                    seg.mType = SectionType::Image;
                    if(outSeg.size() > 0)
                        outSeg[outSeg.size() - 1].mType = SectionType::Image;
                }

                outSeg.push_back(seg);
            }

        
            free(line);
            printf("%d Segments Parsed\n", outSeg.size());
            return true;
        }
    } 

    return false;
}

struct SnapshotSections : SnapshotBase<SectionInfo>{
    SnapshotSections(uint32_t pid)
    {
        type = SNAPSHOT_SECTIONS;

        printf("Creating SnapshotSections PID %d\n", pid);

        getMapsSegments(pid, mArray);

        printf("SnapshotSections Created PID %d, %d Sections\n", mArray.size());
        
    }

    static std::unordered_map<ClientSocketLinux*, std::vector<SnapshotSections*>>& getSnaps() {return gSnapSecs;}
};

struct SnapshotModules : SnapshotBase<ModuleInfo>{
    SnapshotModules(uint32_t pid)
    {
        type = SNAPSHOT_MODULES;

        printf("Creating Snap Modules -> PID %d\n", pid);

        SnapshotSections snapshotSect = SnapshotSections(pid);
        std::unordered_map <std::string, std::vector<SectionInfo*>> modMatchs;
        
        SectionInfo* si;

        printf("Parsing Snap Sections\n", pid);

        if(snapshotSect.PopFirst(&si))
        {
            do {
                if(strstr(si->mPath, ".so"))
                {
                    std::string mPathStr = std::string(si->mPath);

                    if(modMatchs.find(mPathStr) == modMatchs.end())
                        modMatchs[mPathStr] = std::vector<SectionInfo*>();

                    modMatchs[mPathStr].push_back(si);
                }
            } while(snapshotSect.PopNext(&si));
        }

        for(const auto& kv : modMatchs)
        {
            if(kv.second.size() > 1)
            {
                ModuleInfo currMi;
                
                const auto* pFirst = kv.second[0];
                const auto* pLast = kv.second[kv.second.size() - 1];

                currMi.mBase = pFirst->mStart;
                currMi.mSize = pLast->mEnd -currMi.mBase;
                strcpy(currMi.mPath, pFirst->mPath);

                mArray.push_back(currMi);
            }
        }

        printf("%d Modules Found\n", mArray.size());
    }

    static std::unordered_map<ClientSocketLinux*, std::vector<SnapshotModules*>>& getSnaps() {return gSnapMods;}
};

struct OpenedProcessInfo{
    ClientSocketLinux* pOwner;
    LinuxProcess* mProcess;
};

bool CreateToolSecs(uint32_t pid, SnapshotSections** outSnap)
{
    *outSnap = new SnapshotSections(pid);

    return true;
}

bool CreateToolMods(uint32_t pid, SnapshotModules** outSnap)
{
    *outSnap = new SnapshotModules(pid);

    return true;
}

bool CreateToolProcs(SnapshotProcesses** outSnap)
{
    *outSnap = new SnapshotProcesses();

    return *outSnap != 0;
}


void HandleCreateToolProcs(ClientSocketLinux* pClientUnique, HANDLE_API& outHandle)
{
    SnapshotProcesses* pProcsSnap = nullptr;

    if(CreateToolProcs(&pProcsSnap))
    {
        gSnapProcs[pClientUnique].push_back(pProcsSnap);
        outHandle = gSnapProcs[pClientUnique].size() - 1;
    }
}

void HandleCreateToolMods(ClientSocketLinux* pClientUnique, uint32_t pid, HANDLE_API& outHandle)
{
    SnapshotModules* pModsSnap = nullptr;

    if(CreateToolMods(pid, &pModsSnap))
    {
        gSnapMods[pClientUnique].push_back(pModsSnap);
        outHandle = gSnapMods[pClientUnique].size() - 1;
    }

}

void HandleCreateToolSecs(ClientSocketLinux* pClientUnique, uint32_t pid, HANDLE_API& outHandle)
{
    SnapshotSections* pSecsSnap = nullptr;

    if(CreateToolSecs(pid, &pSecsSnap))
    {
        gSnapSecs[pClientUnique].push_back(pSecsSnap);
        outHandle = gSnapSecs[pClientUnique].size() - 1;
    }
}

void HandleCmdCreateToolHelp(ClientSocketLinux* pClientUnique, void* pInPacket)
{
    Packet<CreateRemoteToolHelpIn, 0>* packet = (Packet<CreateRemoteToolHelpIn, 0>*)pInPacket;
    CreateRemoteToolHelpOut resultPacket;
    HANDLE_API& rHandleResult = resultPacket.mHandleTool;
    uint32_t pid = packet->getPayload()->mProcessId;

    //packet->Print();

    printf("Creating Tool %d pid %d\n", packet->getPayload()->mToolType, packet->getPayload()->mProcessId);

    rHandleResult = HandleValue::INVALID;

    switch(packet->getPayload()->mToolType)
    {
        case ToolType::PROCS:   HandleCreateToolProcs(pClientUnique, rHandleResult); break;
        case ToolType::MODULES: HandleCreateToolMods(pClientUnique, pid, rHandleResult); break;
        case ToolType::SECTIONS: HandleCreateToolSecs(pClientUnique, pid, rHandleResult); break;
    }

    if(rHandleResult != HandleValue::INVALID)
        printf("Handle Tool Created %d\n", rHandleResult);

    pClientUnique->Send(&resultPacket, sizeof(resultPacket));
}

void HandleCmdHi(ClientSocketLinux* pClientSocket)
{
    pClientSocket->SendMsg(CMD_WELLCOME);
}

bool ProcessAttached(ClientSocketLinux* pClientSocket)
{
    bool bIsAttached = false;

    OpenedProcessInfo* pOpenedProcInf = gProcs[pClientSocket];

    if(pOpenedProcInf)
    {
        if(pOpenedProcInf->mProcess)
        {
            if(pOpenedProcInf->mProcess->getMemFd() != -1)
                bIsAttached = true;
        }
    }

    return bIsAttached;
}

void DetachProcess(ClientSocketLinux* pClientSocket)
{
    OpenedProcessInfo* pOpenedProcInf = gProcs[pClientSocket];

    if(pOpenedProcInf)
    {
        if(pOpenedProcInf->mProcess) delete pOpenedProcInf->mProcess;
        delete pOpenedProcInf;
        gProcs[pClientSocket] = nullptr;
    }
}

void HandleDisconnect(ClientSocketLinux* pClientSocket)
{
    DetachProcess(pClientSocket);
    printf("Client Disconnected\n");
}

template<typename T>
bool ValidToolHandle(std::unordered_map<ClientSocketLinux*, std::vector<T*>>& rSnap, ClientSocketLinux* pClientUnique, HANDLE_API handle, bool bCheckRange = true)
{
    if(bCheckRange)
    {
        if(!(handle >= 0 && handle < rSnap[pClientUnique].size()))
            return false;
    }

    return rSnap[pClientUnique][handle] != nullptr; // Vector Alredy Created on entry
                                                    // Dont worry!
}

void HandleCmdRemoveProcsToolHelp(ClientSocketLinux* pClientUnique, void* pInPacket)
{
    Packet<RemoveRemoteProcsToolHelpIn, 0>* pckt = ((Packet<RemoveRemoteProcsToolHelpIn, 0>*)pInPacket); 
    HANDLE_API hTool = pckt->getPayload()->mHandleSnap;

    printf("Removing Tool Procs %d\n",hTool);

    if(ValidToolHandle<SnapshotProcesses>(gSnapProcs, pClientUnique, hTool)) {
        delete (gSnapProcs[pClientUnique][hTool]);
        gSnapProcs[pClientUnique][hTool] = nullptr;

        printf("Procs Tool Removed %d\n",hTool);
    }
}

void HandleCmdStatus(ClientSocketLinux* pClientUnique)
{
    //printf("Process Ping\n");
    pClientUnique->SendMsg(CMD_OK);
}

void HandleCmdDisconnect(ClientSocketLinux* pClientUnique)
{
    HandleDisconnect(pClientUnique);
    pClientUnique->SendMsg(CMD_OK);
}

template<typename ToolType, typename ToolIn, typename ToolOut>
void HandleToolOp(ClientSocketLinux* pClientSock, void* pInPacket, bool bFirst, std::function<void(const ToolOut&)> fnPostValidOp = nullptr)
{
    Packet<ToolIn, 0>* pcktIn = (Packet<ToolIn, 0>*)pInPacket;
    ToolOut pOut{};
    HANDLE_API hSnap = pcktIn->getPayload()->mHandleSnap;
    pOut.mRemaining = false;
    auto& rSnaps = ToolType::getSnaps();

    if(ValidToolHandle<ToolType>(rSnaps, pClientSock, hSnap))
    {
        ToolType* pSnap = rSnaps[pClientSock][hSnap];

        pOut.mRemaining = bFirst ? pSnap->PopFirst(pOut.mInfo) : pSnap->PopNext(pOut.mInfo);

        if(fnPostValidOp)
            fnPostValidOp(pOut);
    }

    pClientSock->Send(&pOut, sizeof(pOut));
}

void HandleCmdOpenProcess(ClientSocketLinux* pClientUnique, void* pInPacket){
    Packet<OpenRemoteProcessIn, 0>* pcktIn = (Packet<OpenRemoteProcessIn, 0>*)pInPacket;
    OpenRemoteProcessOut pOpenProcOut {};

    if(ProcessAttached(pClientUnique)) 
        DetachProcess(pClientUnique);

    pOpenProcOut.hProc = HandleValue::INVALID;
    OpenedProcessInfo openedProcInf {};

    printf("Opening Process PID %d\n", pcktIn->getPayload()->mProcessId);

    openedProcInf.mProcess = new LinuxProcess(pcktIn->getPayload()->mProcessId);

    if(openedProcInf.mProcess)
    {
        pOpenProcOut.hProc = (HANDLE_API)openedProcInf.mProcess->getMemFd();

        if(pOpenProcOut.hProc != -1)
        {
            openedProcInf.pOwner = pClientUnique;
            gProcs[pClientUnique] = new OpenedProcessInfo(openedProcInf);
            printf("Process Opened %d\n", pOpenProcOut.hProc);
        } else delete openedProcInf.mProcess;
    }

    pClientUnique->Send(&pOpenProcOut, sizeof(pOpenProcOut));
}

void HandleCmdReadMemory(ClientSocketLinux* pClientUnique, void* pInPacket)
{
    Packet<ReadMemoryIn, 0>* pckt = (Packet<ReadMemoryIn, 0>*)pInPacket;
    HANDLE_API hProc = pckt->getPayload()->mhProc;
    unsigned char packetHolder[MAX_PACKET_SIZE]{};
    ReadMemoryOut* pRdMemOut = (ReadMemoryOut*)packetHolder;
    const OpenedProcessInfo* hProcInf = gProcs[pClientUnique];

    pRdMemOut->mBytesReaded = -1;

    if(hProcInf && hProc == hProcInf->mProcess->getMemFd())
    {
       // printf("Reading %d bytes\n", int(pckt->getPayload()->mSize));
        pRdMemOut->mBytesReaded = hProcInf->mProcess->ReadMemory(pckt->getPayload()->mAddr, pRdMemOut->mBuff, pckt->getPayload()->mSize);
    }

    //printf("%d Bytes Readed\n", int(pRdMemOut->mBytesReaded));

    pClientUnique->Send(pRdMemOut, pRdMemOut->mBytesReaded != -1 ? pRdMemOut->mBytesReaded + sizeof(int64_t) : sizeof(int64_t));
}

void HandleCmdWriteMemory(ClientSocketLinux* pClientUnique, void* pInPacket)
{
    Packet<WriteMemoryIn, 0>* pckt = (Packet<WriteMemoryIn, 0>*)pInPacket;
    HANDLE_API hProc = pckt->getPayload()->mhProc;
    WriteMemoryOut pWrMemOut{};
    const OpenedProcessInfo* hProcInf = gProcs[pClientUnique];
    
    pWrMemOut.mBytesWrited = -1;

     if(hProcInf && hProc == hProcInf->mProcess->getMemFd())
    {
        printf("Writing %d bytes\n", int(pckt->getPayload()->mSize));
        pWrMemOut.mBytesWrited = hProcInf->mProcess->WriteMemory(pckt->getPayload()->mAddr, pckt->getPayload()->mBuff, pckt->getPayload()->mSize);
    }

    printf("%d Bytes Writed\n", int(pWrMemOut.mBytesWrited));
    pClientUnique->Send(&pWrMemOut, sizeof(pWrMemOut));
}

std::function<void(ClientSocketLinux*)> gfnHandleConn = [](ClientSocketLinux* pClientSocket)
{
    unsigned char packet[MAX_PACKET_SIZE + 1]{0};
    short* pCmdId = (short*)packet;
    bool bDisconnecting = false;
    uintptr_t outRecvSize = 0;
    bool bFirstTime = true;

    gSnapProcs[pClientSocket] = std::vector<SnapshotProcesses*>();
    gSnapMods[pClientSocket] = std::vector<SnapshotModules*>();
    gProcs[pClientSocket] = nullptr;

    printf("Client Connected\n");

    while(!bDisconnecting && pClientSocket->Recive(packet, MAX_PACKET_SIZE, &outRecvSize))
    {
        if(bFirstTime)
        {
            if(*pCmdId != CMD_HI)
                return;

            bFirstTime = false;
        }

        switch(*pCmdId)
        {
            case CMD_HI: HandleCmdHi(pClientSocket); break;
            case CMD_CREATE_REMOTE_TOOL_HELP: HandleCmdCreateToolHelp(pClientSocket, packet); break;
            case CMD_PROCESS_FIRST: HandleToolOp<SnapshotProcesses, ProcessFirstIn, ProcessFirstOut>(pClientSocket, packet, true); break;
            case CMD_PROCESS_NEXT: HandleToolOp<SnapshotProcesses, ProcessNextIn, ProcessNextOut>(pClientSocket, packet, false); break;
            case CMD_MODULE_FIRST: HandleToolOp<SnapshotModules, ModuleFirstIn, ModuleFirstOut>(pClientSocket, packet, true); break;
            case CMD_MODULE_NEXT: HandleToolOp<SnapshotModules, ModuleNextIn, ModuleNextOut>(pClientSocket, packet, false); break;
            case CMD_SECTION_FIRST: HandleToolOp<SnapshotSections, SectionFirstIn, SectionFirstOut>(pClientSocket, packet, true); break;
            case CMD_SECTION_NEXT: HandleToolOp<SnapshotSections, SectionNextIn, SectionNextOut>(pClientSocket, packet, false); break;
            case CMD_REMOVE_REMOTE_PROCS_TOOL_HELP: HandleCmdRemoveProcsToolHelp(pClientSocket, packet); break;
            //case CMD_REMOVE_REMOTE_MODS_TOOL_HELP: HandleCmdRemoveProcsToolHelp(pClientSocket, packet); break; // TODO
            //case CMD_REMOVE_REMOTE_SECS_TOOL_HELP: HandleCmdRemoveProcsToolHelp(pClientSocket, packet); break; // TODO
            case CMD_OPEN_REMOTE_PROCESS: HandleCmdOpenProcess(pClientSocket, packet); break;
            //case CMD_CLOSE_REMOTE_PROCESS: HandleCmdCloseProcess(pClientSocket, packet); break; // TODO
            case CMD_READ_MEMORY_CHUCK: HandleCmdReadMemory(pClientSocket, packet); break;
            case CMD_WRITE_MEMORY_CHUCK: HandleCmdWriteMemory(pClientSocket, packet); break; // TODO
            case CMD_STATUS: HandleCmdStatus(pClientSocket); break;
            case CMD_DISCONNECT: HandleCmdDisconnect(pClientSocket); bDisconnecting = true; break;
        }
    }

    LEAVE:
    HandleDisconnect(pClientSocket);
    delete pClientSocket;
};