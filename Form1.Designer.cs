namespace FileExplorer
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

        public class TreeViewEx : TreeView
        {
            private const int TVM_SETEXTENDEDSTYLE = 0x1100 + 44;
            private const int TVS_EX_DOUBLEBUFFER = 0x0004;

            [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
            private static extern IntPtr SendMessage(IntPtr HWnd, int Msg, IntPtr Wp, IntPtr Lp);

            protected override void OnHandleCreated(EventArgs e)
            {
                SendMessage(this.Handle, TVM_SETEXTENDEDSTYLE, (IntPtr)TVS_EX_DOUBLEBUFFER, (IntPtr)TVS_EX_DOUBLEBUFFER);
                base.OnHandleCreated(e);
            }
        }

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            startButton = new Button();
            startDirectoryTextBox = new TextBox();
            filePatternTextBox = new TextBox();
            stopButton = new Button();
            fileSystemWatcher1 = new FileSystemWatcher();
            statusLabel = new Label();
            filesTreeView = new TreeViewEx();
            label1 = new Label();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).BeginInit();
            SuspendLayout();
            // 
            // startButton
            // 
            startButton.Location = new Point(367, 76);
            startButton.Name = "startButton";
            startButton.Size = new Size(94, 29);
            startButton.TabIndex = 0;
            startButton.Text = "Поиск";
            startButton.UseVisualStyleBackColor = true;
            startButton.Click += startButton_Click;
            // 
            // startDirectoryTextBox
            // 
            startDirectoryTextBox.Location = new Point(186, 10);
            startDirectoryTextBox.Name = "startDirectoryTextBox";
            startDirectoryTextBox.Size = new Size(400, 27);
            startDirectoryTextBox.TabIndex = 1;
            // 
            // filePatternTextBox
            // 
            filePatternTextBox.Location = new Point(186, 43);
            filePatternTextBox.Name = "filePatternTextBox";
            filePatternTextBox.Size = new Size(400, 27);
            filePatternTextBox.TabIndex = 2;
            // 
            // stopButton
            // 
            stopButton.Location = new Point(476, 76);
            stopButton.Name = "stopButton";
            stopButton.Size = new Size(94, 29);
            stopButton.TabIndex = 3;
            stopButton.Text = "Стоп";
            stopButton.UseVisualStyleBackColor = true;
            stopButton.Click += stopButton_Click;
            // 
            // fileSystemWatcher1
            // 
            fileSystemWatcher1.EnableRaisingEvents = true;
            fileSystemWatcher1.SynchronizingObject = this;
            // 
            // statusLabel
            // 
            statusLabel.AutoEllipsis = true;
            statusLabel.Location = new Point(12, 109);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(586, 54);
            statusLabel.TabIndex = 4;
            statusLabel.Text = "label1";
            // 
            // filesTreeView
            // 
            filesTreeView.Location = new Point(12, 174);
            filesTreeView.Name = "filesTreeView";
            filesTreeView.Size = new Size(586, 406);
            filesTreeView.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(84, 13);
            label1.Name = "label1";
            label1.Size = new Size(96, 20);
            label1.TabIndex = 6;
            label1.Text = "Директория:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(17, 46);
            label2.Name = "label2";
            label2.Size = new Size(163, 20);
            label2.TabIndex = 7;
            label2.Text = "Шаблон имени файла";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(610, 592);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(filesTreeView);
            Controls.Add(statusLabel);
            Controls.Add(stopButton);
            Controls.Add(filePatternTextBox);
            Controls.Add(startDirectoryTextBox);
            Controls.Add(startButton);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button startButton;
        private TextBox startDirectoryTextBox;
        private TextBox filePatternTextBox;
        private Button stopButton;
        private FileSystemWatcher fileSystemWatcher1;
        private Label statusLabel;
        private Label label2;
        private Label label1;
        private TreeViewEx filesTreeView;
    }
}