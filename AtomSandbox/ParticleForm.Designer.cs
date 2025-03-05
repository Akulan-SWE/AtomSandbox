namespace AtomSandbox
{
    partial class ParticleForm
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
            particleNameTextBox = new TextBox();
            nameLabel = new Label();
            radiusLabel = new Label();
            massLabel = new Label();
            label2 = new Label();
            lifeTimeLabel = new Label();
            tailLengthNumericUpDown = new NumericUpDown();
            TailLengthLabel = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            cancelButton = new Button();
            addButton = new Button();
            colorControl = new WindowsFormsTools.InputControls.ColorControl();
            radiusNumericTextBox = new WindowsFormsTools.InputControls.PreciseNumericTextBox();
            massNumericTextBox = new WindowsFormsTools.InputControls.PreciseNumericTextBox();
            chargeNumericTextBox = new WindowsFormsTools.InputControls.PreciseNumericTextBox();
            lifeTimeNumericTextBox = new WindowsFormsTools.InputControls.PreciseNumericTextBox();
            ((System.ComponentModel.ISupportInitialize)tailLengthNumericUpDown).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // particleNameTextBox
            // 
            particleNameTextBox.Location = new Point(102, 7);
            particleNameTextBox.Margin = new Padding(4, 3, 4, 3);
            particleNameTextBox.Name = "particleNameTextBox";
            particleNameTextBox.Size = new Size(174, 23);
            particleNameTextBox.TabIndex = 0;
            particleNameTextBox.Text = "New particle";
            particleNameTextBox.TextAlign = HorizontalAlignment.Right;
            // 
            // nameLabel
            // 
            nameLabel.AutoSize = true;
            nameLabel.Location = new Point(12, 10);
            nameLabel.Margin = new Padding(4, 0, 4, 0);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new Size(79, 15);
            nameLabel.TabIndex = 1;
            nameLabel.Text = "Particle name";
            // 
            // radiusLabel
            // 
            radiusLabel.AutoSize = true;
            radiusLabel.Location = new Point(12, 39);
            radiusLabel.Margin = new Padding(4, 0, 4, 0);
            radiusLabel.Name = "radiusLabel";
            radiusLabel.Size = new Size(42, 15);
            radiusLabel.TabIndex = 5;
            radiusLabel.Text = "Radius";
            // 
            // massLabel
            // 
            massLabel.AutoSize = true;
            massLabel.Location = new Point(12, 69);
            massLabel.Margin = new Padding(4, 0, 4, 0);
            massLabel.Name = "massLabel";
            massLabel.Size = new Size(34, 15);
            massLabel.TabIndex = 7;
            massLabel.Text = "Mass";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 99);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(45, 15);
            label2.TabIndex = 9;
            label2.Text = "Charge";
            // 
            // lifeTimeLabel
            // 
            lifeTimeLabel.AutoSize = true;
            lifeTimeLabel.Location = new Point(12, 129);
            lifeTimeLabel.Margin = new Padding(4, 0, 4, 0);
            lifeTimeLabel.Name = "lifeTimeLabel";
            lifeTimeLabel.Size = new Size(52, 15);
            lifeTimeLabel.TabIndex = 11;
            lifeTimeLabel.Text = "LifeTime";
            // 
            // tailLengthNumericUpDown
            // 
            tailLengthNumericUpDown.Location = new Point(102, 157);
            tailLengthNumericUpDown.Margin = new Padding(4, 3, 4, 3);
            tailLengthNumericUpDown.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
            tailLengthNumericUpDown.Name = "tailLengthNumericUpDown";
            tailLengthNumericUpDown.Size = new Size(175, 23);
            tailLengthNumericUpDown.TabIndex = 14;
            tailLengthNumericUpDown.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // TailLengthLabel
            // 
            TailLengthLabel.AutoSize = true;
            TailLengthLabel.Location = new Point(12, 159);
            TailLengthLabel.Margin = new Padding(4, 0, 4, 0);
            TailLengthLabel.Name = "TailLengthLabel";
            TailLengthLabel.Size = new Size(61, 15);
            TailLengthLabel.TabIndex = 13;
            TailLengthLabel.Text = "Tail length";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(cancelButton, 1, 0);
            tableLayoutPanel1.Controls.Add(addButton, 0, 0);
            tableLayoutPanel1.Location = new Point(15, 220);
            tableLayoutPanel1.Margin = new Padding(4, 3, 4, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(261, 38);
            tableLayoutPanel1.TabIndex = 16;
            // 
            // cancelButton
            // 
            cancelButton.Dock = DockStyle.Fill;
            cancelButton.Location = new Point(132, 2);
            cancelButton.Margin = new Padding(2);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(127, 34);
            cancelButton.TabIndex = 1;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // addButton
            // 
            addButton.Dock = DockStyle.Fill;
            addButton.Location = new Point(2, 2);
            addButton.Margin = new Padding(2);
            addButton.Name = "addButton";
            addButton.Size = new Size(126, 34);
            addButton.TabIndex = 0;
            addButton.Text = "Add";
            addButton.UseVisualStyleBackColor = true;
            addButton.Click += addButton_Click;
            // 
            // colorControl
            // 
            colorControl.ColorPanelWidth = 150;
            colorControl.Header = "Color";
            colorControl.Location = new Point(15, 187);
            colorControl.Margin = new Padding(5, 3, 5, 3);
            colorControl.Name = "colorControl";
            colorControl.Size = new Size(261, 27);
            colorControl.TabIndex = 15;
            colorControl.Value = Color.DeepSkyBlue;
            // 
            // radiusNumericTextBox
            // 
            radiusNumericTextBox.Location = new Point(102, 36);
            radiusNumericTextBox.Margin = new Padding(5, 3, 5, 3);
            radiusNumericTextBox.MaximumSize = new Size(350, 23);
            radiusNumericTextBox.MinimumSize = new Size(35, 23);
            radiusNumericTextBox.Name = "radiusNumericTextBox";
            radiusNumericTextBox.Size = new Size(175, 23);
            radiusNumericTextBox.TabIndex = 21;
            radiusNumericTextBox.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // massNumericTextBox
            // 
            massNumericTextBox.Location = new Point(102, 66);
            massNumericTextBox.Margin = new Padding(5, 3, 5, 3);
            massNumericTextBox.MaximumSize = new Size(350, 23);
            massNumericTextBox.MinimumSize = new Size(35, 23);
            massNumericTextBox.Name = "massNumericTextBox";
            massNumericTextBox.Size = new Size(175, 23);
            massNumericTextBox.TabIndex = 22;
            massNumericTextBox.Value = new decimal(new int[] { 30, 0, 0, 0 });
            // 
            // chargeNumericTextBox
            // 
            chargeNumericTextBox.Location = new Point(102, 96);
            chargeNumericTextBox.Margin = new Padding(5, 3, 5, 3);
            chargeNumericTextBox.MaximumSize = new Size(350, 23);
            chargeNumericTextBox.MinimumSize = new Size(35, 23);
            chargeNumericTextBox.Name = "chargeNumericTextBox";
            chargeNumericTextBox.Size = new Size(175, 23);
            chargeNumericTextBox.TabIndex = 23;
            chargeNumericTextBox.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // lifeTimeNumericTextBox
            // 
            lifeTimeNumericTextBox.Location = new Point(102, 126);
            lifeTimeNumericTextBox.Margin = new Padding(5, 3, 5, 3);
            lifeTimeNumericTextBox.MaximumSize = new Size(350, 23);
            lifeTimeNumericTextBox.MinimumSize = new Size(35, 23);
            lifeTimeNumericTextBox.Name = "lifeTimeNumericTextBox";
            lifeTimeNumericTextBox.Size = new Size(175, 23);
            lifeTimeNumericTextBox.TabIndex = 24;
            lifeTimeNumericTextBox.Value = new decimal(new int[] { 1, 0, 0, int.MinValue });
            // 
            // ParticleForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(289, 272);
            Controls.Add(lifeTimeNumericTextBox);
            Controls.Add(chargeNumericTextBox);
            Controls.Add(massNumericTextBox);
            Controls.Add(radiusNumericTextBox);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(colorControl);
            Controls.Add(tailLengthNumericUpDown);
            Controls.Add(TailLengthLabel);
            Controls.Add(lifeTimeLabel);
            Controls.Add(label2);
            Controls.Add(massLabel);
            Controls.Add(radiusLabel);
            Controls.Add(nameLabel);
            Controls.Add(particleNameTextBox);
            Margin = new Padding(4, 3, 4, 3);
            Name = "ParticleForm";
            Text = "New particle";
            Load += NewParticleForm_Load;
            ((System.ComponentModel.ISupportInitialize)tailLengthNumericUpDown).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox particleNameTextBox;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label radiusLabel;
        private System.Windows.Forms.Label massLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lifeTimeLabel;
        private System.Windows.Forms.NumericUpDown tailLengthNumericUpDown;
        private System.Windows.Forms.Label TailLengthLabel;
        private WindowsFormsTools.InputControls.ColorControl colorControl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button addButton;
        private WindowsFormsTools.InputControls.PreciseNumericTextBox radiusNumericTextBox;
        private WindowsFormsTools.InputControls.PreciseNumericTextBox massNumericTextBox;
        private WindowsFormsTools.InputControls.PreciseNumericTextBox chargeNumericTextBox;
        private WindowsFormsTools.InputControls.PreciseNumericTextBox lifeTimeNumericTextBox;
    }
}