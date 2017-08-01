namespace Brickred.Exchange.Compiler
{
    public sealed class CSharpCodeGenerator : BaseCodeGenerator
    {
        public CSharpCodeGenerator()
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
