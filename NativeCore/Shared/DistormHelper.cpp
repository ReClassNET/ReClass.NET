#include "DistormHelper.hpp"

#include <distorm.h>
#include <mnemonics.h>
extern "C"
{
#include <../src/instructions.h>
}

bool AreOperandsStatic(_DInst *instruction, const int prefixLength)
{
	const auto fc = META_GET_FC(instruction->meta);
	if (fc == FC_UNC_BRANCH || fc == FC_CND_BRANCH)
	{
		if (instruction->size - prefixLength < 5)
		{
			return true;
		}
	}

	const auto ops = instruction->ops;
	for (auto i = 0; i < OPERANDS_NO; i++)
	{
		switch (ops[i].type)
		{
		case O_NONE:
		case O_REG:
		case O_IMM1:
		case O_IMM2:
			continue;
		case O_IMM:
			if (ops[i].size < 32)
			{
				continue;
			}
			return false;
		case O_DISP:
		case O_SMEM:
		case O_MEM:
			if (instruction->dispSize < 32)
			{
				continue;
			}

#ifdef RECLASSNET64
			if (ops[i].index == R_RIP)
			{
				continue;
			}
#endif
			return false;
		case O_PC:
		case O_PTR:
			return false;
		}
	}

	return true;
}

int GetStaticInstructionBytes(_DInst *instruction, const uint8_t *data)
{
	_CodeInfo info = {};
	info.codeOffset = reinterpret_cast<_OffsetType>(data);
	info.code = data;
	info.codeLen = instruction->size;
	info.features = DF_NONE;
#ifdef RECLASSNET32
	info.dt = Decode32Bits;
#else
	info.dt = Decode64Bits;
#endif

	_PrefixState ps = {};
	memset(ps.pfxIndexer, PFXIDX_NONE, sizeof(int) * PFXIDX_MAX);
	ps.start = data;
	ps.last = data;

	prefixes_decode(data, info.codeLen, &ps, info.dt);

	info.codeOffset = reinterpret_cast<_OffsetType>(ps.last);
	info.code = ps.last;

	const auto prefixLength = static_cast<int>(ps.start - ps.last);
	info.codeLen -= prefixLength;

	inst_lookup(&info, &ps);

	if (AreOperandsStatic(instruction, prefixLength))
	{
		return instruction->size;
	}

	return instruction->size - info.codeLen;
}

bool DisassembleCodeImpl(const RC_Pointer address, const RC_Size length, const RC_Pointer virtualAddress, const bool determineStaticInstructionBytes, InstructionData* instruction)
{
	_CodeInfo info = {};
	info.codeOffset = reinterpret_cast<_OffsetType>(virtualAddress);
	info.code = reinterpret_cast<const uint8_t*>(address);
	info.codeLen = length;
	info.features = DF_NONE;

#ifdef RECLASSNET32
	info.dt = Decode32Bits;
#else
	info.dt = Decode64Bits;
#endif

	_DInst decodedInstructions[1] = {};
	unsigned int instructionCount = 0;

	const auto res = distorm_decompose(&info, decodedInstructions, 1, &instructionCount);
	if (res == DECRES_INPUTERR || !(res == DECRES_SUCCESS || res == DECRES_MEMORYERR) || instructionCount != 1)
	{
		return false;
	}

	_DecodedInst instructionInfo = {};
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

	if (determineStaticInstructionBytes)
	{
		instruction->StaticInstructionBytes = GetStaticInstructionBytes(
			&decodedInstructions[0],
			reinterpret_cast<const uint8_t*>(address)
		);
	}
	else
	{
		instruction->StaticInstructionBytes = -1;
	}

	return true;
}
