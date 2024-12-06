using System;
using System.IO;
using System.Windows.Forms;

namespace AutoCADPluginLauncher
{
    public class PluginAutoLoader
    {
        public static void SetupAutoLoad(string pluginPath, string acadPath)
        {
            try
            {
                // 获取AutoCAD支持文件路径
                string roamingFolder = Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData
                );
                // 获取可能的支持文件路径
                string supportPath = string.Empty;
                string acaddocPath = string.Empty;

                // 首先尝试用户配置文件夹下的Support路径
                string userSupportPath = Path.Combine(
                    roamingFolder,
                    "Autodesk",
                    "AutoCAD 2024",
                    "R24.3",
                    "chs",
                    "Support"
                );

                // 然后尝试CAD安装目录下的Support路径
                string installSupportPath = string.Empty;
                if (!string.IsNullOrEmpty(acadPath))
                {
                    string installDir = Path.GetDirectoryName(acadPath);
                    if (!string.IsNullOrEmpty(installDir))
                    {
                        installSupportPath = Path.Combine(installDir, "Support");
                    }
                }

                // 选择存在的路径，优先使用用户配置文件夹路径
                if (Directory.Exists(installSupportPath))
                {
                    supportPath = installSupportPath;
                }
                else if (Directory.Exists(userSupportPath))
                {
                    supportPath = userSupportPath;
                }
                else
                {
                    // 如果都不存在，使用用户配置文件夹路径并创建
                    supportPath = installSupportPath;
                    try
                    {
                        Directory.CreateDirectory(supportPath);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"无法创建Support文件夹：{ex.Message}");
                    }
                }

                acaddocPath = Path.Combine(supportPath, "acaddoc.lsp");

                // 创建自动加载代码
                string autoLoadCode =
                    $@"
(defun S::STARTUP()
    (command ""_NETLOAD"" ""{pluginPath.Replace("\\", "\\\\")}"")
    (princ)
)";
                // 如果文件不存在，创建新文件
                if (!File.Exists(acaddocPath))
                {
                    File.WriteAllText(acaddocPath, autoLoadCode);
                }
                else
                {
                    // 如果文件已存在，检查是否已包含加载代码
                    string existingContent = File.ReadAllText(acaddocPath);
                    if (!existingContent.Contains(autoLoadCode))
                    {
                        File.AppendAllText(acaddocPath, autoLoadCode);
                    }
                }

                MessageBox.Show("自动加载设置已完成！CAD启动时将自动加载插件。");
            }
            catch (Exception ex)
            {
                MessageBox.Show("设置自动加载失败：" + ex.Message);
            }
        }
    }
}