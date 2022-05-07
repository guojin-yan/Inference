using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

        //// 模型文件地址
        //string model_path = @"E:\Text_Model\flowerclas\flower_clas.onnx";
        //// 图片文件地址
        //string image_path = @"E:\Text_dataset\flowers102\jpg\image_00327.jpg";
        //// lable文件地址
        //string lable_path = @"E:\Git_space\Al模型部署开发方式\model\resNet50\flowers102_label.txt";


        // 模型文件地址
        string model_path = @"E:\Text_Model\yolov5\yolov5s.engine";
        // 图片文件地址
        string image_path = @"E:\Git_space\Al模型部署开发方式\model\yolov5\text_image\0002.jpg";
        // lable文件地址
        string lable_path = @"E:\Git_space\Al模型部署开发方式\model\yolov5\lable.txt";

        // 模型测试结果存储
        string text_result = "";
        List<string> text_results = new List<string>();
        // 模型时间测试结果
        string text_result_time = "";
        List<string> text_results_time = new List<string>();

        Window image_window = null;

        public InferencePlatform()
        {

            InitializeComponent();
            image_window = new Window("result_image",0);
            EmbededOpenCVWindow.embeded_openCV_window("result_image", panel_image_show);
            // image_window.ShowImage(Cv2.ImRead(image_path));
            cb_result_chioce.SelectedIndex = 0;
            pb_speed.MarqueeAnimationSpeed = 1;

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

                text_result = yolov5_result_str(1, runtime, "yolov5模型，onnx/pdmodel/xml文件，openvino平台");
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
                text_result = text_result = yolov5_result_str(1, runtime, "yolov5模型，onnx文件，onnxruntime平台");
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
                text_result = text_result = yolov5_result_str(1, runtime, "yolov5模型，onnx文件，opencv dnn平台");
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
                text_result = yolov5_result_str(1, runtime, "yolov5模型，engine文件，tensorrt平台");
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

                text_result = resnet50_result_str(1, runtime, result_list, "resNet50模型，onnx/pdmodel/xml文件，openvino平台");
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
                text_result = resnet50_result_str(1, runtime, result_list, "resNet50模型，engine文件，tensorrt平台");
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
                text_result = resnet50_result_str(1, runtime, result_list, "resNet50模型，onnx文件，onnxruntime平台");
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
                text_result = resnet50_result_str(1, runtime, result_list, "resNet50模型，onnx文件，opencvdnn平台");
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
            if (cb_result_chioce.SelectedIndex != 0)
            {
                int index = Convert.ToInt16(cb_result_chioce.SelectedIndex.ToString());
                tb_message.Text = text_results[index-1];
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

        #region 时间测试
        private void model_inference_time_Click(object sender, EventArgs e)
        {
            cb_result_chioce_time.SelectedIndex = 0;
            pb_speed.Value = 0;
            // yolov5模型，onnx/pdmodel/xml文件，openvino平台
            if (model_choice == 1 && (model_file_type == 1 || model_file_type == 2 || model_file_type == 4)
                && deployment_platform == 1)
            {
                int num = Convert.ToInt16(tb_text_num.Text);

                TimeSpan[] total_time = new TimeSpan[num];
                TimeSpan[] pre_time = new TimeSpan[num];
                TimeSpan[] infer_time = new TimeSpan[num];
                TimeSpan[] process_time = new TimeSpan[num];

                TimeSpan[] runtime = new TimeSpan[4];
                Mat result_image = new Mat();
                OpenVino openVino = new OpenVino();
                for (int i = 0; i < num; i++) {
                    pb_speed.Value += 100 / num;
                    la_progress.Text = pb_speed.Value.ToString() + "%";
                    la_progress.Refresh();
                    openVino.yolov5_infer(model_path, image_path, lable_path,
                        out result_image, out runtime);
                    total_time[i] = runtime[0];
                    pre_time[i] = runtime[1];
                    infer_time[i] = runtime[2];
                    process_time[i] = runtime[3];
                }
               
                image_window.ShowImage(result_image);

                TimeSpan[] aver_time = average_time(total_time, pre_time, infer_time, process_time, num);

                text_result_time = yolov5_result_str(num, aver_time, " yolov5模型，onnx/pdmodel/xml文件，openvino平台");
                tb_message.Text = text_result_time;

            }
            // yolov5模型，onnx文件，onnxruntime平台
            else if (model_choice == 1 && model_file_type == 1 && deployment_platform == 3)
            {
                int num = Convert.ToInt16(tb_text_num.Text);

                TimeSpan[] total_time = new TimeSpan[num];
                TimeSpan[] pre_time = new TimeSpan[num];
                TimeSpan[] infer_time = new TimeSpan[num];
                TimeSpan[] process_time = new TimeSpan[num];

                TimeSpan[] runtime = new TimeSpan[4];
                Mat result_image = new Mat();
                OnnxRuntime onnxRuntime = new OnnxRuntime();
                for (int i = 0; i < num; i++)
                {
                    pb_speed.Value += 100 / num;
                    la_progress.Text = pb_speed.Value.ToString() + "%";
                    la_progress.Refresh();
                    onnxRuntime.yolov5_infer(model_path, image_path, lable_path,
                    out result_image, out runtime);
                    total_time[i] = runtime[0];
                    pre_time[i] = runtime[1];
                    infer_time[i] = runtime[2];
                    process_time[i] = runtime[3];
                   
                }
                
                image_window.ShowImage(result_image);

                TimeSpan[] aver_time = average_time(total_time, pre_time, infer_time, process_time, num);

                text_result_time = yolov5_result_str(num, aver_time, "yolov5模型，onnx文件，onnxruntime平台");

                tb_message.Text = text_result_time;
            }
            // yolov5模型，onnx文件，opencv dnn平台
            else if (model_choice == 1 && model_file_type == 1 && deployment_platform == 4)
            {
                int num = Convert.ToInt16(tb_text_num.Text);

                TimeSpan[] total_time = new TimeSpan[num];
                TimeSpan[] pre_time = new TimeSpan[num];
                TimeSpan[] infer_time = new TimeSpan[num];
                TimeSpan[] process_time = new TimeSpan[num];

                TimeSpan[] runtime = new TimeSpan[4];
                Mat result_image = new Mat();
                OpenCvDnn openCvDnn = new OpenCvDnn();
               
                for (int i = 0; i < num; i++)
                {
                    pb_speed.Value += 100 / num;
                    la_progress.Text = pb_speed.Value.ToString() + "%";
                    la_progress.Refresh();
                    openCvDnn.yolov5_infer(model_path, image_path, lable_path, 
                        out result_image, out runtime);
                    total_time[i] = runtime[0];
                    pre_time[i] = runtime[1];
                    infer_time[i] = runtime[2];
                    process_time[i] = runtime[3];           
                }

                image_window.ShowImage(result_image);

                TimeSpan[] aver_time = average_time(total_time, pre_time, infer_time, process_time, num);

                text_result_time = yolov5_result_str(num, aver_time, "yolov5模型，onnx文件，opencv dnn平台");

                tb_message.Text = text_result_time;

            }
            // yolov5模型，engine文件，tensorrt平台
            else if (model_choice == 1 && model_file_type == 3 && deployment_platform == 2)
            {
                int num = Convert.ToInt16(tb_text_num.Text);

                TimeSpan[] total_time = new TimeSpan[num];
                TimeSpan[] pre_time = new TimeSpan[num];
                TimeSpan[] infer_time = new TimeSpan[num];
                TimeSpan[] process_time = new TimeSpan[num];

                TimeSpan[] runtime = new TimeSpan[4];
                Mat result_image = new Mat();
                TensorRT tensorRT = new TensorRT();
                
                for (int i = 0; i < num; i++)
                {
                    pb_speed.Value += 100 / num;
                    la_progress.Text = pb_speed.Value.ToString() + "%";
                    la_progress.Refresh();
                    tensorRT.yolov5_infer(model_path, image_path, lable_path,
                     out result_image, out runtime);
                    total_time[i] = runtime[0];
                    pre_time[i] = runtime[1];
                    infer_time[i] = runtime[2];
                    process_time[i] = runtime[3];
                   
                }

                image_window.ShowImage(result_image);

                TimeSpan[] aver_time = average_time(total_time, pre_time, infer_time, process_time, num);

                text_result_time = yolov5_result_str(num, aver_time, "yolov5模型，engine文件，tensorrt平台");

                tb_message.Text = text_result_time;
                result_image.Dispose();

            }
            // resNet50模型，onnx/pdmodel/xml文件，openvino平台
            else if (model_choice == 2 && (model_file_type == 1 || model_file_type == 2 || model_file_type == 4)
                && deployment_platform == 1)
            {
                
                int num = Convert.ToInt16(tb_text_num.Text);

                TimeSpan[] total_time = new TimeSpan[num];
                TimeSpan[] pre_time = new TimeSpan[num];
                TimeSpan[] infer_time = new TimeSpan[num];
                TimeSpan[] process_time = new TimeSpan[num];

                TimeSpan[] runtime = new TimeSpan[4];
                Mat result_image = new Mat();
                List<Tuple<int, float>> result_list = new List<Tuple<int, float>> { };
                OpenVino openVino = new OpenVino();
                
                for (int i = 0; i < num; i++)
                {
                    pb_speed.Value +=  100/num;
                    la_progress.Text = pb_speed.Value.ToString() + "%";
                    la_progress.Refresh();
                    openVino.resnet50_infer(model_path, image_path, lable_path,
                      out result_image, out result_list, out runtime);
                    total_time[i] = runtime[0];
                    pre_time[i] = runtime[1];
                    infer_time[i] = runtime[2];
                    process_time[i] = runtime[3];

                }

                image_window.ShowImage(result_image);

                TimeSpan[] aver_time = average_time(total_time, pre_time, infer_time, process_time, num);

                text_result_time = resnet50_result_str(num, runtime, result_list, "resNet50模型，onnx/pdmodel/xml文件，openvino平台");

                tb_message.Text = text_result_time;

            }
            // resNet50模型，engine文件，tensorrt平台
            else if (model_choice == 2 && model_file_type == 3 && deployment_platform == 2)
            {
                int num = Convert.ToInt16(tb_text_num.Text);

                TimeSpan[] total_time = new TimeSpan[num];
                TimeSpan[] pre_time = new TimeSpan[num];
                TimeSpan[] infer_time = new TimeSpan[num];
                TimeSpan[] process_time = new TimeSpan[num];

                TimeSpan[] runtime = new TimeSpan[4];
                Mat result_image = new Mat();
                List<Tuple<int, float>> result_list = new List<Tuple<int, float>> { };
                TensorRT tensorRT = new TensorRT();
                
                for (int i = 0; i < num; i++)
                {
                    pb_speed.Value += 100 / num;
                    la_progress.Text = pb_speed.Value.ToString() + "%";
                    la_progress.Refresh();
                    tensorRT.resnet50_infer(model_path, image_path, lable_path,
                     out result_image, out result_list, out runtime);
                    total_time[i] = runtime[0];
                    pre_time[i] = runtime[1];
                    infer_time[i] = runtime[2];
                    process_time[i] = runtime[3];
                  
                }

                image_window.ShowImage(result_image);

                TimeSpan[] aver_time = average_time(total_time, pre_time, infer_time, process_time, num);

                text_result_time = resnet50_result_str(num, runtime, result_list, "resNet50模型，engine文件，tensorrt平台");

                tb_message.Text = text_result_time;

            }
            // resNet50模型，onnx文件，onnxruntime平台
            else if (model_choice == 2 && model_file_type == 1 && deployment_platform == 3)
            {
                int num = Convert.ToInt16(tb_text_num.Text);

                TimeSpan[] total_time = new TimeSpan[num];
                TimeSpan[] pre_time = new TimeSpan[num];
                TimeSpan[] infer_time = new TimeSpan[num];
                TimeSpan[] process_time = new TimeSpan[num];

                TimeSpan[] runtime = new TimeSpan[4];
                Mat result_image = new Mat();
                List<Tuple<int, float>> result_list = new List<Tuple<int, float>> { };
                OnnxRuntime onnxRuntime = new OnnxRuntime();
                
                for (int i = 0; i < num; i++)
                {
                    pb_speed.Value += 100 / num;
                    la_progress.Text = pb_speed.Value.ToString() + "%";
                    la_progress.Refresh();
                    onnxRuntime.resnet50_infer(model_path, image_path, lable_path,
                     out result_image, out result_list, out runtime);
                    total_time[i] = runtime[0];
                    pre_time[i] = runtime[1];
                    infer_time[i] = runtime[2];
                    process_time[i] = runtime[3];
                   
                }

                image_window.ShowImage(result_image);

                TimeSpan[] aver_time = average_time(total_time, pre_time, infer_time, process_time, num);

                text_result_time = resnet50_result_str(num, runtime, result_list, "resNet50模型，onnx文件，onnxruntime平台");

                tb_message.Text = text_result_time;
            }
            // resNet50模型，onnx文件，opencvdnn平台
            else if (model_choice == 2 && model_file_type == 1 && deployment_platform == 4)
            {
                int num = Convert.ToInt16(tb_text_num.Text);

                TimeSpan[] total_time = new TimeSpan[num];
                TimeSpan[] pre_time = new TimeSpan[num];
                TimeSpan[] infer_time = new TimeSpan[num];
                TimeSpan[] process_time = new TimeSpan[num];

                TimeSpan[] runtime = new TimeSpan[4];
                Mat result_image = new Mat();
                List<Tuple<int, float>> result_list = new List<Tuple<int, float>> { };
                OpenCvDnn openCvDnn = new OpenCvDnn();
                
                for (int i = 0; i < num; i++)
                {
                    pb_speed.Value += 100 / num;
                    la_progress.Text = pb_speed.Value.ToString() + "%";
                    la_progress.Refresh();
                    openCvDnn.resnet50_infer(model_path, image_path, lable_path,
                     out result_image, out result_list, out runtime);
                    total_time[i] = runtime[0];
                    pre_time[i] = runtime[1];
                    infer_time[i] = runtime[2];
                    process_time[i] = runtime[3];
                  
                }
                image_window.ShowImage(result_image);

                TimeSpan[] aver_time = average_time(total_time, pre_time, infer_time, process_time, num);

                text_result_time = resnet50_result_str(num, runtime, result_list, "resNet50模型，onnx文件，opencvdnn平台");

                tb_message.Text = text_result_time;
            }
            GC.Collect();


        }
        #endregion

        #region 其他控件
        private void cb_result_chioce_time_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_result_chioce_time.SelectedIndex != 0)
            {
                int index = Convert.ToInt16(cb_result_chioce_time.SelectedIndex.ToString());
                tb_message.Text = text_results_time[index - 1];
            }
        }

        private void btn_save_result_time_Click(object sender, EventArgs e)
        {
            text_results_time.Add(text_result_time);
        }

        private void btn_save_results_time_Click(object sender, EventArgs e)
        {
            FileStream file = new FileStream("text_results_time.txt", FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(file);
            writer.Flush();
            writer.BaseStream.Seek(0, SeekOrigin.Begin);
            for (int i = 0; i < text_results_time.Count; i++)
            {
                writer.WriteLine(text_results_time[i]);
                writer.WriteLine("\r\n");

            }
            writer.Flush();
            writer.Close();
            file.Close();

        }
        #endregion


        private TimeSpan[] average_time(TimeSpan[] total_time, TimeSpan[] pre_time,
            TimeSpan[] infer_time, TimeSpan[] process_time, int num) { 

            
            TimeSpan total = new TimeSpan(), pre = new TimeSpan(), infer = new TimeSpan(), pro = new TimeSpan();
            for (int i = 0; i < num; i++)
            {
                total += total_time[i];
                pre += pre_time[i];
                infer += infer_time[i];
                pro += process_time[i];
            }
             
            return new TimeSpan[4] { total/num, pre/num, infer/num, pro/num }; 
        }

        private string yolov5_result_str(int num, TimeSpan[] time, string message) {
            return
                    message+ ":  " + Environment.NewLine +
                    "模型文件：  " + model_path + Environment.NewLine +
                    "测试图片：  " + image_path + Environment.NewLine +
                    "测试次数：  " + num + Environment.NewLine +
                    "推理总时间：   " + time[0].TotalMilliseconds.ToString("0.00") + "ms" + Environment.NewLine +
                    "模型加载时间： " + time[1].TotalMilliseconds.ToString("0.00") + "ms;" + Environment.NewLine +
                    "模型推理时间： " + time[2].TotalMilliseconds.ToString("0.00") + "ms" + Environment.NewLine +
                    "后处理时间：   " + time[3].TotalMilliseconds.ToString("0.00") + "ms";
        }
        private string resnet50_result_str(int num, TimeSpan[] time, List<Tuple<int, float>> result_list
            , string message)
        {
            return
                    message + ":  " + Environment.NewLine +
                    "模型文件：  " + model_path + Environment.NewLine +
                    "测试图片：  " + image_path + Environment.NewLine +
                    "测试次数：  " + num + Environment.NewLine +
                    "推理总时间：   " + time[0].TotalMilliseconds.ToString("0.00") + "ms" + Environment.NewLine +
                    "模型加载时间： " + time[1].TotalMilliseconds.ToString("0.00") + "ms;" + Environment.NewLine +
                    "模型推理时间： " + time[2].TotalMilliseconds.ToString("0.00") + "ms" + Environment.NewLine +
                    "后处理时间：   " + time[3].TotalMilliseconds.ToString("0.00") + "ms" + Environment.NewLine +
                    "Index:  " + result_list[0].Item1 + ";  Score:   " + result_list[0].Item2 + ";" + Environment.NewLine +
                    "Index:  " + result_list[1].Item1 + ";  Score:   " + result_list[1].Item2 + ";" + Environment.NewLine +
                    "Index:  " + result_list[2].Item1 + ";  Score:   " + result_list[2].Item2 + ";" + Environment.NewLine +
                    "Index:  " + result_list[3].Item1 + ";  Score:   " + result_list[3].Item2 + ";" + Environment.NewLine +
                    "Index:  " + result_list[4].Item1 + ";  Score:   " + result_list[4].Item2 + ";";
        }

    }
}
