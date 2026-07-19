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
	public partial class tbl_Class {

		[JsonProperty, Column(IsIdentity = true, IsPrimary = true)]
		public virtual  int ClassID { get; set; }

		[JsonProperty]
		public virtual  int ParentID { get; set; } = 0;

		[JsonProperty, Column(DbType = "VARCHAR(500)", IsNullable = false)]
		public virtual  string ClassName { get; set; }

		[JsonProperty, Column(DbType = "VARCHAR(500)")]
		public virtual  string ModuleClass { get; set; }

		[JsonProperty, Column(DbType = "VARCHAR(500)")]
		public virtual  string StringItem1 { get; set; }

		[JsonProperty, Column(DbType = "VARCHAR(500)")]
		public virtual  string StringItem2 { get; set; }

		[JsonProperty]
		public virtual  int IntItem1 { get; set; } = 0;

		[JsonProperty]
		public virtual  int IntItem2 { get; set; } = 0;

		[JsonProperty, Column(DbType = "DECIMAL(8,2)")]
		public virtual  decimal FloatItem { get; set; } = 0M;

		[JsonProperty, Column(DbType = "DATETIME(8)")]
		public virtual  string TimeItem { get; set; }

		[JsonProperty]
		public virtual  int OrderBy { get; set; } = 0;

		[JsonProperty, Column(DbType = "VARCHAR(500)")]
		public virtual  string Other { get; set; }


	/// <summary>
	/// 一键转换为支持 WPF 双向绑定的 tbl_ClassVM 对象
	/// </summary>
	public virtual tbl_ClassVM ToViewModel() {
		return new tbl_ClassVM {
			ClassID = this.ClassID,
			ParentID = this.ParentID,
			ClassName = this.ClassName,
			ModuleClass = this.ModuleClass,
			StringItem1 = this.StringItem1,
			StringItem2 = this.StringItem2,
			IntItem1 = this.IntItem1,
			IntItem2 = this.IntItem2,
			FloatItem = this.FloatItem,
			TimeItem = this.TimeItem,
			OrderBy = this.OrderBy,
			Other = this.Other,
		};
	}



		// 🎯 【OOP 全局容器】：每个类独立的静态 Orm 引擎句柄
		public static IFreeSql Orm =Comm.Orm;

		private static IFreeSql SafeOrm => Orm ?? throw new Exception($"{nameof(tbl_Class)}.Orm 未在程序启动时初始化。");

		// ==========================================
		// 🚀 【实例方法 (非静态)】：操作对象状态
		// ==========================================
		/// <summary>
		/// 🎯 【正宗 Refresh】：从数据库抓取最新记录，完美回刷并同步当前实例的所有属性
		/// </summary>
		public void Refresh()
		{
			var dbEntity = One(t => t.ClassID == this.ClassID);
			if (dbEntity != null)
			{
				foreach (var prop in typeof(tbl_Class).GetProperties(BindingFlags.Public | BindingFlags.Instance))
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
		public static ISelect<tbl_Class> Select => SafeOrm.Select<tbl_Class>();
		
		/// <summary>
		/// 🎯 PageList 静态方法：纯粹获取分好页的数据集合（不含任何 out 属性），pageIndex 小于等于 0 时自动查全部
		/// </summary>
		public static List<tbl_Class> List(System.Linq.Expressions.Expression<Func<tbl_Class, bool>> exp = null, int pageIndex = 0, int pageSize = 10, string sortField = null, bool isDesc = true)
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
		public static long GetRecordCount(System.Linq.Expressions.Expression<Func<tbl_Class, bool>> exp = null)
		{
			var query = Select;
			if (exp != null) query = query.Where(exp);

			return query.Count();
		}


		/// <summary>
		/// 🎯 One 靜態方法：快捷獲取符合條件的第一條記錄（若有CreateTime則默認按最新時間倒序，確保拿到最新的一條）
		/// </summary>
		public static tbl_Class One(System.Linq.Expressions.Expression<Func<tbl_Class, bool>> exp= null, string sortField = null, bool isDesc = true)
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
    public partial class tbl_ClassVM : tbl_Class, INotifyPropertyChanged {

		public override int ClassID {
			get => base.ClassID;
			set {
				if (System.Collections.Generic.EqualityComparer<int>.Default.Equals(base.ClassID, value)) return;
				base.ClassID = value;
				OnPropertyChanged();
			}
		}

		public override int ParentID {
			get => base.ParentID;
			set {
				if (System.Collections.Generic.EqualityComparer<int>.Default.Equals(base.ParentID, value)) return;
				base.ParentID = value;
				OnPropertyChanged();
			}
		}

		public override string ClassName {
			get => base.ClassName;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.ClassName, value)) return;
				base.ClassName = value;
				OnPropertyChanged();
			}
		}

		public override string ModuleClass {
			get => base.ModuleClass;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.ModuleClass, value)) return;
				base.ModuleClass = value;
				OnPropertyChanged();
			}
		}

		public override string StringItem1 {
			get => base.StringItem1;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.StringItem1, value)) return;
				base.StringItem1 = value;
				OnPropertyChanged();
			}
		}

		public override string StringItem2 {
			get => base.StringItem2;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.StringItem2, value)) return;
				base.StringItem2 = value;
				OnPropertyChanged();
			}
		}

		public override int IntItem1 {
			get => base.IntItem1;
			set {
				if (System.Collections.Generic.EqualityComparer<int>.Default.Equals(base.IntItem1, value)) return;
				base.IntItem1 = value;
				OnPropertyChanged();
			}
		}

		public override int IntItem2 {
			get => base.IntItem2;
			set {
				if (System.Collections.Generic.EqualityComparer<int>.Default.Equals(base.IntItem2, value)) return;
				base.IntItem2 = value;
				OnPropertyChanged();
			}
		}

		public override decimal FloatItem {
			get => base.FloatItem;
			set {
				if (System.Collections.Generic.EqualityComparer<decimal>.Default.Equals(base.FloatItem, value)) return;
				base.FloatItem = value;
				OnPropertyChanged();
			}
		}

		public override string TimeItem {
			get => base.TimeItem;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.TimeItem, value)) return;
				base.TimeItem = value;
				OnPropertyChanged();
			}
		}

		public override int OrderBy {
			get => base.OrderBy;
			set {
				if (System.Collections.Generic.EqualityComparer<int>.Default.Equals(base.OrderBy, value)) return;
				base.OrderBy = value;
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

