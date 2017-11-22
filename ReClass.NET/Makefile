all: debug release

clean: clean_debug clean_release

debug:
	msbuild /p:Configuration=Debug /p:Platform=x86 ReClass.NET.csproj
	msbuild /p:Configuration=Debug /p:Platform=x64 ReClass.NET.csproj

clean_debug:
	msbuild /t:Clean ReClass.NET.csproj

release:
	msbuild /p:Configuration=Release /p:Platform=x86 ReClass.NET.csproj
	msbuild /p:Configuration=Release /p:Platform=x64 ReClass.NET.csproj

clean_release:
	msbuild /t:Clean ReClass.NET.csproj

update:
	mono --runtime=v4.0 ../Dependencies/nuget.exe restore ReClass.NET.csproj -SolutionDirectory ../
