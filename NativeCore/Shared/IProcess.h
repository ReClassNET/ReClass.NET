#pragma once

#include <cstdint>
#include <vector>
#include <string>

class Process {
protected:
	uintptr_t pid;
public:
	Process(uintptr_t _pid)
		: pid(_pid)
	{}

	~Process() = default;

	virtual int ReadMemory(uintptr_t addr, void* buffer, size_t size) = 0;

	virtual int WriteMemory(uintptr_t addr, void* buffer, size_t size) = 0;

	uintptr_t getProcId()
	{
		return pid;
	}

	virtual Process* Clone() = 0;

	template<typename T>
	T ReadMemoryWrapper(uintptr_t addr)
	{
		T obj;

		ReadMemory(addr, &obj, sizeof(obj));

		return obj;
	}

	template<typename T>
	bool WriteMemoryWrapper(uintptr_t addr, const T& obj)
	{
		return WriteMemory(addr, &obj, sizeof(T));
	}

	virtual bool isOpen() = 0;
};

