#ifndef RESULT_H
#define RESULT_H
#include <fstream>
#include <iterator>
#include <memory>
#include <sstream>
#include <string>
#include <vector>

#include "opencv2/opencv.hpp"
std::vector<std::string> txt_to_vector(std::string path_name);
cv::Mat yolov5_result(cv::Mat image, float* result, std::vector<std::string> class_names, float factor);

#endif // !RESULT_H
