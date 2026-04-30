namespace SimplePaint
{
    partial class Form1
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
            lblAppName = new Label();
            groupBox1 = new GroupBox();
            btnCircle = new Button();
            btnRectangle = new Button();
            btnLine = new Button();
            cmbColor = new ComboBox();
            groupBox2 = new GroupBox();
            groupBox3 = new GroupBox();
            trbLineWidth = new TrackBar();
            btnOpenFile = new Button();
            btnSaveFile = new Button();
            picCanvas = new PictureBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trbLineWidth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picCanvas).BeginInit();
            SuspendLayout();
            // 
            // lblAppName
            // 
            lblAppName.AutoSize = true;
            lblAppName.Font = new Font("맑은 고딕", 20F);
            lblAppName.Location = new Point(58, 41);
            lblAppName.Name = "lblAppName";
            lblAppName.Size = new Size(211, 46);
            lblAppName.TabIndex = 0;
            lblAppName.Text = "Simple Paint";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnCircle);
            groupBox1.Controls.Add(btnRectangle);
            groupBox1.Controls.Add(btnLine);
            groupBox1.Location = new Point(19, 109);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(250, 95);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "도형 선택";
            // 
            // btnCircle
            // 
            btnCircle.Image = Properties.Resources.KakaoTalk_20260430_100205471_01;
            btnCircle.ImageAlign = ContentAlignment.TopCenter;
            btnCircle.Location = new Point(156, 26);
            btnCircle.Name = "btnCircle";
            btnCircle.Size = new Size(71, 59);
            btnCircle.TabIndex = 2;
            btnCircle.Text = "원";
            btnCircle.TextAlign = ContentAlignment.BottomCenter;
            btnCircle.UseVisualStyleBackColor = true;
            // 
            // btnRectangle
            // 
            btnRectangle.Image = Properties.Resources.KakaoTalk_20260430_100205471;
            btnRectangle.ImageAlign = ContentAlignment.TopCenter;
            btnRectangle.Location = new Point(85, 26);
            btnRectangle.Name = "btnRectangle";
            btnRectangle.Size = new Size(65, 59);
            btnRectangle.TabIndex = 1;
            btnRectangle.Text = "사각형";
            btnRectangle.TextAlign = ContentAlignment.BottomCenter;
            btnRectangle.UseVisualStyleBackColor = true;
            // 
            // btnLine
            // 
            btnLine.Image = Properties.Resources.KakaoTalk_20260430_1002054711;
            btnLine.ImageAlign = ContentAlignment.TopCenter;
            btnLine.Location = new Point(16, 26);
            btnLine.Name = "btnLine";
            btnLine.Size = new Size(63, 59);
            btnLine.TabIndex = 0;
            btnLine.Text = "직선";
            btnLine.TextAlign = ContentAlignment.BottomCenter;
            btnLine.UseVisualStyleBackColor = true;
            // 
            // cmbColor
            // 
            cmbColor.FormattingEnabled = true;
            cmbColor.Items.AddRange(new object[] { "Black 검정", "Red 빨강", "Blue 파랑", "Green 녹색" });
            cmbColor.Location = new Point(6, 40);
            cmbColor.Name = "cmbColor";
            cmbColor.Size = new Size(139, 28);
            cmbColor.TabIndex = 2;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(cmbColor);
            groupBox2.Location = new Point(287, 109);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(151, 95);
            groupBox2.TabIndex = 3;
            groupBox2.TabStop = false;
            groupBox2.Text = "색 선택";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(trbLineWidth);
            groupBox3.Location = new Point(455, 109);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(185, 95);
            groupBox3.TabIndex = 4;
            groupBox3.TabStop = false;
            groupBox3.Text = "선두께";
            // 
            // trbLineWidth
            // 
            trbLineWidth.Location = new Point(23, 33);
            trbLineWidth.Name = "trbLineWidth";
            trbLineWidth.Size = new Size(130, 56);
            trbLineWidth.TabIndex = 0;
            // 
            // btnOpenFile
            // 
            btnOpenFile.BackColor = SystemColors.Info;
            btnOpenFile.Location = new Point(655, 121);
            btnOpenFile.Name = "btnOpenFile";
            btnOpenFile.Size = new Size(112, 65);
            btnOpenFile.TabIndex = 5;
            btnOpenFile.Text = "열기";
            btnOpenFile.UseVisualStyleBackColor = false;
            // 
            // btnSaveFile
            // 
            btnSaveFile.BackColor = SystemColors.GradientInactiveCaption;
            btnSaveFile.Location = new Point(786, 121);
            btnSaveFile.Name = "btnSaveFile";
            btnSaveFile.Size = new Size(113, 65);
            btnSaveFile.TabIndex = 6;
            btnSaveFile.Text = "저장";
            btnSaveFile.UseVisualStyleBackColor = false;
            // 
            // picCanvas
            // 
            picCanvas.BackColor = Color.White;
            picCanvas.Location = new Point(19, 210);
            picCanvas.Name = "picCanvas";
            picCanvas.Size = new Size(880, 281);
            picCanvas.TabIndex = 7;
            picCanvas.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(938, 503);
            Controls.Add(picCanvas);
            Controls.Add(btnSaveFile);
            Controls.Add(btnOpenFile);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(lblAppName);
            Name = "Form1";
            Text = "Simple Paint v1.0";
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trbLineWidth).EndInit();
            ((System.ComponentModel.ISupportInitialize)picCanvas).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblAppName;
        private GroupBox groupBox1;
        private Button btnCircle;
        private Button btnRectangle;
        private Button btnLine;
        private ComboBox cmbColor;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private Button btnOpenFile;
        private Button btnSaveFile;
        private TrackBar trbLineWidth;
        private PictureBox picCanvas;
    }
}
