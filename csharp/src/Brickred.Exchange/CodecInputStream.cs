using System;
using System.Text;

namespace Brickred.Exchange
{
    public sealed class CodecInputStream
    {
        private readonly byte[] buffer_;
        private int buffer_pos_;
        private int buffer_size_;
        private int buffer_left_size_;

        public CodecInputStream(byte[] buffer, int offset, int length)
        {
            buffer_ = buffer;
            buffer_pos_ = Math.Min(
                Math.Max(offset, 0), buffer.Length);
            buffer_size_ = Math.Min(
                Math.Max(length, 0), buffer.Length - buffer_pos_);
            buffer_left_size_ = buffer_size_;
        }

        public CodecInputStream(byte[] buffer)
        {
            buffer_ = buffer;
            buffer_pos_ = 0;
            buffer_size_ = buffer.Length;
            buffer_left_size_ = buffer_size_;
        }

        public int GetReadSize()
        {
            return buffer_size_ - buffer_left_size_;
        }

        public byte ReadUInt8()
        {
            if (buffer_left_size_ < 1) {
                throw CodecException.BufferOutOfSpace();
            }

            byte val = buffer_[buffer_pos_];

            buffer_pos_ += 1;
            buffer_left_size_ -= 1;

            return val;
        }

        public ushort ReadUInt16()
        {
            if (buffer_left_size_ < 2) {
                throw CodecException.BufferOutOfSpace();
            }

            ushort val = (ushort)(buffer_[buffer_pos_ + 1] |
                                  buffer_[buffer_pos_] << 8);

            buffer_pos_ += 2;
            buffer_left_size_ -= 2;

            return val;
        }

        public uint ReadUInt32()
        {
            if (buffer_left_size_ < 4) {
                throw CodecException.BufferOutOfSpace();
            }

            uint val = (uint)buffer_[buffer_pos_ + 3] |
                       (uint)buffer_[buffer_pos_ + 2] << 8 |
                       (uint)buffer_[buffer_pos_ + 1] << 16 |
                       (uint)buffer_[buffer_pos_] << 24;

            buffer_pos_ += 4;
            buffer_left_size_ -= 4;

            return val;
        }

        public ulong ReadUInt64()
        {
            if (buffer_left_size_ < 8) {
                throw CodecException.BufferOutOfSpace();
            }

            ulong val = (ulong)buffer_[buffer_pos_ + 7] |
                        (ulong)buffer_[buffer_pos_ + 6] << 8 |
                        (ulong)buffer_[buffer_pos_ + 5] << 16 |
                        (ulong)buffer_[buffer_pos_ + 4] << 24 |
                        (ulong)buffer_[buffer_pos_ + 3] << 32 |
                        (ulong)buffer_[buffer_pos_ + 2] << 40 |
                        (ulong)buffer_[buffer_pos_ + 1] << 48 |
                        (ulong)buffer_[buffer_pos_] << 56;

            buffer_pos_ += 8;
            buffer_left_size_ -= 8;

            return val;
        }

        public ushort ReadUInt16V()
        {
            byte val = ReadUInt8();
            if (val < 255) {
                return val;
            } else {
                return ReadUInt16();
            }
        }

        public uint ReadUInt32V()
        {
            byte val = ReadUInt8();
            if (val < 254) {
                return val;
            } else if (val == 254) {
                return ReadUInt16();
            } else {
                return ReadUInt32();
            }
        }

        public ulong ReadUInt64V()
        {
            byte val = ReadUInt8();
            if (val < 253) {
                return val;
            } else if (val == 253) {
                return ReadUInt16();
            } else if (val == 254) {
                return ReadUInt32();
            } else {
                return ReadUInt64();
            }
        }

        public sbyte ReadInt8()
        {
            return (sbyte)ReadUInt8();
        }

        public short ReadInt16()
        {
            return (short)ReadUInt16();
        }

        public int ReadInt32()
        {
            return (int)ReadUInt32();
        }

        public long ReadInt64()
        {
            return (long)ReadUInt64();
        }

        public short ReadInt16V()
        {
            return (short)ReadUInt16V();
        }

        public int ReadInt32V()
        {
            return (int)ReadUInt32V();
        }

        public long ReadInt64V()
        {
            return (long)ReadUInt64V();
        }

        public bool ReadBool()
        {
            return ReadUInt8() != 0;
        }

        public int ReadLength()
        {
            int length = (int)ReadUInt32V();
            if (length < 0) {
                throw CodecException.BufferOutOfSpace();
            }

            return length;
        }

        public string ReadString()
        {
            int length = ReadLength();
            if (length <= 0) {
                return "";
            }

            if (buffer_left_size_ < length) {
                throw CodecException.BufferOutOfSpace();
            }

            // ArgumentException
            // DecoderFallbackException
            string val = Encoding.UTF8.GetString(
                buffer_, buffer_pos_, length);

            buffer_pos_ += length;
            buffer_left_size_ -= length;

            return val;
        }

        public byte[] ReadBytes()
        {
            int length = ReadLength();
            if (length <= 0) {
                return new byte[0];
            }

            if (buffer_left_size_ < length) {
                throw CodecException.BufferOutOfSpace();
            }

            byte[] val = new byte[length];
            Buffer.BlockCopy(buffer_, buffer_pos_, val, 0, length);

            buffer_pos_ += length;
            buffer_left_size_ -= length;

            return val;
        }

        public T ReadStruct<T>() where T : BaseStruct, new()
        {
            T val = new T();

            val.DecodeFromStream(this);

            return val;
        }
    }
}
