/*
distorm.c

diStorm3 C Library Interface
diStorm3 - Powerful disassembler for X86/AMD64
http://ragestorm.net/distorm/
distorm at gmail dot com
Copyright (C) 2003-2021 Gil Dabah
This library is licensed under the BSD license. See the file COPYING.
*/


#include "../include/distorm.h"
#include "config.h"
#include "decoder.h"
#include "x86defs.h"
#include "textdefs.h"
#include "wstring.h"
#include "../include/mnemonics.h"

/* C DLL EXPORTS */
#ifdef SUPPORT_64BIT_OFFSET
	_DLLEXPORT_ _DecodeResult distorm_decompose64(_CodeInfo* ci, _DInst result[], unsigned int maxInstructions, unsigned int* usedInstructionsCount)
#else
	_DLLEXPORT_ _DecodeResult distorm_decompose32(_CodeInfo* ci, _DInst result[], unsigned int maxInstructions, unsigned int* usedInstructionsCount)
#endif
{
	if (usedInstructionsCount == NULL) {
		return DECRES_SUCCESS;
	}

	if ((ci == NULL) ||
		(ci->codeLen < 0) ||
		((unsigned)ci->dt > (unsigned)Decode64Bits) ||
		(ci->code == NULL) ||
		(result == NULL) ||
		(maxInstructions == 0) ||
		((ci->features & (DF_MAXIMUM_ADDR16 | DF_MAXIMUM_ADDR32)) == (DF_MAXIMUM_ADDR16 | DF_MAXIMUM_ADDR32)))
	{
		return DECRES_INPUTERR;
	}

	return decode_internal(ci, FALSE, result, maxInstructions, usedInstructionsCount);
}

#ifndef DISTORM_LIGHT

/* Helper function to concatenate an explicit size when it's unknown from the operands. */
static void distorm_format_size(unsigned char** str, const _DInst* di, int opNum)
{
	int isSizingRequired = 0;
	/*
	 * We only have to output the size explicitly if it's not clear from the operands.
	 * For example:
	 * mov al, [0x1234] -> The size is 8, we know it from the AL register operand.
	 * mov [0x1234], 0x11 -> Now we don't know the size. Pam pam pam
	 *
	 * If given operand number is higher than 2, then output the size anyways.
	 */
	isSizingRequired = ((opNum >= 2) || ((opNum == 0) && (di->ops[0].type != O_REG) && (di->ops[1].type != O_REG)));

	/* Still not sure? Try some special instructions. */
	if (!isSizingRequired) {
		/*
		 * INS/OUTS are exception, because DX is a port specifier and not a real src/dst register.
		 * A few exceptions that always requires sizing:
		 * MOVZX, MOVSX, MOVSXD.
		 * ROL, ROR, RCL, RCR, SHL, SHR, SAL, SAR.
		 * SHLD, SHRD.
		 * CVTSI2SS is also an exception.
		 */
		switch (di->opcode)
		{
			case I_INS:
			case I_OUTS:
			case I_MOVZX:
			case I_MOVSX:
			case I_MOVSXD:
			case I_ROL:
			case I_ROR:
			case I_RCL:
			case I_RCR:
			case I_SHL:
			case I_SHR:
			case I_SAL:
			case I_SAR:
			case I_SHLD:
			case I_SHRD:
			case I_CVTSI2SS:
				isSizingRequired = 1;
			break;
			default: /* Instruction doesn't require sizing. */ break;
		}
	}

	if (isSizingRequired)
	{
		/*case 0: break; OT_MEM's unknown size. */
		switch (di->ops[opNum].size / 8)
		{
			case 1: strcat_WS(*str,  "BYTE    ", 8, 5); break;
			case 2: strcat_WS(*str,  "WORD    ", 8, 5); break;
			case 4: strcat_WS(*str,  "DWORD   ", 8, 6); break;
			case 8: strcat_WS(*str,  "QWORD   ", 8, 6); break;
			case 10: strcat_WS(*str, "TBYTE   ", 8, 6); break;
			case 16: strcat_WS(*str, "DQWORD  ", 8, 7); break;
			case 32: strcat_WS(*str, "YWORD   ", 8, 6); break;
		}
	}
}

static void distorm_format_signed_disp(unsigned char** str, const _DInst* di, uint64_t addrMask)
{
	int64_t tmpDisp64;

	if (di->dispSize) {
		if (((int64_t)di->disp < 0)) {
			chrcat_WS(*str, MINUS_DISP_CHR);
			tmpDisp64 = -(int64_t)di->disp;
			tmpDisp64 &= addrMask; /* Verify only for neg numbers. */
		}
		else {
			chrcat_WS(*str, PLUS_DISP_CHR);
			tmpDisp64 = di->disp;
		}
		str_int(str, tmpDisp64);
	}
}

static uint8_t prefixTable[6][8] = { "", "LOCK ", "REPNZ ", "REPNZ ", "REP ", "REPZ " };
static unsigned int prefixSizesTable[6] = { 0, 5, 6, 6, 4, 5 };
static uint8_t suffixTable[10] = { 0, 'B', 'W', 0, 'D', 0, 0, 0, 'Q' };

/* WARNING: This function is written carefully to be able to work with same input and output buffer in-place! */
#ifdef SUPPORT_64BIT_OFFSET
	_DLLEXPORT_ void distorm_format64(const _CodeInfo* ci, const _DInst* di, _DecodedInst* result)
#else
	_DLLEXPORT_ void distorm_format32(const _CodeInfo* ci, const _DInst* di, _DecodedInst* result)
#endif
{
	unsigned char* str;
	int64_t tmpDisp64;
	uint64_t addrMask = (uint64_t)-1;
	const _WMnemonic* mnemonic;
	int suffixSize = -1;
	unsigned int i;

	/* Set address mask, when default is for 64bits addresses. */
	if (ci->features & DF_USE_ADDR_MASK) addrMask = ci->addrMask;
	else {
		if (ci->features & DF_MAXIMUM_ADDR32) addrMask = 0xffffffff;
		else if (ci->features & DF_MAXIMUM_ADDR16) addrMask = 0xffff;
	}

	/* Gotta have full address for (di->addr - ci->codeOffset) to work in all modes. */
	str_hex(&result->instructionHex, (const uint8_t*)&ci->code[(unsigned int)(di->addr - ci->codeOffset)], di->size);

	if ((int)((int16_t)di->flags) == -1) {
		/* In-place considerations: DI is RESULT. Deref fields first. */
		unsigned int size = di->size;
		unsigned int byte = di->imm.byte;
		_OffsetType offset = di->addr & addrMask;

		result->offset = offset;
		result->size = size;
		str = (unsigned char*)&result->mnemonic.p;
		strcat_WS(str, "DB  ", 4, 3);
		str_int(&str, byte);
		strfinalize_WS(result->mnemonic, str);
		*(uint64_t*)&result->operands = 0; /* Clears length and the string at once. */
		return; /* Skip to next instruction. */
	}

	str = (unsigned char*)&result->operands.p;

	/* Special treatment for String (movs, cmps, stos, lods, scas) instructions. */
	if ((di->opcode >= I_MOVS) && (di->opcode <= I_SCAS)) {
		/*
		 * No operands are needed if the address size is the default one,
		 * and no segment is overridden, so add the suffix letter,
		 * to indicate size of operation and continue to next instruction.
		 */
		if ((SEGMENT_IS_DEFAULT_OR_NONE(di->segment)) && (FLAG_GET_ADDRSIZE(di->flags) == ci->dt)) {
			suffixSize = di->ops[0].size / 8;
			goto skipOperands;
		}
		suffixSize = 0; /* Marks it's a string instruction. */
	}

	for (i = 0; i < di->opsNo; i++) {
		unsigned int type = di->ops[i].type;
		if (i > 0) strcat_WS(str, ", ", 2, 2);
		if (type == O_REG) {
			strcat_WSR(&str, &_REGISTERS[di->ops[i].index]);
		}
		else if (type == O_IMM) {
			/* If the instruction is 'push', show explicit size (except byte imm). */
			if ((di->opcode == I_PUSH) && (di->ops[i].size != 8)) distorm_format_size(&str, di, i);
			/* Special fix for negative sign extended immediates. */
			if ((di->flags & FLAG_IMM_SIGNED) && (di->ops[i].size == 8) && (di->imm.sbyte < 0)) {
				chrcat_WS(str, MINUS_DISP_CHR);
				tmpDisp64 = -di->imm.sbyte;
				str_int(&str, tmpDisp64);
			}
			else {
				/* Notice signedness and size of the immediate. */
				if (di->ops[i].size == 0x20) str_int(&str, di->imm.dword);
				else str_int(&str, di->imm.qword);
			}
		}
		else if (type == O_PC) {
#ifdef SUPPORT_64BIT_OFFSET
			str_int(&str, (di->size + di->imm.sqword + di->addr) & addrMask);
#else
			tmpDisp64 = ((_OffsetType)di->imm.sdword + di->addr + di->size) & (uint32_t)addrMask;
			str_int(&str, tmpDisp64);
#endif
		}
		else if (type == O_DISP) {
			distorm_format_size(&str, di, i);
			chrcat_WS(str, OPEN_CHR);
			if (!SEGMENT_IS_DEFAULT_OR_NONE(di->segment)) {
				strcat_WSR(&str, &_REGISTERS[SEGMENT_GET_UNSAFE(di->segment)]);
				chrcat_WS(str, SEG_OFF_CHR);
			}
			tmpDisp64 = di->disp & addrMask;
			str_int(&str, tmpDisp64);
			chrcat_WS(str, CLOSE_CHR);
		}
		else if (type == O_SMEM) {
			int isDefault;
			int segment;
			distorm_format_size(&str, di, i);
			chrcat_WS(str, OPEN_CHR);

			segment = SEGMENT_GET(di->segment);
			isDefault = SEGMENT_IS_DEFAULT(di->segment);

			/*
			 * This is where we need to take special care for String instructions.
			 * If we got here, it means we need to explicitly show their operands.
			 * The problem with CMPS and MOVS is that they have two(!) memory operands.
			 * So we have to complement(!) them ourselves, since the isntruction structure supplies only the segment that can be overridden.
			 * And make the rest of the String operations explicit.
			 * We ignore default ES/DS in 64 bits.
			 * ["MOVS"], [OPT.REGI_EDI, OPT.REGI_ESI] -- DS can be overridden.
			 * ["CMPS"], [OPT.REGI_ESI, OPT.REGI_EDI] -- DS can be overriden.
			 *
			 * suffixSize == 0 was set above for string opcode already.
			 */
			if (suffixSize == 0) {
				if (((di->opcode == I_MOVS) && (i == 0)) || ((di->opcode == I_CMPS) && (i == 1))) {
					if (ci->dt != Decode64Bits) {
						segment = R_ES;
						isDefault = FALSE;
					}
					else isDefault = TRUE;
				}
				else if (isDefault && ((di->opcode == I_MOVS) || (di->opcode == I_CMPS))) {
					if (ci->dt != Decode64Bits) {
						segment = R_DS;
						isDefault = FALSE;
					}
				}
			}
			if (!isDefault && (segment != R_NONE)) {
				strcat_WSR(&str, &_REGISTERS[segment]);
				chrcat_WS(str, SEG_OFF_CHR);
			}

			strcat_WSR(&str, &_REGISTERS[di->ops[i].index]);

			distorm_format_signed_disp(&str, di, addrMask);
			chrcat_WS(str, CLOSE_CHR);
		}
		else if (type == O_MEM) {
			distorm_format_size(&str, di, i);
			chrcat_WS(str, OPEN_CHR);
			if (!SEGMENT_IS_DEFAULT_OR_NONE(di->segment)) {
				strcat_WSR(&str, &_REGISTERS[SEGMENT_GET_UNSAFE(di->segment)]);
				chrcat_WS(str, SEG_OFF_CHR);
			}
			if (di->base != R_NONE) {
				strcat_WSR(&str, &_REGISTERS[di->base]);
				chrcat_WS(str, PLUS_DISP_CHR);
			}
			strcat_WSR(&str, &_REGISTERS[di->ops[i].index]);
			if (di->scale != 0) {
				switch (di->scale)
				{
					case 2: strcat_WS(str, "*2", 2, 2); break;
					case 4: strcat_WS(str, "*4", 2, 2); break;
					case 8: strcat_WS(str, "*8", 2, 2); break;
				}
			}
			distorm_format_signed_disp(&str, di, addrMask);
			chrcat_WS(str, CLOSE_CHR);
		}
		else if (type == O_PTR) {
			str_int(&str, di->imm.ptr.seg);
			chrcat_WS(str, SEG_OFF_CHR);
			str_int(&str, di->imm.ptr.off);
		}
		else if (type == O_IMM1) {
			str_int(&str, di->imm.ex.i1);
		}
		else if (type == O_IMM2) {
			str_int(&str, di->imm.ex.i2);
		}
	}

skipOperands:

	/* Finalize the operands string. */
	strfinalize_WS(result->operands, str);

	/* Not used anymore.
	if (di->flags & FLAG_HINT_TAKEN) strcat_WSN(str, " ;TAKEN");
	else if (di->flags & FLAG_HINT_NOT_TAKEN) strcat_WSN(str, " ;NOT TAKEN");
	*/
	{
		/* In-place considerations: DI is RESULT. Deref fields first. */
		unsigned int opcode = di->opcode;
		unsigned int prefix = FLAG_GET_PREFIX(di->flags);
		unsigned int size = di->size;
		_OffsetType offset = di->addr & addrMask;
		str = (unsigned char*)&result->mnemonic.p;
		mnemonic = (const _WMnemonic*)&_MNEMONICS[opcode];

		if (prefix) {
			/* REP prefix for CMPS and SCAS is really a REPZ. */
			prefix += (opcode == I_CMPS);
			prefix += (opcode == I_SCAS);
			memcpy(str, &prefixTable[prefix][0], 8);
			str += prefixSizesTable[prefix];
		}

		/*
		 * Always copy 16 bytes from the mnemonic, we have a sentinel padding so we can read past.
		 * This helps the compiler to remove the call to memcpy and therefore makes this copying much faster.
		 * The longest instruction is exactly 16 chars long, so we null terminate the string below.
		 */
		memcpy((int8_t*)str, mnemonic->p, 16);
		str += mnemonic->length;

		if (suffixSize > 0) {
			*str++ = suffixTable[suffixSize];
		}
		strfinalize_WS(result->mnemonic, str);

		result->offset = offset;
		result->size = size;
	}
}

#ifdef SUPPORT_64BIT_OFFSET
	_DLLEXPORT_ _DecodeResult distorm_decode64(_OffsetType codeOffset, const unsigned char* code, int codeLen, _DecodeType dt, _DecodedInst result[], unsigned int maxInstructions, unsigned int* usedInstructionsCount)
#else
	_DLLEXPORT_ _DecodeResult distorm_decode32(_OffsetType codeOffset, const unsigned char* code, int codeLen, _DecodeType dt, _DecodedInst result[], unsigned int maxInstructions, unsigned int* usedInstructionsCount)
#endif
{
	_DecodeResult res;
	_CodeInfo ci;
	unsigned int i, instsCount;

	*usedInstructionsCount = 0;

	/* I use codeLen as a signed variable in order to ease detection of underflow... and besides - */
	if (codeLen < 0) {
		return DECRES_INPUTERR;
	}

	if ((unsigned)dt > (unsigned)Decode64Bits) {
		return DECRES_INPUTERR;
	}

	/* Make sure there's at least one instruction in the result buffer. */
	if ((code == NULL) || (result == NULL) || (maxInstructions == 0)) {
		return DECRES_INPUTERR;
	}

	/*
	 * We have to format the result into text. But the interal decoder works with the new structure of _DInst.
	 * Therefore, we will pass the result array(!) from the caller and the interal decoder will fill it in with _DInst's.
	 * Then we will copy each result to a temporary structure, and use it to reformat that specific result.
	 *
	 * This is all done to save memory allocation and to work on the same result array in-place!!!
	 * It's a bit ugly, I have to admit, but worth it.
	 */

	ci.codeOffset = codeOffset;
	ci.code = code;
	ci.codeLen = codeLen;
	ci.dt = dt;
	ci.features = DF_USE_ADDR_MASK;
	if (dt == Decode16Bits) ci.addrMask = 0xffff;
	else if (dt == Decode32Bits) ci.addrMask = 0xffffffff;
	else ci.addrMask = (_OffsetType)-1;

	res = decode_internal(&ci, TRUE, (_DInst*)result, maxInstructions, usedInstructionsCount);
	instsCount = *usedInstructionsCount;
	for (i = 0; i < instsCount; i++) {
		/* distorm_format is optimized and can work with same input/output buffer in-place. */
#ifdef SUPPORT_64BIT_OFFSET
		distorm_format64(&ci, (_DInst*)&result[i], &result[i]);
#else
		distorm_format32(&ci, (_DInst*)&result[i], &result[i]);
#endif
	}

	return res;
}

#endif /* DISTORM_LIGHT */

_DLLEXPORT_ unsigned int distorm_version(void)
{
	return __DISTORMV__;
}
