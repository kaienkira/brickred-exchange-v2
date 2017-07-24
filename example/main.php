<?php

require_once 'BrickredExchange.php';
require_once 'attr.php';
require_once 'message_test.php';
require_once 'message_type.php';

use Brickred\Exchange\Int64;
use Brickred\Exchange\UInt64;
use Protocol\Client\AttrType;
use Protocol\Client\MsgTest;
use Protocol\Client\MessageType;

$msg = new MsgTest();
$msg->a1 = 0x7f;
$msg->a2 = 0xff;
$msg->a3 = 0x7fff;
$msg->a4 = 0xffff;
$msg->a5 = 0x7fffffff;
$msg->a6 = 0xffffffff;
$msg->a7->reset(0x7fffffff, 0xffffffff);
$msg->a8->reset(0xffffffff, 0xffffffff);
$msg->a9 = 'hello, world!';
$msg->a10 = true;
$msg->a11 = AttrType::STR;
$msg->a12 = 'hello, world!';

for ($i = 0; $i < 254; ++$i) {
    array_push($msg->b5, $i);
}
for ($i = 0; $i < 10; ++$i) {
    array_push($msg->b7, clone $msg->a7);
}
for ($i = 0; $i < 10; ++$i) {
    array_push($msg->b8, clone $msg->a8);
}

$msg->set_c1(1);
$msg->set_c2(1);
$msg->clear_has_c1();

$msg->set_has_c3();
for ($i = 0; $i < 65536; ++$i) {
    array_push($msg->c3, $i);
}

// encode binary
$bin = $msg->encode();
$id = MessageType::id($msg);
// encode array
$array = $msg->toArray();
// encode json
$json = $msg->toJson();

// decode binary
$msg = MessageType::create($id);
$msg->decode($bin);

echo 'encode_size = '.strlen($bin)."\n".
     "a1 = $msg->a1\n".
     "a2 = $msg->a2\n".
     "a3 = $msg->a3\n".
     "a4 = $msg->a4\n".
     "a5 = $msg->a5\n".
     "a6 = $msg->a6\n".
     "a7 = ".$msg->a7->getValue()."\n".
     "a8 = ".$msg->a8->getValue()."\n".
     "a9 = $msg->a9\n".
     "a10 = $msg->a10\n".
     "a11 = $msg->a11\n".
     "a12 = $msg->a12\n".
     "b5 size = ".count($msg->b5)."\n".
     "b5[253] = ".$msg->b5[253]."\n".
     "b7 size = ".count($msg->b7)."\n".
     "b7[0] = ".$msg->b7[0]->getValue()."\n".
     "b8 size = ".count($msg->b8)."\n".
     "b8[0] = ".$msg->b8[0]->getValue()."\n".
     "has c1 = ".(int)$msg->has_c1()."\n".
     "c1 = $msg->c1\n".
     "has c2 = ".(int)$msg->has_c2()."\n".
     "c2 = $msg->c2\n".
     "has c3 = ".$msg->has_c3()."\n".
     "c3 size = ".count($msg->c3)."\n".
     "c3[65535] = ".$msg->c3[65535]."\n";

// decode array
$msg = MessageType::create($id);
$msg->fromArray($array);
// decode json
$msg = MessageType::create($id);
$msg->fromJson($json);

/* end of main.php */
