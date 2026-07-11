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
   

    [Dapper.Contrib.Extensions.Table("tbl_Person8WordList")]
    public partial class TblPerson8WordList
    {
        [Dapper.Contrib.Extensions.Key]
        public string Person8WordListId { get; set; }
        public int AcceptPoint { get; set; }
        public string? AnswerPersonName { get; set; }
        public string? AnswerTrueName { get; set; }
        public DateTime? AsyncTime { get; set; }
        public DateTime? CreateTime { get; set; }
        public string Info { get; set; }
        public int IsBestAccept { get; set; }
        public string? Other { get; set; }
        public int Person8WordId { get; set; }
        public string? Person8WordWebId { get; set; }

       #region 💡 萬能自動 OOP 資料庫操作方法 (Active Record)

        public static ReturnResultType Addnew(TblPerson8WordList entity)
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

        public static ReturnResultType<TblPerson8WordList> GetTblPerson8WordList(string where = "", List<(string name,object value)> nameValue = null, string orderBy = "")
        {
            var rrt = new ReturnResultType<TblPerson8WordList>();
            try
            {
                var colResult = GetTblPerson8WordListCol(where, nameValue, orderBy, 1, 1);
                
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

        public static ReturnResultType<List<TblPerson8WordList>> GetTblPerson8WordListCol(string where = "", List<(string name,object value)> nameValue = null, string orderBy = "" , int pageIndex = 0, int pageSize = 10)
        {
            var rrt = new ReturnResultType<List<TblPerson8WordList>>();
            
            try
            {
                var sqlBuilder = new System.Text.StringBuilder();
                sqlBuilder.Append("SELECT * FROM [tbl_Person8WordList] WHERE 1=1");

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
                    var list = db.Query<TblPerson8WordList>(sqlBuilder.ToString(), dp).ToList();
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
            vm.Person8WordListId = this.Person8WordListId;
            vm.AcceptPoint = this.AcceptPoint;
            vm.AnswerPersonName = this.AnswerPersonName;
            vm.AnswerTrueName = this.AnswerTrueName;
            vm.AsyncTime = this.AsyncTime;
            vm.CreateTime = this.CreateTime;
            vm.Info = this.Info;
            vm.IsBestAccept = this.IsBestAccept;
            vm.Other = this.Other;
            vm.Person8WordId = this.Person8WordId;
            vm.Person8WordWebId = this.Person8WordWebId;
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

            private string _person8WordListId;
            public string Person8WordListId
            {
                get => _person8WordListId;
                set
                {
                    if (!EqualityComparer<string>.Default.Equals(_person8WordListId, value))
                    {
                        _person8WordListId = value;
                        OnPropertyChanged();
                    }
                }
            }

            private int _acceptPoint;
            public int AcceptPoint
            {
                get => _acceptPoint;
                set
                {
                    if (!EqualityComparer<int>.Default.Equals(_acceptPoint, value))
                    {
                        _acceptPoint = value;
                        OnPropertyChanged();
                    }
                }
            }

            private string? _answerPersonName;
            public string? AnswerPersonName
            {
                get => _answerPersonName;
                set
                {
                    if (!EqualityComparer<string?>.Default.Equals(_answerPersonName, value))
                    {
                        _answerPersonName = value;
                        OnPropertyChanged();
                    }
                }
            }

            private string? _answerTrueName;
            public string? AnswerTrueName
            {
                get => _answerTrueName;
                set
                {
                    if (!EqualityComparer<string?>.Default.Equals(_answerTrueName, value))
                    {
                        _answerTrueName = value;
                        OnPropertyChanged();
                    }
                }
            }

            private DateTime? _asyncTime;
            public DateTime? AsyncTime
            {
                get => _asyncTime;
                set
                {
                    if (!EqualityComparer<DateTime?>.Default.Equals(_asyncTime, value))
                    {
                        _asyncTime = value;
                        OnPropertyChanged();
                    }
                }
            }

            private DateTime? _createTime;
            public DateTime? CreateTime
            {
                get => _createTime;
                set
                {
                    if (!EqualityComparer<DateTime?>.Default.Equals(_createTime, value))
                    {
                        _createTime = value;
                        OnPropertyChanged();
                    }
                }
            }

            private string _info;
            public string Info
            {
                get => _info;
                set
                {
                    if (!EqualityComparer<string>.Default.Equals(_info, value))
                    {
                        _info = value;
                        OnPropertyChanged();
                    }
                }
            }

            private int _isBestAccept;
            public int IsBestAccept
            {
                get => _isBestAccept;
                set
                {
                    if (!EqualityComparer<int>.Default.Equals(_isBestAccept, value))
                    {
                        _isBestAccept = value;
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

            private int _person8WordId;
            public int Person8WordId
            {
                get => _person8WordId;
                set
                {
                    if (!EqualityComparer<int>.Default.Equals(_person8WordId, value))
                    {
                        _person8WordId = value;
                        OnPropertyChanged();
                    }
                }
            }

            private string? _person8WordWebId;
            public string? Person8WordWebId
            {
                get => _person8WordWebId;
                set
                {
                    if (!EqualityComparer<string?>.Default.Equals(_person8WordWebId, value))
                    {
                        _person8WordWebId = value;
                        OnPropertyChanged();
                    }
                }
            }

            public TblPerson8WordList ToModel()
            {
                return new TblPerson8WordList
                {
                    Person8WordListId = this.Person8WordListId,
                    AcceptPoint = this.AcceptPoint,
                    AnswerPersonName = this.AnswerPersonName,
                    AnswerTrueName = this.AnswerTrueName,
                    AsyncTime = this.AsyncTime,
                    CreateTime = this.CreateTime,
                    Info = this.Info,
                    IsBestAccept = this.IsBestAccept,
                    Other = this.Other,
                    Person8WordId = this.Person8WordId,
                    Person8WordWebId = this.Person8WordWebId,
                };
            }
        }
    }
}
