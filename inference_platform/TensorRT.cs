using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using TensorRtSharp;
using CommomClass;

namespace inference_platform
{
    public class TensorRT
    {
        public void yolov5_infer(string model_path, string image_path, string lable_path,
           out Mat result_image, out TimeSpan[] runtime)
        {
            string input_node_name = "images";
            string output_node_name = "output";

            DateTime start1_time = DateTime.Now;

            // 创建模型推理类
            Nvinfer nvinfer = new Nvinfer();
            // 读取模型信息
            nvinfer.init(model_path, 2);
            // 配置输入输出gpu缓存区
            nvinfer.creat_gpu_buffer(input_node_name, 640 * 640 * 3);
            nvinfer.creat_gpu_buffer(output_node_name, 25200 * 85);

            DateTime end1_time = DateTime.Now;
            DateTime start2_time = DateTime.Now;

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

            DateTime end2_time = DateTime.Now;
            DateTime start3_time = DateTime.Now;

            // 读取推理结果
            float[] result_array = nvinfer.read_infer_result(output_node_name, 25200 * 85);

            // 创建yolov5结果处理类
            ResultYolov5 result = new ResultYolov5();
            // 加载图片放缩比例
            result.factor = max_image_length / (float)640;
            // 获取本地分类信息lable
            result.read_class_names(lable_path);
            // 处理结果数据
            result_image = result.process_resule(image, result_array);

            DateTime end3_time = DateTime.Now;

            nvinfer.delete();

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

            // 创建模型推理类
            Nvinfer nvinfer = new Nvinfer();
            // 读取模型信息
            nvinfer.init(model_path, 2);
            // 配置输入输出gpu缓存区
            nvinfer.creat_gpu_buffer(input_node_name, 224 * 224 * 3);
            nvinfer.creat_gpu_buffer(output_node_name, 102);

            DateTime end1_time = DateTime.Now;
            DateTime start2_time = DateTime.Now;


            Mat image = new Mat(image_path);
            byte[] image_data = new byte[2048 * 2048 * 3];
            //存储byte的长度
            ulong image_size = new ulong();
            image_data = image.ImEncode(".bmp");
            image_size = Convert.ToUInt64(image_data.Length);
            // 加载推理图片数据
            nvinfer.load_image_data(input_node_name, image_data, image_size, BNFlag.Normal);
            // 模型推理
            nvinfer.infer();

            DateTime end2_time = DateTime.Now;
            DateTime start3_time = DateTime.Now;

            // 读取推理结果
            float[] result_array = nvinfer.read_infer_result(output_node_name, 102);

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

            nvinfer.delete();

            runtime = new TimeSpan[4];
            runtime[0] = end3_time - start1_time;
            runtime[1] = end1_time - start1_time;
            runtime[2] = end2_time - start2_time;
            runtime[3] = end3_time - start3_time;

        }
    }
}
