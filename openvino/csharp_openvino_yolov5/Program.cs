using System;
using OpenCvSharp;
using OpenVinoSharp;
using CommomClass;

namespace charp_openvino_yolov5
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

            // 创建模型推理类
            Core core = new Core(model_path, "CPU");

            // 配置图片数据
            Mat image = new Mat(image_path);
            // 将图片放在矩形背景下
            Mat max_image = Mat.Zeros(new Size(1024, 1024), MatType.CV_8UC3);
            Rect roi = new Rect(0, 0, image.Cols, image.Rows);
            image.CopyTo(new Mat(max_image, roi));
            byte[] image_data = new byte[2048 * 2048 * 3];
            ulong image_size = new ulong();
            image_data = max_image.ImEncode(".bmp");
            image_size = Convert.ToUInt64(image_data.Length);
            // 将图片数据加载到模型
            core.load_input_data(input_node_name, image_data, image_size, 1);
            // 模型推理
            core.infer();

            // 读取模型输出数据
            float[] result_array = core.read_infer_result<float>(output_node_name, 25200 * 85);

            // 创建yolov5结果处理类
            ResultYolov5 result = new ResultYolov5();
            // 读取本地模型类别信息
            result.read_class_names(lable_path);
            // 图片加载缩放比例
            result.factor = (float)(image.Cols > image.Rows ? image.Cols : image.Rows) / (float)640;
            // 处理输出数据
            Mat result_image = result.process_resule(image,result_array);

            Cv2.ImShow("C# + OpenVINO + Yolov5 推理结果", result_image);
            Cv2.WaitKey();

        }
    }
}
