#include<windows.h>

#include <fstream>
#include <iostream>
#include <sstream>
#include <vector>

#include "NvInfer.h"
#include "NvOnnxParser.h"
#include <opencv2/opencv.hpp>

#include "common/cpp_process_infer_result/result.h"

// @brief 用于创建IBuilder、IRuntime或IRefitter实例的记录器用于通过该接口创建的所有对象。
// 在释放所有创建的对象之前，记录器应一直有效。
// 主要是实例化ILogger类下的log()方法。
class Logger : public nvinfer1::ILogger
{
	void log(Severity severity, const char* message)  noexcept
	{
		// suppress info-level messages
		if (severity != Severity::kINFO)
			std::cout << message << std::endl;
	}
} gLogger;

// @brief 
typedef struct tensorRT_nvinfer {
	Logger logger;
	// 反序列化引擎
	nvinfer1::IRuntime* runtime;
	// 推理引擎
	// 保存模型的模型结构、模型参数以及最优计算kernel配置；
	// 不能跨平台和跨TensorRT版本移植
	nvinfer1::ICudaEngine* engine;
	// 上下文
	// 储存中间值，实际进行推理的对象
	// 由engine创建，可创建多个对象，进行多推理任务
	nvinfer1::IExecutionContext* context;
	cudaStream_t stream;
	void** blob_data_buffer;
} NvinferStruct;


void onnx_to_engine(std::string onnx_file_path, std::string engine_file_path, int type) {

	// 构建器，获取cuda内核目录以获取最快的实现
	// 用于创建config、network、engine的其他对象的核心类
	nvinfer1::IBuilder* builder = nvinfer1::createInferBuilder(gLogger);
	const auto explicitBatch = 1U << static_cast<uint32_t>(nvinfer1::NetworkDefinitionCreationFlag::kEXPLICIT_BATCH);
	// 解析onnx网络文件
	// tensorRT模型类
	nvinfer1::INetworkDefinition* network = builder->createNetworkV2(explicitBatch);
	// onnx文件解析类
	// 将onnx文件解析，并填充rensorRT网络结构
	nvonnxparser::IParser* parser = nvonnxparser::createParser(*network, gLogger);
	// 解析onnx文件
	parser->parseFromFile(onnx_file_path.c_str(), 2);
	for (int i = 0; i < parser->getNbErrors(); ++i) {
		std::cout << "load error: " << parser->getError(i)->desc() << std::endl;
	}
	printf("tensorRT load mask onnx model successfully!!!...\n");

	// 创建推理引擎
	// 创建生成器配置对象。
	nvinfer1::IBuilderConfig* config = builder->createBuilderConfig();
	// 设置最大工作空间大小。
	config->setMaxWorkspaceSize(16 * (1 << 20));
	// 设置模型输出精度
	if (type == 1) {
		config->setFlag(nvinfer1::BuilderFlag::kFP16);
	}
	if (type == 2) {
		config->setFlag(nvinfer1::BuilderFlag::kINT8);
	}
	// 创建推理引擎
	nvinfer1::ICudaEngine* engine = builder->buildEngineWithConfig(*network, *config);
	// 将推理银枪保存到本地
	std::cout << "try to save engine file now~~~" << std::endl;
	std::ofstream file_ptr(engine_file_path, std::ios::binary);
	if (!file_ptr) {
		std::cerr << "could not open plan output file" << std::endl;
		return;
	}
	// 将模型转化为文件流数据
	nvinfer1::IHostMemory* model_stream = engine->serialize();
	// 将文件保存到本地
	file_ptr.write(reinterpret_cast<const char*>(model_stream->data()), model_stream->size());
	// 销毁创建的对象
	model_stream->destroy();
	engine->destroy();
	network->destroy();
	parser->destroy();
	std::cout << "convert onnx model to TensorRT engine model successfully!" << std::endl;
}

int main() {

	
	const char* model_path_onnx = "E:/Text_Model/yolov5/yolov5s.onnx";
	const char* model_path_engine = "E:/Text_Model/yolov5/yolov5s.engine";
	const char* image_path = "E:/Text_dataset/YOLOv5/0001.jpg";
	std::string lable_path = "E:/Git_space/Al模型部署开发方式/model/yolov5/lable.txt";
	const char* input_node_name = "images";
	const char* output_node_name = "output";

	int num_ionode = 2;

	// 读取本地模型文件
	std::ifstream file_ptr(model_path_engine, std::ios::binary);
	if (!file_ptr.good()) {
		std::cerr << "文件无法打开，请确定文件是否可用！" << std::endl;
	}

	size_t size = 0;
	file_ptr.seekg(0, file_ptr.end);	// 将读指针从文件末尾开始移动0个字节
	size = file_ptr.tellg();	// 返回读指针的位置，此时读指针的位置就是文件的字节数
	file_ptr.seekg(0, file_ptr.beg);	// 将读指针从文件开头开始移动0个字节
	char* model_stream = new char[size];
	file_ptr.read(model_stream, size);
	file_ptr.close();
	// 创建推理核心结构体，初始化变量
	NvinferStruct* p = new NvinferStruct();
	p->runtime = nvinfer1::createInferRuntime(gLogger);
	p->engine = p->runtime->deserializeCudaEngine(model_stream, size);
	p->context = p->engine->createExecutionContext();
	p->blob_data_buffer = new void* [num_ionode];
	delete[] model_stream;

	// 创建GPU显存缓冲区
	// 创建GPU显存输入缓冲区
	int input_node_index = p->engine->getBindingIndex(input_node_name);
	nvinfer1::Dims input_node_dim = p->engine->getBindingDimensions(input_node_index);
	size_t input_data_length = input_node_dim.d[1]* input_node_dim.d[2] * input_node_dim.d[3];
	cudaMalloc(&(p->blob_data_buffer[input_node_index]), input_data_length * sizeof(float));
	// 创建GPU显存输出缓冲区
	int output_node_index = p->engine->getBindingIndex(output_node_name);
	nvinfer1::Dims output_node_dim = p->engine->getBindingDimensions(output_node_index);
	size_t output_data_length = output_node_dim.d[1] * output_node_dim.d[2] ;
	cudaMalloc(&(p->blob_data_buffer[output_node_index]), output_data_length * sizeof(float));


	// 图象预处理 - 格式化操作
	cv::Mat image = cv::imread(image_path);
	int max_side_length = std::max(image.cols, image.rows);
	cv::Mat max_image = cv::Mat::zeros(cv::Size(max_side_length, max_side_length), CV_8UC3);
	cv::Rect roi(0, 0, image.cols, image.rows);
	image.copyTo(max_image(roi));
	// 将图像归一化，并放缩到指定大小
	cv::Size input_node_shape(input_node_dim.d[2], input_node_dim.d[3]);
	cv::Mat normal_image = cv::dnn::blobFromImage(max_image, 1 / 255.0, input_node_shape, cv::Scalar(0, 0, 0), true, false);

	// 创建输入cuda流
	cudaStreamCreate(&p->stream);
	std::vector<float> input_data(input_data_length);
	memcpy(input_data.data(), normal_image.ptr<float>(), input_data_length * sizeof(float));

	// 输入数据由内存到GPU显存
	cudaMemcpyAsync(p->blob_data_buffer[input_node_index], input_data.data(), input_data_length * sizeof(float), cudaMemcpyHostToDevice, p->stream);

	// 模型推理
	p->context->enqueueV2(p->blob_data_buffer, p->stream, nullptr);

	float* result_array = new float[output_data_length];
	cudaMemcpyAsync(result_array, p->blob_data_buffer[output_node_index], output_data_length * sizeof(float), cudaMemcpyDeviceToHost, p->stream);

	ResultYolov5 result;
	result.factor = max_side_length / (float) input_node_dim.d[2];
	result.read_class_names(lable_path);

	cv::Mat result_image = result.yolov5_result(image, result_array);

	// 查看输出结果
	cv::imshow("C++ + OpenVINO + Yolov5 推理结果", result_image);
	cv::waitKey();

}
