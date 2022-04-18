using System;

namespace OpenVinoSharp
{
    /// <summary>
    ///  OpenVino推理类，可以实现简单的模型推理
    /// </summary>
    public partial class Core
    {
        private IntPtr ptr = new IntPtr();

        /// <summary>
        /// core类默认初始化方法
        /// </summary>
        public Core() { }

        /// <summary>
        /// core类参数输入初始化方法
        /// </summary>
        /// <param name="model_file">本地推理模型地址路径</param>
        /// <param name="device_name">设备名称</param>
        public Core(string model_file, string device_name)
        {
            // 初始化推理核心
            ptr = NativeMethods.core_init(model_file, device_name);
        }

        /// <summary>
        /// 设置推理模型的输入节点的大小
        /// </summary>
        /// <param name="input_node_name">输入节点名</param>
        /// <param name="input_size">输入形状大小数组</param>
        public void set_input_sharp(string input_node_name, ulong[] input_size)
        {
            // 获取输入数组长度
            int length = input_size.Length;
            if (length == 4)
            {
                // 长度为4，判断为设置图片输入的输入参数，调用设置图片形状方法
                ptr = NativeMethods.set_input_image_sharp(ptr, input_node_name, ref input_size[0]);
            }
            else if (length == 2)
            {
                // 长度为2，判断为设置普通数据输入的输入参数，调用设置普通数据形状方法
                ptr = NativeMethods.set_input_data_sharp(ptr, input_node_name, ref input_size[0]);
            }
            else
            {
                // 为防止输入发生异常，直接返回
                return;
            }
        }

        /// <summary>
        /// 加载推理数据
        /// </summary>
        /// <param name="input_node_name">输入节点名</param>
        /// <param name="input_data">输入数据数组</param>
        public void load_input_data(string input_node_name, float[] input_data)
        {
            ptr = NativeMethods.load_input_data(ptr, input_node_name, ref input_data[0]);
        }
 
        /// <summary>
        /// 加载图片推理数据
        /// </summary>
        /// <param name="input_node_name">输入节点名</param>
        /// <param name="image_data">图片矩阵</param>
        /// <param name="image_size">图片矩阵长度</param>
        /// <param name="BN_means">数据归一化处理方式 0：飞桨数据处理；1：普通数据处理</param>
        public void load_input_data(string input_node_name, byte[] image_data, ulong image_size, int BN_means)
        {
            ptr = NativeMethods.load_image_input_data(ptr, input_node_name, ref image_data[0], image_size, BN_means);
        }
        // @brief 模型推理
        public void infer()
        {
            ptr = NativeMethods.core_infer(ptr);
        }

        /// <summary>
        /// 读取推理结果数据
        /// </summary>
        /// <typeparam name="T">待读取的数据类型</typeparam>
        /// <param name="output_node_name">输出节点名</param>
        /// <param name="data_size">输出数据长度</param>
        /// <returns>推理结果数组</returns>
        public T[] read_infer_result<T>(string output_node_name, int data_size)
        {
            // 获取设定类型
            string t = typeof(T).ToString();
            // 新建返回值数组
            T[] result = new T[data_size];
            if (t == "System.Int32")
            { // 读取数据类型为整形数据
                int[] inference_result = new int[data_size];
                NativeMethods.read_infer_result_I32(ptr, output_node_name, data_size, ref inference_result[0]);
                result = (T[])Convert.ChangeType(inference_result, typeof(T[]));
                return result;
            }
            else
            { // 读取数据类型为浮点型数据
                float[] inference_result = new float[data_size];
                NativeMethods.read_infer_result_F32(ptr, output_node_name, data_size, ref inference_result[0]);
                result = (T[])Convert.ChangeType(inference_result, typeof(T[]));
                return result;
            }
        }

        /// <summary>
        /// 删除创建的地址
        /// </summary>
        public void delet()
        {
            NativeMethods.core_delet(ptr);
        }
    }

}
