#include<windows.h>

#include <fstream>
#include <iostream>
#include <sstream>
#include <vector>

#include "NvInfer.h"
#include "NvOnnxParser.h"
#include <opencv2/opencv.hpp>

// @brief ���ڴ���IBuilder��IRuntime��IRefitterʵ���ļ�¼������ͨ���ýӿڴ��������ж���
// ���ͷ����д����Ķ���֮ǰ����¼��Ӧһֱ��Ч��
// ��Ҫ��ʵ����ILogger���µ�log()������
class Logger : public nvinfer1::ILogger{
	void log(Severity severity, const char* message)  noexcept{
		// suppress info-level messages
		if (severity != Severity::kINFO)
			std::cout << message << std::endl;
	}
} gLogger;

// @brief 
typedef struct tensorRT_nvinfer {
	Logger logger;
	// �����л�����
	nvinfer1::IRuntime* runtime;
	// ��������
	// ����ģ�͵�ģ�ͽṹ��ģ�Ͳ����Լ����ż���kernel���ã�
	// ���ܿ�ƽ̨�Ϳ�TensorRT�汾��ֲ
	nvinfer1::ICudaEngine* engine;
	// ������
	// �����м�ֵ��ʵ�ʽ�������Ķ���
	// ��engine�������ɴ���������󣬽��ж���������
	nvinfer1::IExecutionContext* context;
	// cudn�����־
	cudaStream_t stream;
	// GPU�Դ�����/�������
	void** data_buffer;
} NvinferStruct;



// @brief ��wchar_t*�ַ���ָ��ת��Ϊstring�ַ�����ʽ
// @param wchar �����ַ�ָ��
// @return ת������string�ַ��� 
std::string wchar_to_string(const wchar_t* wchar) {
	// ��ȡ����ָ��ĳ���
	int path_size = WideCharToMultiByte(CP_OEMCP, 0, wchar, wcslen(wchar), NULL, 0, NULL, NULL);
	char* chars = new char[path_size + 1];
	// ��˫�ֽ��ַ���ת���ɵ��ֽ��ַ���
	WideCharToMultiByte(CP_OEMCP, 0, wchar, wcslen(wchar), chars, path_size, NULL, NULL);
	chars[path_size] = '\0';
	std::string pattern = chars;
	delete chars; //�ͷ��ڴ�
	return pattern;
}

// @brief ��wchar_t*�ַ���ָ��ת��Ϊstring�ַ�����ʽ
// @param wchar �����ַ�ָ��
// @return ת������string�ַ��� 
char* wchar_to_char(const wchar_t* wchar) {
	// ��ȡ����ָ��ĳ���
	int path_size = WideCharToMultiByte(CP_OEMCP, 0, wchar, wcslen(wchar), NULL, 0, NULL, NULL);
	char* chars = new char[path_size + 1];
	// ��˫�ֽ��ַ���ת���ɵ��ֽ��ַ���
	WideCharToMultiByte(CP_OEMCP, 0, wchar, wcslen(wchar), chars, path_size, NULL, NULL);
	chars[path_size] = '\0';
	return chars;
}

// @brief ��ͼƬ�ľ�������ת��Ϊopencv��mat����
// @param data ͼƬ����
// @param size ͼƬ���󳤶�
// @return ת�����mat����
cv::Mat data_to_mat(uchar* data, size_t size) {
	//��ͼƬ�������ݶ�ȡ��������
	std::vector<uchar> buf;
	for (int i = 0; i < size; i++) {
		buf.push_back(*data);
		data++;
	}
	// ����ͼƬ���룬�������е�����ת��Ϊmat����
	return cv::imdecode(cv::Mat(buf), 1);
}

// @brief ������onnxģ��תΪtensorrt�е�engine��ʽ�������浽����
// @param onnx_file_path_wchar onnxģ�ͱ��ص�ַ
// @param engine_file_path_wchar engineģ�ͱ��ص�ַ
// @param type ���ģ�;��ȣ�
extern "C"  __declspec(dllexport) void __stdcall onnx_to_engine(const wchar_t* onnx_file_path_wchar,
	const wchar_t* engine_file_path_wchar, int type) {
	std::string onnx_file_path = wchar_to_string(onnx_file_path_wchar);
	std::string engine_file_path = wchar_to_string(engine_file_path_wchar);

	// ����������ȡcuda�ں�Ŀ¼�Ի�ȡ����ʵ��
	// ���ڴ���config��network��engine����������ĺ�����
	nvinfer1::IBuilder* builder = nvinfer1::createInferBuilder(gLogger);
	// ������������
	const auto explicit_batch = 1U << static_cast<uint32_t>(nvinfer1::NetworkDefinitionCreationFlag::kEXPLICIT_BATCH);
	// ����onnx�����ļ�
	// tensorRTģ����
	nvinfer1::INetworkDefinition* network = builder->createNetworkV2(explicit_batch);
	// onnx�ļ�������
	// ��onnx�ļ������������rensorRT����ṹ
	nvonnxparser::IParser* parser = nvonnxparser::createParser(*network, gLogger);
	// ����onnx�ļ�
	parser->parseFromFile(onnx_file_path.c_str(), 2);
	for (int i = 0; i < parser->getNbErrors(); ++i) {
		std::cout << "load error: " << parser->getError(i)->desc() << std::endl;
	}
	printf("tensorRT load mask onnx model successfully!!!...\n");

	// ������������
	// �������������ö���
	nvinfer1::IBuilderConfig* config = builder->createBuilderConfig();
	// ����������ռ��С��
	config->setMaxWorkspaceSize(16 * (1 << 20));
	// ����ģ���������
	if (type == 1) {
		config->setFlag(nvinfer1::BuilderFlag::kFP16);
	}
	if (type == 2) {
		config->setFlag(nvinfer1::BuilderFlag::kINT8);
	}
	// ������������
	nvinfer1::ICudaEngine* engine = builder->buildEngineWithConfig(*network, *config);
	// ��������ǹ���浽����
	std::cout << "try to save engine file now~~~" << std::endl;
	std::ofstream file_ptr(engine_file_path, std::ios::binary);
	if (!file_ptr) {
		std::cerr << "could not open plan output file" << std::endl;
		return;
	}
	// ��ģ��ת��Ϊ�ļ�������
	nvinfer1::IHostMemory* model_stream = engine->serialize();
	// ���ļ����浽����
	file_ptr.write(reinterpret_cast<const char*>(model_stream->data()), model_stream->size());
	// ���ٴ����Ķ���
	model_stream->destroy();
	engine->destroy();
	network->destroy();
	parser->destroy();
	std::cout << "convert onnx model to TensorRT engine model successfully!" << std::endl;
}


// @brief ��ȡ����engineģ�ͣ�����ʼ��NvinferStruct
// @param engine_filename_wchar engine����ģ�͵�ַ
// @param num_ionode �Դ滺��������
// @return NvinferStruct�ṹ��ָ��
extern "C"  __declspec(dllexport) void* __stdcall nvinfer_init(const wchar_t* engine_filename_wchar, int num_ionode) {
	// ��ȡ����ģ���ļ�
	std::string engine_filename = wchar_to_string(engine_filename_wchar);
	// �Զ����Ʒ�ʽ��ȡ�ʼ�
	std::ifstream file_ptr(engine_filename, std::ios::binary);
	if (!file_ptr.good()) {
		std::cerr << "�ļ��޷��򿪣���ȷ���ļ��Ƿ���ã�" << std::endl;
	}

	size_t size = 0;
	file_ptr.seekg(0, file_ptr.end);	// ����ָ����ļ�ĩβ��ʼ�ƶ�0���ֽ�
	size = file_ptr.tellg();	// ���ض�ָ���λ�ã���ʱ��ָ���λ�þ����ļ����ֽ���
	file_ptr.seekg(0, file_ptr.beg);	// ����ָ����ļ���ͷ��ʼ�ƶ�0���ֽ�
	char* model_stream = new char[size];
	file_ptr.read(model_stream, size);
	// �ر��ļ�
	file_ptr.close();

	// ����������Ľṹ�壬��ʼ������
	NvinferStruct* p = new NvinferStruct();
	// ��ʼ�������л�����
	p->runtime = nvinfer1::createInferRuntime(gLogger);
	// ��ʼ����������
	p->engine = p->runtime->deserializeCudaEngine(model_stream, size);
	// ����������
	p->context = p->engine->createExecutionContext();
	// ����gpu���ݻ�����
	p->data_buffer = new void* [num_ionode];
	delete[] model_stream;
	return (void*)p;
}

// @brief ����GPU�Դ�����/���������
// @param nvinfer_ptr NvinferStruct�ṹ��ָ��
// @para node_name_wchar ����ڵ�����
// @param data_length ���������ݳ���
// @return NvinferStruct�ṹ��ָ��
extern "C"  __declspec(dllexport) void* __stdcall creat_gpu_buffer(void* nvinfer_ptr,
	const wchar_t* node_name_wchar, size_t data_length) {
	// �ع�NvinferStruct
	NvinferStruct* p = (NvinferStruct*)nvinfer_ptr;
	const char* node_name = wchar_to_char(node_name_wchar);
	// ��ȡ�ڵ����
	int node_index = p->engine->getBindingIndex(node_name);
	// ����ָ���ڵ�GPU�Դ滺����
	cudaMalloc(&(p->data_buffer[node_index]), data_length * sizeof(float));
	return (void*)p;
}

// @brief ����ͼƬ�������ݵ�������
// @param nvinfer_ptr NvinferStruct�ṹ��ָ��
// @para node_name_wchar ����ڵ�����
// @param image_data ͼƬ��������
// @param image_size ͼƬ���ݳ���
// @return NvinferStruct�ṹ��ָ��
extern "C"  __declspec(dllexport) void* __stdcall load_image_data(void* nvinfer_ptr,
	const wchar_t* node_name_wchar, uchar * image_data, size_t image_size, int BN_means) {
	// �ع�NvinferStruct
	NvinferStruct* p = (NvinferStruct*)nvinfer_ptr;

	// ��ȡ����ڵ���Ϣ
	const char* node_name = wchar_to_char(node_name_wchar);
	int node_index = p->engine->getBindingIndex(node_name);
	// ��ȡ����ڵ�δ����Ϣ
	nvinfer1::Dims node_dim = p->engine->getBindingDimensions(node_index);
	int node_shape_w = node_dim.d[2];
	int node_shape_h = node_dim.d[3];
	// ����ڵ��ά��״
	cv::Size node_shape(node_shape_w, node_shape_h);
	// ����ڵ��ά��С
	size_t node_data_length = node_shape_w * node_shape_h;

	// Ԥ������������
	cv::Mat input_image = data_to_mat(image_data, image_size);
	cv::Mat BN_image;
	if (BN_means == 0) {
		cv::cvtColor(input_image, input_image, cv::COLOR_BGR2RGB); // ��ͼƬͨ���� BGR תΪ RGB
		// ������ͼƬ����tensor����Ҫ���������
		cv::resize(input_image, BN_image, node_shape, 0, 0, cv::INTER_LINEAR);
		// ͼ�����ݹ�һ��
		std::vector<cv::Mat> rgb_channels(3);
		cv::split(BN_image, rgb_channels); // ����ͼƬ����ͨ��
		// PaddleDetectionģ��ʹ��imagenet���ݼ��ľ�ֵ Mean = [0.485, 0.456, 0.406]�ͷ��� std = [0.229, 0.224, 0.225]
		std::vector<float> mean_values{ 0.485 * 255, 0.456 * 255, 0.406 * 255 };
		std::vector<float> std_values{ 0.229 * 255, 0.224 * 255, 0.225 * 255 };
		for (auto i = 0; i < rgb_channels.size(); i++) {
			//��ͨ�����˶�ÿһ��ͨ�����ݽ��й�һ������
			rgb_channels[i].convertTo(rgb_channels[i], CV_32FC1, 1.0 / std_values[i], (0.0 - mean_values[i]) / std_values[i]);
		}
		cv::merge(rgb_channels, BN_image);
	}
	else if (BN_means == 1) {
		// ��ͼ���һ������������ָ����С
		BN_image = cv::dnn::blobFromImage(input_image, 1 / 255.0, node_shape, cv::Scalar(0, 0, 0), true, false);

	}


	// ����cuda��
	cudaStreamCreate(&p->stream);
	std::vector<float> input_data(node_data_length * 3);
	// ��ͼƬ����copy����������
	memcpy(input_data.data(), BN_image.ptr<float>(), node_data_length * 3 * sizeof(float));

	// �������������ڴ浽GPU�Դ�
	cudaMemcpyAsync(p->data_buffer[node_index], input_data.data(), node_data_length * 3 * sizeof(float), cudaMemcpyHostToDevice, p->stream);

	return (void*)p;
}


// @brief ģ������
// @param nvinfer_ptr NvinferStruct�ṹ��ָ��
// @return NvinferStruct�ṹ��ָ��
extern "C"  __declspec(dllexport) void* __stdcall infer(void* nvinfer_ptr) {
	NvinferStruct* p = (NvinferStruct*)nvinfer_ptr;
	// ģ������
	p->context->enqueueV2(p->data_buffer, p->stream, nullptr);
	return (void*)p;
}


// @brief ��ȡ��������
// @param nvinfer_ptr NvinferStruct�ṹ��ָ��
// @para node_name_wchar ����ڵ�����
// @param output_result �������ָ��
extern "C"  __declspec(dllexport) void __stdcall read_infer_result(void* nvinfer_ptr,
	const wchar_t* node_name_wchar, float* output_result, size_t node_data_length) {
	// �ع�NvinferStruct
	NvinferStruct* p = (NvinferStruct*)nvinfer_ptr;
	
	// ��ȡ����ڵ���Ϣ
	const char* node_name = wchar_to_char(node_name_wchar);
	int node_index = p->engine->getBindingIndex(node_name);
	// ��ȡ�������
	// �����������
	std::vector<float> output_data(node_data_length * 3);
	// �����������GPU�Դ浽�ڴ�
	cudaMemcpyAsync(output_data.data(), p->data_buffer[node_index], node_data_length * sizeof(float), cudaMemcpyDeviceToHost, p->stream);
	
	for (int i = 0; i < node_data_length; i++) {
		*output_result = output_data[i];
		output_result++;
	}

}

// @brief ɾ���ڴ��ַ
// @param nvinfer_ptr NvinferStruct�ṹ��ָ��
extern "C"  __declspec(dllexport) void __stdcall nvinfer_delete(void* nvinfer_ptr) {
	NvinferStruct* p = (NvinferStruct*)nvinfer_ptr;
	delete[] p->data_buffer;
	delete p->context;
	delete p->engine;
	delete p->runtime;
	delete p;
}