TARGET = build/Brickred.Exchange.dll
SRCS = \
src/Brickred.Exchange/BaseStruct.cs \
src/Brickred.Exchange/CodecException.cs \
src/Brickred.Exchange/CodecInputStream.cs \
src/Brickred.Exchange/CodecOutputStream.cs \

.PHONY: build clean

build:
	mcs -t:library -out:$(TARGET) $(SRCS)

clean:
	rm -f $(TARGET)
