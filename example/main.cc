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
        msg.a11 = AttrType::STR;
        msg.a12 = "hello, world!";

        for (int i = 0; i < 254; ++i) {
            msg.b5.push_back(i);
        }
        for (int i = 0; i < 10; ++i) {
            msg.b7.push_back(msg.a7);
        }
        for (int i = 0; i < 10; ++i) {
            msg.b8.push_back(msg.a8);
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
                  << "a2 = " << (int)msg->a2 << std::endl
                  << "a3 = " << msg->a3 << std::endl
                  << "a4 = " << msg->a4 << std::endl
                  << "a5 = " << msg->a5 << std::endl
                  << "a6 = " << msg->a6 << std::endl
                  << "a7 = " << msg->a7 << std::endl
                  << "a8 = " << msg->a8 << std::endl
                  << "a9 = " << msg->a9 << std::endl
                  << "a10 = " << msg->a10 << std::endl
                  << "a11 = " << msg->a11 << std::endl
                  << "a12 = " << msg->a12 << std::endl
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

    return 0;
}
