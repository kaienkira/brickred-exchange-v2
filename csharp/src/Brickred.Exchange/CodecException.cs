using System.IO;

namespace Brickred.Exchange
{
    public sealed class CodecException : IOException
    {
        internal CodecException(string message) :
            base(message)
        {
        }

        internal static CodecException BufferOutOfSpace()
        {
            return new CodecException("buffer out of space");
        }
    }
}
