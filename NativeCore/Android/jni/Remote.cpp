#include "Remote.h"
#include "LinuxSocketWrapper.h"
#include "API.h"
#include <unordered_map>
#include <Packet.h>
#include "Utils.h"
#include "dirent.h"
#include "LinuxProcess.h"
#include "ReClassNET_Plugin.hpp"
#include <fstream>

enum {
    SNAPSHOT_PROCESSES,
    SNAPSHOT_MODULES,
    SNAPSHOT_SECTIONS
};

struct SnapshotBase {
    protected:
    uint32_t mArrayPos = 0;
    short type;
    

    public:
    short getSnapshotType()
    {
        return type;
    }
};

struct SnapshotProcesses : SnapshotBase{
    std::vector<ProcessInfo> mArrayProcs;

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

                        mArrayProcs.push_back(pi);

                        fclose(fCmdLine);
                    }
                }
            }

            closedir(pProcDir);
        }
    }

    bool PopFirst(ProcessInfo& outProc)
    {
        if(mArrayProcs.size() > 0)
        {
            return Pop(outProc);
        }

        return false;
    }

    bool Pop(ProcessInfo& outProc)
    {
        outProc = mArrayProcs[mArrayPos];

        return ((mArrayProcs.size() - 1) - mArrayPos++) > 0;
    }
};

struct SectionInfo {
	uint64_t mStart;
	uint64_t mEnd;
	SectionProtection mProt;
	char mPath[260];
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
        printf("%s Opened\n", mapsPath);

        if(line)
        {
            while (fgets(line,4096, fp)) {
                SectionInfo seg{};

                //printf("%s\n", line);

                uint64_t unk;
                char r;
                char w;
                char x;
                char p;

                sscanf(line,"%lx-%lx %c%c%c%c %lx %lx:%lx %lx %260[^\n\t]", &seg.mStart, &seg.mEnd, &r, &w, &x, &p, &unk, &unk, &unk, &unk, seg.mPath);

                printf("%lx-%lx %s\n", seg.mStart, seg.mEnd, seg.mPath);

                if(r == 'r')
                    seg.mProt |= SectionProtection::Read;

                if(w == 'w')
                    seg.mProt |= SectionProtection::Write;

                if(x == 'x')
                    seg.mProt |= SectionProtection::Execute;

                outSeg.push_back(seg);
            }

        
            free(line);
            printf("%d Segments Parsed\n", outSeg.size());
            return true;
        }

    } 

    return false;
}

struct SnapshotSections : SnapshotBase{
    SnapshotSections(uint32_t pid)
    {
        type = SNAPSHOT_SECTIONS;
        printf("Creating SnapshotSections PID %d\n", pid);
        getMapsSegments(pid, mArraySecs);

        printf("SnapshotSections Created PID %d, %d Sections\n", mArraySecs.size());
        
    }

    std::vector<SectionInfo> mArraySecs;

    bool Pop(SectionInfo& outSec)
    {
        outSec = mArraySecs[mArrayPos];

        return ((mArraySecs.size() - 1) - mArrayPos++) > 0;
    }

    bool Pop(SectionInfo** outSec)
    {
        *outSec = &mArraySecs[mArrayPos];

        return ((mArraySecs.size() - 1) - mArrayPos++) > 0;
    }
};

struct SnapshotModules : SnapshotBase{
    SnapshotModules(uint32_t pid)
    {
        type = SNAPSHOT_MODULES;

        printf("Creating SnapModules PID %d\n", pid);

        SnapshotSections snapshotSect = SnapshotSections(pid);
        std::unordered_map <std::string, std::vector<SectionInfo*>> modMatchs;
        
        SectionInfo* si;

        printf("Parsing SnapSections\n", pid);

        if(snapshotSect.Pop(&si))
        {
            do {
                if(strstr(si->mPath, ".so"))
                {
                    std::string mPathStr = std::string(si->mPath);

                    if(modMatchs.find(mPathStr) == modMatchs.end())
                        modMatchs[mPathStr] = std::vector<SectionInfo*>();

                    modMatchs[mPathStr].push_back(si);
                }
            } while(snapshotSect.Pop(&si));
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

                mArrayMods.push_back(currMi);
            }
        }

        printf("%d Modules Parsed\n", mArrayMods.size());
    }

    std::vector<ModuleInfo> mArrayMods;

    bool PopFirst(ModuleInfo& outSnap)
    {
        if(mArrayMods.size() > 0)
        {
            return Pop(outSnap);
        }

        return false;
    }


    bool Pop(ModuleInfo& outMod)
    {
        outMod = mArrayMods[mArrayPos];

        return mArrayPos++ < (mArrayMods.size() - 1);
    }
};

struct OpenedProcessInfo{
    ClientSocketLinux* pOwner;
    LinuxProcess* mProcess;
};

std::unordered_map<HANDLE_API, OpenedProcessInfo> gProcs;      
std::unordered_map<ClientSocketLinux*, std::vector<SnapshotProcesses*>> gSnapProcs;
std::unordered_map<ClientSocketLinux*, std::vector<SnapshotModules*>> gSnapMods;

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

void HandleCmdCreateToolHelp(ClientSocketLinux* pClientUnique, void* pInPacket)
{
    Packet<CreateRemoteToolHelpIn, 0>* packet = (Packet<CreateRemoteToolHelpIn, 0>*)pInPacket;
    CreateRemoteToolHelpOut resultPacket;
    HANDLE_API& rHandleResult = resultPacket.mHandleTool;

    packet->Print();

    printf("Creating Tool %d pid %d\n", packet->getPayload()->mToolType, packet->getPayload()->mProcessId);

    rHandleResult = HandleValue::INVALID;

    switch(packet->getPayload()->mToolType)
    {
        case ToolType::PROCS:   HandleCreateToolProcs(pClientUnique, rHandleResult); break;
        case ToolType::MODULES: HandleCreateToolMods(pClientUnique, packet->getPayload()->mProcessId, rHandleResult); break;
    }

    if(rHandleResult != HandleValue::INVALID)
        printf("Handle Tool Created %d\n", rHandleResult);

    pClientUnique->Send(&resultPacket, sizeof(resultPacket));
}

void HandleCmdHi(ClientSocketLinux* pClientSocket)
{
    pClientSocket->SendMsg(CMD_WELLCOME);
}


void RemoveClientProcHandles(ClientSocketLinux* pClientUnique)
{
    for(const auto& kv : gProcs)
    {
        if(kv.second.pOwner == pClientUnique)
        {
            delete kv.second.mProcess;
            gProcs.erase(kv.first);
        }
    }
}

void HandleDisconnect(ClientSocketLinux* pClientSocket)
{
    RemoveClientProcHandles(pClientSocket);
    printf("Client Disconnected\n");
}

bool ValidSnapProcsHandle(ClientSocketLinux* pClientUnique, HANDLE_API handle, bool bCheckRange = true)
{
    if(bCheckRange)
    {
        if(!(handle >= 0 && handle < gSnapProcs[pClientUnique].size()))
            return false;
    }

    return gSnapProcs[pClientUnique][handle] != nullptr;
}

void HandleCmdRemoveProcsToolHelp(ClientSocketLinux* pClientUnique, void* pInPacket)
{
    Packet<RemoveRemoteProcsToolHelpIn, 0>* pckt = ((Packet<RemoveRemoteProcsToolHelpIn, 0>*)pInPacket); 
    HANDLE_API hTool = pckt->getPayload()->mHandleSnap;

    printf("Removing Tool Procs %d\n",hTool);

    if(ValidSnapProcsHandle(pClientUnique, hTool)) {
        delete (gSnapProcs[pClientUnique][hTool]);
        gSnapProcs[pClientUnique][hTool] = nullptr;

        printf("Procs Tool Removed %d\n",hTool);
    }
}

void HandleCmdStatus(ClientSocketLinux* pClientUnique)
{
    printf("Process Ping\n");
    pClientUnique->SendMsg(CMD_OK);
}

void HandleCmdDisconnect(ClientSocketLinux* pClientUnique)
{
    HandleDisconnect(pClientUnique);
    pClientUnique->SendMsg(CMD_OK);
}

bool ValidSnapModsHandle(ClientSocketLinux* pClientUnique, HANDLE_API handle, bool bCheckRange = true)
{
    if(bCheckRange)
    {
        if(!(handle >= 0 && handle < gSnapMods[pClientUnique].size()))
            return false;
    }

    return gSnapMods[pClientUnique][handle] != nullptr;
}

bool HandleValid(ClientSocketLinux* pClientUnique, HANDLE_API handle)
{
    return ValidSnapProcsHandle(pClientUnique, handle, true) || ValidSnapModsHandle(pClientUnique, handle, true);
}

void HandleCmdProcessFirst(ClientSocketLinux* pClientUnique, void* pInPacket)
{
    Packet<ProcessFirstIn, 0>* pcktIn = (Packet<ProcessFirstIn, 0>*)pInPacket;
    ProcessFirstOut pFirstOut{};
    HANDLE_API hSnap = pcktIn->getPayload()->mHandleSnap;
    pFirstOut.mRemaining = false;

    if(ValidSnapProcsHandle(pClientUnique, hSnap))
    {
        SnapshotProcesses* pSnap = gSnapProcs[pClientUnique][hSnap];

        pFirstOut.mRemaining = pSnap->Pop(pFirstOut.mProcessInfo);

        printf("Process First: %s %d\n", pFirstOut.mProcessInfo.mProcessName, pFirstOut.mProcessInfo.mProcessId);
    }

    pClientUnique->Send(&pFirstOut, sizeof(pFirstOut));
}

void HandleCmdProcessNext(ClientSocketLinux* pClientUnique, void* pInPacket)
{
    Packet<ProcessNextIn, 0>* pcktIn = (Packet<ProcessNextIn, 0>*)pInPacket;
    ProcessNextOut pNextOut{};
    HANDLE_API hSnap = pcktIn->getPayload()->mHandleSnap;
    pNextOut.mRemaining = false;

    if(ValidSnapProcsHandle(pClientUnique, hSnap))
    {
        SnapshotProcesses* pSnap = gSnapProcs[pClientUnique][hSnap];

        pNextOut.mRemaining = pSnap->Pop(pNextOut.mProcessInfo);

        printf("Process Next: %s %d\n", pNextOut.mProcessInfo.mProcessName, pNextOut.mProcessInfo.mProcessId);
    }

    pClientUnique->Send(&pNextOut, sizeof(pNextOut));
}

void HandleCmdModuleFirst(ClientSocketLinux* pClientUnique, void* pInPacket)
{
    Packet<ModuleFirstIn, 0>* pcktIn = (Packet<ModuleFirstIn, 0>*)pInPacket;
    ModuleFirstOut pFirstOut{};
    HANDLE_API hSnap = pcktIn->getPayload()->mHandleSnap;
    pFirstOut.mRemaining = false;

    printf("Module First\n");

    if(ValidSnapModsHandle(pClientUnique, hSnap))
    {
        SnapshotModules* pSnap = gSnapMods[pClientUnique][hSnap];

        pFirstOut.mRemaining = pSnap->PopFirst(pFirstOut.mModuleInfo);

        printf("Module First: %s %lx\n", pFirstOut.mModuleInfo.mPath, pFirstOut.mModuleInfo.mBase);
    }

    pClientUnique->Send(&pFirstOut, sizeof(pFirstOut));
}

void HandleCmdModuleNext(ClientSocketLinux* pClientUnique, void* pInPacket)
{
    Packet<ModuleNextIn, 0>* pcktIn = (Packet<ModuleNextIn, 0>*)pInPacket;
    ModuleFirstOut pNextOut{};
    HANDLE_API hSnap = pcktIn->getPayload()->mHandleSnap;
    pNextOut.mRemaining = false;

    if(ValidSnapModsHandle(pClientUnique, hSnap))
    {
        SnapshotModules* pSnap = gSnapMods[pClientUnique][hSnap];

        pNextOut.mRemaining = pSnap->Pop(pNextOut.mModuleInfo);

        printf("Module Next: %s %lx\n", pNextOut.mModuleInfo.mPath, pNextOut.mModuleInfo.mBase);
    }

    pClientUnique->Send(&pNextOut, sizeof(pNextOut));
}


void HandleCmdOpenProcess(ClientSocketLinux* pClientUnique, void* pInPacket){
    Packet<OpenRemoteProcessIn, 0>* pcktIn = (Packet<OpenRemoteProcessIn, 0>*)pInPacket;
    OpenRemoteProcessOut pOpenProcOut {};

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
            gProcs[pOpenProcOut.hProc] = openedProcInf;
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
    pRdMemOut->mBytesReaded = -1;

    if(gProcs.find(hProc) != gProcs.end())
    {
        const OpenedProcessInfo& hProcInf = gProcs[hProc];

        printf("Reading %d bytes\n", int(pckt->getPayload()->mSize));

        if(hProcInf.pOwner == pClientUnique)
            pRdMemOut->mBytesReaded = hProcInf.mProcess->ReadMemory(pckt->getPayload()->mAddr, pRdMemOut->mBuff, pckt->getPayload()->mSize);

        printf("%d Bytes Readed\n", int(pRdMemOut->mBytesReaded));
    }

    pClientUnique->Send(pRdMemOut, pRdMemOut->mBytesReaded != -1 ? pRdMemOut->mBytesReaded + sizeof(int64_t) : sizeof(int64_t));
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
            case CMD_PROCESS_FIRST: HandleCmdProcessFirst(pClientSocket, packet); break;
            case CMD_PROCESS_NEXT: HandleCmdProcessNext(pClientSocket, packet); break;
            case CMD_MODULE_FIRST: HandleCmdModuleFirst(pClientSocket, packet); break;
            case CMD_MODULE_NEXT: HandleCmdModuleNext(pClientSocket, packet); break;
            case CMD_REMOVE_REMOTE_PROCS_TOOL_HELP: HandleCmdRemoveProcsToolHelp(pClientSocket, packet); break;
            case CMD_OPEN_REMOTE_PROCESS: HandleCmdOpenProcess(pClientSocket, packet); break;
            case CMD_READ_MEMORY: HandleCmdReadMemory(pClientSocket, packet); break;
            case CMD_STATUS: HandleCmdStatus(pClientSocket); break;
            case CMD_DISCONNECT: HandleCmdDisconnect(pClientSocket); bDisconnecting = true; break;
        }
    }

    LEAVE:
    HandleDisconnect(pClientSocket);
    delete pClientSocket;
};