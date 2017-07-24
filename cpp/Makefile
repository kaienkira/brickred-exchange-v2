include config.mak

TARGET = build/libbrickredexchange
SRCS = src/brickred/exchange/base_struct.cc
LINK_TYPE = static
INCLUDE = -Isrc
BUILD_DIR = build

include mak/main.mak

.PHONY: install

install:
	@mkdir -p $(BR_INSTALL_PREFIX)/include/brickred/exchange
	@cp src/brickred/exchange/*.h $(BR_INSTALL_PREFIX)/include/brickred/exchange
	@mkdir -p $(BR_INSTALL_PREFIX)/lib
	@cp $(FINAL_TARGET) $(BR_INSTALL_PREFIX)/lib
