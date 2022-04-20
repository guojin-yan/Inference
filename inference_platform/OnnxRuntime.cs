using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using OpenCvSharp;
using CommomClass;


namespace inference_platform
{
    public class OnnxRuntime
    {
        public void yolov5_infer(string model_path, string image_path, string lable_path,
           out Mat result_image, out TimeSpan[] runtime)
        {
            string input_node_name = "images";
            string output_node_name = "output";


            DateTime start1_time = DateTime.Now;

            // 创建输出会话，用于输出模型读取信息
            SessionOptions options = new SessionOptions();
            options.LogSeverityLevel = OrtLoggingLevel.ORT_LOGGING_LEVEL_INFO;
            // 设置为CPU上运行
            options.AppendExecutionProvider_CPU(0);

            // 船舰推理模型类，读取本地模型文件
            InferenceSession onnx_session = new InferenceSession(model_path, options);//model_path 为onnx模型文件的路径

            DateTime end1_time = DateTime.Now;
            DateTime start2_time = DateTime.Now;

            // 处理图片数据
            Mat image = new Mat(image_path);
            // 将图片放在矩形背景下
            Mat max_image = Mat.Zeros(new Size(1024, 1024), MatType.CV_8UC3);
            Rect roi = new Rect(0, 0, image.Cols, image.Rows);
            image.CopyTo(new Mat(max_image, roi));
            // 将图片转为RGB通道
            Mat image_rgb = new Mat();
            Cv2.CvtColor(max_image, image_rgb, ColorConversionCodes.BGR2RGB);
            Mat resize_image = new Mat();
            Cv2.Resize(image_rgb, resize_image, new Size(640, 640));

            // 创建输入Tensor
            Tensor<float> input_tensor = new DenseTensor<float>(new[] { 1, 3, 640, 640 });
            for (int y = 0; y < resize_image.Height; y++)
            {
                for (int x = 0; x < resize_image.Width; x++)
                {
                    input_tensor[0, 0, y, x] = resize_image.At<Vec3b>(y, x)[0] / 255f;
                    input_tensor[0, 1, y, x] = resize_image.At<Vec3b>(y, x)[1] / 255f;
                    input_tensor[0, 2, y, x] = resize_image.At<Vec3b>(y, x)[2] / 255f;
                }
            }

            // 创建输入容器
            List<NamedOnnxValue> input_ontainer = new List<NamedOnnxValue>();
            //将 input_tensor 放入一个输入参数的容器，并指定名称
            input_ontainer.Add(NamedOnnxValue.CreateFromTensor(input_node_name, input_tensor));

            //运行 Inference 并获取结果
            IDisposableReadOnlyCollection<DisposableNamedOnnxValue> result_infer = onnx_session.Run(input_ontainer);

            DateTime end2_time = DateTime.Now;
            DateTime start3_time = DateTime.Now;

            // 将输出结果转为DisposableNamedOnnxValue数组
            DisposableNamedOnnxValue[] results_array = result_infer.ToArray();
            // 读取第一个节点输出并转为Tensor数据
            Tensor<float> tensors = results_array[0].AsTensor<float>();
            // 将数据转为float数组
            float[] result_array = tensors.ToArray();
            // 创建yolov5结果处理类
            ResultYolov5 result = new ResultYolov5();
            // 读取本地模型类别信息
            result.read_class_names(lable_path);
            // 图片加载缩放比例
            result.factor = (float)(image.Cols > image.Rows ? image.Cols : image.Rows) / (float)640;
            // 处理输出数据
            result_image = result.process_resule(image, result_array);

            DateTime end3_time = DateTime.Now;

            onnx_session.Dispose();

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

            // 创建输出会话，用于输出模型读取信息
            SessionOptions options = new SessionOptions();
            options.LogSeverityLevel = OrtLoggingLevel.ORT_LOGGING_LEVEL_INFO;
            // 设置为CPU上运行
            options.AppendExecutionProvider_CPU(0);

            // 船舰推理模型类，读取本地模型文件
            InferenceSession onnx_session = new InferenceSession(model_path, options);//model_path 为onnx模型文件的路径


            DateTime end1_time = DateTime.Now;
            DateTime start2_time = DateTime.Now;

            Mat image = new Mat(image_path);
            Mat image_rgb = new Mat();
            Cv2.CvtColor(image, image_rgb, ColorConversionCodes.BGR2RGB);
            Mat resize_image = new Mat();
            Cv2.Resize(image_rgb, resize_image, new Size(224, 224));
            // 创建输入Tensor
            Tensor<float> input_tensor = new DenseTensor<float>(new[] { 1, 3, 224, 224 });
            for (int y = 0; y < resize_image.Height; y++)
            {
                for (int x = 0; x < resize_image.Width; x++)
                {
                    input_tensor[0, 0, y, x] = resize_image.At<Vec3b>(y, x)[0] / 255f;
                    input_tensor[0, 1, y, x] = resize_image.At<Vec3b>(y, x)[1] / 255f;
                    input_tensor[0, 2, y, x] = resize_image.At<Vec3b>(y, x)[2] / 255f;
                }
            }
            // 创建输入容器
            List<NamedOnnxValue> input_ontainer = new List<NamedOnnxValue>();
            //将 input_tensor 放入一个输入参数的容器，并指定名称
            input_ontainer.Add(NamedOnnxValue.CreateFromTensor(input_node_name, input_tensor));

            //运行 Inference 并获取结果
            IDisposableReadOnlyCollection<DisposableNamedOnnxValue> result_infer = onnx_session.Run(input_ontainer);



            DateTime end2_time = DateTime.Now;
            DateTime start3_time = DateTime.Now;
            // 将输出结果转为DisposableNamedOnnxValue数组
            DisposableNamedOnnxValue[] results_array = result_infer.ToArray();
            // 读取第一个节点输出并转为Tensor数据
            Tensor<float> tensors = results_array[0].AsTensor<float>();
            // 将数据转为float数组
            float[] result_array = tensors.ToArray();

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

            onnx_session.Dispose();

            runtime = new TimeSpan[4];
            runtime[0] = end3_time - start1_time;
            runtime[1] = end1_time - start1_time;
            runtime[2] = end2_time - start2_time;
            runtime[3] = end3_time - start3_time;

        }
    }
}
