<?php

namespace Brickred\Exchange;

abstract class Binary64
{
    protected $high_;
    protected $low_;

    abstract public function getValue();

    public function __construct($str='')
    {
        $this->reset(0, 0);
        if ($str !== '') {
            $this->fromString($str);
        }
    }

    public function getHighInt32()
    {
        return $this->high_;
    }

    public function getLowInt32()
    {
        return $this->low_;
    }

    public function setHighInt32($high)
    {
        $this->high_ = Codec::convertToInt32($high);
    }

    public function setLowInt32($low)
    {
        $this->low_ = Codec::convertToInt32($low);
    }

    public function reset($high = 0, $low = 0)
    {
        $this->setHighInt32($high);
        $this->setLowInt32($low);
    }

    public function encode()
    {
        return Codec::writeInt32($this->high_).
               Codec::writeInt32($this->low_);
    }

    public function decodeFromStream($s)
    {
        $this->high_ = Codec::readInt32($s);
        $this->low_ = Codec::readInt32($s);
    }

    public function decode($buf)
    {
        $s = Codec::openStreamForBuffer($buf);
        $this->decodeFromStream($s);
        fclose($s);
    }

    public function toString()
    {
        return (string)($this->getValue());
    }

    public function fromString($str)
    {
        if (bccomp($str, '0') < 0) {
            $str = bcadd($str, '18446744073709551616');
        }
        $this->setHighInt32(bcdiv($str, '4294967296'));
        $this->setLowInt32(bcmod($str, '4294967296'));
    }
}

final class Int64 extends Binary64
{
    public function getValue()
    {
        if (PHP_INT_SIZE === 8) {
            return $this->high_ << 32 | ($this->low_ & 0xffffffff);
        }

        if ($this->high_ === 0) {
            if ($this->low_ < 0) {
                return $this->low_ + 4294967296;
            } else {
                return $this->low_;
            }
        } else if ($this->high_ === -1) {
            if ($this->low_ < 0) {
                return $this->low_;
            } else {
                return $this->low_ - 4294967296;
            }
        }

        if ($this->low_ < 0) {
            $unsigned_low = $this->low_ + 4294967296;
        } else {
            $unsigned_low = $this->low_;
        }

        return bcadd(bcmul($this->high_, '4294967296'), $unsigned_low);
    }
}

final class UInt64 extends Binary64
{
    public function getValue()
    {
        if (PHP_INT_SIZE === 8) {
            $unsigned_high = $this->high_ & 0xffffffff;
            $unsigned_low = $this->low_ & 0xffffffff;
            if ($this->high_ > 0) {
                return $unsigned_high << 32 | $unsigned_low;
            } else {
                return bcadd(bcmul($unsigned_high, '4294967296'),
                             $unsigned_low);
            }
        }

        if ($this->low_ < 0) {
            $unsigned_low = $this->low_ + 4294967296;
        } else {
            $unsigned_low = $this->low_;
        }

        if ($this->high_ === 0) {
            return $unsigned_low;
        }

        if ($this->high_ < 0) {
            $unsigned_high = $this->high_ + 4294967296;
        } else {
            $unsigned_high = $this->high_;
        }

        return bcadd(bcmul($unsigned_high, '4294967296'), $unsigned_low);
    }
}

final class CodecException extends \Exception
{
}

final class Codec
{
    public static function openStreamForBuffer($buf)
    {
        $s = fopen('php://memory', 'r+');
        fwrite($s, $buf);
        rewind($s);
        return $s;
    }

    public static function convertToInt32($var)
    {
        if (is_string($var)) {
            $var = (float)$var;
        }
        $var &= 0xffffffff;
        if ($var > 2147483647) {
            $var -= 4294967296;
        }

        return $var;
    }

    public static function readUInt8($s)
    {
        $ret = unpack('C', fread($s, 1));
        if ($ret === false) {
            throw new CodecException('read u8 failed');
        }
        return $ret[1];
    }

    public static function readUInt16($s)
    {
        $ret = unpack('n', fread($s, 2));
        if ($ret === false) {
            throw new CodecException('read u16 failed');
        }
        return $ret[1];
    }

    public static function readUInt32($s)
    {
        $ret = unpack('N', fread($s, 4));
        if ($ret === false) {
            throw new CodecException('read u32 failed');
        }

        $var = $ret[1];
        if ($var < 0) {
            return $var + 4294967296;
        }

        return $var;
    }

    public static function readUInt64($s)
    {
        $var = new UInt64();
        $var->decodeFromStream($s);

        return $var;
    }

    public static function readInt8($s)
    {
        $var = self::readUInt8($s);
        if ($var > 127) {
            $var -= 256;
        }

        return $var;
    }

    public static function readInt16($s)
    {
        $var = self::readUInt16($s);
        if ($var > 32767) {
            $var -= 65536;
        }

        return $var;
    }

    public static function readInt32($s)
    {
        $var = self::readUInt32($s);
        if ($var > 2147483647) {
            $var -= 4294967296;
        }

        return (int)$var;
    }

    public static function readInt64($s)
    {
        $var = new Int64();
        $var->decodeFromStream($s);

        return $var;
    }

    public static function readBool($s)
    {
        $var = self::readUInt8($s);
        if ($var === 0) {
            return false;
        } else {
            return true;
        }
    }

    public static function readLength($s)
    {
        $var = self::readUInt8($s);
        if ($var < 254) {
            return $var;
        } else if ($var === 254) {
            return self::readUInt16($s);
        } else {
            return self::readUInt32($s);
        }
    }

    public static function readString($s)
    {
        $length = self::readLength($s);
        if ($length === 0) {
            return '';
        }

        $var = fread($s, $length);
        if (strlen($var) !== $length) {
            throw new CodecException('read string failed');
        }

        return $var;
    }

    public static function readStruct($s, $struct_name)
    {
        $var = new $struct_name();
        $var->decodeFromStream($s);

        return $var;
    }

    public static function readList($s, $read_func)
    {
        $var = array();
        $length = self::readLength($s);

        for ($i = 0; $i < $length; ++$i) {
            array_push($var, self::$read_func($s));
        }

        return $var;
    }

    public static function readStructList($s, $struct_name)
    {
        $var = array();
        $length = self::readLength($s);

        for ($i = 0; $i < $length; ++$i) {
            array_push($var, self::readStruct($s, $struct_name));
        }

        return $var;
    }

    public static function writeInt8($var)
    {
        return pack('C', $var);
    }

    public static function writeInt16($var)
    {
        return pack('n', $var);
    }

    public static function writeInt32($var)
    {
        return pack('N', $var);
    }

    public static function writeInt64($var)
    {
        return $var->encode();
    }

    public static function writeLength($var)
    {
        if ($var < 254) {
            return self::writeInt8($var);
        } else if ($var <= 65535) {
            return self::writeInt8(254).self::writeInt16($var);
        } else {
            return self::writeInt8(255).self::writeInt32($var);
        }
    }

    public static function writeString($var)
    {
        return self::writeLength(strlen($var)).$var;
    }

    public static function writeStruct($var)
    {
        return $var->encode();
    }

    public static function writeList($var, $write_func)
    {
        $bin = self::writeLength(count($var));
        for ($i = 0; $i < count($var); ++$i) {
            $bin .= self::$write_func($var[$i]);
        }

        return $bin;
    }

    public static function readIntFromArray($array, $index)
    {
        if (!isset($array[$index])) {
            throw new CodecException("array['$index'] not set");
        }
        if (is_int($array[$index]) &&
            is_float($array[$index])) {
            return $array[$index];
        }

        return (int)$array[$index];
    }

    public static function readBoolFromArray($array, $index)
    {
        if (!isset($array[$index])) {
            throw new CodecException("array['$index'] not set");
        }

        return (bool)$array[$index];
    }

    public static function readStringFromArray($array, $index)
    {
        if (!isset($array[$index])) {
            throw new CodecException("array['$index'] not set");
        }

        return (string)$array[$index];
    }

    public static function readInt64FromArray($array, $index)
    {
        if (!isset($array[$index])) {
            throw new CodecException("array['$index'] not set");
        }
        return new \Brickred\Exchange\Int64((string)$array[$index]);
    }

    public static function readUInt64FromArray($array, $index)
    {
        if (!isset($array[$index])) {
            throw new CodecException("array['$index'] not set");
        }
        return new \Brickred\Exchange\UInt64((string)$array[$index]);
    }

    public static function readStructFromArray($array, $index, $struct_name)
    {
        if (!isset($array[$index])) {
            throw new CodecException("array['$index'] not set");
        }
        if (!is_array($array[$index])) {
            throw new CodecException("array['$index'] must be array");
        }

        $var = new $struct_name;
        $var->fromArray($array[$index]);
        return $var;
    }

    public static function readListFromArray($array, $index, $read_func)
    {
        $var = array();

        if (!isset($array[$index])) {
            throw new CodecException("array['$index'] not set");
        }
        if (!is_array($array[$index])) {
            throw new CodecException("array['$index'] must be array");
        }

        for ($i = 0; $i < count($array[$index]); ++$i) {
            $var[$i] = self::$read_func($array[$index], $i);
        }

        return $var;
    }

    public static function readStructListFromArray($array, $index,
                                                   $struct_name)
    {
        $var = array();

        if (!isset($array[$index])) {
            throw new CodecException("array['$index'] not set");
        }
        if (!is_array($array[$index])) {
            throw new CodecException("array['$index'] must be array");
        }

        for ($i = 0; $i < count($array[$index]); ++$i) {
            $var[$i] = self::readStructFromArray(
                $array[$index], $i, $struct_name);
        }

        return $var;
    }
}
