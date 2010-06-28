namespace ImageAnalyzer
{
	partial class Form1
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
			this.allocStats = new System.Windows.Forms.Label();
			this.windowHandleBox = new System.Windows.Forms.TextBox();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.OriginalPB = new System.Windows.Forms.PictureBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.timeLabel = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.OriginalPB)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.timeLabel);
			this.panel1.Controls.Add(this.allocStats);
			this.panel1.Controls.Add(this.windowHandleBox);
			this.panel1.Controls.Add(this.button2);
			this.panel1.Controls.Add(this.button1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 206);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(292, 60);
			this.panel1.TabIndex = 1;
			// 
			// allocStats
			// 
			this.allocStats.AutoSize = true;
			this.allocStats.Location = new System.Drawing.Point(29, 10);
			this.allocStats.Name = "allocStats";
			this.allocStats.Size = new System.Drawing.Size(35, 13);
			this.allocStats.TabIndex = 3;
			this.allocStats.Text = "label1";
			// 
			// windowHandleBox
			// 
			this.windowHandleBox.Location = new System.Drawing.Point(137, 24);
			this.windowHandleBox.Name = "windowHandleBox";
			this.windowHandleBox.Size = new System.Drawing.Size(100, 20);
			this.windowHandleBox.TabIndex = 2;
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(243, 19);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(42, 29);
			this.button2.TabIndex = 1;
			this.button2.Text = "button2";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(12, 28);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(292, 206);
			this.tabControl1.TabIndex = 2;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.OriginalPB);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(284, 180);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "tabPage1";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// OriginalPB
			// 
			this.OriginalPB.Dock = System.Windows.Forms.DockStyle.Fill;
			this.OriginalPB.Image = global::ImageAnalyzer.Properties.Resources.bat;
			this.OriginalPB.Location = new System.Drawing.Point(3, 3);
			this.OriginalPB.Name = "OriginalPB";
			this.OriginalPB.Size = new System.Drawing.Size(278, 174);
			this.OriginalPB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.OriginalPB.TabIndex = 0;
			this.OriginalPB.TabStop = false;
			// 
			// tabPage2
			// 
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(284, 180);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "tabPage2";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// timeLabel
			// 
			this.timeLabel.AutoSize = true;
			this.timeLabel.Location = new System.Drawing.Point(145, 3);
			this.timeLabel.Name = "timeLabel";
			this.timeLabel.Size = new System.Drawing.Size(35, 13);
			this.timeLabel.TabIndex = 4;
			this.timeLabel.Text = "label1";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.panel1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.OriginalPB)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.PictureBox OriginalPB;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TextBox windowHandleBox;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Label allocStats;
		private System.Windows.Forms.Label timeLabel;

	}
}

