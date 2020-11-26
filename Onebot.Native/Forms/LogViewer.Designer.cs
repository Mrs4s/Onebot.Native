
namespace Onebot.Native.Forms
{
    partial class LogViewer
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
            this.LogListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CloseButton = new System.Windows.Forms.Button();
            this.RealTimeCheckbox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // LogListView
            // 
            this.LogListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.LogListView.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LogListView.FullRowSelect = true;
            this.LogListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.LogListView.HideSelection = false;
            this.LogListView.LabelWrap = false;
            this.LogListView.Location = new System.Drawing.Point(12, 12);
            this.LogListView.MultiSelect = false;
            this.LogListView.Name = "LogListView";
            this.LogListView.Size = new System.Drawing.Size(779, 382);
            this.LogListView.TabIndex = 1;
            this.LogListView.UseCompatibleStateImageBehavior = false;
            this.LogListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "时间";
            this.columnHeader1.Width = 97;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "来源";
            this.columnHeader2.Width = 91;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "类型";
            this.columnHeader3.Width = 89;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "内容";
            this.columnHeader4.Width = 424;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "状态/耗时";
            this.columnHeader5.Width = 73;
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(716, 402);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 26);
            this.CloseButton.TabIndex = 6;
            this.CloseButton.Text = "关闭";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // RealTimeCheckbox
            // 
            this.RealTimeCheckbox.AutoSize = true;
            this.RealTimeCheckbox.Checked = true;
            this.RealTimeCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RealTimeCheckbox.Location = new System.Drawing.Point(563, 407);
            this.RealTimeCheckbox.Name = "RealTimeCheckbox";
            this.RealTimeCheckbox.Size = new System.Drawing.Size(75, 21);
            this.RealTimeCheckbox.TabIndex = 7;
            this.RealTimeCheckbox.Text = "实时模式";
            this.RealTimeCheckbox.UseVisualStyleBackColor = true;
            // 
            // LogViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 436);
            this.Controls.Add(this.RealTimeCheckbox);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.LogListView);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "LogViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "日志";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LogViewer_FormClosing);
            this.Load += new System.EventHandler(this.LogViewer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView LogListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.CheckBox RealTimeCheckbox;
    }
}