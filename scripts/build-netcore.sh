#!/usr/bin/env bash

set -e

pushd ${BASH_SOURCE%/*}

cd ..

dotnet restore
dotnet build -c Release -f netstandard1.1 src/msgpack.light/msgpack.light.csproj
dotnet build -c Release -f netcoreapp1.0 tests/msgpack.light.tests/msgpack.light.tests.csproj
dotnet build -c Release -f netcoreapp1.1 tests/msgpack.light.tests/msgpack.light.tests.csproj

popd