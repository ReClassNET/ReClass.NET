all: debug release

clean: clean_debug clean_release

debug:
	msbuild /p:Configuration=Debug ReClass.NET_Launcher.csproj

clean_debug:
	msbuild /t:Clean ReClass.NET_Launcher.csproj

release:
	msbuild /p:Configuration=Release ReClass.NET_Launcher.csproj

clean_release:
	msbuild /t:Clean ReClass.NET_Launcher.csproj
