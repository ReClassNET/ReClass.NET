#pragma once

#include "PackDefs.h"

typedef int32_t HANDLE_API;

#define CMD_OK 1
#define CMD_HI 2
#define CMD_DISCONNECT 3
#define CMD_WELLCOME 4
#define CMD_CREATE_REMOTE_TOOL_HELP 5
#define CMD_REMOVE_REMOTE_PROCS_TOOL_HELP 6
#define CMD_REMOVE_REMOTE_MODS_TOOL_HELP 7
#define CMD_STATUS 8
#define CMD_PROCESS_FIRST 9
#define CMD_PROCESS_NEXT 10
#define CMD_OPEN_REMOTE_PROCESS 11
#define CMD_READ_MEMORY 12

#define API_PORT 3443
#define MAX_PACKET_SIZE 4096
#define MAX_PROC_NAME 160

enum HandleValue {
	INVALID = -1,
	NONE
};

enum ToolType {
	PROCS,
	MODULES
};

PACK(struct CreateRemoteToolHelpIn {
	uint16_t mToolType;
	uint32_t mProcessId;
});

PACK(struct CreateRemoteToolHelpOut {
	HANDLE_API mHandleTool;
});

PACK(struct RemoveRemoteProcsToolHelpIn {
	HANDLE_API mHandleSnap;
});

PACK(struct RemoveRemoteModsToolHelpIn {
	HANDLE_API mHandleSnap;
});

PACK(struct ProcessInfo{
    int32_t mProcessId;
    char mProcessName[MAX_PROC_NAME]{};
});

PACK(struct ProcessFirstIn {
	HANDLE_API mHandleSnap;
});

PACK(struct ProcessFirstOut {
	ProcessInfo mProcessInfo;
	bool mRemaining;
});

PACK(struct ProcessNextIn {
	HANDLE_API mHandleSnap;
});

PACK(struct ProcessNextOut {
	ProcessInfo mProcessInfo;
	bool mRemaining;
});

PACK(struct OpenRemoteProcessIn {
	uint32_t mProcessId;
	uint32_t mFlagsAccess; // TODO, ignored for now
});

PACK(struct ReadMemoryIn {
	HANDLE_API mhProc;
	uint64_t mAddr;
	uint64_t mSize;
});

PACK(struct ReadMemoryOut {
	int64_t mBytesReaded;
	uint8_t mBuff[1];
});

PACK(struct OpenRemoteProcessOut {
	HANDLE_API hProc;
});


enum class ClientStatus {
	CONN_OK,
	CONN_ALREDY,
	CONN_FAILED
};