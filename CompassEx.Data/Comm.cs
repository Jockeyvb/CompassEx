
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
        /// 
        /// </summary>
        public static IFreeSql Orm;

        /// <summary>
        /// 全局 SQLite 连接字符串
        /// </summary>
        public static string ConnectionString { get; private set; }


        /// <summary>
        /// 设置错误返回是否为详细
        /// </summary>
        public static bool IsRRTErrorDetailed { get; set; } = true;



        private static void CreateFSQL()
        {
            IFreeSql fsql = new FreeSql.FreeSqlBuilder()
    .UseConnectionString(FreeSql.DataType.Sqlite, ConnectionString)
    //Automatically synchronize the entity structure to the database.
    //FreeSql will not scan the assembly, and will generate a table if and only when the CRUD instruction is executed.
    .UseAutoSyncStructure(true)
    .Build();
            Orm = fsql;
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

                CreateFSQL();
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

            CreateFSQL();
        }



    }
}
