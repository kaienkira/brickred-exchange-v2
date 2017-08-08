using Brickred.Exchange;
using Protocol.Client;
using System;
using System.Text;

public class App
{
    public static void Main()
    {
        byte[] buffer = new byte[10 * 1024 * 1024];
        int id = 0;
        int encode_size = 0;

        // encode message to buffer
        {
            MsgTest msg = new MsgTest();
            msg.a1 = 0x7f;
            msg.a2 = 0xff;
            msg.a3 = 0x7fff;
            msg.a4 = 0xffff;
            msg.a5 = 0x7fffffff;
            msg.a6 = 0xffffffff;
            msg.a7 = 0x7fffffffffffffff;
            msg.a8 = 0xffffffffffffffff;
            msg.a9 = "hello, world!";
            msg.a10 = true;
            msg.a11 = AttrType.STR;
            msg.a12 = Encoding.ASCII.GetBytes("hello, world!");
            msg.a13 = 0x7fff;
            msg.a14 = 0xffff;
            msg.a15 = 0x7fffffff;
            msg.a16 = 0xffffffff;
            msg.a17 = 0x7fffffffffffffff;
            msg.a18 = 0xffffffffffffffff;

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
                return;
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
            s.AppendFormat("a2 = {0}\n", msg.a2);
            s.AppendFormat("a3 = {0}\n", msg.a3);
            s.AppendFormat("a4 = {0}\n", msg.a4);
            s.AppendFormat("a5 = {0}\n", msg.a5);
            s.AppendFormat("a6 = {0}\n", msg.a6);
            s.AppendFormat("a7 = {0}\n", msg.a7);
            s.AppendFormat("a8 = {0}\n", msg.a8);
            s.AppendFormat("a9 = {0}\n", msg.a9);
            s.AppendFormat("a10 = {0}\n", msg.a10 ? 1 : 0);
            s.AppendFormat("a11 = {0}\n", (int)msg.a11);
            s.AppendFormat("a12 = {0}\n", Encoding.ASCII.GetString(msg.a12));
            s.AppendFormat("a13 = {0}\n", msg.a13);
            s.AppendFormat("a14 = {0}\n", msg.a14);
            s.AppendFormat("a15 = {0}\n", msg.a15);
            s.AppendFormat("a16 = {0}\n", msg.a16);
            s.AppendFormat("a17 = {0}\n", msg.a17);
            s.AppendFormat("a18 = {0}\n", msg.a18);
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
    }
}
