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
	public partial class tbl_SixGod {

		[JsonProperty, Column(IsPrimary = true, IsIdentity = true)]
		public virtual  int SixGodID { get; set; }

		[JsonProperty, Column(DbType = "LONG(8)", IsNullable = false)]
		public virtual  string SixGodClassID { get; set; } = "0";

		[JsonProperty]
		public virtual  int SixRelativeClassID { get; set; } = -1;

		[JsonProperty, Column(DbType = "TEXT(16)", IsNullable = false)]
		public virtual  string SixGodInfo { get; set; }

		[JsonProperty, Column(DbType = "DATETIME(8)", IsNullable = false)]
		public virtual  string CreateTime { get; set; }

		[JsonProperty, Column(DbType = "TEXT(16)")]
		public virtual  string Other { get; set; }


	/// <summary>
	/// 一键转换为支持 WPF 双向绑定的 tbl_SixGodVM 对象
	/// </summary>
	public virtual tbl_SixGodVM ToViewModel() {
		return new tbl_SixGodVM {
			SixGodID = this.SixGodID,
			SixGodClassID = this.SixGodClassID,
			SixRelativeClassID = this.SixRelativeClassID,
			SixGodInfo = this.SixGodInfo,
			CreateTime = this.CreateTime,
			Other = this.Other,
		};
	}



		// 🎯 【OOP 全局容器】：每个类独立的静态 Orm 引擎句柄
		public static IFreeSql Orm =Comm.Orm;

		private static IFreeSql SafeOrm => Orm ?? throw new Exception($"{nameof(tbl_SixGod)}.Orm 未在程序启动时初始化。");

	 
		/// <summary>
		/// 🎯 【正宗 Refresh】：从数据库抓取最新记录，完美回刷并同步当前实例的所有属性
		/// </summary>
		public void Refresh()
		{
			var dbEntity = One(t => t.SixGodID == this.SixGodID);
			if (dbEntity != null)
			{
				foreach (var prop in typeof(tbl_SixGod).GetProperties(BindingFlags.Public | BindingFlags.Instance))
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
		public static ISelect<tbl_SixGod> Select => SafeOrm.Select<tbl_SixGod>();
		
		/// <summary>
		/// 🎯 PageList 静态方法：纯粹获取分好页的数据集合（不含任何 out 属性），pageIndex 小于等于 0 时自动查全部
		/// </summary>
		public static List<tbl_SixGod> List(System.Linq.Expressions.Expression<Func<tbl_SixGod, bool>> exp = null, int pageIndex = 0, int pageSize = 10, string sortField = "CreateTime", bool isDesc = true)
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
		public static long GetRecordCount(System.Linq.Expressions.Expression<Func<tbl_SixGod, bool>> exp = null)
		{
			var query = Select;
			if (exp != null) query = query.Where(exp);

			return query.Count();
		}


		/// <summary>
		/// 🎯 One 靜態方法：快捷獲取符合條件的第一條記錄（若有CreateTime則默認按最新時間倒序，確保拿到最新的一條）
		/// </summary>
		public static tbl_SixGod One(System.Linq.Expressions.Expression<Func<tbl_SixGod, bool>> exp= null, string sortField = "CreateTime", bool isDesc = true)
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
    public partial class tbl_SixGodVM : tbl_SixGod, INotifyPropertyChanged {

		public override int SixGodID {
			get => base.SixGodID;
			set {
				if (System.Collections.Generic.EqualityComparer<int>.Default.Equals(base.SixGodID, value)) return;
				base.SixGodID = value;
				OnPropertyChanged();
			}
		}

		public override string SixGodClassID {
			get => base.SixGodClassID;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.SixGodClassID, value)) return;
				base.SixGodClassID = value;
				OnPropertyChanged();
			}
		}

		public override int SixRelativeClassID {
			get => base.SixRelativeClassID;
			set {
				if (System.Collections.Generic.EqualityComparer<int>.Default.Equals(base.SixRelativeClassID, value)) return;
				base.SixRelativeClassID = value;
				OnPropertyChanged();
			}
		}

		public override string SixGodInfo {
			get => base.SixGodInfo;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.SixGodInfo, value)) return;
				base.SixGodInfo = value;
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

