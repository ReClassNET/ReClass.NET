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

template<typename ToolType, typename PacketIn, typename PacketOut, short packetId>
bool ToolOp(HANDLE_API hSnap, ToolType* pPi)
{
	std::lock_guard lck(gMtxReq);
	auto* pSocket = ServerManager::getInstance()->getServerSocket();
	Packet<PacketIn, packetId> pckt{};

	pckt.getPayload()->mHandleSnap = hSnap;

	if (pSocket->Send(pckt.getEntry(), pckt.getSize()))
	{
		if (pSocket->Recive(gPacketHolder, MAX_PACKET_SIZE))
		{
			PacketOut* pOut = ((PacketOut*)gPacketHolder);
			*pPi = pOut->mInfo;

			return pOut->mRemaining;
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
		Packet<ReadMemoryIn, CMD_READ_MEMORY_CHUCK> pckt;

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

int WriteMemoryChuckServer(HANDLE_API handle, uint64_t address, RC_Pointer buffer, int size)
{
	std::lock_guard lck(gMtxReq);
	int bytesWrited = -1;
	constexpr size_t pcktHeaderSize = sizeof(uint64_t) * 2 + sizeof(uint32_t);

	if (size <= (MAX_PACKET_SIZE - pcktHeaderSize))
	{
		auto* pSocket = ServerManager::getInstance()->getServerSocket();
		Packet<WriteMemoryIn, 0>* pckt = (Packet<WriteMemoryIn, 0>*)gPacketHolder;

		pckt->SetPacketid(CMD_WRITE_MEMORY_CHUCK);
		pckt->getPayload()->mAddr = address;
		pckt->getPayload()->mSize = size;
		pckt->getPayload()->mhProc = handle;

		memcpy(pckt->getPayload()->mBuff, buffer, size);

		if (pSocket->Send(pckt->getEntry(), pcktHeaderSize + size))
		{
			if (pSocket->Recive(gPacketHolder, MAX_PACKET_SIZE))
			{
				WriteMemoryOut* pWrMemOut = (WriteMemoryOut*)(gPacketHolder);
				bytesWrited = pWrMemOut->mBytesWrited;
			}
		}
	}

	return bytesWrited;
}

int ReadMemoryServer(RC_Pointer handle, RC_Pointer address, RC_Pointer buffer, int size)
{
	uint64_t bytesReaded = -1;
	constexpr size_t rdMemMaxPacketSize = MAX_PACKET_SIZE - sizeof(int64_t);

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

int WriteMemoryServer(RC_Pointer handle, RC_Pointer address, RC_Pointer buffer, int size)
{
	uint64_t bytesWrited = -1;
	constexpr size_t rdMemMaxPacketSize = MAX_PACKET_SIZE - (sizeof(int64_t) * 2 + sizeof(HANDLE_API));

	if (size <= rdMemMaxPacketSize) bytesWrited = WriteMemoryChuckServer((HANDLE_API)handle, (uint64_t)address, buffer, size);

	return bytesWrited;
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
	HANDLE_API hSnap = CreateRemoteTool(ToolType::SECTIONS, PidFromHandle(hProc));

	if (hSnap != HandleValue::INVALID)
	{
		SectionInfo si{};

		if (ToolOp<SectionInfo, SectionFirstIn, SectionFirstOut, CMD_SECTION_FIRST>(hSnap, &si))
		{
			do {
				EnumerateRemoteSectionData ermd{};

				ermd.BaseAddress = (RC_Pointer)si.mStart;
				ermd.Type = si.mType; // TODO
				ermd.Size = si.mEnd - si.mStart;
				ermd.Protection = si.mProt;
				ermd.Category = SectionCategory::Unknown; // TODO

				constexpr int32_t RdProt = (int)SectionProtection::Read;
				constexpr int32_t RdWrProt = (int)SectionProtection::Read | (int)SectionProtection::Write;
				constexpr int32_t ExProt = (int)SectionProtection::Execute;

				if (((int32_t)ermd.Protection & ExProt) == ExProt)
					ermd.Category = SectionCategory::CODE;
				else if(((int32_t)ermd.Protection & RdProt) == RdProt ||
					((int32_t)ermd.Protection & RdWrProt) == RdWrProt)
					ermd.Category = SectionCategory::DATA;

				lstrcpyW((wchar_t*)ermd.Name, L""); // TODO
				lstrcpyW((wchar_t*)ermd.ModulePath, StrtoWStr(std::string(si.mPath)).c_str());

				callbackSection(&ermd);
			} while (ToolOp<SectionInfo, SectionNextIn, SectionNextOut, CMD_SECTION_NEXT>(hSnap, &si));
		}

		//RemoveRemoteSectionsTool(hSnap);
	}
}

void EnumerateRemoteModulesServer(HANDLE_API hProc, EnumerateRemoteModulesCallback callbackModule)
{
	HANDLE_API hSnap = CreateRemoteTool(ToolType::MODULES, PidFromHandle(hProc));

	if (hSnap != HandleValue::INVALID)
	{
		ModuleInfo mi{};

		if (ToolOp<ModuleInfo, ModuleFirstIn, ModuleFirstOut, CMD_MODULE_FIRST>(hSnap, &mi))
		{
			do {
				EnumerateRemoteModuleData ermd{};

				ermd.BaseAddress = (RC_Pointer)mi.mBase;
				ermd.Size = (RC_Size)mi.mSize;
				lstrcpyW((wchar_t*)ermd.Path, StrtoWStr(std::string(mi.mPath)).c_str());

				callbackModule(&ermd);
			} while (ToolOp<ModuleInfo, ModuleNextIn, ModuleNextOut, CMD_MODULE_NEXT>(hSnap, &mi));
		}

		//RemoveRemoteModulesTool(hSnap);
	}
}

void EnumerateProcessesServer(EnumerateProcessCallback callbackProcess)
{
	HANDLE_API hSnap = CreateRemoteTool(ToolType::PROCS, 0);

	if (hSnap != HandleValue::INVALID)
	{
		ProcessInfo pi{};
		if (ToolOp<ProcessInfo, ProcessFirstIn, ProcessFirstOut, CMD_PROCESS_FIRST>(hSnap, &pi))
		{
			do {
				if (strlen(pi.mProcessName) > 0)
				{
					EnumerateProcessData enumProcData{};

					enumProcData.Id = pi.mProcessId;
					lstrcpyW((wchar_t*)enumProcData.Name, StrtoWStr(pi.mProcessName).c_str());

					callbackProcess(&enumProcData);
				}
			} while (ToolOp<ProcessInfo, ProcessNextIn, ProcessNextOut, CMD_PROCESS_NEXT>(hSnap, &pi));
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



