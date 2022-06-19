#include "LinuxSocketWrapper.h"
#include <sys/socket.h>
#include <unistd.h>
#include <arpa/inet.h>

LinuxSocketWrapper::LinuxSocketWrapper(const std::string& _ip, short _port)
    : SocketWrapper(_ip, _port)
    , bCloseSocket(true)
{}

bool LinuxSocketWrapper::Init()
{
    // Creating The Socket
    family = AF_INET;

    if ((sock = socket(family, SOCK_STREAM, 0)) == -1)
        return false;

    return true;
}


LinuxSocketWrapper::LinuxSocketWrapper(int _sock)
    : sock(_sock)
    , bCloseSocket(false)
    , sai{}
{}

LinuxSocketWrapper::~LinuxSocketWrapper()
{
    if(bCloseSocket && sock != -1)
        close(sock);
}

bool LinuxSocketWrapper::Send(void* buff, uintptr_t size)
{
    uintptr_t recvSize =  write(sock, buff, size);

    return ASSERT_SOCK_RESULT(recvSize);
}

bool LinuxSocketWrapper::Recive(void* buff, uintptr_t size, uintptr_t* outRecvSize)
{
	uintptr_t recvSize = read(sock, buff, size);

	if(outRecvSize)
		*outRecvSize = recvSize;

    return ASSERT_SOCK_RESULT(recvSize);
}

ClientSocketLinux::ClientSocketLinux(int _sock)
    : LinuxSocketWrapper(_sock)
{}

ClientSocketLinux::ClientSocketLinux(const std::string& _ip, short _port)
        : LinuxSocketWrapper(_ip, _port)
{}

bool ClientSocketLinux::Init()
{
    if(LinuxSocketWrapper::Init())
    {
        // Setting Up Hint Values
        sai.sin_family = family;
        sai.sin_port  = htons(port);
        sai.sin_addr.s_addr = inet_addr(ip.c_str());

        // Making the Connection
        if(0 != connect(sock, (const sockaddr*)&sai, sizeof(sai)))
        {
            close(sock);
            return false;
        }

        return true;
    }
    return false;
}

bool ServerSocketLinux::Init()
{
    if(!LinuxSocketWrapper::Init())
        return false;

    // Forcefully attaching socket to the port 8080
    int opt = 1;

    if (setsockopt(sock, SOL_SOCKET, SO_REUSEADDR | SO_REUSEPORT,
                                                &opt, sizeof(opt)))
    {
        close(sock);
        return false;
    }

    // Binding the Socket
    sai.sin_family = family;
    sai.sin_port  = htons(port);
    sai.sin_addr.s_addr = inet_addr(ip.c_str());

    if(0 != bind(sock, (const sockaddr*)&sai, sizeof(sai)))
    {
        close(sock);
        return false;
    }

    // Making the Socket to Listen
    if(0 != listen(sock, SOMAXCONN))
    {
        close(sock);
        return false;
    }

    return true;
}

ServerSocketLinux::ServerSocketLinux(const std::string& _ip, short _port)
    : LinuxSocketWrapper(_ip, _port)
{}

ClientSocketLinux* ServerSocketLinux::WaitConnection()
{
    //Waiting the COnnection
    socklen_t sockAddrLen = sizeof(sai);

    return new ClientSocketLinux(accept(sock, (sockaddr*)&sai, &sockAddrLen));
}