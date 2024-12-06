using System.Windows.Forms;

namespace AutoCADPluginLauncher
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private Button btnLaunch;
        private Button btnSelectPlugin;
        private Button btnSelectDrawing;
        private TextBox txtPluginPath;
        private TextBox txtDrawingPath;
        private Label lblPlugin;
        private Label lblDrawing;
        private CheckBox chkUseExisting;
        private CheckBox chkAutoLoad;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblPlugin = new System.Windows.Forms.Label();
            this.txtPluginPath = new System.Windows.Forms.TextBox();
            this.btnSelectPlugin = new System.Windows.Forms.Button();
            this.lblDrawing = new System.Windows.Forms.Label();
            this.txtDrawingPath = new System.Windows.Forms.TextBox();
            this.btnSelectDrawing = new System.Windows.Forms.Button();
            this.chkUseExisting = new System.Windows.Forms.CheckBox();
            this.btnLaunch = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lblPlugin
            //
            this.lblPlugin.Location = new System.Drawing.Point(20, 20);
            this.lblPlugin.Name = "lblPlugin";
            this.lblPlugin.Size = new System.Drawing.Size(70, 20);
            this.lblPlugin.TabIndex = 0;
            this.lblPlugin.Text = "插件路径:";
            //
            // txtPluginPath
            //
            this.txtPluginPath.Location = new System.Drawing.Point(90, 20);
            this.txtPluginPath.Name = "txtPluginPath";
            this.txtPluginPath.Size = new System.Drawing.Size(300, 21);
            this.txtPluginPath.TabIndex = 1;
            //
            // btnSelectPlugin
            //
            this.btnSelectPlugin.Location = new System.Drawing.Point(400, 20);
            this.btnSelectPlugin.Name = "btnSelectPlugin";
            this.btnSelectPlugin.Size = new System.Drawing.Size(70, 23);
            this.btnSelectPlugin.TabIndex = 2;
            this.btnSelectPlugin.Text = "浏览...";
            this.btnSelectPlugin.Click += new System.EventHandler(this.btnSelectPlugin_Click);
            //
            // lblDrawing
            //
            this.lblDrawing.Location = new System.Drawing.Point(20, 50);
            this.lblDrawing.Name = "lblDrawing";
            this.lblDrawing.Size = new System.Drawing.Size(70, 20);
            this.lblDrawing.TabIndex = 3;
            this.lblDrawing.Text = "图纸路径:";
            //
            // txtDrawingPath
            //
            this.txtDrawingPath.Location = new System.Drawing.Point(90, 50);
            this.txtDrawingPath.Name = "txtDrawingPath";
            this.txtDrawingPath.Size = new System.Drawing.Size(300, 21);
            this.txtDrawingPath.TabIndex = 4;
            //
            // btnSelectDrawing
            //
            this.btnSelectDrawing.Location = new System.Drawing.Point(400, 50);
            this.btnSelectDrawing.Name = "btnSelectDrawing";
            this.btnSelectDrawing.Size = new System.Drawing.Size(70, 23);
            this.btnSelectDrawing.TabIndex = 5;
            this.btnSelectDrawing.Text = "浏览...";
            this.btnSelectDrawing.Click += btnSelectDrawing_Click;
            //
            // chkUseExisting
            //
            this.chkUseExisting.Location = new System.Drawing.Point(90, 80);
            this.chkUseExisting.Name = "chkUseExisting";
            this.chkUseExisting.Size = new System.Drawing.Size(200, 20);
            this.chkUseExisting.TabIndex = 6;
            this.chkUseExisting.Text = "使用已打开的CAD程序";
            //
            // btnLaunch
            //
            this.btnLaunch.Location = new System.Drawing.Point(90, 110);
            this.btnLaunch.Name = "btnLaunch";
            this.btnLaunch.Size = new System.Drawing.Size(200, 30);
            this.btnLaunch.TabIndex = 7;
            this.btnLaunch.Text = "执行操作";
            this.btnLaunch.Click += btnLaunch_Click;
            //
            // MainForm
            //
            this.ClientSize = new System.Drawing.Size(484, 230);
            this.Controls.Add(this.lblPlugin);
            this.Controls.Add(this.txtPluginPath);
            this.Controls.Add(this.btnSelectPlugin);
            this.Controls.Add(this.lblDrawing);
            this.Controls.Add(this.txtDrawingPath);
            this.Controls.Add(this.btnSelectDrawing);
            this.Controls.Add(this.chkUseExisting);
            this.Controls.Add(this.btnLaunch);
            this.Name = "MainForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "AutoCAD Plugin Launcher";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}

