#include "Remote.h"
#include "LinuxSocketWrapper.h"
#include "API.h"
#include <unordered_map>
#include <Packet.h>
#include "Utils.h"

enum {
    SNAPSHOT_PROCESSES,
    SNAPSHOT_MODULES
};

struct SnapshotBase {
    protected:
    short type;

    public:
    short getSnapshotType()
    {
        return type;
    }
};

struct SnapshotProcesses : SnapshotBase{
    SnapshotProcesses()
    {
        type = SNAPSHOT_PROCESSES;
    }
};

struct SnapshotModules : SnapshotBase{
    SnapshotModules()
    {
        type = SNAPSHOT_MODULES;
    }
};

std::unordered_map<ClientSocketLinux*, SnapshotProcesses*> gMapSnapshotsProcesses;
std::unordered_map<ClientSocketLinux*, SnapshotModules*> gMapSnapshotsModules;

bool CreateToolMods(SnapshotModules** outSnap)
{
    *outSnap = new SnapshotModules();

    return true;
}

bool CreateToolProcs(SnapshotProcesses** outSnap)
{
    *outSnap = new SnapshotProcesses();

    return true;
}

template<typename TypeTool, typename K>
void HandleCreateTool(std::unordered_map<K*, TypeTool*>& gMapTool, ClientSocketLinux* pClientUnique, HANDLE_API& rOutHandle, bool(*pfnCreateTool)(TypeTool** outTool))
{
    rOutHandle = HandleValue::INVALID;

    if(gMapTool.find(pClientUnique) == gMapTool.end())
    {
        TypeTool* pSnap = nullptr;

        printf("Creating Tool Snapshot\n");

        if(pfnCreateTool(&pSnap))
        {
            gMapTool[pClientUnique] = pSnap;
            rOutHandle = (HANDLE_API)pSnap;
        }
    } else {
        rOutHandle = (HANDLE_API)gMapTool[pClientUnique];
    }
}

void HandleCmdCreateToolHelp(ClientSocketLinux* pClientUnique, void* pInPacket)
{
    Packet<CreateRemoteToolHelpIn, 0>* packet = (Packet<CreateRemoteToolHelpIn, 0>*)pInPacket;
    CreateRemoteToolHelpOut resultPacket;
    HANDLE_API& rHandleResult = resultPacket.mHandleSnap;

    rHandleResult = HandleValue::INVALID;

    switch(packet->getPayload()->mToolType)
    {
        case ToolType::PROCS:   HandleCreateTool<SnapshotProcesses>(gMapSnapshotsProcesses, pClientUnique, rHandleResult, CreateToolProcs); break;
        case ToolType::MODULES: HandleCreateTool<SnapshotModules>(gMapSnapshotsModules, pClientUnique, rHandleResult, CreateToolMods); break;
    }

    pClientUnique->Send(&resultPacket, sizeof(resultPacket));
}

void HandleCmdHi(ClientSocketLinux* pClientSocket)
{
    pClientSocket->SendMsg(CMD_WELLCOME);
}

template<typename Type, typename K>
void ClientSafeRemoveTool(std::unordered_map<K*, Type*>& gMapSnap, ClientSocketLinux* pClientUnique, bool bCheckExist)
{
    if(bCheckExist && (gMapSnap.find(pClientUnique) == gMapSnap.end()))
        return;

    delete (Type*)gMapSnap[pClientUnique];
    gMapSnap.erase(pClientUnique);
}

template<typename Type, typename K>
bool UMapSafeRemoveHandle(std::unordered_map<K*, Type*>& gMapSnap, ClientSocketLinux* pClientUnique, HANDLE_API handle)
{
    if((gMapSnap.find(pClientUnique) != gMapSnap.end()) &&
    ((HANDLE_API)gMapSnap[pClientUnique]) == handle)
    {
        ClientSafeRemoveTool(gMapSnap, pClientUnique, false);

        return true;
    }

    return false;
}

void HandleDisconnect(ClientSocketLinux* pClientSocket)
{
    ClientSafeRemoveTool(gMapSnapshotsProcesses, pClientSocket, true);
    ClientSafeRemoveTool(gMapSnapshotsModules, pClientSocket, true);

    printf("Client Disconnected\n");
}

void HandleCmdRemoveToolHelp(ClientSocketLinux* pClientUnique, void* pInPacket)
{
    Packet<RemoveRemoteToolHelpIn, 0>* pckt = ((Packet<RemoveRemoteToolHelpIn, 0>*)pInPacket); 
    HANDLE_API hTool = pckt->getPayload()->mHandleSnap;

    //printf("Packet Remove Tool Help %llx\n", hTool);
    //pckt->Print();

    if(UMapSafeRemoveHandle(gMapSnapshotsProcesses, pClientUnique, hTool)) printf("Removed Process Snapshot %llx\n", hTool);
    else if(UMapSafeRemoveHandle(gMapSnapshotsModules, pClientUnique, hTool)) printf("Removed Modules Snapshot %llx\n", hTool);

    pClientUnique->SendMsg(CMD_OK);
}

void HandleCmdStatus(ClientSocketLinux* pClientUnique)
{
    pClientUnique->SendMsg(CMD_OK);
}

void HandleCmdDisconnect(ClientSocketLinux* pClientUnique)
{
    pClientUnique->SendMsg(CMD_OK);
}

std::function<void(ClientSocketLinux*)> gfnHandleConn = [](ClientSocketLinux* pClientSocket)
{
    unsigned char packet[MAX_PACKET_SIZE]{0};
    short* pCmdId = (short*)packet;
    bool bDisconnecting = false;
    uintptr_t outRecvSize = 0;
    bool bFirstTime = true;

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
            case CMD_REMOVE_REMOTE_TOOL_HELP: HandleCmdRemoveToolHelp(pClientSocket, packet); break;
            case CMD_STATUS: HandleCmdStatus(pClientSocket); break;
            case CMD_DISCONNECT: HandleCmdDisconnect(pClientSocket); bDisconnecting = true; break;
        }
    }

    LEAVE:
    HandleDisconnect(pClientSocket);
    delete pClientSocket;
};