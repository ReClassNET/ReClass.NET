#include "../Shared/DistormHelper.hpp"

extern "C" bool DisassembleCode(RC_Pointer address, RC_Size length, RC_Pointer virtualAddress, bool determineStaticInstructionBytes, InstructionData* instruction)
{
	return DisassembleCodeImpl(address, length, virtualAddress, determineStaticInstructionBytes, instruction);
}
