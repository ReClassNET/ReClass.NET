#include <windows.h>
#include <tlhelp32.h>

#include "NativeCore.hpp"

bool RC_CallConv AttachDebuggerToProcess(RC_Pointer id)
{
	if (!DebugActiveProcess(static_cast<DWORD>(reinterpret_cast<size_t>(id))))
	{
		return false;
	}

	DebugSetProcessKillOnExit(FALSE);

	return true;
}

void RC_CallConv DetachDebuggerFromProcess(RC_Pointer id)
{
	DebugActiveProcessStop(static_cast<DWORD>(reinterpret_cast<size_t>(id)));
}

bool RC_CallConv AwaitDebugEvent(DebugEvent* evt, int timeoutInMilliseconds)
{
	DEBUG_EVENT _evt = { };
	if (!WaitForDebugEvent(&_evt, timeoutInMilliseconds))
	{
		return false;
	}

	auto result = false;

	evt->ProcessId = reinterpret_cast<RC_Pointer>(static_cast<size_t>(_evt.dwProcessId));
	evt->ThreadId = reinterpret_cast<RC_Pointer>(static_cast<size_t>(_evt.dwThreadId));

	switch (_evt.dwDebugEventCode)
	{
	case CREATE_PROCESS_DEBUG_EVENT:
		CloseHandle(_evt.u.CreateProcessInfo.hFile);
		break;
	case LOAD_DLL_DEBUG_EVENT:
		CloseHandle(_evt.u.LoadDll.hFile);
		break;
	case EXCEPTION_DEBUG_EVENT:
		auto& exception = _evt.u.Exception;

		// Copy basic informations.
		evt->ExceptionInfo.ExceptionAddress = exception.ExceptionRecord.ExceptionAddress;
		evt->ExceptionInfo.ExceptionCode = exception.ExceptionRecord.ExceptionCode;
		evt->ExceptionInfo.ExceptionFlags = exception.ExceptionRecord.ExceptionFlags;

		const auto handle = OpenThread(THREAD_GET_CONTEXT, FALSE, _evt.dwThreadId);

		CONTEXT ctx = { };
		ctx.ContextFlags = CONTEXT_CONTROL | CONTEXT_INTEGER | CONTEXT_DEBUG_REGISTERS;
		GetThreadContext(handle, &ctx);

		DebugRegister6 dr6;
		dr6.Value = ctx.Dr6;

		// Check if breakpoint was a hardware breakpoint.
		if (dr6.DR0)
		{
			evt->ExceptionInfo.CausedBy = HardwareBreakpointRegister::Dr0;
		}
		else if (dr6.DR1)
		{
			evt->ExceptionInfo.CausedBy = HardwareBreakpointRegister::Dr1;
		}
		else if (dr6.DR2)
		{
			evt->ExceptionInfo.CausedBy = HardwareBreakpointRegister::Dr2;
		}
		else if (dr6.DR3)
		{
			evt->ExceptionInfo.CausedBy = HardwareBreakpointRegister::Dr3;
		}
		else
		{
			evt->ExceptionInfo.CausedBy = HardwareBreakpointRegister::InvalidRegister;
		}

		// Copy registers.
		auto& reg = evt->ExceptionInfo.Registers;
#ifdef RECLASSNET64
		reg.Rax = reinterpret_cast<RC_Pointer>(ctx.Rax);
		reg.Rbx = reinterpret_cast<RC_Pointer>(ctx.Rbx);
		reg.Rcx = reinterpret_cast<RC_Pointer>(ctx.Rcx);
		reg.Rdx = reinterpret_cast<RC_Pointer>(ctx.Rdx);
		reg.Rdi = reinterpret_cast<RC_Pointer>(ctx.Rdi);
		reg.Rsi = reinterpret_cast<RC_Pointer>(ctx.Rsi);
		reg.Rsp = reinterpret_cast<RC_Pointer>(ctx.Rsp);
		reg.Rbp = reinterpret_cast<RC_Pointer>(ctx.Rbp);
		reg.Rip = reinterpret_cast<RC_Pointer>(ctx.Rip);

		reg.R8 = reinterpret_cast<RC_Pointer>(ctx.R8);
		reg.R9 = reinterpret_cast<RC_Pointer>(ctx.R9);
		reg.R10 = reinterpret_cast<RC_Pointer>(ctx.R10);
		reg.R11 = reinterpret_cast<RC_Pointer>(ctx.R11);
		reg.R12 = reinterpret_cast<RC_Pointer>(ctx.R12);
		reg.R13 = reinterpret_cast<RC_Pointer>(ctx.R13);
		reg.R14 = reinterpret_cast<RC_Pointer>(ctx.R14);
		reg.R15 = reinterpret_cast<RC_Pointer>(ctx.R15);
#else
		reg.Eax = reinterpret_cast<RC_Pointer>(ctx.Eax);
		reg.Ebx = reinterpret_cast<RC_Pointer>(ctx.Ebx);
		reg.Ecx = reinterpret_cast<RC_Pointer>(ctx.Ecx);
		reg.Edx = reinterpret_cast<RC_Pointer>(ctx.Edx);
		reg.Edi = reinterpret_cast<RC_Pointer>(ctx.Edi);
		reg.Esi = reinterpret_cast<RC_Pointer>(ctx.Esi);
		reg.Esp = reinterpret_cast<RC_Pointer>(ctx.Esp);
		reg.Ebp = reinterpret_cast<RC_Pointer>(ctx.Ebp);
		reg.Eip = reinterpret_cast<RC_Pointer>(ctx.Eip);
#endif

		CloseHandle(handle);

		result = true;
		break;
	}

	if (result == false)
	{
		ContinueDebugEvent(_evt.dwProcessId, _evt.dwThreadId, DBG_CONTINUE);
	}

	return result;
}

void RC_CallConv HandleDebugEvent(DebugEvent* evt)
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

	ContinueDebugEvent(static_cast<DWORD>(reinterpret_cast<size_t>(evt->ProcessId)), static_cast<DWORD>(reinterpret_cast<size_t>(evt->ThreadId)), continueStatus);
}

bool RC_CallConv SetHardwareBreakpoint(RC_Pointer id, RC_Pointer address, HardwareBreakpointRegister reg, HardwareBreakpointTrigger type, HardwareBreakpointSize size, bool set)
{
	if (reg == HardwareBreakpointRegister::InvalidRegister)
	{
		return false;
	}

	decltype(CONTEXT::Dr0) addressValue = 0;
	int accessValue = 0;
	int lengthValue = 0;

	if (set)
	{
		addressValue = reinterpret_cast<decltype(CONTEXT::Dr0)>(address);

		if (type == HardwareBreakpointTrigger::Execute)
			accessValue = 0;
		else if (type == HardwareBreakpointTrigger::Access)
			accessValue = 3;
		else if (type == HardwareBreakpointTrigger::Write)
			accessValue = 1;

		if (size == HardwareBreakpointSize::Size1)
			lengthValue = 0;
		else if (size == HardwareBreakpointSize::Size2)
			lengthValue = 1;
		else if (size == HardwareBreakpointSize::Size4)
			lengthValue = 3;
		else if (size == HardwareBreakpointSize::Size8)
			lengthValue = 2;
	}

	const auto snapshotHandle = CreateToolhelp32Snapshot(TH32CS_SNAPTHREAD, 0);
	if (snapshotHandle != INVALID_HANDLE_VALUE)
	{
		THREADENTRY32 te32 = {};
		te32.dwSize = sizeof(THREADENTRY32);
		if (Thread32First(snapshotHandle, &te32))
		{
			do
			{
				if (te32.th32OwnerProcessID == static_cast<DWORD>(reinterpret_cast<size_t>(id)))
				{
					const auto threadHandle = OpenThread(THREAD_SUSPEND_RESUME | THREAD_GET_CONTEXT | THREAD_SET_CONTEXT, FALSE, te32.th32ThreadID);

					SuspendThread(threadHandle);

					CONTEXT ctx = { 0 };
					ctx.ContextFlags = CONTEXT_DEBUG_REGISTERS;
					GetThreadContext(threadHandle, &ctx);

					DebugRegister7 dr7;
					dr7.Value = ctx.Dr7;

					switch (reg)
					{
					case HardwareBreakpointRegister::Dr0:
						ctx.Dr0 = addressValue;
						dr7.G0 = true;
						dr7.RW0 = accessValue;
						dr7.Len0 = lengthValue;
						break;
					case HardwareBreakpointRegister::Dr1:
						ctx.Dr1 = addressValue;
						dr7.G1 = true;
						dr7.RW1 = accessValue;
						dr7.Len1 = lengthValue;
						break;
					case HardwareBreakpointRegister::Dr2:
						ctx.Dr2 = addressValue;
						dr7.G2 = true;
						dr7.RW2 = accessValue;
						dr7.Len2 = lengthValue;
						break;
					case HardwareBreakpointRegister::Dr3:
						ctx.Dr3 = addressValue;
						dr7.G3 = true;
						dr7.RW3 = accessValue;
						dr7.Len3 = lengthValue;
						break;
					}

					ctx.Dr7 = dr7.Value;

					SetThreadContext(threadHandle, &ctx);

					ResumeThread(threadHandle);

					CloseHandle(threadHandle);
				}
			} while (Thread32Next(snapshotHandle, &te32));
		}

		CloseHandle(snapshotHandle);
	}

	return true;
}
