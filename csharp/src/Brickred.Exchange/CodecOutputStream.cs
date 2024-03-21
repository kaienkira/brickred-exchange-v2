using System;
using System.Text;

namespace Brickred.Exchange
{
    public sealed class CodecOutputStream
    {
        private byte[] buffer_;
        private int buffer_pos_;
        private int buffer_size_;
        private int buffer_left_size_;

        public CodecOutputStream(byte[] buffer, int offset, int length)
        {
            buffer_ = buffer;
            buffer_pos_ = Math.Min(
                Math.Max(offset, 0), buffer.Length);
            buffer_size_ = Math.Min( 
                Math.Max(length, 0), buffer.Length - buffer_pos_);
            buffer_left_size_ = buffer_size_;
        }

        public CodecOutputStream(byte[] buffer)
        {
            buffer_ = buffer;
            buffer_pos_ = 0;
            buffer_size_ = buffer.Length;
            buffer_left_size_ = buffer_size_;
        }

        public int GetWriteSize()
        {
            return buffer_size_ - buffer_left_size_;
        }

        public void WriteUInt8(byte val)
        {
            if (buffer_left_size_ < 1) {
                throw CodecException.BufferOutOfSpace();
            }

            buffer_[buffer_pos_] = val;

            buffer_pos_ += 1;
            buffer_left_size_ -= 1;
        }

        public void WriteUInt16(ushort val)
        {
            if (buffer_left_size_ < 2) {
                throw CodecException.BufferOutOfSpace();
            }

            buffer_[buffer_pos_] = (byte)(val >> 8);
            buffer_[buffer_pos_ + 1] = (byte)(val);

            buffer_pos_ += 2;
            buffer_left_size_ -= 2;
        }

        public void WriteUInt32(uint val)
        {
            if (buffer_left_size_ < 4) {
                throw CodecException.BufferOutOfSpace();
            }

            buffer_[buffer_pos_] = (byte)(val >> 24);
            buffer_[buffer_pos_ + 1] = (byte)(val >> 16);
            buffer_[buffer_pos_ + 2] = (byte)(val >> 8);
            buffer_[buffer_pos_ + 3] = (byte)(val);

            buffer_pos_ += 4;
            buffer_left_size_ -= 4;
        }

        public void WriteUInt64(ulong val)
        {
            if (buffer_left_size_ < 8) {
                throw CodecException.BufferOutOfSpace();
            }

            buffer_[buffer_pos_] = (byte)(val >> 56);
            buffer_[buffer_pos_ + 1] = (byte)(val >> 48);
            buffer_[buffer_pos_ + 2] = (byte)(val >> 40);
            buffer_[buffer_pos_ + 3] = (byte)(val >> 32);
            buffer_[buffer_pos_ + 4] = (byte)(val >> 24);
            buffer_[buffer_pos_ + 5] = (byte)(val >> 16);
            buffer_[buffer_pos_ + 6] = (byte)(val >> 8);
            buffer_[buffer_pos_ + 7] = (byte)(val);

            buffer_pos_ += 8;
            buffer_left_size_ -= 8;
        }

        public void WriteUInt16V(ushort val)
        {
            if (val < 255) {
                WriteUInt8((byte)val);
            } else {
                WriteUInt8(255);
                WriteUInt16(val);
            }
        }

        public void WriteUInt32V(uint val)
        {
            if (val < 254) {
                WriteUInt8((byte)val);
            } else if (val <= 0xffff) {
                WriteUInt8(254);
                WriteUInt16((ushort)val);
            } else {
                WriteUInt8(255);
                WriteUInt32(val);
            }
        }

        public void WriteUInt64V(ulong val)
        {
            if (val < 253) {
                WriteUInt8((byte)val);
            } else if (val <= 0xffff) {
                WriteUInt8(253);
                WriteUInt16((ushort)val);
            } else if (val <= 0xffffffff) {
                WriteUInt8(254);
                WriteUInt32((uint)val);
            } else {
                WriteUInt8(255);
                WriteUInt64(val);
            }
        }

        public void WriteInt8(sbyte val)
        {
            WriteUInt8((byte)val);
        }

        public void WriteInt16(short val)
        {
            WriteUInt16((ushort)val);
        }

        public void WriteInt32(int val)
        {
            WriteUInt32((uint)val);
        }

        public void WriteInt64(long val)
        {
            WriteUInt64((ulong)val);
        }

        public void WriteInt16V(short val)
        {
            WriteUInt16V((ushort)val);
        }

        public void WriteInt32V(int val)
        {
            WriteUInt32V((uint)val);
        }

        public void WriteInt64V(long val)
        {
            WriteUInt64V((ulong)val);
        }

        public void WriteBool(bool val)
        {
            WriteUInt8((byte)(val ? 1 : 0));
        }

        public void WriteLength(int val)
        {
            WriteUInt32V((uint)val);
        }

        public void WriteString(string val)
        {
            // EncoderFallbackException
            int length = Encoding.UTF8.GetByteCount(val);
            WriteLength(length);
            if (buffer_left_size_ < length) {
                throw CodecException.BufferOutOfSpace();
            }

            // EncoderFallbackException
            Encoding.UTF8.GetBytes(val, 0, val.Length, buffer_, buffer_pos_);

            buffer_pos_ += length;
            buffer_left_size_ -= length;
        }

        public void WriteBytes(byte[] val)
        {
            int length = val.Length;
            WriteLength(length);
            if (buffer_left_size_ < length) {
                throw CodecException.BufferOutOfSpace();
            }

            Buffer.BlockCopy(val, 0, buffer_, buffer_pos_, length);

            buffer_pos_ += length;
            buffer_left_size_ -= length;
        }

        public void WriteStruct<T>(T val) where T : BaseStruct
        {
            val.EncodeToStream(this);
        }
    }
}
