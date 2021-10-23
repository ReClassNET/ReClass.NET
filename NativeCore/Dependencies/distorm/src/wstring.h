/*
wstring.h

diStorm3 - Powerful disassembler for X86/AMD64
http://ragestorm.net/distorm/
distorm at gmail dot com
Copyright (C) 2003-2021 Gil Dabah
This library is licensed under the BSD license. See the file COPYING.
*/


#ifndef WSTRING_H
#define WSTRING_H

#include "config.h"
#include "../include/mnemonics.h"

#ifndef DISTORM_LIGHT

_INLINE_ void strcat_WSR(unsigned char** str, const _WRegister* reg)
{
	/*
	 * Longest register name is YMM15 - 5 characters,
	 * Copy 8 so compiler can do a QWORD move.
	 * We copy nul termination and fix the length, so it's okay to copy more to the output buffer.
	 * There's a sentinel register to make sure we don't read past the end of the registers table.
	 */
	memcpy((int8_t*)*str, (const int8_t*)reg->p, 8);
	*str += reg->length;
}

#define strfinalize_WS(s, end) do { *end = 0; s.length = (unsigned int)((size_t)end - (size_t)s.p); } while (0)
#define chrcat_WS(s, ch) do { *s = ch; s += 1; } while (0)
#define strcat_WS(s, buf, copylen, advancelen) do { memcpy((int8_t*)s, buf, copylen); s += advancelen; } while(0)

#endif /* DISTORM_LIGHT */

#endif /* WSTRING_H */
