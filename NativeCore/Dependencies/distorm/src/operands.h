/*
operands.h

diStorm3 - Powerful disassembler for X86/AMD64
http://ragestorm.net/distorm/
distorm at gmail dot com
Copyright (C) 2003-2021 Gil Dabah
This library is licensed under the BSD license. See the file COPYING.
*/


#ifndef OPERANDS_H
#define OPERANDS_H

#include "config.h"
#include "decoder.h"
#include "prefix.h"
#include "instructions.h"

int operands_extract(_CodeInfo* ci, _DInst* di, _InstInfo* ii,
                     _iflags instFlags, _OpType type,
                     unsigned int modrm, _PrefixState* ps, _DecodeType effOpSz,
                     _DecodeType effAdrSz, _Operand* op);

#endif /* OPERANDS_H */
