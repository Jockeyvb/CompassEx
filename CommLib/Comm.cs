using System;

namespace CommLib
{
    public class Comm
    {

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
