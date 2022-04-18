#include <assert.h>
#include <windows.h>

#include <iostream>  
#include <vector>
#include <fstream>

#include <onnxruntime_cxx_api.h>
#include"opencv2/opencv.hpp"

#include "common/cpp_process_infer_result/result.h"


wchar_t* multiByteToWideChar(const std::string& pKey)
{
	const char* pCStrKey = pKey.c_str();
	//第一次调用返回转换后的字符串长度，用于确认为wchar_t*开辟多大的内存空间
	int pSize = MultiByteToWideChar(CP_OEMCP, 0, pCStrKey, strlen(pCStrKey) + 1, NULL, 0);
	wchar_t* pWCStrKey = new wchar_t[pSize];
	//第二次调用将单字节字符串转换成双字节字符串
	MultiByteToWideChar(CP_OEMCP, 0, pCStrKey, strlen(pCStrKey) + 1, pWCStrKey, pSize);
	return pWCStrKey;
}

int main() {

	// 模型输入进本信息
	// 将模型文件放在英文目录下
	const char* model_path_onnx = "E:/Text_Model/yolov5/yolov5s.onnx";
	const char* image_path = "E:/Text_dataset/YOLOv5/0001.jpg";
	std::string lable_path = "E:/Git_space/Al模型部署开发方式/model/yolov5/lable.txt";
	const char* input_node_name = "images";
	const char* output_node_name = "output";


	// 推理环境设置
	// 设置为VERBOSE（ORT_LOGGING_LEVEL_VERBOSE）时，方便控制台输出时看到是使用了cpu还是gpu执行
	Ort::Env env(ORT_LOGGING_LEVEL_VERBOSE, "OnnxModel");
	// 设置推理会话
	Ort::SessionOptions session_options;
	// 设置线程数，使用1个线程执行，若想提升速度，增加线程数
	session_options.SetIntraOpNumThreads(1);
	session_options.SetGraphOptimizationLevel(GraphOptimizationLevel::ORT_ENABLE_ALL);

	// 创建推理类，并加载本地模型
	Ort::Session session(env, multiByteToWideChar(model_path_onnx), session_options);
	// 读取模型信息所需结构体
	// 打印模型的输入层(node names, types, shape etc.)
	Ort::AllocatorWithDefaultOptions allocator;

	// 获得模型又多少个输入和输出，一般是指对应网络层的数目
	// 一般输入只有图像的话input_nodes为1
	// 如果是多输出网络，就会是对应输出的数目
	size_t num_input_nodes = session.GetInputCount();
	size_t num_output_nodes = session.GetOutputCount();
	// 读取模型输入名称
	const char* input_name = session.GetInputName(0, allocator);
	std::cout << "input_name: " << input_name << std::endl;
	//获取输出名称
	const char* output_name = session.GetOutputName(0, allocator);
	std::cout << "output_name: " << output_name << std::endl;

	// 获取结点的输入和输出维度信息
	auto input_dims = session.GetInputTypeInfo(0).GetTensorTypeAndShapeInfo().GetShape();
	auto output_dims = session.GetOutputTypeInfo(0).GetTensorTypeAndShapeInfo().GetShape();

	// 输入/输出名字容器
	// 将名字放在容器中输入数据时会更方便
	std::vector<const char*> input_names{ input_node_name };
	std::vector<const char*> output_names = { output_node_name };

	// 处理输入图片
	cv::Mat image = cv::imread(image_path);
	//将图片的放入到方型背景中
	int max_side_length = std::max(image.cols, image.rows);
	cv::Mat max_image = cv::Mat::zeros(cv::Size(max_side_length, max_side_length), CV_8UC3);
	cv::Rect roi(0, 0, image.cols, image.rows);
	image.copyTo(max_image(roi));
	// 将图像归一化，转换RGB通道，并放缩到指定大小
	cv::Mat normal_image = cv::dnn::blobFromImage(max_image, 1 / 255.0,
		cv::Size(input_dims[2], input_dims[3]), cv::Scalar(0, 0, 0), true, false);

	Ort::MemoryInfo memory_info = Ort::MemoryInfo::CreateCpu(OrtAllocatorType::OrtArenaAllocator,
		OrtMemType::OrtMemTypeDefault);
	// 创建输入数据容器
	std::vector<Ort::Value> input_tensors;
	// 将输入数据加载到容器中
	input_tensors.emplace_back(Ort::Value::CreateTensor<float>(memory_info, normal_image.ptr<float>(),
		normal_image.total(), input_dims.data(), input_dims.size()));

	// 模型推理
	std::vector<Ort::Value> output_tensors = session.Run(Ort::RunOptions{ nullptr }, input_names.data(),
		input_tensors.data(), input_names.size(), output_names.data(), output_names.size());

	// 判断输出是否正确
	assert(output_tensors.size() == 1 && output_tensors.front().IsTensor());
	// 将推理结果转为数组
	float* result_array = output_tensors[0].GetTensorMutableData<float>();

	// 创建yolov5结果处理类
	ResultYolov5 result;
	result.factor = max_side_length / (float)640;
	result.read_class_names(lable_path);
	// 处理输出结果
	cv::Mat result_image = result.yolov5_result(image, result_array);

	// 查看输出结果
	cv::imshow("C++ + OpenCV Dnn + Yolov5 推理结果", result_image);
	cv::waitKey();



}