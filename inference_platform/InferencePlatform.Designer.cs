namespace inference_platform
{
    partial class InferencePlatform
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rb_opencvdnn = new System.Windows.Forms.RadioButton();
            this.rb_onnxruntime = new System.Windows.Forms.RadioButton();
            this.rb_tensorrt = new System.Windows.Forms.RadioButton();
            this.rb_openvino = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_model_choice = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_model_path = new System.Windows.Forms.TextBox();
            this.rb_xml = new System.Windows.Forms.RadioButton();
            this.rb_pdmodel = new System.Windows.Forms.RadioButton();
            this.rb_engine = new System.Windows.Forms.RadioButton();
            this.rb_onnx = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btn_image_choice = new System.Windows.Forms.Button();
            this.btn_lable_choice = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_image_path = new System.Windows.Forms.TextBox();
            this.tb_label_path = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rb_ppyoloe = new System.Windows.Forms.RadioButton();
            this.rb_resnet50 = new System.Windows.Forms.RadioButton();
            this.rb_yolov5 = new System.Windows.Forms.RadioButton();
            this.panel_image_show = new System.Windows.Forms.Panel();
            this.tb_message = new System.Windows.Forms.TextBox();
            this.model_inference = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.cb_result_chioce = new System.Windows.Forms.ComboBox();
            this.btn_save_result = new System.Windows.Forms.Button();
            this.btn_save_results = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rb_opencvdnn);
            this.groupBox1.Controls.Add(this.rb_onnxruntime);
            this.groupBox1.Controls.Add(this.rb_tensorrt);
            this.groupBox1.Controls.Add(this.rb_openvino);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.groupBox1.Location = new System.Drawing.Point(37, 327);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(735, 103);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "部署平台";
            // 
            // rb_opencvdnn
            // 
            this.rb_opencvdnn.AutoSize = true;
            this.rb_opencvdnn.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.rb_opencvdnn.Location = new System.Drawing.Point(539, 54);
            this.rb_opencvdnn.Name = "rb_opencvdnn";
            this.rb_opencvdnn.Size = new System.Drawing.Size(155, 25);
            this.rb_opencvdnn.TabIndex = 1;
            this.rb_opencvdnn.TabStop = true;
            this.rb_opencvdnn.Text = "OpenCV Dnn";
            this.rb_opencvdnn.UseVisualStyleBackColor = true;
            this.rb_opencvdnn.CheckedChanged += new System.EventHandler(this.rb_opencvdnn_CheckedChanged);
            // 
            // rb_onnxruntime
            // 
            this.rb_onnxruntime.AutoSize = true;
            this.rb_onnxruntime.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.rb_onnxruntime.Location = new System.Drawing.Point(334, 54);
            this.rb_onnxruntime.Name = "rb_onnxruntime";
            this.rb_onnxruntime.Size = new System.Drawing.Size(179, 25);
            this.rb_onnxruntime.TabIndex = 1;
            this.rb_onnxruntime.TabStop = true;
            this.rb_onnxruntime.Text = "ONNX runtime";
            this.rb_onnxruntime.UseVisualStyleBackColor = true;
            this.rb_onnxruntime.CheckedChanged += new System.EventHandler(this.rb_onnxruntime_CheckedChanged);
            // 
            // rb_tensorrt
            // 
            this.rb_tensorrt.AutoSize = true;
            this.rb_tensorrt.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.rb_tensorrt.Location = new System.Drawing.Point(173, 54);
            this.rb_tensorrt.Name = "rb_tensorrt";
            this.rb_tensorrt.Size = new System.Drawing.Size(131, 25);
            this.rb_tensorrt.TabIndex = 1;
            this.rb_tensorrt.TabStop = true;
            this.rb_tensorrt.Text = "TensorRT";
            this.rb_tensorrt.UseVisualStyleBackColor = true;
            this.rb_tensorrt.CheckedChanged += new System.EventHandler(this.rb_tensorrt_CheckedChanged);
            // 
            // rb_openvino
            // 
            this.rb_openvino.AutoSize = true;
            this.rb_openvino.Checked = true;
            this.rb_openvino.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.rb_openvino.Location = new System.Drawing.Point(25, 54);
            this.rb_openvino.Name = "rb_openvino";
            this.rb_openvino.Size = new System.Drawing.Size(131, 25);
            this.rb_openvino.TabIndex = 0;
            this.rb_openvino.TabStop = true;
            this.rb_openvino.Text = "OpenVINO";
            this.rb_openvino.UseVisualStyleBackColor = true;
            this.rb_openvino.CheckedChanged += new System.EventHandler(this.rb_openvino_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_model_choice);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.tb_model_path);
            this.groupBox2.Controls.Add(this.rb_xml);
            this.groupBox2.Controls.Add(this.rb_pdmodel);
            this.groupBox2.Controls.Add(this.rb_engine);
            this.groupBox2.Controls.Add(this.rb_onnx);
            this.groupBox2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.groupBox2.Location = new System.Drawing.Point(37, 153);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(735, 157);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "模型文件选择";
            // 
            // btn_model_choice
            // 
            this.btn_model_choice.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btn_model_choice.Location = new System.Drawing.Point(597, 105);
            this.btn_model_choice.Name = "btn_model_choice";
            this.btn_model_choice.Size = new System.Drawing.Size(112, 34);
            this.btn_model_choice.TabIndex = 4;
            this.btn_model_choice.Text = "选择";
            this.btn_model_choice.UseVisualStyleBackColor = true;
            this.btn_model_choice.Click += new System.EventHandler(this.btn_model_choice_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(25, 110);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 21);
            this.label1.TabIndex = 3;
            this.label1.Text = "模型文件：";
            // 
            // tb_model_path
            // 
            this.tb_model_path.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tb_model_path.Location = new System.Drawing.Point(146, 105);
            this.tb_model_path.Name = "tb_model_path";
            this.tb_model_path.Size = new System.Drawing.Size(428, 31);
            this.tb_model_path.TabIndex = 2;
            // 
            // rb_xml
            // 
            this.rb_xml.AutoSize = true;
            this.rb_xml.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.rb_xml.Location = new System.Drawing.Point(518, 54);
            this.rb_xml.Name = "rb_xml";
            this.rb_xml.Size = new System.Drawing.Size(83, 25);
            this.rb_xml.TabIndex = 1;
            this.rb_xml.TabStop = true;
            this.rb_xml.Text = ".xml";
            this.rb_xml.UseVisualStyleBackColor = true;
            this.rb_xml.CheckedChanged += new System.EventHandler(this.rb_xml_CheckedChanged);
            // 
            // rb_pdmodel
            // 
            this.rb_pdmodel.AutoSize = true;
            this.rb_pdmodel.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.rb_pdmodel.Location = new System.Drawing.Point(160, 54);
            this.rb_pdmodel.Name = "rb_pdmodel";
            this.rb_pdmodel.Size = new System.Drawing.Size(131, 25);
            this.rb_pdmodel.TabIndex = 1;
            this.rb_pdmodel.TabStop = true;
            this.rb_pdmodel.Text = ".pdmodel";
            this.rb_pdmodel.UseVisualStyleBackColor = true;
            this.rb_pdmodel.CheckedChanged += new System.EventHandler(this.rb_pdmodel_CheckedChanged);
            // 
            // rb_engine
            // 
            this.rb_engine.AutoSize = true;
            this.rb_engine.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.rb_engine.Location = new System.Drawing.Point(334, 54);
            this.rb_engine.Name = "rb_engine";
            this.rb_engine.Size = new System.Drawing.Size(119, 25);
            this.rb_engine.TabIndex = 1;
            this.rb_engine.TabStop = true;
            this.rb_engine.Text = ".engine";
            this.rb_engine.UseVisualStyleBackColor = true;
            this.rb_engine.CheckedChanged += new System.EventHandler(this.rb_engine_CheckedChanged);
            // 
            // rb_onnx
            // 
            this.rb_onnx.AutoSize = true;
            this.rb_onnx.Checked = true;
            this.rb_onnx.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.rb_onnx.Location = new System.Drawing.Point(25, 54);
            this.rb_onnx.Name = "rb_onnx";
            this.rb_onnx.Size = new System.Drawing.Size(95, 25);
            this.rb_onnx.TabIndex = 0;
            this.rb_onnx.TabStop = true;
            this.rb_onnx.Text = ".onnx";
            this.rb_onnx.UseVisualStyleBackColor = true;
            this.rb_onnx.CheckedChanged += new System.EventHandler(this.rb_onnx_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btn_image_choice);
            this.groupBox3.Controls.Add(this.btn_lable_choice);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.tb_image_path);
            this.groupBox3.Controls.Add(this.tb_label_path);
            this.groupBox3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.groupBox3.Location = new System.Drawing.Point(37, 447);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(735, 155);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "测试文件选择";
            // 
            // btn_image_choice
            // 
            this.btn_image_choice.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btn_image_choice.Location = new System.Drawing.Point(597, 55);
            this.btn_image_choice.Name = "btn_image_choice";
            this.btn_image_choice.Size = new System.Drawing.Size(112, 34);
            this.btn_image_choice.TabIndex = 4;
            this.btn_image_choice.Text = "选择";
            this.btn_image_choice.UseVisualStyleBackColor = true;
            this.btn_image_choice.Click += new System.EventHandler(this.btn_image_choice_Click);
            // 
            // btn_lable_choice
            // 
            this.btn_lable_choice.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btn_lable_choice.Location = new System.Drawing.Point(597, 103);
            this.btn_lable_choice.Name = "btn_lable_choice";
            this.btn_lable_choice.Size = new System.Drawing.Size(112, 34);
            this.btn_lable_choice.TabIndex = 4;
            this.btn_lable_choice.Text = "选择";
            this.btn_lable_choice.UseVisualStyleBackColor = true;
            this.btn_lable_choice.Click += new System.EventHandler(this.btn_lable_choice_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(25, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 21);
            this.label3.TabIndex = 3;
            this.label3.Text = "测试图片：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(25, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 21);
            this.label2.TabIndex = 3;
            this.label2.Text = "lable文件：";
            // 
            // tb_image_path
            // 
            this.tb_image_path.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tb_image_path.Location = new System.Drawing.Point(160, 56);
            this.tb_image_path.Name = "tb_image_path";
            this.tb_image_path.Size = new System.Drawing.Size(428, 31);
            this.tb_image_path.TabIndex = 2;
            // 
            // tb_label_path
            // 
            this.tb_label_path.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tb_label_path.Location = new System.Drawing.Point(159, 105);
            this.tb_label_path.Name = "tb_label_path";
            this.tb_label_path.Size = new System.Drawing.Size(428, 31);
            this.tb_label_path.TabIndex = 2;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rb_ppyoloe);
            this.groupBox4.Controls.Add(this.rb_resnet50);
            this.groupBox4.Controls.Add(this.rb_yolov5);
            this.groupBox4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.groupBox4.Location = new System.Drawing.Point(37, 35);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(735, 103);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "测试模型";
            // 
            // rb_ppyoloe
            // 
            this.rb_ppyoloe.AutoSize = true;
            this.rb_ppyoloe.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.rb_ppyoloe.Location = new System.Drawing.Point(334, 54);
            this.rb_ppyoloe.Name = "rb_ppyoloe";
            this.rb_ppyoloe.Size = new System.Drawing.Size(131, 25);
            this.rb_ppyoloe.TabIndex = 1;
            this.rb_ppyoloe.TabStop = true;
            this.rb_ppyoloe.Text = "PP-Yoloe";
            this.rb_ppyoloe.UseVisualStyleBackColor = true;
            this.rb_ppyoloe.CheckedChanged += new System.EventHandler(this.rb_ppyoloe_CheckedChanged);
            // 
            // rb_resnet50
            // 
            this.rb_resnet50.AutoSize = true;
            this.rb_resnet50.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.rb_resnet50.Location = new System.Drawing.Point(173, 54);
            this.rb_resnet50.Name = "rb_resnet50";
            this.rb_resnet50.Size = new System.Drawing.Size(131, 25);
            this.rb_resnet50.TabIndex = 1;
            this.rb_resnet50.TabStop = true;
            this.rb_resnet50.Text = "ResNet50";
            this.rb_resnet50.UseVisualStyleBackColor = true;
            this.rb_resnet50.CheckedChanged += new System.EventHandler(this.rb_resnet50_CheckedChanged);
            // 
            // rb_yolov5
            // 
            this.rb_yolov5.AutoSize = true;
            this.rb_yolov5.Checked = true;
            this.rb_yolov5.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.rb_yolov5.Location = new System.Drawing.Point(25, 54);
            this.rb_yolov5.Name = "rb_yolov5";
            this.rb_yolov5.Size = new System.Drawing.Size(107, 25);
            this.rb_yolov5.TabIndex = 0;
            this.rb_yolov5.TabStop = true;
            this.rb_yolov5.Text = "Yolov5";
            this.rb_yolov5.UseVisualStyleBackColor = true;
            this.rb_yolov5.CheckedChanged += new System.EventHandler(this.rb_yolov5_CheckedChanged);
            // 
            // panel_image_show
            // 
            this.panel_image_show.Location = new System.Drawing.Point(808, 105);
            this.panel_image_show.Name = "panel_image_show";
            this.panel_image_show.Size = new System.Drawing.Size(958, 700);
            this.panel_image_show.TabIndex = 1;
            // 
            // tb_message
            // 
            this.tb_message.Location = new System.Drawing.Point(37, 718);
            this.tb_message.Multiline = true;
            this.tb_message.Name = "tb_message";
            this.tb_message.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_message.Size = new System.Drawing.Size(744, 140);
            this.tb_message.TabIndex = 2;
            // 
            // model_inference
            // 
            this.model_inference.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.model_inference.Location = new System.Drawing.Point(37, 625);
            this.model_inference.Name = "model_inference";
            this.model_inference.Size = new System.Drawing.Size(148, 40);
            this.model_inference.TabIndex = 4;
            this.model_inference.Text = "模型推理";
            this.model_inference.UseVisualStyleBackColor = true;
            this.model_inference.Click += new System.EventHandler(this.model_inference_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(808, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(292, 30);
            this.label4.TabIndex = 3;
            this.label4.Text = "推理结果图片展示：";
            // 
            // cb_result_chioce
            // 
            this.cb_result_chioce.FormattingEnabled = true;
            this.cb_result_chioce.Items.AddRange(new object[] {
            "当前检测",
            "测试1",
            "测试2",
            "测试3",
            "测试4",
            "测试5"});
            this.cb_result_chioce.Location = new System.Drawing.Point(37, 685);
            this.cb_result_chioce.Name = "cb_result_chioce";
            this.cb_result_chioce.Size = new System.Drawing.Size(182, 32);
            this.cb_result_chioce.TabIndex = 5;
            this.cb_result_chioce.SelectedIndexChanged += new System.EventHandler(this.cb_result_chioce_SelectedIndexChanged);
            // 
            // btn_save_result
            // 
            this.btn_save_result.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btn_save_result.Location = new System.Drawing.Point(251, 625);
            this.btn_save_result.Name = "btn_save_result";
            this.btn_save_result.Size = new System.Drawing.Size(148, 40);
            this.btn_save_result.TabIndex = 4;
            this.btn_save_result.Text = "保存结果";
            this.btn_save_result.UseVisualStyleBackColor = true;
            this.btn_save_result.Click += new System.EventHandler(this.btn_save_result_Click);
            // 
            // btn_save_results
            // 
            this.btn_save_results.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btn_save_results.Location = new System.Drawing.Point(463, 625);
            this.btn_save_results.Name = "btn_save_results";
            this.btn_save_results.Size = new System.Drawing.Size(148, 40);
            this.btn_save_results.TabIndex = 4;
            this.btn_save_results.Text = "保存本地";
            this.btn_save_results.UseVisualStyleBackColor = true;
            this.btn_save_results.Click += new System.EventHandler(this.btn_save_results_Click);
            // 
            // InferencePlatform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1778, 876);
            this.Controls.Add(this.cb_result_chioce);
            this.Controls.Add(this.tb_message);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btn_save_results);
            this.Controls.Add(this.btn_save_result);
            this.Controls.Add(this.model_inference);
            this.Controls.Add(this.panel_image_show);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Name = "InferencePlatform";
            this.Text = "AI Model Inference Display Platform";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rb_tensorrt;
        private System.Windows.Forms.RadioButton rb_openvino;
        private System.Windows.Forms.RadioButton rb_opencvdnn;
        private System.Windows.Forms.RadioButton rb_onnxruntime;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_model_choice;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_model_path;
        private System.Windows.Forms.RadioButton rb_xml;
        private System.Windows.Forms.RadioButton rb_pdmodel;
        private System.Windows.Forms.RadioButton rb_engine;
        private System.Windows.Forms.RadioButton rb_onnx;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btn_image_choice;
        private System.Windows.Forms.Button btn_lable_choice;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_image_path;
        private System.Windows.Forms.TextBox tb_label_path;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rb_ppyoloe;
        private System.Windows.Forms.RadioButton rb_resnet50;
        private System.Windows.Forms.RadioButton rb_yolov5;
        private System.Windows.Forms.Panel panel_image_show;
        private System.Windows.Forms.TextBox tb_message;
        private System.Windows.Forms.Button model_inference;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cb_result_chioce;
        private System.Windows.Forms.Button btn_save_result;
        private System.Windows.Forms.Button btn_save_results;
    }
}
