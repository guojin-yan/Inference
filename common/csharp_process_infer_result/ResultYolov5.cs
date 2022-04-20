using System;
using System.Collections.Generic;
using System.IO;
using OpenCvSharp;
using OpenCvSharp.Dnn;

namespace CommomClass
{
    /// <summary>
    /// yolov5模型结果处理类
    /// </summary>
    public class ResultYolov5
    {
        // 识别结果类型
        public string[] class_names;
        // 图片放缩比例
        public float factor;

        /// <summary>
        /// 读取本地识别结果类型文件到内存
        /// </summary>
        /// <param name="path">文件路径</param>
        public void read_class_names(string path) {

            List<string> str = new List<string>(); 
            StreamReader sr = new StreamReader(path);
            string line; 
            while ((line = sr.ReadLine()) != null)
            {
                str.Add(line);
            }

            class_names = str.ToArray();
            
        }
        /// <summary>
        /// 处理yolov5模型结果
        /// </summary>
        /// <param name="image">原图片</param>
        /// <param name="result">识别结果</param>
        /// <returns>处理后的图片</returns>
        public Mat process_resule(Mat image, float[] result)
        {
            Mat result_image = image.Clone();

            Mat result_data = new Mat(25200, 85, MatType.CV_32F, result);
            
            // 存放结果list
            List<Rect> position_boxes = new List<Rect>();
            List<int> class_ids = new List<int>();
            List<float> confidences = new List<float>();
            // 预处理输出结果
            for (int i = 0; i < result_data.Rows; i++)
            {
                // 获取置信值
                float confidence = result_data.At<float>(i, 4);
                if (confidence < 0.2)
                {
                    continue;
                }
                Console.WriteLine(confidence);

                Mat classes_scores = result_data.Row(i).ColRange(5, 85);//GetArray(i, 5, classes_scores);
                Point max_classId_point, min_classId_point;
                double max_score, min_score;
                // 获取一组数据中最大值及其位置
                Cv2.MinMaxLoc(classes_scores, out min_score, out max_score,
                    out min_classId_point, out max_classId_point);
                // 置信度 0～1之间
                // 获取识别框信息
                if (max_score > 0.25)
                {
                    float cx = result_data.At<float>(i, 0);
                    float cy = result_data.At<float>(i, 1);
                    float ow = result_data.At<float>(i, 2);
                    float oh = result_data.At<float>(i, 3);
                    int x = (int)((cx - 0.5 * ow) * factor);
                    int y = (int)((cy - 0.5 * oh) * factor);
                    int width = (int)(ow * factor);
                    int height = (int)(oh * factor);
                    Rect box = new Rect();
                    box.X = x;
                    box.Y = y;
                    box.Width = width;
                    box.Height = height;

                    position_boxes.Add(box);
                    class_ids.Add(max_classId_point.X);
                    confidences.Add((float)max_score);
                }
            }

            // NMS非极大值抑制
            int[] indexes = new int[position_boxes.Count];
            CvDnn.NMSBoxes(position_boxes, confidences, 0.25f, 0.45f, out indexes);
            // 将识别结果绘制到图片上
            for (int i = 0; i < indexes.Length; i++)
            {
                int index = indexes[i];
                int idx = class_ids[index];
                Cv2.Rectangle(result_image, position_boxes[index], new Scalar(0, 0, 255), 2, LineTypes.Link8);
                Cv2.Rectangle(result_image, new Point(position_boxes[index].TopLeft.X, position_boxes[index].TopLeft.Y - 20),
                    new Point(position_boxes[index].BottomRight.X, position_boxes[index].TopLeft.Y), new Scalar(0, 255, 255), -1);
                Cv2.PutText(result_image, class_names[idx], new Point(position_boxes[index].X, position_boxes[index].Y - 10),
                    HersheyFonts.HersheySimplex, 0.6, new Scalar(0, 0, 0), 1);
            }

            //Cv2.ImShow("C# + TensorRT + Yolov5 推理结果", result_image);
            //Cv2.WaitKey();

            return result_image;

        }
    }
}
