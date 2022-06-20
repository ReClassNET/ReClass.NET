#pragma once
#include <fcntl.h>
#include "sys/stat.h"

class FDUtils{
    public:
    static bool isValid(int fd)
    {
        struct stat fdStat;

        return !fstat(fd, &fdStat);
    }
};