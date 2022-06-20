#include "ServerRemoteTool.h"
#include "NativeCore.hpp"

#include "WindowsSocketWrapper.h"
#include "../Shared/API.h"
#include "../Shared/Packet.h"
#include <unordered_map>
#include "Utils.h"

unsigned char gPacketHolder[MAX_PACKET_SIZE + 1]{ 0 };
std::mutex gMtxReq;


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

HANDLE_API CreateRemoteTool(uint32_t type, uint32_t pid)
{
	std::lock_guard lck(gMtxReq);
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
	std::lock_guard lck(gMtxReq);
	auto* pSocket = ServerManager::getInstance()->getServerSocket();
	Packet<RemoveRemoteProcsToolHelpIn, CMD_REMOVE_REMOTE_PROCS_TOOL_HELP> pckt;

	pckt.getPayload()->mHandleSnap = hTool;

	pSocket->Send(pckt.getEntry(), pckt.getSize()); 
}

bool ProcessFirst(HANDLE_API hSnap, ProcessInfo* pPi)
{
	std::lock_guard lck(gMtxReq);
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
	std::lock_guard lck(gMtxReq);
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

bool ModuleFirst(HANDLE_API hSnap, ModuleInfo* pMi)
{
	std::lock_guard lck(gMtxReq);
	auto* pSocket = ServerManager::getInstance()->getServerSocket();
	Packet<ModuleFirstIn, CMD_MODULE_FIRST> pckt{};

	pckt.getPayload()->mHandleSnap = hSnap;

	if (pSocket->Send(pckt.getEntry(), pckt.getSize()))
	{
		if (pSocket->Recive(gPacketHolder, MAX_PACKET_SIZE))
		{
			ModuleFirstOut* pFirstOut = ((ModuleFirstOut*)gPacketHolder);
			*pMi = pFirstOut->mModuleInfo;

			return pFirstOut->mRemaining;
		}
	}

	return false;
}

bool ModuleNext(HANDLE_API hSnap, ModuleInfo* pMi)
{
	std::lock_guard lck(gMtxReq);
	auto* pSocket = ServerManager::getInstance()->getServerSocket();
	Packet<ModuleNextIn, CMD_MODULE_NEXT> pckt{};

	pckt.getPayload()->mHandleSnap = hSnap;

	if (pSocket->Send(pckt.getEntry(), pckt.getSize()))
	{
		if (pSocket->Recive(gPacketHolder, MAX_PACKET_SIZE))
		{
			ModuleNextOut* pNextOut = ((ModuleNextOut*)gPacketHolder);
			*pMi = pNextOut->mModuleInfo;

			return pNextOut->mRemaining;
		}
	}

	return false;
}

void CloseServerProcess(HANDLE_API hProc)
{
}

std::unordered_map<HANDLE_API, uint32_t> gPids;

uint32_t PidFromHandle(HANDLE_API hProc)
{
	if (gPids.find(hProc) != gPids.end())
		return gPids[hProc];

	return 0;
}

RC_Pointer OpenRemoteProcessServer(RC_Pointer id)
{
	std::lock_guard lck(gMtxReq);
	auto* pSocket = ServerManager::getInstance()->getServerSocket();
	Packet<OpenRemoteProcessIn, CMD_OPEN_REMOTE_PROCESS> pckt;

	pckt.getPayload()->mProcessId = (uint32_t)id;
	pckt.getPayload()->mFlagsAccess = 0; // TODO, ignored for now

	if (pSocket->Send(pckt.getEntry(), pckt.getSize()))
	{
		if (pSocket->Recive(gPacketHolder, MAX_PACKET_SIZE))
		{
			HANDLE_API hProc = ((OpenRemoteProcessOut*)gPacketHolder)->hProc;

			if (hProc != HandleValue::INVALID)
				gPids[hProc] = (uint32_t)id;

			return (RC_Pointer)hProc;
		}

	}

	return (RC_Pointer)HandleValue::INVALID;
}

int ReadMemoryChuckServer(HANDLE_API handle, uint64_t address, RC_Pointer buffer, int size)
{
	std::lock_guard lck(gMtxReq);
	int bytesReaded = -1;

	if (size <= (MAX_PACKET_SIZE - sizeof(int64_t)))
	{
		auto* pSocket = ServerManager::getInstance()->getServerSocket();
		Packet<ReadMemoryIn, CMD_READ_MEMORY> pckt;

		pckt.getPayload()->mAddr = address;
		pckt.getPayload()->mSize = size;
		pckt.getPayload()->mhProc = handle;

		if (pSocket->Send(pckt.getEntry(), pckt.getSize()))
		{
			if (pSocket->Recive(gPacketHolder, MAX_PACKET_SIZE))
			{
				ReadMemoryOut* pRdMemOut = (ReadMemoryOut*)(gPacketHolder);

				if((bytesReaded = pRdMemOut->mBytesReaded) != -1)
					memcpy(buffer, pRdMemOut->mBuff, pRdMemOut->mBytesReaded);
			}
		}
	}

	return bytesReaded;
}

int ReadMemoryServer(RC_Pointer handle, RC_Pointer address, RC_Pointer buffer, int size)
{
	uint64_t bytesReaded = -1;
	constexpr size_t rdMemMaxPacketSize = MAX_PACKET_SIZE - sizeof(int64_t) - 4;

	if (size > rdMemMaxPacketSize) // need split?
	{
		bytesReaded = 0;
		int nSends = (int)(size / rdMemMaxPacketSize);
		int64_t currBytesReaded = 0;

		uint64_t offset = 0;

		for (int i = 0; i < nSends; i++, offset += rdMemMaxPacketSize)
		{
			currBytesReaded = 0;

			if ((currBytesReaded = ReadMemoryChuckServer((HANDLE_API)handle, ((uintptr_t)address) + offset, (RC_Pointer)((uintptr_t)buffer + offset), rdMemMaxPacketSize)) == -1)
				return bytesReaded;

			bytesReaded += currBytesReaded;
		}

		// Reading Bytes Remainings
		currBytesReaded = ReadMemoryChuckServer((HANDLE_API)handle, (uintptr_t)address + bytesReaded, (RC_Pointer)((uintptr_t)buffer + bytesReaded), size % rdMemMaxPacketSize);

		bytesReaded = currBytesReaded != -1 ? bytesReaded + currBytesReaded : bytesReaded;
	}
	else
		bytesReaded = ReadMemoryChuckServer((HANDLE_API)handle, (uintptr_t)address, buffer, size);

	return bytesReaded;
}

void RemoveRemoteModulesTool(HANDLE_API hSnap)
{
	std::lock_guard lck(gMtxReq);
	auto* pSocket = ServerManager::getInstance()->getServerSocket();
	Packet<RemoveRemoteModsToolHelpIn, CMD_REMOVE_REMOTE_MODS_TOOL_HELP> pckt;

	pckt.getPayload()->mHandleSnap = hSnap;

	pSocket->Send(pckt.getEntry(), pckt.getSize());
}

void RemoveRemoteSectionsTool(HANDLE_API hSnap)
{
	std::lock_guard lck(gMtxReq);
	auto* pSocket = ServerManager::getInstance()->getServerSocket();
	Packet<RemoveRemoteSecsToolHelpIn, CMD_REMOVE_REMOTE_SECS_TOOL_HELP> pckt;

	pckt.getPayload()->mHandleSnap = hSnap;

	pSocket->Send(pckt.getEntry(), pckt.getSize());
}



void EnumerateRemoteSectionsServer(HANDLE_API hProc, EnumerateRemoteSectionsCallback callbackSection)
{
	/*HANDLE_API hSnap = CreateRemoteTool(ToolType::SECTIONS, PidFromHandle(hProc));

	if (hSnap != HandleValue::INVALID)
	{
		ModuleInfo mi{};

		if (ModuleFirst(hSnap, &mi))
		{
			do {
				EnumerateRemoteModuleData ermd{};

				ermd.BaseAddress = (RC_Pointer)mi.mBase;
				ermd.Size = (RC_Size)mi.mSize;
				lstrcpyW((wchar_t*)ermd.Path, StrtoWStr(std::string(mi.mPath)).c_str());

				callbackModule(&ermd);
			} while (ModuleNext(hSnap, &mi));
		}

		RemoveRemoteSectionsTool(hSnap);
	}*/
}

void EnumerateRemoteModulesServer(HANDLE_API hProc, EnumerateRemoteModulesCallback callbackModule)
{
	HANDLE_API hSnap = CreateRemoteTool(ToolType::MODULES, PidFromHandle(hProc));

	if (hSnap != HandleValue::INVALID)
	{
		ModuleInfo mi{};

		if (ModuleFirst(hSnap, &mi))
		{
			do {
				EnumerateRemoteModuleData ermd{};

				ermd.BaseAddress = (RC_Pointer)mi.mBase;
				ermd.Size = (RC_Size)mi.mSize;
				lstrcpyW((wchar_t*)ermd.Path, StrtoWStr(std::string(mi.mPath)).c_str());

				callbackModule(&ermd);
			} while (ModuleNext(hSnap, &mi));
		}

		RemoveRemoteModulesTool(hSnap);
	}
}

void EnumerateProcessesServer(EnumerateProcessCallback callbackProcess)
{
	HANDLE_API hSnap = CreateRemoteTool(ToolType::PROCS, 0);

	if (hSnap != HandleValue::INVALID)
	{
		ProcessInfo pi{};
		if (ProcessFirst(hSnap, &pi))
		{
			do {
				if (strlen(pi.mProcessName) > 0)
				{
					EnumerateProcessData enumProcData{};

					enumProcData.Id = pi.mProcessId;
					lstrcpyW((wchar_t*)enumProcData.Name, StrtoWStr(pi.mProcessName).c_str());

					callbackProcess(&enumProcData);
				}
			} while (ProcessNext(hSnap, &pi));
		}

		RemoveRemoteProcsTool(hSnap);
	}
}

void EnumerateRemoteSectionsAndModulesServer(RC_Pointer process, EnumerateRemoteSectionsCallback callbackSection, EnumerateRemoteModulesCallback callbackModule)
{
	EnumerateRemoteSectionsServer((HANDLE_API)process, callbackSection);
	EnumerateRemoteModulesServer((HANDLE_API)process, callbackModule);
}

void CloseServerProcess(RC_Pointer hProc)
{

}



