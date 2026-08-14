namespace CommLib
{
    /// <summary>
    /// 通用返回类
    /// </summary>
    public class ReturnResultType
    {
        private string? _message;
        private string? _url;

        public int RESULT { get; set; } = 0;   // 非0為成功，0為失敗
        public int Success { get; set; } = 0;  // 成功數量
        public int Failure { get; set; } = 0;  // 失敗數量

        // 使用屬性封裝，確保不論如何初始化，絕對不會返回 null
        public string Message
        {
            get => _message ?? "";
            set => _message = value;
        }

        public object? ReturnObj { get; set; } = default; // 附帶類型

        public string URL
        {
            get => _url ?? "";
            set => _url = value;
        }



        /// <summary>
        /// 提供重置方法：當此類別作為 Scoped 服務注入時，方便在頁面初始化時清空舊狀態
        /// </summary>
        public void Clear()
        {
            RESULT = 0;
            Success = 0;
            Failure = 0;
            _message = null;
            _url = null;
            ReturnObj = default;
        }
    }

    /// <summary>
    /// 泛型版本返回類型（已修改為 Class）
    /// </summary>
    public class ReturnResultType<T>
    {
        private string? _message;
        private string? _url;

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


    }

    // 擴展方法
    public static class ReturnResultTypeEx
    {
        /// <summary>
        /// 擴展方法（加入更安全的轉型保護）
        /// </summary>
        public static ReturnResultType<T> AsTyped<T>(this ReturnResultType rr)
        {
            // 防禦性檢查：如果傳入的實例本身為空，直接返回默認容器
            if (rr == null) return new ReturnResultType<T>();

            T? typedObj = default;

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
                    // 如果轉型失敗，保持 default(T)
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
