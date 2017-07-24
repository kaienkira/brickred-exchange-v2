using ArgumentException = System.ArgumentException;
using DecoderFallbackException = System.Text.DecoderFallbackException;
using EncoderFallbackException = System.Text.EncoderFallbackException;
using Exception = System.Exception;

namespace Brickred.Exchange
{
    public abstract class BaseStruct
    {
        public delegate BaseStruct CreateFunc();

        protected abstract BaseStruct CloneInternal();
        public abstract void EncodeToStream(CodecOutputStream s);
        public abstract void DecodeFromStream(CodecInputStream s);

        public BaseStruct Clone()
        {
            return CloneInternal();
        }

        public int Encode(byte[] buffer)
        {
            return Encode(buffer, 0, buffer.Length);
        }

        public int Encode(byte[] buffer, int offset, int length)
        {
            CodecOutputStream s = new CodecOutputStream(
                buffer, offset, length);

            try {
                EncodeToStream(s);
            } catch (Exception e) {
                if (e is CodecException ||
                    e is EncoderFallbackException) {
                    return -1;
                } else {
                    throw;
                }
            }

            return s.GetWriteSize();
        }

        public int Decode(byte[] buffer)
        {
            return Decode(buffer, 0, buffer.Length);
        }

        public int Decode(byte[] buffer, int offset, int length)
        {
            CodecInputStream s = new CodecInputStream(
                buffer, offset, length);

            try {
                DecodeFromStream(s);
            } catch (Exception e) {
                if (e is CodecException ||
                    e is ArgumentException ||
                    e is DecoderFallbackException) {
                    return -1;
                } else {
                    throw;
                }
            }

            return s.GetReadSize();
        }
    }
}
