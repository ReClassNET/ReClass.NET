#include  "stdio.h"
#include <LinuxSocketWrapper.h>
#include <API.h>
#include <thread>
#include <vector>
#include <Client.h>
#include <unordered_map>

int main()
{
	std::unique_ptr<ServerSocketLinux> pServerSocket = std::make_unique<ServerSocketLinux>("127.0.0.1", API_PORT);

    if(!pServerSocket->Init())
    {
        printf("Error, Cant create the server");
    }

    // Server Main Loop
    std::unordered_map<ReclassClient*, std::unique_ptr<ReclassClient>> allConn;

    while(true)
    {
        SocketWrapper* pIncomingClient = nullptr;

		if((pIncomingClient = pServerSocket->WaitConnection()) == nullptr)
			continue;

		std::unique_ptr<SocketWrapper> clientSocket = std::unique_ptr<SocketWrapper>(pIncomingClient);

		std::unique_ptr<ReclassClient> currConn = std::make_unique<ReclassClient>();

		currConn->setClientSocket(clientSocket);

		currConn->RunThread();

		allConn[currConn.get()] = std::move(currConn);
    }

    return 0;
}