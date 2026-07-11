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
   

    [Dapper.Contrib.Extensions.Table("tbl_PersonPower")]
    public partial class TblPersonPower
    {
        [Dapper.Contrib.Extensions.Key]
        public Guid PersonPowerId { get; set; }
        public DateTime CreateTime { get; set; }
        public int IsOpened { get; set; }
        public string? Other { get; set; }
        public int PersonCompassCount { get; set; }
        public byte[] PersonEightWordCount { get; set; }
        public byte[] PersonGuoCount { get; set; }
        public int PowerLevel { get; set; }
        public string PowerName { get; set; }

       #region 💡 萬能自動 OOP 資料庫操作方法 (Active Record)

        public static ReturnResultType Addnew(TblPersonPower entity)
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

        public static ReturnResultType<TblPersonPower> GetTblPersonPower(string where = "", List<(string name,object value)> nameValue = null, string orderBy = "")
        {
            var rrt = new ReturnResultType<TblPersonPower>();
            try
            {
                var colResult = GetTblPersonPowerCol(where, nameValue, orderBy, 1, 1);
                
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

        public static ReturnResultType<List<TblPersonPower>> GetTblPersonPowerCol(string where = "", List<(string name,object value)> nameValue = null, string orderBy = "" , int pageIndex = 0, int pageSize = 10)
        {
            var rrt = new ReturnResultType<List<TblPersonPower>>();
            
            try
            {
                var sqlBuilder = new System.Text.StringBuilder();
                sqlBuilder.Append("SELECT * FROM [tbl_PersonPower] WHERE 1=1");

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
                    var list = db.Query<TblPersonPower>(sqlBuilder.ToString(), dp).ToList();
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
            vm.PersonPowerId = this.PersonPowerId;
            vm.CreateTime = this.CreateTime;
            vm.IsOpened = this.IsOpened;
            vm.Other = this.Other;
            vm.PersonCompassCount = this.PersonCompassCount;
            vm.PersonEightWordCount = this.PersonEightWordCount;
            vm.PersonGuoCount = this.PersonGuoCount;
            vm.PowerLevel = this.PowerLevel;
            vm.PowerName = this.PowerName;
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

            private Guid _personPowerId;
            public Guid PersonPowerId
            {
                get => _personPowerId;
                set
                {
                    if (!EqualityComparer<Guid>.Default.Equals(_personPowerId, value))
                    {
                        _personPowerId = value;
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

            private int _isOpened;
            public int IsOpened
            {
                get => _isOpened;
                set
                {
                    if (!EqualityComparer<int>.Default.Equals(_isOpened, value))
                    {
                        _isOpened = value;
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

            private int _personCompassCount;
            public int PersonCompassCount
            {
                get => _personCompassCount;
                set
                {
                    if (!EqualityComparer<int>.Default.Equals(_personCompassCount, value))
                    {
                        _personCompassCount = value;
                        OnPropertyChanged();
                    }
                }
            }

            private byte[] _personEightWordCount;
            public byte[] PersonEightWordCount
            {
                get => _personEightWordCount;
                set
                {
                    if (!EqualityComparer<byte[]>.Default.Equals(_personEightWordCount, value))
                    {
                        _personEightWordCount = value;
                        OnPropertyChanged();
                    }
                }
            }

            private byte[] _personGuoCount;
            public byte[] PersonGuoCount
            {
                get => _personGuoCount;
                set
                {
                    if (!EqualityComparer<byte[]>.Default.Equals(_personGuoCount, value))
                    {
                        _personGuoCount = value;
                        OnPropertyChanged();
                    }
                }
            }

            private int _powerLevel;
            public int PowerLevel
            {
                get => _powerLevel;
                set
                {
                    if (!EqualityComparer<int>.Default.Equals(_powerLevel, value))
                    {
                        _powerLevel = value;
                        OnPropertyChanged();
                    }
                }
            }

            private string _powerName;
            public string PowerName
            {
                get => _powerName;
                set
                {
                    if (!EqualityComparer<string>.Default.Equals(_powerName, value))
                    {
                        _powerName = value;
                        OnPropertyChanged();
                    }
                }
            }

            public TblPersonPower ToModel()
            {
                return new TblPersonPower
                {
                    PersonPowerId = this.PersonPowerId,
                    CreateTime = this.CreateTime,
                    IsOpened = this.IsOpened,
                    Other = this.Other,
                    PersonCompassCount = this.PersonCompassCount,
                    PersonEightWordCount = this.PersonEightWordCount,
                    PersonGuoCount = this.PersonGuoCount,
                    PowerLevel = this.PowerLevel,
                    PowerName = this.PowerName,
                };
            }
        }
    }
}
