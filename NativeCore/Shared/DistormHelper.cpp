#include "DistormHelper.hpp"

#include <distorm.h>
#include <mnemonics.h>
extern "C"
{
#include <../src/instructions.h>
}

bool AreOperandsStatic(const _DInst &instruction, const int prefixLength)
{
	const auto fc = META_GET_FC(instruction.meta);
	if (fc == FC_UNC_BRANCH || fc == FC_CND_BRANCH)
	{
		if (instruction.size - prefixLength < 5)
		{
			return true;
		}
	}

	const auto ops = instruction.ops;
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
			if (instruction.dispSize < 32)
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

_CodeInfo CreateCodeInfo(const uint8_t* address, int length, const _OffsetType virtualAddress)
{
	_CodeInfo info = {};
	info.codeOffset = virtualAddress;
	info.code = address;
	info.codeLen = length;
	info.features = DF_NONE;

#ifdef RECLASSNET64
	info.dt = Decode64Bits;
#else
	info.dt = Decode32Bits;
#endif

	return info;
}


int GetStaticInstructionBytes(const _DInst &instruction, const uint8_t *data)
{
	auto info = CreateCodeInfo(data, instruction.size, reinterpret_cast<_OffsetType>(data));

	_PrefixState ps = {};
	int isPrefixed;
	inst_lookup(&info, &ps, &isPrefixed);

	if (AreOperandsStatic(instruction, ps.count))
	{
		return instruction.size;
	}

	return instruction.size - info.codeLen - ps.count;
}

void FillInstructionData(const _CodeInfo& info, const RC_Pointer address, const _DInst& instruction, const bool determineStaticInstructionBytes, InstructionData* data)
{
	data->Address = reinterpret_cast<RC_Pointer>(instruction.addr);
	data->Length = instruction.size;
	std::memcpy(data->Data, address, instruction.size);
	data->StaticInstructionBytes = -1;

	if (instruction.flags == FLAG_NOT_DECODABLE)
	{
		std::memcpy(data->Instruction, L"???", sizeof(RC_UnicodeChar) * 3);
	}
	else
	{
		_DecodedInst instructionInfo = {};
		distorm_format(&info, &instruction, &instructionInfo);
		
		MultiByteToUnicode(
			reinterpret_cast<const char*>(instructionInfo.mnemonic.p),
			data->Instruction,
			instructionInfo.mnemonic.length
		);
		if (instructionInfo.operands.length != 0)
		{
			data->Instruction[instructionInfo.mnemonic.length] = ' ';

			MultiByteToUnicode(
				reinterpret_cast<const char*>(instructionInfo.operands.p),
				0,
				data->Instruction,
				instructionInfo.mnemonic.length + 1,
				std::min<int>(64 - 1 - instructionInfo.mnemonic.length, instructionInfo.operands.length)
			);
		}

		if (determineStaticInstructionBytes)
		{
			data->StaticInstructionBytes = GetStaticInstructionBytes(
				instruction,
				reinterpret_cast<const uint8_t*>(address)
			);
		}
	}
}

bool DisassembleInstructionsImpl(const RC_Pointer address, const RC_Size length, const RC_Pointer virtualAddress, const bool determineStaticInstructionBytes, EnumerateInstructionCallback callback)
{
	auto info = CreateCodeInfo(static_cast<const uint8_t*>(address), static_cast<int>(length), reinterpret_cast<_OffsetType>(virtualAddress));

	const unsigned MaxInstructions = 50;

	_DInst decodedInstructions[MaxInstructions] = {};
	unsigned count = 0;

	auto instructionAddress = static_cast<uint8_t*>(address);

	while (true)
	{
		const auto res = distorm_decompose(&info, decodedInstructions, MaxInstructions, &count);
		if (res == DECRES_INPUTERR)
		{
			return false;
		}

		for (auto i = 0u; i < count; ++i)
		{
			const auto& instruction = decodedInstructions[i];

			InstructionData data = {};
			FillInstructionData(info, instructionAddress, instruction, determineStaticInstructionBytes, &data);

			if (callback(&data) == false)
			{
				return true;
			}

			instructionAddress += instruction.size;
		}

		if (res == DECRES_SUCCESS || count == 0)
		{
			return true;
		}

		const auto offset = static_cast<unsigned>(decodedInstructions[count - 1].addr + decodedInstructions[count - 1].size - info.codeOffset);

		info.codeOffset += offset;
		info.code += offset;
		info.codeLen -= offset;
	}
}
