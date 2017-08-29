#!/usr/bin/env bash

set -ev

pushd ${BASH_SOURCE%/*}

cd ..

dotnet restore
dotnet build -c Release msgpack.sln

popd
