#pragma once

#include <functional>

struct ClientSocketLinux;

extern std::function<void(ClientSocketLinux*)> gfnHandleConn;