#pragma once
#include "../Shared/SocketWrapper.h"
#include <winsock.h>

#pragma comment(lib, "Ws2_32.lib")

class WindowsSocketWrapper : public SocketWrapper {
protected:
	SOCKET sket;
	struct sockaddr_in sai;
	virtual bool Init() override;
public:
	WindowsSocketWrapper(std::string ip, short port);
	~WindowsSocketWrapper();

	
	bool Send(void* buff, uintptr_t size) override;
	bool Recive(void* buff, uintptr_t size, uintptr_t* outRecvSize = nullptr) override;
};

class ClientSocketWindows : public WindowsSocketWrapper {
public:
	ClientSocketWindows(const std::string& _ip, short _port);
	bool Init() override;
};