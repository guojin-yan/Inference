#!/usr/bin/env python
# -*- coding: utf-8 -*-
# @Time    : 2022/5/10 14:10
# @Author  : 颜国进
# @File    : resnet50_to_int8.py
# @Software: PyCharm
# @function: 将ResNet50转为int8格式

from pathlib import Path
from addict import Dict
from openvino.tools.pot.engines.ie_engine import IEEngine
from openvino.tools.pot.graph import load_model, save_model
from openvino.tools.pot.graph.model_utils import compress_model_weights
from openvino.tools.pot.pipeline.initializer import create_pipeline
from accuracy import Accuracy
from dataloader import DataLoader




model_config = Dict({
    'model_name': 'flower_clas',
    # 'model': "E:\\Text_Model\\flowerclas\\flower_clas.xml",
    # 'weights': "E:\\Text_Model\\flowerclas\\flower_clas.bin"
    'model': "E:\\Text_Model\\flowerclas20220427\\flower_clas.xml",
    'weights': "E:\\Text_Model\\flowerclas20220427\\flower_clas.bin"
})
engine_config = Dict({
    'device': 'CPU',
    'stat_requests_number': 2,
    'eval_requests_number': 2
})
dataset_config = {
    'data_path': "E:\\Text_dataset\\flowers102\\",
    'data_file': "val_list.txt"
}
algorithms = [
    {
        'name': 'DefaultQuantization',
        'params': {
            'target_device': 'CPU',
            'preset': 'performance',
            'stat_subset_size': 300
        }
    }
]

def resnet50_to_int8():
    # Step 1: Load the model.
    model = load_model(model_config)
    print("Load the model.")

    # Step 2: Initialize the data loader.
    data_loader = DataLoader(dataset_config)
    print("Initialize the data loader.")

    # Step 3 (Optional. Required for AccuracyAwareQuantization): Initialize the metric.
    metric = Accuracy(top_k=1)
    print("Initialize the metric.")

    # Step 4: Initialize the engine for metric calculation and statistics collection.
    engine = IEEngine(engine_config, data_loader, metric)
    print("Initialize the engine for metric calculation and statistics collection.")

    # Step 5: Create a pipeline of compression algorithms.
    pipeline = create_pipeline(algorithms, engine)
    print("Create a pipeline of compression algorithms.")

    # Step 6: Execute the pipeline.
    compressed_model = pipeline.run(model)
    print("Execute the pipeline.")

    # Step 7 (Optional): Compress model weights quantized precision
    #                    in order to reduce the size of final .bin file.
    compress_model_weights(compressed_model)
    print("Compress model weights quantized precision")

    # Step 8: Save the compressed model to the desired path.
    compressed_model_paths = save_model(model=compressed_model, save_path="E:\\Text_Model\\flowerclas20220427\\",
                                        model_name="flower_clas_quantized"
                                        )
    compressed_model_xml = compressed_model_paths[0]["model"]
    compressed_model_bin = Path(compressed_model_paths[0]["model"]).with_suffix(".bin")
    print("Save the compressed model to the desired path.")

    # Step 9: Compare accuracy of the original and quantized models.
    metric_results = pipeline.evaluate(model)
    if metric_results:
        for name, value in metric_results.items():
            print(f"Accuracy of the original model: {name}: {value}")

    metric_results = pipeline.evaluate(compressed_model)
    if metric_results:
        for name, value in metric_results.items():
            print(f"Accuracy of the optimized model: {name}: {value}")

    print("Compare accuracy of the original and quantized models.")

