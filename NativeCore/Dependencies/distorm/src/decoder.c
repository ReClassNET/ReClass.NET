/*
decoder.c

diStorm3 - Powerful disassembler for X86/AMD64
http://ragestorm.net/distorm/
distorm at gmail dot com
Copyright (C) 2003-2021 Gil Dabah
This library is licensed under the BSD license. See the file COPYING.
*/


#include "decoder.h"
#include "instructions.h"
#include "insts.h"
#include "prefix.h"
#include "x86defs.h"
#include "operands.h"
#include "insts.h"
#include "../include/mnemonics.h"


/* Instruction Prefixes - Opcode - ModR/M - SIB - Displacement - Immediate */

static _DecodeType decode_get_effective_addr_size(_DecodeType dt, _iflags decodedPrefixes)
{
	/*
	 * Map from the current decoding mode to an effective address size:
	 * Decode16 -> Decode32
	 * Decode32 -> Decode16
	 * Decode64 -> Decode32
	 */

	/* Switch to non default mode if prefix exists, only for ADDRESS SIZE. */
	if (decodedPrefixes & INST_PRE_ADDR_SIZE) {
		if (dt == Decode32Bits) return Decode16Bits;
		return Decode32Bits;
	}
	return dt;
}

static _DecodeType decode_get_effective_op_size(_DecodeType dt, _iflags decodedPrefixes, unsigned int rex, _iflags instFlags)
{
	/*
	 * Map from the current decoding mode to an effective operand size:
	 * Decode16 -> Decode32
	 * Decode32 -> Decode16
	 * Decode64 -> Decode16
	 * Not that in 64bits it's a bit more complicated, because of REX and promoted instructions.
	 */

	if (decodedPrefixes & INST_PRE_OP_SIZE) {
		if (dt == Decode16Bits) return Decode32Bits;
		return Decode16Bits;
	}

	if (dt == Decode64Bits) {
		/*
		 * REX Prefix toggles data size to 64 bits.
		 * Operand size prefix toggles data size to 16.
		 * Default data size is 32 bits.
		 * Promoted instructions are 64 bits if they don't require a REX perfix.
		 * Non promoted instructions are 64 bits if the REX prefix exists.
		 */
		/* Automatically promoted instructions have only INST_64BITS SET! */
		if (((instFlags & (INST_64BITS | INST_PRE_REX)) == INST_64BITS) ||
		/* Other instructions in 64 bits can be promoted only with a REX prefix. */
			((decodedPrefixes & INST_PRE_REX) && (rex & PREFIX_EX_W))) return Decode64Bits;
		return Decode32Bits; /* Default. */
	}

	return dt;
}

/*
 * A helper macro to convert from diStorm's CPU flags to EFLAGS.
 * Copy eflags from compact version (8 bits) to eflags compatible (16 bits).
 * From D_COMPACT_IF to D_IF, bit index 1 to 9.
 * From D_COMPACT_DF to D_DF, bit index 3 to 10.
 * From D_COMPACT_OF to D_OF, bit index 5 to 11.
 */
#define CONVERT_FLAGS_TO_EFLAGS(dst, src, field) dst->field = ((src->field & D_COMPACT_SAME_FLAGS) | \
	((src->field & D_COMPACT_IF) << (9 - 1)) | \
	((src->field & D_COMPACT_DF) << (10 - 3)) | \
	((src->field & D_COMPACT_OF) << (11 - 5)));

/* If DECRES_SUCCESS is returned, CI is in sync, otherwise it loses sync. */
/* Important note: CI is keeping track only for code and codeLen, in case of a failure caller has to restart on their own. */
static _DecodeResult decode_inst(_CodeInfo* ci, _PrefixState* ps, const uint8_t* startCode, _DInst* di)
{
	/* Holds the info about the current found instruction. */
	_InstInfo* ii;
	_InstSharedInfo* isi;

	/* Calculate (and cache) effective-operand-size and effective-address-size only once. */
	_DecodeType effOpSz, effAdrSz;
	_iflags instFlags;

	/* The ModR/M byte of the current instruction. */
	unsigned int modrm = 0;
	int isPrefixed = 0;

	ii = inst_lookup(ci, ps, &isPrefixed);
	if (ii == NULL) goto _Undecodable;

	isi = &InstSharedInfoTable[ii->sharedIndex];
	instFlags = FlagsTable[isi->flagsIndex];

	/* Cache the effective operand-size and address-size. */
	if (isPrefixed) {

		/*
		* If both REX and OpSize are available we will have to disable the OpSize, because REX has precedence.
		* However, only if REX.W is set!
		* We had to wait with this test, since the operand size may be a mandatory prefix,
		* and we know it only after fetching opcode.
		*/
		if ((ps->decodedPrefixes & INST_PRE_OP_SIZE) &&
			(ps->prefixExtType == PET_REX) &&
			(ps->vrex & PREFIX_EX_W) &&
			(!ps->isOpSizeMandatory)) {
			ps->decodedPrefixes &= ~INST_PRE_OP_SIZE;
			prefixes_ignore(ps, PFXIDX_OP_SIZE);
		}

		effAdrSz = decode_get_effective_addr_size(ci->dt, ps->decodedPrefixes);
		effOpSz = decode_get_effective_op_size(ci->dt, ps->decodedPrefixes, ps->vrex, instFlags);
	}
	else
	{
		effAdrSz = ci->dt; /* Default is current decoding type since there's no prefix. */
		effOpSz = decode_get_effective_op_size(ci->dt, 0, 0, instFlags);
	}

	/*
	 * In this point we know the instruction we are about to decode and its operands (unless, it's an invalid one!),
	 * so it makes it the right time for decoding-type suitability testing.
	 * Which practically means, don't allow 32 bits instructions in 16 bits decoding mode, but do allow
	 * 16 bits instructions in 32 bits decoding mode, of course...

	 * NOTE: Make sure the instruction set for 32 bits has explicitly this specific flag set.
	 * NOTE2: Make sure the instruction set for 64 bits has explicitly this specific flag set.

	 * If this is the case, drop what we've got and restart all over after DB'ing that byte.

	 * Though, don't drop an instruction which is also supported in 16 and 32 bits.
	 */

	 /* ! ! ! DISABLED UNTIL FURTHER NOTICE ! ! ! Decode16Bits CAN NOW DECODE 32 BITS INSTRUCTIONS ! ! !*/
	 /* if (ii && (dt == Decode16Bits) && (instFlags & INST_32BITS) && (~instFlags & INST_16BITS)) ii = NULL; */

	memset(di, 0, sizeof(_DInst));

	if (instFlags & INST_MODRM_REQUIRED) {
		/* If the ModRM byte is not part of the opcode, skip the last byte code, so code points now to ModRM. */
		if (!(instFlags & INST_MODRM_INCLUDED)) {
			ci->code++;
			if (--ci->codeLen < 0) goto _Undecodable;
		}
		modrm = *ci->code;
	}

	ci->code++; /* Skip the last byte we just read (either last opcode's byte code or a ModRM). */

	di->addr = ci->codeOffset & ci->addrMask;
	di->opcode = ii->opcodeId;
	di->flags = isi->meta & META_INST_PRIVILEGED;

	/*
	 * Store the address size inside the flags.
	 * This is necessary for the caller to know the size of rSP when using PUSHA for example.
	 */
	di->base = R_NONE;
	di->segment = R_NONE;

	FLAG_SET_ADDRSIZE(di, effAdrSz);

	/* Try to extract the next operand only if the latter exists. */
	if (isi->d != OT_NONE) {
		unsigned int opsNo = 1;
		_Operand* op = &di->ops[0];
		if (instFlags & (INST_MODRR_REQUIRED | INST_FORCE_REG0)) {
			/* Some instructions enforce that mod=11, so validate that. */
			if ((modrm < INST_DIVIDED_MODRM) && (instFlags & INST_MODRR_REQUIRED)) goto _Undecodable;
			/* Some instructions enforce that reg=000, so validate that. (Specifically EXTRQ). */
			if ((instFlags & INST_FORCE_REG0) && (((modrm >> 3) & 7) != 0)) goto _Undecodable;
		}
		if (!operands_extract(ci, di, ii, instFlags, (_OpType)isi->d, modrm, ps, effOpSz, effAdrSz, op++)) goto _Undecodable;

		if (isi->s != OT_NONE) {
			if (!operands_extract(ci, di, ii, instFlags, (_OpType)isi->s, modrm, ps, effOpSz, effAdrSz, op++)) goto _Undecodable;
			opsNo++;
			/* Use third operand, only if the flags says this InstInfo requires it. */
			if (instFlags & INST_USE_OP3) {
				if (!operands_extract(ci, di, ii, instFlags, (_OpType)((_InstInfoEx*)ii)->op3, modrm, ps, effOpSz, effAdrSz, op++)) goto _Undecodable;
				opsNo++;
				/* Support for a fourth operand is added for (e.g:) INSERTQ instruction. */
				if (instFlags & INST_USE_OP4) {
					if (!operands_extract(ci, di, ii, instFlags, (_OpType)((_InstInfoEx*)ii)->op4, modrm, ps, effOpSz, effAdrSz, op++)) goto _Undecodable;
					opsNo++;
				}
			}
		}

		/* Copy DST_WR flag. */
		di->flags |= (instFlags & INST_DST_WR) >> (31 - 6); /* Copy bit from INST_DST_WR (bit 31) to FLAG_DST_WR (bit 6). */
		/* operands_extract may touched it for FPU operands, so add on top. */
		di->opsNo += (uint8_t)opsNo;
	}

	if (instFlags & (INST_3DNOW_FETCH |
		INST_PSEUDO_OPCODE |
		INST_NATIVE |
		INST_PRE_REPNZ |
		INST_PRE_REP |
		INST_PRE_ADDR_SIZE |
		INST_INVALID_64BITS |
		INST_64BITS_FETCH)) { /* 8 for 1! */

		/* If it's a native instruction copy OpSize Prefix. */
		if (ps && instFlags & INST_NATIVE) ps->usedPrefixes |= (ps->decodedPrefixes & INST_PRE_OP_SIZE);

		if (ci->dt != Decode64Bits) {
			/* If it's only a 64 bits instruction drop it in other decoding modes. */
			if (instFlags & INST_64BITS_FETCH) goto _Undecodable;
		}
		else {
			/* Drop instructions which are invalid in 64 bits. */
			if (instFlags & INST_INVALID_64BITS) goto _Undecodable;
		}

		/* If it were a 3DNow! instruction, we will have to find the instruction itself now that we got its operands extracted. */
		if (instFlags & INST_3DNOW_FETCH) {
			ii = inst_lookup_3dnow(ci);
			if (ii == NULL) goto _Undecodable;
			isi = &InstSharedInfoTable[ii->sharedIndex];
			instFlags = FlagsTable[isi->flagsIndex];
			di->opcode = ii->opcodeId;
		}

		/* Check whether pseudo opcode is needed, only for CMP instructions: */
		if (instFlags & INST_PSEUDO_OPCODE) {
			/* Used only for special CMP instructions which have pseudo opcodes suffix. */
			unsigned int cmpType;

			if (--ci->codeLen < 0) goto _Undecodable;
			cmpType = *ci->code;
			ci->code++;

			/*
			 * The opcodeId is the offset to the FIRST pseudo compare mnemonic,
			 * we will have to fix it so it offsets into the corrected mnemonic.
			 * Therefore, we use another table to fix the offset.
			 */
			if (instFlags & INST_PRE_VEX) {
				/* AVX Comparison type must be between 0 to 32, otherwise Reserved. */
				if (cmpType >= INST_VCMP_MAX_RANGE) goto _Undecodable;

				/* Use the AVX pseudo compare mnemonics table. */
				di->opcode = ii->opcodeId + VCmpMnemonicOffsets[cmpType];
			}
			else {
				/* SSE Comparison type must be between 0 to 8, otherwise Reserved. */
				if (cmpType >= INST_CMP_MAX_RANGE) goto _Undecodable;
				di->opcode = ii->opcodeId + CmpMnemonicOffsets[cmpType];
			}

			goto _SkipOpcoding;
		}

		/* Start with prefix REP/N/Z. */
		if (isPrefixed && (instFlags & (INST_PRE_REPNZ | INST_PRE_REP))) {
			if ((instFlags & INST_PRE_REPNZ) && (ps->decodedPrefixes & INST_PRE_REPNZ)) {
				ps->usedPrefixes |= INST_PRE_REPNZ;
				di->flags |= FLAG_REPNZ;
			}
			else if ((instFlags & INST_PRE_REP) && (ps->decodedPrefixes & INST_PRE_REP)) {
				ps->usedPrefixes |= INST_PRE_REP;
				di->flags |= FLAG_REP;
			}
		}

		if (instFlags & INST_PRE_ADDR_SIZE) {
			/* If it's JeCXZ the ADDR_SIZE prefix affects them. */
			if (instFlags & INST_USE_EXMNEMONIC) {
				ps->usedPrefixes |= INST_PRE_ADDR_SIZE;
				if (effAdrSz == Decode16Bits) di->opcode = ii->opcodeId;
				else if (effAdrSz == Decode32Bits) di->opcode = ((_InstInfoEx*)ii)->opcodeId2;
				/* Ignore REX.W in 64bits, JECXZ is promoted. */
				else /* Decode64Bits */ di->opcode = ((_InstInfoEx*)ii)->opcodeId3;
			}

			/* LOOPxx instructions are also native instruction, but they are special case ones, ADDR_SIZE prefix affects them. */
			else if (instFlags & INST_NATIVE) {
				di->opcode = ii->opcodeId;

				/* If LOOPxx gets here from 64bits, it must be Decode32Bits because Address Size prefix is set. */
				ps->usedPrefixes |= INST_PRE_ADDR_SIZE;
			}

			goto _SkipOpcoding;
		}
	}

	/*
	 * If we reached here the instruction was fully decoded, we located the instruction in the DB and extracted operands.
	 * Use the correct mnemonic according to the DT.
	 * If we are in 32 bits decoding mode it doesn't necessarily mean we will choose mnemonic2, alas,
	 * it means that if there is a mnemonic2, it will be used.
	 * Note:
	 * If the instruction is prefixed by operand size we will format it in the non-default decoding mode!
	 * So there might be a situation that an instruction of 32 bit gets formatted in 16 bits decoding mode.
	 * Both ways should end up with a correct and expected formatting of the text.
	 */
	if (effOpSz == Decode32Bits) { /* Decode32Bits */

		/* Set operand size. */
		FLAG_SET_OPSIZE(di, Decode32Bits);

		/* Give a chance for special mnemonic instruction in 32 bits decoding. */
		if (instFlags & INST_USE_EXMNEMONIC) {
			/* Is it a special instruction which has another mnemonic for mod=11 ? */
			if (instFlags & INST_MNEMONIC_MODRM_BASED) {
				if (modrm < INST_DIVIDED_MODRM) di->opcode = ((_InstInfoEx*)ii)->opcodeId2;
			}
			else di->opcode = ((_InstInfoEx*)ii)->opcodeId2;
			ps->usedPrefixes |= INST_PRE_OP_SIZE;
		}
	}
	else if (effOpSz == Decode64Bits) { /* Decode64Bits, note that some instructions might be decoded in Decode32Bits above. */

		/* Set operand size. */
		FLAG_SET_OPSIZE(di, Decode64Bits);

		if (instFlags & (INST_USE_EXMNEMONIC | INST_USE_EXMNEMONIC2)) {
			/*
			 * We shouldn't be here for MODRM based mnemonics with a MOD=11,
			 * because they must not use REX (otherwise it will get to the wrong instruction which share same opcode).
			 * See XRSTOR and XSAVEOPT.
			 */
			if ((modrm >= INST_DIVIDED_MODRM) && (instFlags & INST_MNEMONIC_MODRM_BASED)) goto _Undecodable;

			/* Use third mnemonic, for 64 bits. */
			if ((instFlags & INST_USE_EXMNEMONIC2) && (ps->vrex & PREFIX_EX_W)) {
				ps->usedPrefixes |= INST_PRE_REX;
				di->opcode = ((_InstInfoEx*)ii)->opcodeId3;
			}
			else di->opcode = ((_InstInfoEx*)ii)->opcodeId2; /* Use second mnemonic. */
		}
	}
	else { /* Decode16Bits */

		/* Set operand size. */
		FLAG_SET_OPSIZE(di, Decode16Bits);

		/*
		 * If it's a special instruction which has two mnemonics, then use the 16 bits one + update usedPrefixes.
		 * Note: use 16 bits mnemonic if that instruction supports 32 bit or 64 bit explicitly.
		 */
		if ((instFlags & (INST_USE_EXMNEMONIC | INST_32BITS | INST_64BITS)) == INST_USE_EXMNEMONIC) ps->usedPrefixes |= INST_PRE_OP_SIZE;
	}

_SkipOpcoding:

	/* Check VEX mnemonics: */
	if (isPrefixed && (instFlags & INST_PRE_VEX) &&
		(((((_InstInfoEx*)ii)->flagsEx & INST_MNEMONIC_VEXW_BASED) && (ps->vrex & PREFIX_EX_W)) ||
			((((_InstInfoEx*)ii)->flagsEx & INST_MNEMONIC_VEXL_BASED) && (ps->vrex & PREFIX_EX_L)))) {
		di->opcode = ((_InstInfoEx*)ii)->opcodeId2;
	}

	/* Instruction's size should include prefixes too if exist. */
	di->size = (uint8_t)(ci->code - startCode);
	/*
	 * There's a limit of 15 bytes on instruction length. The only way to violate
	 * this limit is by putting redundant prefixes before an instruction.
	 * start points to first prefix if any, otherwise it points to instruction first byte.
	 */
	if (di->size > INST_MAXIMUM_SIZE) goto _Undecodable;

	/* Set the unused prefixes mask, if any prefixes (not) used at all. */
	if (isPrefixed) di->unusedPrefixesMask = prefixes_set_unused_mask(ps);

	/* Copy instruction meta. */
	di->meta = isi->meta;

	if (ci->features & DF_FILL_EFLAGS) {
		/* Copy CPU affected flags. */
		if (isi->testedFlagsMask) CONVERT_FLAGS_TO_EFLAGS(di, isi, testedFlagsMask);
		if (isi->modifiedFlagsMask) CONVERT_FLAGS_TO_EFLAGS(di, isi, modifiedFlagsMask);
		if (isi->undefinedFlagsMask) CONVERT_FLAGS_TO_EFLAGS(di, isi, undefinedFlagsMask);
	}

	/*
	 * Instruction can still be invalid if it's total length is over 15 bytes with prefixes.
	 * Up to the caller to check that.
	 */
	return DECRES_SUCCESS;

_Undecodable: /* If the instruction couldn't be decoded for some reason, fail. */
	/* Special case for WAIT instruction: If it's dropped as a prefix, we have to return a valid instruction! */
	if (*startCode == INST_WAIT_INDEX) {
		int delta;
		memset(di, 0, sizeof(_DInst));
		di->addr = ci->codeOffset & ci->addrMask;
		di->imm.byte = INST_WAIT_INDEX;
		di->segment = R_NONE;
		di->base = R_NONE;
		di->size = 1;
		di->opcode = I_WAIT;
		META_SET_ISC(di, ISC_INTEGER);

		/* Fix ci because WAIT could be a prefix that failed, and ci->code is now out of sync. */
		delta = (int)(ci->code - startCode); /* How many bytes we read so far. */
		ci->codeLen += delta - 1;
		ci->code = startCode + 1;
		/* codeOffset is fixed outside. */

		return DECRES_SUCCESS;
	}

	/* Mark that we didn't manage to decode the instruction well, caller will drop it. */
	return DECRES_INPUTERR;
}

/*
 * decode_internal
 *
 * supportOldIntr - Since now we work with new structure instead of the old _DecodedInst, we are still interested in backward compatibility.
 *                  So although, the array is now of type _DInst, we want to read it in jumps of the old array element's size.
 *                  This is in order to save memory allocation for conversion between the new and the old structures.
 *                  It really means we can do the conversion in-place now.
 */
_DecodeResult decode_internal(_CodeInfo* _ci, int supportOldIntr, _DInst result[], unsigned int maxResultCount, unsigned int* usedInstructionsCount)
{
	_CodeInfo ci = *_ci; /* A working copy, we don't touch user's _ci except OUT params. */
	_PrefixState ps;
	/* Bookkeep these from ci below, as it makes things way simpler. */
	const uint8_t* code;
	int codeLen;
	_OffsetType codeOffset;

	_DecodeResult ret = DECRES_SUCCESS;

	/* Current working decoded instruction in results. */
	_DInst* pdi = (_DInst*)&result[0]; /* There's always a room for at least one slot, checked earlier. */
	_DInst* maxResultAddr;

	unsigned int features = ci.features;

	unsigned int diStructSize;
	/* Use next entry. */
#ifndef DISTORM_LIGHT
	if (supportOldIntr) {
		diStructSize = sizeof(_DecodedInst);
		maxResultAddr = (_DInst*)((size_t)&result[0] + (maxResultCount * sizeof(_DecodedInst)));
	}
	else
#endif /* DISTORM_LIGHT */
	{
		diStructSize = sizeof(_DInst);
		maxResultAddr = &result[maxResultCount];
	}

	ci.addrMask = (_OffsetType)-1;

#ifdef DISTORM_LIGHT
	supportOldIntr; /* Unreferenced. */

	/*
	 * Only truncate address if we are using the decompose interface.
	 * Otherwise, we use the textual interface which needs full addresses for formatting bytes output.
	 * So distorm_format will truncate later.
	 */
	if (features & DF_MAXIMUM_ADDR32) ci.addrMask = 0xffffffff;
	else if (features & DF_MAXIMUM_ADDR16) ci.addrMask = 0xffff;
#endif

	ps.count = 1; /* Force zero'ing ps below. */

	/* Decode instructions as long as we have what to decode/enough room in entries. */
	while (ci.codeLen > 0) {
		code = ci.code;
		codeLen = ci.codeLen;
		codeOffset = ci.codeOffset;

		if (ps.count) memset(&ps, 0, sizeof(ps));

		/**** INSTRUCTION DECODING NEXT: ****/

		/* Make sure we didn't run out of output entries. */
		if (pdi >= maxResultAddr) {
			ret = DECRES_MEMORYERR;
			break;
		}

		ret = decode_inst(&ci, &ps, code, pdi);
		/* decode_inst keeps track (only if successful!) for code and codeLen but ignores codeOffset, fix it here. */
		ci.codeOffset += pdi->size;

		if (ret == DECRES_SUCCESS) {

			if (features & (DF_SINGLE_BYTE_STEP | DF_RETURN_FC_ONLY | DF_STOP_ON_PRIVILEGED | DF_STOP_ON_FLOW_CONTROL)) {

				/* Sync codeinfo, remember that currently it points to beginning of the instruction and prefixes if any. */
				if (features & DF_SINGLE_BYTE_STEP) {
					ci.code = code + 1;
					ci.codeLen = codeLen - 1;
					ci.codeOffset = codeOffset + 1;
				}

				/* See if we need to filter this instruction. */
				if ((features & DF_RETURN_FC_ONLY) && (META_GET_FC(pdi->meta) == FC_NONE)) {
					continue;
				}

				/* Check whether we need to stop on any feature. */
				if ((features & DF_STOP_ON_PRIVILEGED) && (FLAG_GET_PRIVILEGED(pdi->flags))) {
					pdi = (_DInst*)((char*)pdi + diStructSize);
					break; /* ret = DECRES_SUCCESS; */
				}

				if (features & DF_STOP_ON_FLOW_CONTROL) {
					unsigned int mfc = META_GET_FC(pdi->meta);
					if (mfc && (((features & DF_STOP_ON_CALL) && (mfc == FC_CALL)) ||
						((features & DF_STOP_ON_RET) && (mfc == FC_RET)) ||
						((features & DF_STOP_ON_SYS) && (mfc == FC_SYS)) ||
						((features & DF_STOP_ON_UNC_BRANCH) && (mfc == FC_UNC_BRANCH)) ||
						((features & DF_STOP_ON_CND_BRANCH) && (mfc == FC_CND_BRANCH)) ||
						((features & DF_STOP_ON_INT) && (mfc == FC_INT)) ||
						((features & DF_STOP_ON_CMOV) && (mfc == FC_CMOV)) ||
						((features & DF_STOP_ON_HLT) && (mfc == FC_HLT)))) {
						pdi = (_DInst*)((char*)pdi + diStructSize);
						break; /* ret = DECRES_SUCCESS; */
					}
				}
			}

			/* Allocate at least one more entry to use, for the next instruction. */
			pdi = (_DInst*)((char*)pdi + diStructSize);
		}
		else { /* ret == DECRES_INPUTERR */

			/* Handle failure of decoding last instruction. */
			if ((!(features & DF_RETURN_FC_ONLY))) {
				memset(pdi, 0, sizeof(_DInst));
				pdi->flags = FLAG_NOT_DECODABLE;
				pdi->imm.byte = *code;
				pdi->size = 1;
				pdi->addr = codeOffset & ci.addrMask;
				pdi = (_DInst*)((char*)pdi + diStructSize);

				/* If an instruction wasn't decoded then stop on undecodeable if set. */
				if (features & DF_STOP_ON_UNDECODEABLE) {
					ret = DECRES_SUCCESS;
					break;
				}
			}

			/* Skip a single byte in case of a failure and retry instruction. */
			ci.code = code + 1;
			ci.codeLen = codeLen - 1;
			ci.codeOffset = codeOffset + 1;

			/* Reset return value. */
			ret = DECRES_SUCCESS;
		}
	}

	/* Set OUT params. */
	*usedInstructionsCount = (unsigned int)(((size_t)pdi - (size_t)result) / (size_t)diStructSize);
	_ci->nextOffset = ci.codeOffset;

	return ret;
}
