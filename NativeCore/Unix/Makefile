WORKDIR = `pwd`

CC = gcc
CXX = gcc
AR = ar
LD = g++
WINDRES = windres

INC = -I../Dependencies/distorm/include
CFLAGS = -Wall -fPIC -DRECLASSNET64=1
RESINC =
LIBDIR =
LIB = -lstdc++fs -lstdc++
LDFLAGS = -shared -Wl,--no-undefined

INC_DEBUG = $(INC)
CFLAGS_DEBUG = $(CFLAGS) -g
RESINC_DEBUG = $(RESINC)
RCFLAGS_DEBUG = $(RCFLAGS)
LIBDIR_DEBUG = $(LIBDIR)
LIB_DEBUG = $(LIB)
LDFLAGS_DEBUG = $(LDFLAGS)
OBJDIR_DEBUG = obj/debug
DEP_DEBUG =
OUT_DEBUG = build/debug/NativeCore.so

INC_RELEASE = $(INC)
CFLAGS_RELEASE = $(CFLAGS) -O2
RESINC_RELEASE = $(RESINC)
RCFLAGS_RELEASE = $(RCFLAGS)
LIBDIR_RELEASE = $(LIBDIR)
LIB_RELEASE = $(LIB)
LDFLAGS_RELEASE = $(LDFLAGS) -s
OBJDIR_RELEASE = obj/release
DEP_RELEASE =
OUT_RELEASE = build/release/NativeCore.so

OBJ_DEBUG = $(OBJDIR_DEBUG)/WriteRemoteMemory.o $(OBJDIR_DEBUG)/ReadRemoteMemory.o $(OBJDIR_DEBUG)/OpenRemoteProcess.o $(OBJDIR_DEBUG)/IsProcessValid.o $(OBJDIR_DEBUG)/Input.o $(OBJDIR_DEBUG)/EnumerateRemoteSectionsAndModules.o $(OBJDIR_DEBUG)/EnumerateProcesses.o $(OBJDIR_DEBUG)/DisassembleCode.o $(OBJDIR_DEBUG)/DistormHelper.o  $(OBJDIR_DEBUG)/Debugger.o $(OBJDIR_DEBUG)/ControlRemoteProcess.o $(OBJDIR_DEBUG)/CloseRemoteProcess.o $(OBJDIR_DEBUG)/decoder.o $(OBJDIR_DEBUG)/distorm.o $(OBJDIR_DEBUG)/instructions.o $(OBJDIR_DEBUG)/insts.o $(OBJDIR_DEBUG)/mnemonics.o $(OBJDIR_DEBUG)/operands.o $(OBJDIR_DEBUG)/prefix.o $(OBJDIR_DEBUG)/textdefs.o $(OBJDIR_DEBUG)/wstring.o

OBJ_RELEASE = $(OBJDIR_RELEASE)/WriteRemoteMemory.o $(OBJDIR_RELEASE)/ReadRemoteMemory.o $(OBJDIR_RELEASE)/OpenRemoteProcess.o $(OBJDIR_RELEASE)/IsProcessValid.o $(OBJDIR_RELEASE)/Input.o $(OBJDIR_RELEASE)/EnumerateRemoteSectionsAndModules.o $(OBJDIR_RELEASE)/EnumerateProcesses.o $(OBJDIR_RELEASE)/DisassembleCode.o $(OBJDIR_RELEASE)/DistormHelper.o $(OBJDIR_RELEASE)/Debugger.o $(OBJDIR_RELEASE)/ControlRemoteProcess.o $(OBJDIR_RELEASE)/CloseRemoteProcess.o $(OBJDIR_RELEASE)/decoder.o $(OBJDIR_RELEASE)/distorm.o $(OBJDIR_RELEASE)/instructions.o $(OBJDIR_RELEASE)/insts.o $(OBJDIR_RELEASE)/mnemonics.o $(OBJDIR_RELEASE)/operands.o $(OBJDIR_RELEASE)/prefix.o $(OBJDIR_RELEASE)/textdefs.o $(OBJDIR_RELEASE)/wstring.o

all: debug release

clean: clean_debug clean_release

before_debug:
	test -d build/debug || mkdir -p build/debug
	test -d $(OBJDIR_DEBUG) || mkdir -p $(OBJDIR_DEBUG)

after_debug:

debug: before_debug out_debug after_debug

out_debug: before_debug $(OBJ_DEBUG) $(DEP_DEBUG)
	$(CXX) $(LIBDIR_DEBUG) -o $(OUT_DEBUG) $(OBJ_DEBUG)  $(LDFLAGS_DEBUG) $(LIB_DEBUG)

$(OBJDIR_DEBUG)/WriteRemoteMemory.o: WriteRemoteMemory.cpp
	$(CXX) $(CFLAGS_DEBUG) $(INC_DEBUG) -c WriteRemoteMemory.cpp -o $(OBJDIR_DEBUG)/WriteRemoteMemory.o

$(OBJDIR_DEBUG)/ReadRemoteMemory.o: ReadRemoteMemory.cpp
	$(CXX) $(CFLAGS_DEBUG) $(INC_DEBUG) -c ReadRemoteMemory.cpp -o $(OBJDIR_DEBUG)/ReadRemoteMemory.o

$(OBJDIR_DEBUG)/OpenRemoteProcess.o: OpenRemoteProcess.cpp
	$(CXX) $(CFLAGS_DEBUG) $(INC_DEBUG) -c OpenRemoteProcess.cpp -o $(OBJDIR_DEBUG)/OpenRemoteProcess.o

$(OBJDIR_DEBUG)/IsProcessValid.o: IsProcessValid.cpp
	$(CXX) $(CFLAGS_DEBUG) $(INC_DEBUG) -c IsProcessValid.cpp -o $(OBJDIR_DEBUG)/IsProcessValid.o

$(OBJDIR_DEBUG)/Input.o: Input.cpp
	$(CXX) $(CFLAGS_DEBUG) $(INC_DEBUG) -c Input.cpp -o $(OBJDIR_DEBUG)/Input.o

$(OBJDIR_DEBUG)/EnumerateRemoteSectionsAndModules.o: EnumerateRemoteSectionsAndModules.cpp
	$(CXX) $(CFLAGS_DEBUG) $(INC_DEBUG) -c EnumerateRemoteSectionsAndModules.cpp -o $(OBJDIR_DEBUG)/EnumerateRemoteSectionsAndModules.o

$(OBJDIR_DEBUG)/EnumerateProcesses.o: EnumerateProcesses.cpp
	$(CXX) $(CFLAGS_DEBUG) $(INC_DEBUG) -c EnumerateProcesses.cpp -o $(OBJDIR_DEBUG)/EnumerateProcesses.o

$(OBJDIR_DEBUG)/DisassembleCode.o: DisassembleCode.cpp
	$(CXX) $(CFLAGS_DEBUG) $(INC_DEBUG) -c DisassembleCode.cpp -o $(OBJDIR_DEBUG)/DisassembleCode.o

$(OBJDIR_DEBUG)/DistormHelper.o: ../Shared/DistormHelper.cpp
	$(CXX) $(CFLAGS_DEBUG) $(INC_DEBUG) -c ../Shared/DistormHelper.cpp -o $(OBJDIR_DEBUG)/DistormHelper.o

$(OBJDIR_DEBUG)/Debugger.o: Debugger.cpp
	$(CXX) $(CFLAGS_DEBUG) $(INC_DEBUG) -c Debugger.cpp -o $(OBJDIR_DEBUG)/Debugger.o

$(OBJDIR_DEBUG)/ControlRemoteProcess.o: ControlRemoteProcess.cpp
	$(CXX) $(CFLAGS_DEBUG) $(INC_DEBUG) -c ControlRemoteProcess.cpp -o $(OBJDIR_DEBUG)/ControlRemoteProcess.o

$(OBJDIR_DEBUG)/CloseRemoteProcess.o: CloseRemoteProcess.cpp
	$(CXX) $(CFLAGS_DEBUG) $(INC_DEBUG) -c CloseRemoteProcess.cpp -o $(OBJDIR_DEBUG)/CloseRemoteProcess.o

$(OBJDIR_DEBUG)/decoder.o: ../Dependencies/distorm/src/decoder.c
	$(CXX) $(CFLAGS_DEBUG) $(INC_DEBUG) -c ../Dependencies/distorm/src/decoder.c -o $(OBJDIR_DEBUG)/decoder.o

$(OBJDIR_DEBUG)/distorm.o: ../Dependencies/distorm/src/distorm.c
	$(CXX) $(CFLAGS_DEBUG) $(INC_DEBUG) -c ../Dependencies/distorm/src/distorm.c -o $(OBJDIR_DEBUG)/distorm.o

$(OBJDIR_DEBUG)/instructions.o: ../Dependencies/distorm/src/instructions.c
	$(CXX) $(CFLAGS_DEBUG) $(INC_DEBUG) -c ../Dependencies/distorm/src/instructions.c -o $(OBJDIR_DEBUG)/instructions.o

$(OBJDIR_DEBUG)/insts.o: ../Dependencies/distorm/src/insts.c
	$(CXX) $(CFLAGS_DEBUG) $(INC_DEBUG) -c ../Dependencies/distorm/src/insts.c -o $(OBJDIR_DEBUG)/insts.o

$(OBJDIR_DEBUG)/mnemonics.o: ../Dependencies/distorm/src/mnemonics.c
	$(CXX) $(CFLAGS_DEBUG) $(INC_DEBUG) -c ../Dependencies/distorm/src/mnemonics.c -o $(OBJDIR_DEBUG)/mnemonics.o

$(OBJDIR_DEBUG)/operands.o: ../Dependencies/distorm/src/operands.c
	$(CXX) $(CFLAGS_DEBUG) $(INC_DEBUG) -c ../Dependencies/distorm/src/operands.c -o $(OBJDIR_DEBUG)/operands.o

$(OBJDIR_DEBUG)/prefix.o: ../Dependencies/distorm/src/prefix.c
	$(CXX) $(CFLAGS_DEBUG) $(INC_DEBUG) -c ../Dependencies/distorm/src/prefix.c -o $(OBJDIR_DEBUG)/prefix.o

$(OBJDIR_DEBUG)/textdefs.o: ../Dependencies/distorm/src/textdefs.c
	$(CXX) $(CFLAGS_DEBUG) $(INC_DEBUG) -c ../Dependencies/distorm/src/textdefs.c -o $(OBJDIR_DEBUG)/textdefs.o

$(OBJDIR_DEBUG)/wstring.o: ../Dependencies/distorm/src/wstring.c
	$(CXX) $(CFLAGS_DEBUG) $(INC_DEBUG) -c ../Dependencies/distorm/src/wstring.c -o $(OBJDIR_DEBUG)/wstring.o

clean_debug:
	rm -f $(OBJ_DEBUG) $(OUT_DEBUG)
	rm -rf build/debug
	rm -rf $(OBJDIR_DEBUG)

before_release:
	test -d build/release || mkdir -p build/release
	test -d $(OBJDIR_RELEASE) || mkdir -p $(OBJDIR_RELEASE)

after_release:

release: before_release out_release after_release

out_release: before_release $(OBJ_RELEASE) $(DEP_RELEASE)
	$(LD) $(LIBDIR_RELEASE) -o $(OUT_RELEASE) $(OBJ_RELEASE)  $(LDFLAGS_RELEASE) $(LIB_RELEASE)

$(OBJDIR_RELEASE)/WriteRemoteMemory.o: WriteRemoteMemory.cpp
	$(CXX) $(CFLAGS_RELEASE) $(INC_RELEASE) -c WriteRemoteMemory.cpp -o $(OBJDIR_RELEASE)/WriteRemoteMemory.o

$(OBJDIR_RELEASE)/ReadRemoteMemory.o: ReadRemoteMemory.cpp
	$(CXX) $(CFLAGS_RELEASE) $(INC_RELEASE) -c ReadRemoteMemory.cpp -o $(OBJDIR_RELEASE)/ReadRemoteMemory.o

$(OBJDIR_RELEASE)/OpenRemoteProcess.o: OpenRemoteProcess.cpp
	$(CXX) $(CFLAGS_RELEASE) $(INC_RELEASE) -c OpenRemoteProcess.cpp -o $(OBJDIR_RELEASE)/OpenRemoteProcess.o

$(OBJDIR_RELEASE)/IsProcessValid.o: IsProcessValid.cpp
	$(CXX) $(CFLAGS_RELEASE) $(INC_RELEASE) -c IsProcessValid.cpp -o $(OBJDIR_RELEASE)/IsProcessValid.o

$(OBJDIR_RELEASE)/Input.o: Input.cpp
	$(CXX) $(CFLAGS_RELEASE) $(INC_RELEASE) -c Input.cpp -o $(OBJDIR_RELEASE)/Input.o

$(OBJDIR_RELEASE)/EnumerateRemoteSectionsAndModules.o: EnumerateRemoteSectionsAndModules.cpp
	$(CXX) $(CFLAGS_RELEASE) $(INC_RELEASE) -c EnumerateRemoteSectionsAndModules.cpp -o $(OBJDIR_RELEASE)/EnumerateRemoteSectionsAndModules.o

$(OBJDIR_RELEASE)/EnumerateProcesses.o: EnumerateProcesses.cpp
	$(CXX) $(CFLAGS_RELEASE) $(INC_RELEASE) -c EnumerateProcesses.cpp -o $(OBJDIR_RELEASE)/EnumerateProcesses.o

$(OBJDIR_RELEASE)/DisassembleCode.o: DisassembleCode.cpp
	$(CXX) $(CFLAGS_RELEASE) $(INC_RELEASE) -c DisassembleCode.cpp -o $(OBJDIR_RELEASE)/DisassembleCode.o

$(OBJDIR_RELEASE)/DistormHelper.o: ../Shared/DistormHelper.cpp
	$(CXX) $(CFLAGS_RELEASE) $(INC_RELEASE) -c ../Shared/DistormHelper.cpp -o $(OBJDIR_RELEASE)/DistormHelper.o

$(OBJDIR_RELEASE)/Debugger.o: Debugger.cpp
	$(CXX) $(CFLAGS_RELEASE) $(INC_RELEASE) -c Debugger.cpp -o $(OBJDIR_RELEASE)/Debugger.o

$(OBJDIR_RELEASE)/ControlRemoteProcess.o: ControlRemoteProcess.cpp
	$(CXX) $(CFLAGS_RELEASE) $(INC_RELEASE) -c ControlRemoteProcess.cpp -o $(OBJDIR_RELEASE)/ControlRemoteProcess.o

$(OBJDIR_RELEASE)/CloseRemoteProcess.o: CloseRemoteProcess.cpp
	$(CXX) $(CFLAGS_RELEASE) $(INC_RELEASE) -c CloseRemoteProcess.cpp -o $(OBJDIR_RELEASE)/CloseRemoteProcess.o

$(OBJDIR_RELEASE)/decoder.o: ../Dependencies/distorm/src/decoder.c
	$(CXX) $(CFLAGS_RELEASE) $(INC_RELEASE) -c ../Dependencies/distorm/src/decoder.c -o $(OBJDIR_RELEASE)/decoder.o

$(OBJDIR_RELEASE)/distorm.o: ../Dependencies/distorm/src/distorm.c
	$(CXX) $(CFLAGS_RELEASE) $(INC_RELEASE) -c ../Dependencies/distorm/src/distorm.c -o $(OBJDIR_RELEASE)/distorm.o

$(OBJDIR_RELEASE)/instructions.o: ../Dependencies/distorm/src/instructions.c
	$(CXX) $(CFLAGS_RELEASE) $(INC_RELEASE) -c ../Dependencies/distorm/src/instructions.c -o $(OBJDIR_RELEASE)/instructions.o

$(OBJDIR_RELEASE)/insts.o: ../Dependencies/distorm/src/insts.c
	$(CXX) $(CFLAGS_RELEASE) $(INC_RELEASE) -c ../Dependencies/distorm/src/insts.c -o $(OBJDIR_RELEASE)/insts.o

$(OBJDIR_RELEASE)/mnemonics.o: ../Dependencies/distorm/src/mnemonics.c
	$(CXX) $(CFLAGS_RELEASE) $(INC_RELEASE) -c ../Dependencies/distorm/src/mnemonics.c -o $(OBJDIR_RELEASE)/mnemonics.o

$(OBJDIR_RELEASE)/operands.o: ../Dependencies/distorm/src/operands.c
	$(CXX) $(CFLAGS_RELEASE) $(INC_RELEASE) -c ../Dependencies/distorm/src/operands.c -o $(OBJDIR_RELEASE)/operands.o

$(OBJDIR_RELEASE)/prefix.o: ../Dependencies/distorm/src/prefix.c
	$(CXX) $(CFLAGS_RELEASE) $(INC_RELEASE) -c ../Dependencies/distorm/src/prefix.c -o $(OBJDIR_RELEASE)/prefix.o

$(OBJDIR_RELEASE)/textdefs.o: ../Dependencies/distorm/src/textdefs.c
	$(CXX) $(CFLAGS_RELEASE) $(INC_RELEASE) -c ../Dependencies/distorm/src/textdefs.c -o $(OBJDIR_RELEASE)/textdefs.o

$(OBJDIR_RELEASE)/wstring.o: ../Dependencies/distorm/src/wstring.c
	$(CXX) $(CFLAGS_RELEASE) $(INC_RELEASE) -c ../Dependencies/distorm/src/wstring.c -o $(OBJDIR_RELEASE)/wstring.o

clean_release:
	rm -f $(OBJ_RELEASE) $(OUT_RELEASE)
	rm -rf build/release
	rm -rf $(OBJDIR_RELEASE)

.PHONY: before_debug after_debug clean_debug before_release after_release clean_release
