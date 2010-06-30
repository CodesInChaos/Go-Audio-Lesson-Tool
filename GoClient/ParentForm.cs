using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CommonGui.ViewModels;
using System.IO;

namespace GoClient
{
	public partial class ParentForm : Form
	{
		private int childFormNumber = 0;

		public ParentForm()
		{
			InitializeComponent();
			foreach (string arg in Environment.GetCommandLineArgs().Skip(1))//Not the exe-filename
			{
				OpenFile(arg);
			}
		}

		private void ShowAsMDI(Form childForm)
		{
			childForm.MdiParent = this;
			childForm.Text = "Window " + childFormNumber++;
			childForm.Show();
			//Hack to force childform to apply the layout correctly, doesn't work maximized
			Width -= 1;
			Width += 1;
		}

		internal void OpenFile(string filename)
		{
			ViewModel view;
			if (Path.GetExtension(OpenAudioLessonDialog.FileName) == ".GoReplay")
			{
				view = ViewModel.PlayReplay(filename);
			}
			else
			{
				view = ViewModel.PlayLesson(filename);
			}
			GameForm childForm = new GameForm(view);
			ShowAsMDI(childForm);
		}

		private void OpenFile(object sender, EventArgs e)
		{
			if (OpenAudioLessonDialog.ShowDialog() == DialogResult.OK)
			{
				OpenFile(OpenAudioLessonDialog.FileName);
			}
		}

		private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void CutToolStripMenuItem_Click(object sender, EventArgs e)
		{
		}

		private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
		{
		}

		private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
		{
		}

		private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
		{
			toolStrip.Visible = toolBarToolStripMenuItem.Checked;
		}

		private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
		{
			statusStrip.Visible = statusBarToolStripMenuItem.Checked;
		}

		private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.Cascade);
		}

		private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.TileVertical);
		}

		private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.TileHorizontal);
		}

		private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.ArrangeIcons);
		}

		private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (Form childForm in MdiChildren)
			{
				childForm.Close();
			}
		}

		private void replayToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ShowAsMDI(new GameForm(ViewModel.CreateReplay()));
		}

		private void NewLessonToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ShowAsMDI(new GameForm(ViewModel.CreateLesson()));
		}

		private void externalRecorderToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ImageAnalyzerForm form = new ImageAnalyzerForm();
			form.Show();
		}
	}
}
