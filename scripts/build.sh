#!/usr/bin/env bash

source ~/.dnx/dnvm/dnvm.sh
dnu restore
dnx -p tests/msgpack.tests test -parallel none