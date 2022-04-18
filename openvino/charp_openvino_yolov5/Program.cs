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
            string model_path = "E:/Text_Model/yolov5/yolov5s.onnx";
            string image_path = "E:/Text_dataset/YOLOv5/0001.jpg";
            string lable_path = "E:/Git_space/Al模型部署开发方式/model/yolov5/lable.txt";
            string input_node_name = "images";
            string output_node_name = "output";

            Core core = new Core(model_path, "CPU");

            // 配置图片数据
            Mat image = new Mat(image_path);
            Mat max_image = Mat.Zeros(new Size(1024, 1024), MatType.CV_8UC3);
            Rect roi = new Rect(0, 0, image.Cols, image.Rows);
            image.CopyTo(new Mat(max_image, roi));
            byte[] image_data = new byte[2048 * 2048 * 3];
            //存储byte的长度
            ulong image_size = new ulong();
            image_data = max_image.ImEncode(".bmp");
            image_size = Convert.ToUInt64(image_data.Length);
            core.load_input_data(input_node_name, image_data, image_size, 1);
            core.infer();
            float[] result_array = core.read_infer_result<float>(output_node_name, 25200 * 85);

            ResultYolov5 result = new ResultYolov5();
            result.read_class_names(lable_path);
            result.factor = (float)(image.Cols > image.Rows ? image.Cols : image.Rows) / (float)640;

            Mat result_image = result.process_resule(image,result_array);

            Cv2.ImShow("C# + OpenVINO + Yolov5 推理结果", result_image);
            Cv2.WaitKey();

        }
    }
}
