#include "WindowsSocketWrapper.h"

bool gWSAInited = false;

WindowsSocketWrapper::WindowsSocketWrapper(std::string ip, short port)
	: SocketWrapper(ip, port)
	, sket(INVALID_SOCKET)
	, sai{}
{}

WindowsSocketWrapper::~WindowsSocketWrapper()
{
	// Freeing the Socket
	if (sket != INVALID_SOCKET)
		closesocket(sket);

	// Checking for WSA Initted
	if (gWSAInited)
	{
		WSACleanup();
		gWSAInited = false;
	}
}

bool WindowsSocketWrapper::Init()
{
	// Checking for WSA Not Initted
	if (!gWSAInited)
	{
		WSAData wd;

		if (WSAStartup(MAKEWORD(2, 2), &wd) != 0)
			return false;

		if (wd.wVersion != MAKEWORD(2, 2))
		{
			WSACleanup();
			return false;;
		}

		gWSAInited = true;
	}

	// Creating The Socket
	auto family = AF_INET;

	if ((sket = socket(family, SOCK_STREAM, 0)) == INVALID_SOCKET)
	{
		WSACleanup();
		return false;
	}

	//Binding the Socket
	sai.sin_family = family;
	sai.sin_addr.S_un.S_addr = inet_addr(ip.c_str());
	sai.sin_port = htons(port);

	bind(sket, (const sockaddr*)&sai, sizeof(sai));

	return true;
}

bool WindowsSocketWrapper::Send(void* buff, uintptr_t size)
{
	uintptr_t sendedBytes = send(sket, (char*)buff, int(size), 0);

	return ASSERT_SOCK_RESULT(sendedBytes);
}

bool WindowsSocketWrapper::Recive(void* buff, uintptr_t size, uintptr_t* outRecvSize)
{
	uintptr_t recvdSize = recv(sket, (char*)buff, int(size), 0);

	if (outRecvSize)
		*outRecvSize = recvdSize;

	return ASSERT_SOCK_RESULT(recvdSize);
}

ClientSocketWindows::ClientSocketWindows(const std::string& _ip, short _port)
	: WindowsSocketWrapper(_ip, _port)
{}

bool ClientSocketWindows::Init()
{
	if (WindowsSocketWrapper::Init())
	{
		if (connect(sket, (const sockaddr*)&sai, sizeof(sai)) == SOCKET_ERROR)
		{
			closesocket(sket);
			WSACleanup();
			return false;
		}

		return true;
	}

	return false;
}
