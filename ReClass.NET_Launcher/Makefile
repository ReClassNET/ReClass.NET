.PHONY: all clean debug clean_debug release clean_release docker_debug docker_release podman_debug podman_release

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

docker_debug:
	docker container run --rm -v ${PWD}/..:/build:z -w /build -u $(shell id -u ${USER}):$(shell id -g ${USER}) mono:latest bash -c "cd ReClass.NET_Launcher && msbuild /p:Configuration=Debug ReClass.NET_Launcher.csproj"

docker_release:
	docker container run --rm -v ${PWD}/..:/build:z -w /build -u $(shell id -u ${USER}):$(shell id -g ${USER}) mono:latest bash -c "cd ReClass.NET_Launcher && msbuild /p:Configuration=Release ReClass.NET_Launcher.csproj"

podman_debug:
	podman container run --rm -v ${PWD}/..:/build:z -w /build mono:latest bash -c "cd ReClass.NET_Launcher && msbuild /p:Configuration=Debug ReClass.NET_Launcher.csproj"

podman_release:
	podman container run --rm -v ${PWD}/..:/build:z -w /build mono:latest bash -c "cd ReClass.NET_Launcher && msbuild /p:Configuration=Release ReClass.NET_Launcher.csproj"
