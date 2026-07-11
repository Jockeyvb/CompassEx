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
   

    [Dapper.Contrib.Extensions.Table("tbl_PersonGuo")]
    public partial class TblPersonGuo
    {
        [Dapper.Contrib.Extensions.Key]
        public string PersonGuoId { get; set; }
        public DateTime? AsyncTime { get; set; }
        public DateTime? ChangeTime { get; set; }
        public DateTime CreateTime { get; set; }
        public string GuoYaos { get; set; }
        public string? Info { get; set; }
        public string InvitePersonId { get; set; }
        public int InvitePoint { get; set; }
        public int? IsFinished { get; set; }
        public int IsInvite { get; set; }
        public int IsUploaded { get; set; }
        public DateTime? NowDate { get; set; }
        public string? Other { get; set; }
        public string PersonId { get; set; }
        public string Question { get; set; }
        public string SkyLoc { get; set; }

       #region 💡 萬能自動 OOP 資料庫操作方法 (Active Record)

        public static ReturnResultType Addnew(TblPersonGuo entity)
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

        public static ReturnResultType<TblPersonGuo> GetTblPersonGuo(string where = "", List<(string name,object value)> nameValue = null, string orderBy = "")
        {
            var rrt = new ReturnResultType<TblPersonGuo>();
            try
            {
                var colResult = GetTblPersonGuoCol(where, nameValue, orderBy, 1, 1);
                
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

        public static ReturnResultType<List<TblPersonGuo>> GetTblPersonGuoCol(string where = "", List<(string name,object value)> nameValue = null, string orderBy = "" , int pageIndex = 0, int pageSize = 10)
        {
            var rrt = new ReturnResultType<List<TblPersonGuo>>();
            
            try
            {
                var sqlBuilder = new System.Text.StringBuilder();
                sqlBuilder.Append("SELECT * FROM [tbl_PersonGuo] WHERE 1=1");

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
                    var list = db.Query<TblPersonGuo>(sqlBuilder.ToString(), dp).ToList();
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
            vm.PersonGuoId = this.PersonGuoId;
            vm.AsyncTime = this.AsyncTime;
            vm.ChangeTime = this.ChangeTime;
            vm.CreateTime = this.CreateTime;
            vm.GuoYaos = this.GuoYaos;
            vm.Info = this.Info;
            vm.InvitePersonId = this.InvitePersonId;
            vm.InvitePoint = this.InvitePoint;
            vm.IsFinished = this.IsFinished;
            vm.IsInvite = this.IsInvite;
            vm.IsUploaded = this.IsUploaded;
            vm.NowDate = this.NowDate;
            vm.Other = this.Other;
            vm.PersonId = this.PersonId;
            vm.Question = this.Question;
            vm.SkyLoc = this.SkyLoc;
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

            private string _personGuoId;
            public string PersonGuoId
            {
                get => _personGuoId;
                set
                {
                    if (!EqualityComparer<string>.Default.Equals(_personGuoId, value))
                    {
                        _personGuoId = value;
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

            private DateTime? _changeTime;
            public DateTime? ChangeTime
            {
                get => _changeTime;
                set
                {
                    if (!EqualityComparer<DateTime?>.Default.Equals(_changeTime, value))
                    {
                        _changeTime = value;
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

            private string _guoYaos;
            public string GuoYaos
            {
                get => _guoYaos;
                set
                {
                    if (!EqualityComparer<string>.Default.Equals(_guoYaos, value))
                    {
                        _guoYaos = value;
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

            private string _invitePersonId;
            public string InvitePersonId
            {
                get => _invitePersonId;
                set
                {
                    if (!EqualityComparer<string>.Default.Equals(_invitePersonId, value))
                    {
                        _invitePersonId = value;
                        OnPropertyChanged();
                    }
                }
            }

            private int _invitePoint;
            public int InvitePoint
            {
                get => _invitePoint;
                set
                {
                    if (!EqualityComparer<int>.Default.Equals(_invitePoint, value))
                    {
                        _invitePoint = value;
                        OnPropertyChanged();
                    }
                }
            }

            private int? _isFinished;
            public int? IsFinished
            {
                get => _isFinished;
                set
                {
                    if (!EqualityComparer<int?>.Default.Equals(_isFinished, value))
                    {
                        _isFinished = value;
                        OnPropertyChanged();
                    }
                }
            }

            private int _isInvite;
            public int IsInvite
            {
                get => _isInvite;
                set
                {
                    if (!EqualityComparer<int>.Default.Equals(_isInvite, value))
                    {
                        _isInvite = value;
                        OnPropertyChanged();
                    }
                }
            }

            private int _isUploaded;
            public int IsUploaded
            {
                get => _isUploaded;
                set
                {
                    if (!EqualityComparer<int>.Default.Equals(_isUploaded, value))
                    {
                        _isUploaded = value;
                        OnPropertyChanged();
                    }
                }
            }

            private DateTime? _nowDate;
            public DateTime? NowDate
            {
                get => _nowDate;
                set
                {
                    if (!EqualityComparer<DateTime?>.Default.Equals(_nowDate, value))
                    {
                        _nowDate = value;
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

            private string _personId;
            public string PersonId
            {
                get => _personId;
                set
                {
                    if (!EqualityComparer<string>.Default.Equals(_personId, value))
                    {
                        _personId = value;
                        OnPropertyChanged();
                    }
                }
            }

            private string _question;
            public string Question
            {
                get => _question;
                set
                {
                    if (!EqualityComparer<string>.Default.Equals(_question, value))
                    {
                        _question = value;
                        OnPropertyChanged();
                    }
                }
            }

            private string _skyLoc;
            public string SkyLoc
            {
                get => _skyLoc;
                set
                {
                    if (!EqualityComparer<string>.Default.Equals(_skyLoc, value))
                    {
                        _skyLoc = value;
                        OnPropertyChanged();
                    }
                }
            }

            public TblPersonGuo ToModel()
            {
                return new TblPersonGuo
                {
                    PersonGuoId = this.PersonGuoId,
                    AsyncTime = this.AsyncTime,
                    ChangeTime = this.ChangeTime,
                    CreateTime = this.CreateTime,
                    GuoYaos = this.GuoYaos,
                    Info = this.Info,
                    InvitePersonId = this.InvitePersonId,
                    InvitePoint = this.InvitePoint,
                    IsFinished = this.IsFinished,
                    IsInvite = this.IsInvite,
                    IsUploaded = this.IsUploaded,
                    NowDate = this.NowDate,
                    Other = this.Other,
                    PersonId = this.PersonId,
                    Question = this.Question,
                    SkyLoc = this.SkyLoc,
                };
            }
        }
    }
}
