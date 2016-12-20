#include <windows.h>
#include <tlhelp32.h>

#include "NativeCore.hpp"

bool __stdcall AttachDebuggerToProcess(RC_Pointer id)
{
	if (!DebugActiveProcess((DWORD)id))
	{
		return false;
	}

	DebugSetProcessKillOnExit(FALSE);

	return true;
}

void __stdcall DetachDebuggerFromProcess(RC_Pointer id)
{
	DebugActiveProcessStop((DWORD)id);
}

bool __stdcall AwaitDebugEvent(DebugEvent* evt, int timeoutInMilliseconds)
{
	DEBUG_EVENT _evt = { };
	if (!WaitForDebugEvent(&_evt, timeoutInMilliseconds))
	{
		return false;
	}

	evt->ProcessId = (RC_Pointer)_evt.dwProcessId;
	evt->ThreadId = (RC_Pointer)_evt.dwThreadId;

	switch (_evt.dwDebugEventCode)
	{
	case CREATE_PROCESS_DEBUG_EVENT:
		evt->Type = DebugEventType::CreateProcess;
		evt->CreateProcessInfo.FileHandle = _evt.u.CreateProcessInfo.hFile;
		evt->CreateProcessInfo.ProcessHandle = _evt.u.CreateProcessInfo.hProcess;
		break;
	case EXIT_PROCESS_DEBUG_EVENT:
		evt->Type = DebugEventType::ExitProcess;
		evt->ExitProcessInfo.ExitCode = _evt.u.ExitProcess.dwExitCode;
		break;
	case CREATE_THREAD_DEBUG_EVENT:
		evt->Type = DebugEventType::CreateThread;
		evt->CreateThreadInfo.ThreadHandle = _evt.u.CreateThread.hThread;
		break;
	case EXIT_THREAD_DEBUG_EVENT:
		evt->Type = DebugEventType::ExitThread;
		evt->ExitThreadInfo.ExitCode = _evt.u.ExitProcess.dwExitCode;
		break;
	case LOAD_DLL_DEBUG_EVENT:
		evt->Type = DebugEventType::LoadDll;
		evt->LoadDllInfo.FileHandle = _evt.u.LoadDll.hFile;
		evt->LoadDllInfo.BaseOfDll = _evt.u.LoadDll.lpBaseOfDll;
		break;
	case UNLOAD_DLL_DEBUG_EVENT:
		evt->Type = DebugEventType::UnloadDll;
		evt->UnloadDllInfo.BaseOfDll = _evt.u.UnloadDll.lpBaseOfDll;
		break;
	case OUTPUT_DEBUG_STRING_EVENT:
		break;
	case EXCEPTION_DEBUG_EVENT:
		evt->Type = DebugEventType::Exception;

		auto& exception = _evt.u.Exception;

		// Copy basic informations.
		evt->ExceptionInfo.IsFirstChance = exception.dwFirstChance != 0;
		evt->ExceptionInfo.ExceptionAddress = exception.ExceptionRecord.ExceptionAddress;
		evt->ExceptionInfo.ExceptionCode = exception.ExceptionRecord.ExceptionCode;
		evt->ExceptionInfo.ExceptionFlags = exception.ExceptionRecord.ExceptionFlags;

		auto handle = OpenThread(THREAD_GET_CONTEXT, FALSE, _evt.dwThreadId);

		CONTEXT ctx = { };
		ctx.ContextFlags = CONTEXT_CONTROL | CONTEXT_INTEGER | CONTEXT_DEBUG_REGISTERS;
		GetThreadContext(handle, &ctx);

		// Check if breakpoint was a hardware breakpoint.
		if (ctx.Dr6 & 0b0001)
		{
			evt->ExceptionInfo.CausedBy = HardwareBreakpointRegister::Dr0;
		}
		else if (ctx.Dr6 & 0b0010)
		{
			evt->ExceptionInfo.CausedBy = HardwareBreakpointRegister::Dr1;
		}
		else if (ctx.Dr6 & 0b0100)
		{
			evt->ExceptionInfo.CausedBy = HardwareBreakpointRegister::Dr2;
		}
		else if (ctx.Dr6 & 0b1000)
		{
			evt->ExceptionInfo.CausedBy = HardwareBreakpointRegister::Dr3;
		}
		else
		{
			evt->ExceptionInfo.CausedBy = HardwareBreakpointRegister::InvalidRegister;
		}

		// Copy registers.
		auto& reg = evt->ExceptionInfo.Registers;
#ifdef _WIN64
		reg.Rax = (RC_Pointer)ctx.Rax;
		reg.Rbx = (RC_Pointer)ctx.Rbx;
		reg.Rcx = (RC_Pointer)ctx.Rcx;
		reg.Rdx = (RC_Pointer)ctx.Rdx;
		reg.Rdi = (RC_Pointer)ctx.Rdi;
		reg.Rsi = (RC_Pointer)ctx.Rsi;
		reg.Rsp = (RC_Pointer)ctx.Rsp;
		reg.Rbp = (RC_Pointer)ctx.Rbp;
		reg.Rip = (RC_Pointer)ctx.Rip;

		reg.R8 = (RC_Pointer)ctx.R8;
		reg.R9 = (RC_Pointer)ctx.R9;
		reg.R10 = (RC_Pointer)ctx.R10;
		reg.R11 = (RC_Pointer)ctx.R11;
		reg.R12 = (RC_Pointer)ctx.R12;
		reg.R13 = (RC_Pointer)ctx.R13;
		reg.R14 = (RC_Pointer)ctx.R14;
		reg.R15 = (RC_Pointer)ctx.R15;
#else
		reg.Eax = (RC_Pointer)ctx.Eax;
		reg.Ebx = (RC_Pointer)ctx.Ebx;
		reg.Ecx = (RC_Pointer)ctx.Ecx;
		reg.Edx = (RC_Pointer)ctx.Edx;
		reg.Edi = (RC_Pointer)ctx.Edi;
		reg.Esi = (RC_Pointer)ctx.Esi;
		reg.Esp = (RC_Pointer)ctx.Esp;
		reg.Ebp = (RC_Pointer)ctx.Ebp;
		reg.Eip = (RC_Pointer)ctx.Eip;
#endif

		CloseHandle(handle);

		break;
	}

	return true;
}

void __stdcall HandleDebugEvent(DebugEvent* evt)
{
	DWORD continueStatus = 0;
	switch (evt->ContinueStatus)
	{
	case DebugContinueStatus::Handled:
		continueStatus = DBG_CONTINUE;
		break;
	case DebugContinueStatus::NotHandled:
		continueStatus = DBG_EXCEPTION_NOT_HANDLED;
		break;
	}

	ContinueDebugEvent((DWORD)evt->ProcessId, (DWORD)evt->ThreadId, continueStatus);
}

bool __stdcall SetHardwareBreakpoint(RC_Pointer processId, RC_Pointer address, HardwareBreakpointRegister reg, HardwareBreakpointTrigger type, HardwareBreakpointSize size, bool set)
{
	if (reg == HardwareBreakpointRegister::InvalidRegister)
	{
		return false;
	}

	decltype(CONTEXT::Dr0) addressValue = 0;
	int typeValue = 0;
	int sizeValue = 0;

	if (set)
	{
		addressValue = (decltype(CONTEXT::Dr0))address;

		if (type == HardwareBreakpointTrigger::Execute)
			typeValue = 0;
		else if (type == HardwareBreakpointTrigger::Access)
			typeValue = 3;
		else if (type == HardwareBreakpointTrigger::Write)
			typeValue = 1;

		if (size == HardwareBreakpointSize::Size1)
			sizeValue = 0;
		else if (size == HardwareBreakpointSize::Size2)
			sizeValue = 1;
		else if (size == HardwareBreakpointSize::Size4)
			sizeValue = 3;
		else if (size == HardwareBreakpointSize::Size8)
			sizeValue = 2;
	}
	
	auto SetBits = [](DWORD_PTR& dw, int lowBit, int bits, int newValue)
	{
		DWORD_PTR mask = (1 << bits) - 1;
		dw = (dw & ~(mask << lowBit)) | (newValue << lowBit);
	};

	auto handle = CreateToolhelp32Snapshot(TH32CS_SNAPTHREAD, 0);
	if (handle != INVALID_HANDLE_VALUE)
	{
		THREADENTRY32 pe32 = {};
		pe32.dwSize = sizeof(THREADENTRY32);
		if (Thread32First(handle, &pe32))
		{
			do
			{
				if (pe32.th32OwnerProcessID == (DWORD)processId)
				{
					auto handle = OpenThread(THREAD_SUSPEND_RESUME | THREAD_GET_CONTEXT | THREAD_SET_CONTEXT, FALSE, pe32.th32ThreadID);

					SuspendThread(handle);

					CONTEXT ctx = { 0 };
					ctx.ContextFlags = CONTEXT_DEBUG_REGISTERS;
					GetThreadContext(handle, &ctx);

					int index = 0;

					switch (reg)
					{
					case HardwareBreakpointRegister::Dr0:
						index = 0;
						ctx.Dr0 = addressValue;
						break;
					case HardwareBreakpointRegister::Dr1:
						index = 1;
						ctx.Dr1 = addressValue;
						break;
					case HardwareBreakpointRegister::Dr2:
						index = 2;
						ctx.Dr2 = addressValue;
						break;
					case HardwareBreakpointRegister::Dr3:
						index = 3;
						ctx.Dr3 = addressValue;
						break;
					}

					SetBits(ctx.Dr7, 16 + index * 4, 2, typeValue);
					SetBits(ctx.Dr7, 18 + index * 4, 2, sizeValue);
					SetBits(ctx.Dr7, index * 2, 1, set ? 1 : 0);

					SetThreadContext(handle, &ctx);

					ResumeThread(handle);

					CloseHandle(handle);
				}
			} while (Thread32Next(handle, &pe32));
		}

		CloseHandle(handle);
	}

	return true;
}
