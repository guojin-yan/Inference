#!/usr/bin/env python
# -*- coding: utf-8 -*-
# @Time    : 2022/4/27 13:16
# @Author  : 颜国进
# @File    : CifarDataLoader.py
# @Software: PyCharm
# @function:

import os
from openvino.tools.pot.api import DataLoader, Metric
from Accuracy import Accuracy
import cv2


# create DataLoader from CIFAR10 dataset
class CifarDataLoader(DataLoader):

    def __init__(self, config):
        """
        Initialize config and dataset.
        :param config: created config with DATA_DIR path.
        """
        self.path = config['path']
        self.indexes, self.pictures, self.labels = self.load_data(config['data_source'])

    def __len__(self):
        return len(self.labels)

    def __getitem__(self, index):
        """
        Return one sample of index, label and picture.
        :param index: index of the taken sample.
        """
        if index >= len(self):
            raise IndexError

        return (self.indexes[index], self.labels[index]), self.pictures[index]

    def load_data(self, file_path):
        """
        Load dataset in needed format.
        :param dataset:  downloaded dataset.
        """
        pictures, pictures_path, labels, indexes = [], [], [],[]

        with open(file_path, 'r') as f:
            for line in f:
                line = line.split()
                pictures_path.append(line[0])
                labels.append(line[1])


        idx = 0
        for path in pictures_path:
            src = cv2.imread(self.path + path)
            image = cv2.cvtColor(src, cv2.COLOR_BGR2RGB)
            image = image/255.0
            image = cv2.resize(image, (224, 224), interpolation=cv2.INTER_AREA)  # [82 202 255]>[51 228 254]
            image = image.transpose(2, 0, 1)
            pictures.append(image)
            indexes.append(idx)
            idx = idx+1

        return indexes, pictures, labels