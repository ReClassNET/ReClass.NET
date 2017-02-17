#include <windows.h>
#include <beaengine/BeaEngine.h>

#include "NativeCore.hpp"

bool __stdcall DisassembleCode(RC_Pointer address, RC_Size length, RC_Pointer virtualAddress, InstructionData* instruction)
{
	DISASM disasm = {};
	disasm.Options = NasmSyntax;
#ifdef RECLASSNET64
	disasm.Archi = 64;
#endif
	disasm.VirtualAddr = reinterpret_cast<UInt64>(virtualAddress);
	disasm.EIP = reinterpret_cast<UIntPtr>(address);
	disasm.SecurityBlock = static_cast<UInt32>(length);

	auto disamLength = Disasm(&disasm);
	if (disamLength == OUT_OF_BLOCK || disamLength == UNKNOWN_OPCODE)
	{
		return false;
	}

	instruction->Length = disamLength;
	std::memcpy(instruction->Data, address, disamLength);
	MultiByteToUnicode(disasm.CompleteInstr, instruction->Instruction, 64);

	return true;
}
