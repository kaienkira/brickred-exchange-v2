using System;

namespace Brickred.Exchange.Compiler
{
    public abstract class BaseCodeGenerator : IDisposable
    {
        public enum NewLineType
        {
            None = 0,
            Unix = 1,
            Dos = 2,
        }

        public BaseCodeGenerator()
        {
        }

        ~BaseCodeGenerator()
        {
            Dispose();
        }

        public abstract void Dispose();
        public abstract bool Generate(
            ProtocolDescriptor descriptor,
            string outputDir, NewLineType newLineType);
    }
}
