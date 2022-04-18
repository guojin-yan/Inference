#ifndef RESULT_H
#define RESULT_H

#include <fstream>
#include <iterator>
#include <memory>
#include <sstream>
#include <string>
#include <vector>

#include "opencv2/opencv.hpp"

// @brief 处理yolov5的结果
// @note __declspec(dllexport) 导出类识别标志，不加会出错
__declspec(dllexport) class ResultYolov5 {
public:
	std::vector<std::string> class_names;
	float factor;

	//ResultYolov5();
	void read_class_names(std::string path_name);
	cv::Mat yolov5_result(cv::Mat image, float* result);


};


#endif // !RESULT_H
