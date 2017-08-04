brickred-exchange-v2
====================

Brickred Studio data serialization tool

Build Compiler
--------------
Compiler is written in c#, your should install mono first
```
cd compiler
make
ls bin/
```

Compiler Usage
--------------
```
usage: brexc.exe -f <protocol_file> -l <language> 
    [-o <output_dir>]
    [-I <search_path>]
    [-n <new_line_type>] (unix|dos) default is unix
language supported: cpp csharp php
```

Protocol File Quick Example
---------------------------
* example/attr.xml
```xml
<protocol>

<namespace lang="cpp">protocol.client</namespace>
<namespace lang="php">Protocol.Client</namespace>
<namespace lang="csharp">Protocol.Client</namespace>

<enum name="AttrType">
  <item name="MIN" value="0"/>
  <item name="STR" value="MIN"/>
  <item name="AGI"/>
  <item name="INT"/>
  <item name="VIT"/>
  <item name="DEX"/>
  <item name="LUK"/>
  <item name="MAX"/>
</enum>

<enum name="ExtAttrType">
  <item name="MIN" value="AttrType.MAX"/>
  <item name="HIT" value="MIN"/>
  <item name="MAX"/>
</enum>

<struct name="Attr">
  <required name="id" type="AttrType"/>
  <required name="value" type="i32"/>
</struct>

</protocol>
```

* example/message\_test.xml
```xml
<protocol>

<namespace lang="cpp">protocol.client</namespace>
<namespace lang="php">Protocol.Client</namespace>
<namespace lang="csharp">Protocol.Client</namespace>

<import>attr.xml</import>
  
<struct name="MsgTest">
  <required name="a1" type="i8"/>
  <required name="a2" type="u8"/>
  <required name="a3" type="i16"/>
  <required name="a4" type="u16"/>
  <required name="a5" type="i32"/>
  <required name="a6" type="u32"/>
  <required name="a7" type="i64"/>
  <required name="a8" type="u64"/>
  <required name="a9" type="string"/>
  <required name="a10" type="bool"/>
  <required name="a11" type="attr.AttrType"/>
  <required name="a12" type="bytes"/>
  <required name="b1" type="list{i8}"/>
  <required name="b2" type="list{u8}"/>
  <required name="b3" type="list{i16}"/>
  <required name="b4" type="list{u16}"/>
  <required name="b5" type="list{i32}"/>
  <required name="b6" type="list{u32}"/>
  <required name="b7" type="list{i64}"/>
  <required name="b8" type="list{u64}"/>
  <required name="b9" type="list{string}"/>
  <required name="b10" type="list{bool}"/>
  <required name="b11" type="list{attr.AttrType}"/>
  <required name="b12" type="list{bytes}"/>
  <optional name="c1" type="i32"/>
  <optional name="c2" type="i32"/>
  <optional name="c3" type="list{i32}"/>
  <optional name="c4" type="string"/>
  <optional name="c5" type="bytes"/>
</struct>

<struct name="MsgTest2">
  <required name="a1" type="attr.Attr"/>
  <required name="a2" type="list{attr.Attr}"/>
</struct>

<struct name="MsgTest3">
  <required name="a1" type="MsgTest"/>
  <required name="a2" type="MsgTest2"/>
  <required name="a3" type="list{MsgTest}"/>
  <optional name="c1" type="MsgTest"/>
</struct>

<struct name="MsgTest4">
</struct>

<struct name="MsgTest5">
  <optional name="c1" type="i32"/>
  <optional name="c2" type="i32"/>
  <optional name="c3" type="i32"/>
  <optional name="c4" type="i32"/>
  <optional name="c5" type="i32"/>
  <optional name="c6" type="i32"/>
  <optional name="c7" type="i32"/>
  <optional name="c8" type="i32"/>
  <optional name="c9" type="i32"/>
  <optional name="c10" type="i32"/>
</struct>

</protocol>
```

* example/message\_type.xml
```xml
<protocol>

<namespace lang="cpp">protocol.client</namespace>
<namespace lang="php">Protocol.Client</namespace>
<namespace lang="csharp">Protocol.Client</namespace>

<import>message_test.xml</import>

<enum_map name="MessageType">
  <item name="MIN" value="1001"/>
  <item name="MSG_TEST" value="MIN" struct="message_test.MsgTest"/>
  <item name="MSG_TEST2" value="1100" struct="message_test.MsgTest2"/>
  <item name="MSG_TEST3" struct="message_test.MsgTest3"/>
  <item name="MSG_TEST4" struct="message_test.MsgTest4"/>
  <item name="MAX"/>
</enum_map>

</protocol>
```

Use with C++
------------
* build c++ brickred exchange library
```
cd cpp
./config.sh --prefix=<prefix>
make release && make install
```

* generate c++ source and header
```
$ mono brexc.exe -f attr.xml -l cpp
$ mono brexc.exe -f message_test.xml -l cpp
$ mono brexc.exe -f message_type.xml -l cpp
```

* we will get c++ source and header files in current dir
```
$ ls -1 *.cc *.h
attr.cc
attr.h
message_test.cc
message_test.h
message_type.cc
message_type.h
```

* write a main.cc to use the generated code (in example/main.cc)
* compile, link and test
```
$ g++ -c attr.cc
$ g++ -c message_test.cc
$ g++ -c message_type.cc
$ g++ main.cc attr.o message_test.o message_type.o -lbrickredexchange
$ ./a.out
encode_size = 263406
a1 = 127
a2 = 255
a3 = 32767
a4 = 65535
a5 = 2147483647
a6 = 4294967295
a7 = 9223372036854775807
a8 = 18446744073709551615
a9 = hello, world!
a10 = 1
a11 = 0
a12 = hello, world!
b5 size = 254
b5[253] = 253
b7 size = 10
b7[0] = 9223372036854775807
b8 size = 10
b8[0] = 18446744073709551615
has c1 = 0
c1 = 0
has c2 = 1
c2 = 1
has c3 = 1
c3 size = 65536
c3[65535] = 65535
```

Use with C#
---------------
* build csharp brickred exchange library(example use mono)
```
cd csharp
make
```

* generate csharp source
```
$ mono brexc.exe -f attr.xml -l csharp
$ mono brexc.exe -f message_test.xml -l csharp
$ mono brexc.exe -f message_type.xml -l csharp
```

* we will get generated csharp files in current dir
```
$ ls -l *.cs
attr.cs
message_test.cs
message_type.cs
```

* write a main.cs to use the generated code (in example/main.cs)
* compile, link and test
```
mcs main.cs attr.cs message_test.cs message_type.cs -r:Brickred.Exchange.dll
mono main.exe
```

Use with PHP
------------
* put php/BrickredExchange.php to your project dir

* generate php file
```
$ mono brexc.exe -f attr.xml -l php
$ mono brexc.exe -f message_test.xml -l php
$ mono brexc.exe -f message_type.xml -l php
```

* we will get generated php files in current dir
```
$ ls -l *.php
attr.php
BrickredExchange.php
message_test.php
message_type.php
```

* write a main.php to use the generated code (in example/main.php)
* run main.php
```
$ php main.php
```
