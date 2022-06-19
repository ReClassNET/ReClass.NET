#include "NativeCore.hpp"

#include "WindowsSocketWrapper.h"
#include "../Shared/API.h"

ClientSocketWindows* gServerSocket = nullptr;
unsigned char gPacketHolder[MAX_PACKET_SIZE]{ 0 };

class ServerManager {
private:
	ClientSocketWindows* mSocket;

public:
	ServerManager() : mSocket(nullptr) {};

	~ServerManager() {
		Disconnect();
	};

	bool Disconnect()
	{
		if (mSocket) mSocket->SendMsg(CMD_DISCONNECT);

		CleanUp();

		return true;
	}

	bool IsConnected()
	{
		return mSocket && isValidConnection();
	}

	bool isValidConnection()
	{
		if (mSocket->SendMsg(CMD_STATUS))
		{
			if (mSocket->GetMsg() == CMD_OK)
				return true;
		}

		return false;
	}

	bool CleanUp()
	{
		if (mSocket)
			delete mSocket;

		mSocket = nullptr;

		return true;
	}

	bool TryConnect(const std::string& ip, short port)
	{
		mSocket = new ClientSocketWindows(ip, port);

		if (mSocket)
		{
			if (mSocket->Init())
			{
				if (mSocket->SendMsg(CMD_HI))
				{
					if (mSocket->GetMsg() == CMD_WELLCOME)
						return true;
				}
			}
		}

		CleanUp();
		return false;
	}
};

ServerManager* gServerMgr = nullptr;

int32_t RC_CallConv ConnectServer(const char* pIpStr, short port)
{
	if (!gServerMgr)
		gServerMgr = new ServerManager();

	if (gServerMgr->IsConnected())
		return 1;

	gServerMgr->CleanUp();

	if (gServerMgr->TryConnect(std::string(pIpStr), port))
		return 0;
	else
		return 2;
}

void RC_CallConv DisconnectServer()
{
	gServerMgr->Disconnect();
}