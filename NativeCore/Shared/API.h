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
#define CMD_REMOVE_REMOTE_SECS_TOOL_HELP 8
#define CMD_STATUS 9
#define CMD_PROCESS_FIRST 10
#define CMD_PROCESS_NEXT 11
#define CMD_MODULE_FIRST 12
#define CMD_MODULE_NEXT 13
#define CMD_SECTION_FIRST 14
#define CMD_SECTION_NEXT 15
#define CMD_OPEN_REMOTE_PROCESS 16
#define CMD_CLOSE_REMOTE_PROCESS 17
#define CMD_READ_MEMORY_CHUCK 18
#define CMD_WRITE_MEMORY_CHUCK 19

#define API_PORT 3443
#define MAX_PACKET_SIZE 4096
#define MAX_PROC_NAME 360

enum HandleValue {
	INVALID = -1,
	NONE
};

enum ToolType {
	PROCS,
	MODULES,
	SECTIONS
};

/* Perspective of the packet structures*/
/*struct [Name]In <-- In in perspective o Server*/
/*struct [Name]Out --> Are Going Out of the server*/
/*This server is asuming, Host and Dest are same endianesses*/
/*We can even consider a TODO of the endianesses conversions*/

#define CREATE_TOOL_PACKETS(toolName, payloadType) \
PACK(struct toolName##FirstIn{ \
	HANDLE_API mHandleSnap; \
}); \
PACK(struct toolName##FirstOut { \
	payloadType mInfo; \
	bool mRemaining; \
}); \
PACK(struct toolName##NextIn { \
	HANDLE_API mHandleSnap; \
}); \
PACK(struct toolName##NextOut { \
	payloadType mInfo; \
	bool mRemaining; \
});

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

PACK(struct RemoveRemoteSecsToolHelpIn {
	HANDLE_API mHandleSnap;
});

// Proceses
PACK(struct ProcessInfo {
	int32_t mProcessId;
	char mProcessName[MAX_PROC_NAME]{};
});

CREATE_TOOL_PACKETS(Process, ProcessInfo)


// Modules
PACK(struct ModuleInfo {
	uint64_t mBase;
	uint64_t mSize;
	char mPath[260];
});

CREATE_TOOL_PACKETS(Module, ModuleInfo)

// Sections
enum class SectionProtection;
enum class SectionType;

PACK(struct SectionInfo {
	uint64_t mStart;
	uint64_t mEnd;
	SectionType mType; // TODO
	SectionProtection mProt;
	char mPath[260];
});

CREATE_TOOL_PACKETS(Section, SectionInfo)

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

PACK(struct WriteMemoryIn {
	HANDLE_API mhProc;
	uint64_t mAddr;
	uint64_t mSize;
	uint8_t mBuff[1];
});

PACK(struct WriteMemoryOut {
	int64_t mBytesWrited;
});

PACK(struct OpenRemoteProcessOut {
	HANDLE_API hProc;
});

enum class ClientStatus {
	CONN_OK,
	CONN_ALREDY,
	CONN_FAILED
};