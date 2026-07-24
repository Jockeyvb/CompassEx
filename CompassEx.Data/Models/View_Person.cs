using FreeSql.DatabaseModel;using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using FreeSql; 
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace CompassEx.Data.Models {

	[JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true)]
	public partial class View_Person {

		[JsonProperty, Column(StringLength = -2)]
		public virtual  string PersonID { get; set; }

		[JsonProperty]
		public virtual  int? PersonCompassCount { get; set; }

		[JsonProperty]
		public virtual  int? PersonGuoCount { get; set; }

		[JsonProperty]
		public virtual  int? PersonEightWordCount { get; set; }

		[JsonProperty, Column(StringLength = -2)]
		public virtual  string PowerName { get; set; }

		[JsonProperty]
		public virtual  DateTime? AsyncTime { get; set; }

		[JsonProperty, Column(StringLength = -2)]
		public virtual  string MD5 { get; set; }

		[JsonProperty, Column(StringLength = -2)]
		public virtual  string Other { get; set; }

		[JsonProperty, Column(StringLength = -2)]
		public virtual  string WeChat { get; set; }

		[JsonProperty]
		public virtual  int? IsDefault { get; set; }

		[JsonProperty]
		public virtual  int? PowerLevel { get; set; }

		[JsonProperty]
		public virtual  int? IsSaved { get; set; }

		[JsonProperty]
		public virtual  int? Point { get; set; }

		[JsonProperty, Column(StringLength = -2)]
		public virtual  string EMail { get; set; }

		[JsonProperty, Column(StringLength = -2)]
		public virtual  string Tel { get; set; }

		[JsonProperty, Column(StringLength = -2)]
		public virtual  string TrueName { get; set; }

		[JsonProperty, Column(StringLength = -2)]
		public virtual  string Sex { get; set; }

		[JsonProperty, Column(StringLength = -2)]
		public virtual  string QQ { get; set; }

		[JsonProperty, Column(DbType = "VARCHAR(50)")]
		public virtual  string Password { get; set; }

		[JsonProperty, Column(DbType = "VARCHAR(50)")]
		public virtual  string PersonName { get; set; }

		[JsonProperty, Column(DbType = "VARCHAR(36)")]
		public virtual  string PersonPowerID { get; set; }

		[JsonProperty, Column(DbType = "DATETIME(8)")]
		public virtual  string LoginTime { get; set; }

		[JsonProperty]
		public virtual  int? IsOpened { get; set; }


	/// <summary>
	/// 一键转换为支持 WPF 双向绑定的 View_PersonVM 对象
	/// </summary>
	public virtual View_PersonVM ToViewModel() {
		return new View_PersonVM {
			PersonID = this.PersonID,
			PersonCompassCount = this.PersonCompassCount,
			PersonGuoCount = this.PersonGuoCount,
			PersonEightWordCount = this.PersonEightWordCount,
			PowerName = this.PowerName,
			AsyncTime = this.AsyncTime,
			MD5 = this.MD5,
			Other = this.Other,
			WeChat = this.WeChat,
			IsDefault = this.IsDefault,
			PowerLevel = this.PowerLevel,
			IsSaved = this.IsSaved,
			Point = this.Point,
			EMail = this.EMail,
			Tel = this.Tel,
			TrueName = this.TrueName,
			Sex = this.Sex,
			QQ = this.QQ,
			Password = this.Password,
			PersonName = this.PersonName,
			PersonPowerID = this.PersonPowerID,
			LoginTime = this.LoginTime,
			IsOpened = this.IsOpened,
		};
	}



		// 🎯 【OOP 全局容器】：每个类独立的静态 Orm 引擎句柄
		public static IFreeSql Orm =Comm.Orm;

		private static IFreeSql SafeOrm => Orm ?? throw new Exception($"{nameof(View_Person)}.Orm 未在程序启动时初始化。");

	 



		

		

		/// <summary>
		/// 🎯 呼叫当前类的静态查询构造器，支持丝滑的 lambda 高级链式拉取
		/// </summary>
		public static ISelect<View_Person> Select => SafeOrm.Select<View_Person>();
		
		/// <summary>
		/// 🎯 PageList 静态方法：纯粹获取分好页的数据集合（不含任何 out 属性），pageIndex 小于等于 0 时自动查全部
		/// </summary>
		public static List<View_Person> List(System.Linq.Expressions.Expression<Func<View_Person, bool>> exp = null, int pageIndex = 0, int pageSize = 10, string sortField = null, bool isDesc = true)
		{
			var query = Select;
			if (exp != null) query = query.Where(exp);

			// ⚡ 1. 实体生成期检测：若有 CreateTime 则在编译期全自动硬编码为默认值，否则默认为 null
			if (!string.IsNullOrWhiteSpace(sortField))
			{
				query = query.OrderByPropertyName(sortField, isDesc);
			}

			// ⚡ 2. 运行期分页分流：pageIndex <= 0 时直接跳过 Page 限制获取全量
			if (pageIndex > 0)
			{
				query = query.Page(pageIndex, pageSize);
			}

			return query.ToList();
		}

		/// <summary>
		/// 🎯 GetRecordCount 静态方法：传入与 PageList 完全相同的条件，由数据库端执行 COUNT(*) 精准获取总条数
		/// </summary>
		public static long GetRecordCount(System.Linq.Expressions.Expression<Func<View_Person, bool>> exp = null)
		{
			var query = Select;
			if (exp != null) query = query.Where(exp);

			return query.Count();
		}


		/// <summary>
		/// 🎯 One 靜態方法：快捷獲取符合條件的第一條記錄（若有CreateTime則默認按最新時間倒序，確保拿到最新的一條）
		/// </summary>
		public static View_Person One(System.Linq.Expressions.Expression<Func<View_Person, bool>> exp= null, string sortField = null, bool isDesc = true)
		{
			var query = Select.Where(exp);

			// ⚡ 實體生成期檢測：若有 CreateTime 則在編譯期自動硬編碼為默認值，否則默認不排序
			if (!string.IsNullOrWhiteSpace(sortField))
			{
				query = query.OrderByPropertyName(sortField, isDesc);
			}

			return query.ToOne();
		}
	}

	/// <summary>
    /// 2. 专门用于 WPF/MAUI 绑定的 视图模型子类
    /// </summary>
    public partial class View_PersonVM : View_Person, INotifyPropertyChanged {

		public override string PersonID {
			get => base.PersonID;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.PersonID, value)) return;
				base.PersonID = value;
				OnPropertyChanged();
			}
		}

		public override int? PersonCompassCount {
			get => base.PersonCompassCount;
			set {
				if (System.Collections.Generic.EqualityComparer<int?>.Default.Equals(base.PersonCompassCount, value)) return;
				base.PersonCompassCount = value;
				OnPropertyChanged();
			}
		}

		public override int? PersonGuoCount {
			get => base.PersonGuoCount;
			set {
				if (System.Collections.Generic.EqualityComparer<int?>.Default.Equals(base.PersonGuoCount, value)) return;
				base.PersonGuoCount = value;
				OnPropertyChanged();
			}
		}

		public override int? PersonEightWordCount {
			get => base.PersonEightWordCount;
			set {
				if (System.Collections.Generic.EqualityComparer<int?>.Default.Equals(base.PersonEightWordCount, value)) return;
				base.PersonEightWordCount = value;
				OnPropertyChanged();
			}
		}

		public override string PowerName {
			get => base.PowerName;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.PowerName, value)) return;
				base.PowerName = value;
				OnPropertyChanged();
			}
		}

		public override DateTime? AsyncTime {
			get => base.AsyncTime;
			set {
				if (System.Collections.Generic.EqualityComparer<DateTime?>.Default.Equals(base.AsyncTime, value)) return;
				base.AsyncTime = value;
				OnPropertyChanged();
			}
		}

		public override string MD5 {
			get => base.MD5;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.MD5, value)) return;
				base.MD5 = value;
				OnPropertyChanged();
			}
		}

		public override string Other {
			get => base.Other;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.Other, value)) return;
				base.Other = value;
				OnPropertyChanged();
			}
		}

		public override string WeChat {
			get => base.WeChat;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.WeChat, value)) return;
				base.WeChat = value;
				OnPropertyChanged();
			}
		}

		public override int? IsDefault {
			get => base.IsDefault;
			set {
				if (System.Collections.Generic.EqualityComparer<int?>.Default.Equals(base.IsDefault, value)) return;
				base.IsDefault = value;
				OnPropertyChanged();
			}
		}

		public override int? PowerLevel {
			get => base.PowerLevel;
			set {
				if (System.Collections.Generic.EqualityComparer<int?>.Default.Equals(base.PowerLevel, value)) return;
				base.PowerLevel = value;
				OnPropertyChanged();
			}
		}

		public override int? IsSaved {
			get => base.IsSaved;
			set {
				if (System.Collections.Generic.EqualityComparer<int?>.Default.Equals(base.IsSaved, value)) return;
				base.IsSaved = value;
				OnPropertyChanged();
			}
		}

		public override int? Point {
			get => base.Point;
			set {
				if (System.Collections.Generic.EqualityComparer<int?>.Default.Equals(base.Point, value)) return;
				base.Point = value;
				OnPropertyChanged();
			}
		}

		public override string EMail {
			get => base.EMail;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.EMail, value)) return;
				base.EMail = value;
				OnPropertyChanged();
			}
		}

		public override string Tel {
			get => base.Tel;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.Tel, value)) return;
				base.Tel = value;
				OnPropertyChanged();
			}
		}

		public override string TrueName {
			get => base.TrueName;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.TrueName, value)) return;
				base.TrueName = value;
				OnPropertyChanged();
			}
		}

		public override string Sex {
			get => base.Sex;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.Sex, value)) return;
				base.Sex = value;
				OnPropertyChanged();
			}
		}

		public override string QQ {
			get => base.QQ;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.QQ, value)) return;
				base.QQ = value;
				OnPropertyChanged();
			}
		}

		public override string Password {
			get => base.Password;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.Password, value)) return;
				base.Password = value;
				OnPropertyChanged();
			}
		}

		public override string PersonName {
			get => base.PersonName;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.PersonName, value)) return;
				base.PersonName = value;
				OnPropertyChanged();
			}
		}

		public override string PersonPowerID {
			get => base.PersonPowerID;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.PersonPowerID, value)) return;
				base.PersonPowerID = value;
				OnPropertyChanged();
			}
		}

		public override string LoginTime {
			get => base.LoginTime;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.LoginTime, value)) return;
				base.LoginTime = value;
				OnPropertyChanged();
			}
		}

		public override int? IsOpened {
			get => base.IsOpened;
			set {
				if (System.Collections.Generic.EqualityComparer<int?>.Default.Equals(base.IsOpened, value)) return;
				base.IsOpened = value;
				OnPropertyChanged();
			}
		}


	#region 标准 INotifyPropertyChanged 接口实现
	public event PropertyChangedEventHandler PropertyChanged;
	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
	#endregion
    }

}

