# Al模型部署开发方式

## 简介

​	当前项目，主要基于当前比较常见的OpenVINO™、TensorRT、ONNX runtime以及OpenCV Dnn四个部署平台进行对比展开，主要针对这四个平台如何部署深度学习项目进行了测试，并针对OpenVINOTM、TensorRT提出了在C#编程平台的解决办法，建立了C#模型部署检测软件平台。

![image-20220803180405845](E:\Git_space\Al模型部署开发方式\doc\image\image-20220803180405845.png)

![image-20220803180347472](E:\Git_space\Al模型部署开发方式\doc\image\image-20220803180347472.png)

## 项目文档

想要更详细的了解该项目，请参阅[AI model deployment platform.docx](.\doc\AI model deployment platform.docx)

## 使用环境

**系统平台：**

​			Windows

**软件要求：**

​			Visual Studio 2022 / 2019 / 2017

​			OpenCV 4.5.5

​			OpenVINO 2022.1

## 下载

**在Github上克隆下载：**

```shell
git clone https://github.com/guojin-yan/Inference.git
```

**在Gitee上克隆下载：**

```shell
git clone https://gitee.com/guojin-yan/Inference.git
```



## 项目说明

​	该项目目前已经实现了OpenVINO™、TensorRT、ONNX runtime以及OpenCV Dnn四种模型部署工具在C++、C#平台使用，其中由于OpenVINO™、TensorRT未提供C#语言接口，该项目中提出了解决方法。

​	由于笔者水平有限且时间紧促，因此在写文档时会有错误地方，后续使用者在使用时如有问题可以发邮件咨询([guojin_yjs@cumt.edu.cn](mailto:guojin_yjs@cumt.edu.cn))或者在GitHub以及Gitee相应项目下留言。并且欢迎大家对该项目提出意见，后续会相应改正。

​	项目文档编写不易，欢迎大家下载学习与使用，推动在C#平台使用OpenVINO™、TensorRT等工具进行模型部署。未经作者同意严禁将该文档内容上传其他平台进行盈利行为。
