.PHONY: build

TARGET = bin/brexc.exe
SRC = \
src/App.cs \
src/Mono.Options/Options.cs \
src/Brickred.Exchange.Compiler/BaseCodeGenerator.cs \
src/Brickred.Exchange.Compiler/CppCodeGenerator.cs \
src/Brickred.Exchange.Compiler/CSharpCodeGenerator.cs \
src/Brickred.Exchange.Compiler/PhpCodeGenerator.cs \
src/Brickred.Exchange.Compiler/ProtocolDescriptor.cs \
src/Brickred.Exchange.Compiler/ProtocolParser.cs \

build:
	@mcs -r:System.Xml.Linq $(SRC) -out:$(TARGET)

clean:
	@rm -f $(TARGET)
