/*
mnemonics.c

diStorm3 - Powerful disassembler for X86/AMD64
http://ragestorm.net/distorm/
distorm at gmail dot com
Copyright (C) 2003-2018 Gil Dabah
This library is licensed under the BSD license. See the file COPYING.
*/


#include "../include/mnemonics.h"

#ifndef DISTORM_LIGHT

const unsigned char _MNEMONICS[] =
"\x09" "undefined\0" "\x03" "add\0" "\x04" "push\0" "\x03" "pop\0" \
"\x02" "or\0" "\x03" "adc\0" "\x03" "sbb\0" "\x03" "and\0" "\x03" "daa\0" \
"\x03" "sub\0" "\x03" "das\0" "\x03" "xor\0" "\x03" "aaa\0" "\x03" "cmp\0" \
"\x03" "aas\0" "\x03" "inc\0" "\x03" "dec\0" "\x05" "pusha\0" "\x04" "popa\0" \
"\x05" "bound\0" "\x04" "arpl\0" "\x04" "imul\0" "\x03" "ins\0" "\x04" "outs\0" \
"\x02" "jo\0" "\x03" "jno\0" "\x02" "jb\0" "\x03" "jae\0" "\x02" "jz\0" \
"\x03" "jnz\0" "\x03" "jbe\0" "\x02" "ja\0" "\x02" "js\0" "\x03" "jns\0" \
"\x02" "jp\0" "\x03" "jnp\0" "\x02" "jl\0" "\x03" "jge\0" "\x03" "jle\0" \
"\x02" "jg\0" "\x04" "test\0" "\x04" "xchg\0" "\x03" "mov\0" "\x03" "lea\0" \
"\x03" "cbw\0" "\x04" "cwde\0" "\x04" "cdqe\0" "\x03" "cwd\0" "\x03" "cdq\0" \
"\x03" "cqo\0" "\x08" "call far\0" "\x05" "pushf\0" "\x04" "popf\0" \
"\x04" "sahf\0" "\x04" "lahf\0" "\x04" "movs\0" "\x04" "cmps\0" "\x04" "stos\0" \
"\x04" "lods\0" "\x04" "scas\0" "\x03" "ret\0" "\x03" "les\0" "\x03" "lds\0" \
"\x05" "enter\0" "\x05" "leave\0" "\x04" "retf\0" "\x05" "int 3\0" \
"\x03" "int\0" "\x04" "into\0" "\x04" "iret\0" "\x03" "aam\0" "\x03" "aad\0" \
"\x04" "salc\0" "\x04" "xlat\0" "\x06" "loopnz\0" "\x05" "loopz\0" \
"\x04" "loop\0" "\x04" "jcxz\0" "\x05" "jecxz\0" "\x05" "jrcxz\0" "\x02" "in\0" \
"\x03" "out\0" "\x04" "call\0" "\x03" "jmp\0" "\x07" "jmp far\0" "\x04" "int1\0" \
"\x03" "hlt\0" "\x03" "cmc\0" "\x03" "clc\0" "\x03" "stc\0" "\x03" "cli\0" \
"\x03" "sti\0" "\x03" "cld\0" "\x03" "std\0" "\x03" "lar\0" "\x03" "lsl\0" \
"\x07" "syscall\0" "\x04" "clts\0" "\x06" "sysret\0" "\x04" "invd\0" \
"\x06" "wbinvd\0" "\x03" "ud2\0" "\x05" "femms\0" "\x03" "nop\0" "\x05" "wrmsr\0" \
"\x05" "rdtsc\0" "\x05" "rdmsr\0" "\x05" "rdpmc\0" "\x08" "sysenter\0" \
"\x07" "sysexit\0" "\x06" "getsec\0" "\x05" "cmovo\0" "\x06" "cmovno\0" \
"\x05" "cmovb\0" "\x06" "cmovae\0" "\x05" "cmovz\0" "\x06" "cmovnz\0" \
"\x06" "cmovbe\0" "\x05" "cmova\0" "\x05" "cmovs\0" "\x06" "cmovns\0" \
"\x05" "cmovp\0" "\x06" "cmovnp\0" "\x05" "cmovl\0" "\x06" "cmovge\0" \
"\x06" "cmovle\0" "\x05" "cmovg\0" "\x04" "seto\0" "\x05" "setno\0" \
"\x04" "setb\0" "\x05" "setae\0" "\x04" "setz\0" "\x05" "setnz\0" "\x05" "setbe\0" \
"\x04" "seta\0" "\x04" "sets\0" "\x05" "setns\0" "\x04" "setp\0" "\x05" "setnp\0" \
"\x04" "setl\0" "\x05" "setge\0" "\x05" "setle\0" "\x04" "setg\0" "\x05" "cpuid\0" \
"\x02" "bt\0" "\x04" "shld\0" "\x03" "rsm\0" "\x03" "bts\0" "\x04" "shrd\0" \
"\x07" "cmpxchg\0" "\x03" "lss\0" "\x03" "btr\0" "\x03" "lfs\0" "\x03" "lgs\0" \
"\x05" "movzx\0" "\x03" "btc\0" "\x05" "movsx\0" "\x04" "xadd\0" "\x06" "movnti\0" \
"\x05" "bswap\0" "\x03" "rol\0" "\x03" "ror\0" "\x03" "rcl\0" "\x03" "rcr\0" \
"\x03" "shl\0" "\x03" "shr\0" "\x03" "sal\0" "\x03" "sar\0" "\x06" "xabort\0" \
"\x06" "xbegin\0" "\x04" "fadd\0" "\x04" "fmul\0" "\x04" "fcom\0" "\x05" "fcomp\0" \
"\x04" "fsub\0" "\x05" "fsubr\0" "\x04" "fdiv\0" "\x05" "fdivr\0" "\x03" "fld\0" \
"\x03" "fst\0" "\x04" "fstp\0" "\x06" "fldenv\0" "\x05" "fldcw\0" "\x04" "fxch\0" \
"\x04" "fnop\0" "\x04" "fchs\0" "\x04" "fabs\0" "\x04" "ftst\0" "\x04" "fxam\0" \
"\x04" "fld1\0" "\x06" "fldl2t\0" "\x06" "fldl2e\0" "\x05" "fldpi\0" \
"\x06" "fldlg2\0" "\x06" "fldln2\0" "\x04" "fldz\0" "\x05" "f2xm1\0" \
"\x05" "fyl2x\0" "\x05" "fptan\0" "\x06" "fpatan\0" "\x07" "fxtract\0" \
"\x06" "fprem1\0" "\x07" "fdecstp\0" "\x07" "fincstp\0" "\x05" "fprem\0" \
"\x07" "fyl2xp1\0" "\x05" "fsqrt\0" "\x07" "fsincos\0" "\x07" "frndint\0" \
"\x06" "fscale\0" "\x04" "fsin\0" "\x04" "fcos\0" "\x05" "fiadd\0" \
"\x05" "fimul\0" "\x05" "ficom\0" "\x06" "ficomp\0" "\x05" "fisub\0" \
"\x06" "fisubr\0" "\x05" "fidiv\0" "\x06" "fidivr\0" "\x06" "fcmovb\0" \
"\x06" "fcmove\0" "\x07" "fcmovbe\0" "\x06" "fcmovu\0" "\x07" "fucompp\0" \
"\x04" "fild\0" "\x06" "fisttp\0" "\x04" "fist\0" "\x05" "fistp\0" \
"\x07" "fcmovnb\0" "\x07" "fcmovne\0" "\x08" "fcmovnbe\0" "\x07" "fcmovnu\0" \
"\x04" "feni\0" "\x06" "fedisi\0" "\x06" "fsetpm\0" "\x06" "fucomi\0" \
"\x05" "fcomi\0" "\x06" "frstor\0" "\x05" "ffree\0" "\x05" "fucom\0" \
"\x06" "fucomp\0" "\x05" "faddp\0" "\x05" "fmulp\0" "\x06" "fcompp\0" \
"\x06" "fsubrp\0" "\x05" "fsubp\0" "\x06" "fdivrp\0" "\x05" "fdivp\0" \
"\x04" "fbld\0" "\x05" "fbstp\0" "\x07" "fucomip\0" "\x06" "fcomip\0" \
"\x03" "not\0" "\x03" "neg\0" "\x03" "mul\0" "\x03" "div\0" "\x04" "idiv\0" \
"\x04" "sldt\0" "\x03" "str\0" "\x04" "lldt\0" "\x03" "ltr\0" "\x04" "verr\0" \
"\x04" "verw\0" "\x04" "sgdt\0" "\x04" "sidt\0" "\x04" "lgdt\0" "\x04" "lidt\0" \
"\x04" "smsw\0" "\x04" "lmsw\0" "\x06" "invlpg\0" "\x06" "vmcall\0" \
"\x08" "vmlaunch\0" "\x08" "vmresume\0" "\x06" "vmxoff\0" "\x07" "monitor\0" \
"\x05" "mwait\0" "\x06" "xgetbv\0" "\x06" "xsetbv\0" "\x06" "vmfunc\0" \
"\x04" "xend\0" "\x05" "vmrun\0" "\x07" "vmmcall\0" "\x06" "vmload\0" \
"\x06" "vmsave\0" "\x04" "stgi\0" "\x04" "clgi\0" "\x06" "skinit\0" \
"\x07" "invlpga\0" "\x06" "swapgs\0" "\x06" "rdtscp\0" "\x08" "prefetch\0" \
"\x09" "prefetchw\0" "\x05" "pi2fw\0" "\x05" "pi2fd\0" "\x05" "pf2iw\0" \
"\x05" "pf2id\0" "\x06" "pfnacc\0" "\x07" "pfpnacc\0" "\x07" "pfcmpge\0" \
"\x05" "pfmin\0" "\x05" "pfrcp\0" "\x07" "pfrsqrt\0" "\x05" "pfsub\0" \
"\x05" "pfadd\0" "\x07" "pfcmpgt\0" "\x05" "pfmax\0" "\x08" "pfrcpit1\0" \
"\x08" "pfrsqit1\0" "\x06" "pfsubr\0" "\x05" "pfacc\0" "\x07" "pfcmpeq\0" \
"\x05" "pfmul\0" "\x08" "pfrcpit2\0" "\x07" "pmulhrw\0" "\x06" "pswapd\0" \
"\x07" "pavgusb\0" "\x06" "movups\0" "\x06" "movupd\0" "\x05" "movss\0" \
"\x05" "movsd\0" "\x07" "vmovups\0" "\x07" "vmovupd\0" "\x06" "vmovss\0" \
"\x06" "vmovsd\0" "\x07" "movhlps\0" "\x06" "movlps\0" "\x06" "movlpd\0" \
"\x08" "movsldup\0" "\x07" "movddup\0" "\x08" "vmovhlps\0" "\x07" "vmovlps\0" \
"\x07" "vmovlpd\0" "\x09" "vmovsldup\0" "\x08" "vmovddup\0" "\x08" "unpcklps\0" \
"\x08" "unpcklpd\0" "\x09" "vunpcklps\0" "\x09" "vunpcklpd\0" "\x08" "unpckhps\0" \
"\x08" "unpckhpd\0" "\x09" "vunpckhps\0" "\x09" "vunpckhpd\0" "\x07" "movlhps\0" \
"\x06" "movhps\0" "\x06" "movhpd\0" "\x08" "movshdup\0" "\x08" "vmovlhps\0" \
"\x07" "vmovhps\0" "\x07" "vmovhpd\0" "\x09" "vmovshdup\0" "\x0b" "prefetchnta\0" \
"\x0a" "prefetcht0\0" "\x0a" "prefetcht1\0" "\x0a" "prefetcht2\0" "\x06" "movaps\0" \
"\x06" "movapd\0" "\x07" "vmovaps\0" "\x07" "vmovapd\0" "\x08" "cvtpi2ps\0" \
"\x08" "cvtpi2pd\0" "\x08" "cvtsi2ss\0" "\x08" "cvtsi2sd\0" "\x09" "vcvtsi2ss\0" \
"\x09" "vcvtsi2sd\0" "\x07" "movntps\0" "\x07" "movntpd\0" "\x07" "movntss\0" \
"\x07" "movntsd\0" "\x08" "vmovntps\0" "\x08" "vmovntpd\0" "\x09" "cvttps2pi\0" \
"\x09" "cvttpd2pi\0" "\x09" "cvttss2si\0" "\x09" "cvttsd2si\0" "\x0a" "vcvttss2si\0" \
"\x0a" "vcvttsd2si\0" "\x08" "cvtps2pi\0" "\x08" "cvtpd2pi\0" "\x08" "cvtss2si\0" \
"\x08" "cvtsd2si\0" "\x09" "vcvtss2si\0" "\x09" "vcvtsd2si\0" "\x07" "ucomiss\0" \
"\x07" "ucomisd\0" "\x08" "vucomiss\0" "\x08" "vucomisd\0" "\x06" "comiss\0" \
"\x06" "comisd\0" "\x07" "vcomiss\0" "\x07" "vcomisd\0" "\x08" "movmskps\0" \
"\x08" "movmskpd\0" "\x09" "vmovmskps\0" "\x09" "vmovmskpd\0" "\x06" "sqrtps\0" \
"\x06" "sqrtpd\0" "\x06" "sqrtss\0" "\x06" "sqrtsd\0" "\x07" "vsqrtps\0" \
"\x07" "vsqrtpd\0" "\x07" "vsqrtss\0" "\x07" "vsqrtsd\0" "\x07" "rsqrtps\0" \
"\x07" "rsqrtss\0" "\x08" "vrsqrtps\0" "\x08" "vrsqrtss\0" "\x05" "rcpps\0" \
"\x05" "rcpss\0" "\x06" "vrcpps\0" "\x06" "vrcpss\0" "\x05" "andps\0" \
"\x05" "andpd\0" "\x06" "vandps\0" "\x06" "vandpd\0" "\x06" "andnps\0" \
"\x06" "andnpd\0" "\x07" "vandnps\0" "\x07" "vandnpd\0" "\x04" "orps\0" \
"\x04" "orpd\0" "\x05" "vorps\0" "\x05" "vorpd\0" "\x05" "xorps\0" \
"\x05" "xorpd\0" "\x06" "vxorps\0" "\x06" "vxorpd\0" "\x05" "addps\0" \
"\x05" "addpd\0" "\x05" "addss\0" "\x05" "addsd\0" "\x06" "vaddps\0" \
"\x06" "vaddpd\0" "\x06" "vaddss\0" "\x06" "vaddsd\0" "\x05" "mulps\0" \
"\x05" "mulpd\0" "\x05" "mulss\0" "\x05" "mulsd\0" "\x06" "vmulps\0" \
"\x06" "vmulpd\0" "\x06" "vmulss\0" "\x06" "vmulsd\0" "\x08" "cvtps2pd\0" \
"\x08" "cvtpd2ps\0" "\x08" "cvtss2sd\0" "\x08" "cvtsd2ss\0" "\x09" "vcvtps2pd\0" \
"\x09" "vcvtpd2ps\0" "\x09" "vcvtss2sd\0" "\x09" "vcvtsd2ss\0" "\x08" "cvtdq2ps\0" \
"\x08" "cvtps2dq\0" "\x09" "cvttps2dq\0" "\x09" "vcvtdq2ps\0" "\x09" "vcvtps2dq\0" \
"\x0a" "vcvttps2dq\0" "\x05" "subps\0" "\x05" "subpd\0" "\x05" "subss\0" \
"\x05" "subsd\0" "\x06" "vsubps\0" "\x06" "vsubpd\0" "\x06" "vsubss\0" \
"\x06" "vsubsd\0" "\x05" "minps\0" "\x05" "minpd\0" "\x05" "minss\0" \
"\x05" "minsd\0" "\x06" "vminps\0" "\x06" "vminpd\0" "\x06" "vminss\0" \
"\x06" "vminsd\0" "\x05" "divps\0" "\x05" "divpd\0" "\x05" "divss\0" \
"\x05" "divsd\0" "\x06" "vdivps\0" "\x06" "vdivpd\0" "\x06" "vdivss\0" \
"\x06" "vdivsd\0" "\x05" "maxps\0" "\x05" "maxpd\0" "\x05" "maxss\0" \
"\x05" "maxsd\0" "\x06" "vmaxps\0" "\x06" "vmaxpd\0" "\x06" "vmaxss\0" \
"\x06" "vmaxsd\0" "\x09" "punpcklbw\0" "\x0a" "vpunpcklbw\0" "\x09" "punpcklwd\0" \
"\x0a" "vpunpcklwd\0" "\x09" "punpckldq\0" "\x0a" "vpunpckldq\0" "\x08" "packsswb\0" \
"\x09" "vpacksswb\0" "\x07" "pcmpgtb\0" "\x08" "vpcmpgtb\0" "\x07" "pcmpgtw\0" \
"\x08" "vpcmpgtw\0" "\x07" "pcmpgtd\0" "\x08" "vpcmpgtd\0" "\x08" "packuswb\0" \
"\x09" "vpackuswb\0" "\x09" "punpckhbw\0" "\x0a" "vpunpckhbw\0" "\x09" "punpckhwd\0" \
"\x0a" "vpunpckhwd\0" "\x09" "punpckhdq\0" "\x0a" "vpunpckhdq\0" "\x08" "packssdw\0" \
"\x09" "vpackssdw\0" "\x0a" "punpcklqdq\0" "\x0b" "vpunpcklqdq\0" "\x0a" "punpckhqdq\0" \
"\x0b" "vpunpckhqdq\0" "\x04" "movd\0" "\x04" "movq\0" "\x05" "vmovd\0" \
"\x05" "vmovq\0" "\x06" "movdqa\0" "\x06" "movdqu\0" "\x07" "vmovdqa\0" \
"\x07" "vmovdqu\0" "\x06" "pshufw\0" "\x06" "pshufd\0" "\x07" "pshufhw\0" \
"\x07" "pshuflw\0" "\x07" "vpshufd\0" "\x08" "vpshufhw\0" "\x08" "vpshuflw\0" \
"\x07" "pcmpeqb\0" "\x08" "vpcmpeqb\0" "\x07" "pcmpeqw\0" "\x08" "vpcmpeqw\0" \
"\x07" "pcmpeqd\0" "\x08" "vpcmpeqd\0" "\x04" "emms\0" "\x0a" "vzeroupper\0" \
"\x08" "vzeroall\0" "\x06" "vmread\0" "\x05" "extrq\0" "\x07" "insertq\0" \
"\x07" "vmwrite\0" "\x08" "cvtph2ps\0" "\x08" "cvtps2ph\0" "\x06" "haddpd\0" \
"\x06" "haddps\0" "\x07" "vhaddpd\0" "\x07" "vhaddps\0" "\x06" "hsubpd\0" \
"\x06" "hsubps\0" "\x07" "vhsubpd\0" "\x07" "vhsubps\0" "\x05" "xsave\0" \
"\x07" "xsave64\0" "\x06" "lfence\0" "\x06" "xrstor\0" "\x08" "xrstor64\0" \
"\x06" "mfence\0" "\x08" "xsaveopt\0" "\x0a" "xsaveopt64\0" "\x06" "sfence\0" \
"\x07" "clflush\0" "\x06" "popcnt\0" "\x03" "bsf\0" "\x05" "tzcnt\0" \
"\x03" "bsr\0" "\x05" "lzcnt\0" "\x07" "cmpeqps\0" "\x07" "cmpltps\0" \
"\x07" "cmpleps\0" "\x0a" "cmpunordps\0" "\x08" "cmpneqps\0" "\x08" "cmpnltps\0" \
"\x08" "cmpnleps\0" "\x08" "cmpordps\0" "\x07" "cmpeqpd\0" "\x07" "cmpltpd\0" \
"\x07" "cmplepd\0" "\x0a" "cmpunordpd\0" "\x08" "cmpneqpd\0" "\x08" "cmpnltpd\0" \
"\x08" "cmpnlepd\0" "\x08" "cmpordpd\0" "\x07" "cmpeqss\0" "\x07" "cmpltss\0" \
"\x07" "cmpless\0" "\x0a" "cmpunordss\0" "\x08" "cmpneqss\0" "\x08" "cmpnltss\0" \
"\x08" "cmpnless\0" "\x08" "cmpordss\0" "\x07" "cmpeqsd\0" "\x07" "cmpltsd\0" \
"\x07" "cmplesd\0" "\x0a" "cmpunordsd\0" "\x08" "cmpneqsd\0" "\x08" "cmpnltsd\0" \
"\x08" "cmpnlesd\0" "\x08" "cmpordsd\0" "\x08" "vcmpeqps\0" "\x08" "vcmpltps\0" \
"\x08" "vcmpleps\0" "\x0b" "vcmpunordps\0" "\x09" "vcmpneqps\0" "\x09" "vcmpnltps\0" \
"\x09" "vcmpnleps\0" "\x09" "vcmpordps\0" "\x0b" "vcmpeq_uqps\0" "\x09" "vcmpngeps\0" \
"\x09" "vcmpngtps\0" "\x0b" "vcmpfalseps\0" "\x0c" "vcmpneq_oqps\0" "\x08" "vcmpgeps\0" \
"\x08" "vcmpgtps\0" "\x0a" "vcmptrueps\0" "\x0b" "vcmpeq_osps\0" "\x0b" "vcmplt_oqps\0" \
"\x0b" "vcmple_oqps\0" "\x0d" "vcmpunord_sps\0" "\x0c" "vcmpneq_usps\0" \
"\x0c" "vcmpnlt_uqps\0" "\x0c" "vcmpnle_uqps\0" "\x0b" "vcmpord_sps\0" \
"\x0b" "vcmpeq_usps\0" "\x0c" "vcmpnge_uqps\0" "\x0c" "vcmpngt_uqps\0" \
"\x0e" "vcmpfalse_osps\0" "\x0c" "vcmpneq_osps\0" "\x0b" "vcmpge_oqps\0" \
"\x0b" "vcmpgt_oqps\0" "\x0d" "vcmptrue_usps\0" "\x08" "vcmpeqpd\0" "\x08" "vcmpltpd\0" \
"\x08" "vcmplepd\0" "\x0b" "vcmpunordpd\0" "\x09" "vcmpneqpd\0" "\x09" "vcmpnltpd\0" \
"\x09" "vcmpnlepd\0" "\x09" "vcmpordpd\0" "\x0b" "vcmpeq_uqpd\0" "\x09" "vcmpngepd\0" \
"\x09" "vcmpngtpd\0" "\x0b" "vcmpfalsepd\0" "\x0c" "vcmpneq_oqpd\0" "\x08" "vcmpgepd\0" \
"\x08" "vcmpgtpd\0" "\x0a" "vcmptruepd\0" "\x0b" "vcmpeq_ospd\0" "\x0b" "vcmplt_oqpd\0" \
"\x0b" "vcmple_oqpd\0" "\x0d" "vcmpunord_spd\0" "\x0c" "vcmpneq_uspd\0" \
"\x0c" "vcmpnlt_uqpd\0" "\x0c" "vcmpnle_uqpd\0" "\x0b" "vcmpord_spd\0" \
"\x0b" "vcmpeq_uspd\0" "\x0c" "vcmpnge_uqpd\0" "\x0c" "vcmpngt_uqpd\0" \
"\x0e" "vcmpfalse_ospd\0" "\x0c" "vcmpneq_ospd\0" "\x0b" "vcmpge_oqpd\0" \
"\x0b" "vcmpgt_oqpd\0" "\x0d" "vcmptrue_uspd\0" "\x08" "vcmpeqss\0" "\x08" "vcmpltss\0" \
"\x08" "vcmpless\0" "\x0b" "vcmpunordss\0" "\x09" "vcmpneqss\0" "\x09" "vcmpnltss\0" \
"\x09" "vcmpnless\0" "\x09" "vcmpordss\0" "\x0b" "vcmpeq_uqss\0" "\x09" "vcmpngess\0" \
"\x09" "vcmpngtss\0" "\x0b" "vcmpfalsess\0" "\x0c" "vcmpneq_oqss\0" "\x08" "vcmpgess\0" \
"\x08" "vcmpgtss\0" "\x0a" "vcmptruess\0" "\x0b" "vcmpeq_osss\0" "\x0b" "vcmplt_oqss\0" \
"\x0b" "vcmple_oqss\0" "\x0d" "vcmpunord_sss\0" "\x0c" "vcmpneq_usss\0" \
"\x0c" "vcmpnlt_uqss\0" "\x0c" "vcmpnle_uqss\0" "\x0b" "vcmpord_sss\0" \
"\x0b" "vcmpeq_usss\0" "\x0c" "vcmpnge_uqss\0" "\x0c" "vcmpngt_uqss\0" \
"\x0e" "vcmpfalse_osss\0" "\x0c" "vcmpneq_osss\0" "\x0b" "vcmpge_oqss\0" \
"\x0b" "vcmpgt_oqss\0" "\x0d" "vcmptrue_usss\0" "\x08" "vcmpeqsd\0" "\x08" "vcmpltsd\0" \
"\x08" "vcmplesd\0" "\x0b" "vcmpunordsd\0" "\x09" "vcmpneqsd\0" "\x09" "vcmpnltsd\0" \
"\x09" "vcmpnlesd\0" "\x09" "vcmpordsd\0" "\x0b" "vcmpeq_uqsd\0" "\x09" "vcmpngesd\0" \
"\x09" "vcmpngtsd\0" "\x0b" "vcmpfalsesd\0" "\x0c" "vcmpneq_oqsd\0" "\x08" "vcmpgesd\0" \
"\x08" "vcmpgtsd\0" "\x0a" "vcmptruesd\0" "\x0b" "vcmpeq_ossd\0" "\x0b" "vcmplt_oqsd\0" \
"\x0b" "vcmple_oqsd\0" "\x0d" "vcmpunord_ssd\0" "\x0c" "vcmpneq_ussd\0" \
"\x0c" "vcmpnlt_uqsd\0" "\x0c" "vcmpnle_uqsd\0" "\x0b" "vcmpord_ssd\0" \
"\x0b" "vcmpeq_ussd\0" "\x0c" "vcmpnge_uqsd\0" "\x0c" "vcmpngt_uqsd\0" \
"\x0e" "vcmpfalse_ossd\0" "\x0c" "vcmpneq_ossd\0" "\x0b" "vcmpge_oqsd\0" \
"\x0b" "vcmpgt_oqsd\0" "\x0d" "vcmptrue_ussd\0" "\x06" "pinsrw\0" "\x07" "vpinsrw\0" \
"\x06" "pextrw\0" "\x07" "vpextrw\0" "\x06" "shufps\0" "\x06" "shufpd\0" \
"\x07" "vshufps\0" "\x07" "vshufpd\0" "\x09" "cmpxchg8b\0" "\x0a" "cmpxchg16b\0" \
"\x07" "vmptrst\0" "\x08" "addsubpd\0" "\x08" "addsubps\0" "\x09" "vaddsubpd\0" \
"\x09" "vaddsubps\0" "\x05" "psrlw\0" "\x06" "vpsrlw\0" "\x05" "psrld\0" \
"\x06" "vpsrld\0" "\x05" "psrlq\0" "\x06" "vpsrlq\0" "\x05" "paddq\0" \
"\x06" "vpaddq\0" "\x06" "pmullw\0" "\x07" "vpmullw\0" "\x07" "movq2dq\0" \
"\x07" "movdq2q\0" "\x08" "pmovmskb\0" "\x09" "vpmovmskb\0" "\x07" "psubusb\0" \
"\x08" "vpsubusb\0" "\x07" "psubusw\0" "\x08" "vpsubusw\0" "\x06" "pminub\0" \
"\x07" "vpminub\0" "\x04" "pand\0" "\x05" "vpand\0" "\x07" "paddusb\0" \
"\x08" "vpaddusw\0" "\x07" "paddusw\0" "\x06" "pmaxub\0" "\x07" "vpmaxub\0" \
"\x05" "pandn\0" "\x06" "vpandn\0" "\x05" "pavgb\0" "\x06" "vpavgb\0" \
"\x05" "psraw\0" "\x06" "vpsraw\0" "\x05" "psrad\0" "\x06" "vpsrad\0" \
"\x05" "pavgw\0" "\x06" "vpavgw\0" "\x07" "pmulhuw\0" "\x08" "vpmulhuw\0" \
"\x06" "pmulhw\0" "\x07" "vpmulhw\0" "\x09" "cvttpd2dq\0" "\x08" "cvtdq2pd\0" \
"\x08" "cvtpd2dq\0" "\x0a" "vcvttpd2dq\0" "\x09" "vcvtdq2pd\0" "\x09" "vcvtpd2dq\0" \
"\x06" "movntq\0" "\x07" "movntdq\0" "\x08" "vmovntdq\0" "\x06" "psubsb\0" \
"\x07" "vpsubsb\0" "\x06" "psubsw\0" "\x07" "vpsubsw\0" "\x06" "pminsw\0" \
"\x07" "vpminsw\0" "\x03" "por\0" "\x04" "vpor\0" "\x06" "paddsb\0" \
"\x07" "vpaddsb\0" "\x06" "paddsw\0" "\x07" "vpaddsw\0" "\x06" "pmaxsw\0" \
"\x07" "vpmaxsw\0" "\x04" "pxor\0" "\x05" "vpxor\0" "\x05" "lddqu\0" \
"\x06" "vlddqu\0" "\x05" "psllw\0" "\x06" "vpsllw\0" "\x05" "pslld\0" \
"\x06" "vpslld\0" "\x05" "psllq\0" "\x06" "vpsllq\0" "\x07" "pmuludq\0" \
"\x08" "vpmuludq\0" "\x07" "pmaddwd\0" "\x08" "vpmaddwd\0" "\x06" "psadbw\0" \
"\x07" "vpsadbw\0" "\x08" "maskmovq\0" "\x0a" "maskmovdqu\0" "\x0b" "vmaskmovdqu\0" \
"\x05" "psubb\0" "\x06" "vpsubb\0" "\x05" "psubw\0" "\x06" "vpsubw\0" \
"\x05" "psubd\0" "\x06" "vpsubd\0" "\x05" "psubq\0" "\x06" "vpsubq\0" \
"\x05" "paddb\0" "\x06" "vpaddb\0" "\x05" "paddw\0" "\x06" "vpaddw\0" \
"\x05" "paddd\0" "\x06" "vpaddd\0" "\x07" "fnstenv\0" "\x06" "fstenv\0" \
"\x06" "fnstcw\0" "\x05" "fstcw\0" "\x06" "fnclex\0" "\x05" "fclex\0" \
"\x06" "fninit\0" "\x05" "finit\0" "\x06" "fnsave\0" "\x05" "fsave\0" \
"\x06" "fnstsw\0" "\x05" "fstsw\0" "\x06" "pshufb\0" "\x07" "vpshufb\0" \
"\x06" "phaddw\0" "\x07" "vphaddw\0" "\x06" "phaddd\0" "\x07" "vphaddd\0" \
"\x07" "phaddsw\0" "\x08" "vphaddsw\0" "\x09" "pmaddubsw\0" "\x0a" "vpmaddubsw\0" \
"\x06" "phsubw\0" "\x07" "vphsubw\0" "\x06" "phsubd\0" "\x07" "vphsubd\0" \
"\x07" "phsubsw\0" "\x08" "vphsubsw\0" "\x06" "psignb\0" "\x07" "vpsignb\0" \
"\x06" "psignw\0" "\x07" "vpsignw\0" "\x06" "psignd\0" "\x07" "vpsignd\0" \
"\x08" "pmulhrsw\0" "\x09" "vpmulhrsw\0" "\x09" "vpermilps\0" "\x09" "vpermilpd\0" \
"\x07" "vtestps\0" "\x07" "vtestpd\0" "\x08" "pblendvb\0" "\x08" "blendvps\0" \
"\x08" "blendvpd\0" "\x05" "ptest\0" "\x06" "vptest\0" "\x0c" "vbroadcastss\0" \
"\x0c" "vbroadcastsd\0" "\x0e" "vbroadcastf128\0" "\x05" "pabsb\0" "\x06" "vpabsb\0" \
"\x05" "pabsw\0" "\x06" "vpabsw\0" "\x05" "pabsd\0" "\x06" "vpabsd\0" \
"\x08" "pmovsxbw\0" "\x09" "vpmovsxbw\0" "\x08" "pmovsxbd\0" "\x09" "vpmovsxbd\0" \
"\x08" "pmovsxbq\0" "\x09" "vpmovsxbq\0" "\x08" "pmovsxwd\0" "\x09" "vpmovsxwd\0" \
"\x08" "pmovsxwq\0" "\x09" "vpmovsxwq\0" "\x08" "pmovsxdq\0" "\x09" "vpmovsxdq\0" \
"\x06" "pmuldq\0" "\x07" "vpmuldq\0" "\x07" "pcmpeqq\0" "\x08" "vpcmpeqq\0" \
"\x08" "movntdqa\0" "\x09" "vmovntdqa\0" "\x08" "packusdw\0" "\x09" "vpackusdw\0" \
"\x0a" "vmaskmovps\0" "\x0a" "vmaskmovpd\0" "\x08" "pmovzxbw\0" "\x09" "vpmovzxbw\0" \
"\x08" "pmovzxbd\0" "\x09" "vpmovzxbd\0" "\x08" "pmovzxbq\0" "\x09" "vpmovzxbq\0" \
"\x08" "pmovzxwd\0" "\x09" "vpmovzxwd\0" "\x08" "pmovzxwq\0" "\x09" "vpmovzxwq\0" \
"\x08" "pmovzxdq\0" "\x09" "vpmovzxdq\0" "\x07" "pcmpgtq\0" "\x08" "vpcmpgtq\0" \
"\x06" "pminsb\0" "\x07" "vpminsb\0" "\x06" "pminsd\0" "\x07" "vpminsd\0" \
"\x06" "pminuw\0" "\x07" "vpminuw\0" "\x06" "pminud\0" "\x07" "vpminud\0" \
"\x06" "pmaxsb\0" "\x07" "vpmaxsb\0" "\x06" "pmaxsd\0" "\x07" "vpmaxsd\0" \
"\x06" "pmaxuw\0" "\x07" "vpmaxuw\0" "\x06" "pmaxud\0" "\x07" "vpmaxud\0" \
"\x06" "pmulld\0" "\x07" "vpmulld\0" "\x0a" "phminposuw\0" "\x0b" "vphminposuw\0" \
"\x06" "invept\0" "\x07" "invvpid\0" "\x07" "invpcid\0" "\x0e" "vfmaddsub132ps\0" \
"\x0e" "vfmaddsub132pd\0" "\x0e" "vfmsubadd132ps\0" "\x0e" "vfmsubadd132pd\0" \
"\x0b" "vfmadd132ps\0" "\x0b" "vfmadd132pd\0" "\x0b" "vfmadd132ss\0" \
"\x0b" "vfmadd132sd\0" "\x0b" "vfmsub132ps\0" "\x0b" "vfmsub132pd\0" \
"\x0b" "vfmsub132ss\0" "\x0b" "vfmsub132sd\0" "\x0c" "vfnmadd132ps\0" \
"\x0c" "vfnmadd132pd\0" "\x0c" "vfnmadd132ss\0" "\x0c" "vfnmadd132sd\0" \
"\x0c" "vfnmsub132ps\0" "\x0c" "vfnmsub132pd\0" "\x0c" "vfnmsub132ss\0" \
"\x0c" "vfnmsub132sd\0" "\x0e" "vfmaddsub213ps\0" "\x0e" "vfmaddsub213pd\0" \
"\x0e" "vfmsubadd213ps\0" "\x0e" "vfmsubadd213pd\0" "\x0b" "vfmadd213ps\0" \
"\x0b" "vfmadd213pd\0" "\x0b" "vfmadd213ss\0" "\x0b" "vfmadd213sd\0" \
"\x0b" "vfmsub213ps\0" "\x0b" "vfmsub213pd\0" "\x0b" "vfmsub213ss\0" \
"\x0b" "vfmsub213sd\0" "\x0c" "vfnmadd213ps\0" "\x0c" "vfnmadd213pd\0" \
"\x0c" "vfnmadd213ss\0" "\x0c" "vfnmadd213sd\0" "\x0c" "vfnmsub213ps\0" \
"\x0c" "vfnmsub213pd\0" "\x0c" "vfnmsub213ss\0" "\x0c" "vfnmsub213sd\0" \
"\x0e" "vfmaddsub231ps\0" "\x0e" "vfmaddsub231pd\0" "\x0e" "vfmsubadd231ps\0" \
"\x0e" "vfmsubadd231pd\0" "\x0b" "vfmadd231ps\0" "\x0b" "vfmadd231pd\0" \
"\x0b" "vfmadd231ss\0" "\x0b" "vfmadd231sd\0" "\x0b" "vfmsub231ps\0" \
"\x0b" "vfmsub231pd\0" "\x0b" "vfmsub231ss\0" "\x0b" "vfmsub231sd\0" \
"\x0c" "vfnmadd231ps\0" "\x0c" "vfnmadd231pd\0" "\x0c" "vfnmadd231ss\0" \
"\x0c" "vfnmadd231sd\0" "\x0c" "vfnmsub231ps\0" "\x0c" "vfnmsub231pd\0" \
"\x0c" "vfnmsub231ss\0" "\x0c" "vfnmsub231sd\0" "\x06" "aesimc\0" "\x07" "vaesimc\0" \
"\x06" "aesenc\0" "\x07" "vaesenc\0" "\x0a" "aesenclast\0" "\x0b" "vaesenclast\0" \
"\x06" "aesdec\0" "\x07" "vaesdec\0" "\x0a" "aesdeclast\0" "\x0b" "vaesdeclast\0" \
"\x05" "movbe\0" "\x05" "crc32\0" "\x0a" "vperm2f128\0" "\x07" "roundps\0" \
"\x08" "vroundps\0" "\x07" "roundpd\0" "\x08" "vroundpd\0" "\x07" "roundss\0" \
"\x08" "vroundss\0" "\x07" "roundsd\0" "\x08" "vroundsd\0" "\x07" "blendps\0" \
"\x08" "vblendps\0" "\x07" "blendpd\0" "\x08" "vblendpd\0" "\x07" "pblendw\0" \
"\x08" "vpblendw\0" "\x07" "palignr\0" "\x08" "vpalignr\0" "\x06" "pextrb\0" \
"\x07" "vpextrb\0" "\x06" "pextrd\0" "\x06" "pextrq\0" "\x07" "vpextrd\0" \
"\x07" "vpextrq\0" "\x09" "extractps\0" "\x0a" "vextractps\0" "\x0b" "vinsertf128\0" \
"\x0c" "vextractf128\0" "\x06" "pinsrb\0" "\x07" "vpinsrb\0" "\x08" "insertps\0" \
"\x09" "vinsertps\0" "\x06" "pinsrd\0" "\x06" "pinsrq\0" "\x07" "vpinsrd\0" \
"\x07" "vpinsrq\0" "\x04" "dpps\0" "\x05" "vdpps\0" "\x04" "dppd\0" \
"\x05" "vdppd\0" "\x07" "mpsadbw\0" "\x08" "vmpsadbw\0" "\x09" "pclmulqdq\0" \
"\x0a" "vpclmulqdq\0" "\x09" "vblendvps\0" "\x09" "vblendvpd\0" "\x09" "vpblendvb\0" \
"\x09" "pcmpestrm\0" "\x0a" "vpcmpestrm\0" "\x09" "pcmpestri\0" "\x0a" "vpcmpestri\0" \
"\x09" "pcmpistrm\0" "\x0a" "vpcmpistrm\0" "\x09" "pcmpistri\0" "\x0a" "vpcmpistri\0" \
"\x0f" "aeskeygenassist\0" "\x10" "vaeskeygenassist\0" "\x06" "psrldq\0" \
"\x07" "vpsrldq\0" "\x06" "pslldq\0" "\x07" "vpslldq\0" "\x06" "fxsave\0" \
"\x08" "fxsave64\0" "\x08" "rdfsbase\0" "\x07" "fxrstor\0" "\x09" "fxrstor64\0" \
"\x08" "rdgsbase\0" "\x07" "ldmxcsr\0" "\x08" "wrfsbase\0" "\x08" "vldmxcsr\0" \
"\x07" "stmxcsr\0" "\x08" "wrgsbase\0" "\x08" "vstmxcsr\0" "\x07" "vmptrld\0" \
"\x07" "vmclear\0" "\x05" "vmxon\0" "\x06" "movsxd\0" "\x05" "pause\0" \
"\x04" "wait\0" "\x06" "rdrand\0" "\x06" "_3dnow\0";

const _WRegister _REGISTERS[] = {
	{ 3, "rax" }, { 3, "rcx" }, { 3, "rdx" }, { 3, "rbx" }, { 3, "rsp" }, { 3, "rbp" }, { 3, "rsi" }, { 3, "rdi" }, { 2, "r8" }, { 2, "r9" }, { 3, "r10" }, { 3, "r11" }, { 3, "r12" }, { 3, "r13" }, { 3, "r14" }, { 3, "r15" },
	{ 3, "eax" }, { 3, "ecx" }, { 3, "edx" }, { 3, "ebx" }, { 3, "esp" }, { 3, "ebp" }, { 3, "esi" }, { 3, "edi" }, { 3, "r8d" }, { 3, "r9d" }, { 4, "r10d" }, { 4, "r11d" }, { 4, "r12d" }, { 4, "r13d" }, { 4, "r14d" }, { 4, "r15d" },
	{ 2, "ax" }, { 2, "cx" }, { 2, "dx" }, { 2, "bx" }, { 2, "sp" }, { 2, "bp" }, { 2, "si" }, { 2, "di" }, { 3, "r8w" }, { 3, "r9w" }, { 4, "r10w" }, { 4, "r11w" }, { 4, "r12w" }, { 4, "r13w" }, { 4, "r14w" }, { 4, "r15w" },
	{ 2, "al" }, { 2, "cl" }, { 2, "dl" }, { 2, "bl" }, { 2, "ah" }, { 2, "ch" }, { 2, "dh" }, { 2, "bh" }, { 3, "r8b" }, { 3, "r9b" }, { 4, "r10b" }, { 4, "r11b" }, { 4, "r12b" }, { 4, "r13b" }, { 4, "r14b" }, { 4, "r15b" },
	{ 3, "spl" }, { 3, "bpl" }, { 3, "sil" }, { 3, "dil" },
	{ 2, "es" }, { 2, "cs" }, { 2, "ss" }, { 2, "ds" }, { 2, "fs" }, { 2, "gs" },
	{ 3, "rip" },
	{ 3, "st0" }, { 3, "st1" }, { 3, "st2" }, { 3, "st3" }, { 3, "st4" }, { 3, "st5" }, { 3, "st6" }, { 3, "st7" },
	{ 3, "mm0" }, { 3, "mm1" }, { 3, "mm2" }, { 3, "mm3" }, { 3, "mm4" }, { 3, "mm5" }, { 3, "mm6" }, { 3, "mm7" },
	{ 4, "xmm0" }, { 4, "xmm1" }, { 4, "xmm2" }, { 4, "xmm3" }, { 4, "xmm4" }, { 4, "xmm5" }, { 4, "xmm6" }, { 4, "xmm7" }, { 4, "xmm8" }, { 4, "xmm9" }, { 5, "xmm10" }, { 5, "xmm11" }, { 5, "xmm12" }, { 5, "xmm13" }, { 5, "xmm14" }, { 5, "xmm15" },
	{ 4, "ymm0" }, { 4, "ymm1" }, { 4, "ymm2" }, { 4, "ymm3" }, { 4, "ymm4" }, { 4, "ymm5" }, { 4, "ymm6" }, { 4, "ymm7" }, { 4, "ymm8" }, { 4, "ymm9" }, { 5, "ymm10" }, { 5, "ymm11" }, { 5, "ymm12" }, { 5, "ymm13" }, { 5, "ymm14" }, { 5, "ymm15" },
	{ 3, "cr0" }, { 0, "" }, { 3, "cr2" }, { 3, "cr3" }, { 3, "cr4" }, { 0, "" }, { 0, "" }, { 0, "" }, { 3, "cr8" },
	{ 3, "dr0" }, { 3, "dr1" }, { 3, "dr2" }, { 3, "dr3" }, { 0, "" }, { 0, "" }, { 3, "dr6" }, { 3, "dr7" }
};

#endif /* DISTORM_LIGHT */
