#!/usr/bin/env bash

set -ev

pushd ${BASH_SOURCE%/*}

cd ..

dotnet test -c Release -f netcoreapp1.0 --no-build tests/msgpack.light.tests/msgpack.light.tests.csproj -- -parallel assemblies
dotnet test -c Release -f netcoreapp1.1 --no-build tests/msgpack.light.tests/msgpack.light.tests.csproj -- -parallel assemblies
dotnet test -c Release -f netcoreapp2.0 --no-build tests/msgpack.light.tests/msgpack.light.tests.csproj -- -parallel assemblies

popd
