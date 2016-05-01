#!/usr/bin/env bash

source ~/.dnx/dnvm/dnvm.sh
dnu restore
dnu build src/msgpack.light
dnu build tests/msgpack.light.tests
dnx -p tests/msgpack.light.tests test -parallel none