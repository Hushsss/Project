using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Serilog;

namespace PersonnelManagementSystem
{
    public class ConfigurationHelper
    {
        public static string GetConnectionString(string name)
        {
            // 获取当前程序集路径
            string assemblyPath = typeof(ConfigurationHelper).Assembly.Location;
            // 动态生成配置文件路径
            string configFileName = Path.GetFileName(assemblyPath) + ".config";
            string configPath = Path.Combine(Path.GetDirectoryName(assemblyPath), configFileName);
            if (!File.Exists(configPath))
            {
                throw new FileNotFoundException($"配置文件未找到: {configPath}");
            }

            var map = new ExeConfigurationFileMap { ExeConfigFilename = configPath };
            var config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            return config.ConnectionStrings.ConnectionStrings[name]?.ConnectionString;
        }

        public static void CheckConnectionString(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                Log.Error("连接字符串 'LocalDb1' 不存在，请检查配置文件。");
                throw new ConfigurationErrorsException(
                    "连接字符串 'LocalDb1' 不存在，请检查配置文件。"
                );
            }
        }
    }
}