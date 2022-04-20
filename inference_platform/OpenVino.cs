using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using OpenVinoSharp;
using CommomClass;

namespace inference_platform
{
    public class OpenVino
    {
       public void yolov5_infer(string model_path, string image_path, string lable_path, 
           out Mat result_image, out TimeSpan[] runtime) 
        {
            string input_node_name = "images";
            string output_node_name = "output";

            DateTime start1_time = DateTime.Now;
            // 创建模型推理类
            Core core = new Core(model_path, "CPU");

            DateTime end1_time = DateTime.Now;
            DateTime start2_time = DateTime.Now;

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

            DateTime end2_time = DateTime.Now;
            DateTime start3_time = DateTime.Now;

            // 读取模型输出数据
            float[] result_array = core.read_infer_result<float>(output_node_name, 25200 * 85);

            // 创建yolov5结果处理类
            ResultYolov5 result = new ResultYolov5();
            // 读取本地模型类别信息
            result.read_class_names(lable_path);
            // 图片加载缩放比例
            result.factor = (float)(image.Cols > image.Rows ? image.Cols : image.Rows) / (float)640;
            // 处理输出数据
            result_image = result.process_resule(image, result_array);

            DateTime end3_time = DateTime.Now;

            core.delet();

            runtime = new TimeSpan[4];
            runtime[0] = end3_time - start1_time;
            runtime[1] = end1_time - start1_time;
            runtime[2] = end2_time - start2_time;
            runtime[3] = end3_time - start3_time;

            //return result_image.Clone();
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
            Core core = new Core(model_path, "CPU");

            DateTime end1_time = DateTime.Now;
            DateTime start2_time = DateTime.Now;

            // 设置图片输入大小
            ulong[] image_sharp = new ulong[] { 1, 3, 224, 224 };
            core.set_input_sharp(input_node_name, image_sharp);

            Mat image = new Mat(image_path);
            byte[] image_data = new byte[2048 * 2048 * 3];
            //存储byte的长度
            ulong image_size = new ulong();
            image_data = image.ImEncode(".bmp");
            image_size = Convert.ToUInt64(image_data.Length);
            core.load_input_data(input_node_name, image_data, image_size, 1);
            // 模型推理
            core.infer();

            DateTime end2_time = DateTime.Now;
            DateTime start3_time = DateTime.Now;
            // 读取推理结果
            float[] result_array = new float[102];
            result_array = core.read_infer_result<float>(output_node_name, 102);

            ResultResNET50 resultResNET50 = new ResultResNET50();
            resultResNET50.read_class_names(lable_path);
            int[] index; ;
            resultResNET50.process_resule(image, result_array, out result_image,out index);
            result_list = new List<Tuple<int,float>>();
            for (int i = 0; i < 5; i++)
            {
                Tuple<int, float> tuple = new Tuple<int, float>(index[i], result_array[index[i]]);
                result_list.Add(tuple);
            }

            DateTime end3_time = DateTime.Now;

            core.delet();

            runtime = new TimeSpan[4];
            runtime[0] = end3_time - start1_time;
            runtime[1] = end1_time - start1_time;
            runtime[2] = end2_time - start2_time;
            runtime[3] = end3_time - start3_time;

        }
    }
}
