
namespace Onebot.Native.Forms
{
    partial class MainForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.AccessTokenTextbox = new System.Windows.Forms.TextBox();
            this.AddressLabel = new System.Windows.Forms.Label();
            this.StartupButton = new System.Windows.Forms.Button();
            this.PortTextbox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.AddressTextbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.Notify = new System.Windows.Forms.NotifyIcon(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(3, 1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(376, 134);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.AccessTokenTextbox);
            this.tabPage1.Controls.Add(this.AddressLabel);
            this.tabPage1.Controls.Add(this.StartupButton);
            this.tabPage1.Controls.Add(this.PortTextbox);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.AddressTextbox);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(368, 104);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "连接设置";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "AccessToken";
            // 
            // AccessTokenTextbox
            // 
            this.AccessTokenTextbox.Location = new System.Drawing.Point(92, 55);
            this.AccessTokenTextbox.Name = "AccessTokenTextbox";
            this.AccessTokenTextbox.Size = new System.Drawing.Size(187, 23);
            this.AccessTokenTextbox.TabIndex = 7;
            // 
            // AddressLabel
            // 
            this.AddressLabel.AutoSize = true;
            this.AddressLabel.Location = new System.Drawing.Point(3, 84);
            this.AddressLabel.Name = "AddressLabel";
            this.AddressLabel.Size = new System.Drawing.Size(15, 17);
            this.AddressLabel.TabIndex = 6;
            this.AddressLabel.Text = "1";
            // 
            // StartupButton
            // 
            this.StartupButton.Location = new System.Drawing.Point(287, 55);
            this.StartupButton.Name = "StartupButton";
            this.StartupButton.Size = new System.Drawing.Size(75, 43);
            this.StartupButton.TabIndex = 5;
            this.StartupButton.Text = "启动";
            this.StartupButton.UseVisualStyleBackColor = true;
            this.StartupButton.Click += new System.EventHandler(this.StartupButton_Click);
            // 
            // PortTextbox
            // 
            this.PortTextbox.Location = new System.Drawing.Point(299, 23);
            this.PortTextbox.Name = "PortTextbox";
            this.PortTextbox.Size = new System.Drawing.Size(55, 23);
            this.PortTextbox.TabIndex = 4;
            this.PortTextbox.Text = "8989";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(286, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = ":";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(297, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "端口";
            // 
            // AddressTextbox
            // 
            this.AddressTextbox.Location = new System.Drawing.Point(3, 23);
            this.AddressTextbox.Name = "AddressTextbox";
            this.AddressTextbox.Size = new System.Drawing.Size(278, 23);
            this.AddressTextbox.TabIndex = 1;
            this.AddressTextbox.Text = "127.0.0.1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "监听IP";
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(368, 104);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "关于";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // Notify
            // 
            this.Notify.Text = "Onebot.Native";
            this.Notify.Visible = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 137);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Onebot.Native";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox PortTextbox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox AddressTextbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button StartupButton;
        private System.Windows.Forms.Label AddressLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox AccessTokenTextbox;
        private System.Windows.Forms.NotifyIcon Notify;
    }
}