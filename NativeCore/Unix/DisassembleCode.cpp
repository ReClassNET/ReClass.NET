#include <vector>
#include <cstdint>
//#include <beaengine/BeaEngine.h>

#include "NativeCore.hpp"

extern "C" bool DisassembleCode(RC_Pointer address, RC_Size length, RC_Pointer virtualAddress, InstructionData* instruction)
{
	return false;
}
