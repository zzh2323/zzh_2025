namespace FingerTest
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbIdx = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.picFPImg = new System.Windows.Forms.PictureBox();
            this.textRes = new System.Windows.Forms.TextBox();
            this.bnIdentify = new System.Windows.Forms.Button();
            this.bnVerify = new System.Windows.Forms.Button();
            this.bnEnroll = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtLevelPwd = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.bnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picFPImg)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbIdx
            // 
            this.cmbIdx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbIdx.FormattingEnabled = true;
            this.cmbIdx.Location = new System.Drawing.Point(302, 23);
            this.cmbIdx.Name = "cmbIdx";
            this.cmbIdx.Size = new System.Drawing.Size(44, 20);
            this.cmbIdx.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(243, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 20;
            this.label1.Text = "设备号：";
            // 
            // picFPImg
            // 
            this.picFPImg.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picFPImg.Location = new System.Drawing.Point(244, 110);
            this.picFPImg.Name = "picFPImg";
            this.picFPImg.Size = new System.Drawing.Size(131, 161);
            this.picFPImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picFPImg.TabIndex = 19;
            this.picFPImg.TabStop = false;
            // 
            // textRes
            // 
            this.textRes.Location = new System.Drawing.Point(11, 161);
            this.textRes.Multiline = true;
            this.textRes.Name = "textRes";
            this.textRes.ReadOnly = true;
            this.textRes.Size = new System.Drawing.Size(210, 110);
            this.textRes.TabIndex = 18;
            // 
            // bnIdentify
            // 
            this.bnIdentify.Enabled = false;
            this.bnIdentify.Location = new System.Drawing.Point(119, 132);
            this.bnIdentify.Name = "bnIdentify";
            this.bnIdentify.Size = new System.Drawing.Size(75, 24);
            this.bnIdentify.TabIndex = 17;
            this.bnIdentify.Text = "指纹识别";
            this.bnIdentify.UseVisualStyleBackColor = true;
            this.bnIdentify.Click += new System.EventHandler(this.bnIdentify_Click);
            // 
            // bnVerify
            // 
            this.bnVerify.Enabled = false;
            this.bnVerify.Location = new System.Drawing.Point(37, 132);
            this.bnVerify.Name = "bnVerify";
            this.bnVerify.Size = new System.Drawing.Size(75, 23);
            this.bnVerify.TabIndex = 14;
            this.bnVerify.Text = "注册校验";
            this.bnVerify.UseVisualStyleBackColor = true;
            this.bnVerify.Click += new System.EventHandler(this.bnVerify_Click);
            // 
            // bnEnroll
            // 
            this.bnEnroll.Enabled = false;
            this.bnEnroll.Location = new System.Drawing.Point(37, 96);
            this.bnEnroll.Name = "bnEnroll";
            this.bnEnroll.Size = new System.Drawing.Size(75, 23);
            this.bnEnroll.TabIndex = 13;
            this.bnEnroll.Text = "开始注册";
            this.bnEnroll.UseVisualStyleBackColor = true;
            this.bnEnroll.Click += new System.EventHandler(this.bnEnroll_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(47, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 22;
            this.label2.Text = "姓名";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 23;
            this.label3.Text = "权限密钥";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(94, 23);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(100, 21);
            this.txtName.TabIndex = 24;
            // 
            // txtLevelPwd
            // 
            this.txtLevelPwd.Location = new System.Drawing.Point(94, 53);
            this.txtLevelPwd.Name = "txtLevelPwd";
            this.txtLevelPwd.Size = new System.Drawing.Size(100, 21);
            this.txtLevelPwd.TabIndex = 25;
            this.txtLevelPwd.UseSystemPasswordChar = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(212, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 26;
            this.label4.Text = "指纹图像：";
            // 
            // bnCancel
            // 
            this.bnCancel.Location = new System.Drawing.Point(119, 96);
            this.bnCancel.Name = "bnCancel";
            this.bnCancel.Size = new System.Drawing.Size(75, 23);
            this.bnCancel.TabIndex = 27;
            this.bnCancel.Text = "取消注册";
            this.bnCancel.UseVisualStyleBackColor = true;
            this.bnCancel.Click += new System.EventHandler(this.bnCancel_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 278);
            this.Controls.Add(this.bnCancel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtLevelPwd);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbIdx);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.picFPImg);
            this.Controls.Add(this.textRes);
            this.Controls.Add(this.bnIdentify);
            this.Controls.Add(this.bnVerify);
            this.Controls.Add(this.bnEnroll);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MyFinger";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picFPImg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbIdx;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox picFPImg;
        private System.Windows.Forms.TextBox textRes;
        private System.Windows.Forms.Button bnIdentify;
        private System.Windows.Forms.Button bnVerify;
        private System.Windows.Forms.Button bnEnroll;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtLevelPwd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button bnCancel;
    }
}

