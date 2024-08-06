#!/bin/bash

set -o pipefail

script_name=`basename "$0"`
script_abs_name=`readlink -f "$0"`
script_path=`dirname "$script_abs_name"`

cd "$script_path"/../compiler && make build
if [ $? -ne 0 ]; then exit 1; fi

# create test dir
test_dir="$script_path"/test
mkdir -p "$test_dir"
if [ $? -ne 0 ]; then exit 1; fi
cd "$test_dir"
if [ $? -ne 0 ]; then exit 1; fi

# copy files
mv "$script_path"/../compiler/bin/brexc.exe .
if [ $? -ne 0 ]; then exit 1; fi
cp "$script_path"/attr.xml .
if [ $? -ne 0 ]; then exit 1; fi
cp "$script_path"/message_test.xml .
if [ $? -ne 0 ]; then exit 1; fi
cp "$script_path"/message_type.xml .
if [ $? -ne 0 ]; then exit 1; fi
cp "$script_path"/../php/BrickredExchange.php .
if [ $? -ne 0 ]; then exit 1; fi
cp "$script_path"/main.cc .
if [ $? -ne 0 ]; then exit 1; fi
cp "$script_path"/main.cs .
if [ $? -ne 0 ]; then exit 1; fi
cp "$script_path"/main.php .
if [ $? -ne 0 ]; then exit 1; fi

# cpp test
mono brexc.exe -f attr.xml -l cpp
if [ $? -ne 0 ]; then exit 1; fi
mono brexc.exe -f message_test.xml -l cpp
if [ $? -ne 0 ]; then exit 1; fi
mono brexc.exe -f message_type.xml -l cpp
if [ $? -ne 0 ]; then exit 1; fi
g++ -I "$script_path"/../cpp/src \
    -o "cpp_test" \
    main.cc \
    attr.cc \
    message_test.cc \
    message_type.cc \
    "$script_path"/../cpp/src/brickred/exchange/base_struct.cc
if [ $? -ne 0 ]; then exit 1; fi
./cpp_test > cpp.text
if [ $? -ne 0 ]; then exit 1; fi

# csharp test
mono brexc.exe -f attr.xml -l csharp
if [ $? -ne 0 ]; then exit 1; fi
mono brexc.exe -f message_test.xml -l csharp
if [ $? -ne 0 ]; then exit 1; fi
mono brexc.exe -f message_type.xml -l csharp
if [ $? -ne 0 ]; then exit 1; fi
mcs -out:csharp_test.exe \
    main.cs \
    attr.cs \
    message_test.cs \
    message_type.cs \
    "$script_path"/../csharp/src/Brickred.Exchange/BaseStruct.cs \
    "$script_path"/../csharp/src/Brickred.Exchange/CodecException.cs \
    "$script_path"/../csharp/src/Brickred.Exchange/CodecInputStream.cs \
    "$script_path"/../csharp/src/Brickred.Exchange/CodecOutputStream.cs
if [ $? -ne 0 ]; then exit 1; fi
./csharp_test.exe > csharp.text
if [ $? -ne 0 ]; then exit 1; fi

# php test
mono brexc.exe -f attr.xml -l php
if [ $? -ne 0 ]; then exit 1; fi
mono brexc.exe -f message_test.xml -l php
if [ $? -ne 0 ]; then exit 1; fi
mono brexc.exe -f message_type.xml -l php
if [ $? -ne 0 ]; then exit 1; fi
php main.php > php.text
if [ $? -ne 0 ]; then exit 1; fi

# check test md5
md5sum cpp.text
if [ $? -ne 0 ]; then exit 1; fi
md5sum csharp.text
if [ $? -ne 0 ]; then exit 1; fi
md5sum php.text
if [ $? -ne 0 ]; then exit 1; fi

# check bin md5
md5sum cpp.bin
if [ $? -ne 0 ]; then exit 1; fi
md5sum csharp.bin
if [ $? -ne 0 ]; then exit 1; fi
md5sum php.bin
if [ $? -ne 0 ]; then exit 1; fi

exit 0
