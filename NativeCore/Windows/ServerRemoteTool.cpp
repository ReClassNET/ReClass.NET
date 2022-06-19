#include "NativeCore.hpp"

#include "WindowsSocketWrapper.h"
#include "../Shared/API.h"
#include "ServerRemoteTool.h"
#include "../Shared/Packet.h"

ClientSocketWindows* gServerSocket = nullptr;
unsigned char gPacketHolder[MAX_PACKET_SIZE + 1]{ 0 };

ServerManager* ServerManager::mInstance = nullptr;

int32_t RC_CallConv ConnectServer(const char* pIpStr, short port)
{
	auto* pServerMgr = ServerManager::getInstance();

	if (pServerMgr->IsConnected())
		return 1;

	pServerMgr->CleanUp();

	if (pServerMgr->TryConnect(std::string(pIpStr), port))
		return 0;
	else
		return 2;
}

void RC_CallConv DisconnectServer()
{
	ServerManager::getInstance()->Disconnect();
}

ServerManager::ServerManager() : mSocket(nullptr) {}

ServerManager::~ServerManager() {
	Disconnect();
}

bool ServerManager::Disconnect()
{
	if (mSocket) mSocket->SendMsg(CMD_DISCONNECT);

	CleanUp();

	return true;
}

bool ServerManager::IsConnected()
{
	return PartiallyConnected() && CheckConnection();
}

bool ServerManager::CheckConnection()
{
	mSocket->SendMsg(CMD_STATUS);

	if (mSocket->GetMsg() == CMD_OK)
		return true;

	return false;
}

bool ServerManager::CleanUp()
{
	if (mSocket)
		delete mSocket;

	mSocket = nullptr;

	return true;
}

bool ServerManager::TryConnect(const std::string& ip, short port)
{
	mSocket = new ClientSocketWindows(ip, port);

	if (mSocket)
	{
		if (mSocket->Init())
		{
			if (mSocket->SendMsg(CMD_HI))
			{
				if (mSocket->GetMsg() == CMD_WELLCOME)
					return true;
			}
		}
	}

	CleanUp();
	return false;
}

ClientSocketWindows* ServerManager::getServerSocket() { return mSocket; }

ServerManager* ServerManager::getInstance()
{
	if (!mInstance)
		mInstance = new ServerManager();

	return mInstance;
}

bool ServerManager::PartiallyConnected()
{
	return mSocket != nullptr;
}

extern ServerManager* gServerMgr;

HANDLE_API CreateRemoteTool(uint32_t type, uint32_t pid)
{
	auto* pSocket = ServerManager::getInstance()->getServerSocket();
	Packet<CreateRemoteToolHelpIn, CMD_CREATE_REMOTE_TOOL_HELP> pckt;

	pckt.getPayload()->mProcessId = pid;
	pckt.getPayload()->mToolType = type;

	if (pSocket->Send(pckt.getEntry(), pckt.getSize()))
	{
		if (pSocket->Recive(gPacketHolder, MAX_PACKET_SIZE))
			return ((CreateRemoteToolHelpOut*)gPacketHolder)->mHandleTool;
	}

	return HandleValue::INVALID;
}

void RemoveRemoteProcsTool(HANDLE_API hTool)
{
	auto* pSocket = ServerManager::getInstance()->getServerSocket();
	Packet<RemoveRemoteProcsToolHelpIn, CMD_REMOVE_REMOTE_PROCS_TOOL_HELP> pckt;

	pckt.getPayload()->mHandleSnap = hTool;

	pSocket->Send(pckt.getEntry(), pckt.getSize()); 
}

bool ProcessFirst(HANDLE_API hSnap, ProcessInfo* pPi)
{
	auto* pSocket = ServerManager::getInstance()->getServerSocket();
	Packet<ProcessFirstIn, CMD_PROCESS_FIRST> pckt {};

	pckt.getPayload()->mHandleSnap = hSnap;

	if (pSocket->Send(pckt.getEntry(), pckt.getSize()))
	{
		if (pSocket->Recive(gPacketHolder, MAX_PACKET_SIZE))
		{
			ProcessFirstOut* pFirstOut = ((ProcessFirstOut*)gPacketHolder);
			*pPi = pFirstOut->mProcessInfo;

			return pFirstOut->mRemaining;
		}
	}

	return false;
}

bool ProcessNext(HANDLE_API hSnap, ProcessInfo* pPi)
{
	auto* pSocket = ServerManager::getInstance()->getServerSocket();
	Packet<ProcessNextIn, CMD_PROCESS_NEXT> pckt;

	pckt.getPayload()->mHandleSnap = hSnap;

	if (pSocket->Send(pckt.getEntry(), pckt.getSize()))
	{
		if (pSocket->Recive(gPacketHolder, MAX_PACKET_SIZE))
		{
			ProcessNextOut* pNextOut = ((ProcessNextOut*)gPacketHolder);
			*pPi = pNextOut->mProcessInfo;

			return pNextOut->mRemaining;
		}
	}

	return false;
}



