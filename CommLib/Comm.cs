using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace CommLib
{
    public static class Comm
    {

        /// <summary>
        /// 判断是否为数值类型（包含基础数值及其可空形式）
        /// </summary>
        public static bool IsNumericType(Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;

            return type == typeof(byte) || type == typeof(sbyte) ||
                   type == typeof(short) || type == typeof(ushort) ||
                   type == typeof(int) || type == typeof(uint) ||
                   type == typeof(long) || type == typeof(ulong) ||
                   type == typeof(float) || type == typeof(double) ||
                   type == typeof(decimal);
        }




        /// <summary>
        /// 利用JSON尝试复制对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T? DeepJSONClone<T>(this T obj)
        {
            if (obj == null) return default!;
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));
        }


        /// <summary>
        /// 把一个base64字符转成字节数组
        /// </summary>
        /// <param name="base64str"></param>
        /// <returns></returns>
        public static async Task<ReturnResultType<byte[]>> Base64ToBytes(string base64str)
        {



            ReturnResultType<byte[]> rrt = new ReturnResultType<byte[]>();

            try
            {
                // 1. 去除 Base64 前缀
                if (base64str.Contains(","))
                {
                    base64str = base64str.Split(',')[1];
                }

                // 2. 转为字节数组
                byte[] imageBytes = Convert.FromBase64String(base64str);

                rrt.RESULT = 1;
                rrt.ReturnObj = imageBytes;


            }
            catch (Exception ex)
            {
                rrt.RESULT = 0;
                rrt.Message = ex.ToString();
                Console.WriteLine($"内存流处理分享出错: {ex.Message}");
            }
            return rrt;
        }




        /// <summary>
        /// 扩展方法：利用运行时反射（Reflection）机制，强行从父级基类模板中将所有属性与字段的值“浅拷贝”反灌给当前继承类实例。
        /// </summary>
        /// <typeparam name="TChild">继承类（派生类）的具体类型，必须隐式继承自 <typeparamref name="TBase"/>。</typeparam>
        /// <typeparam name="TBase">基类（模板类）的具体类型。</typeparam>
        /// <param name="child">正在接受赋值的当前继承类（子类）目标对象实例。</param>
        /// <param name="baseTemplate">作为数据源的基类（父类）实体模板对象。</param>
        /// <remarks>
        /// <b>🛠️ 内部反射拷贝门道与避坑提示：</b>
        /// <list type="number">
        /// <item><description><b>属性遍历</b>：方法首先通过 <c>typeof(TBase).GetProperties()</c> 捕获父类公开属性，并通过 <c>prop.CanWrite</c> 安全拦截，防止对只读属性或受保护的 Get 块执行非法注入。</description></item>
        /// <item><description><b>字段反灌</b>：随后通过 <c>typeof(TBase).GetFields()</c> 递归提取底层物理字段并完成原子值覆写。</description></item>
        /// <item><description><b>⚠️ 性能与安全警告</b>：由于使用了运行时动态反射，该操作会带来较大的 CPU 耗能，<b>在高频循环或深层装卦排盘时应克制使用</b>。另外，它仅支持第一层浅拷贝，若字段中含有 <c>List&lt;T&gt;</c> 等引用类型，两端实例会共享同一份内存地址，修改时存在联动篡改的隐患，在跨变卦深度计算时需高度注意。</description></item>
        /// </list>
        /// 该方法常用于在 JSON 反序列化后期生命周期中，快速将多字段的静态基类参数直接克隆给派生实体类。
        /// </remarks>
        public static void ApplyBaseProperties<TChild, TBase>(this TChild child, TBase baseTemplate)
            where TChild : TBase
        {
            // 用反射复制（慎用，性能差且容易出错）
            foreach (var prop in typeof(TBase).GetProperties())
            {
                if (prop.CanWrite)
                {
                    prop.SetValue(child, prop.GetValue(baseTemplate));
                }
            }

            foreach (var f in typeof(TBase).GetFields())
            {
                if (f.IsInitOnly || f.IsLiteral)
                {
                    continue;
                }
                f.SetValue(child, f.GetValue(baseTemplate));
            }
        }



        /// <summary>
        /// 从十六进制字符串解析 System.Drawing.Color
        /// 支持格式: "#RGB", "#RRGGBB", "#AARRGGBB" (也可以不带 # 号)
        /// </summary>
        public static Color ColorFromHex(string hex, bool IsFromWeb = true)
        {
            if (string.IsNullOrWhiteSpace(hex))
            {
                return Color.Transparent;
            }

            // 去除空格和前缀 '#'
            hex = hex.Trim().TrimStart('#');

            try
            {
                if (hex.Length == 3) // 例如: "F00" -> "#FF0000"
                {
                    string r = new string(hex[0], 2);
                    string g = new string(hex[1], 2);
                    string b = new string(hex[2], 2);
                    return Color.FromArgb(
                        255,
                        int.Parse(r, NumberStyles.HexNumber),
                        int.Parse(g, NumberStyles.HexNumber),
                        int.Parse(b, NumberStyles.HexNumber)
                    );
                }
                else if (hex.Length == 6) // 例如: "FF0000"
                {
                    int r = int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                    int g = int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
                    int b = int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
                    return Color.FromArgb(255, r, g, b);
                }
                else if (hex.Length == 8) // 例如: "80FF0000" (带 Alpha 通道)
                {
                    if (IsFromWeb == false)
                    {
                        int a = int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                        int r = int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
                        int g = int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
                        int b = int.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
                        return Color.FromArgb(a, r, g, b);
                    }
                    else
                    {
                        int r = int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                        int g = int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
                        int b = int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
                        int a = int.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
                        return Color.FromArgb(a, r, g, b);
                    }

                }
            }
            catch
            {
                // 解析失败时返回透明色或默认色
            }

            return Color.Transparent;
        }



        /// <summary>
        /// 写入调用日志
        /// </summary>
        /// <param name="sFile">日志文件</param>
        /// <param name="sLog">内容</param>
        public static void WriteFileLog(string sLog, string sFile = "/Log.log")
        {
            if (!Path.IsPathRooted(sFile) || sFile.StartsWith("/"))
            {
                sFile = sFile.TrimStart('/', '\\');

                // 使用标准 .NET 的本地应用数据目录（Windows下对应 AppData\Local，Android下对应应用私有目录）
                string baseDir = AppContext.BaseDirectory;

                // 为了避免日志文件散落在根目录，建议加一层你的应用专属文件夹名
                sFile = Path.Combine(baseDir, sFile);
            }

            string sPath = Path.GetDirectoryName(sFile);
            if (!string.IsNullOrEmpty(sPath) && !Directory.Exists(sPath))
            {
                Directory.CreateDirectory(sPath);
            }



            if (System.IO.File.Exists(sFile))
            {
                FileInfo fi = new FileInfo(sFile);
                if (fi.Length > 1024 * 10000) // 大于 10MB, 重命名备份
                {
                    string backupPath = Path.Combine(sPath, $"{DateTime.Now:yyyyMMddHHmmss}.log");
                    System.IO.File.Move(sFile, backupPath);
                }
            }

            string st = $"[{DateTime.Now}]:\r\n";
            st += sLog + "\r\n";
            System.IO.File.AppendAllText(sFile, st, Encoding.UTF8);
        }


        /// <summary>
        /// base64加解密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="IsEnCode"></param>
        /// <returns></returns>
        public static string Base64(string str, bool IsEnCode = true)
        {

            if (IsEnCode)
            {
                return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(str));
            }
            else
            {
                byte[] bytes = Convert.FromBase64String(str);
                string json = Encoding.UTF8.GetString(bytes);
                return json;
            }


        }


        /// <summary>
        /// 调用类中的泛型方法
        /// </summary>
        /// <param name="ClsOrType">这个方法所在的类或类型(如果是非静态方法必传入实例类)</param>
        /// <param name="sTargetTypeName">泛型的类型名称（带完整的命名空间）</param>
        /// <param name="MethodName">方法名称</param>
        /// <param name="args">入参</param>
        /// <returns></returns>
        public static async Task<ReturnResultType> CallTMethod(object ClsOrType, string sTargetTypeName, string MethodName, params object[] args)
        {
            ReturnResultType rrt = new ReturnResultType();
            try
            {
                if (string.IsNullOrWhiteSpace(sTargetTypeName)) throw new Exception("类名不能为空");
                Type TargetType = AppDomain.CurrentDomain.GetAssemblies()
    .Select(assembly => assembly.GetType(sTargetTypeName))
    .FirstOrDefault(t => t != null);

                rrt = await CallTMethod(ClsOrType, TargetType, MethodName, args);

            }
            catch (Exception ex)
            {
                rrt.RESULT = 0;
                rrt.Message = $"调用泛型方法【{sTargetTypeName}.{MethodName}】时出错：" + ex.ToString();
            }
            return rrt;

        }


        /// <summary>
        /// 调用类中的静态泛型方法
        /// </summary>
        /// <param name="ClsOrType">这个方法所在的类或类型(如果是非静态方法必传入实例类)</param>
        /// <param name="TargetType">泛型的类型</param>
        /// <param name="MethodName">方法名称</param>
        /// <param name="args">入参</param>
        /// <returns></returns>
        public static async Task<ReturnResultType> CallTMethod(object ClsOrType, Type TargetType, string MethodName, params object[] args)
        {
            ReturnResultType rrt = new();
            try
            {
                // 0. 基础判空
                if (ClsOrType == null)
                    throw new ArgumentNullException(nameof(ClsOrType));

                // 1. 获取目标类型
                Type sourceClassType = ClsOrType is Type t ? t : ClsOrType.GetType();

                // 2. 查找静态方法
                MethodInfo method = sourceClassType.GetMethod(MethodName, BindingFlags.Public | BindingFlags.Instance);
                if (method == null)
                {
                    throw new Exception($"未能在类 {sourceClassType.Name} 中找到方法 {MethodName}");
                }

                // 3. 校验并具现化泛型方法（必须在 MakeGenericMethod 之前校验 Definition）
                if (!method.IsGenericMethodDefinition)
                {
                    throw new Exception($"方法 {MethodName} 不是泛型方法定义");
                }
                MethodInfo genericMethod = method.MakeGenericMethod(TargetType);
                //==========================补齐参数==============================
                List<object> lsArgs = args != null ? args.ToList() : new List<object>();
                var mParas = genericMethod.GetParameters();
                for (int i = lsArgs.Count; i < mParas.Length; i++)
                {

                    lsArgs.Add(mParas[i].HasDefaultValue ? mParas[i].DefaultValue : default!);

                }
                //==========================补齐参数==============================

                // 4. 动态调用并利用 dynamic 优雅 Await 异步任务（彻底避开 Task 强转报错）
                var taskObj = genericMethod.Invoke(ClsOrType, lsArgs.ToArray());
                if (taskObj is Task task)
                {
                    await task; // 等待泛型任务执行完成

                    // 利用反射或 dynamic 捞出 Task 里的 Result 结果
                    var resultProperty = task.GetType().GetProperty("Result");
                    if (resultProperty != null)
                    {
                        object actualResult = resultProperty.GetValue(task);

                        Type actualType = actualResult.GetType();

                        // 判断是否是 ReturnResultType<> 的某个泛型实例
                        if (actualType.IsGenericType && actualType.GetGenericTypeDefinition() == typeof(ReturnResultType<>))
                        {


                            // 直接反射获取属性值
                            rrt.Success = (int)actualType.GetProperty("Success")?.GetValue(actualResult)!;
                            rrt.Failure = (int)actualType.GetProperty("Failure")?.GetValue(actualResult)!;
                            rrt.URL = (string)actualType.GetProperty("URL")?.GetValue(actualResult)!;
                            rrt.RESULT = (int)actualType.GetProperty("RESULT")?.GetValue(actualResult)!;
                            rrt.Message = (string)actualType.GetProperty("Message")?.GetValue(actualResult)!;
                            rrt.ReturnObj = actualType.GetProperty("ReturnObj")?.GetValue(actualResult);

                        }
                        else //
                        {


                            rrt.RESULT = 1;
                            rrt.Message = "OK";
                            rrt.ReturnObj = actualResult;
                        }


                    }
                    else
                    {
                        rrt.RESULT = 1;
                        rrt.ReturnObj = null;
                        rrt.Message = "OK";
                    }
                }
                else
                {
                    Type actualType = taskObj.GetType();

                    if (taskObj is ReturnResultType && actualType.GetGenericTypeDefinition() == typeof(ReturnResultType<>))
                    {


                        // 直接反射获取属性值
                        rrt.Success = (int)actualType.GetProperty("Success")?.GetValue(taskObj)!;
                        rrt.Failure = (int)actualType.GetProperty("Failure")?.GetValue(taskObj)!;
                        rrt.URL = (string)actualType.GetProperty("URL")?.GetValue(taskObj)!;
                        rrt.RESULT = (int)actualType.GetProperty("RESULT")?.GetValue(taskObj)!;
                        rrt.Message = (string)actualType.GetProperty("Message")?.GetValue(taskObj)!;
                        rrt.ReturnObj = actualType.GetProperty("ReturnObj")?.GetValue(taskObj);
                    }
                    else
                    {
                        rrt.RESULT = 1;
                        rrt.Message = "OK";
                        rrt.ReturnObj = taskObj;
                    }

                }
            }
            catch (Exception ex)
            {
                rrt.RESULT = 0;
                rrt.Message = ex.InnerException?.ToString() ?? ex.ToString();
            }

            return rrt;
        }


        /// <summary>
        /// 自动把Json字串转换为泛型，如果转换失败则为 null 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T? ConvertJSON<T>(string s)
        {
            if (String.IsNullOrWhiteSpace(s)) return default;

            if (s.IndexOf("{") == -1 && s.IndexOf("[") == -1) return default;
            try
            {
                var p = JToken.Parse(s);
                if (p.Type == JTokenType.String)
                {
                    p = JToken.Parse(p.ToString());
                }
                if (p.Type == JTokenType.Array)
                {
                    return p.ToObject<T>();

                }
                else if (p.Type == JTokenType.Object)
                {
                    return p.ToObject<T>();
                }
                else
                {
                    return default;

                }
            }
            catch (Exception)
            {

                return default;
            }


        }


        /// <summary>
        /// 自动转换为JToken，如果转换失败则为 null 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static JToken? ConvertJSON(string s)
        {
            if (String.IsNullOrWhiteSpace(s)) return null;

            if (s.IndexOf("{") == -1 && s.IndexOf("[") == -1) return null;
            try
            {
                var p = JToken.Parse(s);
                if (p.Type == JTokenType.String)
                {
                    p = JToken.Parse(p.ToString());
                }
                return p;

            }
            catch (Exception)
            {

                return null;
            }


        }
        // 辅助方法：将你的 ApiMethod 枚举转为标准的 HttpMethod
        private static HttpMethod GetHttpMethod(ApiMethod am)
        {
            return am switch
            {
                ApiMethod.Post => HttpMethod.Post,
                ApiMethod.Get => HttpMethod.Get,
                ApiMethod.Put => HttpMethod.Put,
                ApiMethod.Delete => HttpMethod.Delete,
                _ => HttpMethod.Get
            };
        }

        /// <summary>
        /// 不需要令牌请求方法
        /// </summary>
        /// <param name="hc"></param>
        /// <param name="RouterPath"></param>
        /// <param name="Dto"></param>
        /// <param name="am"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public static async Task<ReturnResultType<T>> CallWebApiMethodWithoutTokenAsync<T>(HttpClient hc, string RouterPath, object? Dto = null, ApiMethod am = ApiMethod.Get)
        {
            if (hc == null || hc.BaseAddress == null) throw new NullReferenceException(nameof(hc));

            ReturnResultType<T> rrt = new();
            try
            {
                HttpResponseMessage rp;

                if (RouterPath.IndexOf("__jt=") == -1 && (am == ApiMethod.Get || am == ApiMethod.Post))
                {
                    RouterPath += RouterPath.IndexOf("?") == -1 ? "?" : "&";
                    RouterPath += $"__jt={DateTime.Now.Ticks}";
                }
                // 核心：使用 HttpRequestMessage 确保绝对不带任何全局的 Authorization 默认头
                var request = new HttpRequestMessage(GetHttpMethod(am), RouterPath);

                if (Dto != null && (am == ApiMethod.Post || am == ApiMethod.Put))
                {
                    request.Content = JsonContent.Create(Dto);
                }

                rp = await hc.SendAsync(request);

                if (rp.IsSuccessStatusCode)
                {
                    var jsonString = await rp.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiDynamicResult>(jsonString);
                    if (result == null || !result.Succeeded)
                    {
                    cx: throw new Exception(result?.Errors?.ToString(), new Exception(JsonConvert.SerializeObject(result)));
                    }
                    var tmprrt = result?.Data?.ToObject<ReturnResultType<T>>();
                    if (tmprrt == null) throw new Exception("非ReturnResultType类型");

                    rrt = tmprrt;
                }
                else
                {
                    throw new Exception($"请求失败，状态码: {(int)rp.StatusCode}", new Exception(JsonConvert.SerializeObject(rp)));
                }
            }
            catch (Exception ex)
            {
                rrt.RESULT = 0;
                rrt.Message = ex.Message;
                rrt.ReturnObj = default!;
            }

            return rrt;
        }



        /// <summary>
        /// WebApi动态调用方法，【注意】：对于Dto入参的类将使用Newtonsoft 库进行序列化
        /// </summary>
        /// <param name="hc">已初始化基uri的httpclient，并初化Auth验证</param>
        /// <param name="RouterPath">路由路径</param>
        /// <param name="Dto">入参类</param>
        /// <param name="am">访问方法</param>
        /// <param name="IsNotUseToken">不使用访问令牌（兼容）</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public static async Task<ReturnResultType<T>> CallWebApiMethodAsync<T>(HttpClient hc, string RouterPath, object? Dto = null, ApiMethod am = ApiMethod.Get, bool IsNotUseToken = false)
        {

            if (IsNotUseToken)
            {
                return await CallWebApiMethodWithoutTokenAsync<T>(hc, RouterPath, Dto, am);
            }


            if (RouterPath.IndexOf("__jt=") == -1 && (am == ApiMethod.Get || am == ApiMethod.Post))
            {
                RouterPath += RouterPath.IndexOf("?") == -1 ? "?" : "&";
                RouterPath += $"__jt={DateTime.Now.Ticks}";
            }

            if (hc == null || hc.BaseAddress == null) throw new NullReferenceException(nameof(hc));
            HttpResponseMessage rp = default!;

            HttpContent content = null!;
            if (Dto != null)
            {
                string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(Dto);
                content = new StringContent(jsonStr, System.Text.Encoding.UTF8, "application/json");
            }


            ReturnResultType<T> rrt = new();
            try
            {

                if (am == ApiMethod.Post)
                {



                    rp = await hc.PostAsync(RouterPath, content);
                }
                else if (am == ApiMethod.Get)
                {
                    rp = await hc.GetAsync(RouterPath);
                }
                else if (am == ApiMethod.Put)
                {
                    if (Dto == null)
                    {
                        rp = await hc.PutAsync(RouterPath, null);

                    }
                    else
                    {
                        rp = await hc.PutAsync(RouterPath, content);
                    }

                }
                else
                {
                    rp = await hc.DeleteAsync(RouterPath);
                }


                if (rp.IsSuccessStatusCode)
                {
                    var jsonString = await rp.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiDynamicResult>(jsonString);
                    if (result == null || !result.Succeeded)
                    {
                        throw new Exception(result?.Errors?.ToString(), new Exception(JsonConvert.SerializeObject(result)));

                    }

                    var tmprrt = result?.Data?.ToObject<ReturnResultType<T>>();
                    if (tmprrt == null) throw new Exception("非ReturnResultType类型");

                    rrt = tmprrt;




                }
                else
                {
                    if (rp.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new Exception($"登录失败或凭证已过期，请重新登录，code：{(int)rp.StatusCode}", new Exception(JsonConvert.SerializeObject(rp)));
                    }

                    throw new Exception($"登录失败，请检查账号密码或服务器地址，code：{(int)rp.StatusCode}", new Exception(JsonConvert.SerializeObject(rp)));
                }
            }
            catch (Exception ex)
            {
                rrt.Failure = (int)rp?.StatusCode;
                rrt.RESULT = 0;
                rrt.Message = ex.ToString();
                rrt.ReturnObj = default!;
            }


            return rrt;

        }

        /// <summary>
        /// 文字转义HTML
        /// </summary>
        /// <param name="sText"></param>
        /// <returns></returns>
        public static string HTMLEncode(this string sText)
        {
            string htmlEncoded = WebUtility.HtmlEncode(sText);
            htmlEncoded = htmlEncoded.Replace(" ", "&nbsp;");
            return htmlEncoded;

        }

        /// <summary>
        /// HTML转义文字
        /// </summary>
        /// <param name="sHTML"></param>
        /// <returns></returns>
        public static string HTMLDeCode(this string sHTML)
        {
            string st = WebUtility.HtmlDecode(sHTML);
            st = st.Replace("&nbsp;", " ");
            return st;

        }


        /// <summary>
        /// 数字转在中文数字
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string ToCNName(this int d)
        {
            if (d < 0) return "";
            char[] n = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
            char[] s = ['零', '一', '二', '三', '四', '五', '六', '七', '八', '九'];
            string str = d.ToString();
            string sv = "";
            foreach (char c in str)
            {
                sv += s[Array.IndexOf(n, c)];
            }
            return sv;
        }




        /// <summary>
        /// 【擴充方法】將 System.Drawing.Color 完美轉換為網頁標準的 HEX 字串（如 #1BD1A5）
        /// </summary>
        /// <param name="color">System.Drawing.Color 實例</param>
        /// <param name="includeAlpha">是否包含透明度通道（預設不包含，返回 6 碼 #RRGGBB）</param>
        public static string ToHex(this Color color, bool ForceNotAlpha = false, bool IsToWeb = true)
        {
            if (IsToWeb)
            {

                if (color.A < 255 && ForceNotAlpha == false)
                {
                    // CSS 标准：#RRGGBBAA
                    return $"#{color.R:X2}{color.G:X2}{color.B:X2}{color.A:X2}";
                }
                else
                {
                    return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
                }
            }
            else
            {
                if (color.A < 255 && ForceNotAlpha == false)
                {
                    // CSS 标准：#RRGGBBAA
                    return $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
                }
                else
                {
                    return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
                }
            }


        }


        /// <summary>
        /// 获得枚举类型的Categor（要求是JSON)格式
        /// </summary>
        /// <param name="en"></param>
        /// <param name="IsDebarMixedValue">排除附加值的混合值</param>
        /// <returns>返回带个JObject 的字典</returns>
        public static Dictionary<Enum, JObject> GetEnumCategoryDC(this Enum en, bool IsDebarMixedValue = true)
        {
            Dictionary<Enum, JObject> dc = new();
            Type et = en.GetType();
            foreach (Enum testEnum in Enum.GetValues(et))
            {


                UInt64 ev = Convert.ToUInt64(testEnum);
                bool b = ev != 0 && (ev & (ev - 1)) != 0;//是否为混合值 
                if (en.HasFlag(testEnum) && ev != 0) //只取符合整除的则排除混合值 
                {
                    if (b && IsDebarMixedValue) continue;
                    // 获取枚举字段信息
                    FieldInfo field = et.GetField(testEnum.ToString());

                    // 获取 CategoryAttribute 特性
                    var categoryAttr = field?.GetCustomAttribute<CategoryAttribute>();

                    if (categoryAttr != null)
                    {
                        string categoryRaw = categoryAttr.Category;
                        var jt = ConvertJSON(categoryRaw);
                        if (jt != null && jt.Type == JTokenType.Object)
                        {
                            dc.Add(testEnum, (JObject)jt);
                        }


                    }

                }
            }

            return dc;

        }

        /// <summary>
        /// 获得标志（Flags）枚举实例中包含多少个具体的枚举值集合。
        /// </summary>
        /// <param name="enumConstant">枚举实例常量。</param>
        /// <returns>返回包含的枚举整型数值集合。</returns>
        public static List<UInt64> GetEnumValues(this Enum enumConstant)
        {
            var iL = new List<UInt64>();
            UInt64 constantValue = Convert.ToUInt64(enumConstant);

            foreach (var testEnum in Enum.GetValues(enumConstant.GetType()))
            {
                var testValue = Convert.ToUInt64(testEnum);
                // 模拟 VB.NET 的位与运算 (EnumConstant And testEnum)
                if ((constantValue & testValue) == testValue && testValue != 0)
                {
                    iL.Add(testValue);
                }
            }

            return iL;
        }

        /// <summary>
        /// 返回枚举类型的说明和对应数值的字典集合。
        /// </summary>
        /// <param name="enumType">枚举的运行时类型，可用 <c>typeof(EnumType)</c> 获取。</param>
        /// <param name="isGetUserHide">是否获取用户隐藏的枚举项，默认为 <see langword="false"/>。</param>
        /// <param name="isRemoveHtml">Ref 开关：是否移除说明中的 HTML 标签，默认为 <see langword="false"/>。</param>
        /// <returns>返回以枚举描述为键、枚举整型数值为值的字典集合。</returns>
        public static Dictionary<string, UInt64> EnumToDict(Type enumType, bool isGetUserHide = false, bool isRemoveHtml = false)
        {
            var dc = new Dictionary<string, UInt64>();

            foreach (Enum testEnum in Enum.GetValues(enumType))
            {
                // 注意：此处调用了下面定义的扩展方法
                string sN = testEnum.GetEnumDescription(opEQ: true);
                if (isRemoveHtml)
                {
                    sN = RemoveHtmlTag(sN);
                }

                FieldInfo field = enumType.GetField(testEnum.ToString());
                var cd = (CategoryAttribute[])field.GetCustomAttributes(typeof(CategoryAttribute), false);
                string sJSON = cd.Length > 0 ? cd[0].Category : "";
                var iv = Convert.ToUInt64(testEnum);


                if (!string.IsNullOrWhiteSpace(sJSON))
                {
                    JObject jO = JsonConvert.DeserializeObject<JObject>(sJSON);
                    string ivStr = jO["IsUserHide"]?.ToString();

                    if (!int.TryParse(ivStr, out int ih))
                    {
                        ih = 0;
                    }

                    if (ih == 0 && isGetUserHide)
                    {
                        dc.Add(sN, iv);
                    }
                    else if (iv > 0)
                    {
                        dc.Add(sN, iv);
                    }
                }
                else
                {
                    dc.Add(sN, iv);
                }
            }

            return dc;
        }

        /// <summary>
        /// 返回当前标志枚举实例中所包含的有效枚举项的说明和数值字典。
        /// </summary>
        /// <param name="enumConstant">枚举实例常量。</param>
        /// <returns>返回符合位运算匹配的枚举描述与数值字典。</returns>
        public static Dictionary<string, UInt64> GetEnumDict(this Enum enumConstant)
        {
            var dc = new Dictionary<string, UInt64>();

            foreach (Enum testEnum in Enum.GetValues(enumConstant.GetType()))
            {
                if (enumConstant.HasFlag(testEnum) && Convert.ToInt32(testEnum) != 0)
                {
                    string sN = testEnum.GetEnumDescription(opEQ: true);
                    dc.Add(sN, Convert.ToUInt64(testEnum));
                }
            }

            return dc;
        }

        /// <summary>
        /// 获得枚举的说明，可分权重（Level）进行排序或过滤显示。
        /// </summary>
        /// <param name="enumConstant">枚举实例常量。</param>
        /// <param name="splitChar">分隔符，默认项为逗号。</param>
        /// <param name="isReturnMaxLevelOnly">是否仅返回最大权重值。可在枚举项上使用 <c>[Category("{'level':2}")]</c> 确定权重。若为 <see langword="false"/> 则返回全部有效说明。</param>
        /// <param name="includeZero">是否附加包含零（0）的说明。若为 <see langword="false"/>，则只有在枚举值本身为 0 时才显示默认说明。</param>
        /// <param name="hasValue">显示内容是否包含数据值前缀（例如：1.说明）。</param>
        /// <param name="opEQ">是否使用相等操作符判定。若为 <see langword="true"/> 则使用等于判定，默认为 <see langword="false"/>（即使用位运算）。</param>
        /// <param name="orderbyLevelDesc">是否按照 JSON 配置中的 LEVEL 属性进行降序排序。</param>
        /// <returns>返回组合后的枚举描述字符串。</returns>
        public static string GetEnumDescription(
            this Enum enumConstant,
            string splitChar = ",",
            bool isReturnMaxLevelOnly = false,
            bool includeZero = false,
            bool hasValue = false,
            bool opEQ = false,
            bool orderbyLevelDesc = false)
        {
            var iL = new List<Enum>();
            var constantValue = Convert.ToUInt64(enumConstant);
            Type enumType = enumConstant.GetType();

            if (constantValue != 0)
            {
                foreach (Enum testEnum in Enum.GetValues(enumType))
                {
                    var testValue = Convert.ToUInt64(testEnum);
                    if (opEQ)
                    {
                        if (Equals(enumConstant, testEnum)) iL.Add(testEnum);
                    }
                    else
                    {
                        if ((constantValue & testValue) == testValue && testValue != 0) iL.Add(testEnum);
                    }
                }
            }
            else if (includeZero)
            {
                FieldInfo field = enumType.GetField(enumConstant.ToString());
                if (field != null)
                {
                    var attr = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    return attr.Length > 0 ? attr[0].Description : enumConstant.ToString();
                }
                return enumConstant.ToString();
            }

            if (orderbyLevelDesc)
            {
                var dc = new Dictionary<Enum, string>();
                foreach (var ev in iL)
                {
                    FieldInfo field = enumType.GetField(ev.ToString());
                    var cd = (CategoryAttribute[])field.GetCustomAttributes(typeof(CategoryAttribute), false);
                    string sJSON = cd.Length > 0 ? cd[0].Category : "";

                    dc.Add(ev, "");
                    JToken jt = ConvertJson(sJSON);
                    if (jt != null && jt.Type == JTokenType.Object)
                    {
                        var jO = (JObject)jt;
                        if (jO.ContainsKey("level"))
                        {
                            dc[ev] = jO["level"]?.ToString() ?? "";
                        }
                    }
                }

                if (dc.Count > 0)
                {
                    var rs = dc.OrderByDescending(kv => kv.Value).ToList();
                    iL = rs.Select(kv => kv.Key).ToList();
                }
            }

            string s = "";
            int iMax = 0;
            Enum eMax = enumConstant;

            foreach (var ev in iL)
            {
                FieldInfo field = enumType.GetField(ev.ToString());
                var attr = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                string sNV = attr.Length > 0 ? attr[0].Description : ev.ToString();

                if (hasValue && !string.IsNullOrEmpty(sNV))
                {
                    // 转换原 VB XML 节点解析逻辑
                    XDocument xD = XDocument.Parse(sNV);
                    XElement xE = xD.Root; // 原 VB FirstNode 对应 Root 节点安全读取
                    if (xE != null)
                    {
                        xE.Value = Convert.ToInt32(ev) + "." + xE.Value;
                        sNV = xE.ToString();
                    }
                }

                s += splitChar + sNV;

                if (isReturnMaxLevelOnly)
                {
                    var cd = (CategoryAttribute[])field.GetCustomAttributes(typeof(CategoryAttribute), false);
                    string sJSON = cd.Length > 0 ? cd[0].Category : "";
                    if (!string.IsNullOrWhiteSpace(sJSON))
                    {
                        JObject jO = JsonConvert.DeserializeObject<JObject>(sJSON);
                        string ivStr = jO["level"]?.ToString();
                        if (int.TryParse(ivStr, out int iv))
                        {
                            if (iMax < iv)
                            {
                                iMax = iv;
                                eMax = ev;
                            }
                        }
                    }
                }
            }

            if (isReturnMaxLevelOnly)
            {
                return eMax.GetEnumDescription(splitChar, false, includeZero, hasValue, opEQ, false);
            }

            if (!string.IsNullOrEmpty(s))
            {
                s = s.Substring(splitChar.Length);
            }

            return s;
        }

        #region 外部依赖占位方法（请根据您项目中实际的工具类进行对接）

        /// <summary>
        /// 移除字符串中的 HTML 标签。
        /// </summary>
        private static string RemoveHtmlTag(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            // 示例：此处应当使用您的现有公共库，或使用正则过滤
            return System.Text.RegularExpressions.Regex.Replace(input, "<[^>]*>", "");
        }

        /// <summary>
        /// 将字符串安全转换为 JToken。
        /// </summary>
        private static JToken? ConvertJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            try
            {
                return JToken.Parse(json);
            }
            catch
            {
                return null;
            }
        }

        #endregion



        /// <summary>
        /// 如果转换失败将会返回Guid.Empty
        /// </summary>
        /// <param name="sv">要转换的字符串</param>
        /// <returns></returns>
        public static Guid ConvertGUID(string sv)
        {

            if (string.IsNullOrWhiteSpace(sv)) return Guid.Empty;
            if (Guid.TryParse(sv.Trim(), out Guid g))
            {
                return g;
            }

            return Guid.Empty;
        }

    }
}
