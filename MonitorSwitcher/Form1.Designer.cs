namespace MonitorSwitcher
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
            outputListBox = new ListBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            loadConfigurationbutton = new Button();
            saveConfigurationButton = new Button();
            gameListBox = new ListBox();
            removeGameButton = new Button();
            addGameButton = new Button();
            gameNameTextBox = new TextBox();
            label1 = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            tableLayoutPanel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // outputListBox
            // 
            outputListBox.FormattingEnabled = true;
            outputListBox.Location = new Point(471, 3);
            outputListBox.Name = "outputListBox";
            outputListBox.Size = new Size(463, 536);
            outputListBox.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(flowLayoutPanel1, 0, 0);
            tableLayoutPanel1.Controls.Add(outputListBox, 1, 0);
            tableLayoutPanel1.Location = new Point(171, 44);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(937, 542);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // loadConfigurationbutton
            // 
            loadConfigurationbutton.Location = new Point(3, 471);
            loadConfigurationbutton.Name = "loadConfigurationbutton";
            loadConfigurationbutton.Size = new Size(125, 29);
            loadConfigurationbutton.TabIndex = 5;
            loadConfigurationbutton.Text = "Load Games";
            loadConfigurationbutton.UseVisualStyleBackColor = true;
            loadConfigurationbutton.Click += button2_Click;
            // 
            // saveConfigurationButton
            // 
            saveConfigurationButton.Location = new Point(3, 436);
            saveConfigurationButton.Name = "saveConfigurationButton";
            saveConfigurationButton.Size = new Size(140, 29);
            saveConfigurationButton.TabIndex = 4;
            saveConfigurationButton.Text = "Save Games";
            saveConfigurationButton.UseVisualStyleBackColor = true;
            // 
            // gameListBox
            // 
            gameListBox.Dock = DockStyle.Fill;
            gameListBox.FormattingEnabled = true;
            gameListBox.Location = new Point(3, 126);
            gameListBox.Name = "gameListBox";
            gameListBox.Size = new Size(445, 304);
            gameListBox.TabIndex = 3;
            // 
            // removeGameButton
            // 
            removeGameButton.Location = new Point(3, 91);
            removeGameButton.Name = "removeGameButton";
            removeGameButton.Size = new Size(145, 29);
            removeGameButton.TabIndex = 2;
            removeGameButton.Text = "Remove Selected";
            removeGameButton.UseVisualStyleBackColor = true;
            // 
            // addGameButton
            // 
            addGameButton.Location = new Point(3, 56);
            addGameButton.Name = "addGameButton";
            addGameButton.Size = new Size(94, 29);
            addGameButton.TabIndex = 1;
            addGameButton.Text = "Add Games";
            addGameButton.UseVisualStyleBackColor = true;
            addGameButton.Click += button1_Click;
            // 
            // gameNameTextBox
            // 
            gameNameTextBox.Location = new Point(3, 23);
            gameNameTextBox.Name = "gameNameTextBox";
            gameNameTextBox.Size = new Size(445, 27);
            gameNameTextBox.TabIndex = 0;
            gameNameTextBox.TextChanged += gameNameTextBox_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(148, 20);
            label1.TabIndex = 6;
            label1.Text = "Game Process Name:";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            flowLayoutPanel1.Controls.Add(label1);
            flowLayoutPanel1.Controls.Add(gameNameTextBox);
            flowLayoutPanel1.Controls.Add(addGameButton);
            flowLayoutPanel1.Controls.Add(removeGameButton);
            flowLayoutPanel1.Controls.Add(gameListBox);
            flowLayoutPanel1.Controls.Add(saveConfigurationButton);
            flowLayoutPanel1.Controls.Add(loadConfigurationbutton);
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(3, 3);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(462, 536);
            flowLayoutPanel1.TabIndex = 1;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1210, 647);
            Controls.Add(tableLayoutPanel1);
            Name = "Form1";
            Text = "Form1";
            tableLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private ListBox outputListBox;
        private TableLayoutPanel tableLayoutPanel1;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label label1;
        private TextBox gameNameTextBox;
        private Button addGameButton;
        private Button removeGameButton;
        private ListBox gameListBox;
        private Button saveConfigurationButton;
        private Button loadConfigurationbutton;
    }
}