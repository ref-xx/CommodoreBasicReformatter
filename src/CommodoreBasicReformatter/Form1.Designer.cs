namespace CommodoreBasicReformatter
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
            button1 = new System.Windows.Forms.Button();
            inputBox = new System.Windows.Forms.TextBox();
            outputBox = new System.Windows.Forms.TextBox();
            chkSplit = new System.Windows.Forms.CheckBox();
            chkAddExplanations = new System.Windows.Forms.CheckBox();
            button2 = new System.Windows.Forms.Button();
            button3 = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(300, 12);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(94, 23);
            button1.TabIndex = 0;
            button1.Text = "Clean Up";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // inputBox
            // 
            inputBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            inputBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            inputBox.Location = new System.Drawing.Point(12, 41);
            inputBox.Multiline = true;
            inputBox.Name = "inputBox";
            inputBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            inputBox.Size = new System.Drawing.Size(382, 397);
            inputBox.TabIndex = 1;
            // 
            // outputBox
            // 
            outputBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            outputBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            outputBox.Location = new System.Drawing.Point(400, 41);
            outputBox.Multiline = true;
            outputBox.Name = "outputBox";
            outputBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            outputBox.Size = new System.Drawing.Size(388, 397);
            outputBox.TabIndex = 2;
            // 
            // chkSplit
            // 
            chkSplit.AutoSize = true;
            chkSplit.Location = new System.Drawing.Point(147, 16);
            chkSplit.Name = "chkSplit";
            chkSplit.Size = new System.Drawing.Size(49, 19);
            chkSplit.TabIndex = 3;
            chkSplit.Text = "Split";
            chkSplit.UseVisualStyleBackColor = true;
            // 
            // chkAddExplanations
            // 
            chkAddExplanations.AutoSize = true;
            chkAddExplanations.Location = new System.Drawing.Point(202, 16);
            chkAddExplanations.Name = "chkAddExplanations";
            chkAddExplanations.Size = new System.Drawing.Size(92, 19);
            chkAddExplanations.TabIndex = 4;
            chkAddExplanations.Text = "Explanations";
            chkAddExplanations.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(12, 12);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(75, 23);
            button2.TabIndex = 5;
            button2.Text = "Open";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            button3.Location = new System.Drawing.Point(713, 12);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(75, 23);
            button3.TabIndex = 6;
            button3.Text = "Save";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(chkAddExplanations);
            Controls.Add(chkSplit);
            Controls.Add(outputBox);
            Controls.Add(inputBox);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Commodore Basic Reformatter";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox inputBox;
        private System.Windows.Forms.TextBox outputBox;
        private System.Windows.Forms.CheckBox chkSplit;
        private System.Windows.Forms.CheckBox chkAddExplanations;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}