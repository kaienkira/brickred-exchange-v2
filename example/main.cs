using Brickred.Exchange;
using Protocol.Client;
using System;
using System.IO;
using System.Text;

public class App
{
    public static int Main()
    {
        byte[] buffer = new byte[10 * 1024 * 1024];
        int id = 0;
        int encode_size = 0;

        // encode message to buffer
        {
            MsgTest msg = new MsgTest();
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
            msg.a7_1 = -9223372036854775808;
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
            msg.a8_26 = 9223372036854775808;
            msg.a8_27 = 18446744073709551615;
            // string
            msg.a9 = "hello, world!";
            // bool
            msg.a10 = true;
            // attr.AttrType
            msg.a11 = AttrType.STR;
            // bytes
            msg.a12 = Encoding.ASCII.GetBytes("hello, world!");
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
            msg.a17_1 = -9223372036854775808;
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
            msg.a18_26 = 9223372036854775808;
            msg.a18_27 = 18446744073709551615;

            for (int i = 0; i < 254; ++i) {
                msg.b5.Add(i);
            }
            for (int i = 0; i < 10; ++i) {
                msg.b7.Add(msg.a7);
            }
            for (int i = 0; i < 10; ++i) {
                msg.b8.Add(msg.a8);
            }

            for (int i = 0; i < 254; ++i) {
                msg.b15.Add(i);
            }
            for (int i = 0; i < 10; ++i) {
                msg.b17.Add(msg.a17);
            }
            for (int i = 0; i < 10; ++i) {
                msg.b18.Add(msg.a18);
            }

            msg.set_c1(1);
            msg.set_c2(1);
            msg.clear_has_c1();

            msg.set_has_c3();
            for (int i = 0; i < 65536; ++i) {
                msg.c3.Add(i);
            }

            // do encode
            encode_size = msg.Encode(buffer);
            if (-1 == encode_size) {
                Console.WriteLine("buffer is too small");
                return 1;
            }
            // get message id from type
            id = MessageType.GetId<MsgTest>();
        }

        // decode message from buffer
        {
            BaseStruct msg_decoded = MessageType.Create(id);
            msg_decoded.Decode(buffer, 0, encode_size);
            MsgTest msg = (MsgTest)msg_decoded;

            StringBuilder s = new StringBuilder();
            s.AppendFormat("encode_size = {0}\n", encode_size);
            s.AppendFormat("a1 = {0}\n", msg.a1);
            s.AppendFormat("a1_1 = {0}\n", msg.a1_1);
            s.AppendFormat("a1_2 = {0}\n", msg.a1_2);
            s.AppendFormat("a1_3 = {0}\n", msg.a1_3);
            s.AppendFormat("a1_4 = {0}\n", msg.a1_4);
            s.AppendFormat("a1_5 = {0}\n", msg.a1_5);
            s.AppendFormat("a1_6 = {0}\n", msg.a1_6);
            s.AppendFormat("a1_7 = {0}\n", msg.a1_7);
            s.AppendFormat("a2 = {0}\n", msg.a2);
            s.AppendFormat("a2_1 = {0}\n", msg.a2_1);
            s.AppendFormat("a2_2 = {0}\n", msg.a2_2);
            s.AppendFormat("a2_3 = {0}\n", msg.a2_3);
            s.AppendFormat("a2_4 = {0}\n", msg.a2_4);
            s.AppendFormat("a2_5 = {0}\n", msg.a2_5);
            s.AppendFormat("a2_6 = {0}\n", msg.a2_6);
            s.AppendFormat("a2_7 = {0}\n", msg.a2_7);
            s.AppendFormat("a3 = {0}\n", msg.a3);
            s.AppendFormat("a3_1 = {0}\n", msg.a3_1);
            s.AppendFormat("a3_2 = {0}\n", msg.a3_2);
            s.AppendFormat("a3_3 = {0}\n", msg.a3_3);
            s.AppendFormat("a3_4 = {0}\n", msg.a3_4);
            s.AppendFormat("a3_5 = {0}\n", msg.a3_5);
            s.AppendFormat("a3_6 = {0}\n", msg.a3_6);
            s.AppendFormat("a3_7 = {0}\n", msg.a3_7);
            s.AppendFormat("a3_8 = {0}\n", msg.a3_8);
            s.AppendFormat("a3_9 = {0}\n", msg.a3_9);
            s.AppendFormat("a3_10 = {0}\n", msg.a3_10);
            s.AppendFormat("a3_11 = {0}\n", msg.a3_11);
            s.AppendFormat("a3_12 = {0}\n", msg.a3_12);
            s.AppendFormat("a3_13 = {0}\n", msg.a3_13);
            s.AppendFormat("a3_14 = {0}\n", msg.a3_14);
            s.AppendFormat("a3_15 = {0}\n", msg.a3_15);
            s.AppendFormat("a3_16 = {0}\n", msg.a3_16);
            s.AppendFormat("a3_17 = {0}\n", msg.a3_17);
            s.AppendFormat("a3_18 = {0}\n", msg.a3_18);
            s.AppendFormat("a3_19 = {0}\n", msg.a3_19);
            s.AppendFormat("a3_20 = {0}\n", msg.a3_20);
            s.AppendFormat("a3_21 = {0}\n", msg.a3_21);
            s.AppendFormat("a3_22 = {0}\n", msg.a3_22);
            s.AppendFormat("a3_23 = {0}\n", msg.a3_23);
            s.AppendFormat("a4 = {0}\n", msg.a4);
            s.AppendFormat("a4_1 = {0}\n", msg.a4_1);
            s.AppendFormat("a4_2 = {0}\n", msg.a4_2);
            s.AppendFormat("a4_3 = {0}\n", msg.a4_3);
            s.AppendFormat("a4_4 = {0}\n", msg.a4_4);
            s.AppendFormat("a4_5 = {0}\n", msg.a4_5);
            s.AppendFormat("a4_6 = {0}\n", msg.a4_6);
            s.AppendFormat("a4_7 = {0}\n", msg.a4_7);
            s.AppendFormat("a4_8 = {0}\n", msg.a4_8);
            s.AppendFormat("a4_9 = {0}\n", msg.a4_9);
            s.AppendFormat("a4_10 = {0}\n", msg.a4_10);
            s.AppendFormat("a4_11 = {0}\n", msg.a4_11);
            s.AppendFormat("a4_12 = {0}\n", msg.a4_12);
            s.AppendFormat("a4_13 = {0}\n", msg.a4_13);
            s.AppendFormat("a4_14 = {0}\n", msg.a4_14);
            s.AppendFormat("a5 = {0}\n", msg.a5);
            s.AppendFormat("a5_1 = {0}\n", msg.a5_1);
            s.AppendFormat("a5_2 = {0}\n", msg.a5_2);
            s.AppendFormat("a5_3 = {0}\n", msg.a5_3);
            s.AppendFormat("a5_4 = {0}\n", msg.a5_4);
            s.AppendFormat("a5_5 = {0}\n", msg.a5_5);
            s.AppendFormat("a5_6 = {0}\n", msg.a5_6);
            s.AppendFormat("a5_7 = {0}\n", msg.a5_7);
            s.AppendFormat("a5_8 = {0}\n", msg.a5_8);
            s.AppendFormat("a5_9 = {0}\n", msg.a5_9);
            s.AppendFormat("a5_10 = {0}\n", msg.a5_10);
            s.AppendFormat("a5_11 = {0}\n", msg.a5_11);
            s.AppendFormat("a5_12 = {0}\n", msg.a5_12);
            s.AppendFormat("a5_13 = {0}\n", msg.a5_13);
            s.AppendFormat("a5_14 = {0}\n", msg.a5_14);
            s.AppendFormat("a5_15 = {0}\n", msg.a5_15);
            s.AppendFormat("a5_16 = {0}\n", msg.a5_16);
            s.AppendFormat("a5_17 = {0}\n", msg.a5_17);
            s.AppendFormat("a5_18 = {0}\n", msg.a5_18);
            s.AppendFormat("a5_19 = {0}\n", msg.a5_19);
            s.AppendFormat("a5_20 = {0}\n", msg.a5_20);
            s.AppendFormat("a5_21 = {0}\n", msg.a5_21);
            s.AppendFormat("a5_22 = {0}\n", msg.a5_22);
            s.AppendFormat("a5_23 = {0}\n", msg.a5_23);
            s.AppendFormat("a5_24 = {0}\n", msg.a5_24);
            s.AppendFormat("a5_25 = {0}\n", msg.a5_25);
            s.AppendFormat("a5_26 = {0}\n", msg.a5_26);
            s.AppendFormat("a5_27 = {0}\n", msg.a5_27);
            s.AppendFormat("a5_28 = {0}\n", msg.a5_28);
            s.AppendFormat("a5_29 = {0}\n", msg.a5_29);
            s.AppendFormat("a5_30 = {0}\n", msg.a5_30);
            s.AppendFormat("a5_31 = {0}\n", msg.a5_31);
            s.AppendFormat("a5_32 = {0}\n", msg.a5_32);
            s.AppendFormat("a6 = {0}\n", msg.a6);
            s.AppendFormat("a6_1 = {0}\n", msg.a6_1);
            s.AppendFormat("a6_2 = {0}\n", msg.a6_2);
            s.AppendFormat("a6_3 = {0}\n", msg.a6_3);
            s.AppendFormat("a6_4 = {0}\n", msg.a6_4);
            s.AppendFormat("a6_5 = {0}\n", msg.a6_5);
            s.AppendFormat("a6_6 = {0}\n", msg.a6_6);
            s.AppendFormat("a6_7 = {0}\n", msg.a6_7);
            s.AppendFormat("a6_8 = {0}\n", msg.a6_8);
            s.AppendFormat("a6_9 = {0}\n", msg.a6_9);
            s.AppendFormat("a6_10 = {0}\n", msg.a6_10);
            s.AppendFormat("a6_11 = {0}\n", msg.a6_11);
            s.AppendFormat("a6_12 = {0}\n", msg.a6_12);
            s.AppendFormat("a6_13 = {0}\n", msg.a6_13);
            s.AppendFormat("a6_14 = {0}\n", msg.a6_14);
            s.AppendFormat("a6_15 = {0}\n", msg.a6_15);
            s.AppendFormat("a6_16 = {0}\n", msg.a6_16);
            s.AppendFormat("a6_17 = {0}\n", msg.a6_17);
            s.AppendFormat("a7 = {0}\n", msg.a7);
            s.AppendFormat("a7_1 = {0}\n", msg.a7_1);
            s.AppendFormat("a7_2 = {0}\n", msg.a7_2);
            s.AppendFormat("a7_3 = {0}\n", msg.a7_3);
            s.AppendFormat("a7_4 = {0}\n", msg.a7_4);
            s.AppendFormat("a7_5 = {0}\n", msg.a7_5);
            s.AppendFormat("a7_6 = {0}\n", msg.a7_6);
            s.AppendFormat("a7_7 = {0}\n", msg.a7_7);
            s.AppendFormat("a7_8 = {0}\n", msg.a7_8);
            s.AppendFormat("a7_9 = {0}\n", msg.a7_9);
            s.AppendFormat("a7_10 = {0}\n", msg.a7_10);
            s.AppendFormat("a7_11 = {0}\n", msg.a7_11);
            s.AppendFormat("a7_12 = {0}\n", msg.a7_12);
            s.AppendFormat("a7_13 = {0}\n", msg.a7_13);
            s.AppendFormat("a7_14 = {0}\n", msg.a7_14);
            s.AppendFormat("a7_15 = {0}\n", msg.a7_15);
            s.AppendFormat("a7_16 = {0}\n", msg.a7_16);
            s.AppendFormat("a7_17 = {0}\n", msg.a7_17);
            s.AppendFormat("a7_18 = {0}\n", msg.a7_18);
            s.AppendFormat("a7_19 = {0}\n", msg.a7_19);
            s.AppendFormat("a7_20 = {0}\n", msg.a7_20);
            s.AppendFormat("a7_21 = {0}\n", msg.a7_21);
            s.AppendFormat("a7_22 = {0}\n", msg.a7_22);
            s.AppendFormat("a7_23 = {0}\n", msg.a7_23);
            s.AppendFormat("a7_24 = {0}\n", msg.a7_24);
            s.AppendFormat("a7_25 = {0}\n", msg.a7_25);
            s.AppendFormat("a7_26 = {0}\n", msg.a7_26);
            s.AppendFormat("a7_27 = {0}\n", msg.a7_27);
            s.AppendFormat("a7_28 = {0}\n", msg.a7_28);
            s.AppendFormat("a7_29 = {0}\n", msg.a7_29);
            s.AppendFormat("a7_30 = {0}\n", msg.a7_30);
            s.AppendFormat("a7_31 = {0}\n", msg.a7_31);
            s.AppendFormat("a7_32 = {0}\n", msg.a7_32);
            s.AppendFormat("a7_33 = {0}\n", msg.a7_33);
            s.AppendFormat("a7_34 = {0}\n", msg.a7_34);
            s.AppendFormat("a7_35 = {0}\n", msg.a7_35);
            s.AppendFormat("a7_36 = {0}\n", msg.a7_36);
            s.AppendFormat("a7_37 = {0}\n", msg.a7_37);
            s.AppendFormat("a7_38 = {0}\n", msg.a7_38);
            s.AppendFormat("a7_39 = {0}\n", msg.a7_39);
            s.AppendFormat("a7_40 = {0}\n", msg.a7_40);
            s.AppendFormat("a7_41 = {0}\n", msg.a7_41);
            s.AppendFormat("a7_42 = {0}\n", msg.a7_42);
            s.AppendFormat("a7_43 = {0}\n", msg.a7_43);
            s.AppendFormat("a7_44 = {0}\n", msg.a7_44);
            s.AppendFormat("a7_45 = {0}\n", msg.a7_45);
            s.AppendFormat("a7_46 = {0}\n", msg.a7_46);
            s.AppendFormat("a7_47 = {0}\n", msg.a7_47);
            s.AppendFormat("a7_48 = {0}\n", msg.a7_48);
            s.AppendFormat("a7_49 = {0}\n", msg.a7_49);
            s.AppendFormat("a7_50 = {0}\n", msg.a7_50);
            s.AppendFormat("a8 = {0}\n", msg.a8);
            s.AppendFormat("a8_1 = {0}\n", msg.a8_1);
            s.AppendFormat("a8_2 = {0}\n", msg.a8_2);
            s.AppendFormat("a8_3 = {0}\n", msg.a8_3);
            s.AppendFormat("a8_4 = {0}\n", msg.a8_4);
            s.AppendFormat("a8_5 = {0}\n", msg.a8_5);
            s.AppendFormat("a8_6 = {0}\n", msg.a8_6);
            s.AppendFormat("a8_7 = {0}\n", msg.a8_7);
            s.AppendFormat("a8_8 = {0}\n", msg.a8_8);
            s.AppendFormat("a8_9 = {0}\n", msg.a8_9);
            s.AppendFormat("a8_10 = {0}\n", msg.a8_10);
            s.AppendFormat("a8_11 = {0}\n", msg.a8_11);
            s.AppendFormat("a8_12 = {0}\n", msg.a8_12);
            s.AppendFormat("a8_13 = {0}\n", msg.a8_13);
            s.AppendFormat("a8_14 = {0}\n", msg.a8_14);
            s.AppendFormat("a8_15 = {0}\n", msg.a8_15);
            s.AppendFormat("a8_16 = {0}\n", msg.a8_16);
            s.AppendFormat("a8_17 = {0}\n", msg.a8_17);
            s.AppendFormat("a8_18 = {0}\n", msg.a8_18);
            s.AppendFormat("a8_19 = {0}\n", msg.a8_19);
            s.AppendFormat("a8_20 = {0}\n", msg.a8_20);
            s.AppendFormat("a8_21 = {0}\n", msg.a8_21);
            s.AppendFormat("a8_22 = {0}\n", msg.a8_22);
            s.AppendFormat("a8_23 = {0}\n", msg.a8_23);
            s.AppendFormat("a8_24 = {0}\n", msg.a8_24);
            s.AppendFormat("a8_25 = {0}\n", msg.a8_25);
            s.AppendFormat("a8_26 = {0}\n", msg.a8_26);
            s.AppendFormat("a8_27 = {0}\n", msg.a8_27);
            s.AppendFormat("a9 = {0}\n", msg.a9);
            s.AppendFormat("a10 = {0}\n", msg.a10 ? 1 : 0);
            s.AppendFormat("a11 = {0}\n", (int)msg.a11);
            s.AppendFormat("a12 = {0}\n", Encoding.ASCII.GetString(msg.a12));
            s.AppendFormat("a13 = {0}\n", msg.a13);
            s.AppendFormat("a13_1 = {0}\n", msg.a13_1);
            s.AppendFormat("a13_2 = {0}\n", msg.a13_2);
            s.AppendFormat("a13_3 = {0}\n", msg.a13_3);
            s.AppendFormat("a13_4 = {0}\n", msg.a13_4);
            s.AppendFormat("a13_5 = {0}\n", msg.a13_5);
            s.AppendFormat("a13_6 = {0}\n", msg.a13_6);
            s.AppendFormat("a13_7 = {0}\n", msg.a13_7);
            s.AppendFormat("a13_8 = {0}\n", msg.a13_8);
            s.AppendFormat("a13_9 = {0}\n", msg.a13_9);
            s.AppendFormat("a13_10 = {0}\n", msg.a13_10);
            s.AppendFormat("a13_11 = {0}\n", msg.a13_11);
            s.AppendFormat("a13_12 = {0}\n", msg.a13_12);
            s.AppendFormat("a13_13 = {0}\n", msg.a13_13);
            s.AppendFormat("a13_14 = {0}\n", msg.a13_14);
            s.AppendFormat("a13_15 = {0}\n", msg.a13_15);
            s.AppendFormat("a13_16 = {0}\n", msg.a13_16);
            s.AppendFormat("a13_17 = {0}\n", msg.a13_17);
            s.AppendFormat("a13_18 = {0}\n", msg.a13_18);
            s.AppendFormat("a13_19 = {0}\n", msg.a13_19);
            s.AppendFormat("a13_20 = {0}\n", msg.a13_20);
            s.AppendFormat("a13_21 = {0}\n", msg.a13_21);
            s.AppendFormat("a13_22 = {0}\n", msg.a13_22);
            s.AppendFormat("a13_23 = {0}\n", msg.a13_23);
            s.AppendFormat("a14 = {0}\n", msg.a14);
            s.AppendFormat("a14_1 = {0}\n", msg.a14_1);
            s.AppendFormat("a14_2 = {0}\n", msg.a14_2);
            s.AppendFormat("a14_3 = {0}\n", msg.a14_3);
            s.AppendFormat("a14_4 = {0}\n", msg.a14_4);
            s.AppendFormat("a14_5 = {0}\n", msg.a14_5);
            s.AppendFormat("a14_6 = {0}\n", msg.a14_6);
            s.AppendFormat("a14_7 = {0}\n", msg.a14_7);
            s.AppendFormat("a14_8 = {0}\n", msg.a14_8);
            s.AppendFormat("a14_9 = {0}\n", msg.a14_9);
            s.AppendFormat("a14_10 = {0}\n", msg.a14_10);
            s.AppendFormat("a14_11 = {0}\n", msg.a14_11);
            s.AppendFormat("a14_12 = {0}\n", msg.a14_12);
            s.AppendFormat("a14_13 = {0}\n", msg.a14_13);
            s.AppendFormat("a14_14 = {0}\n", msg.a14_14);
            s.AppendFormat("a15 = {0}\n", msg.a15);
            s.AppendFormat("a15_1 = {0}\n", msg.a15_1);
            s.AppendFormat("a15_2 = {0}\n", msg.a15_2);
            s.AppendFormat("a15_3 = {0}\n", msg.a15_3);
            s.AppendFormat("a15_4 = {0}\n", msg.a15_4);
            s.AppendFormat("a15_5 = {0}\n", msg.a15_5);
            s.AppendFormat("a15_6 = {0}\n", msg.a15_6);
            s.AppendFormat("a15_7 = {0}\n", msg.a15_7);
            s.AppendFormat("a15_8 = {0}\n", msg.a15_8);
            s.AppendFormat("a15_9 = {0}\n", msg.a15_9);
            s.AppendFormat("a15_10 = {0}\n", msg.a15_10);
            s.AppendFormat("a15_11 = {0}\n", msg.a15_11);
            s.AppendFormat("a15_12 = {0}\n", msg.a15_12);
            s.AppendFormat("a15_13 = {0}\n", msg.a15_13);
            s.AppendFormat("a15_14 = {0}\n", msg.a15_14);
            s.AppendFormat("a15_15 = {0}\n", msg.a15_15);
            s.AppendFormat("a15_16 = {0}\n", msg.a15_16);
            s.AppendFormat("a15_17 = {0}\n", msg.a15_17);
            s.AppendFormat("a15_18 = {0}\n", msg.a15_18);
            s.AppendFormat("a15_19 = {0}\n", msg.a15_19);
            s.AppendFormat("a15_20 = {0}\n", msg.a15_20);
            s.AppendFormat("a15_21 = {0}\n", msg.a15_21);
            s.AppendFormat("a15_22 = {0}\n", msg.a15_22);
            s.AppendFormat("a15_23 = {0}\n", msg.a15_23);
            s.AppendFormat("a15_24 = {0}\n", msg.a15_24);
            s.AppendFormat("a15_25 = {0}\n", msg.a15_25);
            s.AppendFormat("a15_26 = {0}\n", msg.a15_26);
            s.AppendFormat("a15_27 = {0}\n", msg.a15_27);
            s.AppendFormat("a15_28 = {0}\n", msg.a15_28);
            s.AppendFormat("a15_29 = {0}\n", msg.a15_29);
            s.AppendFormat("a15_30 = {0}\n", msg.a15_30);
            s.AppendFormat("a15_31 = {0}\n", msg.a15_31);
            s.AppendFormat("a15_32 = {0}\n", msg.a15_32);
            s.AppendFormat("a16 = {0}\n", msg.a16);
            s.AppendFormat("a16_1 = {0}\n", msg.a16_1);
            s.AppendFormat("a16_2 = {0}\n", msg.a16_2);
            s.AppendFormat("a16_3 = {0}\n", msg.a16_3);
            s.AppendFormat("a16_4 = {0}\n", msg.a16_4);
            s.AppendFormat("a16_5 = {0}\n", msg.a16_5);
            s.AppendFormat("a16_6 = {0}\n", msg.a16_6);
            s.AppendFormat("a16_7 = {0}\n", msg.a16_7);
            s.AppendFormat("a16_8 = {0}\n", msg.a16_8);
            s.AppendFormat("a16_9 = {0}\n", msg.a16_9);
            s.AppendFormat("a16_10 = {0}\n", msg.a16_10);
            s.AppendFormat("a16_11 = {0}\n", msg.a16_11);
            s.AppendFormat("a16_12 = {0}\n", msg.a16_12);
            s.AppendFormat("a16_13 = {0}\n", msg.a16_13);
            s.AppendFormat("a16_14 = {0}\n", msg.a16_14);
            s.AppendFormat("a16_15 = {0}\n", msg.a16_15);
            s.AppendFormat("a16_16 = {0}\n", msg.a16_16);
            s.AppendFormat("a16_17 = {0}\n", msg.a16_17);
            s.AppendFormat("a17 = {0}\n", msg.a17);
            s.AppendFormat("a17_1 = {0}\n", msg.a17_1);
            s.AppendFormat("a17_2 = {0}\n", msg.a17_2);
            s.AppendFormat("a17_3 = {0}\n", msg.a17_3);
            s.AppendFormat("a17_4 = {0}\n", msg.a17_4);
            s.AppendFormat("a17_5 = {0}\n", msg.a17_5);
            s.AppendFormat("a17_6 = {0}\n", msg.a17_6);
            s.AppendFormat("a17_7 = {0}\n", msg.a17_7);
            s.AppendFormat("a17_8 = {0}\n", msg.a17_8);
            s.AppendFormat("a17_9 = {0}\n", msg.a17_9);
            s.AppendFormat("a17_10 = {0}\n", msg.a17_10);
            s.AppendFormat("a17_11 = {0}\n", msg.a17_11);
            s.AppendFormat("a17_12 = {0}\n", msg.a17_12);
            s.AppendFormat("a17_13 = {0}\n", msg.a17_13);
            s.AppendFormat("a17_14 = {0}\n", msg.a17_14);
            s.AppendFormat("a17_15 = {0}\n", msg.a17_15);
            s.AppendFormat("a17_16 = {0}\n", msg.a17_16);
            s.AppendFormat("a17_17 = {0}\n", msg.a17_17);
            s.AppendFormat("a17_18 = {0}\n", msg.a17_18);
            s.AppendFormat("a17_19 = {0}\n", msg.a17_19);
            s.AppendFormat("a17_20 = {0}\n", msg.a17_20);
            s.AppendFormat("a17_21 = {0}\n", msg.a17_21);
            s.AppendFormat("a17_22 = {0}\n", msg.a17_22);
            s.AppendFormat("a17_23 = {0}\n", msg.a17_23);
            s.AppendFormat("a17_24 = {0}\n", msg.a17_24);
            s.AppendFormat("a17_25 = {0}\n", msg.a17_25);
            s.AppendFormat("a17_26 = {0}\n", msg.a17_26);
            s.AppendFormat("a17_27 = {0}\n", msg.a17_27);
            s.AppendFormat("a17_28 = {0}\n", msg.a17_28);
            s.AppendFormat("a17_29 = {0}\n", msg.a17_29);
            s.AppendFormat("a17_30 = {0}\n", msg.a17_30);
            s.AppendFormat("a17_31 = {0}\n", msg.a17_31);
            s.AppendFormat("a17_32 = {0}\n", msg.a17_32);
            s.AppendFormat("a17_33 = {0}\n", msg.a17_33);
            s.AppendFormat("a17_34 = {0}\n", msg.a17_34);
            s.AppendFormat("a17_35 = {0}\n", msg.a17_35);
            s.AppendFormat("a17_36 = {0}\n", msg.a17_36);
            s.AppendFormat("a17_37 = {0}\n", msg.a17_37);
            s.AppendFormat("a17_38 = {0}\n", msg.a17_38);
            s.AppendFormat("a17_39 = {0}\n", msg.a17_39);
            s.AppendFormat("a17_40 = {0}\n", msg.a17_40);
            s.AppendFormat("a17_41 = {0}\n", msg.a17_41);
            s.AppendFormat("a17_42 = {0}\n", msg.a17_42);
            s.AppendFormat("a17_43 = {0}\n", msg.a17_43);
            s.AppendFormat("a17_44 = {0}\n", msg.a17_44);
            s.AppendFormat("a17_45 = {0}\n", msg.a17_45);
            s.AppendFormat("a17_46 = {0}\n", msg.a17_46);
            s.AppendFormat("a17_47 = {0}\n", msg.a17_47);
            s.AppendFormat("a17_48 = {0}\n", msg.a17_48);
            s.AppendFormat("a17_49 = {0}\n", msg.a17_49);
            s.AppendFormat("a17_50 = {0}\n", msg.a17_50);
            s.AppendFormat("a18 = {0}\n", msg.a18);
            s.AppendFormat("a18_1 = {0}\n", msg.a18_1);
            s.AppendFormat("a18_2 = {0}\n", msg.a18_2);
            s.AppendFormat("a18_3 = {0}\n", msg.a18_3);
            s.AppendFormat("a18_4 = {0}\n", msg.a18_4);
            s.AppendFormat("a18_5 = {0}\n", msg.a18_5);
            s.AppendFormat("a18_6 = {0}\n", msg.a18_6);
            s.AppendFormat("a18_7 = {0}\n", msg.a18_7);
            s.AppendFormat("a18_8 = {0}\n", msg.a18_8);
            s.AppendFormat("a18_9 = {0}\n", msg.a18_9);
            s.AppendFormat("a18_10 = {0}\n", msg.a18_10);
            s.AppendFormat("a18_11 = {0}\n", msg.a18_11);
            s.AppendFormat("a18_12 = {0}\n", msg.a18_12);
            s.AppendFormat("a18_13 = {0}\n", msg.a18_13);
            s.AppendFormat("a18_14 = {0}\n", msg.a18_14);
            s.AppendFormat("a18_15 = {0}\n", msg.a18_15);
            s.AppendFormat("a18_16 = {0}\n", msg.a18_16);
            s.AppendFormat("a18_17 = {0}\n", msg.a18_17);
            s.AppendFormat("a18_18 = {0}\n", msg.a18_18);
            s.AppendFormat("a18_19 = {0}\n", msg.a18_19);
            s.AppendFormat("a18_20 = {0}\n", msg.a18_20);
            s.AppendFormat("a18_21 = {0}\n", msg.a18_21);
            s.AppendFormat("a18_22 = {0}\n", msg.a18_22);
            s.AppendFormat("a18_23 = {0}\n", msg.a18_23);
            s.AppendFormat("a18_24 = {0}\n", msg.a18_24);
            s.AppendFormat("a18_25 = {0}\n", msg.a18_25);
            s.AppendFormat("a18_26 = {0}\n", msg.a18_26);
            s.AppendFormat("a18_27 = {0}\n", msg.a18_27);
            s.AppendFormat("b5 size = {0}\n", msg.b5.Count);
            s.AppendFormat("b5[253] = {0}\n", msg.b5[253]);
            s.AppendFormat("b7 size = {0}\n", msg.b7.Count);
            s.AppendFormat("b7[0] = {0}\n", msg.b7[0]);
            s.AppendFormat("b8 size = {0}\n", msg.b8.Count);
            s.AppendFormat("b8[0] = {0}\n", msg.b8[0]);
            s.AppendFormat("has c1 = {0}\n", msg.has_c1() ? 1 : 0);
            s.AppendFormat("c1 = {0}\n", msg.c1);
            s.AppendFormat("has c2 = {0}\n", msg.has_c2() ? 1 : 0);
            s.AppendFormat("c2 = {0}\n", msg.c2);
            s.AppendFormat("has c3 = {0}\n", msg.has_c3() ? 1 : 0);
            s.AppendFormat("c3 size = {0}\n", msg.c3.Count);
            s.AppendFormat("c3[65535] = {0}\n", msg.c3[65535]);

            Console.Write(s);
        }

        byte[] bin = new byte[encode_size];
        Buffer.BlockCopy(buffer, 0, bin, 0, encode_size);
        try {
            File.WriteAllBytes("csharp.bin", bin);
        } catch {
            return 1;
        }

        return 0;
    }
}
