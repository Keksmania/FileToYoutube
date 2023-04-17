
namespace FileToYoutube
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.startButton = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Select1 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.Select2 = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.fpsNumber = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.videoHeightNumber = new System.Windows.Forms.NumericUpDown();
            this.videoWidthNumber = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.infoLabel = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.button4 = new System.Windows.Forms.Button();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.youtubeGroup = new System.Windows.Forms.GroupBox();
            this.youtubeButton = new System.Windows.Forms.Button();
            this.youtubeList = new System.Windows.Forms.ListBox();
            this.youtubeText = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.PasswordEncode = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.PasswordDecode = new System.Windows.Forms.TextBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpsNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.videoHeightNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.videoWidthNumber)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.youtubeGroup.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.startButton.Location = new System.Drawing.Point(6, 50);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(331, 23);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = false;
            this.startButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 404);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(343, 23);
            this.progressBar1.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.Select1);
            this.groupBox2.Location = new System.Drawing.Point(12, 23);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(343, 48);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Step 1: Select file";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(7, 20);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(249, 20);
            this.textBox1.TabIndex = 1;
            // 
            // Select1
            // 
            this.Select1.Location = new System.Drawing.Point(262, 19);
            this.Select1.Name = "Select1";
            this.Select1.Size = new System.Drawing.Size(75, 23);
            this.Select1.TabIndex = 0;
            this.Select1.Text = "Browse";
            this.Select1.UseVisualStyleBackColor = true;
            this.Select1.Click += new System.EventHandler(this.Select1_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBox2);
            this.groupBox3.Controls.Add(this.Select2);
            this.groupBox3.Location = new System.Drawing.Point(12, 77);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(343, 48);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Step 2: Select work folder";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(7, 19);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(249, 20);
            this.textBox2.TabIndex = 16;
            // 
            // Select2
            // 
            this.Select2.Location = new System.Drawing.Point(262, 18);
            this.Select2.Name = "Select2";
            this.Select2.Size = new System.Drawing.Size(75, 23);
            this.Select2.TabIndex = 15;
            this.Select2.Text = "Browse";
            this.Select2.UseVisualStyleBackColor = true;
            this.Select2.Click += new System.EventHandler(this.Select2_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.fpsNumber);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.videoHeightNumber);
            this.groupBox5.Controls.Add(this.videoWidthNumber);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Location = new System.Drawing.Point(12, 131);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(343, 107);
            this.groupBox5.TabIndex = 16;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Step 3: video options";
            // 
            // fpsNumber
            // 
            this.fpsNumber.Location = new System.Drawing.Point(186, 33);
            this.fpsNumber.Maximum = new decimal(new int[] {
            144,
            0,
            0,
            0});
            this.fpsNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.fpsNumber.Name = "fpsNumber";
            this.fpsNumber.Size = new System.Drawing.Size(120, 20);
            this.fpsNumber.TabIndex = 7;
            this.fpsNumber.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(183, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(24, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Fps";
            // 
            // videoHeightNumber
            // 
            this.videoHeightNumber.Location = new System.Drawing.Point(15, 74);
            this.videoHeightNumber.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.videoHeightNumber.Minimum = new decimal(new int[] {
            144,
            0,
            0,
            0});
            this.videoHeightNumber.Name = "videoHeightNumber";
            this.videoHeightNumber.Size = new System.Drawing.Size(120, 20);
            this.videoHeightNumber.TabIndex = 4;
            this.videoHeightNumber.Value = new decimal(new int[] {
            720,
            0,
            0,
            0});
            // 
            // videoWidthNumber
            // 
            this.videoWidthNumber.Location = new System.Drawing.Point(15, 33);
            this.videoWidthNumber.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.videoWidthNumber.Minimum = new decimal(new int[] {
            144,
            0,
            0,
            0});
            this.videoWidthNumber.Name = "videoWidthNumber";
            this.videoWidthNumber.Size = new System.Drawing.Size(120, 20);
            this.videoWidthNumber.TabIndex = 3;
            this.videoWidthNumber.Value = new decimal(new int[] {
            720,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Width";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Height";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.startButton);
            this.groupBox6.Location = new System.Drawing.Point(12, 298);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(343, 100);
            this.groupBox6.TabIndex = 17;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Step 4: start processing";
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.Location = new System.Drawing.Point(509, 1048);
            this.infoLabel.MinimumSize = new System.Drawing.Size(100, 0);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(100, 13);
            this.infoLabel.TabIndex = 18;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.textBox4);
            this.groupBox8.Controls.Add(this.button3);
            this.groupBox8.Location = new System.Drawing.Point(376, 190);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(343, 48);
            this.groupBox8.TabIndex = 17;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Step 2: Select work folder";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(7, 19);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(249, 20);
            this.textBox4.TabIndex = 16;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(262, 18);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 15;
            this.button3.Text = "Browse";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 7);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Encode";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(379, 7);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Decode";
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.button4);
            this.groupBox9.Location = new System.Drawing.Point(376, 298);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(343, 100);
            this.groupBox9.TabIndex = 19;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Step 3: start processing";
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button4.Location = new System.Drawing.Point(6, 50);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(331, 23);
            this.button4.TabIndex = 0;
            this.button4.Text = "Start";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(376, 404);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(343, 23);
            this.progressBar2.TabIndex = 18;
            // 
            // youtubeGroup
            // 
            this.youtubeGroup.Controls.Add(this.youtubeButton);
            this.youtubeGroup.Controls.Add(this.youtubeList);
            this.youtubeGroup.Controls.Add(this.youtubeText);
            this.youtubeGroup.Location = new System.Drawing.Point(376, 23);
            this.youtubeGroup.Name = "youtubeGroup";
            this.youtubeGroup.Size = new System.Drawing.Size(343, 138);
            this.youtubeGroup.TabIndex = 23;
            this.youtubeGroup.TabStop = false;
            this.youtubeGroup.Text = "Step 1: Youtube links in order (first: part 1, second: part 2 and so on)";
            // 
            // youtubeButton
            // 
            this.youtubeButton.Location = new System.Drawing.Point(262, 18);
            this.youtubeButton.Name = "youtubeButton";
            this.youtubeButton.Size = new System.Drawing.Size(75, 23);
            this.youtubeButton.TabIndex = 2;
            this.youtubeButton.Text = "Add";
            this.youtubeButton.UseVisualStyleBackColor = true;
            this.youtubeButton.Click += new System.EventHandler(this.youtubeButton_Click);
            // 
            // youtubeList
            // 
            this.youtubeList.FormattingEnabled = true;
            this.youtubeList.Location = new System.Drawing.Point(7, 47);
            this.youtubeList.Name = "youtubeList";
            this.youtubeList.Size = new System.Drawing.Size(330, 82);
            this.youtubeList.TabIndex = 1;
            // 
            // youtubeText
            // 
            this.youtubeText.Location = new System.Drawing.Point(7, 20);
            this.youtubeText.Name = "youtubeText";
            this.youtubeText.Size = new System.Drawing.Size(249, 20);
            this.youtubeText.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(644, 164);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Delete Link";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.PasswordEncode);
            this.groupBox1.Location = new System.Drawing.Point(12, 244);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(343, 53);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Optional: Key";
            // 
            // PasswordEncode
            // 
            this.PasswordEncode.Location = new System.Drawing.Point(6, 20);
            this.PasswordEncode.Name = "PasswordEncode";
            this.PasswordEncode.Size = new System.Drawing.Size(331, 20);
            this.PasswordEncode.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.PasswordDecode);
            this.groupBox4.Location = new System.Drawing.Point(376, 244);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(343, 53);
            this.groupBox4.TabIndex = 18;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Optional: Key";
            // 
            // PasswordDecode
            // 
            this.PasswordDecode.Location = new System.Drawing.Point(6, 20);
            this.PasswordDecode.Name = "PasswordDecode";
            this.PasswordDecode.Size = new System.Drawing.Size(331, 20);
            this.PasswordDecode.TabIndex = 0;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(731, 461);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.youtubeGroup);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.progressBar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(500);
            this.Text = "FileToVideoEncoder";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpsNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.videoHeightNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.videoWidthNumber)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.youtubeGroup.ResumeLayout(false);
            this.youtubeGroup.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button Select1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button Select2;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.NumericUpDown fpsNumber;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown videoHeightNumber;
        private System.Windows.Forms.NumericUpDown videoWidthNumber;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.GroupBox youtubeGroup;
        private System.Windows.Forms.Button youtubeButton;
        private System.Windows.Forms.ListBox youtubeList;
        private System.Windows.Forms.TextBox youtubeText;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox PasswordEncode;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox PasswordDecode;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}

