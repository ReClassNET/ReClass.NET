#include "Remote.h"
#include "LinuxSocketWrapper.h"
#include "API.h"
#include <unordered_map>
#include <Packet.h>
#include "Utils.h"
#include "dirent.h"

enum {
    SNAPSHOT_PROCESSES,
    SNAPSHOT_MODULES
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

    bool Pop(ProcessInfo& outProc)
    {
        outProc = mArrayProcs[mArrayPos];

        return ((mArrayProcs.size() - 1) - mArrayPos++) > 0;
    }
};

struct SnapshotModules : SnapshotBase{
    SnapshotModules()
    {
        type = SNAPSHOT_MODULES;
    }
};

std::unordered_map<ClientSocketLinux*, std::vector<SnapshotProcesses*>> gSnapProcs;
std::unordered_map<ClientSocketLinux*, std::vector<SnapshotModules*>> gSnapMods;

bool CreateToolMods(SnapshotModules** outSnap)
{
    *outSnap = new SnapshotModules();

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

void HandleCreateToolMods(ClientSocketLinux* pClientUnique, HANDLE_API& outHandle)
{

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
        case ToolType::MODULES: HandleCreateToolMods(pClientUnique, rHandleResult); break;
    }

    if(rHandleResult != HandleValue::INVALID)
        printf("Handle Tool Created %d\n", rHandleResult);

    pClientUnique->Send(&resultPacket, sizeof(resultPacket));
}

void HandleCmdHi(ClientSocketLinux* pClientSocket)
{
    pClientSocket->SendMsg(CMD_WELLCOME);
}

void HandleDisconnect(ClientSocketLinux* pClientSocket)
{
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
            case CMD_REMOVE_REMOTE_PROCS_TOOL_HELP: HandleCmdRemoveProcsToolHelp(pClientSocket, packet); break;
            case CMD_STATUS: HandleCmdStatus(pClientSocket); break;
            case CMD_DISCONNECT: HandleCmdDisconnect(pClientSocket); bDisconnecting = true; break;
        }
    }

    LEAVE:
    HandleDisconnect(pClientSocket);
    delete pClientSocket;
};