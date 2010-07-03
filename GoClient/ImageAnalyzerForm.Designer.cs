namespace GoClient
{
	partial class ImageAnalyzerForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.panel1 = new System.Windows.Forms.Panel();
			this.WindowTitle = new System.Windows.Forms.Label();
			this.FinishButton = new System.Windows.Forms.Button();
			this.AudioCheckBox = new System.Windows.Forms.CheckBox();
			this.timeLabel = new System.Windows.Forms.Label();
			this.RecordButton = new System.Windows.Forms.Button();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.Preview = new System.Windows.Forms.PictureBox();
			this.ProcessingTime = new System.Windows.Forms.Label();
			this.FrameCounterLabel = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.Preview)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.FrameCounterLabel);
			this.panel1.Controls.Add(this.ProcessingTime);
			this.panel1.Controls.Add(this.WindowTitle);
			this.panel1.Controls.Add(this.FinishButton);
			this.panel1.Controls.Add(this.AudioCheckBox);
			this.panel1.Controls.Add(this.timeLabel);
			this.panel1.Controls.Add(this.RecordButton);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 182);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(193, 70);
			this.panel1.TabIndex = 1;
			// 
			// WindowTitle
			// 
			this.WindowTitle.AutoSize = true;
			this.WindowTitle.Location = new System.Drawing.Point(10, 48);
			this.WindowTitle.Name = "WindowTitle";
			this.WindowTitle.Size = new System.Drawing.Size(66, 13);
			this.WindowTitle.TabIndex = 7;
			this.WindowTitle.Text = "WindowTitle";
			// 
			// FinishButton
			// 
			this.FinishButton.Enabled = false;
			this.FinishButton.Location = new System.Drawing.Point(129, 6);
			this.FinishButton.Name = "FinishButton";
			this.FinishButton.Size = new System.Drawing.Size(52, 20);
			this.FinishButton.TabIndex = 6;
			this.FinishButton.Text = "Finish";
			this.FinishButton.UseVisualStyleBackColor = true;
			this.FinishButton.Click += new System.EventHandler(this.FinishButton_Click);
			// 
			// AudioCheckBox
			// 
			this.AudioCheckBox.AutoSize = true;
			this.AudioCheckBox.Location = new System.Drawing.Point(13, 9);
			this.AudioCheckBox.Name = "AudioCheckBox";
			this.AudioCheckBox.Size = new System.Drawing.Size(53, 17);
			this.AudioCheckBox.TabIndex = 5;
			this.AudioCheckBox.Text = "Audio";
			this.AudioCheckBox.UseVisualStyleBackColor = true;
			// 
			// timeLabel
			// 
			this.timeLabel.AutoSize = true;
			this.timeLabel.Location = new System.Drawing.Point(10, 29);
			this.timeLabel.Name = "timeLabel";
			this.timeLabel.Size = new System.Drawing.Size(30, 13);
			this.timeLabel.TabIndex = 4;
			this.timeLabel.Text = "Time";
			// 
			// RecordButton
			// 
			this.RecordButton.Location = new System.Drawing.Point(68, 5);
			this.RecordButton.Name = "RecordButton";
			this.RecordButton.Size = new System.Drawing.Size(55, 20);
			this.RecordButton.TabIndex = 1;
			this.RecordButton.Text = "Record";
			this.RecordButton.UseVisualStyleBackColor = true;
			this.RecordButton.Click += new System.EventHandler(this.start_Click);
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// Preview
			// 
			this.Preview.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Preview.Location = new System.Drawing.Point(0, 0);
			this.Preview.Name = "Preview";
			this.Preview.Size = new System.Drawing.Size(193, 182);
			this.Preview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.Preview.TabIndex = 2;
			this.Preview.TabStop = false;
			// 
			// ProcessingTime
			// 
			this.ProcessingTime.AutoSize = true;
			this.ProcessingTime.Location = new System.Drawing.Point(146, 29);
			this.ProcessingTime.Name = "ProcessingTime";
			this.ProcessingTime.Size = new System.Drawing.Size(38, 13);
			this.ProcessingTime.TabIndex = 8;
			this.ProcessingTime.Text = "Speed";
			// 
			// FrameCounterLabel
			// 
			this.FrameCounterLabel.AutoSize = true;
			this.FrameCounterLabel.Location = new System.Drawing.Point(88, 29);
			this.FrameCounterLabel.Name = "FrameCounterLabel";
			this.FrameCounterLabel.Size = new System.Drawing.Size(44, 13);
			this.FrameCounterLabel.TabIndex = 9;
			this.FrameCounterLabel.Text = "Counter";
			// 
			// ImageAnalyzerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(193, 252);
			this.Controls.Add(this.Preview);
			this.Controls.Add(this.panel1);
			this.Name = "ImageAnalyzerForm";
			this.Text = "Go Image Analyzer";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.Preview)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button RecordButton;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Label timeLabel;
		private System.Windows.Forms.PictureBox Preview;
		private System.Windows.Forms.CheckBox AudioCheckBox;
		private System.Windows.Forms.Button FinishButton;
		private System.Windows.Forms.Label WindowTitle;
		private System.Windows.Forms.Label ProcessingTime;
		private System.Windows.Forms.Label FrameCounterLabel;

	}
}

