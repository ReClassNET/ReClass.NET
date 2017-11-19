all: debug release dist

clean: clean_debug clean_release

debug:
	cd ReClass.NET_Launcher && make debug
	cd ReClass.NET && make debug
	cd NativeCore/Unix && make debug

clean_debug:
	cd ReClass.NET_Launcher && make clean_debug
	cd ReClass.NET && make clean_debug
	cd NativeCore/Unix && make clean_debug
	rm -rf build/Debug

release:
	cd ReClass.NET_Launcher && make release
	cd ReClass.NET && make release
	cd NativeCore/Unix && make release

clean_release:
	cd ReClass.NET_Launcher && make clean_release
	cd ReClass.NET && make clean_release
	cd NativeCore/Unix && make clean_release
	rm -rf build/Release

update:
	cd ReClass.NET && make update

dist:
	test -d build || mkdir -p build
	cp -r ReClass.NET/bin/* build/
	cp -r ReClass.NET_Launcher/bin/* build/
	cp NativeCore/Unix/build/debug/NativeCore.so build/Debug/x64
	cp NativeCore/Unix/build/release/NativeCore.so build/Release/x64
	test -d build/Debug/x86/Plugins || mkdir build/Debug/x86/Plugins
	test -d build/Debug/x64/Plugins || mkdir build/Debug/x64/Plugins
	test -d build/Release/x86/Plugins || mkdir build/Release/x86/Plugins
	test -d build/Release/x64/Plugins || mkdir build/Release/x64/Plugins
	test -d build/Debug/x86 && cp -r Dependencies/x86/* build/Debug/x86
	test -d build/Debug/x64 && cp -r Dependencies/x64/* build/Debug/x64
	test -d build/Release/x86 && cp -r Dependencies/x86/* build/Release/x86
	test -d build/Release/x64 && cp -r Dependencies/x64/* build/Release/x64
