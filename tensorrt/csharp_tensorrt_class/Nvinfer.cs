﻿using System;

namespace TensorRtSharp
{
    /// <summary>
    /// TensorRT推理类，可以实现模型推理
    /// </summary>
    public class Nvinfer
    {
        private IntPtr ptr = new IntPtr();
        public Nvinfer() { }
        /// <summary>
        /// 将onnx模型转为enging
        /// </summary>
        /// <param name="onnx_file_path">onnx模型本地地址</param>
        /// <param name="engine_file_path">engine模型本地保存地址</param>
        /// <param name="type">模型精度类型</param>
        public void onnx_to_engine(string onnx_file_path, string engine_file_path, int type)
        {
            NativeMethods.onnx_to_engine(onnx_file_path, engine_file_path, type);
        }
        /// <summary>
        /// 读取本地engine模型
        /// </summary>
        /// <param name="engine_filename">engine模型本地地址</param>
        /// <param name="num_ionode">输入输出节点数量</param>
        public void init(string engine_filename, int num_ionode)
        {
            ptr = NativeMethods.nvinfer_init(engine_filename, num_ionode);
        }
        /// <summary>
        /// 创建gpu显存缓存
        /// </summary>
        /// <param name="node_name">网络节点名称</param>
        /// <param name="data_length">数据长度</param>
        public void creat_gpu_buffer(string node_name, ulong data_length)
        {
            ptr = NativeMethods.creat_gpu_buffer(ptr, node_name, data_length);
        }
        /// <summary>
        /// 加载图片数据到推理模型
        /// </summary>
        /// <param name="node_name">节点名称</param>
        /// <param name="image_data">图片矩阵数据</param>
        /// <param name="image_size">图片数据长度</param>
        /// <param name="BN_means">归一化处理数据方式</param>
        public void load_image_data(string node_name, byte[] image_data, ulong image_size,int BN_means)
        {
            ptr = NativeMethods.load_image_data(ptr, node_name, ref image_data[0], image_size, BN_means);
        }
        /// <summary>
        /// 模型推理
        /// </summary>
        public void infer()
        {
            ptr = NativeMethods.infer(ptr);
        }
        /// <summary>
        /// 读取推理结果信息
        /// </summary>
        /// <param name="node_name_wchar">模型节点名</param>
        /// <param name="data_length">输出数据长度</param>
        /// <returns>输出数据数组</returns>
        public float[] read_infer_result(string node_name_wchar,ulong data_length)
        {
            float[] result = new float[data_length];
            NativeMethods.read_infer_result(ptr, node_name_wchar, ref result[0], data_length);
            return result;
        }
        /// <summary>
        /// 删除模型内存地址
        /// </summary>
        public void delete()
        {
            NativeMethods.nvinfer_delete(ptr);
        }

    }
}
