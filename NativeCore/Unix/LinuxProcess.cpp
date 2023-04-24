#include "LinuxProcess.h"
#include <unistd.h>
#include <dirent.h>
#include <stdio.h>
#include <string>

LinuxProcess::LinuxProcess(uintptr_t _pid)
    : Process(_pid)
    , mapF(nullptr)
    , memFd(-1)
{
    char memPath[1024];

    sprintf(mapPath, "/proc/%d/maps", int(pid));
    sprintf(memPath, "/proc/%d/mem", int(pid));

    mapF = fopen(mapPath, "r");
    memFd = open(memPath, O_RDWR);
}

LinuxProcess::LinuxProcess(void* fd)
    : Process(0)
    , mapF(nullptr)
    , memFd(size_t(fd))
{}

LinuxProcess::~LinuxProcess()
{
    if(mapF)
    {
        fclose(mapF);
    }

    if(memFd != -1)
    {
        close(memFd);
    }
}

int LinuxProcess::ReadMemory(uintptr_t addr, void* buffer, size_t size){
	std::lock_guard<std::mutex> lck(ioLock);

    if(lseek64(memFd, addr, SEEK_SET))
    {
        return read(memFd, buffer, size);
    }

    return -1;
}

int LinuxProcess::WriteMemory(uintptr_t addr, void* buffer, size_t size){
	std::lock_guard<std::mutex> lck(ioLock);

    if(lseek64(memFd, addr, SEEK_SET))
    {
        return write(memFd, buffer, size);
    }

    return -1;
}

bool LinuxProcess::Find(const std::string& procName, LinuxProcess** ppOutProc)
{
    DIR* procDir = nullptr;
    int cmdLineBuffLen = procName.size() + 1;

    *ppOutProc = nullptr;

    if((procDir = opendir("/proc/")) != nullptr)
    {
        dirent* currDir = nullptr;
        char* cmdLineBuff = (char*)malloc(cmdLineBuffLen);

        while(((currDir = readdir(procDir)) != nullptr) && !*ppOutProc)
        {
            int currPid = 0;

            if((currPid = atoi(currDir->d_name)) != 0)
            {
                char cmdLinePath[1024];
                int currProcCmdLineFd = -1;

                sprintf(cmdLinePath, "/proc/%d/cmdline", currPid);

                if((currProcCmdLineFd = open(cmdLinePath, O_RDONLY)) != -1)
                {
                    if(read(currProcCmdLineFd, cmdLineBuff, cmdLineBuffLen))
                    {
                        if(!strcmp(cmdLineBuff, procName.c_str()))
                        {
                            *ppOutProc = new LinuxProcess(currPid);
                        }
                    }

                    close(currProcCmdLineFd);
                }
            }
        }

        free(cmdLineBuff);
        closedir(procDir);

        if(*ppOutProc)
        {
            return true;
        }
    }

    return (bool)(*ppOutProc);
}