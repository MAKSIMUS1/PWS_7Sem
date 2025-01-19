namespace SimplexClient
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxS1 = new System.Windows.Forms.TextBox();
            this.textBoxS2 = new System.Windows.Forms.TextBox();
            this.textBoxK1 = new System.Windows.Forms.TextBox();
            this.textBoxK2 = new System.Windows.Forms.TextBox();
            this.textBoxF1 = new System.Windows.Forms.TextBox();
            this.textBoxF2 = new System.Windows.Forms.TextBox();
            this.CalculateButton = new System.Windows.Forms.Button();
            this.resultLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxS1
            // 
            this.textBoxS1.Location = new System.Drawing.Point(12, 16);
            this.textBoxS1.Name = "textBoxS1";
            this.textBoxS1.Size = new System.Drawing.Size(100, 22);
            this.textBoxS1.TabIndex = 0;
            // 
            // textBoxS2
            // 
            this.textBoxS2.Location = new System.Drawing.Point(148, 16);
            this.textBoxS2.Name = "textBoxS2";
            this.textBoxS2.Size = new System.Drawing.Size(100, 22);
            this.textBoxS2.TabIndex = 1;
            // 
            // textBoxK1
            // 
            this.textBoxK1.Location = new System.Drawing.Point(12, 98);
            this.textBoxK1.Name = "textBoxK1";
            this.textBoxK1.Size = new System.Drawing.Size(100, 22);
            this.textBoxK1.TabIndex = 2;
            // 
            // textBoxK2
            // 
            this.textBoxK2.Location = new System.Drawing.Point(148, 98);
            this.textBoxK2.Name = "textBoxK2";
            this.textBoxK2.Size = new System.Drawing.Size(100, 22);
            this.textBoxK2.TabIndex = 3;
            // 
            // textBoxF1
            // 
            this.textBoxF1.Location = new System.Drawing.Point(12, 185);
            this.textBoxF1.Name = "textBoxF1";
            this.textBoxF1.Size = new System.Drawing.Size(100, 22);
            this.textBoxF1.TabIndex = 4;
            // 
            // textBoxF2
            // 
            this.textBoxF2.Location = new System.Drawing.Point(148, 185);
            this.textBoxF2.Name = "textBoxF2";
            this.textBoxF2.Size = new System.Drawing.Size(100, 22);
            this.textBoxF2.TabIndex = 5;
            // 
            // CalculateButton
            // 
            this.CalculateButton.Location = new System.Drawing.Point(12, 230);
            this.CalculateButton.Name = "CalculateButton";
            this.CalculateButton.Size = new System.Drawing.Size(100, 39);
            this.CalculateButton.TabIndex = 6;
            this.CalculateButton.Text = "Calculate";
            this.CalculateButton.UseVisualStyleBackColor = true;
            this.CalculateButton.Click += new System.EventHandler(this.CalculateButton_Click);
            // 
            // resultLabel
            // 
            this.resultLabel.AutoSize = true;
            this.resultLabel.Location = new System.Drawing.Point(145, 230);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Size = new System.Drawing.Size(45, 16);
            this.resultLabel.TabIndex = 7;
            this.resultLabel.Text = "Result";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 316);
            this.Controls.Add(this.resultLabel);
            this.Controls.Add(this.CalculateButton);
            this.Controls.Add(this.textBoxF2);
            this.Controls.Add(this.textBoxF1);
            this.Controls.Add(this.textBoxK2);
            this.Controls.Add(this.textBoxK1);
            this.Controls.Add(this.textBoxS2);
            this.Controls.Add(this.textBoxS1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxS1;
        private System.Windows.Forms.TextBox textBoxS2;
        private System.Windows.Forms.TextBox textBoxK1;
        private System.Windows.Forms.TextBox textBoxK2;
        private System.Windows.Forms.TextBox textBoxF1;
        private System.Windows.Forms.TextBox textBoxF2;
        private System.Windows.Forms.Button CalculateButton;
        private System.Windows.Forms.Label resultLabel;
    }
}

