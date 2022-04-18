using System;
using OpenCvSharp;
using OpenCvSharp.Dnn;
using CommomClass;

namespace csharp_opencv_dnn_yolov5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 模型输入进本信息
            // 将模型文件放在英文目录下
            string model_path = "E:/Text_Model/yolov5/yolov5s.onnx";
            string image_path = "E:/Text_dataset/YOLOv5/0001.jpg";
            string lable_path = "E:/Git_space/Al模型部署开发方式/model/yolov5/lable.txt";
            string input_node_name = "images";
            string output_node_name = "output";

            // 初始化网络类，读取本地模型
            Net net = CvDnn.ReadNetFromOnnx(model_path);

            // 配置图片数据
            Mat image = new Mat(image_path);
            // 将图片放在矩形背景下
            Mat max_image = Mat.Zeros(new Size(1024, 1024), MatType.CV_8UC3);
            Rect roi = new Rect(0, 0, image.Cols, image.Rows);
            image.CopyTo(new Mat(max_image, roi));
            // 数据归一化处理
            Mat BN_image = CvDnn.BlobFromImage(max_image, 1 / 255.0, new Size(640, 640), new Scalar(0, 0, 0), true, false);

            // 配置图片输入数据
            net.SetInput(BN_image);

            // 模型推理，读取推理结果
            Mat result_mat = net.Forward();
            // 将推理结果转为float数据类型
            Mat result_mat_to_float = new Mat(25200, 85, MatType.CV_32F, result_mat.Data);
            float[] result_array = new float[result_mat.Cols* result_mat.Rows];
            // 将数据读取到数组中
            result_mat_to_float.GetArray<float>(out result_array);
            Console.WriteLine("Hello World!");
            // 创建yolov5结果处理类
            ResultYolov5 result = new ResultYolov5();
            // 读取本地模型类别信息
            result.read_class_names(lable_path);
            // 图片加载缩放比例
            result.factor = (float)(image.Cols > image.Rows ? image.Cols : image.Rows) / (float)640;
            // 处理输出数据
            Mat result_image = result.process_resule(image, result_array);

            Cv2.ImShow("C# + OpenCV Dnn + Yolov5 推理结果", result_image);
            Cv2.WaitKey();
        }
    }
}
