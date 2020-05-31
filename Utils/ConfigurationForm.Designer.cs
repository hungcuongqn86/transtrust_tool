namespace transtrusttool.Utils
{
    partial class ConfigurationForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._tbImap4SearchPattern = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this._tbImap4Server = new System.Windows.Forms.TextBox();
            this._lImap4Server = new System.Windows.Forms.Label();
            this._tbImap4Password = new System.Windows.Forms.TextBox();
            this._lImap4Password = new System.Windows.Forms.Label();
            this._tbImap4Username = new System.Windows.Forms.TextBox();
            this._lImap4Username = new System.Windows.Forms.Label();
            
            this._bCancel = new System.Windows.Forms.Button();
            this._bSave = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this._tbImap4SearchPattern);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this._tbImap4Server);
            this.groupBox1.Controls.Add(this._lImap4Server);
            this.groupBox1.Controls.Add(this._tbImap4Password);
            this.groupBox1.Controls.Add(this._lImap4Password);
            this.groupBox1.Controls.Add(this._tbImap4Username);
            this.groupBox1.Controls.Add(this._lImap4Username);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(542, 123);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Imap4";
            // 
            // _tbImap4SearchPattern
            // 
            this._tbImap4SearchPattern.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._tbImap4SearchPattern.Location = new System.Drawing.Point(142, 97);
            this._tbImap4SearchPattern.Name = "_tbImap4SearchPattern";
            this._tbImap4SearchPattern.Size = new System.Drawing.Size(397, 20);
            this._tbImap4SearchPattern.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(9, 97);
            this.label5.Name = "label5";
            this.label5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label5.Size = new System.Drawing.Size(126, 19);
            this.label5.TabIndex = 6;
            this.label5.Text = "Search pattern :";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _tbImap4Server
            // 
            this._tbImap4Server.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._tbImap4Server.Location = new System.Drawing.Point(142, 71);
            this._tbImap4Server.Name = "_tbImap4Server";
            this._tbImap4Server.Size = new System.Drawing.Size(397, 20);
            this._tbImap4Server.TabIndex = 5;
            // 
            // _lImap4Server
            // 
            this._lImap4Server.Location = new System.Drawing.Point(9, 71);
            this._lImap4Server.Name = "_lImap4Server";
            this._lImap4Server.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._lImap4Server.Size = new System.Drawing.Size(126, 19);
            this._lImap4Server.TabIndex = 4;
            this._lImap4Server.Text = "Server :";
            this._lImap4Server.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _tbImap4Password
            // 
            this._tbImap4Password.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._tbImap4Password.Location = new System.Drawing.Point(142, 45);
            this._tbImap4Password.Name = "_tbImap4Password";
            this._tbImap4Password.Size = new System.Drawing.Size(397, 20);
            this._tbImap4Password.TabIndex = 3;
            // 
            // _lImap4Password
            // 
            this._lImap4Password.Location = new System.Drawing.Point(9, 45);
            this._lImap4Password.Name = "_lImap4Password";
            this._lImap4Password.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._lImap4Password.Size = new System.Drawing.Size(126, 19);
            this._lImap4Password.TabIndex = 2;
            this._lImap4Password.Text = "Password :";
            this._lImap4Password.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _tbImap4Username
            // 
            this._tbImap4Username.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._tbImap4Username.Location = new System.Drawing.Point(142, 19);
            this._tbImap4Username.Name = "_tbImap4Username";
            this._tbImap4Username.Size = new System.Drawing.Size(397, 20);
            this._tbImap4Username.TabIndex = 1;
            // 
            // _lImap4Username
            // 
            this._lImap4Username.Location = new System.Drawing.Point(9, 19);
            this._lImap4Username.Name = "_lImap4Username";
            this._lImap4Username.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._lImap4Username.Size = new System.Drawing.Size(126, 19);
            this._lImap4Username.TabIndex = 0;
            this._lImap4Username.Text = "User name :";
            this._lImap4Username.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // 
            // _bCancel
            // 
            this._bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._bCancel.Location = new System.Drawing.Point(496, 349);
            this._bCancel.Name = "_bCancel";
            this._bCancel.Size = new System.Drawing.Size(75, 23);
            this._bCancel.TabIndex = 23;
            this._bCancel.Text = "Cancel";
            this._bCancel.UseVisualStyleBackColor = true;
            this._bCancel.Click += new System.EventHandler(this._bCancel_Click);
            // 
            // _bSave
            // 
            this._bSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._bSave.Location = new System.Drawing.Point(415, 349);
            this._bSave.Name = "_bSave";
            this._bSave.Size = new System.Drawing.Size(75, 23);
            this._bSave.TabIndex = 24;
            this._bSave.Text = "Save";
            this._bSave.UseVisualStyleBackColor = true;
            this._bSave.Click += new System.EventHandler(this._bSave_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 29);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(559, 314);
            this.tabControl1.TabIndex = 0;

            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(551, 288);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Imap4";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // ConfigurationForm
            // 
            this.AcceptButton = this._bSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._bCancel;
            this.ClientSize = new System.Drawing.Size(576, 379);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this._bSave);
            this.Controls.Add(this._bCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ConfigurationForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configuration";
            
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox _tbImap4Server;
        private System.Windows.Forms.Label _lImap4Server;
        private System.Windows.Forms.TextBox _tbImap4Password;
        private System.Windows.Forms.Label _lImap4Password;
        private System.Windows.Forms.TextBox _tbImap4Username;
        private System.Windows.Forms.Label _lImap4Username;
        private System.Windows.Forms.TextBox _tbImap4SearchPattern;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button _bCancel;
        private System.Windows.Forms.Button _bSave;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox3;
    }
}