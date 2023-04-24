#pragma once 

#include <cstdint>
#include <deque>

template<typename T>
struct SnapshotBase  {
    protected:
    short type;

    public:
	std::deque<T> mArray;
    short getSnapshotType()
    {
        return type;
    }
};