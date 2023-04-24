#pragma once
#include <string>

class SocketWrapper {
protected:
	std::string ip;
	short port;
public:
	SocketWrapper()
		: ip("")
		, port(0x0)
	{}

	SocketWrapper(const std::string& _ip, short _port)
		: ip(_ip)
		, port(_port)
	{}

	virtual ~SocketWrapper(){}

	virtual bool Init() = 0;
	virtual bool Send(void* buff, uintptr_t size) = 0;
	virtual bool Recive(void* buff, uintptr_t size, uintptr_t* outRecvSize = nullptr) = 0;
	bool SendMsg(int32_t id);
	int32_t GetMsg();
};

#define ASSERT_SOCK_RESULT(x) (-1 != x && x != 0)
