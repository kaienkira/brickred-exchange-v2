#!/bin/bash

usage()
{
    echo 'usage: config.sh [options]'
    echo '-h --help            print usage'
    echo '--prefix=<prefix>    install prefix'
    exit 1
}

brickred_install_prefix='/usr/local'
brickred_compile_flag=
brickred_link_flag=

options=`getopt -o h -l \
help,\
prefix:\
 -- "$@"`
eval set -- "$options"

while [ $# -gt 0 ]
do
    case "$1" in
    -h|--help) usage;;
    --prefix) brickred_install_prefix=$2; shift;;
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
echo "BRICKRED_INSTALL_PREFIX = $brickred_install_prefix" >config.mak
echo "BRICKRED_COMPILE_FLAG = $brickred_compile_flag" >>config.mak
echo "BRICKRED_LINK_FLAG = $brickred_link_flag" >>config.mak
