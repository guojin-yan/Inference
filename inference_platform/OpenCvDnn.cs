using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using OpenCvSharp.Dnn;
using CommomClass;

namespace inference_platform
{
    public class OpenCvDnn
    {
        public void yolov5_infer(string model_path, string image_path, string lable_path,
            out Mat result_image, out TimeSpan[] runtime)
        {
            string input_node_name = "images";
            string output_node_name = "output";

            DateTime start1_time = DateTime.Now;

            // 初始化网络类，读取本地模型
            Net net = CvDnn.ReadNetFromOnnx(model_path);

            DateTime end1_time = DateTime.Now;
            DateTime start2_time = DateTime.Now;

            // 配置图片数据
            Mat image = new Mat(image_path);
            int max = (image.Cols > image.Rows ? image.Cols : image.Rows);
            // 将图片放在矩形背景下
            Mat max_image = Mat.Zeros(new Size(max, max), MatType.CV_8UC3);
            Rect roi = new Rect(0, 0, image.Cols, image.Rows);
            image.CopyTo(new Mat(max_image, roi));
            // 数据归一化处理
            Mat BN_image = CvDnn.BlobFromImage(max_image, 1 / 255.0, new Size(640, 640), new Scalar(0, 0, 0), true, false);

            // 配置图片输入数据
            net.SetInput(BN_image);
            // 模型推理，读取推理结果
            Mat result_mat = net.Forward();

            DateTime end2_time = DateTime.Now;
            DateTime start3_time = DateTime.Now;

            // 将推理结果转为float数据类型
            Mat result_mat_to_float = new Mat(25200, 85, MatType.CV_32F, result_mat.Data);
            float[] result_array = new float[result_mat.Cols * result_mat.Rows];
            // 将数据读取到数组中
            result_mat_to_float.GetArray<float>(out result_array);
            // 创建yolov5结果处理类
            ResultYolov5 result = new ResultYolov5();
            // 读取本地模型类别信息
            result.read_class_names(lable_path);
            // 图片加载缩放比例
            result.factor = (float)max / (float)640;
            // 处理输出数据
            result_image = result.process_resule(image, result_array);
            DateTime end3_time = DateTime.Now;

            net.Dispose();

            runtime = new TimeSpan[4];
            runtime[0] = end3_time - start1_time;
            runtime[1] = end1_time - start1_time;
            runtime[2] = end2_time - start2_time;
            runtime[3] = end3_time - start3_time;
        }

        public void resnet50_infer(string model_path, string image_path, string lable_path,
            out Mat result_image, out List<Tuple<int, float>> result_list, out TimeSpan[] runtime)
        {
            // 模型输入节点名
            string input_node_name = "x";
            // 模型输出节点名
            string output_node_name = "softmax_1.tmp_0";

            DateTime start1_time = DateTime.Now;
            // 初始化网络类，读取本地模型
            Net net = CvDnn.ReadNetFromOnnx(model_path);

            DateTime end1_time = DateTime.Now;
            DateTime start2_time = DateTime.Now;


            // 配置图片数据
            Mat image = new Mat(image_path);

            // 数据归一化处理
            Mat BN_image = CvDnn.BlobFromImage(image, 1 / 255.0, new Size(224, 224), new Scalar(0, 0, 0), true, false);

            // 配置图片输入数据
            net.SetInput(BN_image);
            // 模型推理，读取推理结果
            Mat result_mat = net.Forward();

            DateTime end2_time = DateTime.Now;
            DateTime start3_time = DateTime.Now;

            // 将推理结果转为float数据类型
            Mat result_mat_to_float = new Mat(1, 102, MatType.CV_32F, result_mat.Data);
            float[] result_array = new float[result_mat.Cols * result_mat.Rows];
            // 将数据读取到数组中
            result_mat_to_float.GetArray<float>(out result_array);

            ResultResNET50 resultResNET50 = new ResultResNET50();
            resultResNET50.read_class_names(lable_path);
            int[] index; ;
            resultResNET50.process_resule(image, result_array, out result_image, out index);
            result_list = new List<Tuple<int, float>>();
            for (int i = 0; i < 5; i++)
            {
                Tuple<int, float> tuple = new Tuple<int, float>(index[i], result_array[index[i]]);
                result_list.Add(tuple);
            }

            DateTime end3_time = DateTime.Now;

            net.Dispose();

            runtime = new TimeSpan[4];
            runtime[0] = end3_time - start1_time;
            runtime[1] = end1_time - start1_time;
            runtime[2] = end2_time - start2_time;
            runtime[3] = end3_time - start3_time;

        }
    }
}
