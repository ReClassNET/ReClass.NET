#include "Client.h"
#include "SnapshotBase.h"
#include <dirent.h>
#include <thread>
#include <ReClassNET_Plugin.hpp>

#define PHYSICAL_ADDRESS 0xFEFEFE

bool getMapsSegments(uint32_t pid, std::deque<SectionInfo>& outSeg)
{
    char mapsPath[260] {};

    // if(pid == PHYSICAL_ADDRESS)
    // {
    //     SectionInfo seg{};

    //     seg.mType = SectionType::Private;
    //     seg.mProt = SectionProtection::Read | SectionProtection::Write | SectionProtection::Execute;
    //     strcpy(seg.mPath, "/dev/port");
    //     seg.mStart = 0x0;
    //     seg.mEnd = 0x7FFFFFFFFFFF;

    //     outSeg.push_front(seg);

    //     return true;
    // }

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

                sscanf(line,"%llx-%llx %c%c%c%c %llx %llx:%llx %llx %260[^\n\t]", 
				(void*)&seg.mStart, 
				(void*)&seg.mEnd, 
				&r, &w, &x, &m, 
				(void*)&offset, 
				(void*)&unk, 
				(void*)&unk, 
				(void*)&unk, 
				seg.mPath);

                /*printf("%llx-%llx %c%c%c%c %llx %llx:%llx %llx %s\n", 
				seg.mStart, 
				seg.mEnd, 
				r, w, x, m, 
				offset, 
				unk, 
				unk, 
				unk, 
				seg.mPath);*/

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
            printf("%lu Segments Parsed\n", outSeg.size());
            return true;
        }
    }

    return false;
}

SnapshotProcesses::SnapshotProcesses()
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

					mArray.push_front(pi);

					fclose(fCmdLine);
				}
			}
		}

		closedir(pProcDir);
	}

	ProcessInfo pi;

	pi.mProcessId = PHYSICAL_ADDRESS;
	strcpy(pi.mProcessName, "Physical Address");

	mArray.push_front(pi);
}

SnapshotSections::SnapshotSections(uint32_t pid)
{
	type = SNAPSHOT_SECTIONS;

	printf("Creating SnapshotSections PID %d\n", pid);

	getMapsSegments(pid, mArray);

	printf("SnapshotSections Created PID %d, %d Sections\n", pid, mArray.size());

}

SnapshotModules::SnapshotModules(uint32_t pid)
{
	type = SNAPSHOT_MODULES;

	printf("Creating Snap Modules -> PID %d\n", pid);

	SnapshotSections snapshotSect = SnapshotSections(pid);

	if(snapshotSect.mArray.size() > 0 == false)
		return;

	std::unordered_map <std::string, std::deque<SectionInfo>> modMatchs;

	SectionInfo si = snapshotSect.mArray.front();

	do {
		if(strstr(si.mPath, ".so"))
		{
			std::string mPathStr = std::string(si.mPath);

			if(modMatchs.find(mPathStr) == modMatchs.end())
				modMatchs[mPathStr] = std::deque<SectionInfo>();

			modMatchs[mPathStr].push_back(si);
		}
		si = snapshotSect.mArray.front(); snapshotSect.mArray.pop_front();
	} while(snapshotSect.mArray.size() > 0);

	for(const auto& kv : modMatchs)
	{
		if(kv.second.size() > 1)
		{
			ModuleInfo currMi;

			const auto& pFirst = kv.second[0];
			const auto& pLast = kv.second[kv.second.size() - 1];

			currMi.mBase = pFirst.mStart;
			currMi.mSize = pLast.mEnd - currMi.mBase;
			strcpy(currMi.mPath, pFirst.mPath);

			//printf("Module %s\n", pFirst.mPath);

			mArray.push_back(currMi);
		}
	}

	printf("%d Modules Found\n", mArray.size());
}

void ReclassClient::setClientSocket(std::unique_ptr<SocketWrapper>& socket)
{
	mClientSocket = std::move(socket);
}

void ReclassClient::onReceivedMessage()
{
	//printf("Message Received, ID: %d\n", getPacketCMDId());

	switch(getPacketCMDId())
	{
		case CMD_CREATE_REMOTE_TOOL_HELP: HandleCreateToolHelpCMD(); break;

		case CMD_PROCESS_FIRST: HandleProcessFirstCMD(); break;
		case CMD_PROCESS_NEXT: HandleProcessNextCMD(); break;

		case CMD_SECTION_FIRST: HandleSectionFirstCMD(); break;
		case CMD_SECTION_NEXT: HandleSectionNextCMD(); break;

		case CMD_MODULE_FIRST: HandleModuleFirstCMD(); break;
		case CMD_MODULE_NEXT: HandleModuleNextCMD(); break;

		case CMD_STATUS: HandleStatusCmd(); break;
		case CMD_OPEN_REMOTE_PROCESS: HandleOpenProcessCmd(); break;
		case CMD_CLOSE_REMOTE_PROCESS: HandleCloseProcessCMD(); break; // TODO
		case CMD_READ_MEMORY_CHUCK: HandleReadMemoryCMD(); break;
		/*case CMD_REMOVE_REMOTE_PROCS_TOOL_HELP: HandleCmdRemoveProcsToolHelp(pClientSocket, packet); break;
		//case CMD_REMOVE_REMOTE_MODS_TOOL_HELP: HandleCmdRemoveProcsToolHelp(pClientSocket, packet); break; // TODO
		//case CMD_REMOVE_REMOTE_SECS_TOOL_HELP: HandleCmdRemoveProcsToolHelp(pClientSocket, packet); break; // TODO
		case CMD_WRITE_MEMORY_CHUCK: HandleCmdWriteMemory(pClientSocket, packet); break; // TODO
		case CMD_DISCONNECT: HandleCmdDisconnect(pClientSocket); bDisconnecting = true; break;*/
	}
}

bool ReclassClient::WaitMessage()
{
	if(mIsDisconnecting == true)
		return false;

	return mClientSocket->Recive(mPacket, MAX_PACKET_SIZE, &mReceivedPacketLen);
}

bool ReclassClient::ReceiveAndValidateFirstCMD()
{
	WaitMessage(); // Waiting first msg

	if(getPacketCMDId() != CMD_HI)
		return false; // invalid conection, rejecting

	mClientSocket->SendMsg(CMD_WELLCOME); // Responding to that valid conn

	return true; // Valid Connection
}

void ReclassClient::DettachProcess()
{
	if(mAttachedProcess)
		mAttachedProcess.reset();
}

void ReclassClient::RunThread()
{
	if(mHandlerThread)
		return;

	mHandlerThread = std::make_unique<std::thread>([&]{
		Run();
	});
}

void ReclassClient::Run()
{
	if(ReceiveAndValidateFirstCMD() == false)
		return;

	printf("Client Connected!\n");

	while(WaitMessage()) // at this point, we are in a valid connection
		onReceivedMessage();
}

void ReclassClient::HandleCreateToolHelpCMD()
{
	Packet<CreateRemoteToolHelpIn>* toolHelp = CastPacket<Packet<CreateRemoteToolHelpIn>>();

	switch(toolHelp->mPacketContent.mToolType)
	{
		case ToolType::PROCS: HandleCreateToolHelpProcs(); break;
		case ToolType::SECTIONS: HandleCreateToolHelpSegments(); break;
		case ToolType::MODULES: HandleCreateToolHelpModules(); break;
	}
}

void ReclassClient::HandleCreateToolHelpProcs()
{
	CreateRemoteToolHelpOut toolHelpProcs {};

	// Defaulting to invalid
	toolHelpProcs.mHandleTool = HandleValue::INVALID;

	// Create the snapshot
	std::unique_ptr<SnapshotProcesses> snapshot = std::make_unique<SnapshotProcesses>();
	SnapshotProcesses* pSnapshot = snapshot.get();

	// Compute the Handle Value
	toolHelpProcs.mHandleTool = mHandleSeed++;

	// Save the Snapshot respectively
	mToolHelpProcs[pSnapshot] = std::move(snapshot);
	mHandlesToolHelpProcs[toolHelpProcs.mHandleTool] = pSnapshot;

	// Report handle to Client
	mClientSocket->Send(&toolHelpProcs, sizeof(toolHelpProcs));
}

void ReclassClient::HandleProcessFirstCMD()
{
	Packet<ProcessFirstIn>* procFirstin = CastPacket<Packet<ProcessFirstIn>>();
	HANDLE_API hSnap = procFirstin->getPayload()->mHandleSnap;

	// Remaining = False by default

	ProcessFirstOut procFirstOut { 0 };

	if(mHandlesToolHelpProcs.find(hSnap) != mHandlesToolHelpProcs.end())
	{
		SnapshotProcesses* snapshot = mHandlesToolHelpProcs[hSnap];

		procFirstOut.mInfo = snapshot->mArray.front(); snapshot->mArray.pop_front();
		procFirstOut.mRemaining = snapshot->mArray.empty() != true;

		if(procFirstOut.mRemaining == false)
			OnConsumedProcessList();

	}

	mClientSocket->Send(&procFirstOut, sizeof(procFirstOut));
}

void ReclassClient::HandleProcessNextCMD()
{
	Packet<ProcessNextIn>* procNextIn = CastPacket<Packet<ProcessNextIn>>();
	HANDLE_API hSnap = procNextIn->getPayload()->mHandleSnap;

	// Remaining = False by default

	ProcessNextOut procNextOut { 0 };

	if(mHandlesToolHelpProcs.find(hSnap) != mHandlesToolHelpProcs.end())
	{
		SnapshotProcesses* snapshot = mHandlesToolHelpProcs[hSnap];

		procNextOut.mInfo = snapshot->mArray.front(); snapshot->mArray.pop_front();
		procNextOut.mRemaining = snapshot->mArray.empty() != true;

		if(procNextOut.mRemaining == false)
			OnConsumedProcessList();
	}

	mClientSocket->Send(&procNextOut, sizeof(procNextOut));
}

void ReclassClient::OnConsumedProcessList()
{
	mHandlesToolHelpProcs.clear();
	mToolHelpProcs.clear();
}

void ReclassClient::HandleCreateToolHelpSegments()
{
	CreateRemoteToolHelpIn* toolhelpIn = CastGetPacketInfo<CreateRemoteToolHelpIn>();
	CreateRemoteToolHelpOut toolHelp {};

	// Defaulting to invalid
	toolHelp.mHandleTool = HandleValue::INVALID;

	// Create the snapshot
	std::unique_ptr<SnapshotSections> snapshot = std::make_unique<SnapshotSections>(toolhelpIn->mProcessId);
	SnapshotSections* pSnapshot = snapshot.get();

	// Compute the Handle Value
	toolHelp.mHandleTool = mHandleSeed++;

	// Save the Snapshot respectively
	mToolHelpSegments[pSnapshot] = std::move(snapshot);
	mHandlesToolHelpSegments[toolHelp.mHandleTool] = pSnapshot;

	// Report handle to Client
	mClientSocket->Send(&toolHelp, sizeof(toolHelp));
}

void ReclassClient::HandleSectionFirstCMD()
{
	SectionFirstIn* in = CastGetPacketInfo<SectionFirstIn>();
	HANDLE_API hSnap = in->mHandleSnap;

	// Remaining = False by default

	SectionFirstOut out { 0 };

	if(mHandlesToolHelpSegments.find(hSnap) != mHandlesToolHelpSegments.end())
	{
		SnapshotSections* snapshot = mHandlesToolHelpSegments[hSnap];

		out.mInfo = snapshot->mArray.front(); snapshot->mArray.pop_front();
		out.mRemaining = snapshot->mArray.empty() != true;

		if(out.mRemaining == false)
			OnConsumedSectionList();
	}

	mClientSocket->Send(&out, sizeof(out));
}

void ReclassClient::HandleSectionNextCMD()
{
	SectionNextIn* in = CastGetPacketInfo<SectionNextIn>();
	HANDLE_API hSnap = in->mHandleSnap;

	// Remaining = False by default

	SectionNextOut out { 0 };

	if(mHandlesToolHelpSegments.find(hSnap) != mHandlesToolHelpSegments.end())
	{
		SnapshotSections* snapshot = mHandlesToolHelpSegments[hSnap];

		out.mInfo = snapshot->mArray.front(); snapshot->mArray.pop_front();
		out.mRemaining = snapshot->mArray.empty() != true;

		if(out.mRemaining == false)
			OnConsumedSectionList();
	}

	mClientSocket->Send(&out, sizeof(out));
}

void ReclassClient::OnConsumedSectionList()
{
	mHandlesToolHelpSegments.clear();
	mToolHelpSegments.clear();
}

void ReclassClient::HandleCreateToolHelpModules()
{
	CreateRemoteToolHelpIn* toolhelpIn = CastGetPacketInfo<CreateRemoteToolHelpIn>();
	CreateRemoteToolHelpOut toolHelp {};

	// Defaulting to invalid
	toolHelp.mHandleTool = HandleValue::INVALID;

	// Create the snapshot
	std::unique_ptr<SnapshotModules> snapshot = std::make_unique<SnapshotModules>(toolhelpIn->mProcessId);
	SnapshotModules* pSnapshot = snapshot.get();

	// Compute the Handle Value
	toolHelp.mHandleTool = mHandleSeed++;

	// Save the Snapshot respectively
	mToolHelpModules[pSnapshot] = std::move(snapshot);
	mHandlesToolHelpModules[toolHelp.mHandleTool] = pSnapshot;

	// Report handle to Client
	mClientSocket->Send(&toolHelp, sizeof(toolHelp));
}

void ReclassClient::HandleModuleFirstCMD()
{
	ModuleFirstIn* in = CastGetPacketInfo<ModuleFirstIn>();
	HANDLE_API hSnap = in->mHandleSnap;

	// Remaining = False by default

	ModuleNextOut out { 0 };

	if(mHandlesToolHelpModules.find(hSnap) != mHandlesToolHelpModules.end())
	{
		SnapshotModules* snapshot = mHandlesToolHelpModules[hSnap];

		out.mInfo = snapshot->mArray.front(); snapshot->mArray.pop_front();
		out.mRemaining = snapshot->mArray.empty() != true;

		if(out.mRemaining == false)
			OnConsumedModuleList();
	}

	mClientSocket->Send(&out, sizeof(out));
}

void ReclassClient::HandleModuleNextCMD()
{
	ModuleNextIn* in = CastGetPacketInfo<ModuleNextIn>();
	HANDLE_API hSnap = in->mHandleSnap;

	// Remaining = False by default

	ModuleNextOut out { 0 };

	if(mHandlesToolHelpModules.find(hSnap) != mHandlesToolHelpModules.end())
	{
		SnapshotModules* snapshot = mHandlesToolHelpModules[hSnap];

		out.mInfo = snapshot->mArray.front(); snapshot->mArray.pop_front();
		out.mRemaining = snapshot->mArray.empty() != true;

		if(out.mRemaining == false)
			OnConsumedModuleList();
	}

	mClientSocket->Send(&out, sizeof(out));
}

void ReclassClient::OnConsumedModuleList()
{
		mHandlesToolHelpModules.clear();
		mToolHelpModules.clear();
}

void ReclassClient::HandleStatusCmd()
{
	mClientSocket->SendMsg(CMD_OK);
}

void ReclassClient::HandleOpenProcessCmd()
{
	Packet<OpenRemoteProcessIn>* pOpenProcInf = CastPacket<Packet<OpenRemoteProcessIn>>();

	OpenRemoteProcessOut openProcOutInf { 0 };

	// Attaching to the process
	openProcOutInf.hProc = AttachProcess(pOpenProcInf->mPacketContent.mProcessId);

	mClientSocket->Send(&openProcOutInf, sizeof(openProcOutInf));
}

void ReclassClient::HandleCloseProcessCMD()
{
	// TODO, not used in the client
}

void ReclassClient::HandleReadMemoryCMD()
{
	ReadMemoryIn* in = CastGetPacketInfo<ReadMemoryIn>();
	unsigned char mHolderPacket[MAX_PACKET_SIZE + 1]{0};
	ReadMemoryOut* out = (ReadMemoryOut*)(mHolderPacket);

	out->mBytesReaded = -1;

    if(in->mhProc && mAttachedProcess && in->mhProc == mAttachedProcess->getMemFd())
    {
       // printf("Reading %d bytes\n", int(in->mSize));
        out->mBytesReaded = mAttachedProcess->ReadMemory(in->mAddr, out->mBuff, in->mSize);
    }

    //printf("%d Bytes Readed\n", int(out.mBytesReaded));

    mClientSocket->Send(out, out->mBytesReaded != -1 ? out->mBytesReaded + sizeof(int64_t) : sizeof(int64_t));
}

short ReclassClient::getPacketCMDId()
{
	return *((short*)mPacket);
}

size_t ReclassClient::getPacketSize()
{
	return mReceivedPacketLen;
}

HANDLE_API ReclassClient::AttachProcess(uintptr_t procId)
{
	// Detaching from alredy attached process (if attached)
	DettachProcess();

	// Attaching to a new process
	mAttachedProcess = std::make_unique<LinuxProcess>(procId);
	
	// Computing Handle
	int openFdLinux = mAttachedProcess->getMemFd();
	HANDLE_API hProc = openFdLinux < 0 ? HandleValue::INVALID : openFdLinux;
	
	return hProc;
}

ReclassClient::~ReclassClient()
{
	if(mHandlerThread)
	{
		if( mHandlerThread->joinable())
			mHandlerThread->join();
	}
}