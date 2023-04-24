WORKDIR = `pwd`

CC = gcc
CXX = gcc
AR = ar
LD = g++
WINDRES = windres

INC = -I../Dependencies/distorm/include
CFLAGS32 = -Wall -fPIC -m32
CFLAGS64 = -Wall -fPIC -m64 -DRECLASSNET64=1
RESINC =
LIBDIR =
LIB = -lstdc++fs -lstdc++
LDFLAGS32 = -m32 -shared -Wl,--no-undefined
LDFLAGS64 = -m64 -shared -Wl,--no-undefined

INC_DEBUG = $(INC)
CFLAGS32_DEBUG = $(CFLAGS32) -g
CFLAGS64_DEBUG = $(CFLAGS64) -g
RESINC_DEBUG = $(RESINC)
RCFLAGS_DEBUG = $(RCFLAGS)
LIBDIR_DEBUG = $(LIBDIR)
LIB_DEBUG = $(LIB)
LDFLAGS32_DEBUG = $(LDFLAGS32)
LDFLAGS64_DEBUG = $(LDFLAGS64)
OBJDIR32_DEBUG = obj/debug/x86
OBJDIR64_DEBUG = obj/debug/x64
DEP_DEBUG =
OUT32_DEBUG = build/debug/x86/NativeCore.so
OUT64_DEBUG = build/debug/x64/NativeCore.so

INC_RELEASE = $(INC)
CFLAGS32_RELEASE = $(CFLAGS32) -O2
CFLAGS64_RELEASE = $(CFLAGS64) -O2
RESINC_RELEASE = $(RESINC)
RCFLAGS_RELEASE = $(RCFLAGS)
LIBDIR_RELEASE = $(LIBDIR)
LIB_RELEASE = $(LIB)
LDFLAGS32_RELEASE = $(LDFLAGS32) -s
LDFLAGS64_RELEASE = $(LDFLAGS64) -s
OBJDIR32_RELEASE = obj/release/x86
OBJDIR64_RELEASE = obj/release/x64
DEP_RELEASE =
OUT32_RELEASE = build/release/x86/NativeCore.so
OUT64_RELEASE = build/release/x64/NativeCore.so

OBJ32_DEBUG = $(OBJDIR32_DEBUG)/WriteRemoteMemory.o $(OBJDIR32_DEBUG)/ReadRemoteMemory.o $(OBJDIR32_DEBUG)/OpenRemoteProcess.o $(OBJDIR32_DEBUG)/IsProcessValid.o $(OBJDIR32_DEBUG)/Input.o $(OBJDIR32_DEBUG)/EnumerateRemoteSectionsAndModules.o $(OBJDIR32_DEBUG)/EnumerateProcesses.o $(OBJDIR32_DEBUG)/DisassembleCode.o $(OBJDIR32_DEBUG)/DistormHelper.o  $(OBJDIR32_DEBUG)/Debugger.o $(OBJDIR32_DEBUG)/ControlRemoteProcess.o $(OBJDIR32_DEBUG)/CloseRemoteProcess.o $(OBJDIR32_DEBUG)/decoder.o $(OBJDIR32_DEBUG)/distorm.o $(OBJDIR32_DEBUG)/instructions.o $(OBJDIR32_DEBUG)/insts.o $(OBJDIR32_DEBUG)/mnemonics.o $(OBJDIR32_DEBUG)/operands.o $(OBJDIR32_DEBUG)/prefix.o $(OBJDIR32_DEBUG)/textdefs.o
OBJ64_DEBUG = $(OBJDIR64_DEBUG)/WriteRemoteMemory.o $(OBJDIR64_DEBUG)/ReadRemoteMemory.o $(OBJDIR64_DEBUG)/OpenRemoteProcess.o $(OBJDIR64_DEBUG)/IsProcessValid.o $(OBJDIR64_DEBUG)/Input.o $(OBJDIR64_DEBUG)/EnumerateRemoteSectionsAndModules.o $(OBJDIR64_DEBUG)/EnumerateProcesses.o $(OBJDIR64_DEBUG)/DisassembleCode.o $(OBJDIR64_DEBUG)/DistormHelper.o  $(OBJDIR64_DEBUG)/Debugger.o $(OBJDIR64_DEBUG)/ControlRemoteProcess.o $(OBJDIR64_DEBUG)/CloseRemoteProcess.o $(OBJDIR64_DEBUG)/decoder.o $(OBJDIR64_DEBUG)/distorm.o $(OBJDIR64_DEBUG)/instructions.o $(OBJDIR64_DEBUG)/insts.o $(OBJDIR64_DEBUG)/mnemonics.o $(OBJDIR64_DEBUG)/operands.o $(OBJDIR64_DEBUG)/prefix.o $(OBJDIR64_DEBUG)/textdefs.o

OBJ32_RELEASE = $(OBJDIR32_RELEASE)/WriteRemoteMemory.o $(OBJDIR32_RELEASE)/ReadRemoteMemory.o $(OBJDIR32_RELEASE)/OpenRemoteProcess.o $(OBJDIR32_RELEASE)/IsProcessValid.o $(OBJDIR32_RELEASE)/Input.o $(OBJDIR32_RELEASE)/EnumerateRemoteSectionsAndModules.o $(OBJDIR32_RELEASE)/EnumerateProcesses.o $(OBJDIR32_RELEASE)/DisassembleCode.o $(OBJDIR32_RELEASE)/DistormHelper.o $(OBJDIR32_RELEASE)/Debugger.o $(OBJDIR32_RELEASE)/ControlRemoteProcess.o $(OBJDIR32_RELEASE)/CloseRemoteProcess.o $(OBJDIR32_RELEASE)/decoder.o $(OBJDIR32_RELEASE)/distorm.o $(OBJDIR32_RELEASE)/instructions.o $(OBJDIR32_RELEASE)/insts.o $(OBJDIR32_RELEASE)/mnemonics.o $(OBJDIR32_RELEASE)/operands.o $(OBJDIR32_RELEASE)/prefix.o $(OBJDIR32_RELEASE)/textdefs.o
OBJ64_RELEASE = $(OBJDIR64_RELEASE)/WriteRemoteMemory.o $(OBJDIR64_RELEASE)/ReadRemoteMemory.o $(OBJDIR64_RELEASE)/OpenRemoteProcess.o $(OBJDIR64_RELEASE)/IsProcessValid.o $(OBJDIR64_RELEASE)/Input.o $(OBJDIR64_RELEASE)/EnumerateRemoteSectionsAndModules.o $(OBJDIR64_RELEASE)/EnumerateProcesses.o $(OBJDIR64_RELEASE)/DisassembleCode.o $(OBJDIR64_RELEASE)/DistormHelper.o $(OBJDIR64_RELEASE)/Debugger.o $(OBJDIR64_RELEASE)/ControlRemoteProcess.o $(OBJDIR64_RELEASE)/CloseRemoteProcess.o $(OBJDIR64_RELEASE)/decoder.o $(OBJDIR64_RELEASE)/distorm.o $(OBJDIR64_RELEASE)/instructions.o $(OBJDIR64_RELEASE)/insts.o $(OBJDIR64_RELEASE)/mnemonics.o $(OBJDIR64_RELEASE)/operands.o $(OBJDIR64_RELEASE)/prefix.o $(OBJDIR64_RELEASE)/textdefs.o

all: debug release

clean: clean_debug clean_release

before_debug:
	test -d build/debug/x86 || mkdir -p build/debug/x86
	test -d build/debug/x64 || mkdir -p build/debug/x64
	test -d $(OBJDIR32_DEBUG) || mkdir -p $(OBJDIR32_DEBUG)
	test -d $(OBJDIR64_DEBUG) || mkdir -p $(OBJDIR64_DEBUG)

after_debug:

debug: before_debug out_debug32 out_debug64 after_debug

out_debug32: before_debug $(OBJ32_DEBUG) $(DEP_DEBUG)
	$(CXX) $(LIBDIR_DEBUG) -o $(OUT32_DEBUG) $(OBJ32_DEBUG) $(LDFLAGS32_DEBUG) $(LIB_DEBUG)

$(OBJDIR32_DEBUG)/WriteRemoteMemory.o: WriteRemoteMemory.cpp
	$(CXX) $(CFLAGS32_DEBUG) $(INC_DEBUG) -c WriteRemoteMemory.cpp -o $(OBJDIR32_DEBUG)/WriteRemoteMemory.o

$(OBJDIR32_DEBUG)/ReadRemoteMemory.o: ReadRemoteMemory.cpp
	$(CXX) $(CFLAGS32_DEBUG) $(INC_DEBUG) -c ReadRemoteMemory.cpp -o $(OBJDIR32_DEBUG)/ReadRemoteMemory.o

$(OBJDIR32_DEBUG)/OpenRemoteProcess.o: OpenRemoteProcess.cpp
	$(CXX) $(CFLAGS32_DEBUG) $(INC_DEBUG) -c OpenRemoteProcess.cpp -o $(OBJDIR32_DEBUG)/OpenRemoteProcess.o

$(OBJDIR32_DEBUG)/IsProcessValid.o: IsProcessValid.cpp
	$(CXX) $(CFLAGS32_DEBUG) $(INC_DEBUG) -c IsProcessValid.cpp -o $(OBJDIR32_DEBUG)/IsProcessValid.o

$(OBJDIR32_DEBUG)/Input.o: Input.cpp
	$(CXX) $(CFLAGS32_DEBUG) $(INC_DEBUG) -c Input.cpp -o $(OBJDIR32_DEBUG)/Input.o

$(OBJDIR32_DEBUG)/EnumerateRemoteSectionsAndModules.o: EnumerateRemoteSectionsAndModules.cpp
	$(CXX) $(CFLAGS32_DEBUG) $(INC_DEBUG) -c EnumerateRemoteSectionsAndModules.cpp -o $(OBJDIR32_DEBUG)/EnumerateRemoteSectionsAndModules.o

$(OBJDIR32_DEBUG)/EnumerateProcesses.o: EnumerateProcesses.cpp
	$(CXX) $(CFLAGS32_DEBUG) $(INC_DEBUG) -c EnumerateProcesses.cpp -o $(OBJDIR32_DEBUG)/EnumerateProcesses.o

$(OBJDIR32_DEBUG)/DisassembleCode.o: DisassembleCode.cpp
	$(CXX) $(CFLAGS32_DEBUG) $(INC_DEBUG) -c DisassembleCode.cpp -o $(OBJDIR32_DEBUG)/DisassembleCode.o

$(OBJDIR32_DEBUG)/DistormHelper.o: ../Shared/DistormHelper.cpp
	$(CXX) $(CFLAGS32_DEBUG) $(INC_DEBUG) -c ../Shared/DistormHelper.cpp -o $(OBJDIR32_DEBUG)/DistormHelper.o

$(OBJDIR32_DEBUG)/Debugger.o: Debugger.cpp
	$(CXX) $(CFLAGS32_DEBUG) $(INC_DEBUG) -c Debugger.cpp -o $(OBJDIR32_DEBUG)/Debugger.o

$(OBJDIR32_DEBUG)/ControlRemoteProcess.o: ControlRemoteProcess.cpp
	$(CXX) $(CFLAGS32_DEBUG) $(INC_DEBUG) -c ControlRemoteProcess.cpp -o $(OBJDIR32_DEBUG)/ControlRemoteProcess.o

$(OBJDIR32_DEBUG)/CloseRemoteProcess.o: CloseRemoteProcess.cpp
	$(CXX) $(CFLAGS32_DEBUG) $(INC_DEBUG) -c CloseRemoteProcess.cpp -o $(OBJDIR32_DEBUG)/CloseRemoteProcess.o

$(OBJDIR32_DEBUG)/decoder.o: ../Dependencies/distorm/src/decoder.c
	$(CXX) $(CFLAGS32_DEBUG) $(INC_DEBUG) -c ../Dependencies/distorm/src/decoder.c -o $(OBJDIR32_DEBUG)/decoder.o

$(OBJDIR32_DEBUG)/distorm.o: ../Dependencies/distorm/src/distorm.c
	$(CXX) $(CFLAGS32_DEBUG) $(INC_DEBUG) -c ../Dependencies/distorm/src/distorm.c -o $(OBJDIR32_DEBUG)/distorm.o

$(OBJDIR32_DEBUG)/instructions.o: ../Dependencies/distorm/src/instructions.c
	$(CXX) $(CFLAGS32_DEBUG) $(INC_DEBUG) -c ../Dependencies/distorm/src/instructions.c -o $(OBJDIR32_DEBUG)/instructions.o

$(OBJDIR32_DEBUG)/insts.o: ../Dependencies/distorm/src/insts.c
	$(CXX) $(CFLAGS32_DEBUG) $(INC_DEBUG) -c ../Dependencies/distorm/src/insts.c -o $(OBJDIR32_DEBUG)/insts.o

$(OBJDIR32_DEBUG)/mnemonics.o: ../Dependencies/distorm/src/mnemonics.c
	$(CXX) $(CFLAGS32_DEBUG) $(INC_DEBUG) -c ../Dependencies/distorm/src/mnemonics.c -o $(OBJDIR32_DEBUG)/mnemonics.o

$(OBJDIR32_DEBUG)/operands.o: ../Dependencies/distorm/src/operands.c
	$(CXX) $(CFLAGS32_DEBUG) $(INC_DEBUG) -c ../Dependencies/distorm/src/operands.c -o $(OBJDIR32_DEBUG)/operands.o

$(OBJDIR32_DEBUG)/prefix.o: ../Dependencies/distorm/src/prefix.c
	$(CXX) $(CFLAGS32_DEBUG) $(INC_DEBUG) -c ../Dependencies/distorm/src/prefix.c -o $(OBJDIR32_DEBUG)/prefix.o

$(OBJDIR32_DEBUG)/textdefs.o: ../Dependencies/distorm/src/textdefs.c
	$(CXX) $(CFLAGS32_DEBUG) $(INC_DEBUG) -c ../Dependencies/distorm/src/textdefs.c -o $(OBJDIR32_DEBUG)/textdefs.o

out_debug64: before_debug $(OBJ64_DEBUG) $(DEP_DEBUG)
	$(CXX) $(LIBDIR_DEBUG) -o $(OUT64_DEBUG) $(OBJ64_DEBUG) $(LDFLAGS64_DEBUG) $(LIB_DEBUG)

$(OBJDIR64_DEBUG)/WriteRemoteMemory.o: WriteRemoteMemory.cpp
	$(CXX) $(CFLAGS64_DEBUG) $(INC_DEBUG) -c WriteRemoteMemory.cpp -o $(OBJDIR64_DEBUG)/WriteRemoteMemory.o

$(OBJDIR64_DEBUG)/ReadRemoteMemory.o: ReadRemoteMemory.cpp
	$(CXX) $(CFLAGS64_DEBUG) $(INC_DEBUG) -c ReadRemoteMemory.cpp -o $(OBJDIR64_DEBUG)/ReadRemoteMemory.o

$(OBJDIR64_DEBUG)/OpenRemoteProcess.o: OpenRemoteProcess.cpp
	$(CXX) $(CFLAGS64_DEBUG) $(INC_DEBUG) -c OpenRemoteProcess.cpp -o $(OBJDIR64_DEBUG)/OpenRemoteProcess.o

$(OBJDIR64_DEBUG)/IsProcessValid.o: IsProcessValid.cpp
	$(CXX) $(CFLAGS64_DEBUG) $(INC_DEBUG) -c IsProcessValid.cpp -o $(OBJDIR64_DEBUG)/IsProcessValid.o

$(OBJDIR64_DEBUG)/Input.o: Input.cpp
	$(CXX) $(CFLAGS64_DEBUG) $(INC_DEBUG) -c Input.cpp -o $(OBJDIR64_DEBUG)/Input.o

$(OBJDIR64_DEBUG)/EnumerateRemoteSectionsAndModules.o: EnumerateRemoteSectionsAndModules.cpp
	$(CXX) $(CFLAGS64_DEBUG) $(INC_DEBUG) -c EnumerateRemoteSectionsAndModules.cpp -o $(OBJDIR64_DEBUG)/EnumerateRemoteSectionsAndModules.o

$(OBJDIR64_DEBUG)/EnumerateProcesses.o: EnumerateProcesses.cpp
	$(CXX) $(CFLAGS64_DEBUG) $(INC_DEBUG) -c EnumerateProcesses.cpp -o $(OBJDIR64_DEBUG)/EnumerateProcesses.o

$(OBJDIR64_DEBUG)/DisassembleCode.o: DisassembleCode.cpp
	$(CXX) $(CFLAGS64_DEBUG) $(INC_DEBUG) -c DisassembleCode.cpp -o $(OBJDIR64_DEBUG)/DisassembleCode.o

$(OBJDIR64_DEBUG)/DistormHelper.o: ../Shared/DistormHelper.cpp
	$(CXX) $(CFLAGS64_DEBUG) $(INC_DEBUG) -c ../Shared/DistormHelper.cpp -o $(OBJDIR64_DEBUG)/DistormHelper.o

$(OBJDIR64_DEBUG)/Debugger.o: Debugger.cpp
	$(CXX) $(CFLAGS64_DEBUG) $(INC_DEBUG) -c Debugger.cpp -o $(OBJDIR64_DEBUG)/Debugger.o

$(OBJDIR64_DEBUG)/ControlRemoteProcess.o: ControlRemoteProcess.cpp
	$(CXX) $(CFLAGS64_DEBUG) $(INC_DEBUG) -c ControlRemoteProcess.cpp -o $(OBJDIR64_DEBUG)/ControlRemoteProcess.o

$(OBJDIR64_DEBUG)/CloseRemoteProcess.o: CloseRemoteProcess.cpp
	$(CXX) $(CFLAGS64_DEBUG) $(INC_DEBUG) -c CloseRemoteProcess.cpp -o $(OBJDIR64_DEBUG)/CloseRemoteProcess.o

$(OBJDIR64_DEBUG)/decoder.o: ../Dependencies/distorm/src/decoder.c
	$(CXX) $(CFLAGS64_DEBUG) $(INC_DEBUG) -c ../Dependencies/distorm/src/decoder.c -o $(OBJDIR64_DEBUG)/decoder.o

$(OBJDIR64_DEBUG)/distorm.o: ../Dependencies/distorm/src/distorm.c
	$(CXX) $(CFLAGS64_DEBUG) $(INC_DEBUG) -c ../Dependencies/distorm/src/distorm.c -o $(OBJDIR64_DEBUG)/distorm.o

$(OBJDIR64_DEBUG)/instructions.o: ../Dependencies/distorm/src/instructions.c
	$(CXX) $(CFLAGS64_DEBUG) $(INC_DEBUG) -c ../Dependencies/distorm/src/instructions.c -o $(OBJDIR64_DEBUG)/instructions.o

$(OBJDIR64_DEBUG)/insts.o: ../Dependencies/distorm/src/insts.c
	$(CXX) $(CFLAGS64_DEBUG) $(INC_DEBUG) -c ../Dependencies/distorm/src/insts.c -o $(OBJDIR64_DEBUG)/insts.o

$(OBJDIR64_DEBUG)/mnemonics.o: ../Dependencies/distorm/src/mnemonics.c
	$(CXX) $(CFLAGS64_DEBUG) $(INC_DEBUG) -c ../Dependencies/distorm/src/mnemonics.c -o $(OBJDIR64_DEBUG)/mnemonics.o

$(OBJDIR64_DEBUG)/operands.o: ../Dependencies/distorm/src/operands.c
	$(CXX) $(CFLAGS64_DEBUG) $(INC_DEBUG) -c ../Dependencies/distorm/src/operands.c -o $(OBJDIR64_DEBUG)/operands.o

$(OBJDIR64_DEBUG)/prefix.o: ../Dependencies/distorm/src/prefix.c
	$(CXX) $(CFLAGS64_DEBUG) $(INC_DEBUG) -c ../Dependencies/distorm/src/prefix.c -o $(OBJDIR64_DEBUG)/prefix.o

$(OBJDIR64_DEBUG)/textdefs.o: ../Dependencies/distorm/src/textdefs.c
	$(CXX) $(CFLAGS64_DEBUG) $(INC_DEBUG) -c ../Dependencies/distorm/src/textdefs.c -o $(OBJDIR64_DEBUG)/textdefs.o

clean_debug:
	rm -f $(OBJ32_DEBUG) $(OUT32_DEBUG)
	rm -f $(OBJ64_DEBUG) $(OUT64_DEBUG)
	rm -rf build/debug
	rm -rf $(OBJDIR32_DEBUG)
	rm -rf $(OBJDIR64_DEBUG)

before_release:
	test -d build/release/x86 || mkdir -p build/release/x86
	test -d build/release/x64 || mkdir -p build/release/x64
	test -d $(OBJDIR32_RELEASE) || mkdir -p $(OBJDIR32_RELEASE)
	test -d $(OBJDIR64_RELEASE) || mkdir -p $(OBJDIR64_RELEASE)

after_release:

release: before_release out_release32 out_release64 after_release

out_release32: before_release $(OBJ32_RELEASE) $(DEP_RELEASE)
	$(CXX) $(LIBDIR_RELEASE) -o $(OUT32_RELEASE) $(OBJ32_RELEASE) $(LDFLAGS32_RELEASE) $(LIB_RELEASE)

$(OBJDIR32_RELEASE)/WriteRemoteMemory.o: WriteRemoteMemory.cpp
	$(CXX) $(CFLAGS32_RELEASE) $(INC_RELEASE) -c WriteRemoteMemory.cpp -o $(OBJDIR32_RELEASE)/WriteRemoteMemory.o

$(OBJDIR32_RELEASE)/ReadRemoteMemory.o: ReadRemoteMemory.cpp
	$(CXX) $(CFLAGS32_RELEASE) $(INC_RELEASE) -c ReadRemoteMemory.cpp -o $(OBJDIR32_RELEASE)/ReadRemoteMemory.o

$(OBJDIR32_RELEASE)/OpenRemoteProcess.o: OpenRemoteProcess.cpp
	$(CXX) $(CFLAGS32_RELEASE) $(INC_RELEASE) -c OpenRemoteProcess.cpp -o $(OBJDIR32_RELEASE)/OpenRemoteProcess.o

$(OBJDIR32_RELEASE)/IsProcessValid.o: IsProcessValid.cpp
	$(CXX) $(CFLAGS32_RELEASE) $(INC_RELEASE) -c IsProcessValid.cpp -o $(OBJDIR32_RELEASE)/IsProcessValid.o

$(OBJDIR32_RELEASE)/Input.o: Input.cpp
	$(CXX) $(CFLAGS32_RELEASE) $(INC_RELEASE) -c Input.cpp -o $(OBJDIR32_RELEASE)/Input.o

$(OBJDIR32_RELEASE)/EnumerateRemoteSectionsAndModules.o: EnumerateRemoteSectionsAndModules.cpp
	$(CXX) $(CFLAGS32_RELEASE) $(INC_RELEASE) -c EnumerateRemoteSectionsAndModules.cpp -o $(OBJDIR32_RELEASE)/EnumerateRemoteSectionsAndModules.o

$(OBJDIR32_RELEASE)/EnumerateProcesses.o: EnumerateProcesses.cpp
	$(CXX) $(CFLAGS32_RELEASE) $(INC_RELEASE) -c EnumerateProcesses.cpp -o $(OBJDIR32_RELEASE)/EnumerateProcesses.o

$(OBJDIR32_RELEASE)/DisassembleCode.o: DisassembleCode.cpp
	$(CXX) $(CFLAGS32_RELEASE) $(INC_RELEASE) -c DisassembleCode.cpp -o $(OBJDIR32_RELEASE)/DisassembleCode.o

$(OBJDIR32_RELEASE)/DistormHelper.o: ../Shared/DistormHelper.cpp
	$(CXX) $(CFLAGS32_RELEASE) $(INC_RELEASE) -c ../Shared/DistormHelper.cpp -o $(OBJDIR32_RELEASE)/DistormHelper.o

$(OBJDIR32_RELEASE)/Debugger.o: Debugger.cpp
	$(CXX) $(CFLAGS32_RELEASE) $(INC_RELEASE) -c Debugger.cpp -o $(OBJDIR32_RELEASE)/Debugger.o

$(OBJDIR32_RELEASE)/ControlRemoteProcess.o: ControlRemoteProcess.cpp
	$(CXX) $(CFLAGS32_RELEASE) $(INC_RELEASE) -c ControlRemoteProcess.cpp -o $(OBJDIR32_RELEASE)/ControlRemoteProcess.o

$(OBJDIR32_RELEASE)/CloseRemoteProcess.o: CloseRemoteProcess.cpp
	$(CXX) $(CFLAGS32_RELEASE) $(INC_RELEASE) -c CloseRemoteProcess.cpp -o $(OBJDIR32_RELEASE)/CloseRemoteProcess.o

$(OBJDIR32_RELEASE)/decoder.o: ../Dependencies/distorm/src/decoder.c
	$(CXX) $(CFLAGS32_RELEASE) $(INC_RELEASE) -c ../Dependencies/distorm/src/decoder.c -o $(OBJDIR32_RELEASE)/decoder.o

$(OBJDIR32_RELEASE)/distorm.o: ../Dependencies/distorm/src/distorm.c
	$(CXX) $(CFLAGS32_RELEASE) $(INC_RELEASE) -c ../Dependencies/distorm/src/distorm.c -o $(OBJDIR32_RELEASE)/distorm.o

$(OBJDIR32_RELEASE)/instructions.o: ../Dependencies/distorm/src/instructions.c
	$(CXX) $(CFLAGS32_RELEASE) $(INC_RELEASE) -c ../Dependencies/distorm/src/instructions.c -o $(OBJDIR32_RELEASE)/instructions.o

$(OBJDIR32_RELEASE)/insts.o: ../Dependencies/distorm/src/insts.c
	$(CXX) $(CFLAGS32_RELEASE) $(INC_RELEASE) -c ../Dependencies/distorm/src/insts.c -o $(OBJDIR32_RELEASE)/insts.o

$(OBJDIR32_RELEASE)/mnemonics.o: ../Dependencies/distorm/src/mnemonics.c
	$(CXX) $(CFLAGS32_RELEASE) $(INC_RELEASE) -c ../Dependencies/distorm/src/mnemonics.c -o $(OBJDIR32_RELEASE)/mnemonics.o

$(OBJDIR32_RELEASE)/operands.o: ../Dependencies/distorm/src/operands.c
	$(CXX) $(CFLAGS32_RELEASE) $(INC_RELEASE) -c ../Dependencies/distorm/src/operands.c -o $(OBJDIR32_RELEASE)/operands.o

$(OBJDIR32_RELEASE)/prefix.o: ../Dependencies/distorm/src/prefix.c
	$(CXX) $(CFLAGS32_RELEASE) $(INC_RELEASE) -c ../Dependencies/distorm/src/prefix.c -o $(OBJDIR32_RELEASE)/prefix.o

$(OBJDIR32_RELEASE)/textdefs.o: ../Dependencies/distorm/src/textdefs.c
	$(CXX) $(CFLAGS32_RELEASE) $(INC_RELEASE) -c ../Dependencies/distorm/src/textdefs.c -o $(OBJDIR32_RELEASE)/textdefs.o

out_release64: before_release $(OBJ64_RELEASE) $(DEP_RELEASE)
	$(CXX) $(LIBDIR_RELEASE) -o $(OUT64_RELEASE) $(OBJ64_RELEASE) $(LDFLAGS64_RELEASE) $(LIB_RELEASE)

$(OBJDIR64_RELEASE)/WriteRemoteMemory.o: WriteRemoteMemory.cpp
	$(CXX) $(CFLAGS64_RELEASE) $(INC_RELEASE) -c WriteRemoteMemory.cpp -o $(OBJDIR64_RELEASE)/WriteRemoteMemory.o

$(OBJDIR64_RELEASE)/ReadRemoteMemory.o: ReadRemoteMemory.cpp
	$(CXX) $(CFLAGS64_RELEASE) $(INC_RELEASE) -c ReadRemoteMemory.cpp -o $(OBJDIR64_RELEASE)/ReadRemoteMemory.o

$(OBJDIR64_RELEASE)/OpenRemoteProcess.o: OpenRemoteProcess.cpp
	$(CXX) $(CFLAGS64_RELEASE) $(INC_RELEASE) -c OpenRemoteProcess.cpp -o $(OBJDIR64_RELEASE)/OpenRemoteProcess.o

$(OBJDIR64_RELEASE)/IsProcessValid.o: IsProcessValid.cpp
	$(CXX) $(CFLAGS64_RELEASE) $(INC_RELEASE) -c IsProcessValid.cpp -o $(OBJDIR64_RELEASE)/IsProcessValid.o

$(OBJDIR64_RELEASE)/Input.o: Input.cpp
	$(CXX) $(CFLAGS64_RELEASE) $(INC_RELEASE) -c Input.cpp -o $(OBJDIR64_RELEASE)/Input.o

$(OBJDIR64_RELEASE)/EnumerateRemoteSectionsAndModules.o: EnumerateRemoteSectionsAndModules.cpp
	$(CXX) $(CFLAGS64_RELEASE) $(INC_RELEASE) -c EnumerateRemoteSectionsAndModules.cpp -o $(OBJDIR64_RELEASE)/EnumerateRemoteSectionsAndModules.o

$(OBJDIR64_RELEASE)/EnumerateProcesses.o: EnumerateProcesses.cpp
	$(CXX) $(CFLAGS64_RELEASE) $(INC_RELEASE) -c EnumerateProcesses.cpp -o $(OBJDIR64_RELEASE)/EnumerateProcesses.o

$(OBJDIR64_RELEASE)/DisassembleCode.o: DisassembleCode.cpp
	$(CXX) $(CFLAGS64_RELEASE) $(INC_RELEASE) -c DisassembleCode.cpp -o $(OBJDIR64_RELEASE)/DisassembleCode.o

$(OBJDIR64_RELEASE)/DistormHelper.o: ../Shared/DistormHelper.cpp
	$(CXX) $(CFLAGS64_RELEASE) $(INC_RELEASE) -c ../Shared/DistormHelper.cpp -o $(OBJDIR64_RELEASE)/DistormHelper.o

$(OBJDIR64_RELEASE)/Debugger.o: Debugger.cpp
	$(CXX) $(CFLAGS64_RELEASE) $(INC_RELEASE) -c Debugger.cpp -o $(OBJDIR64_RELEASE)/Debugger.o

$(OBJDIR64_RELEASE)/ControlRemoteProcess.o: ControlRemoteProcess.cpp
	$(CXX) $(CFLAGS64_RELEASE) $(INC_RELEASE) -c ControlRemoteProcess.cpp -o $(OBJDIR64_RELEASE)/ControlRemoteProcess.o

$(OBJDIR64_RELEASE)/CloseRemoteProcess.o: CloseRemoteProcess.cpp
	$(CXX) $(CFLAGS64_RELEASE) $(INC_RELEASE) -c CloseRemoteProcess.cpp -o $(OBJDIR64_RELEASE)/CloseRemoteProcess.o

$(OBJDIR64_RELEASE)/decoder.o: ../Dependencies/distorm/src/decoder.c
	$(CXX) $(CFLAGS64_RELEASE) $(INC_RELEASE) -c ../Dependencies/distorm/src/decoder.c -o $(OBJDIR64_RELEASE)/decoder.o

$(OBJDIR64_RELEASE)/distorm.o: ../Dependencies/distorm/src/distorm.c
	$(CXX) $(CFLAGS64_RELEASE) $(INC_RELEASE) -c ../Dependencies/distorm/src/distorm.c -o $(OBJDIR64_RELEASE)/distorm.o

$(OBJDIR64_RELEASE)/instructions.o: ../Dependencies/distorm/src/instructions.c
	$(CXX) $(CFLAGS64_RELEASE) $(INC_RELEASE) -c ../Dependencies/distorm/src/instructions.c -o $(OBJDIR64_RELEASE)/instructions.o

$(OBJDIR64_RELEASE)/insts.o: ../Dependencies/distorm/src/insts.c
	$(CXX) $(CFLAGS64_RELEASE) $(INC_RELEASE) -c ../Dependencies/distorm/src/insts.c -o $(OBJDIR64_RELEASE)/insts.o

$(OBJDIR64_RELEASE)/mnemonics.o: ../Dependencies/distorm/src/mnemonics.c
	$(CXX) $(CFLAGS64_RELEASE) $(INC_RELEASE) -c ../Dependencies/distorm/src/mnemonics.c -o $(OBJDIR64_RELEASE)/mnemonics.o

$(OBJDIR64_RELEASE)/operands.o: ../Dependencies/distorm/src/operands.c
	$(CXX) $(CFLAGS64_RELEASE) $(INC_RELEASE) -c ../Dependencies/distorm/src/operands.c -o $(OBJDIR64_RELEASE)/operands.o

$(OBJDIR64_RELEASE)/prefix.o: ../Dependencies/distorm/src/prefix.c
	$(CXX) $(CFLAGS64_RELEASE) $(INC_RELEASE) -c ../Dependencies/distorm/src/prefix.c -o $(OBJDIR64_RELEASE)/prefix.o

$(OBJDIR64_RELEASE)/textdefs.o: ../Dependencies/distorm/src/textdefs.c
	$(CXX) $(CFLAGS64_RELEASE) $(INC_RELEASE) -c ../Dependencies/distorm/src/textdefs.c -o $(OBJDIR64_RELEASE)/textdefs.o

clean_release:
	rm -f $(OBJ32_RELEASE) $(OUT32_RELEASE)
	rm -f $(OBJ64_RELEASE) $(OUT64_RELEASE)
	rm -rf build/release
	rm -rf $(OBJDIR32_RELEASE)
	rm -rf $(OBJDIR64_RELEASE)

.PHONY: before_debug after_debug clean_debug before_release after_release clean_release
