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
	public partial class sqlite_stat2 {

		[JsonProperty, Column(DbType = "")]
		public virtual  string tbl { get; set; }

		[JsonProperty, Column(DbType = "")]
		public virtual  string idx { get; set; }

		[JsonProperty, Column(DbType = "")]
		public virtual  string sampleno { get; set; }

		[JsonProperty, Column(DbType = "")]
		public virtual  string sample { get; set; }


	/// <summary>
	/// 一键转换为支持 WPF 双向绑定的 sqlite_stat2VM 对象
	/// </summary>
	public virtual sqlite_stat2VM Tosqlite_stat2VM() {
		return new sqlite_stat2VM {
			tbl = this.tbl,
			idx = this.idx,
			sampleno = this.sampleno,
			sample = this.sample,
		};
	}



		// 🎯 【OOP 全局容器】：每个类独立的静态 Orm 引擎句柄
		public static IFreeSql Orm =Comm.Orm;

		private static IFreeSql SafeOrm => Orm ?? throw new Exception($"{nameof(sqlite_stat2)}.Orm 未在程序启动时初始化。");

		// ==========================================
		// 🚀 【实例方法 (非静态)】：操作对象状态
		// ==========================================

		/// <summary>
		    /// 🎯 实例更新：将当前对象自身的最新属性同步更新回数据库
		    /// </summary>
		    public bool Update() => SafeOrm.Update<sqlite_stat2>().SetSource(this).ExecuteAffrows() > 0;
		    /// <summary>
		    /// 🎯 实例销毁：从数据库中将当前对象自己删除
		    /// </summary>
		    public bool Delete() => SafeOrm.Delete<sqlite_stat2>().WhereDynamic(this).ExecuteAffrows() > 0;
		// ==========================================
		// ⚡ 【静态方法 (Static)】：唯一安全入口与集合操作
		// ==========================================
		
		/// <summary>
		/// 🎯 静态安全添加（AddNew）：极致性能分流！完美兼顾 Guid 本地生成与自增 int 智能回填！
		/// </summary>
		public static bool AddNew(sqlite_stat2 entity)
		{
			if (entity == null) throw new ArgumentNullException(nameof(entity));
			
			if (typeof(String) == typeof(Guid))
			{
				var pkProp = typeof(sqlite_stat2).GetProperty("tbl");
				if (pkProp != null && (Guid)pkProp.GetValue(entity) == Guid.Empty)
				{
					pkProp.SetValue(entity, Guid.NewGuid());
				}
				return SafeOrm.Insert(entity).ExecuteAffrows() > 0;
			}
			else
			{
				long identityVal = SafeOrm.Insert(entity).ExecuteIdentity();
				if (identityVal > 0)
				{
					var pkProp = typeof(sqlite_stat2).GetProperty("tbl");
					if (pkProp != null)
					{
						object convertedId = Convert.ChangeType(identityVal, typeof(String));
						pkProp.SetValue(entity, convertedId);
					}
					entity.Refresh();
					return true;
				}
				return false;
			}
		}
		
		/// <summary>
		/// 🎯 静态按主键删除：根据传入的 ID 直接轰炸数据库，无需提前载入内存
		/// </summary>
		public static bool Delete(String id) => SafeOrm.Delete<sqlite_stat2>().WhereDynamic(id).ExecuteAffrows() > 0;
		
		/// <summary>
		/// 🎯 静态按条件批量删除：支持复杂的 Lambda 表达式过滤删除
		/// </summary>
		public static bool Delete(System.Linq.Expressions.Expression<Func<sqlite_stat2, bool>> exp) => SafeOrm.Delete<sqlite_stat2>().Where(exp).ExecuteAffrows() > 0;


		

		

		/// <summary>
		/// 🎯 呼叫当前类的静态查询构造器，支持丝滑的 lambda 高级链式拉取
		/// </summary>
		public static ISelect<sqlite_stat2> Select => SafeOrm.Select<sqlite_stat2>();
		
		/// <summary>
		/// 🎯 PageList 静态方法：纯粹获取分好页的数据集合（不含任何 out 属性），pageIndex 小于等于 0 时自动查全部
		/// </summary>
		public static List<sqlite_stat2> List(System.Linq.Expressions.Expression<Func<sqlite_stat2, bool>> exp = null, int pageIndex = 0, int pageSize = 10, string sortField = null, bool isDesc = true)
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
		public static long GetRecordCount(System.Linq.Expressions.Expression<Func<sqlite_stat2, bool>> exp = null)
		{
			var query = Select;
			if (exp != null) query = query.Where(exp);

			return query.Count();
		}


		/// <summary>
		/// 🎯 One 靜態方法：快捷獲取符合條件的第一條記錄（若有CreateTime則默認按最新時間倒序，確保拿到最新的一條）
		/// </summary>
		public static sqlite_stat2 One(System.Linq.Expressions.Expression<Func<sqlite_stat2, bool>> exp= null, string sortField = null, bool isDesc = true)
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
    public partial class sqlite_stat2VM : sqlite_stat2, INotifyPropertyChanged {

		public override string tbl {
			get => base.tbl;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.tbl, value)) return;
				base.tbl = value;
				OnPropertyChanged();
			}
		}

		public override string idx {
			get => base.idx;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.idx, value)) return;
				base.idx = value;
				OnPropertyChanged();
			}
		}

		public override string sampleno {
			get => base.sampleno;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.sampleno, value)) return;
				base.sampleno = value;
				OnPropertyChanged();
			}
		}

		public override string sample {
			get => base.sample;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.sample, value)) return;
				base.sample = value;
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

