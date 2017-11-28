#pragma once

#include "../ReClassNET_Plugin.hpp"

typedef bool(RC_CallConv EnumerateInstructionCallback)(InstructionData* data);

bool DisassembleInstructionsImpl(const RC_Pointer address, const RC_Size length, const RC_Pointer virtualAddress, const bool determineStaticInstructionBytes, EnumerateInstructionCallback callback);