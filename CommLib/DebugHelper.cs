using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CommLib
{
    public static class DebugHelper
    {
        public static void WriteLog(string message, string sPath = "~/Log.log")
        {
            try
            {
                string physicalPath = sPath;

                // 如果包含虚拟路径符号 ~，将其转换为当前应用程序的根目录物理路径
                if (sPath.StartsWith("~"))
                {
                    string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    // 去除 ~ 和可能的斜杠，拼接到主程序根目录下
                    string relativePath = sPath.TrimStart('~', '/', '\\');
                    physicalPath = Path.Combine(baseDirectory, relativePath);
                }

                // 确保日志文件的目标文件夹存在
                string directoryName = Path.GetDirectoryName(physicalPath);
                if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }

                // 格式化日志内容并追加写入
                string logContent = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}";
                File.AppendAllText(physicalPath, logContent);
            }
            catch (Exception ex)
            {
                // 防止日志本身出错导致主程序崩溃
                System.Diagnostics.Debug.WriteLine($"WriteLog Error: {ex.Message}");
            }
        }
    }
}
