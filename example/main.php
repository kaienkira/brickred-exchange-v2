<?php

if (PHP_SAPI !== 'cli') {
    exit(1);
}

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
// i8
$msg->a1 = 0x7f;
$msg->a1_1 = -128;
$msg->a1_2 = -80;
$msg->a1_3 = -1;
$msg->a1_4 = 0;
$msg->a1_5 = 1;
$msg->a1_6 = 80;
$msg->a1_7 = 127;
// u8
$msg->a2 = 0xff;
$msg->a2_1 = 0;
$msg->a2_2 = 1;
$msg->a2_3 = 80;
$msg->a2_4 = 127;
$msg->a2_5 = 128;
$msg->a2_6 = 180;
$msg->a2_7 = 255;
// i16
$msg->a3 = 0x7fff;
$msg->a3_1 = -32768;
$msg->a3_2 = -16384;
$msg->a3_3 = -16383;
$msg->a3_4 = -10000;
$msg->a3_5 = -5000;
$msg->a3_6 = -2500;
$msg->a3_7 = -256;
$msg->a3_8 = -255;
$msg->a3_9 = -128;
$msg->a3_10 = -127;
$msg->a3_11 = -1;
$msg->a3_12 = 0;
$msg->a3_13 = 1;
$msg->a3_14 = 127;
$msg->a3_15 = 128;
$msg->a3_16 = 255;
$msg->a3_17 = 256;
$msg->a3_18 = 2500;
$msg->a3_19 = 5000;
$msg->a3_20 = 10000;
$msg->a3_21 = 16383;
$msg->a3_22 = 16384;
$msg->a3_23 = 32767;
// u16
$msg->a4 = 0xffff;
$msg->a4_1 = 0;
$msg->a4_2 = 127;
$msg->a4_3 = 128;
$msg->a4_4 = 255;
$msg->a4_5 = 256;
$msg->a4_6 = 2500;
$msg->a4_7 = 5000;
$msg->a4_8 = 10000;
$msg->a4_9 = 16383;
$msg->a4_10 = 16384;
$msg->a4_11 = 32767;
$msg->a4_12 = 32768;
$msg->a4_13 = 50000;
$msg->a4_14 = 65535;
// i32
$msg->a5 = 0x7fffffff;
$msg->a5_1 = -2147483648;
$msg->a5_2 = -2147483647;
$msg->a5_3 = -1000000000;
$msg->a5_4 = -16777216;
$msg->a5_5 = -16777215;
$msg->a5_6 = -65536;
$msg->a5_7 = -65535;
$msg->a5_8 = -32768;
$msg->a5_9 = -32767;
$msg->a5_10 = -16384;
$msg->a5_11 = -16383;
$msg->a5_12 = -256;
$msg->a5_13 = -255;
$msg->a5_14 = -128;
$msg->a5_15 = -127;
$msg->a5_16 = -1;
$msg->a5_17 = 0;
$msg->a5_18 = 1;
$msg->a5_19 = 127;
$msg->a5_20 = 128;
$msg->a5_21 = 255;
$msg->a5_22 = 256;
$msg->a5_23 = 16383;
$msg->a5_24 = 16384;
$msg->a5_25 = 32767;
$msg->a5_26 = 32768;
$msg->a5_27 = 65535;
$msg->a5_28 = 65536;
$msg->a5_29 = 16777215;
$msg->a5_30 = 16777216;
$msg->a5_31 = 1000000000;
$msg->a5_32 = 2147483647;
// u32
$msg->a6 = 0xffffffff;
$msg->a6_1 = 0;
$msg->a6_2 = 127;
$msg->a6_3 = 128;
$msg->a6_4 = 255;
$msg->a6_5 = 256;
$msg->a6_6 = 16383;
$msg->a6_7 = 16384;
$msg->a6_8 = 32767;
$msg->a6_9 = 32768;
$msg->a6_10 = 65535;
$msg->a6_11 = 65536;
$msg->a6_12 = 16777215;
$msg->a6_13 = 16777216;
$msg->a6_14 = 1000000000;
$msg->a6_15 = 2147483647;
$msg->a6_16 = 2147483648;
$msg->a6_17 = 4294967295;
// i64
$msg->a7->reset(0x7fffffff, 0xffffffff);
//$msg->a7_1->reset(0x80000000, 0x00000000);
//$msg->a7_2->reset(0x80000000, 0x00000001);
//$msg->a7_3->reset(0xff000000, 0x00000000);
//$msg->a7_4->reset(0xff000000, 0x00000001);
//$msg->a7_5->reset(0xffff0000, 0x00000000);
//$msg->a7_6->reset(0xffff0000, 0x00000001);
//$msg->a7_7->reset(0xffffff00, 0x00000000);
//$msg->a7_8->reset(0xffffff00, 0x00000001);
//$msg->a7_9->reset(0xffffffff, 0x00000000);
//$msg->a7_10->reset(0xffffffff, 0x00000001);
//$msg->a7_11->reset(0xffffffff, 0x80000000);
//$msg->a7_12->reset(0xffffffff, 0x80000001);
//$msg->a7_13->reset(0xffffffff, 0xff000000);
//$msg->a7_14->reset(0xffffffff, 0xff000001);
//$msg->a7_15->reset(0xffffffff, 0xffff0000);
//$msg->a7_16->reset(0xffffffff, 0xffff0001);
//$msg->a7_17->reset(0xffffffff, 0xffff8000);
//$msg->a7_18->reset(0xffffffff, 0xffff8001);
//$msg->a7_19->reset(0xffffffff, 0xffffc000);
//$msg->a7_20->reset(0xffffffff, 0xffffc001);
//$msg->a7_21->reset(0xffffffff, 0xffffff00);
//$msg->a7_22->reset(0xffffffff, 0xffffff01);
//$msg->a7_23->reset(0xffffffff, 0xffffff80);
//$msg->a7_24->reset(0xffffffff, 0xffffff81);
//$msg->a7_25->reset(0xffffffff, 0xffffffff);
//$msg->a7_26->reset(0x00000000, 0x00000000);
//$msg->a7_27->reset(0x00000000, 0x00000001);
//$msg->a7_28->reset(0x00000000, 0x0000007f);
//$msg->a7_29->reset(0x00000000, 0x00000080);
//$msg->a7_30->reset(0x00000000, 0x000000ff);
//$msg->a7_31->reset(0x00000000, 0x00000100);
//$msg->a7_32->reset(0x00000000, 0x00003fff);
//$msg->a7_33->reset(0x00000000, 0x00004000);
//$msg->a7_34->reset(0x00000000, 0x00007fff);
//$msg->a7_35->reset(0x00000000, 0x00008000);
//$msg->a7_36->reset(0x00000000, 0x0000ffff);
//$msg->a7_37->reset(0x00000000, 0x00010000);
//$msg->a7_38->reset(0x00000000, 0x00ffffff);
//$msg->a7_39->reset(0x00000000, 0x01000000);
//$msg->a7_40->reset(0x00000000, 0x7fffffff);
//$msg->a7_41->reset(0x00000000, 0x80000000);
//$msg->a7_42->reset(0x00000000, 0xffffffff);
//$msg->a7_43->reset(0x00000001, 0x00000000);
//$msg->a7_44->reset(0x000000ff, 0xffffffff);
//$msg->a7_45->reset(0x00000100, 0x00000000);
//$msg->a7_46->reset(0x0000ffff, 0xffffffff);
//$msg->a7_47->reset(0x00010000, 0x00000000);
//$msg->a7_48->reset(0x00ffffff, 0xffffffff);
//$msg->a7_49->reset(0x01000000, 0x00000000);
//$msg->a7_50->reset(0x7fffffff, 0xffffffff);
$msg->a7_1->fromString('-9223372036854775808');
$msg->a7_2->fromString('-9223372036854775807');
$msg->a7_3->fromString('-72057594037927936');
$msg->a7_4->fromString('-72057594037927935');
$msg->a7_5->fromString('-281474976710656');
$msg->a7_6->fromString('-281474976710655');
$msg->a7_7->fromString('-1099511627776');
$msg->a7_8->fromString('-1099511627775');
$msg->a7_9->fromString('-4294967296');
$msg->a7_10->fromString('-4294967295');
$msg->a7_11->fromString('-2147483648');
$msg->a7_12->fromString('-2147483647');
$msg->a7_13->fromString('-16777216');
$msg->a7_14->fromString('-16777215');
$msg->a7_15->fromString('-65536');
$msg->a7_16->fromString('-65535');
$msg->a7_17->fromString('-32768');
$msg->a7_18->fromString('-32767');
$msg->a7_19->fromString('-16384');
$msg->a7_20->fromString('-16383');
$msg->a7_21->fromString('-256');
$msg->a7_22->fromString('-255');
$msg->a7_23->fromString('-128');
$msg->a7_24->fromString('-127');
$msg->a7_25->fromString('-1');
$msg->a7_26->fromString('0');
$msg->a7_27->fromString('1');
$msg->a7_28->fromString('127');
$msg->a7_29->fromString('128');
$msg->a7_30->fromString('255');
$msg->a7_31->fromString('256');
$msg->a7_32->fromString('16383');
$msg->a7_33->fromString('16384');
$msg->a7_34->fromString('32767');
$msg->a7_35->fromString('32768');
$msg->a7_36->fromString('65535');
$msg->a7_37->fromString('65536');
$msg->a7_38->fromString('16777215');
$msg->a7_39->fromString('16777216');
$msg->a7_40->fromString('2147483647');
$msg->a7_41->fromString('2147483648');
$msg->a7_42->fromString('4294967295');
$msg->a7_43->fromString('4294967296');
$msg->a7_44->fromString('1099511627775');
$msg->a7_45->fromString('1099511627776');
$msg->a7_46->fromString('281474976710655');
$msg->a7_47->fromString('281474976710656');
$msg->a7_48->fromString('72057594037927935');
$msg->a7_49->fromString('72057594037927936');
$msg->a7_50->fromString('9223372036854775807');
// u64
$msg->a8->reset(0xffffffff, 0xffffffff);
//$msg->a8_1->reset(0x00000000, 0x00000000);
//$msg->a8_2->reset(0x00000000, 0x00000001);
//$msg->a8_3->reset(0x00000000, 0x0000007f);
//$msg->a8_4->reset(0x00000000, 0x00000080);
//$msg->a8_5->reset(0x00000000, 0x000000ff);
//$msg->a8_6->reset(0x00000000, 0x00000100);
//$msg->a8_7->reset(0x00000000, 0x00003fff);
//$msg->a8_8->reset(0x00000000, 0x00004000);
//$msg->a8_9->reset(0x00000000, 0x00007fff);
//$msg->a8_10->reset(0x00000000, 0x00008000);
//$msg->a8_11->reset(0x00000000, 0x0000ffff);
//$msg->a8_12->reset(0x00000000, 0x00010000);
//$msg->a8_13->reset(0x00000000, 0x00ffffff);
//$msg->a8_14->reset(0x00000000, 0x01000000);
//$msg->a8_15->reset(0x00000000, 0x7fffffff);
//$msg->a8_16->reset(0x00000000, 0x80000000);
//$msg->a8_17->reset(0x00000000, 0xffffffff);
//$msg->a8_18->reset(0x00000001, 0x00000000);
//$msg->a8_19->reset(0x000000ff, 0xffffffff);
//$msg->a8_20->reset(0x00000100, 0x00000000);
//$msg->a8_21->reset(0x0000ffff, 0xffffffff);
//$msg->a8_22->reset(0x00010000, 0x00000000);
//$msg->a8_23->reset(0x00ffffff, 0xffffffff);
//$msg->a8_24->reset(0x01000000, 0x00000000);
//$msg->a8_25->reset(0x7fffffff, 0xffffffff);
//$msg->a8_26->reset(0x80000000, 0x00000000);
//$msg->a8_27->reset(0xffffffff, 0xffffffff);
$msg->a8_1->fromString('0');
$msg->a8_2->fromString('1');
$msg->a8_3->fromString('127');
$msg->a8_4->fromString('128');
$msg->a8_5->fromString('255');
$msg->a8_6->fromString('256');
$msg->a8_7->fromString('16383');
$msg->a8_8->fromString('16384');
$msg->a8_9->fromString('32767');
$msg->a8_10->fromString('32768');
$msg->a8_11->fromString('65535');
$msg->a8_12->fromString('65536');
$msg->a8_13->fromString('16777215');
$msg->a8_14->fromString('16777216');
$msg->a8_15->fromString('2147483647');
$msg->a8_16->fromString('2147483648');
$msg->a8_17->fromString('4294967295');
$msg->a8_18->fromString('4294967296');
$msg->a8_19->fromString('1099511627775');
$msg->a8_20->fromString('1099511627776');
$msg->a8_21->fromString('281474976710655');
$msg->a8_22->fromString('281474976710656');
$msg->a8_23->fromString('72057594037927935');
$msg->a8_24->fromString('72057594037927936');
$msg->a8_25->fromString('9223372036854775807');
$msg->a8_26->fromString('9223372036854775808');
$msg->a8_27->fromString('18446744073709551615');
// string
$msg->a9 = 'hello, world!';
// bool
$msg->a10 = true;
// attr.AttrType
$msg->a11 = AttrType::STR;
// bytes
$msg->a12 = 'hello, world!';
// i16v
$msg->a13 = 0x7fff;
$msg->a13_1 = -32768;
$msg->a13_2 = -16384;
$msg->a13_3 = -16383;
$msg->a13_4 = -10000;
$msg->a13_5 = -5000;
$msg->a13_6 = -2500;
$msg->a13_7 = -256;
$msg->a13_8 = -255;
$msg->a13_9 = -128;
$msg->a13_10 = -127;
$msg->a13_11 = -1;
$msg->a13_12 = 0;
$msg->a13_13 = 1;
$msg->a13_14 = 127;
$msg->a13_15 = 128;
$msg->a13_16 = 255;
$msg->a13_17 = 256;
$msg->a13_18 = 2500;
$msg->a13_19 = 5000;
$msg->a13_20 = 10000;
$msg->a13_21 = 16383;
$msg->a13_22 = 16384;
$msg->a13_23 = 32767;
// u16v
$msg->a14 = 0xffff;
$msg->a14_1 = 0;
$msg->a14_2 = 127;
$msg->a14_3 = 128;
$msg->a14_4 = 255;
$msg->a14_5 = 256;
$msg->a14_6 = 2500;
$msg->a14_7 = 5000;
$msg->a14_8 = 10000;
$msg->a14_9 = 16383;
$msg->a14_10 = 16384;
$msg->a14_11 = 32767;
$msg->a14_12 = 32768;
$msg->a14_13 = 50000;
$msg->a14_14 = 65535;
// i32v
$msg->a15 = 0x7fffffff;
$msg->a15_1 = -2147483648;
$msg->a15_2 = -2147483647;
$msg->a15_3 = -1000000000;
$msg->a15_4 = -16777216;
$msg->a15_5 = -16777215;
$msg->a15_6 = -65536;
$msg->a15_7 = -65535;
$msg->a15_8 = -32768;
$msg->a15_9 = -32767;
$msg->a15_10 = -16384;
$msg->a15_11 = -16383;
$msg->a15_12 = -256;
$msg->a15_13 = -255;
$msg->a15_14 = -128;
$msg->a15_15 = -127;
$msg->a15_16 = -1;
$msg->a15_17 = 0;
$msg->a15_18 = 1;
$msg->a15_19 = 127;
$msg->a15_20 = 128;
$msg->a15_21 = 255;
$msg->a15_22 = 256;
$msg->a15_23 = 16383;
$msg->a15_24 = 16384;
$msg->a15_25 = 32767;
$msg->a15_26 = 32768;
$msg->a15_27 = 65535;
$msg->a15_28 = 65536;
$msg->a15_29 = 16777215;
$msg->a15_30 = 16777216;
$msg->a15_31 = 1000000000;
$msg->a15_32 = 2147483647;
// u32v
$msg->a16 = 0xffffffff;
$msg->a16_1 = 0;
$msg->a16_2 = 127;
$msg->a16_3 = 128;
$msg->a16_4 = 255;
$msg->a16_5 = 256;
$msg->a16_6 = 16383;
$msg->a16_7 = 16384;
$msg->a16_8 = 32767;
$msg->a16_9 = 32768;
$msg->a16_10 = 65535;
$msg->a16_11 = 65536;
$msg->a16_12 = 16777215;
$msg->a16_13 = 16777216;
$msg->a16_14 = 1000000000;
$msg->a16_15 = 2147483647;
$msg->a16_16 = 2147483648;
$msg->a16_17 = 4294967295;
// i64v
$msg->a17->reset(0x7fffffff, 0xffffffff);
$msg->a17_1->fromString('-9223372036854775808');
$msg->a17_2->fromString('-9223372036854775807');
$msg->a17_3->fromString('-72057594037927936');
$msg->a17_4->fromString('-72057594037927935');
$msg->a17_5->fromString('-281474976710656');
$msg->a17_6->fromString('-281474976710655');
$msg->a17_7->fromString('-1099511627776');
$msg->a17_8->fromString('-1099511627775');
$msg->a17_9->fromString('-4294967296');
$msg->a17_10->fromString('-4294967295');
$msg->a17_11->fromString('-2147483648');
$msg->a17_12->fromString('-2147483647');
$msg->a17_13->fromString('-16777216');
$msg->a17_14->fromString('-16777215');
$msg->a17_15->fromString('-65536');
$msg->a17_16->fromString('-65535');
$msg->a17_17->fromString('-32768');
$msg->a17_18->fromString('-32767');
$msg->a17_19->fromString('-16384');
$msg->a17_20->fromString('-16383');
$msg->a17_21->fromString('-256');
$msg->a17_22->fromString('-255');
$msg->a17_23->fromString('-128');
$msg->a17_24->fromString('-127');
$msg->a17_25->fromString('-1');
$msg->a17_26->fromString('0');
$msg->a17_27->fromString('1');
$msg->a17_28->fromString('127');
$msg->a17_29->fromString('128');
$msg->a17_30->fromString('255');
$msg->a17_31->fromString('256');
$msg->a17_32->fromString('16383');
$msg->a17_33->fromString('16384');
$msg->a17_34->fromString('32767');
$msg->a17_35->fromString('32768');
$msg->a17_36->fromString('65535');
$msg->a17_37->fromString('65536');
$msg->a17_38->fromString('16777215');
$msg->a17_39->fromString('16777216');
$msg->a17_40->fromString('2147483647');
$msg->a17_41->fromString('2147483648');
$msg->a17_42->fromString('4294967295');
$msg->a17_43->fromString('4294967296');
$msg->a17_44->fromString('1099511627775');
$msg->a17_45->fromString('1099511627776');
$msg->a17_46->fromString('281474976710655');
$msg->a17_47->fromString('281474976710656');
$msg->a17_48->fromString('72057594037927935');
$msg->a17_49->fromString('72057594037927936');
$msg->a17_50->fromString('9223372036854775807');
// u64v
$msg->a18->reset(0xffffffff, 0xffffffff);
$msg->a18_1->fromString('0');
$msg->a18_2->fromString('1');
$msg->a18_3->fromString('127');
$msg->a18_4->fromString('128');
$msg->a18_5->fromString('255');
$msg->a18_6->fromString('256');
$msg->a18_7->fromString('16383');
$msg->a18_8->fromString('16384');
$msg->a18_9->fromString('32767');
$msg->a18_10->fromString('32768');
$msg->a18_11->fromString('65535');
$msg->a18_12->fromString('65536');
$msg->a18_13->fromString('16777215');
$msg->a18_14->fromString('16777216');
$msg->a18_15->fromString('2147483647');
$msg->a18_16->fromString('2147483648');
$msg->a18_17->fromString('4294967295');
$msg->a18_18->fromString('4294967296');
$msg->a18_19->fromString('1099511627775');
$msg->a18_20->fromString('1099511627776');
$msg->a18_21->fromString('281474976710655');
$msg->a18_22->fromString('281474976710656');
$msg->a18_23->fromString('72057594037927935');
$msg->a18_24->fromString('72057594037927936');
$msg->a18_25->fromString('9223372036854775807');
$msg->a18_26->fromString('9223372036854775808');
$msg->a18_27->fromString('18446744073709551615');

for ($i = 0; $i < 254; ++$i) {
    array_push($msg->b5, $i);
}
for ($i = 0; $i < 10; ++$i) {
    array_push($msg->b7, clone $msg->a7);
}
for ($i = 0; $i < 10; ++$i) {
    array_push($msg->b8, clone $msg->a8);
}

for ($i = 0; $i < 254; ++$i) {
    array_push($msg->b15, $i);
}
for ($i = 0; $i < 10; ++$i) {
    array_push($msg->b17, clone $msg->a17);
}
for ($i = 0; $i < 10; ++$i) {
    array_push($msg->b18, clone $msg->a18);
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
     "a1_1 = $msg->a1_1\n".
     "a1_2 = $msg->a1_2\n".
     "a1_3 = $msg->a1_3\n".
     "a1_4 = $msg->a1_4\n".
     "a1_5 = $msg->a1_5\n".
     "a1_6 = $msg->a1_6\n".
     "a1_7 = $msg->a1_7\n".
     "a2 = $msg->a2\n".
     "a2_1 = $msg->a2_1\n".
     "a2_2 = $msg->a2_2\n".
     "a2_3 = $msg->a2_3\n".
     "a2_4 = $msg->a2_4\n".
     "a2_5 = $msg->a2_5\n".
     "a2_6 = $msg->a2_6\n".
     "a2_7 = $msg->a2_7\n".
     "a3 = $msg->a3\n".
     "a3_1 = $msg->a3_1\n".
     "a3_2 = $msg->a3_2\n".
     "a3_3 = $msg->a3_3\n".
     "a3_4 = $msg->a3_4\n".
     "a3_5 = $msg->a3_5\n".
     "a3_6 = $msg->a3_6\n".
     "a3_7 = $msg->a3_7\n".
     "a3_8 = $msg->a3_8\n".
     "a3_9 = $msg->a3_9\n".
     "a3_10 = $msg->a3_10\n".
     "a3_11 = $msg->a3_11\n".
     "a3_12 = $msg->a3_12\n".
     "a3_13 = $msg->a3_13\n".
     "a3_14 = $msg->a3_14\n".
     "a3_15 = $msg->a3_15\n".
     "a3_16 = $msg->a3_16\n".
     "a3_17 = $msg->a3_17\n".
     "a3_18 = $msg->a3_18\n".
     "a3_19 = $msg->a3_19\n".
     "a3_20 = $msg->a3_20\n".
     "a3_21 = $msg->a3_21\n".
     "a3_22 = $msg->a3_22\n".
     "a3_23 = $msg->a3_23\n".
     "a4 = $msg->a4\n".
     "a4_1 = $msg->a4_1\n".
     "a4_2 = $msg->a4_2\n".
     "a4_3 = $msg->a4_3\n".
     "a4_4 = $msg->a4_4\n".
     "a4_5 = $msg->a4_5\n".
     "a4_6 = $msg->a4_6\n".
     "a4_7 = $msg->a4_7\n".
     "a4_8 = $msg->a4_8\n".
     "a4_9 = $msg->a4_9\n".
     "a4_10 = $msg->a4_10\n".
     "a4_11 = $msg->a4_11\n".
     "a4_12 = $msg->a4_12\n".
     "a4_13 = $msg->a4_13\n".
     "a4_14 = $msg->a4_14\n".
     "a5 = $msg->a5\n".
     "a5_1 = $msg->a5_1\n".
     "a5_2 = $msg->a5_2\n".
     "a5_3 = $msg->a5_3\n".
     "a5_4 = $msg->a5_4\n".
     "a5_5 = $msg->a5_5\n".
     "a5_6 = $msg->a5_6\n".
     "a5_7 = $msg->a5_7\n".
     "a5_8 = $msg->a5_8\n".
     "a5_9 = $msg->a5_9\n".
     "a5_10 = $msg->a5_10\n".
     "a5_11 = $msg->a5_11\n".
     "a5_12 = $msg->a5_12\n".
     "a5_13 = $msg->a5_13\n".
     "a5_14 = $msg->a5_14\n".
     "a5_15 = $msg->a5_15\n".
     "a5_16 = $msg->a5_16\n".
     "a5_17 = $msg->a5_17\n".
     "a5_18 = $msg->a5_18\n".
     "a5_19 = $msg->a5_19\n".
     "a5_20 = $msg->a5_20\n".
     "a5_21 = $msg->a5_21\n".
     "a5_22 = $msg->a5_22\n".
     "a5_23 = $msg->a5_23\n".
     "a5_24 = $msg->a5_24\n".
     "a5_25 = $msg->a5_25\n".
     "a5_26 = $msg->a5_26\n".
     "a5_27 = $msg->a5_27\n".
     "a5_28 = $msg->a5_28\n".
     "a5_29 = $msg->a5_29\n".
     "a5_30 = $msg->a5_30\n".
     "a5_31 = $msg->a5_31\n".
     "a5_32 = $msg->a5_32\n".
     "a6 = $msg->a6\n".
     "a6_1 = $msg->a6_1\n".
     "a6_2 = $msg->a6_2\n".
     "a6_3 = $msg->a6_3\n".
     "a6_4 = $msg->a6_4\n".
     "a6_5 = $msg->a6_5\n".
     "a6_6 = $msg->a6_6\n".
     "a6_7 = $msg->a6_7\n".
     "a6_8 = $msg->a6_8\n".
     "a6_9 = $msg->a6_9\n".
     "a6_10 = $msg->a6_10\n".
     "a6_11 = $msg->a6_11\n".
     "a6_12 = $msg->a6_12\n".
     "a6_13 = $msg->a6_13\n".
     "a6_14 = $msg->a6_14\n".
     "a6_15 = $msg->a6_15\n".
     "a6_16 = $msg->a6_16\n".
     "a6_17 = $msg->a6_17\n".
     "a7 = ".$msg->a7->getValue()."\n".
     "a7_1 = ".$msg->a7_1->getValue()."\n".
     "a7_2 = ".$msg->a7_2->getValue()."\n".
     "a7_3 = ".$msg->a7_3->getValue()."\n".
     "a7_4 = ".$msg->a7_4->getValue()."\n".
     "a7_5 = ".$msg->a7_5->getValue()."\n".
     "a7_6 = ".$msg->a7_6->getValue()."\n".
     "a7_7 = ".$msg->a7_7->getValue()."\n".
     "a7_8 = ".$msg->a7_8->getValue()."\n".
     "a7_9 = ".$msg->a7_9->getValue()."\n".
     "a7_10 = ".$msg->a7_10->getValue()."\n".
     "a7_11 = ".$msg->a7_11->getValue()."\n".
     "a7_12 = ".$msg->a7_12->getValue()."\n".
     "a7_13 = ".$msg->a7_13->getValue()."\n".
     "a7_14 = ".$msg->a7_14->getValue()."\n".
     "a7_15 = ".$msg->a7_15->getValue()."\n".
     "a7_16 = ".$msg->a7_16->getValue()."\n".
     "a7_17 = ".$msg->a7_17->getValue()."\n".
     "a7_18 = ".$msg->a7_18->getValue()."\n".
     "a7_19 = ".$msg->a7_19->getValue()."\n".
     "a7_20 = ".$msg->a7_20->getValue()."\n".
     "a7_21 = ".$msg->a7_21->getValue()."\n".
     "a7_22 = ".$msg->a7_22->getValue()."\n".
     "a7_23 = ".$msg->a7_23->getValue()."\n".
     "a7_24 = ".$msg->a7_24->getValue()."\n".
     "a7_25 = ".$msg->a7_25->getValue()."\n".
     "a7_26 = ".$msg->a7_26->getValue()."\n".
     "a7_27 = ".$msg->a7_27->getValue()."\n".
     "a7_28 = ".$msg->a7_28->getValue()."\n".
     "a7_29 = ".$msg->a7_29->getValue()."\n".
     "a7_30 = ".$msg->a7_30->getValue()."\n".
     "a7_31 = ".$msg->a7_31->getValue()."\n".
     "a7_32 = ".$msg->a7_32->getValue()."\n".
     "a7_33 = ".$msg->a7_33->getValue()."\n".
     "a7_34 = ".$msg->a7_34->getValue()."\n".
     "a7_35 = ".$msg->a7_35->getValue()."\n".
     "a7_36 = ".$msg->a7_36->getValue()."\n".
     "a7_37 = ".$msg->a7_37->getValue()."\n".
     "a7_38 = ".$msg->a7_38->getValue()."\n".
     "a7_39 = ".$msg->a7_39->getValue()."\n".
     "a7_40 = ".$msg->a7_40->getValue()."\n".
     "a7_41 = ".$msg->a7_41->getValue()."\n".
     "a7_42 = ".$msg->a7_42->getValue()."\n".
     "a7_43 = ".$msg->a7_43->getValue()."\n".
     "a7_44 = ".$msg->a7_44->getValue()."\n".
     "a7_45 = ".$msg->a7_45->getValue()."\n".
     "a7_46 = ".$msg->a7_46->getValue()."\n".
     "a7_47 = ".$msg->a7_47->getValue()."\n".
     "a7_48 = ".$msg->a7_48->getValue()."\n".
     "a7_49 = ".$msg->a7_49->getValue()."\n".
     "a7_50 = ".$msg->a7_50->getValue()."\n".
     "a8 = ".$msg->a8->getValue()."\n".
     "a8_1 = ".$msg->a8_1->getValue()."\n".
     "a8_2 = ".$msg->a8_2->getValue()."\n".
     "a8_3 = ".$msg->a8_3->getValue()."\n".
     "a8_4 = ".$msg->a8_4->getValue()."\n".
     "a8_5 = ".$msg->a8_5->getValue()."\n".
     "a8_6 = ".$msg->a8_6->getValue()."\n".
     "a8_7 = ".$msg->a8_7->getValue()."\n".
     "a8_8 = ".$msg->a8_8->getValue()."\n".
     "a8_9 = ".$msg->a8_9->getValue()."\n".
     "a8_10 = ".$msg->a8_10->getValue()."\n".
     "a8_11 = ".$msg->a8_11->getValue()."\n".
     "a8_12 = ".$msg->a8_12->getValue()."\n".
     "a8_13 = ".$msg->a8_13->getValue()."\n".
     "a8_14 = ".$msg->a8_14->getValue()."\n".
     "a8_15 = ".$msg->a8_15->getValue()."\n".
     "a8_16 = ".$msg->a8_16->getValue()."\n".
     "a8_17 = ".$msg->a8_17->getValue()."\n".
     "a8_18 = ".$msg->a8_18->getValue()."\n".
     "a8_19 = ".$msg->a8_19->getValue()."\n".
     "a8_20 = ".$msg->a8_20->getValue()."\n".
     "a8_21 = ".$msg->a8_21->getValue()."\n".
     "a8_22 = ".$msg->a8_22->getValue()."\n".
     "a8_23 = ".$msg->a8_23->getValue()."\n".
     "a8_24 = ".$msg->a8_24->getValue()."\n".
     "a8_25 = ".$msg->a8_25->getValue()."\n".
     "a8_26 = ".$msg->a8_26->getValue()."\n".
     "a8_27 = ".$msg->a8_27->getValue()."\n".
     "a9 = $msg->a9\n".
     "a10 = $msg->a10\n".
     "a11 = $msg->a11\n".
     "a12 = $msg->a12\n".
     "a13 = $msg->a13\n".
     "a13_1 = $msg->a13_1\n".
     "a13_2 = $msg->a13_2\n".
     "a13_3 = $msg->a13_3\n".
     "a13_4 = $msg->a13_4\n".
     "a13_5 = $msg->a13_5\n".
     "a13_6 = $msg->a13_6\n".
     "a13_7 = $msg->a13_7\n".
     "a13_8 = $msg->a13_8\n".
     "a13_9 = $msg->a13_9\n".
     "a13_10 = $msg->a13_10\n".
     "a13_11 = $msg->a13_11\n".
     "a13_12 = $msg->a13_12\n".
     "a13_13 = $msg->a13_13\n".
     "a13_14 = $msg->a13_14\n".
     "a13_15 = $msg->a13_15\n".
     "a13_16 = $msg->a13_16\n".
     "a13_17 = $msg->a13_17\n".
     "a13_18 = $msg->a13_18\n".
     "a13_19 = $msg->a13_19\n".
     "a13_20 = $msg->a13_20\n".
     "a13_21 = $msg->a13_21\n".
     "a13_22 = $msg->a13_22\n".
     "a13_23 = $msg->a13_23\n".
     "a14 = $msg->a14\n".
     "a14_1 = $msg->a14_1\n".
     "a14_2 = $msg->a14_2\n".
     "a14_3 = $msg->a14_3\n".
     "a14_4 = $msg->a14_4\n".
     "a14_5 = $msg->a14_5\n".
     "a14_6 = $msg->a14_6\n".
     "a14_7 = $msg->a14_7\n".
     "a14_8 = $msg->a14_8\n".
     "a14_9 = $msg->a14_9\n".
     "a14_10 = $msg->a14_10\n".
     "a14_11 = $msg->a14_11\n".
     "a14_12 = $msg->a14_12\n".
     "a14_13 = $msg->a14_13\n".
     "a14_14 = $msg->a14_14\n".
     "a15 = $msg->a15\n".
     "a15_1 = $msg->a15_1\n".
     "a15_2 = $msg->a15_2\n".
     "a15_3 = $msg->a15_3\n".
     "a15_4 = $msg->a15_4\n".
     "a15_5 = $msg->a15_5\n".
     "a15_6 = $msg->a15_6\n".
     "a15_7 = $msg->a15_7\n".
     "a15_8 = $msg->a15_8\n".
     "a15_9 = $msg->a15_9\n".
     "a15_10 = $msg->a15_10\n".
     "a15_11 = $msg->a15_11\n".
     "a15_12 = $msg->a15_12\n".
     "a15_13 = $msg->a15_13\n".
     "a15_14 = $msg->a15_14\n".
     "a15_15 = $msg->a15_15\n".
     "a15_16 = $msg->a15_16\n".
     "a15_17 = $msg->a15_17\n".
     "a15_18 = $msg->a15_18\n".
     "a15_19 = $msg->a15_19\n".
     "a15_20 = $msg->a15_20\n".
     "a15_21 = $msg->a15_21\n".
     "a15_22 = $msg->a15_22\n".
     "a15_23 = $msg->a15_23\n".
     "a15_24 = $msg->a15_24\n".
     "a15_25 = $msg->a15_25\n".
     "a15_26 = $msg->a15_26\n".
     "a15_27 = $msg->a15_27\n".
     "a15_28 = $msg->a15_28\n".
     "a15_29 = $msg->a15_29\n".
     "a15_30 = $msg->a15_30\n".
     "a15_31 = $msg->a15_31\n".
     "a15_32 = $msg->a15_32\n".
     "a16 = $msg->a16\n".
     "a16_1 = $msg->a16_1\n".
     "a16_2 = $msg->a16_2\n".
     "a16_3 = $msg->a16_3\n".
     "a16_4 = $msg->a16_4\n".
     "a16_5 = $msg->a16_5\n".
     "a16_6 = $msg->a16_6\n".
     "a16_7 = $msg->a16_7\n".
     "a16_8 = $msg->a16_8\n".
     "a16_9 = $msg->a16_9\n".
     "a16_10 = $msg->a16_10\n".
     "a16_11 = $msg->a16_11\n".
     "a16_12 = $msg->a16_12\n".
     "a16_13 = $msg->a16_13\n".
     "a16_14 = $msg->a16_14\n".
     "a16_15 = $msg->a16_15\n".
     "a16_16 = $msg->a16_16\n".
     "a16_17 = $msg->a16_17\n".
     "a17 = ".$msg->a17->getValue()."\n".
     "a17_1 = ".$msg->a17_1->getValue()."\n".
     "a17_2 = ".$msg->a17_2->getValue()."\n".
     "a17_3 = ".$msg->a17_3->getValue()."\n".
     "a17_4 = ".$msg->a17_4->getValue()."\n".
     "a17_5 = ".$msg->a17_5->getValue()."\n".
     "a17_6 = ".$msg->a17_6->getValue()."\n".
     "a17_7 = ".$msg->a17_7->getValue()."\n".
     "a17_8 = ".$msg->a17_8->getValue()."\n".
     "a17_9 = ".$msg->a17_9->getValue()."\n".
     "a17_10 = ".$msg->a17_10->getValue()."\n".
     "a17_11 = ".$msg->a17_11->getValue()."\n".
     "a17_12 = ".$msg->a17_12->getValue()."\n".
     "a17_13 = ".$msg->a17_13->getValue()."\n".
     "a17_14 = ".$msg->a17_14->getValue()."\n".
     "a17_15 = ".$msg->a17_15->getValue()."\n".
     "a17_16 = ".$msg->a17_16->getValue()."\n".
     "a17_17 = ".$msg->a17_17->getValue()."\n".
     "a17_18 = ".$msg->a17_18->getValue()."\n".
     "a17_19 = ".$msg->a17_19->getValue()."\n".
     "a17_20 = ".$msg->a17_20->getValue()."\n".
     "a17_21 = ".$msg->a17_21->getValue()."\n".
     "a17_22 = ".$msg->a17_22->getValue()."\n".
     "a17_23 = ".$msg->a17_23->getValue()."\n".
     "a17_24 = ".$msg->a17_24->getValue()."\n".
     "a17_25 = ".$msg->a17_25->getValue()."\n".
     "a17_26 = ".$msg->a17_26->getValue()."\n".
     "a17_27 = ".$msg->a17_27->getValue()."\n".
     "a17_28 = ".$msg->a17_28->getValue()."\n".
     "a17_29 = ".$msg->a17_29->getValue()."\n".
     "a17_30 = ".$msg->a17_30->getValue()."\n".
     "a17_31 = ".$msg->a17_31->getValue()."\n".
     "a17_32 = ".$msg->a17_32->getValue()."\n".
     "a17_33 = ".$msg->a17_33->getValue()."\n".
     "a17_34 = ".$msg->a17_34->getValue()."\n".
     "a17_35 = ".$msg->a17_35->getValue()."\n".
     "a17_36 = ".$msg->a17_36->getValue()."\n".
     "a17_37 = ".$msg->a17_37->getValue()."\n".
     "a17_38 = ".$msg->a17_38->getValue()."\n".
     "a17_39 = ".$msg->a17_39->getValue()."\n".
     "a17_40 = ".$msg->a17_40->getValue()."\n".
     "a17_41 = ".$msg->a17_41->getValue()."\n".
     "a17_42 = ".$msg->a17_42->getValue()."\n".
     "a17_43 = ".$msg->a17_43->getValue()."\n".
     "a17_44 = ".$msg->a17_44->getValue()."\n".
     "a17_45 = ".$msg->a17_45->getValue()."\n".
     "a17_46 = ".$msg->a17_46->getValue()."\n".
     "a17_47 = ".$msg->a17_47->getValue()."\n".
     "a17_48 = ".$msg->a17_48->getValue()."\n".
     "a17_49 = ".$msg->a17_49->getValue()."\n".
     "a17_50 = ".$msg->a17_50->getValue()."\n".
     "a18 = ".$msg->a18->getValue()."\n".
     "a18_1 = ".$msg->a18_1->getValue()."\n".
     "a18_2 = ".$msg->a18_2->getValue()."\n".
     "a18_3 = ".$msg->a18_3->getValue()."\n".
     "a18_4 = ".$msg->a18_4->getValue()."\n".
     "a18_5 = ".$msg->a18_5->getValue()."\n".
     "a18_6 = ".$msg->a18_6->getValue()."\n".
     "a18_7 = ".$msg->a18_7->getValue()."\n".
     "a18_8 = ".$msg->a18_8->getValue()."\n".
     "a18_9 = ".$msg->a18_9->getValue()."\n".
     "a18_10 = ".$msg->a18_10->getValue()."\n".
     "a18_11 = ".$msg->a18_11->getValue()."\n".
     "a18_12 = ".$msg->a18_12->getValue()."\n".
     "a18_13 = ".$msg->a18_13->getValue()."\n".
     "a18_14 = ".$msg->a18_14->getValue()."\n".
     "a18_15 = ".$msg->a18_15->getValue()."\n".
     "a18_16 = ".$msg->a18_16->getValue()."\n".
     "a18_17 = ".$msg->a18_17->getValue()."\n".
     "a18_18 = ".$msg->a18_18->getValue()."\n".
     "a18_19 = ".$msg->a18_19->getValue()."\n".
     "a18_20 = ".$msg->a18_20->getValue()."\n".
     "a18_21 = ".$msg->a18_21->getValue()."\n".
     "a18_22 = ".$msg->a18_22->getValue()."\n".
     "a18_23 = ".$msg->a18_23->getValue()."\n".
     "a18_24 = ".$msg->a18_24->getValue()."\n".
     "a18_25 = ".$msg->a18_25->getValue()."\n".
     "a18_26 = ".$msg->a18_26->getValue()."\n".
     "a18_27 = ".$msg->a18_27->getValue()."\n".
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

if (file_put_contents("php.bin", $bin) === false) {
    exit(1);
}

exit(0);

/* end of main.php */
