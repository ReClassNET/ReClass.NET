#include "ServerRemoteTool.h"
#include "WindowsSocketWrapper.h"

ServerManager* ServerManager::mInstance = nullptr;

ServerManager::ServerManager() : mSocket(nullptr) {}

ServerManager::~ServerManager() {
	Disconnect();
}

bool ServerManager::Disconnect()
{
	std::lock_guard lck(gMtxReq);
	if (mSocket) mSocket->SendMsg(CMD_DISCONNECT);

	CleanUp();

	return true;
}

bool ServerManager::IsConnected()
{
	return PartiallyConnected() && CheckConnection();
}

bool ServerManager::CheckConnection()
{
	std::lock_guard lck(gMtxReq);
	mSocket->SendMsg(CMD_STATUS);

	if (mSocket->GetMsg() == CMD_OK)
		return true;

	return false;
}

bool ServerManager::CleanUp()
{
	if (mSocket)
		delete mSocket;

	mSocket = nullptr;

	return true;
}

bool ServerManager::TryConnect(const std::string& ip, short port)
{
	std::lock_guard lck(gMtxReq);
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

ClientSocketWindows* ServerManager::getServerSocket() { return mSocket; }

ServerManager* ServerManager::getInstance()
{
	if (!mInstance)
		mInstance = new ServerManager();

	return mInstance;
}

bool ServerManager::PartiallyConnected()
{
	return mSocket != nullptr;
}