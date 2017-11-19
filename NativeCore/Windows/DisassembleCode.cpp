#include "../Shared/DistormHelper.hpp"

bool __stdcall DisassembleCode(RC_Pointer address, RC_Size length, RC_Pointer virtualAddress, bool determineStaticInstructionBytes, InstructionData* instruction)
{
	return DisassembleCodeImpl(address, length, virtualAddress, determineStaticInstructionBytes, instruction);
}
