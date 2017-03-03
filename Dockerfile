FROM progaudi/dotnet:1.1-rc3-xenial

WORKDIR /app

# copy csproj and restore as distinct layers
COPY . .
RUN dotnet --version \
    && dotnet restore \
    && dotnet build -c Release -f netstandard1.4 src/msgpack.light/msgpack.light.csproj \
    && dotnet build -c Release -f netcoreapp1.1 tests/msgpack.light.tests/msgpack.light.tests.csproj

RUN dotnet test -c Release --no-build tests/msgpack.light.tests/msgpack.light.tests.csproj -- -parallel assemblies
