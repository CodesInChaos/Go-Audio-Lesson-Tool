namespace GoClient
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.Field = new System.Windows.Forms.PictureBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.GameTreeBox = new System.Windows.Forms.GroupBox();
			this.GameBox = new System.Windows.Forms.GroupBox();
			this.PlayerToMove = new System.Windows.Forms.Label();
			this.MoveIndex = new System.Windows.Forms.Label();
			this.PlayBox = new System.Windows.Forms.GroupBox();
			this.PlayTimeLabel = new System.Windows.Forms.Label();
			this.PlayButton = new System.Windows.Forms.Button();
			this.PlayProgress = new System.Windows.Forms.TrackBar();
			this.RecordingBox = new System.Windows.Forms.GroupBox();
			this.FinishButton = new System.Windows.Forms.Button();
			this.RecordTimeLabel = new System.Windows.Forms.Label();
			this.RecordButton = new System.Windows.Forms.Button();
			this.RecordingState = new System.Windows.Forms.Label();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.LessonMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.CancelLessonMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.FinishLessonMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.CloseLessonMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.PauseLessonMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
			this.GameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ClearGameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.LoadGameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.AddGameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.SaveGameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ScreenshotMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MoveToolMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.PutStoneToolMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ScoreToolMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.TriangleToolMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.SquareToolMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.CircleToolMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.TextLabelToolMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.NumberLabelToolMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.SymbolLabelToolMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ActionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.PassActionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ResignActionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.navigationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.firstToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.lastToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.leftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.lastForkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
			this.moveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.movesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.secondsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.secondsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.rightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.nextForkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
			this.moveToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.movesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
			this.secondsToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.secondsToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.SaveAudioLessonDialog = new System.Windows.Forms.SaveFileDialog();
			((System.ComponentModel.ISupportInitialize)(this.Field)).BeginInit();
			this.panel1.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.GameBox.SuspendLayout();
			this.PlayBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PlayProgress)).BeginInit();
			this.RecordingBox.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// Field
			// 
			this.Field.BackColor = System.Drawing.Color.Green;
			this.Field.Dock = System.Windows.Forms.DockStyle.Left;
			this.Field.Location = new System.Drawing.Point(0, 24);
			this.Field.Name = "Field";
			this.Field.Size = new System.Drawing.Size(403, 422);
			this.Field.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.Field.TabIndex = 0;
			this.Field.TabStop = false;
			this.Field.Click += new System.EventHandler(this.Field_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.splitContainer1);
			this.panel1.Controls.Add(this.GameBox);
			this.panel1.Controls.Add(this.PlayBox);
			this.panel1.Controls.Add(this.RecordingBox);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(403, 24);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(229, 422);
			this.panel1.TabIndex = 1;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 200);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.richTextBox1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.GameTreeBox);
			this.splitContainer1.Size = new System.Drawing.Size(229, 222);
			this.splitContainer1.SplitterDistance = 84;
			this.splitContainer1.TabIndex = 16;
			// 
			// richTextBox1
			// 
			this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.richTextBox1.Location = new System.Drawing.Point(0, 0);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.Size = new System.Drawing.Size(229, 84);
			this.richTextBox1.TabIndex = 16;
			this.richTextBox1.Text = "";
			// 
			// GameTreeBox
			// 
			this.GameTreeBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GameTreeBox.Location = new System.Drawing.Point(0, 0);
			this.GameTreeBox.Name = "GameTreeBox";
			this.GameTreeBox.Size = new System.Drawing.Size(229, 134);
			this.GameTreeBox.TabIndex = 14;
			this.GameTreeBox.TabStop = false;
			this.GameTreeBox.Text = "GameTree";
			// 
			// GameBox
			// 
			this.GameBox.Controls.Add(this.PlayerToMove);
			this.GameBox.Controls.Add(this.MoveIndex);
			this.GameBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.GameBox.Location = new System.Drawing.Point(0, 160);
			this.GameBox.Name = "GameBox";
			this.GameBox.Size = new System.Drawing.Size(229, 40);
			this.GameBox.TabIndex = 15;
			this.GameBox.TabStop = false;
			this.GameBox.Text = "Game";
			// 
			// PlayerToMove
			// 
			this.PlayerToMove.AutoSize = true;
			this.PlayerToMove.Location = new System.Drawing.Point(84, 16);
			this.PlayerToMove.Name = "PlayerToMove";
			this.PlayerToMove.Size = new System.Drawing.Size(76, 13);
			this.PlayerToMove.TabIndex = 3;
			this.PlayerToMove.Text = "PlayerToMove";
			// 
			// MoveIndex
			// 
			this.MoveIndex.AutoSize = true;
			this.MoveIndex.Location = new System.Drawing.Point(6, 16);
			this.MoveIndex.Name = "MoveIndex";
			this.MoveIndex.Size = new System.Drawing.Size(60, 13);
			this.MoveIndex.TabIndex = 2;
			this.MoveIndex.Text = "MoveIndex";
			// 
			// PlayBox
			// 
			this.PlayBox.Controls.Add(this.PlayTimeLabel);
			this.PlayBox.Controls.Add(this.PlayButton);
			this.PlayBox.Controls.Add(this.PlayProgress);
			this.PlayBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.PlayBox.Location = new System.Drawing.Point(0, 75);
			this.PlayBox.Name = "PlayBox";
			this.PlayBox.Size = new System.Drawing.Size(229, 85);
			this.PlayBox.TabIndex = 14;
			this.PlayBox.TabStop = false;
			this.PlayBox.Text = "Play";
			// 
			// PlayTimeLabel
			// 
			this.PlayTimeLabel.AutoSize = true;
			this.PlayTimeLabel.Location = new System.Drawing.Point(49, 61);
			this.PlayTimeLabel.Name = "PlayTimeLabel";
			this.PlayTimeLabel.Size = new System.Drawing.Size(48, 13);
			this.PlayTimeLabel.TabIndex = 6;
			this.PlayTimeLabel.Text = "Progress";
			// 
			// PlayButton
			// 
			this.PlayButton.Location = new System.Drawing.Point(6, 23);
			this.PlayButton.Name = "PlayButton";
			this.PlayButton.Size = new System.Drawing.Size(37, 35);
			this.PlayButton.TabIndex = 8;
			this.PlayButton.Text = "Play";
			this.PlayButton.UseVisualStyleBackColor = true;
			this.PlayButton.Click += new System.EventHandler(this.PlayButton_Click);
			// 
			// PlayProgress
			// 
			this.PlayProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.PlayProgress.LargeChange = 60;
			this.PlayProgress.Location = new System.Drawing.Point(43, 23);
			this.PlayProgress.Maximum = 3100;
			this.PlayProgress.Name = "PlayProgress";
			this.PlayProgress.Size = new System.Drawing.Size(180, 45);
			this.PlayProgress.TabIndex = 7;
			this.PlayProgress.TickFrequency = 300;
			this.PlayProgress.Scroll += new System.EventHandler(this.PlayProgress_Scroll);
			// 
			// RecordingBox
			// 
			this.RecordingBox.Controls.Add(this.FinishButton);
			this.RecordingBox.Controls.Add(this.RecordTimeLabel);
			this.RecordingBox.Controls.Add(this.RecordButton);
			this.RecordingBox.Controls.Add(this.RecordingState);
			this.RecordingBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.RecordingBox.Location = new System.Drawing.Point(0, 0);
			this.RecordingBox.Margin = new System.Windows.Forms.Padding(8);
			this.RecordingBox.Name = "RecordingBox";
			this.RecordingBox.Size = new System.Drawing.Size(229, 75);
			this.RecordingBox.TabIndex = 13;
			this.RecordingBox.TabStop = false;
			this.RecordingBox.Text = "Recording";
			// 
			// FinishButton
			// 
			this.FinishButton.Location = new System.Drawing.Point(90, 41);
			this.FinishButton.Name = "FinishButton";
			this.FinishButton.Size = new System.Drawing.Size(93, 23);
			this.FinishButton.TabIndex = 3;
			this.FinishButton.Text = "Finish && Save";
			this.FinishButton.UseVisualStyleBackColor = true;
			this.FinishButton.Click += new System.EventHandler(this.FinishLessonMenuItem_Click);
			// 
			// RecordTimeLabel
			// 
			this.RecordTimeLabel.AutoSize = true;
			this.RecordTimeLabel.Location = new System.Drawing.Point(115, 16);
			this.RecordTimeLabel.Name = "RecordTimeLabel";
			this.RecordTimeLabel.Size = new System.Drawing.Size(35, 13);
			this.RecordTimeLabel.TabIndex = 2;
			this.RecordTimeLabel.Text = "label1";
			// 
			// RecordButton
			// 
			this.RecordButton.Location = new System.Drawing.Point(9, 41);
			this.RecordButton.Name = "RecordButton";
			this.RecordButton.Size = new System.Drawing.Size(75, 23);
			this.RecordButton.TabIndex = 1;
			this.RecordButton.Text = "Record";
			this.RecordButton.UseVisualStyleBackColor = true;
			this.RecordButton.Click += new System.EventHandler(this.RecordButton_Click);
			// 
			// RecordingState
			// 
			this.RecordingState.AutoSize = true;
			this.RecordingState.Location = new System.Drawing.Point(16, 16);
			this.RecordingState.Name = "RecordingState";
			this.RecordingState.Size = new System.Drawing.Size(81, 13);
			this.RecordingState.TabIndex = 0;
			this.RecordingState.Text = "RecordingState";
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LessonMenuItem,
            this.GameMenuItem,
            this.toolsToolStripMenuItem,
            this.ActionMenuItem,
            this.navigationToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(632, 24);
			this.menuStrip1.TabIndex = 2;
			this.menuStrip1.Text = "menuStrip1";
			this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
			// 
			// LessonMenuItem
			// 
			this.LessonMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CancelLessonMenuItem,
            this.FinishLessonMenuItem,
            this.CloseLessonMenuItem,
            this.PauseLessonMenuItem,
            this.toolStripMenuItem7});
			this.LessonMenuItem.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
			this.LessonMenuItem.MergeIndex = 0;
			this.LessonMenuItem.Name = "LessonMenuItem";
			this.LessonMenuItem.Size = new System.Drawing.Size(35, 20);
			this.LessonMenuItem.Text = "&File";
			// 
			// CancelLessonMenuItem
			// 
			this.CancelLessonMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
			this.CancelLessonMenuItem.MergeIndex = 4;
			this.CancelLessonMenuItem.Name = "CancelLessonMenuItem";
			this.CancelLessonMenuItem.Size = new System.Drawing.Size(176, 22);
			this.CancelLessonMenuItem.Text = "&Close && Cancel";
			this.CancelLessonMenuItem.Click += new System.EventHandler(this.CancelLessonMenuItem_Click);
			// 
			// FinishLessonMenuItem
			// 
			this.FinishLessonMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
			this.FinishLessonMenuItem.MergeIndex = 4;
			this.FinishLessonMenuItem.Name = "FinishLessonMenuItem";
			this.FinishLessonMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.FinishLessonMenuItem.Size = new System.Drawing.Size(176, 22);
			this.FinishLessonMenuItem.Text = "&Finish && Save";
			this.FinishLessonMenuItem.Click += new System.EventHandler(this.FinishLessonMenuItem_Click);
			// 
			// CloseLessonMenuItem
			// 
			this.CloseLessonMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
			this.CloseLessonMenuItem.MergeIndex = 4;
			this.CloseLessonMenuItem.Name = "CloseLessonMenuItem";
			this.CloseLessonMenuItem.Size = new System.Drawing.Size(176, 22);
			this.CloseLessonMenuItem.Text = "&Close";
			this.CloseLessonMenuItem.Click += new System.EventHandler(this.CloseLessonMenuItem_Click);
			// 
			// PauseLessonMenuItem
			// 
			this.PauseLessonMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
			this.PauseLessonMenuItem.MergeIndex = 4;
			this.PauseLessonMenuItem.Name = "PauseLessonMenuItem";
			this.PauseLessonMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
			this.PauseLessonMenuItem.Size = new System.Drawing.Size(176, 22);
			this.PauseLessonMenuItem.Text = "&Pause";
			// 
			// toolStripMenuItem7
			// 
			this.toolStripMenuItem7.MergeAction = System.Windows.Forms.MergeAction.Insert;
			this.toolStripMenuItem7.MergeIndex = 4;
			this.toolStripMenuItem7.Name = "toolStripMenuItem7";
			this.toolStripMenuItem7.Size = new System.Drawing.Size(173, 6);
			// 
			// GameMenuItem
			// 
			this.GameMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ClearGameMenuItem,
            this.LoadGameMenuItem,
            this.AddGameMenuItem,
            this.SaveGameMenuItem,
            this.ScreenshotMenuItem});
			this.GameMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
			this.GameMenuItem.MergeIndex = 1;
			this.GameMenuItem.Name = "GameMenuItem";
			this.GameMenuItem.Size = new System.Drawing.Size(46, 20);
			this.GameMenuItem.Text = "&Game";
			// 
			// ClearGameMenuItem
			// 
			this.ClearGameMenuItem.Name = "ClearGameMenuItem";
			this.ClearGameMenuItem.Size = new System.Drawing.Size(132, 22);
			this.ClearGameMenuItem.Text = "&Clear";
			// 
			// LoadGameMenuItem
			// 
			this.LoadGameMenuItem.Name = "LoadGameMenuItem";
			this.LoadGameMenuItem.Size = new System.Drawing.Size(132, 22);
			this.LoadGameMenuItem.Text = "&Load";
			// 
			// AddGameMenuItem
			// 
			this.AddGameMenuItem.Name = "AddGameMenuItem";
			this.AddGameMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
			this.AddGameMenuItem.Size = new System.Drawing.Size(132, 22);
			this.AddGameMenuItem.Text = "&Add";
			// 
			// SaveGameMenuItem
			// 
			this.SaveGameMenuItem.Name = "SaveGameMenuItem";
			this.SaveGameMenuItem.Size = new System.Drawing.Size(132, 22);
			this.SaveGameMenuItem.Text = "&Save";
			// 
			// ScreenshotMenuItem
			// 
			this.ScreenshotMenuItem.Name = "ScreenshotMenuItem";
			this.ScreenshotMenuItem.Size = new System.Drawing.Size(132, 22);
			this.ScreenshotMenuItem.Text = "Sc&reenshot";
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MoveToolMenuItem,
            this.PutStoneToolMenuItem,
            this.ScoreToolMenuItem,
            this.toolStripMenuItem1,
            this.TriangleToolMenuItem,
            this.SquareToolMenuItem,
            this.CircleToolMenuItem,
            this.toolStripMenuItem2,
            this.TextLabelToolMenuItem,
            this.NumberLabelToolMenuItem,
            this.SymbolLabelToolMenuItem});
			this.toolsToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
			this.toolsToolStripMenuItem.MergeIndex = 2;
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			this.toolsToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.toolsToolStripMenuItem.Text = "&Tools";
			// 
			// MoveToolMenuItem
			// 
			this.MoveToolMenuItem.Name = "MoveToolMenuItem";
			this.MoveToolMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
			this.MoveToolMenuItem.Size = new System.Drawing.Size(158, 22);
			this.MoveToolMenuItem.Tag = "";
			this.MoveToolMenuItem.Text = "&Move";
			this.MoveToolMenuItem.Click += new System.EventHandler(this.moveToolStripMenuItem_Click);
			// 
			// PutStoneToolMenuItem
			// 
			this.PutStoneToolMenuItem.Name = "PutStoneToolMenuItem";
			this.PutStoneToolMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
			this.PutStoneToolMenuItem.Size = new System.Drawing.Size(158, 22);
			this.PutStoneToolMenuItem.Tag = "";
			this.PutStoneToolMenuItem.Text = "Put &Stone";
			this.PutStoneToolMenuItem.Click += new System.EventHandler(this.putStoneToolStripMenuItem_Click);
			// 
			// ScoreToolMenuItem
			// 
			this.ScoreToolMenuItem.Enabled = false;
			this.ScoreToolMenuItem.Name = "ScoreToolMenuItem";
			this.ScoreToolMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
			this.ScoreToolMenuItem.Size = new System.Drawing.Size(158, 22);
			this.ScoreToolMenuItem.Tag = "";
			this.ScoreToolMenuItem.Text = "&Score";
			this.ScoreToolMenuItem.Click += new System.EventHandler(this.scoreToolStripMenuItem_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(155, 6);
			// 
			// TriangleToolMenuItem
			// 
			this.TriangleToolMenuItem.Name = "TriangleToolMenuItem";
			this.TriangleToolMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F4;
			this.TriangleToolMenuItem.Size = new System.Drawing.Size(158, 22);
			this.TriangleToolMenuItem.Text = "T&riangle";
			this.TriangleToolMenuItem.Click += new System.EventHandler(this.triangleToolStripMenuItem_Click);
			// 
			// SquareToolMenuItem
			// 
			this.SquareToolMenuItem.Name = "SquareToolMenuItem";
			this.SquareToolMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.SquareToolMenuItem.Size = new System.Drawing.Size(158, 22);
			this.SquareToolMenuItem.Text = "S&quare";
			this.SquareToolMenuItem.Click += new System.EventHandler(this.squareToolStripMenuItem_Click);
			// 
			// CircleToolMenuItem
			// 
			this.CircleToolMenuItem.Name = "CircleToolMenuItem";
			this.CircleToolMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F6;
			this.CircleToolMenuItem.Size = new System.Drawing.Size(158, 22);
			this.CircleToolMenuItem.Text = "&Circle";
			this.CircleToolMenuItem.Click += new System.EventHandler(this.circleToolStripMenuItem_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(155, 6);
			// 
			// TextLabelToolMenuItem
			// 
			this.TextLabelToolMenuItem.Name = "TextLabelToolMenuItem";
			this.TextLabelToolMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F7;
			this.TextLabelToolMenuItem.Size = new System.Drawing.Size(158, 22);
			this.TextLabelToolMenuItem.Text = "&Text Label";
			this.TextLabelToolMenuItem.Click += new System.EventHandler(this.textLabelToolStripMenuItem_Click);
			// 
			// NumberLabelToolMenuItem
			// 
			this.NumberLabelToolMenuItem.Name = "NumberLabelToolMenuItem";
			this.NumberLabelToolMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F8;
			this.NumberLabelToolMenuItem.Size = new System.Drawing.Size(158, 22);
			this.NumberLabelToolMenuItem.Text = "&Number Label";
			this.NumberLabelToolMenuItem.Click += new System.EventHandler(this.numberLabelToolStripMenuItem_Click);
			// 
			// SymbolLabelToolMenuItem
			// 
			this.SymbolLabelToolMenuItem.Name = "SymbolLabelToolMenuItem";
			this.SymbolLabelToolMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F9;
			this.SymbolLabelToolMenuItem.Size = new System.Drawing.Size(158, 22);
			this.SymbolLabelToolMenuItem.Text = "S&ymbol Label";
			this.SymbolLabelToolMenuItem.Click += new System.EventHandler(this.symbolLabelToolStripMenuItem_Click);
			// 
			// ActionMenuItem
			// 
			this.ActionMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PassActionMenuItem,
            this.ResignActionMenuItem});
			this.ActionMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
			this.ActionMenuItem.MergeIndex = 3;
			this.ActionMenuItem.Name = "ActionMenuItem";
			this.ActionMenuItem.Size = new System.Drawing.Size(49, 20);
			this.ActionMenuItem.Text = "&Action";
			// 
			// PassActionMenuItem
			// 
			this.PassActionMenuItem.Name = "PassActionMenuItem";
			this.PassActionMenuItem.Size = new System.Drawing.Size(106, 22);
			this.PassActionMenuItem.Text = "&Pass";
			// 
			// ResignActionMenuItem
			// 
			this.ResignActionMenuItem.Name = "ResignActionMenuItem";
			this.ResignActionMenuItem.Size = new System.Drawing.Size(106, 22);
			this.ResignActionMenuItem.Text = "&Resign";
			// 
			// navigationToolStripMenuItem
			// 
			this.navigationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.firstToolStripMenuItem,
            this.lastToolStripMenuItem,
            this.leftToolStripMenuItem,
            this.rightToolStripMenuItem});
			this.navigationToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
			this.navigationToolStripMenuItem.MergeIndex = 4;
			this.navigationToolStripMenuItem.Name = "navigationToolStripMenuItem";
			this.navigationToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
			this.navigationToolStripMenuItem.Text = "&Navigation";
			// 
			// firstToolStripMenuItem
			// 
			this.firstToolStripMenuItem.Name = "firstToolStripMenuItem";
			this.firstToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
			this.firstToolStripMenuItem.Text = "&First";
			// 
			// lastToolStripMenuItem
			// 
			this.lastToolStripMenuItem.Name = "lastToolStripMenuItem";
			this.lastToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
			this.lastToolStripMenuItem.Text = "&Last";
			// 
			// leftToolStripMenuItem
			// 
			this.leftToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lastForkToolStripMenuItem,
            this.toolStripMenuItem5,
            this.moveToolStripMenuItem1,
            this.movesToolStripMenuItem,
            this.toolStripMenuItem3,
            this.secondsToolStripMenuItem,
            this.secondsToolStripMenuItem1});
			this.leftToolStripMenuItem.Name = "leftToolStripMenuItem";
			this.leftToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
			this.leftToolStripMenuItem.Text = "Left";
			// 
			// lastForkToolStripMenuItem
			// 
			this.lastForkToolStripMenuItem.Name = "lastForkToolStripMenuItem";
			this.lastForkToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
			this.lastForkToolStripMenuItem.Text = "Last Fork";
			// 
			// toolStripMenuItem5
			// 
			this.toolStripMenuItem5.Name = "toolStripMenuItem5";
			this.toolStripMenuItem5.Size = new System.Drawing.Size(126, 6);
			// 
			// moveToolStripMenuItem1
			// 
			this.moveToolStripMenuItem1.Name = "moveToolStripMenuItem1";
			this.moveToolStripMenuItem1.Size = new System.Drawing.Size(129, 22);
			this.moveToolStripMenuItem1.Text = "1 Move";
			// 
			// movesToolStripMenuItem
			// 
			this.movesToolStripMenuItem.Name = "movesToolStripMenuItem";
			this.movesToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
			this.movesToolStripMenuItem.Text = "10 Moves";
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(126, 6);
			// 
			// secondsToolStripMenuItem
			// 
			this.secondsToolStripMenuItem.Name = "secondsToolStripMenuItem";
			this.secondsToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
			this.secondsToolStripMenuItem.Text = "10 Seconds";
			// 
			// secondsToolStripMenuItem1
			// 
			this.secondsToolStripMenuItem1.Name = "secondsToolStripMenuItem1";
			this.secondsToolStripMenuItem1.Size = new System.Drawing.Size(129, 22);
			this.secondsToolStripMenuItem1.Text = "60 Seconds";
			// 
			// rightToolStripMenuItem
			// 
			this.rightToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nextForkToolStripMenuItem,
            this.toolStripMenuItem6,
            this.moveToolStripMenuItem2,
            this.movesToolStripMenuItem1,
            this.toolStripMenuItem4,
            this.secondsToolStripMenuItem2,
            this.secondsToolStripMenuItem3});
			this.rightToolStripMenuItem.Name = "rightToolStripMenuItem";
			this.rightToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
			this.rightToolStripMenuItem.Text = "Right";
			// 
			// nextForkToolStripMenuItem
			// 
			this.nextForkToolStripMenuItem.Name = "nextForkToolStripMenuItem";
			this.nextForkToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
			this.nextForkToolStripMenuItem.Text = "Next Fork";
			// 
			// toolStripMenuItem6
			// 
			this.toolStripMenuItem6.Name = "toolStripMenuItem6";
			this.toolStripMenuItem6.Size = new System.Drawing.Size(126, 6);
			// 
			// moveToolStripMenuItem2
			// 
			this.moveToolStripMenuItem2.Name = "moveToolStripMenuItem2";
			this.moveToolStripMenuItem2.Size = new System.Drawing.Size(129, 22);
			this.moveToolStripMenuItem2.Text = "1 Move";
			// 
			// movesToolStripMenuItem1
			// 
			this.movesToolStripMenuItem1.Name = "movesToolStripMenuItem1";
			this.movesToolStripMenuItem1.Size = new System.Drawing.Size(129, 22);
			this.movesToolStripMenuItem1.Text = "10 Moves";
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Size = new System.Drawing.Size(126, 6);
			// 
			// secondsToolStripMenuItem2
			// 
			this.secondsToolStripMenuItem2.Name = "secondsToolStripMenuItem2";
			this.secondsToolStripMenuItem2.Size = new System.Drawing.Size(129, 22);
			this.secondsToolStripMenuItem2.Text = "10 Seconds";
			// 
			// secondsToolStripMenuItem3
			// 
			this.secondsToolStripMenuItem3.Name = "secondsToolStripMenuItem3";
			this.secondsToolStripMenuItem3.Size = new System.Drawing.Size(129, 22);
			this.secondsToolStripMenuItem3.Text = "60 Seconds";
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Interval = 20;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// SaveAudioLessonDialog
			// 
			this.SaveAudioLessonDialog.DefaultExt = "goal";
			this.SaveAudioLessonDialog.Filter = "GoAudioLessons (*.goal)|*.goal|All files|*.*";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(632, 446);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.Field);
			this.Controls.Add(this.menuStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.MinimizeBox = false;
			this.Name = "Form1";
			this.Text = "GO Audio Lesson Editor";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.Field)).EndInit();
			this.panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.GameBox.ResumeLayout(false);
			this.GameBox.PerformLayout();
			this.PlayBox.ResumeLayout(false);
			this.PlayBox.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.PlayProgress)).EndInit();
			this.RecordingBox.ResumeLayout(false);
			this.RecordingBox.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox Field;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem LessonMenuItem;
		private System.Windows.Forms.ToolStripMenuItem PauseLessonMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem MoveToolMenuItem;
		private System.Windows.Forms.ToolStripMenuItem PutStoneToolMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ScoreToolMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem TriangleToolMenuItem;
		private System.Windows.Forms.ToolStripMenuItem SquareToolMenuItem;
		private System.Windows.Forms.ToolStripMenuItem CircleToolMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem TextLabelToolMenuItem;
		private System.Windows.Forms.ToolStripMenuItem NumberLabelToolMenuItem;
		private System.Windows.Forms.ToolStripMenuItem SymbolLabelToolMenuItem;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.ToolStripMenuItem ActionMenuItem;
		private System.Windows.Forms.ToolStripMenuItem FinishLessonMenuItem;
		private System.Windows.Forms.ToolStripMenuItem PassActionMenuItem;
		private System.Windows.Forms.ToolStripMenuItem GameMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ClearGameMenuItem;
		private System.Windows.Forms.ToolStripMenuItem LoadGameMenuItem;
		private System.Windows.Forms.ToolStripMenuItem AddGameMenuItem;
		private System.Windows.Forms.ToolStripMenuItem SaveGameMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ScreenshotMenuItem;
		private System.Windows.Forms.ToolStripMenuItem navigationToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem firstToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem lastToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem leftToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem lastForkToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
		private System.Windows.Forms.ToolStripMenuItem moveToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem movesToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem secondsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem secondsToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem rightToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem nextForkToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
		private System.Windows.Forms.ToolStripMenuItem moveToolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem movesToolStripMenuItem1;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
		private System.Windows.Forms.ToolStripMenuItem secondsToolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem secondsToolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem ResignActionMenuItem;
		private System.Windows.Forms.ToolStripMenuItem CloseLessonMenuItem;
		private System.Windows.Forms.ToolStripMenuItem CancelLessonMenuItem;
		private System.Windows.Forms.GroupBox GameBox;
		private System.Windows.Forms.Label PlayerToMove;
		private System.Windows.Forms.Label MoveIndex;
		private System.Windows.Forms.GroupBox PlayBox;
		private System.Windows.Forms.Label PlayTimeLabel;
		private System.Windows.Forms.Button PlayButton;
		private System.Windows.Forms.TrackBar PlayProgress;
		private System.Windows.Forms.GroupBox RecordingBox;
		private System.Windows.Forms.Label RecordTimeLabel;
		private System.Windows.Forms.Button RecordButton;
		private System.Windows.Forms.Label RecordingState;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.RichTextBox richTextBox1;
		private System.Windows.Forms.GroupBox GameTreeBox;
		private System.Windows.Forms.Button FinishButton;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
		private System.Windows.Forms.SaveFileDialog SaveAudioLessonDialog;
	}
}

