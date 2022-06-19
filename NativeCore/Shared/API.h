#pragma once

#include "PackDefs.h"

typedef uint64_t HANDLE_API;

#define CMD_OK 1
#define CMD_HI 2
#define CMD_DISCONNECT 3
#define CMD_WELLCOME 4
#define CMD_CREATE_REMOTE_TOOL_HELP 5
#define CMD_REMOVE_REMOTE_TOOL_HELP 6
#define CMD_STATUS 7

#define API_PORT 3443
#define MAX_PACKET_SIZE 4096

enum HandleValue {
	INVALID = -1,
	NONE
};

enum ToolType {
	PROCS,
	MODULES
};

PACK(struct CreateRemoteToolHelpIn {
	HANDLE_API mProcHandle;
	uint16_t mToolType;
});

PACK(struct CreateRemoteToolHelpOut {
	HANDLE_API mHandleSnap;
});

PACK(struct RemoveRemoteToolHelpIn {
	HANDLE_API mHandleSnap;
});

enum class ClientStatus {
	CONN_OK,
	CONN_ALREDY,
	CONN_FAILED
};