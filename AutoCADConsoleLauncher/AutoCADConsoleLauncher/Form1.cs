using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Autodesk.AutoCAD.Interop;
using Microsoft.Win32;

namespace AutoCADPluginLauncher
{
    public partial class MainForm : Form
    {
        private string acadPath = string.Empty;
        private const string COMMAND_NAME = "ExportDWGToSqlServer";
        private string regexDeleteText = string.Empty;

        public MainForm()
        {
            InitializeComponent();
            acadPath = GetAutoCADPath().Path;
            // 添加自动加载设置选项
            //this.chkAutoLoad = new CheckBox();
            //this.chkAutoLoad.Text = "设置为CAD启动时自动加载";
            //this.chkAutoLoad.Location = new System.Drawing.Point(90, 140);
            //this.chkAutoLoad.Size = new System.Drawing.Size(200, 20);
            //this.Controls.Add(this.chkAutoLoad);
        }

        private void btnSelectPlugin_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "DLL files (*.dll)|*.dll|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtPluginPath.Text = openFileDialog.FileName;
                }
            }
        }

        private void btnSelectDrawing_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "AutoCAD files (*.dwg)|*.dwg|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtDrawingPath.Text = openFileDialog.FileName;
                }
            }
        }

        private AutoCADInfo GetAutoCADPath()
        {
            try
            {
                string highestVersionPath = null;
                int highestVersionYear = 0;

                // 定义要检查的注册表基项路径
                string[] baseKeys = new string[]
                {
                    @"SOFTWARE\Autodesk\AutoCAD", // 64位AutoCAD在64位系统，32位AutoCAD在32位系统
                    @"SOFTWARE\Wow6432Node\Autodesk\AutoCAD", // 32位AutoCAD在64位系统
                };

                // 检查不同的注册表视图
                RegistryView[] registryViews = new RegistryView[]
                {
                    RegistryView.Registry64,
                    RegistryView.Registry32,
                };

                foreach (RegistryView registryView in registryViews)
                {
                    foreach (
                        RegistryHive hive in new[]
                        {
                            RegistryHive.LocalMachine,
                            RegistryHive.CurrentUser,
                        }
                    )
                    {
                        foreach (string baseKey in baseKeys)
                        {
                            using (
                                RegistryKey baseRegistryKey = RegistryKey
                                    .OpenBaseKey(hive, registryView)
                                    .OpenSubKey(baseKey)
                            )
                            {
                                if (baseRegistryKey != null)
                                {
                                    foreach (string subKeyName in baseRegistryKey.GetSubKeyNames())
                                    {
                                        using (
                                            RegistryKey versionKey = baseRegistryKey.OpenSubKey(
                                                subKeyName
                                            )
                                        )
                                        {
                                            if (versionKey != null)
                                            {
                                                foreach (
                                                    string productKeyName in versionKey.GetSubKeyNames()
                                                )
                                                {
                                                    using (
                                                        RegistryKey productKey =
                                                            versionKey.OpenSubKey(productKeyName)
                                                    )
                                                    {
                                                        if (productKey != null)
                                                        {
                                                            string productName =
                                                                productKey.GetValue("ProductName")
                                                                as string;
                                                            if (!string.IsNullOrEmpty(productName))
                                                            {
                                                                int currentVersionYear =
                                                                    ExtractYearFromProductName(
                                                                        productName
                                                                    );
                                                                if (
                                                                    currentVersionYear
                                                                    > highestVersionYear
                                                                )
                                                                {
                                                                    highestVersionYear =
                                                                        currentVersionYear;
                                                                    // 获取AutoCAD安装路径
                                                                    string installPath =
                                                                        productKey.GetValue(
                                                                            "AcadLocation"
                                                                        ) as string;
                                                                    if (
                                                                        string.IsNullOrEmpty(
                                                                            installPath
                                                                        )
                                                                    )
                                                                    {
                                                                        // 尝试从"InstallLocation"获取安装路径
                                                                        installPath =
                                                                            productKey.GetValue(
                                                                                "InstallLocation"
                                                                            ) as string;
                                                                    }
                                                                    if (
                                                                        !string.IsNullOrEmpty(
                                                                            installPath
                                                                        )
                                                                    )
                                                                    {
                                                                        string acadExePath =
                                                                            Path.Combine(
                                                                                installPath,
                                                                                "acad.exe"
                                                                            );
                                                                        if (
                                                                            File.Exists(acadExePath)
                                                                        )
                                                                        {
                                                                            highestVersionPath =
                                                                                acadExePath;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return new AutoCADInfo { Version = highestVersionYear, Path = highestVersionPath };
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取 AutoCAD 路径失败：" + ex.Message);
                return null;
            }
        }

        private int ExtractYearFromProductName(string productName)
        {
            // 示例ProductName："AutoCAD 2021 - 简体中文 (Simplified Chinese)"
            // 我们需要提取其中的年份2021

            Match match = Regex.Match(productName, @"\b(20\d{2})\b");
            if (match.Success && int.TryParse(match.Value, out int year))
            {
                return year;
            }
            return 0;
        }

        private void btnLaunch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPluginPath.Text))
            {
                MessageBox.Show("请选择插件文件！");
                return;
            }

            if (string.IsNullOrEmpty(txtDrawingPath.Text))
            {
                MessageBox.Show("请选择要打开的图纸！");
                return;
            }

            if (!File.Exists(txtPluginPath.Text))
            {
                MessageBox.Show("路径文件不存在！");
                return;
            }

            regexDeleteText = Regex.Replace(txtDrawingPath.Text, @"[\p{C}]", string.Empty);
            if (!File.Exists(regexDeleteText))
            {
                MessageBox.Show("路径图纸不存在！");
                return;
            }

            try
            {
                // 如果选择了自动加载，设置自动加载配置
                //if (chkAutoLoad.Checked)
                //{
                //    PluginAutoLoader.SetupAutoLoad(txtPluginPath.Text, acadPath);
                //}
                PluginAutoLoader.SetupAutoLoad(txtPluginPath.Text, acadPath);

                if (chkUseExisting.Checked)
                {
                    // 尝试使用已存在的CAD程序
                    HandleExistingAutoCAD();
                }
                else
                {
                    // 启动新的CAD程序
                    LaunchNewAutoCAD();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败：" + ex.Message);
            }
        }

        private void HandleExistingAutoCAD()
        {
            try
            {
                // 获取所有运行中的AutoCAD实例
                dynamic acadApp = Marshal.GetActiveObject("AutoCAD.Application");

                // 加载插件
                //if (!chkAutoLoad.Checked)
                //{
                //    acadApp.ActiveDocument.SendCommand($"_NETLOAD \"{txtPluginPath.Text}\" ");
                //}

                // 打开图纸
                OpenDrawing(acadApp, regexDeleteText);
                System.Threading.Thread.Sleep(1000);

                // 执行命令
                //acadApp.ActiveDocument.SendCommand(COMMAND_NAME + "\n");
                ExecutePluginCommand();
            }
            catch (COMException)
            {
                MessageBox.Show(
                    "未找到正在运行的AutoCAD程序，请启动AutoCAD或取消勾选使用已打开的CAD程序选项。"
                );
            }
        }

        private void LaunchNewAutoCAD()
        {
            if (string.IsNullOrEmpty(acadPath))
            {
                MessageBox.Show("未找到 AutoCAD 安装路径！");
                return;
            }

            try
            {
                // 启动 AutoCAD
                Process acadProcess = new Process();
                acadProcess.StartInfo.FileName = acadPath;

                // 添加命令行参数来加载插件和打开图纸
                //acadProcess.StartInfo.Arguments = $"/b \"{regexDeleteText}\"";

                acadProcess.Start();

                // 等待 AutoCAD 完全启动
                acadProcess.WaitForInputIdle();

                // 给 AutoCAD 足够的启动时间
                System.Threading.Thread.Sleep(10000);

                // 分步执行操作
                try
                {
                    // 获取所有运行中的AutoCAD实例
                    dynamic acadApp = Marshal.GetActiveObject("AutoCAD.Application");

                    // 加载插件
                    //if (!chkAutoLoad.Checked)
                    //{
                    //    acadApp.ActiveDocument.SendCommand($"_NETLOAD \"{txtPluginPath.Text}\" ");
                    //}

                    // 打开图纸
                    OpenDrawing(acadApp, regexDeleteText);
                    System.Threading.Thread.Sleep(1000);

                    // 执行命令
                    //acadApp.ActiveDocument.SendCommand(COMMAND_NAME + "\n");
                    ExecutePluginCommand();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"执行操作失败：{ex.Message}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("启动 AutoCAD 失败：" + ex.Message);
            }
        }

        private void ExecutePluginCommand()
        {
            int maxRetries = 5;
            int retryDelay = 1000;
            int currentRetry = 0;

            while (currentRetry < maxRetries)
            {
                try
                {
                    dynamic acadApp = Marshal.GetActiveObject("AutoCAD.Application");
                    if (acadApp != null)
                    {
                        System.Threading.Thread.Sleep(1000);

                        dynamic acadDoc = acadApp.ActiveDocument;
                        if (acadDoc != null)
                        {
                            // 使用COM接口的方法直接执行命令
                            acadDoc.SendCommand(COMMAND_NAME + " ");
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (ex.HResult == unchecked((int)0x80010001)) // RPC 被拒绝
                    {
                        // 延迟 1 秒后再重试
                        System.Threading.Thread.Sleep(retryDelay);
                        currentRetry++;
                    }
                    else
                    {
                        currentRetry++;
                        System.Threading.Thread.Sleep(retryDelay);
                    }

                    if (currentRetry >= maxRetries)
                    {
                        MessageBox.Show($"执行命令失败：{ex.Message}\n已重试 {maxRetries} 次。");
                        return;
                    }
                }
            }
        }

        private static AcadDocument OpenDrawing(AcadApplication autoCAD, string drawingPath)
        {
            try
            {
                // 检查 Documents.Open 方法支持的参数数量
                // 一般来说，Open 方法支持三个参数：文件路径、只读标志、隐藏标志
                // 如果您的 AutoCAD 版本支持更多参数，请根据需要调整
                return autoCAD.Documents.Open(drawingPath, false, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开绘图文件时发生错误: {ex.Message}");
                return null;
            }
        }

        public class AutoCADInfo
        {
            public int Version { get; set; }
            public string Path { get; set; }
        }
    }
}