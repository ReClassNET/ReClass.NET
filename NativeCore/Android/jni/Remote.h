#pragma once

#include <functional>

struct ClientSocketLinux;

struct ProcessInfo{

};

struct SnapshotTool{

};


extern std::function<void(ClientSocketLinux*)> gfnHandleConn;