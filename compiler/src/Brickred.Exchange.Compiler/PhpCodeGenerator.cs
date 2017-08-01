namespace Brickred.Exchange.Compiler
{
    public sealed class PhpCodeGenerator : BaseCodeGenerator
    {
        public PhpCodeGenerator()
        {
        }

        public override void Dispose()
        {
        }

        public override bool Generate(
            ProtocolDescriptor descriptor,
            string outputDir, NewLineType newLineType)
        {
            return true;
        }
    }
}
