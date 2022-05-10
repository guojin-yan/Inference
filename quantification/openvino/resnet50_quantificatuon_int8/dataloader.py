#!/usr/bin/env python
# -*- coding: utf-8 -*-
# @Time    : 2022/5/10 14:12
# @Author  : 颜国进
# @File    : dataloader.py
# @Software: PyCharm
# @function: 读取测试集本地数据

from openvino.tools.pot.api import DataLoader, Metric
import cv2

class DataLoader(DataLoader):

    def __init__(self, config):
        path = config['data_path']
        file = config['data_file']
        self.indexes, self.pictures, self.labels = self.load_data(path,file)

    def __len__(self):
        return len(self.labels)

    def __getitem__(self, index):
        if index >= len(self):
            raise IndexError
        return (self.indexes[index], self.labels[index]), self.pictures[index]


    def load_data(self, path,file):
        pictures, pictures_path, labels, indexes = [], [], [],[]
        with open(path+file, 'r') as f:
            for line in f:
                line = line.split()
                pictures_path.append(line[0])
                labels.append(int(line[1]))
        idx = 0
        for pic_path in pictures_path:
            src = cv2.imread(path+pic_path)
            image = cv2.cvtColor(src, cv2.COLOR_BGR2RGB)
            image = image/255.0
            image -= [0.485, 0.456, 0.406]
            image /= [0.229, 0.224, 0.225]
            image = cv2.resize(image, (224, 224), interpolation=cv2.INTER_AREA)  # [82 202 255]>[51 228 254]
            image = image.transpose(2, 0, 1)
            pictures.append(image)
            indexes.append(idx)
            idx = idx+1

        return indexes, pictures, labels


