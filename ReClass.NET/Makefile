all: debug release

clean: clean_debug clean_release

debug:
	xbuild /p:Configuration=Debug /p:Platform=x86 ReClass.NET.csproj
	xbuild /p:Configuration=Debug /p:Platform=x64 ReClass.NET.csproj

clean_debug:
	xbuild /t:Clean ReClass.NET.csproj

release:
	xbuild /p:Configuration=Release /p:Platform=x86 ReClass.NET.csproj
	xbuild /p:Configuration=Release /p:Platform=x64 ReClass.NET.csproj

clean_release:
	xbuild /t:Clean ReClass.NET.csproj

update:
	mono --runtime=v4.0 ../Dependencies/nuget.exe restore ReClass.NET.csproj -SolutionDirectory ../
