using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;

namespace inference_platform
{
    public partial class InferencePlatform : Form
    {
        string[] model_file_types = new string[] { ".onnx", ".pdmodel", ".engine", ".xml" };
        // 模型类型
        int model_choice = 1;
        // 模型文件类型
        int model_file_type = 1;
        // 部署平台
        int deployment_platform = 1;

        // 模型文件地址
        string model_path = @"E:\Text_Model\flowerclas\flower_clas.onnx";
        // 图片文件地址
        string image_path = @"E:\Text_dataset\flowers102\jpg\image_00001.jpg";
        // lable文件地址
        string lable_path = @"E:\Git_space\Al模型部署开发方式\model\resNet50\flowers102_label.txt";

        string text_result = "";
        List<string> text_results = new List<string>();


        Window image_window = null;

        public InferencePlatform()
        {

            InitializeComponent();
            image_window = new Window("result_image",0);
            EmbededOpenCVWindow.embeded_openCV_window("result_image", panel_image_show);
            // image_window.ShowImage(Cv2.ImRead(image_path));
            cb_result_chioce.SelectedIndex = 0;

        }

        #region Radiobutton设置
        private void rb_yolov5_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_yolov5.Checked)
            {
                model_choice = 1;
            }
        }

        private void rb_resnet50_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_resnet50.Checked)
            {
                model_choice = 2;
            }
        }

        private void rb_ppyoloe_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_ppyoloe.Checked)
            {
                model_choice = 3;
            }
        }

        private void rb_onnx_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_onnx.Checked)
            {
                model_file_type = 1;
            }
        }

        private void rb_pdmodel_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_pdmodel.Checked)
            {
                model_file_type = 2;
            }
        }

        private void rb_engine_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_engine.Checked)
            {
                model_file_type = 3;
            }
        }

        private void rb_xml_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_xml.Checked)
            {
                model_file_type = 4;
            }
        }

        private void rb_openvino_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_openvino.Checked)
            {
                deployment_platform = 1;
            }
        }

        private void rb_tensorrt_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_tensorrt.Checked)
            {
                deployment_platform = 2;
            }
        }

        private void rb_onnxruntime_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_onnxruntime.Checked)
            {
                deployment_platform = 3;
            }
        }

        private void rb_opencvdnn_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_opencvdnn.Checked)
            {
                deployment_platform = 4;
            }
        }


        #endregion

        #region 文件选择设置
        private void btn_model_choice_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            string file = "";
            dialog.Multiselect = true;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件夹";
            dialog.Filter = "所有文件(*.*)|*.*|onnx文件|*.onnx|pdmodel文件|*.pdmodel|engine文件|*.engine|xml文件|*.xml";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
               file = dialog.FileName;
            }
            // 查看文件扩展名
            string extension = Path.GetExtension(file);
            if (extension == model_file_types[model_file_type-1])
            {
                model_path = file;
                tb_model_path.Text = model_path;
                return;
            }
            else
            {
                tb_model_path.Text = "重新选择";
                return;
            }
           
        }

        private void btn_image_choice_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            string file = "";
            dialog.Multiselect = true;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件夹";
            dialog.Filter = "所有文件(*.*)|*.*|jpg文件|*.jpg|png文件|*.png|jepg文件|*.jepg";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                file = dialog.FileName;
            }
            image_path = file;
            tb_image_path.Text = image_path;

        }

        private void btn_lable_choice_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            string file = "";
            dialog.Multiselect = true;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件夹";
            dialog.Filter = "所有文件(*.*)|*.*";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                file = dialog.FileName;
            }
            lable_path = file; 
            tb_label_path.Text = lable_path;
        }
        #endregion

        #region 推理按键
        private void model_inference_Click(object sender, EventArgs e)
        {
            cb_result_chioce.SelectedIndex = 0;
            // yolov5模型，onnx/pdmodel/xml文件，openvino平台
            if (model_choice == 1 && (model_file_type == 1 || model_file_type == 2 || model_file_type == 4)
                && deployment_platform == 1)
            {
                TimeSpan[] runtime = new TimeSpan[4];
                Mat result_image = new Mat();
                OpenVino openVino = new OpenVino();
                openVino.yolov5_infer(model_path, image_path, lable_path,
                    out result_image, out runtime);
                image_window.ShowImage(result_image);

                text_result =
                    "yolov5模型，onnx/pdmodel/xml文件，openvino平台:  " + Environment.NewLine +
                    "模型文件：" + model_path + Environment.NewLine +
                    "测试图片：" + image_path + Environment.NewLine +
                    "推理总时间： " + runtime[0].TotalMilliseconds.ToString("0.00") +
                    "ms;    模型加载时间： " + runtime[1].TotalMilliseconds.ToString("0.00") + "ms;" + Environment.NewLine +
                    "模型推理时间： " + runtime[2].TotalMilliseconds.ToString("0.00") +
                    "ms;    后处理（NMS）时间： " + runtime[3].TotalMilliseconds.ToString("0.00") + "ms";
                tb_message.Text = text_result;

            }
            // yolov5模型，onnx文件，onnxruntime平台
            else if (model_choice == 1 && model_file_type == 1 && deployment_platform == 3)
            {
                TimeSpan[] runtime = new TimeSpan[4];
                Mat result_image = new Mat();
                OnnxRuntime onnxRuntime = new OnnxRuntime();
                onnxRuntime.yolov5_infer(model_path, image_path, lable_path,
                    out result_image, out runtime);
                image_window.ShowImage(result_image);
                text_result =
                    "yolov5模型，onnx文件，onnxruntime平台:  " + Environment.NewLine +
                    "模型文件：" + model_path + Environment.NewLine +
                    "测试图片：" + image_path + Environment.NewLine +
                    "推理总时间： " + runtime[0].TotalMilliseconds.ToString("0.00") +
                    "ms;    模型加载时间： " + runtime[1].TotalMilliseconds.ToString("0.00") + "ms;" + Environment.NewLine +
                    "模型推理时间： " + runtime[2].TotalMilliseconds.ToString("0.00") +
                    "ms;    后处理（NMS）时间： " + runtime[3].TotalMilliseconds.ToString("0.00") + "ms";
                tb_message.Text = text_result;
            }
            // yolov5模型，onnx文件，opencv dnn平台
            else if (model_choice == 1 && model_file_type == 1 && deployment_platform == 4)
            {
                TimeSpan[] runtime = new TimeSpan[4];
                Mat result_image = new Mat();
                OpenCvDnn openCvDnn = new OpenCvDnn();
                openCvDnn.yolov5_infer(model_path, image_path, lable_path,
                    out result_image, out runtime);
                image_window.ShowImage(result_image);
                text_result =
                    "yolov5模型，onnx文件，opencv dnn平台:  " + Environment.NewLine +
                    "模型文件：" + model_path + Environment.NewLine +
                    "测试图片：" + image_path + Environment.NewLine +
                    "推理总时间： " + runtime[0].TotalMilliseconds.ToString("0.00") +
                    "ms;    模型加载时间： " + runtime[1].TotalMilliseconds.ToString("0.00") + "ms;" + Environment.NewLine +
                    "模型推理时间： " + runtime[2].TotalMilliseconds.ToString("0.00") +
                    "ms;    后处理（NMS）时间： " + runtime[3].TotalMilliseconds.ToString("0.00") + "ms";
                tb_message.Text = text_result;

            }
            // yolov5模型，engine文件，tensorrt平台
            else if (model_choice == 1 && model_file_type == 3 && deployment_platform == 2)
            {
                TimeSpan[] runtime = new TimeSpan[4];
                Mat result_image = new Mat();
                TensorRT tensorRT = new TensorRT();
                tensorRT.yolov5_infer(model_path, image_path, lable_path,
                    out result_image, out runtime);
                image_window.ShowImage(result_image);
                text_result =
                    "yolov5模型，engine文件，tensorrt平台:  " + Environment.NewLine +
                    "模型文件：" + model_path + Environment.NewLine +
                    "测试图片：" + image_path + Environment.NewLine +
                    "推理总时间： " + runtime[0].TotalMilliseconds.ToString("0.00") +
                    "ms;    模型加载时间： " + runtime[1].TotalMilliseconds.ToString("0.00") + "ms;" + Environment.NewLine +
                    "模型推理时间： " + runtime[2].TotalMilliseconds.ToString("0.00") +
                    "ms;    后处理（NMS）时间： " + runtime[3].TotalMilliseconds.ToString("0.00") + "ms";
                tb_message.Text = text_result;
            }
            // resNet50模型，onnx/pdmodel/xml文件，openvino平台
            else if (model_choice == 2 && (model_file_type == 1 || model_file_type == 2 || model_file_type == 4)
                && deployment_platform == 1)
            {
                TimeSpan[] runtime = new TimeSpan[4];
                Mat result_image = new Mat();
                List<Tuple<int, float>> result_list;
                OpenVino openVino = new OpenVino();
                openVino.resnet50_infer(model_path, image_path, lable_path,
                    out result_image, out result_list, out runtime);
                image_window.ShowImage(result_image);

                text_result =
                    "yolov5模型，onnx/pdmodel/xml文件，openvino平台:  " + Environment.NewLine +
                    "模型文件：" + model_path + Environment.NewLine +
                    "测试图片：" + image_path + Environment.NewLine +
                    "推理总时间： " + runtime[0].TotalMilliseconds.ToString("0.00") +
                    "ms;    模型加载时间： " + runtime[1].TotalMilliseconds.ToString("0.00") + "ms;" + Environment.NewLine +
                    "模型推理时间： " + runtime[2].TotalMilliseconds.ToString("0.00") +
                    "ms;    后处理（NMS）时间： " + runtime[3].TotalMilliseconds.ToString("0.00") + "ms"+ Environment.NewLine +
                    "Index:  " + result_list[0].Item1 + ";  Score:   " + result_list[0].Item2 + ";" + Environment.NewLine +
                    "Index:  " + result_list[1].Item1 + ";  Score:   " + result_list[1].Item2 + ";" + Environment.NewLine +
                    "Index:  " + result_list[2].Item1 + ";  Score:   " + result_list[2].Item2 + ";" + Environment.NewLine +
                    "Index:  " + result_list[3].Item1 + ";  Score:   " + result_list[3].Item2 + ";" + Environment.NewLine +
                    "Index:  " + result_list[4].Item1 + ";  Score:   " + result_list[4].Item2 + ";";
                tb_message.Text = text_result;

            }
            // resNet50模型，engine文件，tensorrt平台
            else if (model_choice == 2 && model_file_type == 3 && deployment_platform == 2)
            {
                TimeSpan[] runtime = new TimeSpan[4];
                Mat result_image = new Mat();
                List<Tuple<int, float>> result_list;
                TensorRT tensorRT = new TensorRT();
                tensorRT.resnet50_infer(model_path, image_path, lable_path,
                    out result_image, out result_list, out runtime);
                image_window.ShowImage(result_image);

                text_result =
                    "resNet50模型，onnx/pdmodel/xml文件，tensorrt平台:  " + Environment.NewLine +
                    "模型文件：" + model_path + Environment.NewLine +
                    "测试图片：" + image_path + Environment.NewLine +
                    "推理总时间： " + runtime[0].TotalMilliseconds.ToString("0.00") +
                    "ms;    模型加载时间： " + runtime[1].TotalMilliseconds.ToString("0.00") + "ms;" + Environment.NewLine +
                    "模型推理时间： " + runtime[2].TotalMilliseconds.ToString("0.00") +
                    "ms;    后处理（NMS）时间： " + runtime[3].TotalMilliseconds.ToString("0.00") + "ms" + Environment.NewLine +
                    "Index:  " + result_list[0].Item1 + ";  Score:   " + result_list[0].Item2 + ";" + Environment.NewLine +
                    "Index:  " + result_list[1].Item1 + ";  Score:   " + result_list[1].Item2 + ";" + Environment.NewLine +
                    "Index:  " + result_list[2].Item1 + ";  Score:   " + result_list[2].Item2 + ";" + Environment.NewLine +
                    "Index:  " + result_list[3].Item1 + ";  Score:   " + result_list[3].Item2 + ";" + Environment.NewLine +
                    "Index:  " + result_list[4].Item1 + ";  Score:   " + result_list[4].Item2 + ";";
                tb_message.Text = text_result;

            }
            // resNet50模型，onnx文件，onnxruntime平台
            else if (model_choice == 2 && model_file_type == 1 && deployment_platform == 3)
            {
                TimeSpan[] runtime = new TimeSpan[4];
                Mat result_image = new Mat();
                List<Tuple<int, float>> result_list;
                OnnxRuntime onnxRuntime = new OnnxRuntime();
                onnxRuntime.resnet50_infer(model_path, image_path, lable_path,
                    out result_image, out result_list, out runtime);
                image_window.ShowImage(result_image);
                text_result =
                    "resNet50模型，onnx/pdmodel/xml文件，tensorrt平台:  " + Environment.NewLine +
                    "模型文件：" + model_path + Environment.NewLine +
                    "测试图片：" + image_path + Environment.NewLine +
                    "推理总时间： " + runtime[0].TotalMilliseconds.ToString("0.00") +
                    "ms;    模型加载时间： " + runtime[1].TotalMilliseconds.ToString("0.00") + "ms;" + Environment.NewLine +
                    "模型推理时间： " + runtime[2].TotalMilliseconds.ToString("0.00") +
                    "ms;    后处理（NMS）时间： " + runtime[3].TotalMilliseconds.ToString("0.00") + "ms" + Environment.NewLine +
                    "Index:  " + result_list[0].Item1 + ";  Score:   " + result_list[0].Item2 + ";" + Environment.NewLine +
                    "Index:  " + result_list[1].Item1 + ";  Score:   " + result_list[1].Item2 + ";" + Environment.NewLine +
                    "Index:  " + result_list[2].Item1 + ";  Score:   " + result_list[2].Item2 + ";" + Environment.NewLine +
                    "Index:  " + result_list[3].Item1 + ";  Score:   " + result_list[3].Item2 + ";" + Environment.NewLine +
                    "Index:  " + result_list[4].Item1 + ";  Score:   " + result_list[4].Item2 + ";";
                tb_message.Text = text_result;
            }
            // resNet50模型，onnx文件，opencvdnn平台
            else if (model_choice == 2 && model_file_type == 1 && deployment_platform == 4)
            {
                TimeSpan[] runtime = new TimeSpan[4];
                Mat result_image = new Mat();
                List<Tuple<int, float>> result_list;
                OpenCvDnn openCvDnn = new OpenCvDnn();
                openCvDnn.resnet50_infer(model_path, image_path, lable_path,
                    out result_image, out result_list, out runtime);
                image_window.ShowImage(result_image);
                text_result =
                    "resNet50模型，onnx/pdmodel/xml文件，tensorrt平台:  " + Environment.NewLine +
                    "模型文件：" + model_path + Environment.NewLine +
                    "测试图片：" + image_path + Environment.NewLine +
                    "推理总时间： " + runtime[0].TotalMilliseconds.ToString("0.00") +
                    "ms;    模型加载时间： " + runtime[1].TotalMilliseconds.ToString("0.00") + "ms;" + Environment.NewLine +
                    "模型推理时间： " + runtime[2].TotalMilliseconds.ToString("0.00") +
                    "ms;    后处理（NMS）时间： " + runtime[3].TotalMilliseconds.ToString("0.00") + "ms" + Environment.NewLine +
                    "Index:  " + result_list[0].Item1 + ";  Score:   " + result_list[0].Item2 + ";" + Environment.NewLine +
                    "Index:  " + result_list[1].Item1 + ";  Score:   " + result_list[1].Item2 + ";" + Environment.NewLine +
                    "Index:  " + result_list[2].Item1 + ";  Score:   " + result_list[2].Item2 + ";" + Environment.NewLine +
                    "Index:  " + result_list[3].Item1 + ";  Score:   " + result_list[3].Item2 + ";" + Environment.NewLine +
                    "Index:  " + result_list[4].Item1 + ";  Score:   " + result_list[4].Item2 + ";";
                tb_message.Text = text_result;
            }

        }
        #endregion

        #region 其他控件
        private void btn_save_result_Click(object sender, EventArgs e)
        {
            text_results.Add(text_result);
        }

        private void cb_result_chioce_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_result_chioce.SelectedIndex == 1)
            {
                tb_message.Text = text_results[0];
            }
            else if (cb_result_chioce.SelectedIndex == 2)
            {
                tb_message.Text = text_results[1];
            }
            else if (cb_result_chioce.SelectedIndex ==3)
            {
                tb_message.Text = text_results[2];
            }
            else if (cb_result_chioce.SelectedIndex ==4)
            {
                tb_message.Text = text_results[3];
            }
            else if (cb_result_chioce.SelectedIndex == 5)
            {
                tb_message.Text = text_results[4];
            }
        }
        private void btn_save_results_Click(object sender, EventArgs e)
        {
            FileStream file = new FileStream("text_results.txt", FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(file);
            writer.Flush();
            writer.BaseStream.Seek(0, SeekOrigin.Begin);
            for (int i = 0; i < text_results.Count; i++)
            {
                writer.WriteLine(text_results[i]);
                writer.WriteLine("\r\n");

            }
            writer.Flush();
            writer.Close();
            file.Close();
           
        }
        #endregion


    }
}
