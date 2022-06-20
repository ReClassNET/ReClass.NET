#pragma once 

#include <cstdint>
#include <vector>

template<typename T>
struct SnapshotBase {
    protected:
    std::vector<T> mArray;
    uint32_t mArrayPos = 0;
    short type;
    

    public:
    short getSnapshotType()
    {
        return type;
    }

    bool PopFirst(T** outObj){
        if(mArray.size() > 0 && mArrayPos == 0)
        {
            *outObj = &mArray[mArrayPos++];
            return true;
        }

        return false;
    }

    bool PopNext(T** outObj){
        if(mArrayPos < mArray.size())
        {
            *outObj = &mArray[mArrayPos++];
            return true;
        }

        return false;
    }

    bool PopFirst(T& outObj)
    {
        if(mArray.size() > 0 && mArrayPos == 0)
        {
            outObj = mArray[mArrayPos++];
            return true;
        }

        return false;
    }

    bool PopNext(T& outObj)
    {
        if(mArrayPos < mArray.size())
        {
            outObj = mArray[mArrayPos++];
            return true;
        }

        return false;
    }
};