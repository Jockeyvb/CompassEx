using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CompassEx.Data.Models
{


    [Dapper.Contrib.Extensions.Table("tbl_GoodDay")]
    public partial class TblGoodDay
    {
        [Dapper.Contrib.Extensions.Key]
        public int GoodDayId { get; set; }
        public string DayLoc { get; set; }
        public string? Info { get; set; }
        public int IsGood { get; set; }
        public string Month { get; set; }
        public string? Other { get; set; }

        #region 💡 萬能自動 OOP 資料庫操作方法 (Active Record)

        public static ReturnResultType Addnew(TblGoodDay entity)
        {
            var rrt = new ReturnResultType();
            if (entity == null)
            {
                rrt.RESULT = 0;
                rrt.Message = "傳入的實體不能為空 (ArgumentNullException)。";
                rrt.Failure = 1;
                return rrt;
            }

            try
            {
                using (IDbConnection db = Comm.GetOpenConnection())
                {
                    long id = db.Insert(entity);

                    // 💡 修正：long 型別的識別碼或狀態直接對應為 RESULT 與 Success 數量
                    rrt.RESULT = id > 0 ? (int)id : 0;
                    if (id > 0)
                    {
                        rrt.Success = 1;
                    }
                    else
                    {
                        rrt.Failure = 1;
                        rrt.Message = "新增失敗，未生成有效的識別碼。";
                    }
                }
            }
            catch (Exception ex)
            {
                rrt.RESULT = 0;
                rrt.Failure = 1;
                rrt.Message = Comm.IsRRTErrorDetailed ? ex.ToString() : ex.Message;
            }
            return rrt;
        }

        public ReturnResultType Update()
        {
            var rrt = new ReturnResultType();
            try
            {
                using (IDbConnection db = Comm.GetOpenConnection())
                {
                    bool isSuccess = db.Update(this);

                    // 💡 修正：bool 結果直接映射為 RESULT、Success 或 Failure
                    rrt.RESULT = isSuccess ? 1 : 0;
                    if (isSuccess)
                    {
                        rrt.Success = 1;
                    }
                    else
                    {
                        rrt.Failure = 1;
                        rrt.Message = "更新失敗，可能該資料已被刪除或未發生變更。";
                    }
                }
            }
            catch (Exception ex)
            {
                rrt.RESULT = 0;
                rrt.Failure = 1;
                rrt.Message = Comm.IsRRTErrorDetailed ? ex.ToString() : ex.Message;
            }
            return rrt;
        }

        public ReturnResultType Delete()
        {
            var rrt = new ReturnResultType();
            try
            {
                using (IDbConnection db = Comm.GetOpenConnection())
                {
                    bool isSuccess = db.Delete(this);

                    // 💡 修正：bool 結果直接映射為 RESULT、Success 或 Failure
                    rrt.RESULT = isSuccess ? 1 : 0;
                    if (isSuccess)
                    {
                        rrt.Success = 1;
                    }
                    else
                    {
                        rrt.Failure = 1;
                        rrt.Message = "刪除失敗，找不到指定的資料紀錄。";
                    }
                }
            }
            catch (Exception ex)
            {
                rrt.RESULT = 0;
                rrt.Failure = 1;
                rrt.Message = Comm.IsRRTErrorDetailed ? ex.ToString() : ex.Message;
            }
            return rrt;
        }

        #endregion

        #region 💡 萬能進階動態查詢方法

        public static ReturnResultType<TblGoodDay> GetTblGoodDay(string where = "", List<(string name, object value)> nameValue = null, string orderBy = "")
        {
            var rrt = new ReturnResultType<TblGoodDay>();
            try
            {
                var colResult = GetTblGoodDayCol(where, nameValue, orderBy, 1, 1).Result;

                if (colResult.RESULT == 0)
                {
                    rrt.RESULT = 0;
                    rrt.Failure = 1;
                    rrt.Message = colResult.Message;
                    return rrt;
                }

                var item = colResult.ReturnObj?.FirstOrDefault();
                if (item != null)
                {
                    rrt.ReturnObj = item;
                    rrt.RESULT = 1;
                    rrt.Success = 1;
                }
                else
                {
                    rrt.RESULT = 0;
                    rrt.Failure = 1;
                    rrt.Message = "未找到符合條件的資料。";
                }
            }
            catch (Exception ex)
            {
                rrt.RESULT = 0;
                rrt.Failure = 1;
                rrt.Message = Comm.IsRRTErrorDetailed ? ex.ToString() : ex.Message;
            }
            return rrt;
        }

        public static async Task<ReturnResultType<List<TblGoodDay>>> GetTblGoodDayCol(string where = "", List<(string name, object value)> nameValue = null, string orderBy = "", int pageIndex = 0, int pageSize = 10)
        {
            var rrt = new ReturnResultType<List<TblGoodDay>>();

            try
            {
                var sqlBuilder = new System.Text.StringBuilder();
                sqlBuilder.Append("SELECT * FROM [tbl_GoodDay] WHERE 1=1");

                if (!string.IsNullOrWhiteSpace(where))
                {
                    string cleanWhere = where.Trim();
                    if (!cleanWhere.StartsWith("AND", StringComparison.OrdinalIgnoreCase) && !cleanWhere.StartsWith("OR", StringComparison.OrdinalIgnoreCase))
                    {
                        sqlBuilder.Append(" AND ");
                    }
                    else
                    {
                        sqlBuilder.Append(" ");
                    }
                    sqlBuilder.Append(cleanWhere);
                }


                if (!string.IsNullOrWhiteSpace(orderBy))
                {
                    string cleanOrder = orderBy.Trim();
                    if (cleanOrder.StartsWith("order by", StringComparison.OrdinalIgnoreCase))
                    {
                        cleanOrder = cleanOrder.Substring(8).Trim();
                    }
                    sqlBuilder.Append(" ORDER BY " + cleanOrder);
                }

                if (pageIndex > 0 && pageSize > 0)
                {
                    sqlBuilder.Append($" LIMIT {pageSize} OFFSET {(pageIndex - 1) * pageSize}");
                }

                var dp = new DynamicParameters();
                if (nameValue != null && nameValue.Count > 0)
                {
                    foreach (var p in nameValue)
                    {
                        dp.Add(p.name, p.value);
                    }
                }

                using (IDbConnection db = Comm.GetOpenConnection())
                {
                    // 如果连接支持 DbConnection，强转后使用 OpenAsync
                    if (db is System.Data.Common.DbConnection dbAsync)
                    {
                        if (dbAsync.State != ConnectionState.Open) await dbAsync.OpenAsync();
                    }

                    // 💡 2. 关键：Dapper 的 ExecuteReader 必须改用异步版本 ExecuteReaderAsync 并且加上 await！
                    // 这样底层驱动的任何 I/O 崩溃、断线，都会被安全捕获，绝对不中断！
                    using (var reader = await db.ExecuteReaderAsync(sqlBuilder.ToString(), dp))
                    {
                        var list = new List<TblGoodDay>();
                        var rowParser = reader.GetRowParser<TblGoodDay>();
                        while (reader.Read())
                        {
                            list.Add(rowParser(reader));
                        }
                        rrt.ReturnObj = list;
                        rrt.RESULT = 1;
                        rrt.Success = list.Count;
                    }
                }
            }
            catch (Exception ex)
            {
                rrt.RESULT = 0;
                rrt.Failure = 1;
                rrt.Message = Comm.IsRRTErrorDetailed ? ex.ToString() : ex.Message;

            }

            return rrt;
        }



        #endregion

        #region 💡 工廠方法：轉換為 100% 相容的內部 ViewModel

        public ViewModel GetVM()
        {
            var vm = new ViewModel();
            vm.GoodDayId = this.GoodDayId;
            vm.DayLoc = this.DayLoc;
            vm.Info = this.Info;
            vm.IsGood = this.IsGood;
            vm.Month = this.Month;
            vm.Other = this.Other;
            return vm;
        }

        #endregion

        /// <summary>
        /// 💡 專門給 MAUI UI 綁定的視圖模型類別 (巢狀設計)
        /// </summary>
        public class ViewModel : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            private int _goodDayId;
            public int GoodDayId
            {
                get => _goodDayId;
                set
                {
                    if (!EqualityComparer<int>.Default.Equals(_goodDayId, value))
                    {
                        _goodDayId = value;
                        OnPropertyChanged();
                    }
                }
            }

            private string _dayLoc;
            public string DayLoc
            {
                get => _dayLoc;
                set
                {
                    if (!EqualityComparer<string>.Default.Equals(_dayLoc, value))
                    {
                        _dayLoc = value;
                        OnPropertyChanged();
                    }
                }
            }

            private string? _info;
            public string? Info
            {
                get => _info;
                set
                {
                    if (!EqualityComparer<string?>.Default.Equals(_info, value))
                    {
                        _info = value;
                        OnPropertyChanged();
                    }
                }
            }

            private int _isGood;
            public int IsGood
            {
                get => _isGood;
                set
                {
                    if (!EqualityComparer<int>.Default.Equals(_isGood, value))
                    {
                        _isGood = value;
                        OnPropertyChanged();
                    }
                }
            }

            private string _month;
            public string Month
            {
                get => _month;
                set
                {
                    if (!EqualityComparer<string>.Default.Equals(_month, value))
                    {
                        _month = value;
                        OnPropertyChanged();
                    }
                }
            }

            private string? _other;
            public string? Other
            {
                get => _other;
                set
                {
                    if (!EqualityComparer<string?>.Default.Equals(_other, value))
                    {
                        _other = value;
                        OnPropertyChanged();
                    }
                }
            }

            public TblGoodDay ToModel()
            {
                return new TblGoodDay
                {
                    GoodDayId = this.GoodDayId,
                    DayLoc = this.DayLoc,
                    Info = this.Info,
                    IsGood = this.IsGood,
                    Month = this.Month,
                    Other = this.Other,
                };
            }
        }
    }
}
