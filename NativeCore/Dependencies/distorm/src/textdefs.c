/*
textdefs.c

diStorm3 - Powerful disassembler for X86/AMD64
http://ragestorm.net/distorm/
distorm at gmail dot com
Copyright (C) 2003-2021 Gil Dabah
This library is licensed under the BSD license. See the file COPYING.
*/


#include "textdefs.h"

#ifndef DISTORM_LIGHT

static uint8_t Nibble2ChrTable[16] = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'};
#define NIBBLE_TO_CHR Nibble2ChrTable[t]

void str_hex(_WString* s, const uint8_t* buf, unsigned int len)
{
	/* 256 * 2 : 2 chars per byte value. */
	static const char* TextBTable =
		"000102030405060708090a0b0c0d0e0f" \
		"101112131415161718191a1b1c1d1e1f" \
		"202122232425262728292a2b2c2d2e2f" \
		"303132333435363738393a3b3c3d3e3f" \
		"404142434445464748494a4b4c4d4e4f" \
		"505152535455565758595a5b5c5d5e5f" \
		"606162636465666768696a6b6c6d6e6f" \
		"707172737475767778797a7b7c7d7e7f" \
		"808182838485868788898a8b8c8d8e8f" \
		"909192939495969798999a9b9c9d9e9f" \
		"a0a1a2a3a4a5a6a7a8a9aaabacadaeaf" \
		"b0b1b2b3b4b5b6b7b8b9babbbcbdbebf" \
		"c0c1c2c3c4c5c6c7c8c9cacbcccdcecf" \
		"d0d1d2d3d4d5d6d7d8d9dadbdcdddedf" \
		"e0e1e2e3e4e5e6e7e8e9eaebecedeeef" \
		"f0f1f2f3f4f5f6f7f8f9fafbfcfdfeff";
	
	unsigned int i = 0;
	/* Length is at least 1, enter loop. */
	s->length = len * 2;
	s->p[len * 2] = 0;
	do {
		RSHORT(&s->p[i]) = RSHORT(&TextBTable[(*buf) * 2]);
		buf++;
		i += 2;
	} while (i < len * 2);
}

#ifdef SUPPORT_64BIT_OFFSET

void str_int_impl(unsigned char** s, uint64_t x)
{
	int8_t* buf;
	int shift = 0;
	OFFSET_INTEGER t = x;

	buf = (int8_t*)*s;

	*buf++ = '0';
	*buf++ = 'x';

	if (x == 0) {
		*buf = '0';
		*s += 3;
		return;
	}

	do {
		t >>= 4;
		shift += 4;
	} while (t);

	do {
		shift -= 4;
		t = (x >> shift) & 0xf;
		*buf++ = NIBBLE_TO_CHR;
	} while (shift > 0);

	*s = (unsigned char*)buf;
}

#else

void str_int_impl(unsigned char** s, uint8_t src[8])
{
	int8_t* buf;
	int i = 0, shift = 0;
	uint32_t x = RULONG(&src[sizeof(int32_t)]);
	int t;

	buf = (int8_t*)*s;
	buf[0] = '0';
	buf[1] = 'x';
	buf += 2;

	for (shift = 28; shift != -4; shift -= 4) {
		t = (x >> shift) & 0xf;
		if (i | t) buf[i++] = NIBBLE_TO_CHR;
	}

	x = RULONG(src);
	for (shift = 28; shift != 0; shift -= 4) {
		t = (x >> shift) & 0xf;
		if (i | t) buf[i++] = NIBBLE_TO_CHR;
	}
	t = x & 0xf;
	buf[i++] = NIBBLE_TO_CHR;

	*s += (size_t)(i + 2);
}

#endif /* SUPPORT_64BIT_OFFSET */

#endif /* DISTORM_LIGHT */
