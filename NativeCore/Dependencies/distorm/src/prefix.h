/*
prefix.h

diStorm3 - Powerful disassembler for X86/AMD64
http://ragestorm.net/distorm/
distorm at gmail dot com
Copyright (C) 2003-2021 Gil Dabah
This library is licensed under the BSD license. See the file COPYING.
*/


#ifndef PREFIX_H
#define PREFIX_H

#include "config.h"
#include "decoder.h"


/* Specifies the type of the extension prefix, such as: REX, 2 bytes VEX, 3 bytes VEX. */
typedef enum {PET_NONE = 0, PET_REX, PET_VEX2BYTES, PET_VEX3BYTES} _PrefixExtType;

/* Specifies an index into a table of prefixes by their type. */
typedef enum {PFXIDX_NONE = -1, PFXIDX_REX, PFXIDX_LOREP, PFXIDX_SEG, PFXIDX_OP_SIZE, PFXIDX_ADRS, PFXIDX_MAX} _PrefixIndexer;

/*
* This holds the prefixes state for the current instruction we decode.
* decodedPrefixes includes all specific prefixes that the instruction got.
* start is a pointer to the first prefix to take into account.
* last is a pointer to the last byte we scanned.
* Other pointers are used to keep track of prefixes positions and help us know if they appeared already and where.
*/
typedef struct {
	_iflags decodedPrefixes, usedPrefixes;
	/* Number of prefixes scanned for current instruction, including VEX! */
	unsigned int count;
	uint16_t unusedPrefixesMask;
	/* Holds the offset to the prefix byte by its type. */
	uint16_t pfxIndexer[PFXIDX_MAX];
	_PrefixExtType prefixExtType;
	/* Indicates whether the operand size prefix (0x66) was used as a mandatory prefix. */
	int isOpSizeMandatory;
	/* If VEX prefix is used, store the VEX.vvvv field. */
	unsigned int vexV;
	/* The fields B/X/R/W/L of REX and VEX are stored together in this byte. */
	unsigned int vrex;
	const uint8_t* vexPos;
} _PrefixState;

/*
* Intel supports 6 types of prefixes, whereas AMD supports 5 types (lock is seperated from rep/nz).
* REX is the fifth prefix type, this time I'm based on AMD64.
* VEX is the 6th, though it can't be repeated.
*/
#define MAX_PREFIXES (5)

extern int PrefixTables[256 * 2];

_INLINE_ int prefixes_is_valid(unsigned char ch, _DecodeType dt)
{
	/* The predicate selects (branchlessly) second half table for 64 bits otherwise selects first half. */
	return PrefixTables[ch + ((dt >> 1) << 8)];
}

/* Ignore a specific prefix type. */
_INLINE_ void prefixes_ignore(_PrefixState* ps, _PrefixIndexer pi)
{
	/*
	 * If that type of prefix appeared already, set the bit of that *former* prefix.
	 * Anyway, set the new index of that prefix type to the current index, so next time we know its position.
	 */
	ps->unusedPrefixesMask |= ps->pfxIndexer[pi];
}

void prefixes_ignore_all(_PrefixState* ps);
uint16_t prefixes_set_unused_mask(_PrefixState* ps);
void prefixes_decode(_CodeInfo* ci, _PrefixState* ps);
void prefixes_use_segment(_iflags defaultSeg, _PrefixState* ps, _DecodeType dt, _DInst* di);

#endif /* PREFIX_H */
