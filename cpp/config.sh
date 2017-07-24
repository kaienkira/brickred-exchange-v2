#!/bin/bash

usage()
{
    echo 'usage: config.sh [options]'
    echo '-h --help            print usage'
    echo '--prefix=<prefix>    install prefix'
    exit 1
}

opt_prefix='/usr/local'

while [ $# -gt 0 ]
do
    case "$1" in
    -h|--help) usage;;
    --prefix=*) opt_prefix=${1#*=}; shift;;
    --) shift; break;;
    *) usage;;
    esac
    shift
done

# check compiler
which g++ >/dev/null 2>&1
if [ $? -ne 0 ]
then
    echo 'can not find g++'
    exit 1
fi

# check make
which make >/dev/null 2>&1
if [ $? -ne 0 ]
then
    echo 'can not find make'
    exit 1
fi

# output
echo "BR_INSTALL_PREFIX = $opt_prefix" >config.mak
