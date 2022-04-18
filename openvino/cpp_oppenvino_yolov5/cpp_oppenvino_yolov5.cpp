


#include <iterator>
#include <memory>
#include <sstream>
#include <string>
#include <vector>

#include "openvino/openvino.hpp"
#include "opencv2/opencv.hpp"

#include "common/cpp_process_infer_result/result.h"


// @brief 对网络的输入为图片数据的节点进行赋值，实现图片数据输入网络
// @param input_tensor 输入节点的tensor
// @param inpt_image 输入图片数据
void fill_tensor_data_image(ov::Tensor& input_tensor, const cv::Mat& input_image) {
    // 获取输入节点要求的输入图片数据的大小
    ov::Shape tensor_shape = input_tensor.get_shape();
    const size_t width = tensor_shape[3]; // 要求输入图片数据的宽度
    const size_t height = tensor_shape[2]; // 要求输入图片数据的高度
    const size_t channels = tensor_shape[1]; // 要求输入图片数据的维度
    // 读取节点数据内存指针
    float* input_tensor_data = input_tensor.data<float>();
    // 将图片数据填充到网络中
    // 原有图片数据为 H、W、C 格式，输入要求的为 C、H、W 格式
    for (size_t c = 0; c < channels; c++) {
        for (size_t h = 0; h < height; h++) {
            for (size_t w = 0; w < width; w++) {
                input_tensor_data[c * width * height + h * width + w] = input_image.at<cv::Vec<float, 3>>(h, w)[c];
            }
        }
    }
}

// @brief 推理核心结构体
typedef struct openvino_core {
    ov::Core core; // core对象
    std::shared_ptr<ov::Model> model_ptr; // 读取模型指针
    ov::CompiledModel compiled_model; // 模型加载到设备对象
    ov::InferRequest infer_request; // 推理请求对象
} CoreStruct;



int openvino_yolov5(std::string model_path, std::string image_path, std::string input_node_name, 
    std::string output_node_name, std::vector<std::string> class_names) {
    
    // 初始化推理核心结构体
    CoreStruct* p = new CoreStruct();
    p->model_ptr = p->core.read_model(model_path);
    p->compiled_model = p->core.compile_model(p->model_ptr, "CPU");
    p->infer_request = p->compiled_model.create_infer_request();
    
    // 对输入图片进行预处理
    // 获取输入节点tensor
    ov::Tensor input_image_tensor = p->infer_request.get_tensor(input_node_name);
    int input_H = input_image_tensor.get_shape()[2]; //获得"image"节点的Height
    int input_W = input_image_tensor.get_shape()[3]; //获得"image"节点的Width
    cv::Mat image = cv::imread(image_path);; // 读取输入图片
    // 将输入图片放置在正方形背景上
    int max_side_length = std::max(image.cols, image.rows);
    cv::Mat max_image = cv::Mat::zeros(cv::Size(max_side_length, max_side_length), CV_8UC3);
    cv::Rect roi(0, 0, image.cols, image.rows);
    image.copyTo(max_image(roi));
    // 交换RGB通道
    cv::Mat rgb_image;
    cv::cvtColor(max_image, rgb_image, cv::COLOR_BGRA2RGB);
    // 缩放至指定大小
    cv::Mat normal_image;
    cv::resize(rgb_image, normal_image, cv::Size(input_H, input_W), 0, 0, cv::INTER_LINEAR);
    // 将图像归一化
    std::vector<cv::Mat> rgb_channels(3);
    cv::split(normal_image, rgb_channels);// 分离数据通道
    for (auto i = 0; i < rgb_channels.size(); i++) {
        rgb_channels[i].convertTo(rgb_channels[i], CV_32FC1, 1.0 / 255, 0);
    }
    cv::merge(rgb_channels, normal_image);
    // 填充输入数据
    fill_tensor_data_image(input_image_tensor, normal_image);

    // 模型推理
    p->infer_request.infer();
    
    // 获取输出节点tensor
    const ov::Tensor& output_tensor = p->infer_request.get_tensor(output_node_name);
    // 读取输出数据
    float* results = output_tensor.data<float>();

    // 处理输出结果
    float factor = max_side_length / (float) input_H;
    cv::Mat result_image =  yolov5_result(image, results, class_names, factor);
     
    // 查看输出结果
    cv::imshow("openvino - yolov5 - result", result_image);        
    cv::waitKey();

}


int main() {


    std::string model_path = "E:/Text_Model/yolov5/yolov5s.onnx";
    std::string image_path = "E:/Text_dataset/YOLOv5/0001.jpg";
    std::string input_node_name = "images";
    std::string output_node_name = "output";

    std::string lable_path = "E:/Git_space/Al模型部署开发方式/model/yolov5/lable.txt";

    // 读取识别结果lable
    std::vector<std::string> class_names;
    class_names = txt_to_vector(lable_path);

    // 模型推理
    openvino_yolov5(model_path, image_path, input_node_name, output_node_name, class_names);
}


