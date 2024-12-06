using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PersonnelManagementSystem
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblWelcome;
        private Button btnUserManagement;
        private Button btnDataEntry;
        private Button btnDataView;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblWelcome = new Label();
            this.btnUserManagement = new Button();
            this.btnDataEntry = new Button();
            this.btnDataView = new Button();
            this.SuspendLayout();
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Location = new System.Drawing.Point(20, 20);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(65, 12);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "欢迎：XXX";
            // 
            // btnUserManagement
            // 
            this.btnUserManagement.Location = new System.Drawing.Point(20, 60);
            this.btnUserManagement.Name = "btnUserManagement";
            this.btnUserManagement.Size = new System.Drawing.Size(100, 23);
            this.btnUserManagement.TabIndex = 1;
            this.btnUserManagement.Text = "用户管理";
            this.btnUserManagement.UseVisualStyleBackColor = true;
            this.btnUserManagement.Visible = false;
            this.btnUserManagement.Click += new System.EventHandler(this.btnUserManagement_Click);
            // 
            // btnDataEntry
            // 
            this.btnDataEntry.Location = new System.Drawing.Point(20, 100);
            this.btnDataEntry.Name = "btnDataEntry";
            this.btnDataEntry.Size = new System.Drawing.Size(100, 23);
            this.btnDataEntry.TabIndex = 2;
            this.btnDataEntry.Text = "数据录入";
            this.btnDataEntry.UseVisualStyleBackColor = true;
            this.btnDataEntry.Visible = false;
            this.btnDataEntry.Click += new System.EventHandler(this.btnDataEntry_Click);
            // 
            // btnDataView
            // 
            this.btnDataView.Location = new System.Drawing.Point(20, 140);
            this.btnDataView.Name = "btnDataView";
            this.btnDataView.Size = new System.Drawing.Size(100, 23);
            this.btnDataView.TabIndex = 3;
            this.btnDataView.Text = "数据查看";
            this.btnDataView.UseVisualStyleBackColor = true;
            this.btnDataView.Visible = false;
            this.btnDataView.Click += new System.EventHandler(this.btnDataView_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 201);
            this.Controls.Add(this.btnDataView);
            this.Controls.Add(this.btnDataEntry);
            this.Controls.Add(this.btnUserManagement);
            this.Controls.Add(this.lblWelcome);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "主界面";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }

}
