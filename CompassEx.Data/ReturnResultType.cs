namespace CompassEx.Data
{
    // <summary>
    /// 返回類型
    /// </summary>
    public struct ReturnResultType
    {
        private string _message;
        private string _url;

        public int RESULT { get; set; } = 0;   // 非0為成功，0為失敗
        public int Success { get; set; } = 0;  // 成功數量
        public int Failure { get; set; } = 0;  // 失敗數量

        // 使用屬性封裝，確保不論如何初始化，絕對不會返回 null
        public string Message
        {
            get => _message ?? "";
            set => _message = value;
        }

        public object ReturnObj { get; set; } = default; // 附帶類型

        public string URL
        {
            get => _url ?? "";
            set => _url = value;
        }

        public ReturnResultType()
        {
        }
    }

    /// <summary>
    /// 泛型版本返回類型
    /// </summary>
    public struct ReturnResultType<T>
    {
        private string _message;
        private string _url;

        public int RESULT { get; set; } = 0;
        public int Success { get; set; } = 0;
        public int Failure { get; set; } = 0;

        public string Message
        {
            get => _message ?? "";
            set => _message = value;
        }

        public T ReturnObj { get; set; } = default; // 這裡變成了具體類型 T

        public string URL
        {
            get => _url ?? "";
            set => _url = value;
        }

        public ReturnResultType()
        {
        }
    }

    // 擴展方法
    public static class ReturnResultTypeEx
    {
        /// <summary>
        /// 擴展方法（加入更安全的轉型保護）
        /// </summary>
        public static ReturnResultType<T> AsTyped<T>(this ReturnResultType rr)
        {
            T typedObj = default;

            // 防禦性轉型：只有當 ReturnObj 不為 null 且類型相符時才進行轉換
            if (rr.ReturnObj is T obj)
            {
                typedObj = obj;
            }
            else if (rr.ReturnObj != null)
            {
                try
                {
                    // 嘗試強制轉換（處理一些可以隱式轉換的類型）
                    typedObj = (T)rr.ReturnObj;
                }
                catch
                {
                    // 如果轉型失敗，保持 default(T)，或者您可以在這裡記錄 Log / 拋出異常
                }
            }

            return new ReturnResultType<T>
            {
                RESULT = rr.RESULT,
                Success = rr.Success,
                Failure = rr.Failure,
                Message = rr.Message,
                URL = rr.URL,
                ReturnObj = typedObj
            };
        }
    }
}
