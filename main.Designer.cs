namespace transtrusttool
{
    partial class main
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
            this.config_btn = new System.Windows.Forms.Button();
            this.start_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // config_btn
            // 
            this.config_btn.Location = new System.Drawing.Point(0, 0);
            this.config_btn.Name = "config_btn";
            this.config_btn.Size = new System.Drawing.Size(75, 23);
            this.config_btn.TabIndex = 0;
            this.config_btn.Text = "Config";
            this.config_btn.UseVisualStyleBackColor = true;
            this.config_btn.Click += new System.EventHandler(this.config_btn_Click);
            // 
            // start_btn
            // 
            this.start_btn.Location = new System.Drawing.Point(182, 94);
            this.start_btn.Name = "start_btn";
            this.start_btn.Size = new System.Drawing.Size(75, 56);
            this.start_btn.TabIndex = 1;
            this.start_btn.Text = "Start";
            this.start_btn.UseVisualStyleBackColor = true;
            this.start_btn.Click += new System.EventHandler(this.start_btn_Click);
            // 
            // main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 255);
            this.Controls.Add(this.start_btn);
            this.Controls.Add(this.config_btn);
            this.Name = "main";
            this.Text = "TranstrustTool";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button config_btn;
        private System.Windows.Forms.Button start_btn;
    }
}

