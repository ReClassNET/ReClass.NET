.PHONY: all clean debug clean_debug release clean_release update docker_debug docker_release podman_debug podman_release

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

docker_debug:
	docker container run --rm -v ${PWD}/..:/build:z -w /build -u $(shell id -u ${USER}):$(shell id -g ${USER}) mono:latest bash -c "cd ReClass.NET && msbuild /p:Configuration=Debug /p:Platform=x86 ReClass.NET.csproj"
	docker container run --rm -v ${PWD}/..:/build:z -w /build -u $(shell id -u ${USER}):$(shell id -g ${USER}) mono:latest bash -c "cd ReClass.NET && msbuild /p:Configuration=Debug /p:Platform=x64 ReClass.NET.csproj"

docker_release:
	docker container run --rm -v ${PWD}/..:/build:z -w /build -u $(shell id -u ${USER}):$(shell id -g ${USER}) mono:latest bash -c "cd ReClass.NET && msbuild /p:Configuration=Release /p:Platform=x86 ReClass.NET.csproj"
	docker container run --rm -v ${PWD}/..:/build:z -w /build -u $(shell id -u ${USER}):$(shell id -g ${USER}) mono:latest bash -c "cd ReClass.NET && msbuild /p:Configuration=Release /p:Platform=x64 ReClass.NET.csproj"

podman_debug:
	podman container run --rm -v ${PWD}/..:/build:z -w /build mono:latest bash -c "cd ReClass.NET && msbuild /p:Configuration=Debug /p:Platform=x86 ReClass.NET.csproj"
	podman container run --rm -v ${PWD}/..:/build:z -w /build mono:latest bash -c "cd ReClass.NET && msbuild /p:Configuration=Debug /p:Platform=x64 ReClass.NET.csproj"

podman_release:
	podman container run --rm -v ${PWD}/..:/build:z -w /build mono:latest bash -c "cd ReClass.NET && msbuild /p:Configuration=Release /p:Platform=x86 ReClass.NET.csproj"
	podman container run --rm -v ${PWD}/..:/build:z -w /build mono:latest bash -c "cd ReClass.NET && msbuild /p:Configuration=Release /p:Platform=x64 ReClass.NET.csproj"
