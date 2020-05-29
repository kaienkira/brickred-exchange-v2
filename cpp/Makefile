include config.mak

TARGET = build/libbrickredexchange
SRCS = \
src/brickred/exchange/base_struct.cc \

LINK_TYPE = static
INCLUDE = -Isrc
CPP_FLAG = $(BRICKRED_COMPILE_FLAG)
BUILD_DIR = build

include mak/main.mak

.PHONY: install

install:
	@mkdir -p "$(BRICKRED_INSTALL_PREFIX)"/include/brickred/exchange
	@cp src/brickred/exchange/*.h \
		"$(BRICKRED_INSTALL_PREFIX)"/include/brickred/exchange
	@mkdir -p "$(BRICKRED_INSTALL_PREFIX)"/lib
	@cp "$(FINAL_TARGET)" "$(BRICKRED_INSTALL_PREFIX)"/lib
