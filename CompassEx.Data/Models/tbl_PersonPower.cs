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
	public partial class tbl_PersonPower {

		[JsonProperty, Column(IsIdentity = true, StringLength = -2, IsPrimary = true, IsNullable = false)]
		public virtual  string PersonPowerID { get; set; }

		[JsonProperty, Column(StringLength = -2, IsNullable = false)]
		public virtual  string PowerName { get; set; }

		[JsonProperty]
		public virtual  int PersonEightWordCount { get; set; } = 10;

		[JsonProperty]
		public virtual  int PersonGuoCount { get; set; } = 50;

		[JsonProperty]
		public virtual  int PersonCompassCount { get; set; } = 10;

		[JsonProperty]
		public virtual  int PowerLevel { get; set; } = 0;

		[JsonProperty, Column(StringLength = -2, IsNullable = false)]
		public virtual  string CreateTime { get; set; }

		[JsonProperty]
		public virtual  int IsOpened { get; set; } = 1;

		[JsonProperty, Column(StringLength = -2)]
		public virtual  string Other { get; set; }


	/// <summary>
	/// 一键转换为支持 WPF 双向绑定的 tbl_PersonPowerVM 对象
	/// </summary>
	public virtual tbl_PersonPowerVM ToViewModel() {
		return new tbl_PersonPowerVM {
			PersonPowerID = this.PersonPowerID,
			PowerName = this.PowerName,
			PersonEightWordCount = this.PersonEightWordCount,
			PersonGuoCount = this.PersonGuoCount,
			PersonCompassCount = this.PersonCompassCount,
			PowerLevel = this.PowerLevel,
			CreateTime = this.CreateTime,
			IsOpened = this.IsOpened,
			Other = this.Other,
		};
	}



		// 🎯 【OOP 全局容器】：每个类独立的静态 Orm 引擎句柄
		public static IFreeSql Orm =Comm.Orm;

		private static IFreeSql SafeOrm => Orm ?? throw new Exception($"{nameof(tbl_PersonPower)}.Orm 未在程序启动时初始化。");

	 
		/// <summary>
		/// 🎯 【正宗 Refresh】：从数据库抓取最新记录，完美回刷并同步当前实例的所有属性
		/// </summary>
		public void Refresh()
		{
			var dbEntity = One(t => t.PersonPowerID == this.PersonPowerID);
			if (dbEntity != null)
			{
				foreach (var prop in typeof(tbl_PersonPower).GetProperties(BindingFlags.Public | BindingFlags.Instance))
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
		public static ISelect<tbl_PersonPower> Select => SafeOrm.Select<tbl_PersonPower>();
		
		/// <summary>
		/// 🎯 PageList 静态方法：纯粹获取分好页的数据集合（不含任何 out 属性），pageIndex 小于等于 0 时自动查全部
		/// </summary>
		public static List<tbl_PersonPower> List(System.Linq.Expressions.Expression<Func<tbl_PersonPower, bool>> exp = null, int pageIndex = 0, int pageSize = 10, string sortField = "CreateTime", bool isDesc = true)
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
		public static long GetRecordCount(System.Linq.Expressions.Expression<Func<tbl_PersonPower, bool>> exp = null)
		{
			var query = Select;
			if (exp != null) query = query.Where(exp);

			return query.Count();
		}


		/// <summary>
		/// 🎯 One 靜態方法：快捷獲取符合條件的第一條記錄（若有CreateTime則默認按最新時間倒序，確保拿到最新的一條）
		/// </summary>
		public static tbl_PersonPower One(System.Linq.Expressions.Expression<Func<tbl_PersonPower, bool>> exp= null, string sortField = "CreateTime", bool isDesc = true)
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
    public partial class tbl_PersonPowerVM : tbl_PersonPower, INotifyPropertyChanged {

		public override string PersonPowerID {
			get => base.PersonPowerID;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.PersonPowerID, value)) return;
				base.PersonPowerID = value;
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

		public override int PersonEightWordCount {
			get => base.PersonEightWordCount;
			set {
				if (System.Collections.Generic.EqualityComparer<int>.Default.Equals(base.PersonEightWordCount, value)) return;
				base.PersonEightWordCount = value;
				OnPropertyChanged();
			}
		}

		public override int PersonGuoCount {
			get => base.PersonGuoCount;
			set {
				if (System.Collections.Generic.EqualityComparer<int>.Default.Equals(base.PersonGuoCount, value)) return;
				base.PersonGuoCount = value;
				OnPropertyChanged();
			}
		}

		public override int PersonCompassCount {
			get => base.PersonCompassCount;
			set {
				if (System.Collections.Generic.EqualityComparer<int>.Default.Equals(base.PersonCompassCount, value)) return;
				base.PersonCompassCount = value;
				OnPropertyChanged();
			}
		}

		public override int PowerLevel {
			get => base.PowerLevel;
			set {
				if (System.Collections.Generic.EqualityComparer<int>.Default.Equals(base.PowerLevel, value)) return;
				base.PowerLevel = value;
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

		public override int IsOpened {
			get => base.IsOpened;
			set {
				if (System.Collections.Generic.EqualityComparer<int>.Default.Equals(base.IsOpened, value)) return;
				base.IsOpened = value;
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

