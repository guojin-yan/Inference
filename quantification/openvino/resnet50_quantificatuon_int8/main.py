#!/usr/bin/env python
# -*- coding: utf-8 -*-
# @Time    : 2022/5/10 13:51
# @Author  : 颜国进
# @File    : main.py
# @Software: PyCharm
# @function: 主函数

from resnet50_to_int8 import resnet50_to_int8


def main(argv=None):
    resnet50_to_int8()


if __name__ == "__main__":
    main()