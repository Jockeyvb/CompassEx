using Dapper;               // 💡 请确保 NuGet 安装了 Dapper
using Microsoft.Data.Sqlite; // 💡 请确保 NuGet 安装了 Microsoft.Data.Sqlite.Core
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;

namespace CompassEx.Data
{
    public static class Comm
    {
        /// <summary>
        /// 全局 SQLite 连接字符串
        /// </summary>
        public static string ConnectionString { get; private set; }







        /// <summary>
        /// 获取一个全新的、已经打开的 SQLite 数据库连接（供 Dapper 使用）
        /// </summary>
        /// <returns>已打开的 IDbConnection 对象</returns>
        public static IDbConnection GetOpenConnection()
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                throw new InvalidOperationException("数据库尚未初始化！请先调用 Comm.InitializeDatabase() 方法。");
            }

            // 建立原生 SQLite 连接
            var connection = new SqliteConnection(ConnectionString);

            // 显式打开连接（Dapper 虽然能自动打开，但手动打开在多线程下更安全）
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            return connection;
        }

        /// <summary>
        /// 检查并从 DLL 中释放 SQLite 数据库文件到指定的实体路径
        /// </summary>
        /// <param name="targetDbDir">目标实体路径（例如从 MAUI 或 .NET 4.8 传进来的沙盒/执行路径）</param>
        public static void InitializeDatabase(string targetDbDir = "")
        {


            if (string.IsNullOrWhiteSpace(targetDbDir)) targetDbDir = AppDomain.CurrentDomain.BaseDirectory;
            string dbPath = Path.Combine(targetDbDir, "JockeyCalendar.db3"); ;

            //    这里的连接字符串包含了缓存共享（Cache=Shared）机制，能极大提升 SQLite 的多线程读写性能
            string connStr = $"Data Source={dbPath};Cache=Shared;Mode=ReadWriteCreate;";
            ConnectionString = connStr;
            // 【注意】如果你在 Comm.InitializeDatabase 内部已经硬编码了连接字符串前缀，
            // 记得确认两边格式一致。直接用上面的 connStr 注入最为安全。

            // 1. 如果实体文件已经存在，直接跳过（避免覆盖掉用户之后写入的新数据！）
            if (File.Exists(dbPath))
            {
                return;
            }

            // 2. 确保目标文件夹目录存在，不存在就自动建立
            string folder = Path.GetDirectoryName(targetDbDir);
            if (!string.IsNullOrEmpty(folder) && !Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            // 3. 取得当前这个类库的 Assembly（程序集）
            var assembly = Assembly.GetExecutingAssembly();

            // 4. 动态组合内嵌资源的完整名称
            string rootNamespace = assembly.GetName().Name;
            string resourceName = $"{rootNamespace}.JockeyCalendar.db3";

            // 5. 从 DLL 中读取二进制数据流（Stream）
            using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                {
                    // 【除错保险】如果找不到，印出所有内嵌资源名称，方便你检查是不是命名空间拼错
                    string[] names = assembly.GetManifestResourceNames();
                    string availableNames = string.Join(", ", names);
                    throw new FileNotFoundException($"在 DLL 中找不到内嵌资源：'{resourceName}'。\n当前 DLL 内含的资源有：{availableNames}");
                }

                // 6. 建立实体文件并将 DLL 内的数据复制（释放）过去
                using (FileStream fileStream = new FileStream(dbPath, FileMode.Create, FileAccess.Write))
                {
                    resourceStream.CopyTo(fileStream);
                }
            }



        }

        #region 全局通用 Dapper 快捷方法示例（选填，可选保留）

        /// <summary>
        /// 全局通用：执行无返回值的 SQL 语句（如 INSERT, UPDATE, DELETE）
        /// </summary>
        public static int Execute(string sql, object param = null)
        {
            using (IDbConnection db = GetOpenConnection())
            {
                return db.Execute(sql, param);
            }
        }

        /// <summary>
        /// 全局通用：查询数据列表
        /// </summary>
        public static IEnumerable<T> Query<T>(string sql, object param = null)
        {
            using (IDbConnection db = GetOpenConnection())
            {
                return db.Query<T>(sql, param);
            }
        }

        #endregion
    }
}
