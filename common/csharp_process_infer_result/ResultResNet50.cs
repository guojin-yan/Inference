using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace CommomClass
{
    public class ResultResNET50
    {
        // 识别结果类型
        public string[] class_names;

        /// <summary>
        /// 读取本地识别结果类型文件到内存
        /// </summary>
        /// <param name="path">文件路径</param>
        public void read_class_names(string path)
        {

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
        /// 获取数组中前N个最大的数据的位置
        /// </summary>
        /// <param name="result">输入数组</param>
        /// <param name="max_num">n</param>
        /// <returns>中前N个最大的数据的索引值</returns>
        private int[] find_array_max(float[] result, int max_num)
        {
            int size = result.Length;
            float[] temp_result = new float[size];
            // 拷贝输入数据
            for (int i = 0; i < size; i++)
            {
                temp_result[i] = result[i];
            }
            // 冒泡排序法排序
            for (int i = 0; i < size; i++)
            {
                float max = temp_result[i];
                for (int j = i + 1; j < size; j++)
                {
                    if (max < temp_result[j])
                    {
                        float temp = temp_result[j];
                        temp_result[j] = max;
                        max = temp;
                    }
                }
                temp_result[i] = max;
            }
            // 获取指定数据的索引值
            int[] index = new int[max_num];
            for (int i = 0; i < max_num; i++)
            {
                int s;
                for (s = 0; s < size; s++)
                {
                    if (result[s] == temp_result[i])
                        break;
                }
                index[i] = s;
            }
            return index;
        }

        public void process_resule(Mat image,float[] result, out Mat result_image, out int[] index) 
        {
            index = new int[5];
            index = find_array_max(result, 5);

            result_image = image.Clone();
            Cv2.PutText(result_image, "Index: "+ class_names[index[0]], new Point(20, 50),
                    HersheyFonts.HersheySimplex, 1, new Scalar(0, 0, 255), 2);
            Cv2.PutText(result_image, "Score: " + result[index[0]], new Point(20, 90),
                   HersheyFonts.HersheySimplex, 1, new Scalar(0, 0, 255), 2);
        }
    }
}
