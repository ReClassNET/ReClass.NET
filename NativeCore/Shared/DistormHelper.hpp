#pragma once

#include "../ReClassNET_Plugin.hpp"

bool DisassembleCodeImpl(const RC_Pointer address, const RC_Size length, const RC_Pointer virtualAddress, const bool determineStaticInstructionBytes, InstructionData* instruction);