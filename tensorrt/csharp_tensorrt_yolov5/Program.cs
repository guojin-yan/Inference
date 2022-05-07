using System;
using OpenCvSharp;
using TensorRtSharp;
using CommomClass;

namespace charp_tensorrt_yolov5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 模型基本信息
            string engine_path = "E:/Text_Model/yolov5/yolov5s1.engine";
            string image_path = "E:/Text_dataset/YOLOv5/0001.jpg";
            string lable_path = "E:/Git_space/Al模型部署开发方式/model/yolov5/lable.txt";
            string input_node_name = "images";
            string output_node_name = "output";

            // 创建模型推理类
            Nvinfer nvinfer = new Nvinfer();
            // 读取模型信息
            nvinfer.init(engine_path, 2);
            // 配置输入输出gpu缓存区
            nvinfer.creat_gpu_buffer(input_node_name, 640 * 640 * 3);
            nvinfer.creat_gpu_buffer(output_node_name, 25200 * 85);

            // 配置图片数据
            Mat image = new Mat(image_path);
            int max_image_length = image.Cols > image.Rows ? image.Cols : image.Rows;
            Mat max_image = Mat.Zeros(new Size(max_image_length, max_image_length), MatType.CV_8UC3);
            Rect roi = new Rect(0, 0, image.Cols, image.Rows);
            image.CopyTo(new Mat(max_image, roi));
            byte[] image_data = new byte[2048 * 2048 * 3];
            //存储byte的长度
            ulong image_size = new ulong();
            image_data = max_image.ImEncode(".bmp");
            image_size = Convert.ToUInt64(image_data.Length);

            // 加载推理图片数据
            nvinfer.load_image_data(input_node_name, image_data, image_size, BNFlag.Normal);
            // 模型推理
            nvinfer.infer();
            // 读取推理结果
            float[] result_array = nvinfer.read_infer_result(output_node_name, 25200 * 85);

            // 创建yolov5结果处理类
            ResultYolov5 result = new ResultYolov5();
            // 加载图片放缩比例
            result.factor = max_image_length / (float)640;
            // 获取本地分类信息lable
            result.read_class_names(lable_path);
            // 处理结果数据
            Mat result_image = result.process_resule(image, result_array);
            Console.WriteLine(i);


            Cv2.ImShow("C# + TensorRT + Yolov5 推理结果", result_image);
            Cv2.WaitKey();

        }
    }
}
