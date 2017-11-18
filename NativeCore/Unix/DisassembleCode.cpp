#include <vector>
#include <cstdint>
#include <distorm.h>

#include "NativeCore.hpp"

extern "C" bool DisassembleCode(RC_Pointer address, RC_Size length, RC_Pointer virtualAddress, InstructionData* instruction)
{
	_CodeInfo info;
	info.codeOffset = reinterpret_cast<_OffsetType>(virtualAddress);
	info.code = reinterpret_cast<const uint8_t*>(address);
	info.codeLen = length;
	info.features = DF_NONE;

#ifdef RECLASSNET32
	info.dt = Decode32Bits;
#else
	info.dt = Decode64Bits;
#endif

	_DInst decodedInstructions[1];
	unsigned int instructionCount = 0;

	const auto res = distorm_decompose(&info, decodedInstructions, 1, &instructionCount);
	if (res == DECRES_INPUTERR || !(res == DECRES_SUCCESS || res == DECRES_MEMORYERR) || instructionCount == 1)
	{
		return false;
	}

	_DecodedInst instructionInfo;
	distorm_format(&info, &decodedInstructions[0], &instructionInfo);

	instruction->Length = instructionInfo.size;
	std::memcpy(instruction->Data, address, instructionInfo.size);

	MultiByteToUnicode(
		reinterpret_cast<const char*>(instructionInfo.mnemonic.p),
		instruction->Instruction,
		instructionInfo.mnemonic.length
	);
	if (instructionInfo.operands.length != 0)
	{
		instruction->Instruction[instructionInfo.mnemonic.length] = ' ';

		MultiByteToUnicode(
			reinterpret_cast<const char*>(instructionInfo.operands.p),
			0,
			instruction->Instruction,
			instructionInfo.mnemonic.length + 1,
			std::min<int>(64 - 1 - instructionInfo.mnemonic.length, instructionInfo.operands.length)
		);
	}

	return true;
}
