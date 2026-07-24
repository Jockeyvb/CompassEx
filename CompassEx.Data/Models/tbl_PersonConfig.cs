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
	public partial class tbl_PersonConfig {

		[JsonProperty, Column(IsIdentity = true, DbType = "GUID", IsPrimary = true, IsNullable = false)]
		public virtual  string PersonConfigID { get; set; }

		[JsonProperty, Column(DbType = "GUID", IsNullable = false)]
		public virtual  string PersonID { get; set; }

		[JsonProperty]
		public virtual  int IsAutoSetTip { get; set; } = 0;

		[JsonProperty]
		public virtual  int TipDays { get; set; } = 0;

		[JsonProperty]
		public virtual  DateTime TipTime { get; set; }

		[JsonProperty, Column(StringLength = -2)]
		public virtual  string TipSoundFile { get; set; }

		[JsonProperty, Column(StringLength = -2, IsNullable = false)]
		public virtual  string EightWordOrderByField { get; set; } = "DateTime(ChangeTime)";

		[JsonProperty, Column(StringLength = -2, IsNullable = false)]
		public virtual  string EightWordOrderBy { get; set; } = "desc";

		[JsonProperty, Column(StringLength = -2, IsNullable = false)]
		public virtual  string GuoOrderByField { get; set; } = "DateTime(ChangeTime)";

		[JsonProperty, Column(StringLength = -2, IsNullable = false)]
		public virtual  string GuoOrderBy { get; set; } = "desc";

		[JsonProperty, Column(StringLength = -2, IsNullable = false)]
		public virtual  string CompassOrderByField { get; set; } = "DateTime(ChangeTime)";

		[JsonProperty, Column(StringLength = -2, IsNullable = false)]
		public virtual  string CompassOrderBy { get; set; } = "desc";

		[JsonProperty]
		public virtual  DateTime? CreateTime { get; set; }

		[JsonProperty]
		public virtual  int IsUploaded { get; set; } = 0;

		[JsonProperty, Column(StringLength = -2)]
		public virtual  string Other { get; set; }


	/// <summary>
	/// 一键转换为支持 WPF 双向绑定的 tbl_PersonConfigVM 对象
	/// </summary>
	public virtual tbl_PersonConfigVM ToViewModel() {
		return new tbl_PersonConfigVM {
			PersonConfigID = this.PersonConfigID,
			PersonID = this.PersonID,
			IsAutoSetTip = this.IsAutoSetTip,
			TipDays = this.TipDays,
			TipTime = this.TipTime,
			TipSoundFile = this.TipSoundFile,
			EightWordOrderByField = this.EightWordOrderByField,
			EightWordOrderBy = this.EightWordOrderBy,
			GuoOrderByField = this.GuoOrderByField,
			GuoOrderBy = this.GuoOrderBy,
			CompassOrderByField = this.CompassOrderByField,
			CompassOrderBy = this.CompassOrderBy,
			CreateTime = this.CreateTime,
			IsUploaded = this.IsUploaded,
			Other = this.Other,
		};
	}



		// 🎯 【OOP 全局容器】：每个类独立的静态 Orm 引擎句柄
		public static IFreeSql Orm =Comm.Orm;

		private static IFreeSql SafeOrm => Orm ?? throw new Exception($"{nameof(tbl_PersonConfig)}.Orm 未在程序启动时初始化。");

	 
		/// <summary>
		/// 🎯 【正宗 Refresh】：从数据库抓取最新记录，完美回刷并同步当前实例的所有属性
		/// </summary>
		public void Refresh()
		{
			var dbEntity = One(t => t.PersonConfigID == this.PersonConfigID);
			if (dbEntity != null)
			{
				foreach (var prop in typeof(tbl_PersonConfig).GetProperties(BindingFlags.Public | BindingFlags.Instance))
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
		public static ISelect<tbl_PersonConfig> Select => SafeOrm.Select<tbl_PersonConfig>();
		
		/// <summary>
		/// 🎯 PageList 静态方法：纯粹获取分好页的数据集合（不含任何 out 属性），pageIndex 小于等于 0 时自动查全部
		/// </summary>
		public static List<tbl_PersonConfig> List(System.Linq.Expressions.Expression<Func<tbl_PersonConfig, bool>> exp = null, int pageIndex = 0, int pageSize = 10, string sortField = "CreateTime", bool isDesc = true)
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
		public static long GetRecordCount(System.Linq.Expressions.Expression<Func<tbl_PersonConfig, bool>> exp = null)
		{
			var query = Select;
			if (exp != null) query = query.Where(exp);

			return query.Count();
		}


		/// <summary>
		/// 🎯 One 靜態方法：快捷獲取符合條件的第一條記錄（若有CreateTime則默認按最新時間倒序，確保拿到最新的一條）
		/// </summary>
		public static tbl_PersonConfig One(System.Linq.Expressions.Expression<Func<tbl_PersonConfig, bool>> exp= null, string sortField = "CreateTime", bool isDesc = true)
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
    public partial class tbl_PersonConfigVM : tbl_PersonConfig, INotifyPropertyChanged {

		public override string PersonConfigID {
			get => base.PersonConfigID;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.PersonConfigID, value)) return;
				base.PersonConfigID = value;
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

		public override int IsAutoSetTip {
			get => base.IsAutoSetTip;
			set {
				if (System.Collections.Generic.EqualityComparer<int>.Default.Equals(base.IsAutoSetTip, value)) return;
				base.IsAutoSetTip = value;
				OnPropertyChanged();
			}
		}

		public override int TipDays {
			get => base.TipDays;
			set {
				if (System.Collections.Generic.EqualityComparer<int>.Default.Equals(base.TipDays, value)) return;
				base.TipDays = value;
				OnPropertyChanged();
			}
		}

		public override DateTime TipTime {
			get => base.TipTime;
			set {
				if (System.Collections.Generic.EqualityComparer<DateTime>.Default.Equals(base.TipTime, value)) return;
				base.TipTime = value;
				OnPropertyChanged();
			}
		}

		public override string TipSoundFile {
			get => base.TipSoundFile;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.TipSoundFile, value)) return;
				base.TipSoundFile = value;
				OnPropertyChanged();
			}
		}

		public override string EightWordOrderByField {
			get => base.EightWordOrderByField;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.EightWordOrderByField, value)) return;
				base.EightWordOrderByField = value;
				OnPropertyChanged();
			}
		}

		public override string EightWordOrderBy {
			get => base.EightWordOrderBy;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.EightWordOrderBy, value)) return;
				base.EightWordOrderBy = value;
				OnPropertyChanged();
			}
		}

		public override string GuoOrderByField {
			get => base.GuoOrderByField;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.GuoOrderByField, value)) return;
				base.GuoOrderByField = value;
				OnPropertyChanged();
			}
		}

		public override string GuoOrderBy {
			get => base.GuoOrderBy;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.GuoOrderBy, value)) return;
				base.GuoOrderBy = value;
				OnPropertyChanged();
			}
		}

		public override string CompassOrderByField {
			get => base.CompassOrderByField;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.CompassOrderByField, value)) return;
				base.CompassOrderByField = value;
				OnPropertyChanged();
			}
		}

		public override string CompassOrderBy {
			get => base.CompassOrderBy;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.CompassOrderBy, value)) return;
				base.CompassOrderBy = value;
				OnPropertyChanged();
			}
		}

		public override DateTime? CreateTime {
			get => base.CreateTime;
			set {
				if (System.Collections.Generic.EqualityComparer<DateTime?>.Default.Equals(base.CreateTime, value)) return;
				base.CreateTime = value;
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

