#pragma once

#include <string>
#include "../Shared/API.h"
#include <vector>
#include "NativeCore.hpp"
#include <mutex>

class ClientSocketWindows;

class ServerManager {
private:
	ClientSocketWindows* mSocket;
	static ServerManager* mInstance;
public:
	ServerManager();

	~ServerManager();

	bool Disconnect();

	bool IsConnected();

	bool CheckConnection();

	bool CleanUp();

	bool TryConnect(const std::string& ip, short port);

	bool isValid();

	bool PartiallyConnected();

	ClientSocketWindows* getServerSocket();

	static ServerManager* getInstance();
};

extern std::mutex gMtxReq;

HANDLE_API CreateRemoteTool(uint32_t type, uint32_t pid);
void RemoveRemoteProcsTool(HANDLE_API hTool);

RC_Pointer OpenRemoteProcessServer(RC_Pointer id);
void CloseServerProcess(RC_Pointer hProc);

int ReadMemoryServer(RC_Pointer handle, RC_Pointer address, RC_Pointer buffer, int size);
int WriteMemoryServer(RC_Pointer handle, RC_Pointer address, RC_Pointer buffer, int size);

void EnumerateRemoteSectionsAndModulesServer(RC_Pointer process, EnumerateRemoteSectionsCallback callbackSection, EnumerateRemoteModulesCallback callbackModule);
void EnumerateProcessesServer(EnumerateProcessCallback callbackProcess);
