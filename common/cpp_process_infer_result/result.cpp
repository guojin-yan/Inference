#include "result.h"


void ResultYolov5::read_class_names(std::string path_name)
{
	std::ifstream infile;
	infile.open(path_name.data());   //将文件流对象与文件连接起来 
	assert(infile.is_open());   //若失败,则输出错误消息,并终止程序运行 

	std::string str;
	while (getline(infile, str)) {
		class_names.push_back(str);
		std::cout << str << std::endl;
		str.clear();

	}
	infile.close();             //关闭文件输入流 

}

cv::Mat ResultYolov5::yolov5_result(cv::Mat image, float* result) {
	cv::Mat det_output = cv::Mat(25200, 85, CV_32F, result);
	//// post-process
	std::vector<cv::Rect> position_boxes;
	std::vector<int> classIds;
	std::vector<float> confidences;

	std::cout << det_output.rows << std::endl;
	for (int i = 0; i < det_output.rows; i++) {
		float confidence = det_output.at<float>(i, 4);
		if (confidence < 0.2) {
			continue;
		}
		std::cout << "confidence" << confidence << std::endl;
		cv::Mat classes_scores = det_output.row(i).colRange(5, 85);
		cv::Point classIdPoint;
		double score;
		// 获取一组数据中最大值及其位置
		minMaxLoc(classes_scores, 0, &score, 0, &classIdPoint);
		// 置信度 0～1之间
		if (score > 0.25)
		{
			float cx = det_output.at<float>(i, 0);
			float cy = det_output.at<float>(i, 1);
			float ow = det_output.at<float>(i, 2);
			float oh = det_output.at<float>(i, 3);
			int x = static_cast<int>((cx - 0.5 * ow) * factor);
			int y = static_cast<int>((cy - 0.5 * oh) * factor);
			int width = static_cast<int>(ow * factor);
			int height = static_cast<int>(oh * factor);
			cv::Rect box;
			box.x = x;
			box.y = y;
			box.width = width;
			box.height = height;

			position_boxes.push_back(box);
			classIds.push_back(classIdPoint.x);
			confidences.push_back(score);
		}
	}
	// NMS
	std::vector<int> indexes;
	cv::dnn::NMSBoxes(position_boxes, confidences, 0.25, 0.45, indexes);
	for (size_t i = 0; i < indexes.size(); i++) {
		int index = indexes[i];
		int idx = classIds[index];
		cv::rectangle(image, position_boxes[index], cv::Scalar(0, 0, 255), 2, 8);
		cv::rectangle(image, cv::Point(position_boxes[index].tl().x, position_boxes[index].tl().y - 20),
			cv::Point(position_boxes[index].br().x, position_boxes[index].tl().y), cv::Scalar(0, 255, 255), -1);
		cv::putText(image, class_names[idx], cv::Point(position_boxes[index].tl().x, position_boxes[index].tl().y - 10), cv::FONT_HERSHEY_SIMPLEX, .5, cv::Scalar(0, 0, 0));
	}

	return image;
}
