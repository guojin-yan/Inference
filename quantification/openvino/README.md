ResNet50_INT8_OpenVINO™

## 1. 项目功能

项目主要功能实现通过OpenVINO™中的POT工具，将ResNet50模型转为INT8格式。



## 2. 源码实现

项目实现源码已经放置在python文件夹下，获取源码后，对以下位置进行修改：

- 模型信息

	```python
	model_config = Dict({
	    'model_name': 'flower_clas',
	    'model': "E:\\Text_Model\\flowerclas20220427\\flower_clas.xml",
	    'weights': "E:\\Text_Model\\flowerclas20220427\\flower_clas.bin"
	})
	```

将该字典中的模型信息修改为自己本地地址。

- 测试数据信息

	```python
	dataset_config = {
	    'data_source': "E:\\Text_dataset\\flowers102\\val_list.txt",
	    'path': "E:\\Text_dataset\\flowers102\\"
	}
	```

将测试数据集修改为本地数据位置

运行main_resnet50_int8_openvino.py文件，进行模型转换，结果如下：

```python
Load the model.
Initialize the data loader.
Initialize the metric.
Initialize the engine for metric calculation and statistics collection.
 Create a pipeline of compression algorithms.
Execute the pipeline.
 Compress model weights quantized precision
Save the compressed model to the desired path.
Accuracy of the original model: accuracy@top1: 0.0
Accuracy of the optimized model: accuracy@top1: 0.0
Compare accuracy of the original and quantized models.
```

转换后的模型在D盘根目录下：

![image-20220427140007923](.image/image-20220427140007923.png)



## 3. OpenVINO™工具实现

OpenVINO™提供了精度转换工具，位置在<./openvino/tools/>文件夹下，其中需要使用的配置文件在该项目下<configs>文件夹下：ResNet50.yaml，ResNet50.json

### 3.1 修改配置文件

首先需要对配置文件中的路径进行修改，改为自己电脑的本地路径：

- ResNet50.yaml

```yaml
datasets:
  - name: classification_dataset
    data_source: E:/Text_dataset/flowers102/jpg
    annotation_conversion:
      converter: imagenet
      annotation_file: E:/Text_dataset/flowers102/train_list.txt
    reader: pillow_imread
```

修改data_source与annotation_file路径地址。

- ResNet50.json

```json
    "model": {
        "model_name": "flower_clas",
        "model": "E:\\Text_Model\\flowerclas20220427\\flower_clas.xml",
        "weights": "E:\\Text_Model\\flowerclas20220427\\flower_clas.bin"
    },
    "engine": {
        "config": "./ResNet50.yaml"
    },
```

根据本地文件路径，修改以上信息。

### 3.2 模型精度检测

命令提示窗口打开一下位置：.\openvino\tools

![image-20220427141354684](.image/image-20220427141354684.png)

输入以下命令：

```
accuracy_check -c ResNet50.yaml -m E:\\Text_Model\\flowerclas20220427
```

运行结果如下：

```shell
(pycharm_openvino) D:\ProgramFiles\anacoonda3\envs\pycharm_openvino\Lib\site-packages\openvino\tools>accuracy_check -c ResNet50.yaml -m E:\\Text_Model\\flowerclas20220427
Processing info:
model: ResNet50
launcher: openvino
device: CPU
dataset: classification_dataset
OpenCV version: 4.5.5
Annotation conversion for classification_dataset dataset has been started
Parameters to be used for conversion:
converter: imagenet
annotation_file: E:/Text_dataset/flowers102/train_list.txt
Annotation conversion for classification_dataset dataset has been finished

IE version: 2022.1.0-7019-cdb9bec7210-releases/2022/1
Loaded CPU plugin version:
    CPU - openvino_intel_cpu_plugin: 2022.1.2022.1.0-7019-cdb9bec7210-releases/2022/1
Found model E:\Text_Model\flowerclas20220427\flower_clas.xml
Found weights E:\Text_Model\flowerclas20220427\flower_clas.bin
Input info:
        Node name: x
        Tensor names: x
        precision: f32
        shape: (1, 3, 224, 224)

Output info
        Node name: softmax_1.tmp_0/sink_port_0
        Tensor names: softmax_1.tmp_0
        precision: f32
        shape: (1, 102)

1019 objects processed in 38.744 seconds
accuracy@top1: 0.98%
accuracy@top5: 4.22%
```

精度检测结果如上所示，不过精度比较低，目前不知道什么原因。

### 3.3 模型量化INT8

在命令提示行输入以下命令：

```shell
pot -c ResNet50.json -e
```

运行结果如下：

```shell
(pycharm_openvino) D:\ProgramFiles\anacoonda3\envs\pycharm_openvino\Lib\site-packages\openvino\tools>pot -c ResNet50.json -e
INFO:openvino.tools.pot.app.run:Output log dir: ./results\flower_clas_DefaultQuantization\2022-04-27_14-25-06
INFO:openvino.tools.pot.app.run:Creating pipeline:
 Algorithm: DefaultQuantization
 Parameters:
        preset                     : mixed
        stat_subset_size           : 300
        target_device              : ANY
        model_type                 : None
        dump_intermediate_model    : False
        inplace_statistics         : True

        exec_log_dir               : ./results\flower_clas_DefaultQuantization\2022-04-27_14-25-06
 ===========================================================================

IE version: 2022.1.0-7019-cdb9bec7210-releases/2022/1
Loaded CPU plugin version:
    CPU - openvino_intel_cpu_plugin: 2022.1.2022.1.0-7019-cdb9bec7210-releases/2022/1
Annotation conversion for classification_dataset dataset has been started
Parameters to be used for conversion:
converter: imagenet
annotation_file: E:/Text_dataset/flowers102/train_list.txt
Annotation conversion for classification_dataset dataset has been finished

INFO:openvino.tools.pot.pipeline.pipeline:Inference Engine version:                2022.1.0-7019-cdb9bec7210-releases/2022/1
INFO:openvino.tools.pot.pipeline.pipeline:Model Optimizer version:                 2022.1.0-7019-cdb9bec7210-releases/2022/1
INFO:openvino.tools.pot.pipeline.pipeline:Post-Training Optimization Tool version: 2022.1.0-7019-cdb9bec7210-releases/2022/1
INFO:openvino.tools.pot.statistics.collector:Start computing statistics for algorithms : DefaultQuantization
INFO:openvino.tools.pot.statistics.collector:Computing statistics finished
INFO:openvino.tools.pot.pipeline.pipeline:Start algorithm: DefaultQuantization
INFO:openvino.tools.pot.algorithms.quantization.default.algorithm:Start computing statistics for algorithm : ActivationChannelAlignment
INFO:openvino.tools.pot.algorithms.quantization.default.algorithm:Computing statistics finished
INFO:openvino.tools.pot.algorithms.quantization.default.algorithm:Start computing statistics for algorithms : MinMaxQuantization,FastBiasCorrection
INFO:openvino.tools.pot.algorithms.quantization.default.algorithm:Computing statistics finished

INFO:openvino.tools.pot.pipeline.pipeline:Finished: DefaultQuantization
 ===========================================================================

INFO:openvino.tools.pot.pipeline.pipeline:Evaluation of generated model
INFO:openvino.tools.pot.engines.ac_engine:Start inference on the whole dataset
Total dataset size: 1019
1000 / 1019 processed in 10.756s
1019 objects processed in 10.959 seconds
INFO:openvino.tools.pot.engines.ac_engine:Inference finished
INFO:openvino.tools.pot.app.run:accuracy@top1              : 0.009813542688910697
INFO:openvino.tools.pot.app.run:accuracy@top5              : 0.05103042198233562
```

生成的模型在<./results\flower_clas_DefaultQuantization\2022-04-27_14-25-06>路径下。

