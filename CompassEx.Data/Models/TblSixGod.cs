using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Dapper;
using Dapper.Contrib.Extensions;

namespace CompassEx.Data.Models
{
   

    [Dapper.Contrib.Extensions.Table("tbl_SixGod")]
    public partial class TblSixGod
    {
        [Dapper.Contrib.Extensions.Key]
        public int SixGodId { get; set; }
        public DateTime CreateTime { get; set; }
        public string? Other { get; set; }
        public byte[] SixGodClassId { get; set; }
        public string SixGodInfo { get; set; }
        public int SixRelativeClassId { get; set; }

       #region 💡 萬能自動 OOP 資料庫操作方法 (Active Record)

        public static ReturnResultType Addnew(TblSixGod entity)
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

        public static ReturnResultType<TblSixGod> GetTblSixGod(string where = "", List<(string name,object value)> nameValue = null, string orderBy = "")
        {
            var rrt = new ReturnResultType<TblSixGod>();
            try
            {
                var colResult = GetTblSixGodCol(where, nameValue, orderBy, 1, 1);
                
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

        public static ReturnResultType<List<TblSixGod>> GetTblSixGodCol(string where = "", List<(string name,object value)> nameValue = null, string orderBy = "" , int pageIndex = 0, int pageSize = 10)
        {
            var rrt = new ReturnResultType<List<TblSixGod>>();
            
            try
            {
                var sqlBuilder = new System.Text.StringBuilder();
                sqlBuilder.Append("SELECT * FROM [tbl_SixGod] WHERE 1=1");

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

                if (string.IsNullOrWhiteSpace(orderBy))
                {
                    orderBy = "CreateTime DESC";
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
                    var list = db.Query<TblSixGod>(sqlBuilder.ToString(), dp).ToList();
                    rrt.ReturnObj = list;
                    rrt.RESULT = 1;
                    rrt.Success = list.Count;
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
            vm.SixGodId = this.SixGodId;
            vm.CreateTime = this.CreateTime;
            vm.Other = this.Other;
            vm.SixGodClassId = this.SixGodClassId;
            vm.SixGodInfo = this.SixGodInfo;
            vm.SixRelativeClassId = this.SixRelativeClassId;
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

            private int _sixGodId;
            public int SixGodId
            {
                get => _sixGodId;
                set
                {
                    if (!EqualityComparer<int>.Default.Equals(_sixGodId, value))
                    {
                        _sixGodId = value;
                        OnPropertyChanged();
                    }
                }
            }

            private DateTime _createTime;
            public DateTime CreateTime
            {
                get => _createTime;
                set
                {
                    if (!EqualityComparer<DateTime>.Default.Equals(_createTime, value))
                    {
                        _createTime = value;
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

            private byte[] _sixGodClassId;
            public byte[] SixGodClassId
            {
                get => _sixGodClassId;
                set
                {
                    if (!EqualityComparer<byte[]>.Default.Equals(_sixGodClassId, value))
                    {
                        _sixGodClassId = value;
                        OnPropertyChanged();
                    }
                }
            }

            private string _sixGodInfo;
            public string SixGodInfo
            {
                get => _sixGodInfo;
                set
                {
                    if (!EqualityComparer<string>.Default.Equals(_sixGodInfo, value))
                    {
                        _sixGodInfo = value;
                        OnPropertyChanged();
                    }
                }
            }

            private int _sixRelativeClassId;
            public int SixRelativeClassId
            {
                get => _sixRelativeClassId;
                set
                {
                    if (!EqualityComparer<int>.Default.Equals(_sixRelativeClassId, value))
                    {
                        _sixRelativeClassId = value;
                        OnPropertyChanged();
                    }
                }
            }

            public TblSixGod ToModel()
            {
                return new TblSixGod
                {
                    SixGodId = this.SixGodId,
                    CreateTime = this.CreateTime,
                    Other = this.Other,
                    SixGodClassId = this.SixGodClassId,
                    SixGodInfo = this.SixGodInfo,
                    SixRelativeClassId = this.SixRelativeClassId,
                };
            }
        }
    }
}
