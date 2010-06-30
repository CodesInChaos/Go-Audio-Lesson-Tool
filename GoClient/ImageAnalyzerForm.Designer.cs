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
			this.timeLabel = new System.Windows.Forms.Label();
			this.allocStats = new System.Windows.Forms.Label();
			this.windowHandleBox = new System.Windows.Forms.TextBox();
			this.button2 = new System.Windows.Forms.Button();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.Preview = new System.Windows.Forms.PictureBox();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.Preview)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.timeLabel);
			this.panel1.Controls.Add(this.allocStats);
			this.panel1.Controls.Add(this.windowHandleBox);
			this.panel1.Controls.Add(this.button2);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 184);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(193, 60);
			this.panel1.TabIndex = 1;
			// 
			// timeLabel
			// 
			this.timeLabel.AutoSize = true;
			this.timeLabel.Location = new System.Drawing.Point(78, 3);
			this.timeLabel.Name = "timeLabel";
			this.timeLabel.Size = new System.Drawing.Size(35, 13);
			this.timeLabel.TabIndex = 4;
			this.timeLabel.Text = "label1";
			// 
			// allocStats
			// 
			this.allocStats.AutoSize = true;
			this.allocStats.Location = new System.Drawing.Point(9, 3);
			this.allocStats.Name = "allocStats";
			this.allocStats.Size = new System.Drawing.Size(35, 13);
			this.allocStats.TabIndex = 3;
			this.allocStats.Text = "label1";
			// 
			// windowHandleBox
			// 
			this.windowHandleBox.Location = new System.Drawing.Point(12, 19);
			this.windowHandleBox.Name = "windowHandleBox";
			this.windowHandleBox.Size = new System.Drawing.Size(100, 20);
			this.windowHandleBox.TabIndex = 2;
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(118, 19);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(42, 29);
			this.button2.TabIndex = 1;
			this.button2.Text = "button2";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
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
			this.Preview.Size = new System.Drawing.Size(193, 184);
			this.Preview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.Preview.TabIndex = 2;
			this.Preview.TabStop = false;
			// 
			// ImageAnalyzerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(193, 244);
			this.Controls.Add(this.Preview);
			this.Controls.Add(this.panel1);
			this.Name = "ImageAnalyzerForm";
			this.Text = "Go Image Analyzer";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.Preview)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TextBox windowHandleBox;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Label allocStats;
		private System.Windows.Forms.Label timeLabel;
		private System.Windows.Forms.PictureBox Preview;

	}
}

