WORKDIR = `pwd`

CC = gcc
CXX = g++
AR = ar
LD = g++
WINDRES = windres

INC = -I../Dependencies/beaengine/include
CFLAGS = -Wall -fPIC -D RECLASSNET64=1
RESINC =
LIBDIR =
LIB = -lstdc++fs
LDFLAGS = -shared

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

OBJ_DEBUG = $(OBJDIR_DEBUG)/WriteRemoteMemory.o $(OBJDIR_DEBUG)/ReadRemoteMemory.o $(OBJDIR_DEBUG)/OpenRemoteProcess.o $(OBJDIR_DEBUG)/IsProcessValid.o $(OBJDIR_DEBUG)/Input.o $(OBJDIR_DEBUG)/EnumerateRemoteSectionsAndModules.o $(OBJDIR_DEBUG)/EnumerateProcesses.o $(OBJDIR_DEBUG)/DisassembleCode.o $(OBJDIR_DEBUG)/Debugger.o $(OBJDIR_DEBUG)/ControlRemoteProcess.o $(OBJDIR_DEBUG)/CloseRemoteProcess.o $(OBJDIR_DEBUG)/BaeEngine.o

OBJ_RELEASE = $(OBJDIR_RELEASE)/WriteRemoteMemory.o $(OBJDIR_RELEASE)/ReadRemoteMemory.o $(OBJDIR_RELEASE)/OpenRemoteProcess.o $(OBJDIR_RELEASE)/IsProcessValid.o $(OBJDIR_RELEASE)/Input.o $(OBJDIR_RELEASE)/EnumerateRemoteSectionsAndModules.o $(OBJDIR_RELEASE)/EnumerateProcesses.o $(OBJDIR_RELEASE)/DisassembleCode.o $(OBJDIR_RELEASE)/Debugger.o $(OBJDIR_RELEASE)/ControlRemoteProcess.o $(OBJDIR_RELEASE)/CloseRemoteProcess.o $(OBJDIR_RELEASE)/BaeEngine.o

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

$(OBJDIR_DEBUG)/Debugger.o: Debugger.cpp
	$(CXX) $(CFLAGS_DEBUG) $(INC_DEBUG) -c Debugger.cpp -o $(OBJDIR_DEBUG)/Debugger.o

$(OBJDIR_DEBUG)/ControlRemoteProcess.o: ControlRemoteProcess.cpp
	$(CXX) $(CFLAGS_DEBUG) $(INC_DEBUG) -c ControlRemoteProcess.cpp -o $(OBJDIR_DEBUG)/ControlRemoteProcess.o

$(OBJDIR_DEBUG)/CloseRemoteProcess.o: CloseRemoteProcess.cpp
	$(CXX) $(CFLAGS_DEBUG) $(INC_DEBUG) -c CloseRemoteProcess.cpp -o $(OBJDIR_DEBUG)/CloseRemoteProcess.o

$(OBJDIR_DEBUG)/BaeEngine.o: ../Dependencies/beaengine/src/BeaEngine.c
	$(CXX) $(CFLAGS_DEBUG) $(INC_DEBUG) -c ../Dependencies/beaengine/src/BeaEngine.c -o $(OBJDIR_DEBUG)/BaeEngine.o

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

$(OBJDIR_RELEASE)/Debugger.o: Debugger.cpp
	$(CXX) $(CFLAGS_RELEASE) $(INC_RELEASE) -c Debugger.cpp -o $(OBJDIR_RELEASE)/Debugger.o

$(OBJDIR_RELEASE)/ControlRemoteProcess.o: ControlRemoteProcess.cpp
	$(CXX) $(CFLAGS_RELEASE) $(INC_RELEASE) -c ControlRemoteProcess.cpp -o $(OBJDIR_RELEASE)/ControlRemoteProcess.o

$(OBJDIR_RELEASE)/CloseRemoteProcess.o: CloseRemoteProcess.cpp
	$(CXX) $(CFLAGS_RELEASE) $(INC_RELEASE) -c CloseRemoteProcess.cpp -o $(OBJDIR_RELEASE)/CloseRemoteProcess.o

$(OBJDIR_RELEASE)/BaeEngine.o: ../Dependencies/beaengine/src/BeaEngine.c
	$(CXX) $(CFLAGS_RELEASE) $(INC_RELEASE) -c ../Dependencies/beaengine/src/BeaEngine.c -o $(OBJDIR_RELEASE)/BaeEngine.o

clean_release:
	rm -f $(OBJ_RELEASE) $(OUT_RELEASE)
	rm -rf build/release
	rm -rf $(OBJDIR_RELEASE)

.PHONY: before_debug after_debug clean_debug before_release after_release clean_release
