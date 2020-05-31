#ifndef BRICKRED_EXCHANGE_BASE_STRUCT_H
#define BRICKRED_EXCHANGE_BASE_STRUCT_H

#include <cstddef>
#include <string>

namespace brickred::exchange {

class BaseStruct {
public:
    using CreateFunc = BaseStruct *(*)();

    BaseStruct();
    virtual ~BaseStruct();
    virtual BaseStruct *clone() const = 0;

    virtual int encode(char *buffer, size_t size) const = 0;
    virtual int decode(const char *buffer, size_t size) = 0;
    virtual std::string dump() const = 0;

protected:
    static std::string dumpBytes(const std::string &val);
};

} // namespace brickred::exchange

#endif
