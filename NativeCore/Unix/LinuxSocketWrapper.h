#pragma once
#include "../Shared/SocketWrapper.h"
#include <netinet/in.h>

struct LinuxSocketWrapper : public SocketWrapper{
    private: 
    bool bCloseSocket;
    protected:
    int sock;
    int family;
    struct sockaddr_in sai;
    public:
    LinuxSocketWrapper(const std::string& _ip, short _port);
    LinuxSocketWrapper(int _sock);
    ~LinuxSocketWrapper();

    virtual bool Init() override;

    bool Send(void* buff, uintptr_t size) override;
    bool Recive(void* buff, uintptr_t size, uintptr_t* outRecvSize = nullptr) override;
};

struct ClientSocketLinux : public LinuxSocketWrapper{
    public:
    ClientSocketLinux(int _sock);
    ClientSocketLinux(const std::string& _ip, short _port);
    bool Init() override;
};

struct ServerSocketLinux : public LinuxSocketWrapper{
private:
    public:
    ServerSocketLinux(const std::string& _ip, short _port);
    bool Init() override;
    ClientSocketLinux* WaitConnection();
};
