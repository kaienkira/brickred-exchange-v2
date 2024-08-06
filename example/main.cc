#include <fstream>
#include <iostream>
#include <vector>

#include "message_type.h"
#include "message_test.h"

using namespace brickred::exchange;
using namespace protocol::client;

int main()
{
    std::vector<char> buffer(10 * 1024 * 1024);
    int id = 0;
    int encode_size = 0;

    // encode message to buffer
    {
        MsgTest msg;
        // i8
        msg.a1 = 0x7f;
        msg.a1_1 = -128;
        msg.a1_2 = -80;
        msg.a1_3 = -1;
        msg.a1_4 = 0;
        msg.a1_5 = 1;
        msg.a1_6 = 80;
        msg.a1_7 = 127;
        // u8
        msg.a2 = 0xff;
        msg.a2_1 = 0;
        msg.a2_2 = 1;
        msg.a2_3 = 80;
        msg.a2_4 = 127;
        msg.a2_5 = 128;
        msg.a2_6 = 180;
        msg.a2_7 = 255;
        // i16
        msg.a3 = 0x7fff;
        msg.a3_1 = -32768;
        msg.a3_2 = -16384;
        msg.a3_3 = -16383;
        msg.a3_4 = -10000;
        msg.a3_5 = -5000;
        msg.a3_6 = -2500;
        msg.a3_7 = -256;
        msg.a3_8 = -255;
        msg.a3_9 = -128;
        msg.a3_10 = -127;
        msg.a3_11 = -1;
        msg.a3_12 = 0;
        msg.a3_13 = 1;
        msg.a3_14 = 127;
        msg.a3_15 = 128;
        msg.a3_16 = 255;
        msg.a3_17 = 256;
        msg.a3_18 = 2500;
        msg.a3_19 = 5000;
        msg.a3_20 = 10000;
        msg.a3_21 = 16383;
        msg.a3_22 = 16384;
        msg.a3_23 = 32767;
        // u16
        msg.a4 = 0xffff;
        msg.a4_1 = 0;
        msg.a4_2 = 127;
        msg.a4_3 = 128;
        msg.a4_4 = 255;
        msg.a4_5 = 256;
        msg.a4_6 = 2500;
        msg.a4_7 = 5000;
        msg.a4_8 = 10000;
        msg.a4_9 = 16383;
        msg.a4_10 = 16384;
        msg.a4_11 = 32767;
        msg.a4_12 = 32768;
        msg.a4_13 = 50000;
        msg.a4_14 = 65535;
        // i32
        msg.a5 = 0x7fffffff;
        msg.a5_1 = -2147483648;
        msg.a5_2 = -2147483647;
        msg.a5_3 = -1000000000;
        msg.a5_4 = -16777216;
        msg.a5_5 = -16777215;
        msg.a5_6 = -65536;
        msg.a5_7 = -65535;
        msg.a5_8 = -32768;
        msg.a5_9 = -32767;
        msg.a5_10 = -16384;
        msg.a5_11 = -16383;
        msg.a5_12 = -256;
        msg.a5_13 = -255;
        msg.a5_14 = -128;
        msg.a5_15 = -127;
        msg.a5_16 = -1;
        msg.a5_17 = 0;
        msg.a5_18 = 1;
        msg.a5_19 = 127;
        msg.a5_20 = 128;
        msg.a5_21 = 255;
        msg.a5_22 = 256;
        msg.a5_23 = 16383;
        msg.a5_24 = 16384;
        msg.a5_25 = 32767;
        msg.a5_26 = 32768;
        msg.a5_27 = 65535;
        msg.a5_28 = 65536;
        msg.a5_29 = 16777215;
        msg.a5_30 = 16777216;
        msg.a5_31 = 1000000000;
        msg.a5_32 = 2147483647;
        // u32
        msg.a6 = 0xffffffff;
        msg.a6_1 = 0;
        msg.a6_2 = 127;
        msg.a6_3 = 128;
        msg.a6_4 = 255;
        msg.a6_5 = 256;
        msg.a6_6 = 16383;
        msg.a6_7 = 16384;
        msg.a6_8 = 32767;
        msg.a6_9 = 32768;
        msg.a6_10 = 65535;
        msg.a6_11 = 65536;
        msg.a6_12 = 16777215;
        msg.a6_13 = 16777216;
        msg.a6_14 = 1000000000;
        msg.a6_15 = 2147483647;
        msg.a6_16 = 2147483648;
        msg.a6_17 = 4294967295;
        // i64
        msg.a7 = 0x7fffffffffffffff;
        msg.a7_1 = -9223372036854775807 - 1;
        msg.a7_2 = -9223372036854775807;
        msg.a7_3 = -72057594037927936;
        msg.a7_4 = -72057594037927935;
        msg.a7_5 = -281474976710656;
        msg.a7_6 = -281474976710655;
        msg.a7_7 = -1099511627776;
        msg.a7_8 = -1099511627775;
        msg.a7_9 = -4294967296;
        msg.a7_10 = -4294967295;
        msg.a7_11 = -2147483648;
        msg.a7_12 = -2147483647;
        msg.a7_13 = -16777216;
        msg.a7_14 = -16777215;
        msg.a7_15 = -65536;
        msg.a7_16 = -65535;
        msg.a7_17 = -32768;
        msg.a7_18 = -32767;
        msg.a7_19 = -16384;
        msg.a7_20 = -16383;
        msg.a7_21 = -256;
        msg.a7_22 = -255;
        msg.a7_23 = -128;
        msg.a7_24 = -127;
        msg.a7_25 = -1;
        msg.a7_26 = 0;
        msg.a7_27 = 1;
        msg.a7_28 = 127;
        msg.a7_29 = 128;
        msg.a7_30 = 255;
        msg.a7_31 = 256;
        msg.a7_32 = 16383;
        msg.a7_33 = 16384;
        msg.a7_34 = 32767;
        msg.a7_35 = 32768;
        msg.a7_36 = 65535;
        msg.a7_37 = 65536;
        msg.a7_38 = 16777215;
        msg.a7_39 = 16777216;
        msg.a7_40 = 2147483647;
        msg.a7_41 = 2147483648;
        msg.a7_42 = 4294967295;
        msg.a7_43 = 4294967296;
        msg.a7_44 = 1099511627775;
        msg.a7_45 = 1099511627776;
        msg.a7_46 = 281474976710655;
        msg.a7_47 = 281474976710656;
        msg.a7_48 = 72057594037927935;
        msg.a7_49 = 72057594037927936;
        msg.a7_50 = 9223372036854775807;
        // u64
        msg.a8 = 0xffffffffffffffff;
        msg.a8_1 = 0;
        msg.a8_2 = 1;
        msg.a8_3 = 127;
        msg.a8_4 = 128;
        msg.a8_5 = 255;
        msg.a8_6 = 256;
        msg.a8_7 = 16383;
        msg.a8_8 = 16384;
        msg.a8_9 = 32767;
        msg.a8_10 = 32768;
        msg.a8_11 = 65535;
        msg.a8_12 = 65536;
        msg.a8_13 = 16777215;
        msg.a8_14 = 16777216;
        msg.a8_15 = 2147483647;
        msg.a8_16 = 2147483648;
        msg.a8_17 = 4294967295;
        msg.a8_18 = 4294967296;
        msg.a8_19 = 1099511627775;
        msg.a8_20 = 1099511627776;
        msg.a8_21 = 281474976710655;
        msg.a8_22 = 281474976710656;
        msg.a8_23 = 72057594037927935;
        msg.a8_24 = 72057594037927936;
        msg.a8_25 = 9223372036854775807;
        msg.a8_26 = 9223372036854775808UL;
        msg.a8_27 = 18446744073709551615UL;
        // string
        msg.a9 = "hello, world!";
        // bool
        msg.a10 = true;
        // attr.AttrType
        msg.a11 = AttrType::STR;
        // bytes
        msg.a12 = "hello, world!";
        // i16v
        msg.a13 = 0x7fff;
        msg.a13_1 = -32768;
        msg.a13_2 = -16384;
        msg.a13_3 = -16383;
        msg.a13_4 = -10000;
        msg.a13_5 = -5000;
        msg.a13_6 = -2500;
        msg.a13_7 = -256;
        msg.a13_8 = -255;
        msg.a13_9 = -128;
        msg.a13_10 = -127;
        msg.a13_11 = -1;
        msg.a13_12 = 0;
        msg.a13_13 = 1;
        msg.a13_14 = 127;
        msg.a13_15 = 128;
        msg.a13_16 = 255;
        msg.a13_17 = 256;
        msg.a13_18 = 2500;
        msg.a13_19 = 5000;
        msg.a13_20 = 10000;
        msg.a13_21 = 16383;
        msg.a13_22 = 16384;
        msg.a13_23 = 32767;
        // u16v
        msg.a14 = 0xffff;
        msg.a14_1 = 0;
        msg.a14_2 = 127;
        msg.a14_3 = 128;
        msg.a14_4 = 255;
        msg.a14_5 = 256;
        msg.a14_6 = 2500;
        msg.a14_7 = 5000;
        msg.a14_8 = 10000;
        msg.a14_9 = 16383;
        msg.a14_10 = 16384;
        msg.a14_11 = 32767;
        msg.a14_12 = 32768;
        msg.a14_13 = 50000;
        msg.a14_14 = 65535;
        // i32v
        msg.a15 = 0x7fffffff;
        msg.a15_1 = -2147483648;
        msg.a15_2 = -2147483647;
        msg.a15_3 = -1000000000;
        msg.a15_4 = -16777216;
        msg.a15_5 = -16777215;
        msg.a15_6 = -65536;
        msg.a15_7 = -65535;
        msg.a15_8 = -32768;
        msg.a15_9 = -32767;
        msg.a15_10 = -16384;
        msg.a15_11 = -16383;
        msg.a15_12 = -256;
        msg.a15_13 = -255;
        msg.a15_14 = -128;
        msg.a15_15 = -127;
        msg.a15_16 = -1;
        msg.a15_17 = 0;
        msg.a15_18 = 1;
        msg.a15_19 = 127;
        msg.a15_20 = 128;
        msg.a15_21 = 255;
        msg.a15_22 = 256;
        msg.a15_23 = 16383;
        msg.a15_24 = 16384;
        msg.a15_25 = 32767;
        msg.a15_26 = 32768;
        msg.a15_27 = 65535;
        msg.a15_28 = 65536;
        msg.a15_29 = 16777215;
        msg.a15_30 = 16777216;
        msg.a15_31 = 1000000000;
        msg.a15_32 = 2147483647;
        // u32v
        msg.a16 = 0xffffffff;
        msg.a16_1 = 0;
        msg.a16_2 = 127;
        msg.a16_3 = 128;
        msg.a16_4 = 255;
        msg.a16_5 = 256;
        msg.a16_6 = 16383;
        msg.a16_7 = 16384;
        msg.a16_8 = 32767;
        msg.a16_9 = 32768;
        msg.a16_10 = 65535;
        msg.a16_11 = 65536;
        msg.a16_12 = 16777215;
        msg.a16_13 = 16777216;
        msg.a16_14 = 1000000000;
        msg.a16_15 = 2147483647;
        msg.a16_16 = 2147483648;
        msg.a16_17 = 4294967295;
        // i64v
        msg.a17 = 0x7fffffffffffffff;
        msg.a17_1 = -9223372036854775807 - 1;
        msg.a17_2 = -9223372036854775807;
        msg.a17_3 = -72057594037927936;
        msg.a17_4 = -72057594037927935;
        msg.a17_5 = -281474976710656;
        msg.a17_6 = -281474976710655;
        msg.a17_7 = -1099511627776;
        msg.a17_8 = -1099511627775;
        msg.a17_9 = -4294967296;
        msg.a17_10 = -4294967295;
        msg.a17_11 = -2147483648;
        msg.a17_12 = -2147483647;
        msg.a17_13 = -16777216;
        msg.a17_14 = -16777215;
        msg.a17_15 = -65536;
        msg.a17_16 = -65535;
        msg.a17_17 = -32768;
        msg.a17_18 = -32767;
        msg.a17_19 = -16384;
        msg.a17_20 = -16383;
        msg.a17_21 = -256;
        msg.a17_22 = -255;
        msg.a17_23 = -128;
        msg.a17_24 = -127;
        msg.a17_25 = -1;
        msg.a17_26 = 0;
        msg.a17_27 = 1;
        msg.a17_28 = 127;
        msg.a17_29 = 128;
        msg.a17_30 = 255;
        msg.a17_31 = 256;
        msg.a17_32 = 16383;
        msg.a17_33 = 16384;
        msg.a17_34 = 32767;
        msg.a17_35 = 32768;
        msg.a17_36 = 65535;
        msg.a17_37 = 65536;
        msg.a17_38 = 16777215;
        msg.a17_39 = 16777216;
        msg.a17_40 = 2147483647;
        msg.a17_41 = 2147483648;
        msg.a17_42 = 4294967295;
        msg.a17_43 = 4294967296;
        msg.a17_44 = 1099511627775;
        msg.a17_45 = 1099511627776;
        msg.a17_46 = 281474976710655;
        msg.a17_47 = 281474976710656;
        msg.a17_48 = 72057594037927935;
        msg.a17_49 = 72057594037927936;
        msg.a17_50 = 9223372036854775807;
        // u64v
        msg.a18 = 0xffffffffffffffff;
        msg.a18_1 = 0;
        msg.a18_2 = 1;
        msg.a18_3 = 127;
        msg.a18_4 = 128;
        msg.a18_5 = 255;
        msg.a18_6 = 256;
        msg.a18_7 = 16383;
        msg.a18_8 = 16384;
        msg.a18_9 = 32767;
        msg.a18_10 = 32768;
        msg.a18_11 = 65535;
        msg.a18_12 = 65536;
        msg.a18_13 = 16777215;
        msg.a18_14 = 16777216;
        msg.a18_15 = 2147483647;
        msg.a18_16 = 2147483648;
        msg.a18_17 = 4294967295;
        msg.a18_18 = 4294967296;
        msg.a18_19 = 1099511627775;
        msg.a18_20 = 1099511627776;
        msg.a18_21 = 281474976710655;
        msg.a18_22 = 281474976710656;
        msg.a18_23 = 72057594037927935;
        msg.a18_24 = 72057594037927936;
        msg.a18_25 = 9223372036854775807;
        msg.a18_26 = 9223372036854775808UL;
        msg.a18_27 = 18446744073709551615UL;

        for (int i = 0; i < 254; ++i) {
            msg.b5.push_back(i);
        }
        for (int i = 0; i < 10; ++i) {
            msg.b7.push_back(msg.a7);
        }
        for (int i = 0; i < 10; ++i) {
            msg.b8.push_back(msg.a8);
        }

        for (int i = 0; i < 254; ++i) {
            msg.b15.push_back(i);
        }
        for (int i = 0; i < 10; ++i) {
            msg.b17.push_back(msg.a17);
        }
        for (int i = 0; i < 10; ++i) {
            msg.b18.push_back(msg.a18);
        }

        msg.set_c1(1);
        msg.set_c2(1);
        msg.clear_has_c1();

        msg.set_has_c3();
        for (int i = 0; i < 65536; ++i) {
            msg.c3.push_back(i);
        }

        // do encode
        encode_size = msg.encode(&buffer[0], buffer.size());
        if (-1 == encode_size) {
            std::cerr << "buffer is too small" << std::endl;
            return 1;
        }
        // get message id from type
        id = MessageType::id<MsgTest>::value;
    }

    // decode message from buffer
    {
        // create message by id
        BaseStruct *msg_decoded = MessageType::create(id);
        msg_decoded->decode(&buffer[0], encode_size);

        MsgTest *msg = (MsgTest *)msg_decoded;

        std::cout << "encode_size = " << encode_size << std::endl
                  << "a1 = " << (int)msg->a1 << std::endl
                  << "a1_1 = " << (int)msg->a1_1 << std::endl
                  << "a1_2 = " << (int)msg->a1_2 << std::endl
                  << "a1_3 = " << (int)msg->a1_3 << std::endl
                  << "a1_4 = " << (int)msg->a1_4 << std::endl
                  << "a1_5 = " << (int)msg->a1_5 << std::endl
                  << "a1_6 = " << (int)msg->a1_6 << std::endl
                  << "a1_7 = " << (int)msg->a1_7 << std::endl
                  << "a2 = " << (int)msg->a2 << std::endl
                  << "a2_1 = " << (int)msg->a2_1 << std::endl
                  << "a2_2 = " << (int)msg->a2_2 << std::endl
                  << "a2_3 = " << (int)msg->a2_3 << std::endl
                  << "a2_4 = " << (int)msg->a2_4 << std::endl
                  << "a2_5 = " << (int)msg->a2_5 << std::endl
                  << "a2_6 = " << (int)msg->a2_6 << std::endl
                  << "a2_7 = " << (int)msg->a2_7 << std::endl
                  << "a3 = " << msg->a3 << std::endl
                  << "a3_1 = " << msg->a3_1 << std::endl
                  << "a3_2 = " << msg->a3_2 << std::endl
                  << "a3_3 = " << msg->a3_3 << std::endl
                  << "a3_4 = " << msg->a3_4 << std::endl
                  << "a3_5 = " << msg->a3_5 << std::endl
                  << "a3_6 = " << msg->a3_6 << std::endl
                  << "a3_7 = " << msg->a3_7 << std::endl
                  << "a3_8 = " << msg->a3_8 << std::endl
                  << "a3_9 = " << msg->a3_9 << std::endl
                  << "a3_10 = " << msg->a3_10 << std::endl
                  << "a3_11 = " << msg->a3_11 << std::endl
                  << "a3_12 = " << msg->a3_12 << std::endl
                  << "a3_13 = " << msg->a3_13 << std::endl
                  << "a3_14 = " << msg->a3_14 << std::endl
                  << "a3_15 = " << msg->a3_15 << std::endl
                  << "a3_16 = " << msg->a3_16 << std::endl
                  << "a3_17 = " << msg->a3_17 << std::endl
                  << "a3_18 = " << msg->a3_18 << std::endl
                  << "a3_19 = " << msg->a3_19 << std::endl
                  << "a3_20 = " << msg->a3_20 << std::endl
                  << "a3_21 = " << msg->a3_21 << std::endl
                  << "a3_22 = " << msg->a3_22 << std::endl
                  << "a3_23 = " << msg->a3_23 << std::endl
                  << "a4 = " << msg->a4 << std::endl
                  << "a4_1 = " << msg->a4_1 << std::endl
                  << "a4_2 = " << msg->a4_2 << std::endl
                  << "a4_3 = " << msg->a4_3 << std::endl
                  << "a4_4 = " << msg->a4_4 << std::endl
                  << "a4_5 = " << msg->a4_5 << std::endl
                  << "a4_6 = " << msg->a4_6 << std::endl
                  << "a4_7 = " << msg->a4_7 << std::endl
                  << "a4_8 = " << msg->a4_8 << std::endl
                  << "a4_9 = " << msg->a4_9 << std::endl
                  << "a4_10 = " << msg->a4_10 << std::endl
                  << "a4_11 = " << msg->a4_11 << std::endl
                  << "a4_12 = " << msg->a4_12 << std::endl
                  << "a4_13 = " << msg->a4_13 << std::endl
                  << "a4_14 = " << msg->a4_14 << std::endl
                  << "a5 = " << msg->a5 << std::endl
                  << "a5_1 = " << msg->a5_1 << std::endl
                  << "a5_2 = " << msg->a5_2 << std::endl
                  << "a5_3 = " << msg->a5_3 << std::endl
                  << "a5_4 = " << msg->a5_4 << std::endl
                  << "a5_5 = " << msg->a5_5 << std::endl
                  << "a5_6 = " << msg->a5_6 << std::endl
                  << "a5_7 = " << msg->a5_7 << std::endl
                  << "a5_8 = " << msg->a5_8 << std::endl
                  << "a5_9 = " << msg->a5_9 << std::endl
                  << "a5_10 = " << msg->a5_10 << std::endl
                  << "a5_11 = " << msg->a5_11 << std::endl
                  << "a5_12 = " << msg->a5_12 << std::endl
                  << "a5_13 = " << msg->a5_13 << std::endl
                  << "a5_14 = " << msg->a5_14 << std::endl
                  << "a5_15 = " << msg->a5_15 << std::endl
                  << "a5_16 = " << msg->a5_16 << std::endl
                  << "a5_17 = " << msg->a5_17 << std::endl
                  << "a5_18 = " << msg->a5_18 << std::endl
                  << "a5_19 = " << msg->a5_19 << std::endl
                  << "a5_20 = " << msg->a5_20 << std::endl
                  << "a5_21 = " << msg->a5_21 << std::endl
                  << "a5_22 = " << msg->a5_22 << std::endl
                  << "a5_23 = " << msg->a5_23 << std::endl
                  << "a5_24 = " << msg->a5_24 << std::endl
                  << "a5_25 = " << msg->a5_25 << std::endl
                  << "a5_26 = " << msg->a5_26 << std::endl
                  << "a5_27 = " << msg->a5_27 << std::endl
                  << "a5_28 = " << msg->a5_28 << std::endl
                  << "a5_29 = " << msg->a5_29 << std::endl
                  << "a5_30 = " << msg->a5_30 << std::endl
                  << "a5_31 = " << msg->a5_31 << std::endl
                  << "a5_32 = " << msg->a5_32 << std::endl
                  << "a6 = " << msg->a6 << std::endl
                  << "a6_1 = " << msg->a6_1 << std::endl
                  << "a6_2 = " << msg->a6_2 << std::endl
                  << "a6_3 = " << msg->a6_3 << std::endl
                  << "a6_4 = " << msg->a6_4 << std::endl
                  << "a6_5 = " << msg->a6_5 << std::endl
                  << "a6_6 = " << msg->a6_6 << std::endl
                  << "a6_7 = " << msg->a6_7 << std::endl
                  << "a6_8 = " << msg->a6_8 << std::endl
                  << "a6_9 = " << msg->a6_9 << std::endl
                  << "a6_10 = " << msg->a6_10 << std::endl
                  << "a6_11 = " << msg->a6_11 << std::endl
                  << "a6_12 = " << msg->a6_12 << std::endl
                  << "a6_13 = " << msg->a6_13 << std::endl
                  << "a6_14 = " << msg->a6_14 << std::endl
                  << "a6_15 = " << msg->a6_15 << std::endl
                  << "a6_16 = " << msg->a6_16 << std::endl
                  << "a6_17 = " << msg->a6_17 << std::endl
                  << "a7 = " << msg->a7 << std::endl
                  << "a7_1 = " << msg->a7_1 << std::endl
                  << "a7_2 = " << msg->a7_2 << std::endl
                  << "a7_3 = " << msg->a7_3 << std::endl
                  << "a7_4 = " << msg->a7_4 << std::endl
                  << "a7_5 = " << msg->a7_5 << std::endl
                  << "a7_6 = " << msg->a7_6 << std::endl
                  << "a7_7 = " << msg->a7_7 << std::endl
                  << "a7_8 = " << msg->a7_8 << std::endl
                  << "a7_9 = " << msg->a7_9 << std::endl
                  << "a7_10 = " << msg->a7_10 << std::endl
                  << "a7_11 = " << msg->a7_11 << std::endl
                  << "a7_12 = " << msg->a7_12 << std::endl
                  << "a7_13 = " << msg->a7_13 << std::endl
                  << "a7_14 = " << msg->a7_14 << std::endl
                  << "a7_15 = " << msg->a7_15 << std::endl
                  << "a7_16 = " << msg->a7_16 << std::endl
                  << "a7_17 = " << msg->a7_17 << std::endl
                  << "a7_18 = " << msg->a7_18 << std::endl
                  << "a7_19 = " << msg->a7_19 << std::endl
                  << "a7_20 = " << msg->a7_20 << std::endl
                  << "a7_21 = " << msg->a7_21 << std::endl
                  << "a7_22 = " << msg->a7_22 << std::endl
                  << "a7_23 = " << msg->a7_23 << std::endl
                  << "a7_24 = " << msg->a7_24 << std::endl
                  << "a7_25 = " << msg->a7_25 << std::endl
                  << "a7_26 = " << msg->a7_26 << std::endl
                  << "a7_27 = " << msg->a7_27 << std::endl
                  << "a7_28 = " << msg->a7_28 << std::endl
                  << "a7_29 = " << msg->a7_29 << std::endl
                  << "a7_30 = " << msg->a7_30 << std::endl
                  << "a7_31 = " << msg->a7_31 << std::endl
                  << "a7_32 = " << msg->a7_32 << std::endl
                  << "a7_33 = " << msg->a7_33 << std::endl
                  << "a7_34 = " << msg->a7_34 << std::endl
                  << "a7_35 = " << msg->a7_35 << std::endl
                  << "a7_36 = " << msg->a7_36 << std::endl
                  << "a7_37 = " << msg->a7_37 << std::endl
                  << "a7_38 = " << msg->a7_38 << std::endl
                  << "a7_39 = " << msg->a7_39 << std::endl
                  << "a7_40 = " << msg->a7_40 << std::endl
                  << "a7_41 = " << msg->a7_41 << std::endl
                  << "a7_42 = " << msg->a7_42 << std::endl
                  << "a7_43 = " << msg->a7_43 << std::endl
                  << "a7_44 = " << msg->a7_44 << std::endl
                  << "a7_45 = " << msg->a7_45 << std::endl
                  << "a7_46 = " << msg->a7_46 << std::endl
                  << "a7_47 = " << msg->a7_47 << std::endl
                  << "a7_48 = " << msg->a7_48 << std::endl
                  << "a7_49 = " << msg->a7_49 << std::endl
                  << "a7_50 = " << msg->a7_50 << std::endl
                  << "a8 = " << msg->a8 << std::endl
                  << "a8_1 = " << msg->a8_1 << std::endl
                  << "a8_2 = " << msg->a8_2 << std::endl
                  << "a8_3 = " << msg->a8_3 << std::endl
                  << "a8_4 = " << msg->a8_4 << std::endl
                  << "a8_5 = " << msg->a8_5 << std::endl
                  << "a8_6 = " << msg->a8_6 << std::endl
                  << "a8_7 = " << msg->a8_7 << std::endl
                  << "a8_8 = " << msg->a8_8 << std::endl
                  << "a8_9 = " << msg->a8_9 << std::endl
                  << "a8_10 = " << msg->a8_10 << std::endl
                  << "a8_11 = " << msg->a8_11 << std::endl
                  << "a8_12 = " << msg->a8_12 << std::endl
                  << "a8_13 = " << msg->a8_13 << std::endl
                  << "a8_14 = " << msg->a8_14 << std::endl
                  << "a8_15 = " << msg->a8_15 << std::endl
                  << "a8_16 = " << msg->a8_16 << std::endl
                  << "a8_17 = " << msg->a8_17 << std::endl
                  << "a8_18 = " << msg->a8_18 << std::endl
                  << "a8_19 = " << msg->a8_19 << std::endl
                  << "a8_20 = " << msg->a8_20 << std::endl
                  << "a8_21 = " << msg->a8_21 << std::endl
                  << "a8_22 = " << msg->a8_22 << std::endl
                  << "a8_23 = " << msg->a8_23 << std::endl
                  << "a8_24 = " << msg->a8_24 << std::endl
                  << "a8_25 = " << msg->a8_25 << std::endl
                  << "a8_26 = " << msg->a8_26 << std::endl
                  << "a8_27 = " << msg->a8_27 << std::endl
                  << "a9 = " << msg->a9 << std::endl
                  << "a10 = " << msg->a10 << std::endl
                  << "a11 = " << msg->a11 << std::endl
                  << "a12 = " << msg->a12 << std::endl
                  << "a13 = " << msg->a13 << std::endl
                  << "a13_1 = " << msg->a13_1 << std::endl
                  << "a13_2 = " << msg->a13_2 << std::endl
                  << "a13_3 = " << msg->a13_3 << std::endl
                  << "a13_4 = " << msg->a13_4 << std::endl
                  << "a13_5 = " << msg->a13_5 << std::endl
                  << "a13_6 = " << msg->a13_6 << std::endl
                  << "a13_7 = " << msg->a13_7 << std::endl
                  << "a13_8 = " << msg->a13_8 << std::endl
                  << "a13_9 = " << msg->a13_9 << std::endl
                  << "a13_10 = " << msg->a13_10 << std::endl
                  << "a13_11 = " << msg->a13_11 << std::endl
                  << "a13_12 = " << msg->a13_12 << std::endl
                  << "a13_13 = " << msg->a13_13 << std::endl
                  << "a13_14 = " << msg->a13_14 << std::endl
                  << "a13_15 = " << msg->a13_15 << std::endl
                  << "a13_16 = " << msg->a13_16 << std::endl
                  << "a13_17 = " << msg->a13_17 << std::endl
                  << "a13_18 = " << msg->a13_18 << std::endl
                  << "a13_19 = " << msg->a13_19 << std::endl
                  << "a13_20 = " << msg->a13_20 << std::endl
                  << "a13_21 = " << msg->a13_21 << std::endl
                  << "a13_22 = " << msg->a13_22 << std::endl
                  << "a13_23 = " << msg->a13_23 << std::endl
                  << "a14 = " << msg->a14 << std::endl
                  << "a14_1 = " << msg->a14_1 << std::endl
                  << "a14_2 = " << msg->a14_2 << std::endl
                  << "a14_3 = " << msg->a14_3 << std::endl
                  << "a14_4 = " << msg->a14_4 << std::endl
                  << "a14_5 = " << msg->a14_5 << std::endl
                  << "a14_6 = " << msg->a14_6 << std::endl
                  << "a14_7 = " << msg->a14_7 << std::endl
                  << "a14_8 = " << msg->a14_8 << std::endl
                  << "a14_9 = " << msg->a14_9 << std::endl
                  << "a14_10 = " << msg->a14_10 << std::endl
                  << "a14_11 = " << msg->a14_11 << std::endl
                  << "a14_12 = " << msg->a14_12 << std::endl
                  << "a14_13 = " << msg->a14_13 << std::endl
                  << "a14_14 = " << msg->a14_14 << std::endl
                  << "a15 = " << msg->a15 << std::endl
                  << "a15_1 = " << msg->a15_1 << std::endl
                  << "a15_2 = " << msg->a15_2 << std::endl
                  << "a15_3 = " << msg->a15_3 << std::endl
                  << "a15_4 = " << msg->a15_4 << std::endl
                  << "a15_5 = " << msg->a15_5 << std::endl
                  << "a15_6 = " << msg->a15_6 << std::endl
                  << "a15_7 = " << msg->a15_7 << std::endl
                  << "a15_8 = " << msg->a15_8 << std::endl
                  << "a15_9 = " << msg->a15_9 << std::endl
                  << "a15_10 = " << msg->a15_10 << std::endl
                  << "a15_11 = " << msg->a15_11 << std::endl
                  << "a15_12 = " << msg->a15_12 << std::endl
                  << "a15_13 = " << msg->a15_13 << std::endl
                  << "a15_14 = " << msg->a15_14 << std::endl
                  << "a15_15 = " << msg->a15_15 << std::endl
                  << "a15_16 = " << msg->a15_16 << std::endl
                  << "a15_17 = " << msg->a15_17 << std::endl
                  << "a15_18 = " << msg->a15_18 << std::endl
                  << "a15_19 = " << msg->a15_19 << std::endl
                  << "a15_20 = " << msg->a15_20 << std::endl
                  << "a15_21 = " << msg->a15_21 << std::endl
                  << "a15_22 = " << msg->a15_22 << std::endl
                  << "a15_23 = " << msg->a15_23 << std::endl
                  << "a15_24 = " << msg->a15_24 << std::endl
                  << "a15_25 = " << msg->a15_25 << std::endl
                  << "a15_26 = " << msg->a15_26 << std::endl
                  << "a15_27 = " << msg->a15_27 << std::endl
                  << "a15_28 = " << msg->a15_28 << std::endl
                  << "a15_29 = " << msg->a15_29 << std::endl
                  << "a15_30 = " << msg->a15_30 << std::endl
                  << "a15_31 = " << msg->a15_31 << std::endl
                  << "a15_32 = " << msg->a15_32 << std::endl
                  << "a16 = " << msg->a16 << std::endl
                  << "a16_1 = " << msg->a16_1 << std::endl
                  << "a16_2 = " << msg->a16_2 << std::endl
                  << "a16_3 = " << msg->a16_3 << std::endl
                  << "a16_4 = " << msg->a16_4 << std::endl
                  << "a16_5 = " << msg->a16_5 << std::endl
                  << "a16_6 = " << msg->a16_6 << std::endl
                  << "a16_7 = " << msg->a16_7 << std::endl
                  << "a16_8 = " << msg->a16_8 << std::endl
                  << "a16_9 = " << msg->a16_9 << std::endl
                  << "a16_10 = " << msg->a16_10 << std::endl
                  << "a16_11 = " << msg->a16_11 << std::endl
                  << "a16_12 = " << msg->a16_12 << std::endl
                  << "a16_13 = " << msg->a16_13 << std::endl
                  << "a16_14 = " << msg->a16_14 << std::endl
                  << "a16_15 = " << msg->a16_15 << std::endl
                  << "a16_16 = " << msg->a16_16 << std::endl
                  << "a16_17 = " << msg->a16_17 << std::endl
                  << "a17 = " << msg->a17 << std::endl
                  << "a17_1 = " << msg->a17_1 << std::endl
                  << "a17_2 = " << msg->a17_2 << std::endl
                  << "a17_3 = " << msg->a17_3 << std::endl
                  << "a17_4 = " << msg->a17_4 << std::endl
                  << "a17_5 = " << msg->a17_5 << std::endl
                  << "a17_6 = " << msg->a17_6 << std::endl
                  << "a17_7 = " << msg->a17_7 << std::endl
                  << "a17_8 = " << msg->a17_8 << std::endl
                  << "a17_9 = " << msg->a17_9 << std::endl
                  << "a17_10 = " << msg->a17_10 << std::endl
                  << "a17_11 = " << msg->a17_11 << std::endl
                  << "a17_12 = " << msg->a17_12 << std::endl
                  << "a17_13 = " << msg->a17_13 << std::endl
                  << "a17_14 = " << msg->a17_14 << std::endl
                  << "a17_15 = " << msg->a17_15 << std::endl
                  << "a17_16 = " << msg->a17_16 << std::endl
                  << "a17_17 = " << msg->a17_17 << std::endl
                  << "a17_18 = " << msg->a17_18 << std::endl
                  << "a17_19 = " << msg->a17_19 << std::endl
                  << "a17_20 = " << msg->a17_20 << std::endl
                  << "a17_21 = " << msg->a17_21 << std::endl
                  << "a17_22 = " << msg->a17_22 << std::endl
                  << "a17_23 = " << msg->a17_23 << std::endl
                  << "a17_24 = " << msg->a17_24 << std::endl
                  << "a17_25 = " << msg->a17_25 << std::endl
                  << "a17_26 = " << msg->a17_26 << std::endl
                  << "a17_27 = " << msg->a17_27 << std::endl
                  << "a17_28 = " << msg->a17_28 << std::endl
                  << "a17_29 = " << msg->a17_29 << std::endl
                  << "a17_30 = " << msg->a17_30 << std::endl
                  << "a17_31 = " << msg->a17_31 << std::endl
                  << "a17_32 = " << msg->a17_32 << std::endl
                  << "a17_33 = " << msg->a17_33 << std::endl
                  << "a17_34 = " << msg->a17_34 << std::endl
                  << "a17_35 = " << msg->a17_35 << std::endl
                  << "a17_36 = " << msg->a17_36 << std::endl
                  << "a17_37 = " << msg->a17_37 << std::endl
                  << "a17_38 = " << msg->a17_38 << std::endl
                  << "a17_39 = " << msg->a17_39 << std::endl
                  << "a17_40 = " << msg->a17_40 << std::endl
                  << "a17_41 = " << msg->a17_41 << std::endl
                  << "a17_42 = " << msg->a17_42 << std::endl
                  << "a17_43 = " << msg->a17_43 << std::endl
                  << "a17_44 = " << msg->a17_44 << std::endl
                  << "a17_45 = " << msg->a17_45 << std::endl
                  << "a17_46 = " << msg->a17_46 << std::endl
                  << "a17_47 = " << msg->a17_47 << std::endl
                  << "a17_48 = " << msg->a17_48 << std::endl
                  << "a17_49 = " << msg->a17_49 << std::endl
                  << "a17_50 = " << msg->a17_50 << std::endl
                  << "a18 = " << msg->a18 << std::endl
                  << "a18_1 = " << msg->a18_1 << std::endl
                  << "a18_2 = " << msg->a18_2 << std::endl
                  << "a18_3 = " << msg->a18_3 << std::endl
                  << "a18_4 = " << msg->a18_4 << std::endl
                  << "a18_5 = " << msg->a18_5 << std::endl
                  << "a18_6 = " << msg->a18_6 << std::endl
                  << "a18_7 = " << msg->a18_7 << std::endl
                  << "a18_8 = " << msg->a18_8 << std::endl
                  << "a18_9 = " << msg->a18_9 << std::endl
                  << "a18_10 = " << msg->a18_10 << std::endl
                  << "a18_11 = " << msg->a18_11 << std::endl
                  << "a18_12 = " << msg->a18_12 << std::endl
                  << "a18_13 = " << msg->a18_13 << std::endl
                  << "a18_14 = " << msg->a18_14 << std::endl
                  << "a18_15 = " << msg->a18_15 << std::endl
                  << "a18_16 = " << msg->a18_16 << std::endl
                  << "a18_17 = " << msg->a18_17 << std::endl
                  << "a18_18 = " << msg->a18_18 << std::endl
                  << "a18_19 = " << msg->a18_19 << std::endl
                  << "a18_20 = " << msg->a18_20 << std::endl
                  << "a18_21 = " << msg->a18_21 << std::endl
                  << "a18_22 = " << msg->a18_22 << std::endl
                  << "a18_23 = " << msg->a18_23 << std::endl
                  << "a18_24 = " << msg->a18_24 << std::endl
                  << "a18_25 = " << msg->a18_25 << std::endl
                  << "a18_26 = " << msg->a18_26 << std::endl
                  << "a18_27 = " << msg->a18_27 << std::endl
                  << "b5 size = " << msg->b5.size() << std::endl
                  << "b5[253] = " << msg->b5[253] << std::endl
                  << "b7 size = " << msg->b7.size() << std::endl
                  << "b7[0] = " << msg->b7[0] << std::endl
                  << "b8 size = " << msg->b8.size() << std::endl
                  << "b8[0] = " << msg->b8[0] << std::endl
                  << "has c1 = " << msg->has_c1() << std::endl
                  << "c1 = " << msg->c1 << std::endl
                  << "has c2 = " << msg->has_c2() << std::endl
                  << "c2 = " << msg->c2 << std::endl
                  << "has c3 = " << msg->has_c3() << std::endl
                  << "c3 size = " << msg->c3.size() << std::endl
                  << "c3[65535] = " << msg->c3[65535] << std::endl;

        delete msg;
    }

    std::ofstream fs("cpp.bin", std::ios::binary);
    fs.write((char *)&buffer[0], encode_size);
    fs.close();
    if (fs.bad()) {
        return 1;
    }

    return 0;
}
