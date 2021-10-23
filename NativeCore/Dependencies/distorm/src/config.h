/*
config.h

diStorm3 - Powerful disassembler for X86/AMD64
http://ragestorm.net/distorm/
distorm at gmail dot com
Copyright (C) 2003-2021 Gil Dabah
This library is licensed under the BSD license. See the file COPYING.
*/


#ifndef CONFIG_H
#define CONFIG_H

/* diStorm version number. */
#define __DISTORMV__ 0x030502

#include <string.h> /* memset, memcpy - can be easily self implemented for libc independency. */

#include "../include/distorm.h"


/*
 * 64 bit offsets support:
 * This macro should be defined from compiler command line flags, e.g: -DSUPPORT_64BIT_OFFSET
 * Note: make sure that the caller (library user) defines it too!
 */
/* #define SUPPORT_64BIT_OFFSET */

/*
 * If you compile diStorm as a dynamic library (.dll or .so) file, make sure you uncomment the next line.
 * So the interface functions will be exported, otherwise they are useable only for static library.
 * For example, this macro is being set for compiling diStorm as a .dll for Python with CTypes.
 */
/* #define DISTORM_DYNAMIC */

/*
 * If DISTORM_LIGHT is defined, everything involved in formatting the instructions
 * as text will be excluded from compilation.
 * distorm_decode(..) and distorm_format(..) will not be available.
 * This will decrease the size of the executable and leave you with decomposition functionality only.
 *
 * Note: it should be either set in the preprocessor definitions manually or in command line -D switch.
 * #define DISTORM_LIGHT
 */

/*
 * diStorm now supports little/big endian CPU's.
 * It should detect the endianness according to predefined macro's of the compiler.
 * If you don't use GCC/MSVC you will have to define it on your own.
 */

/* These macros are used in order to make the code portable. */
#ifdef __GNUC__

#include <stdint.h>

#define _DLLEXPORT_
#define _FASTCALL_
/* Keep inline as static (arrrrg) as it would break linux on some flavors otherwise. */
#define _INLINE_ static
/* GCC ignores this directive... */
/*#define _FASTCALL_ __attribute__((__fastcall__))*/

/* Set endianity (supposed to be LE though): */
#ifdef __BIG_ENDIAN__
	#define BE_SYSTEM
#endif

/* End of __GCC__ */

#elif __WATCOMC__

#include <stdint.h>

#define _DLLEXPORT_
#define _FASTCALL_
#define _INLINE_ __inline

/* End of __WATCOMC__ */

#elif __DMC__

#include <stdint.h>

#define _DLLEXPORT_
#define _FASTCALL_
#define _INLINE_ __inline

/* End of __DMC__ */

#elif __TINYC__

#include <stdint.h>

#define _DLLEXPORT_
#define _FASTCALL_
#define _INLINE_ static

/* End of __TINYC__ */

#elif _MSC_VER

/* stdint alternative is defined in distorm.h */

#define _DLLEXPORT_ __declspec(dllexport)
#define _FASTCALL_ __fastcall
#define _INLINE_ __inline

/* Set endianity (supposed to be LE though): */
#if !defined(_M_IX86) && !defined(_M_X64)
	#define BE_SYSTEM
#endif

#endif /* #elif _MSC_VER */

/* If the library isn't compiled as a dynamic library don't export any functions. */
#ifndef DISTORM_DYNAMIC
#undef _DLLEXPORT_
#define _DLLEXPORT_
#endif

#ifndef FALSE
#define FALSE 0
#endif
#ifndef TRUE
#define TRUE 1
#endif

/* Define stream read functions for big endian systems. */
#ifdef BE_SYSTEM

/* Avoid defining 'static static' for GCC. */
#ifndef __GNUC__
#define STATIC_INLINE static _INLINE_
#else
#define STATIC_INLINE static
#endif

/*
 * Assumption: These functions can read from the stream safely!
 * Swap endianity of input to little endian.
 */
STATIC_INLINE int16_t RSHORT(const uint8_t *s)
{
	return s[0] | (s[1] << 8);
}
STATIC_INLINE uint16_t RUSHORT(const uint8_t *s)
{
	return s[0] | (s[1] << 8);
}
STATIC_INLINE int32_t RLONG(const uint8_t *s)
{
	return s[0] | (s[1] << 8) | (s[2] << 16) | (s[3] << 24);
}
STATIC_INLINE uint32_t RULONG(const uint8_t *s)
{
	return s[0] | (s[1] << 8) | (s[2] << 16) | (s[3] << 24);
}
STATIC_INLINE int64_t RLLONG(const uint8_t *s)
{
	return s[0] | (s[1] << 8) | (s[2] << 16) | (s[3] << 24) | ((uint64_t)s[4] << 32) | ((uint64_t)s[5] << 40) | ((uint64_t)s[6] << 48) | ((uint64_t)s[7] << 56);
}
STATIC_INLINE uint64_t RULLONG(const uint8_t *s)
{
	return s[0] | (s[1] << 8) | (s[2] << 16) | (s[3] << 24) | ((uint64_t)s[4] << 32) | ((uint64_t)s[5] << 40) | ((uint64_t)s[6] << 48) | ((uint64_t)s[7] << 56);
}

#undef STATIC_INLINE

#else
/* Little endian macro's will just make the cast. */
#define RSHORT(x) *(int16_t *)x
#define RUSHORT(x) *(uint16_t *)x
#define RLONG(x) *(int32_t *)x
#define RULONG(x) *(uint32_t *)x
#define RLLONG(x) *(int64_t *)x
#define RULLONG(x) *(uint64_t *)x
#endif

#endif /* CONFIG_H */
