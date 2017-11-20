all: debug release

clean: clean_debug clean_release

debug:
	xbuild /p:Configuration=Debug ReClass.NET_Launcher.csproj

clean_debug:
	xbuild /t:Clean ReClass.NET_Launcher.csproj

release:
	xbuild /p:Configuration=Release ReClass.NET_Launcher.csproj

clean_release:
	xbuild /t:Clean ReClass.NET_Launcher.csproj
