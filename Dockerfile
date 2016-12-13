FROM microsoft/dotnet:1.1-sdk-msbuild
MAINTAINER aensidhe

ADD . /app
WORKDIR /app

RUN dotnet --version && dotnet build --help
RUN dotnet build -f netstandard1.3 -c Release -r AnyCPU src/msgpack.light/msgpack.light.csproj
RUN dotnet build -f netstandard1.3 -c Release -r AnyCPU tests/msgpack.light.tests/msgpack.light.tests.csproj
RUN dotnet test -f netstandard1.3 -c Release --noBuild tests/msgpack.light.tests/msgpack.light.tests.csproj