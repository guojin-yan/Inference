#include <iostream>
#include <fstream>

#include <opencv2/opencv.hpp>
#include <opencv2/dnn.hpp>
#include <opencv2/dnn/all_layers.hpp>

#include "common/cpp_process_infer_result/result.h"

int main() {
	// 模型输入进本信息
	// 将模型文件放在英文目录下
	const char* model_path_onnx = "E:/Text_Model/yolov5/yolov5s.onnx";
	const char* model_path_engine = "E:/Text_Model/yolov5/yolov5s.engine";
	const char* image_path = "E:/Text_dataset/YOLOv5/0001.jpg";
	std::string lable_path = "E:/Git_space/Al模型部署开发方式/model/yolov5/lable.txt";
	const char* input_node_name = "images";
	const char* output_node_name = "output";

	// 初始化网络结构，读取本地模型
	cv::dnn::Net net = cv::dnn::readNetFromONNX(model_path_onnx);

	// 图象预处理 - 格式化操作
	cv::Mat image = cv::imread(image_path);
	// 将图片数据放置在方形背景上
	int max_side_length = std::max(image.cols, image.rows);
	cv::Mat max_image = cv::Mat::zeros(cv::Size(max_side_length, max_side_length), CV_8UC3);
	cv::Rect roi(0, 0, image.cols, image.rows);
	image.copyTo(max_image(roi));
	// 将图像归一化，并放缩到指定大小
	cv::Size input_node_shape(640, 640);
	cv::Mat BN_image = cv::dnn::blobFromImage(max_image, 1 / 255.0, input_node_shape, cv::Scalar(0, 0, 0), true, false);

	// 将图片数据加载到模型中
	net.setInput(BN_image, input_node_name);
	
	// 推理模型，并读取输出数据
	cv::Mat result_mat= net.forward();
	// 将输出数据转为数组
	float* result_array = (float*)result_mat.data;

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