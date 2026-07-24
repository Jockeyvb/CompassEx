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
	public partial class View_YearSeason {

		[JsonProperty]
		public virtual  int? YearSeasonID { get; set; }

		[JsonProperty]
		public virtual  int? YearID { get; set; }

		[JsonProperty]
		public virtual  int? TrueYearID { get; set; }

		[JsonProperty]
		public virtual  int? SeasonID { get; set; }

		[JsonProperty, Column(DbType = "VARCHAR(500)")]
		public virtual  string Value { get; set; }

		[JsonProperty, Column(DbType = "VARCHAR(500)")]
		public virtual  string Other { get; set; }

		[JsonProperty]
		public virtual  int? YearName { get; set; }

		[JsonProperty, Column(DbType = "VARCHAR(50)")]
		public virtual  string MonthValue { get; set; }

		[JsonProperty, Column(DbType = "VARCHAR(500)")]
		public virtual  string SeasonName { get; set; }


	/// <summary>
	/// 一键转换为支持 WPF 双向绑定的 View_YearSeasonVM 对象
	/// </summary>
	public virtual View_YearSeasonVM ToViewModel() {
		return new View_YearSeasonVM {
			YearSeasonID = this.YearSeasonID,
			YearID = this.YearID,
			TrueYearID = this.TrueYearID,
			SeasonID = this.SeasonID,
			Value = this.Value,
			Other = this.Other,
			YearName = this.YearName,
			MonthValue = this.MonthValue,
			SeasonName = this.SeasonName,
		};
	}



		// 🎯 【OOP 全局容器】：每个类独立的静态 Orm 引擎句柄
		public static IFreeSql Orm =Comm.Orm;

		private static IFreeSql SafeOrm => Orm ?? throw new Exception($"{nameof(View_YearSeason)}.Orm 未在程序启动时初始化。");

	 



		

		

		/// <summary>
		/// 🎯 呼叫当前类的静态查询构造器，支持丝滑的 lambda 高级链式拉取
		/// </summary>
		public static ISelect<View_YearSeason> Select => SafeOrm.Select<View_YearSeason>();
		
		/// <summary>
		/// 🎯 PageList 静态方法：纯粹获取分好页的数据集合（不含任何 out 属性），pageIndex 小于等于 0 时自动查全部
		/// </summary>
		public static List<View_YearSeason> List(System.Linq.Expressions.Expression<Func<View_YearSeason, bool>> exp = null, int pageIndex = 0, int pageSize = 10, string sortField = null, bool isDesc = true)
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
		public static long GetRecordCount(System.Linq.Expressions.Expression<Func<View_YearSeason, bool>> exp = null)
		{
			var query = Select;
			if (exp != null) query = query.Where(exp);

			return query.Count();
		}


		/// <summary>
		/// 🎯 One 靜態方法：快捷獲取符合條件的第一條記錄（若有CreateTime則默認按最新時間倒序，確保拿到最新的一條）
		/// </summary>
		public static View_YearSeason One(System.Linq.Expressions.Expression<Func<View_YearSeason, bool>> exp= null, string sortField = null, bool isDesc = true)
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
    public partial class View_YearSeasonVM : View_YearSeason, INotifyPropertyChanged {

		public override int? YearSeasonID {
			get => base.YearSeasonID;
			set {
				if (System.Collections.Generic.EqualityComparer<int?>.Default.Equals(base.YearSeasonID, value)) return;
				base.YearSeasonID = value;
				OnPropertyChanged();
			}
		}

		public override int? YearID {
			get => base.YearID;
			set {
				if (System.Collections.Generic.EqualityComparer<int?>.Default.Equals(base.YearID, value)) return;
				base.YearID = value;
				OnPropertyChanged();
			}
		}

		public override int? TrueYearID {
			get => base.TrueYearID;
			set {
				if (System.Collections.Generic.EqualityComparer<int?>.Default.Equals(base.TrueYearID, value)) return;
				base.TrueYearID = value;
				OnPropertyChanged();
			}
		}

		public override int? SeasonID {
			get => base.SeasonID;
			set {
				if (System.Collections.Generic.EqualityComparer<int?>.Default.Equals(base.SeasonID, value)) return;
				base.SeasonID = value;
				OnPropertyChanged();
			}
		}

		public override string Value {
			get => base.Value;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.Value, value)) return;
				base.Value = value;
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

		public override int? YearName {
			get => base.YearName;
			set {
				if (System.Collections.Generic.EqualityComparer<int?>.Default.Equals(base.YearName, value)) return;
				base.YearName = value;
				OnPropertyChanged();
			}
		}

		public override string MonthValue {
			get => base.MonthValue;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.MonthValue, value)) return;
				base.MonthValue = value;
				OnPropertyChanged();
			}
		}

		public override string SeasonName {
			get => base.SeasonName;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.SeasonName, value)) return;
				base.SeasonName = value;
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

