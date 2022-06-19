#pragma once

#include <string>
#include "../Shared/API.h"

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

HANDLE_API CreateRemoteTool(uint32_t type, uint32_t pid);
void RemoveRemoteProcsTool(HANDLE_API hTool);

bool ProcessFirst(HANDLE_API hSnap, ProcessInfo* pPi);
bool ProcessNext(HANDLE_API hSnap, ProcessInfo* pPi);
