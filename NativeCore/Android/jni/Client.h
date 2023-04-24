#pragma once

#include <SocketWrapper.h>
#include <memory>
#include <thread>
#include <API.h>
#include <Packet.h>
#include <unordered_map>
#include <SnapshotBase.h>
#include <atomic>
#include <LinuxProcess.h>

enum {
    SNAPSHOT_PROCESSES,
    SNAPSHOT_MODULES,
    SNAPSHOT_SECTIONS
};

bool getMapsSegments(uint32_t pid, std::vector<SectionInfo>& outSeg);

struct SnapshotProcesses : SnapshotBase<ProcessInfo> {
    SnapshotProcesses();
};

struct SnapshotSections : SnapshotBase<SectionInfo> {
    SnapshotSections(uint32_t pid);
};

struct SnapshotModules : SnapshotBase<ModuleInfo> {
    SnapshotModules(uint32_t pid);
};

class ReclassClient {
private:

	std::atomic<size_t>  mHandleSeed;
	
	std::unique_ptr<SocketWrapper> mClientSocket;
	bool mIsDisconnecting = false;
	
	unsigned char mPacket[MAX_PACKET_SIZE + 1]{0};

    uintptr_t mReceivedPacketLen = 0;
	std::unique_ptr<std::thread> mHandlerThread;

	std::unordered_map<SnapshotProcesses*, std::unique_ptr<SnapshotProcesses>> mToolHelpProcs;
	std::unordered_map<HANDLE_API, SnapshotProcesses*> mHandlesToolHelpProcs;

	std::unordered_map<SnapshotSections*, std::unique_ptr<SnapshotSections>> mToolHelpSegments;
	std::unordered_map<HANDLE_API, SnapshotSections*> mHandlesToolHelpSegments;

	std::unordered_map<SnapshotModules*, std::unique_ptr<SnapshotModules>> mToolHelpModules;
	std::unordered_map<HANDLE_API, SnapshotModules*> mHandlesToolHelpModules;

	std::unique_ptr<LinuxProcess> mAttachedProcess;

	public:

	~ReclassClient();

	short getPacketCMDId();
	size_t getPacketSize();

	HANDLE_API AttachProcess(uintptr_t procId);

	void setClientSocket(std::unique_ptr<SocketWrapper>& socket);

	bool WaitMessage();

	void onReceivedMessage();

	bool ReceiveAndValidateFirstCMD();

	void RunThread();
	void Run();

	void DettachProcess();

	void HandleCreateToolHelpCMD();

	void HandleCreateToolHelpProcs();
	void HandleProcessFirstCMD();
	void HandleProcessNextCMD();
	void OnConsumedProcessList();
	
	void HandleCreateToolHelpSegments();
	void HandleSectionFirstCMD();
	void HandleSectionNextCMD();
	void OnConsumedSectionList();

	void HandleCreateToolHelpModules();
	void HandleModuleFirstCMD();
	void HandleModuleNextCMD();
	void OnConsumedModuleList();

	void HandleStatusCmd();
	void HandleOpenProcessCmd();
	void HandleCloseProcessCMD();
	

	void HandleReadMemoryCMD();

	template<typename T>
	T* CastPacket();

	template<typename T>
	T* CastGetPacketInfo();
};

template<typename T>
T* ReclassClient::CastPacket()
{
	return (T*)mPacket;
}

template<typename T>
T* ReclassClient::CastGetPacketInfo()
{
	Packet<T>* pckt = CastPacket<Packet<T>>();

	return pckt->getPayload();
}
