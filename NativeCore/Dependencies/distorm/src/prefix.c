/*
prefix.c

diStorm3 - Powerful disassembler for X86/AMD64
http://ragestorm.net/distorm/
distorm at gmail dot com
Copyright (C) 2003-2021 Gil Dabah
This library is licensed under the BSD license. See the file COPYING.
*/


#include "prefix.h"

#include "x86defs.h"
#include "instructions.h"
#include "../include/mnemonics.h"


/*
 * The main purpose of this module is to keep track of all kind of prefixes a single instruction may have.
 * The problem is that a single instruction may have up to six different prefix-types.
 * That's why I have to detect such cases and drop those excess prefixes.
 */


int PrefixTables[256 * 2] = {
	/* Decode 16/32 Bits */
	0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
	0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
	0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, /* ES (0x26) CS (0x2e) */
	0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, /* DS (0x3e) SS (0x36) */
	0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
	0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
	0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, /* FS(0x64) GS(0x65) OP_SIZE(0x66) ADDR_SIZE(0x67) */
	0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
	0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
	0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
	0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
	0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
	0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, /* VEX2b (0xc5) VEX3b (0xc4) */
	0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
	0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
	1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, /* LOCK (0xf0) REPNZ (0xf2) REP (0xf3) */
	/* Decode64Bits */
	0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
	0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
	0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0,
	0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0,
	1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, /* REX: 0x40 - 0x4f */
	0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
	0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0,
	0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
	0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
	0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
	0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
	0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
	0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
	0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
	0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
	1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
};

/* Ignore all prefix. */
void prefixes_ignore_all(_PrefixState* ps)
{
	int i;
	for (i = 0; i < PFXIDX_MAX; i++)
		prefixes_ignore(ps, i);
}

/* Calculates which prefixes weren't used and accordingly sets the bits in the unusedPrefixesMask. */
uint16_t prefixes_set_unused_mask(_PrefixState* ps)
{
	/*
	 * The decodedPrefixes represents the prefixes that were *read* from the binary stream for the instruction.
	 * The usedPrefixes represents the prefixes that were actually used by the instruction in the *decode* phase.
	 * Xoring between the two will result in a 'diff' which returns the prefixes that were read
	 * from the stream *and* that were never used in the actual decoding.
	 *
	 * Only one prefix per type can be set in decodedPrefixes from the stream.
	 * Therefore it's enough to check each type once and set the flag accordingly.
	 * That's why we had to book-keep each prefix type and its position.
	 * So now we know which bits we need to set exactly in the mask.
	 */
	_iflags unusedPrefixesDiff = ps->decodedPrefixes ^ ps->usedPrefixes;
	uint16_t unusedPrefixesMask = ps->unusedPrefixesMask;

	/* Examine unused prefixes by type: */
	/*
	 * About REX: it might be set in the diff although it was never in the stream itself.
	 * This is because the vrex is shared between VEX and REX and some places flag it as REX usage, while
	 * we were really decoding an AVX instruction.
	 * It's not a big problem, because the prefixes_ignore func will ignore it anyway,
	 * since it wasn't seen earlier. But it's important to know this.
	 */
	if (unusedPrefixesDiff) {
		if (unusedPrefixesDiff & INST_PRE_REX) unusedPrefixesMask |= ps->pfxIndexer[PFXIDX_REX];
		if (unusedPrefixesDiff & INST_PRE_SEGOVRD_MASK) unusedPrefixesMask |= ps->pfxIndexer[PFXIDX_SEG];
		if (unusedPrefixesDiff & INST_PRE_LOKREP_MASK) unusedPrefixesMask |= ps->pfxIndexer[PFXIDX_LOREP];
		if (unusedPrefixesDiff & INST_PRE_OP_SIZE) unusedPrefixesMask |= ps->pfxIndexer[PFXIDX_OP_SIZE];
		if (unusedPrefixesDiff & INST_PRE_ADDR_SIZE) unusedPrefixesMask |= ps->pfxIndexer[PFXIDX_ADRS];
		/* If a VEX instruction was found, its prefix is considered as used, therefore no point for checking for it. */
	}

	return unusedPrefixesMask;
}

/*
 * Mark a prefix as unused, and bookkeep where we last saw this same type,
 * because in the future we might want to disable it too.
 */
_INLINE_ void prefixes_track_unused(_PrefixState* ps, int index, _PrefixIndexer pi)
{
	/* Mark the previously used prefix (if exists) in the unused mask. */
	prefixes_ignore(ps, pi);
	/* Book-keep the current index for this type. */
	ps->pfxIndexer[pi] = 1 << index;
}

/*
 * Read as many prefixes as possible, up to 15 bytes, and halt when we encounter non-prefix byte.
 * This algorithm tries to imitate a real processor, where the same prefix can appear a few times, etc.
 * The tiny complexity is that we want to know when a prefix was superfluous and mark any copy of it as unused.
 * Note that the last prefix of its type will be considered as used, and all the others (of same type) before it as unused.
 */
void prefixes_decode(_CodeInfo* ci, _PrefixState* ps)
{
	const uint8_t* rexPos = NULL;
	const uint8_t* start = ci->code;
	uint8_t byte, vex;
	unsigned int index;
	/*
	 * First thing to do, scan for prefixes, there are six types of prefixes.
	 * There may be up to six prefixes before a single instruction, not the same type, no special order,
	 * except REX/VEX must precede immediately the first opcode byte.
	 * BTW - This is the reason why I didn't make the REP prefixes part of the instructions (STOS/SCAS/etc).
	 *
	 * Another thing, the instruction maximum size is 15 bytes, thus if we read more than 15 bytes, we will halt.
	 *
	 * We attach all prefixes to the next instruction, there might be two or more occurrences from the same prefix.
	 * Also, since VEX can be allowed only once we will test it separately.
	 */
	for (index = 0;
		(ci->codeLen > 0) && (index < INST_MAXIMUM_SIZE);
		ci->code++, ci->codeLen--, index++) {
		/*
		NOTE: AMD treat lock/rep as two different groups... But I am based on Intel.

			- Lock and Repeat:
				- 0xF0 — LOCK
				- 0xF2 — REPNE/REPNZ
				- 0xF3 - REP/REPE/REPZ
			- Segment Override:
				- 0x2E - CS
				- 0x36 - SS
				- 0x3E - DS
				- 0x26 - ES
				- 0x64 - FS
				- 0x65 - GS
			- Operand-Size Override: 0x66, switching default size.
			- Address-Size Override: 0x67, switching default size.

		64 Bits:
			- REX: 0x40 - 0x4f, extends register access.
			- 2 Bytes VEX: 0xc4
			- 3 Bytes VEX: 0xc5
		32 Bits:
			- 2 Bytes VEX: 0xc4 11xx-xxxx
			- 3 Bytes VEX: 0xc5 11xx-xxxx
		*/

		/* Examine what type of prefix we got. */
		byte = *ci->code;
		switch (byte)
		{
		case PREFIX_OP_SIZE: {/* Op Size type: */
			ps->decodedPrefixes |= INST_PRE_OP_SIZE;
			prefixes_track_unused(ps, index, PFXIDX_OP_SIZE);
		} break;
			/* Look for both common arch prefixes. */
		case PREFIX_LOCK: {
			/* LOCK and REPx type: */
			ps->decodedPrefixes |= INST_PRE_LOCK;
			prefixes_track_unused(ps, index, PFXIDX_LOREP);
		} break;
		case PREFIX_REPNZ: {
			ps->decodedPrefixes |= INST_PRE_REPNZ;
			prefixes_track_unused(ps, index, PFXIDX_LOREP);
		} break;
		case PREFIX_REP: {
			ps->decodedPrefixes |= INST_PRE_REP;
			prefixes_track_unused(ps, index, PFXIDX_LOREP);
		} break;
		case PREFIX_CS: {
			/* Seg Overide type: */
			ps->decodedPrefixes &= ~INST_PRE_SEGOVRD_MASK;
			ps->decodedPrefixes |= INST_PRE_CS;
			prefixes_track_unused(ps, index, PFXIDX_SEG);
		} break;
		case PREFIX_SS: {
			ps->decodedPrefixes &= ~INST_PRE_SEGOVRD_MASK;
			ps->decodedPrefixes |= INST_PRE_SS;
			prefixes_track_unused(ps, index, PFXIDX_SEG);
		} break;
		case PREFIX_DS: {
			ps->decodedPrefixes &= ~INST_PRE_SEGOVRD_MASK;
			ps->decodedPrefixes |= INST_PRE_DS;
			prefixes_track_unused(ps, index, PFXIDX_SEG);
		} break;
		case PREFIX_ES: {
			ps->decodedPrefixes &= ~INST_PRE_SEGOVRD_MASK;
			ps->decodedPrefixes |= INST_PRE_ES;
			prefixes_track_unused(ps, index, PFXIDX_SEG);
		} break;
		case PREFIX_FS: {
			ps->decodedPrefixes &= ~INST_PRE_SEGOVRD_MASK;
			ps->decodedPrefixes |= INST_PRE_FS;
			prefixes_track_unused(ps, index, PFXIDX_SEG);
		} break;
		case PREFIX_GS: {
			ps->decodedPrefixes &= ~INST_PRE_SEGOVRD_MASK;
			ps->decodedPrefixes |= INST_PRE_GS;
			prefixes_track_unused(ps, index, PFXIDX_SEG);
		} break;
		case PREFIX_ADDR_SIZE: {
			/* Addr Size type: */
			ps->decodedPrefixes |= INST_PRE_ADDR_SIZE;
			prefixes_track_unused(ps, index, PFXIDX_ADRS);
		} break;
		default:
			if (ci->dt == Decode64Bits) {
				/* REX type, 64 bits decoding mode only: */
				if ((byte & 0xf0) == 0x40) {
					ps->decodedPrefixes |= INST_PRE_REX;
					rexPos = ci->code;
					ps->vrex = byte & 0xf; /* Keep only BXRW. */
					ps->prefixExtType = PET_REX;
					prefixes_track_unused(ps, index, PFXIDX_REX);
					continue;
				}
			}
			goto _Break2;
		}
	}
_Break2:

	/* 2 Bytes VEX: */
	if ((ci->codeLen >= 2) &&
		(*ci->code == PREFIX_VEX2b) &&
		((ci->code - start) <= INST_MAXIMUM_SIZE - 2)) {
		/*
		 * In 32 bits the second byte has to be in the special range of Mod=11.
		 * Otherwise it might be a normal LDS instruction.
		 */
		if ((ci->dt == Decode64Bits) || (*(ci->code + 1) >= INST_DIVIDED_MODRM)) {
			ps->vexPos = ci->code + 1;
			ps->decodedPrefixes |= INST_PRE_VEX;
			ps->prefixExtType = PET_VEX2BYTES;

			/*
			 * VEX 1 byte bits:
			 * |7-6--3-2-10|
			 * |R|vvvv|L|pp|
			 * |-----------|
			 */

			/* -- Convert from VEX prefix to VREX flags -- */
			vex = *ps->vexPos;
			if (!(vex & 0x80) && (ci->dt == Decode64Bits)) ps->vrex |= PREFIX_EX_R; /* Convert VEX.R. */
			if (vex & 4) ps->vrex |= PREFIX_EX_L; /* Convert VEX.L. */

			ci->code += 2;
			ci->codeLen -= 2;
		}
	}

	/* 3 Bytes VEX: */
	if ((ci->codeLen >= 3) &&
		(*ci->code == PREFIX_VEX3b) &&
		((ci->code - start) <= INST_MAXIMUM_SIZE - 3) &&
		(!(ps->decodedPrefixes & INST_PRE_VEX))) {
		/*
		 * In 32 bits the second byte has to be in the special range of Mod=11.
		 * Otherwise it might be a normal LES instruction.
		 * And we don't care now about the 3rd byte.
		 */
		if ((ci->dt == Decode64Bits) || (*(ci->code + 1) >= INST_DIVIDED_MODRM)) {
			ps->vexPos = ci->code + 1;
			ps->decodedPrefixes |= INST_PRE_VEX;
			ps->prefixExtType = PET_VEX3BYTES;

			/*
			 * VEX first and second bytes:
			 * |7-6-5-4----0|  |7-6--3-2-10|
			 * |R|X|B|m-mmmm|  |W|vvvv|L|pp|
			 * |------------|  |-----------|
			 */

			/* -- Convert from VEX prefix to VREX flags -- */
			vex = *ps->vexPos;
			ps->vrex |= ((~vex >> 5) & 0x7); /* Shift and invert VEX.R/X/B to their place */
			vex = *(ps->vexPos + 1);
			if (vex & 4) ps->vrex |= PREFIX_EX_L; /* Convert VEX.L. */
			if (vex & 0x80) ps->vrex |= PREFIX_EX_W; /* Convert VEX.W. */

			/* Clear some flags if the mode isn't 64 bits. */
			if (ci->dt != Decode64Bits) ps->vrex &= ~(PREFIX_EX_B | PREFIX_EX_X | PREFIX_EX_R | PREFIX_EX_W);

			ci->code += 3;
			ci->codeLen -= 3;
		}
	}

	if (ci->dt == Decode64Bits) {
		if (ps->decodedPrefixes & INST_PRE_REX) {
			/* REX prefix must precede first byte of instruction. */
			if (rexPos != (ci->code - 1)) {
				ps->decodedPrefixes &= ~INST_PRE_REX;
				if (ps->prefixExtType == PET_REX) ps->prefixExtType = PET_NONE; /* It might be a VEX by now, keep it that way. */
				prefixes_ignore(ps, PFXIDX_REX);
			}
			/*
			 * We will disable operand size prefix,
			 * if it exists only after decoding the instruction, since it might be a mandatory prefix.
			 * This will be done after calling inst_lookup in decode_inst.
			 */
		}
		/* In 64 bits, segment overrides of CS, DS, ES and SS are ignored. So don't take'em into account. */
		if (ps->decodedPrefixes & INST_PRE_SEGOVRD_MASK32) {
			ps->decodedPrefixes &= ~INST_PRE_SEGOVRD_MASK32;
			prefixes_ignore(ps, PFXIDX_SEG);
		}
	}

	/* Store number of prefixes scanned. */
	ps->count = (uint8_t)(ci->code - start);
}

/*
 * For every memory-indirection operand we want to set a used segment.
 * If the segment is being overrided with a prefix, we will need to check if it's a default.
 * Defaults don't use their prefix, e.g "mov [rsp]" can ignore a given SS: prefix,
 * but still set the used segment as SS.
 * This function is called only with SS and DS as defaults.
 * If there's a segment prefix used, it will override the default one.
 * And If the prefix is a default seg in 64 bits, it will be ignored.
 */
void prefixes_use_segment(_iflags defaultSeg, _PrefixState* ps, _DecodeType dt, _DInst* di)
{
	/* Extract given segment prefix from the decoded prefixes. */
	_iflags flags;

	if (dt == Decode64Bits) {
		if (ps->decodedPrefixes & INST_PRE_SEGOVRD_MASK64) { /* Either GS or FS. */
			di->segment = ps->decodedPrefixes & INST_PRE_GS ? R_GS : R_FS;
		}

		return;
	}

	flags = ps->decodedPrefixes & INST_PRE_SEGOVRD_MASK;

	/* Use the given prefix only if it's not the default. */
	if (flags && (flags != defaultSeg)) {
		ps->usedPrefixes |= flags;

		switch (flags >> 7) /* INST_PRE_CS is 1 << 7. And the rest of the prefixes follow as bit fields. */
		{
			case 1: di->segment = R_CS; break;
			case 2: di->segment = R_SS; break;
			case 4: di->segment = R_DS; break;
			case 8: di->segment = R_ES; break;
			case 0x10: di->segment = R_FS; break;
			case 0x20: di->segment = R_GS; break;
		}
	}
	else {
		if (defaultSeg == INST_PRE_SS) di->segment = SEGMENT_DEFAULT | R_SS;
		else di->segment = SEGMENT_DEFAULT | R_DS;
	}
}
