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
	public partial class tbl_PersonEightWord {

		[JsonProperty, Column(IsIdentity = true, StringLength = -2, IsPrimary = true, IsNullable = false)]
		public virtual  string PersonEightWordID { get; set; }

		[JsonProperty, Column(StringLength = -2, IsNullable = false)]
		public virtual  string PersonID { get; set; }

		[JsonProperty, Column(DbType = "DATE")]
		public virtual  DateTime? NowDate { get; set; }

		[JsonProperty, Column(StringLength = -2)]
		public virtual  string OldDate { get; set; }

		[JsonProperty, Column(StringLength = -2, IsNullable = false)]
		public virtual  string SkyLoc { get; set; }

		[JsonProperty, Column(StringLength = -2, IsNullable = false)]
		public virtual  string CreateTime { get; set; }

		[JsonProperty, Column(StringLength = -2)]
		public virtual  string ChangeTime { get; set; }

		[JsonProperty, Column(StringLength = -2)]
		public virtual  string Name { get; set; }

		[JsonProperty, Column(StringLength = -2)]
		public virtual  string Info { get; set; }

		[JsonProperty, Column(StringLength = -2)]
		public virtual  string Question { get; set; }

		[JsonProperty]
		public virtual  int IsInvite { get; set; } = 0;

		[JsonProperty]
		public virtual  int InvitePoint { get; set; } = 0;

		[JsonProperty]
		public virtual  int IsFinished { get; set; } = 0;

		[JsonProperty]
		public virtual  int IsUploaded { get; set; } = 0;

		[JsonProperty]
		public virtual  DateTime? AsyncTime { get; set; }

		[JsonProperty, Column(StringLength = -2)]
		public virtual  string Other { get; set; }


	/// <summary>
	/// 一键转换为支持 WPF 双向绑定的 tbl_PersonEightWordVM 对象
	/// </summary>
	public virtual tbl_PersonEightWordVM ToViewModel() {
		return new tbl_PersonEightWordVM {
			PersonEightWordID = this.PersonEightWordID,
			PersonID = this.PersonID,
			NowDate = this.NowDate,
			OldDate = this.OldDate,
			SkyLoc = this.SkyLoc,
			CreateTime = this.CreateTime,
			ChangeTime = this.ChangeTime,
			Name = this.Name,
			Info = this.Info,
			Question = this.Question,
			IsInvite = this.IsInvite,
			InvitePoint = this.InvitePoint,
			IsFinished = this.IsFinished,
			IsUploaded = this.IsUploaded,
			AsyncTime = this.AsyncTime,
			Other = this.Other,
		};
	}



		// 🎯 【OOP 全局容器】：每个类独立的静态 Orm 引擎句柄
		public static IFreeSql Orm =Comm.Orm;

		private static IFreeSql SafeOrm => Orm ?? throw new Exception($"{nameof(tbl_PersonEightWord)}.Orm 未在程序启动时初始化。");

		// ==========================================
		// 🚀 【实例方法 (非静态)】：操作对象状态
		// ==========================================
		/// <summary>
		/// 🎯 【正宗 Refresh】：从数据库抓取最新记录，完美回刷并同步当前实例的所有属性
		/// </summary>
		public void Refresh()
		{
			var dbEntity = One(t => t.PersonEightWordID == this.PersonEightWordID);
			if (dbEntity != null)
			{
				foreach (var prop in typeof(tbl_PersonEightWord).GetProperties(BindingFlags.Public | BindingFlags.Instance))
				{
					if (prop.CanWrite)
					{
						prop.SetValue(this, prop.GetValue(dbEntity));
					}
				}
			}
		}



		

		

		/// <summary>
		/// 🎯 呼叫当前类的静态查询构造器，支持丝滑的 lambda 高级链式拉取
		/// </summary>
		public static ISelect<tbl_PersonEightWord> Select => SafeOrm.Select<tbl_PersonEightWord>();
		
		/// <summary>
		/// 🎯 PageList 静态方法：纯粹获取分好页的数据集合（不含任何 out 属性），pageIndex 小于等于 0 时自动查全部
		/// </summary>
		public static List<tbl_PersonEightWord> List(System.Linq.Expressions.Expression<Func<tbl_PersonEightWord, bool>> exp = null, int pageIndex = 0, int pageSize = 10, string sortField = "CreateTime", bool isDesc = true)
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
		public static long GetRecordCount(System.Linq.Expressions.Expression<Func<tbl_PersonEightWord, bool>> exp = null)
		{
			var query = Select;
			if (exp != null) query = query.Where(exp);

			return query.Count();
		}


		/// <summary>
		/// 🎯 One 靜態方法：快捷獲取符合條件的第一條記錄（若有CreateTime則默認按最新時間倒序，確保拿到最新的一條）
		/// </summary>
		public static tbl_PersonEightWord One(System.Linq.Expressions.Expression<Func<tbl_PersonEightWord, bool>> exp= null, string sortField = "CreateTime", bool isDesc = true)
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
    public partial class tbl_PersonEightWordVM : tbl_PersonEightWord, INotifyPropertyChanged {

		public override string PersonEightWordID {
			get => base.PersonEightWordID;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.PersonEightWordID, value)) return;
				base.PersonEightWordID = value;
				OnPropertyChanged();
			}
		}

		public override string PersonID {
			get => base.PersonID;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.PersonID, value)) return;
				base.PersonID = value;
				OnPropertyChanged();
			}
		}

		public override DateTime? NowDate {
			get => base.NowDate;
			set {
				if (System.Collections.Generic.EqualityComparer<DateTime?>.Default.Equals(base.NowDate, value)) return;
				base.NowDate = value;
				OnPropertyChanged();
			}
		}

		public override string OldDate {
			get => base.OldDate;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.OldDate, value)) return;
				base.OldDate = value;
				OnPropertyChanged();
			}
		}

		public override string SkyLoc {
			get => base.SkyLoc;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.SkyLoc, value)) return;
				base.SkyLoc = value;
				OnPropertyChanged();
			}
		}

		public override string CreateTime {
			get => base.CreateTime;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.CreateTime, value)) return;
				base.CreateTime = value;
				OnPropertyChanged();
			}
		}

		public override string ChangeTime {
			get => base.ChangeTime;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.ChangeTime, value)) return;
				base.ChangeTime = value;
				OnPropertyChanged();
			}
		}

		public override string Name {
			get => base.Name;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.Name, value)) return;
				base.Name = value;
				OnPropertyChanged();
			}
		}

		public override string Info {
			get => base.Info;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.Info, value)) return;
				base.Info = value;
				OnPropertyChanged();
			}
		}

		public override string Question {
			get => base.Question;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.Question, value)) return;
				base.Question = value;
				OnPropertyChanged();
			}
		}

		public override int IsInvite {
			get => base.IsInvite;
			set {
				if (System.Collections.Generic.EqualityComparer<int>.Default.Equals(base.IsInvite, value)) return;
				base.IsInvite = value;
				OnPropertyChanged();
			}
		}

		public override int InvitePoint {
			get => base.InvitePoint;
			set {
				if (System.Collections.Generic.EqualityComparer<int>.Default.Equals(base.InvitePoint, value)) return;
				base.InvitePoint = value;
				OnPropertyChanged();
			}
		}

		public override int IsFinished {
			get => base.IsFinished;
			set {
				if (System.Collections.Generic.EqualityComparer<int>.Default.Equals(base.IsFinished, value)) return;
				base.IsFinished = value;
				OnPropertyChanged();
			}
		}

		public override int IsUploaded {
			get => base.IsUploaded;
			set {
				if (System.Collections.Generic.EqualityComparer<int>.Default.Equals(base.IsUploaded, value)) return;
				base.IsUploaded = value;
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

		public override string Other {
			get => base.Other;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.Other, value)) return;
				base.Other = value;
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

