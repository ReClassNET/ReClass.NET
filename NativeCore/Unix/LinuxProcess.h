#pragma once

#include "IProcess.h"
#include "FDUtils.h"
#include <mutex>

class LinuxProcess : public Process
{
    private:
    char mapPath[1024];
    FILE* mapF;
    int memFd;
	std::mutex ioLock;
    public:
    LinuxProcess(uintptr_t _pid);
    LinuxProcess(void* fd);

    ~LinuxProcess();

    int ReadMemory(uintptr_t addr, void* buffer, size_t size) override;

    int WriteMemory(uintptr_t addr, void* buffer, size_t size) override;

    Process* Clone() override {
        return nullptr;
    }

    bool isOpen() override{
        return FDUtils::isValid(memFd);
    }

    static bool Find(const std::string& procName, LinuxProcess** ppOutProc);

    void Refresh()
    {
        fclose(mapF);
        mapF = fopen(mapPath, "r");
    }

    FILE* getMapFile()
    {
        fseek(mapF, 0, SEEK_SET);

        return mapF;
    }

    int getMemFd()
    {
        return memFd;
    }
};