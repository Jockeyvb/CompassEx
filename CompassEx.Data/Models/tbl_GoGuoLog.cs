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
	public partial class tbl_GoGuoLog {

		[JsonProperty, Column(IsIdentity = true, StringLength = -2, IsPrimary = true, IsNullable = false)]
		public virtual  string GoGuoLogID { get; set; }

		[JsonProperty, Column(StringLength = -2)]
		public virtual  string SkyLoc { get; set; }

		[JsonProperty, Column(StringLength = -2)]
		public virtual  string GuoYaos { get; set; }

		[JsonProperty]
		public virtual  DateTime? CreateTime { get; set; }

		[JsonProperty, Column(StringLength = -2)]
		public virtual  string Other { get; set; }


	/// <summary>
	/// 一键转换为支持 WPF 双向绑定的 tbl_GoGuoLogVM 对象
	/// </summary>
	public virtual tbl_GoGuoLogVM ToViewModel() {
		return new tbl_GoGuoLogVM {
			GoGuoLogID = this.GoGuoLogID,
			SkyLoc = this.SkyLoc,
			GuoYaos = this.GuoYaos,
			CreateTime = this.CreateTime,
			Other = this.Other,
		};
	}



		// 🎯 【OOP 全局容器】：每个类独立的静态 Orm 引擎句柄
		public static IFreeSql Orm =Comm.Orm;

		private static IFreeSql SafeOrm => Orm ?? throw new Exception($"{nameof(tbl_GoGuoLog)}.Orm 未在程序启动时初始化。");

		// ==========================================
		// 🚀 【实例方法 (非静态)】：操作对象状态
		// ==========================================
		/// <summary>
		/// 🎯 【正宗 Refresh】：从数据库抓取最新记录，完美回刷并同步当前实例的所有属性
		/// </summary>
		public void Refresh()
		{
			var dbEntity = One(t => t.GoGuoLogID == this.GoGuoLogID);
			if (dbEntity != null)
			{
				foreach (var prop in typeof(tbl_GoGuoLog).GetProperties(BindingFlags.Public | BindingFlags.Instance))
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
		public static ISelect<tbl_GoGuoLog> Select => SafeOrm.Select<tbl_GoGuoLog>();
		
		/// <summary>
		/// 🎯 PageList 静态方法：纯粹获取分好页的数据集合（不含任何 out 属性），pageIndex 小于等于 0 时自动查全部
		/// </summary>
		public static List<tbl_GoGuoLog> List(System.Linq.Expressions.Expression<Func<tbl_GoGuoLog, bool>> exp = null, int pageIndex = 0, int pageSize = 10, string sortField = "CreateTime", bool isDesc = true)
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
		public static long GetRecordCount(System.Linq.Expressions.Expression<Func<tbl_GoGuoLog, bool>> exp = null)
		{
			var query = Select;
			if (exp != null) query = query.Where(exp);

			return query.Count();
		}


		/// <summary>
		/// 🎯 One 靜態方法：快捷獲取符合條件的第一條記錄（若有CreateTime則默認按最新時間倒序，確保拿到最新的一條）
		/// </summary>
		public static tbl_GoGuoLog One(System.Linq.Expressions.Expression<Func<tbl_GoGuoLog, bool>> exp= null, string sortField = "CreateTime", bool isDesc = true)
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
    public partial class tbl_GoGuoLogVM : tbl_GoGuoLog, INotifyPropertyChanged {

		public override string GoGuoLogID {
			get => base.GoGuoLogID;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.GoGuoLogID, value)) return;
				base.GoGuoLogID = value;
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

		public override string GuoYaos {
			get => base.GuoYaos;
			set {
				if (System.Collections.Generic.EqualityComparer<string>.Default.Equals(base.GuoYaos, value)) return;
				base.GuoYaos = value;
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

