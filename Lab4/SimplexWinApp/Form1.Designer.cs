namespace SimplexWinApp
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
            this.btnCalculateSum = new System.Windows.Forms.Button();
            this.txtK2 = new System.Windows.Forms.TextBox();
            this.txtK1 = new System.Windows.Forms.TextBox();
            this.lblResult = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCalculateSum
            // 
            this.btnCalculateSum.Location = new System.Drawing.Point(12, 176);
            this.btnCalculateSum.Name = "btnCalculateSum";
            this.btnCalculateSum.Size = new System.Drawing.Size(63, 29);
            this.btnCalculateSum.TabIndex = 0;
            this.btnCalculateSum.Text = "Calc";
            this.btnCalculateSum.UseVisualStyleBackColor = true;
            this.btnCalculateSum.Click += new System.EventHandler(this.btnCalculateSum_Click);
            // 
            // txtK2
            // 
            this.txtK2.Location = new System.Drawing.Point(156, 72);
            this.txtK2.Name = "txtK2";
            this.txtK2.Size = new System.Drawing.Size(100, 22);
            this.txtK2.TabIndex = 3;
            // 
            // txtK1
            // 
            this.txtK1.Location = new System.Drawing.Point(12, 72);
            this.txtK1.Name = "txtK1";
            this.txtK1.Size = new System.Drawing.Size(100, 22);
            this.txtK1.TabIndex = 5;
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(111, 176);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(45, 16);
            this.lblResult.TabIndex = 7;
            this.lblResult.Text = "Result";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 246);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.txtK1);
            this.Controls.Add(this.txtK2);
            this.Controls.Add(this.btnCalculateSum);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCalculateSum;
        private System.Windows.Forms.TextBox txtK2;
        private System.Windows.Forms.TextBox txtK1;
        private System.Windows.Forms.Label lblResult;
    }
}

