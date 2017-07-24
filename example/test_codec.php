<?php

require_once 'BrickredExchange.php';
require_once 'attr.php';
require_once 'message_test.php';
require_once 'message_type.php';

use Brickred\Exchange\Codec;
use Brickred\Exchange\Int64;
use Brickred\Exchange\UInt64;
use Brickred\Protocol\AttrType;
use Brickred\Protocol\MsgTest;

// basic test
$bin = Codec::writeInt8(-1);
var_dump(bin2hex($bin));
$bin = Codec::writeInt8(255);
var_dump(bin2hex($bin));
$s = Codec::openStreamForBuffer($bin);
var_dump(Codec::readUInt8($s));
$s = Codec::openStreamForBuffer($bin);
var_dump(Codec::readInt8($s));
$s = Codec::openStreamForBuffer($bin);
var_dump(Codec::readBool($s));

$bin = Codec::writeInt16(-1);
var_dump(bin2hex($bin));
$bin = Codec::writeInt16(65535);
var_dump(bin2hex($bin));
$s = Codec::openStreamForBuffer($bin);
var_dump(Codec::readUInt16($s));
$s = Codec::openStreamForBuffer($bin);
var_dump(Codec::readInt16($s));

$bin = Codec::writeInt32(-1);
var_dump(bin2hex($bin));
$bin = Codec::writeInt32(4294967295);
var_dump(bin2hex($bin));
$s = Codec::openStreamForBuffer($bin);
var_dump(Codec::readUInt32($s));
$s = Codec::openStreamForBuffer($bin);
var_dump(Codec::readInt32($s));

$bin = Codec::writeLength(253);
var_dump(bin2hex($bin));
$s = Codec::openStreamForBuffer($bin);
var_dump(Codec::readLength($s));

$bin = Codec::writeLength(65535);
var_dump(bin2hex($bin));
$s = Codec::openStreamForBuffer($bin);
var_dump(Codec::readLength($s));

$bin = Codec::writeLength(4294967295);
var_dump(bin2hex($bin));
$s = Codec::openStreamForBuffer($bin);
var_dump(Codec::readLength($s));

$bin = Codec::writeString('Hello, world!');
var_dump(bin2hex($bin));
$s = Codec::openStreamForBuffer($bin);
var_dump(Codec::readString($s));

$bin = Codec::writeList(array(1, 2, 3, 4, 5, 10, 9, 8, 7, 6), 'writeInt32');
var_dump(bin2hex($bin));
$s = Codec::openStreamForBuffer($bin);
var_dump(Codec::readList($s, 'readInt32'));

$bin = Codec::writeInt64(new Int64(-1, -1));
var_dump(bin2hex($bin));
$s = Codec::openStreamForBuffer($bin);
$v = Codec::readUInt64($s);
var_dump($v);
var_dump($v->getValue());
$s = Codec::openStreamForBuffer($bin);
$v = Codec::readInt64($s);
var_dump($v);
var_dump($v->getValue());

// json test
$msg = new MsgTest();
$msg->a1 = 0x7f;
$msg->a2 = 0xff;
$msg->a3 = 0x7fff;
$msg->a4 = 0xffff;
$msg->a5 = 0x7fffffff;
$msg->a6 = 0xffffffff;
$msg->a7->reset(0x7fffffff, 0xffffffff);
$msg->a8->reset(0xffffffff, 0xffffffff);
$msg->a9 = "hello, world!";
$msg->a10 = true;
$msg->a11 = AttrType::STR;
$msg->set_a23(1);

for ($i = 0; $i < 10; ++$i) {
    array_push($msg->a18, clone $msg->a7);
}
for ($i = 0; $i < 10; ++$i) {
    array_push($msg->a19, clone $msg->a8);
}

$bin = $msg->encode();
$json = $msg->toJson();
echo "\nbefore json:\n$json\n";
echo "\nbefore bin:\n".bin2hex($bin)."\n";

$msg = new MsgTest();
$msg->fromJson($json);
$json = $msg->toJson();
$bin = $msg->encode();
echo "\nafter json:\n$json\n";
echo "\nafter bin:\n".bin2hex($bin)."\n";

/* end of test_codec.php */
