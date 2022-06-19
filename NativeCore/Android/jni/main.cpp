#include  "stdio.h"
#include <LinuxSocketWrapper.h>
#include <API.h>
#include <thread>
#include <vector>
#include "Remote.h"

ServerSocketLinux* pServerSocket = nullptr;

int main()
{
    pServerSocket = new ServerSocketLinux("127.0.0.1", API_PORT);

    if(!pServerSocket->Init())
    {
        printf("Error, Cant create the server");
    }

    // Server Main Loop
    std::vector<std::thread> vecClients;

    while(true)
    {
        ClientSocketLinux* pIncomingClient = nullptr;

        if((pIncomingClient = pServerSocket->WaitConnection()))
        {
            vecClients.push_back(std::thread(gfnHandleConn, pIncomingClient));
        }
    }
    

    return 0;
}