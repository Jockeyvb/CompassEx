using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace CommLib
{
    /// <summary>
    /// 丁兰尺大格计算实体类
    /// </summary>
    /// <remarks>
    /// 核心功能：根据输入刻度值/格位循环次数，解析丁兰尺十大基础大格，
    /// 自动匹配格位名称、吉凶释义、吉凶颜色、起止刻度、归属细分小格，
    /// 支持获取当前凶格左右最近吉祥刻度值，10个大格为一组循环往复
    /// 丁兰尺单标准大格固定尺寸：38.78MM
    /// </remarks>
    public class DingLanGrid
    {
        /// <summary>
        /// 丁兰尺十大基础大格名称数组（固定循环顺序）
        /// </summary>
        /// <value>存储10个基础大格名称，用于取模循环计算格位索引</value>
        public static readonly string[] DingLanGridNames = { "丁", "害", "旺", "苦", "义", "官", "死", "兴", "失", "财" }; //丁兰大格

        /// <summary>
        /// 丁兰尺十大大格吉凶释义说明数组
        /// </summary>
        /// <value>与 DingLanGridNames 索引一一对应，存储每格吉凶详细释义</value>
        public static readonly string[] DingLanGridInfos = { "生子,添丁。", "遇小人,伤害,灾害,病灾", "进财,旺盛。", "悲苦,损财,生病", "厚利,而行财运", "升官,进财利", "痼疾,重病,意外伤亡", "蒸蒸日上", "失败致破产", "财源滚滚" };

        /// <summary>
        /// 丁兰尺十大大格吉凶颜色映射数组
        /// </summary>
        /// <value>Red=吉格，Black=凶格，与名称、释义数组索引完全对应</value>
        public static readonly Color[] DingLanGridColor = { Color.Red, Color.Black, Color.Red, Color.Black, Color.Red, Color.Red, Color.Black, Color.Red, Color.Black, Color.Red };

        /// <summary>
        /// 当前大格对应的吉凶展示颜色
        /// </summary>
        /// <value>根据当前格位索引自动匹配，红色为吉、黑色为凶</value>
        public Color GridColor { get; private set; }

        /// <summary>
        /// 当前大格在10格循环组中的索引
        /// </summary>
        /// <value>对应名称、释义、颜色数组下标（0-9循环）</value>
        public int Index { get; private set; }

        /// <summary>
        /// 当前丁兰大格起始刻度（单位：MM，向下取整）
        /// </summary>
        public double StartValue { get; private set; }

        /// <summary>
        /// 当前丁兰大格结束刻度（单位：MM，向下取整）
        /// </summary>
        public double EndValue { get; private set; }

        /// <summary>
        /// 当前大格中文名称
        /// </summary>
        /// <value>取值于 DingLanGridNames 对应索引名称</value>
        public string GridName { get; private set; }

        /// <summary>
        /// 丁兰尺单一大格固定标准刻度宽度
        /// </summary>
        /// <value>固定值 38.78MM，为丁兰尺标准单大格尺寸</value>
        public const double GridFixed = 38.78;

        /// <summary>
        /// 当前计算的目标刻度值
        /// </summary>
        /// <remarks>默认初始值 -1 代表未赋值；根据名称初始化时，默认赋值为格起始值</remarks>
        public double Value { get; private set; } = -1;

        /// <summary>
        /// 当前刻度归属的丁兰细分小格实体
        /// </summary>
        /// <value>绑定当前刻度对应的最小细分格对象，关联父级大格</value>
        public DingLanSubGrid SG { get; private set; }

        /// <summary>
        /// 当前大格的吉凶文字说明
        /// </summary>
        public string DingLanGridInfo { get; private set; } = "";

        /// <summary>
        /// 当前大格包含的所有细分小格子级集合
        /// </summary>
        /// <value>自动加载当前大格对应的4个一组细分小格数据</value>
        public List<DingLanSubGrid> Child { get; private set; } = new List<DingLanSubGrid>();

        /// <summary>
        /// 私有缓存字段：右侧最近吉祥刻度值
        /// </summary>
        private double rightGoodValue;

        /// <summary>
        /// 获取【大于当前刻度】的最近吉祥刻度（向右寻吉）
        /// </summary>
        /// <returns>
        /// -1：刻度未赋值 / 当前格为吉格无需查找；
        /// 有效数值：右侧首个吉格的起始刻度+1
        /// </returns>
        /// <remarks>仅当前格为凶格时生效，向后遍历10大格循环组，匹配首个吉色格位</remarks>
        public double RightGoodValue
        {
            get
            {
                if (this.Value < 0) return -1;
                if (this.GridColor == Color.Red) return -1;//如果是吉则不需要寻
                double EndValue = this.EndValue;
                for (int i = this.Index + 1; i < DingLanGridNames.Length; i++)
                {
                    EndValue = Math.Floor(EndValue + GridFixed) - 1;
                    if (DingLanGridColor[i] == Color.Red)
                    {
                        DingLanGrid LBG = new DingLanGrid(EndValue);
                        rightGoodValue = LBG.StartValue + 1;
                        break;
                    }
                }
                return rightGoodValue;
            }
        }

        /// <summary>
        /// 私有缓存字段：左侧最近吉祥刻度值
        /// </summary>
        private double leftGoodValue;

        /// <summary>
        /// 获取【小于当前刻度】的最近吉祥刻度（向左寻吉）
        /// </summary>
        /// <returns>
        /// -1：刻度未赋值 / 当前格为吉格无需查找；
        /// 有效数值：左侧首个吉格的结束刻度
        /// </returns>
        /// <remarks>仅当前格为凶格时生效，向前遍历10大格循环组，匹配首个吉色格位</remarks>
        public double LeftGoodValue
        {
            get
            {
                if (this.Value < 0) return -1;
                if (this.GridColor == Color.Red) return -1;//如果是吉则不需要寻
                double EndValue = this.EndValue;
                for (int i = this.Index - 1; i >= 0; i--)
                {
                    EndValue = Math.Floor(EndValue - GridFixed);
                    if (DingLanGridColor[i] == Color.Red)
                    {
                        DingLanGrid LBG = new DingLanGrid(EndValue);
                        leftGoodValue = LBG.EndValue;
                        break;
                    }
                }
                return leftGoodValue;
            }
        }

        /// <summary>
        /// 根据循环次数初始化当前大格所有属性
        /// </summary>
        /// <param name="Times">大格循环倍数，用于计算10格循环索引与刻度区间</param>
        /// <remarks>
        /// 1. 对10大格取模获取循环索引
        /// 2. 自动赋值名称、释义、吉凶颜色、起止刻度
        /// 3. 绑定对应细分小格、加载本组子格列表
        /// </remarks>
        public void SetGrid(int Times)
        {
            int index = Times % DingLanGridNames.Length;
            this.Index = index;
            this.GridName = DingLanGrid.DingLanGridNames[index];
            this.StartValue = Math.Floor(Times * GridFixed);
            this.EndValue = Math.Floor((Times + 1) * GridFixed);
            this.Value = this.Value < 0 ? StartValue : this.Value;
            this.GridColor = DingLanGridColor[index];
            this.SG = new DingLanSubGrid(this.Value);
            this.SG.Parent = this;
            this.DingLanGridInfo = DingLanGridInfos[this.Index];
            this.Child = DingLanSubGrid.GetGroup(this.Value, this);
        }

        /// <summary>
        /// 根据大格名称初始化丁兰大格对象
        /// </summary>
        /// <param name="GridName">丁兰十大基础大格名称</param>
        /// <remarks>传入无效名称则不初始化任何属性</remarks>
        public DingLanGrid(String GridName)
        {
            int index = Array.IndexOf(DingLanGridNames, GridName);
            if (index < 0) return;

            SetGrid(index);
        }

        /// <summary>
        /// 根据具体刻度值初始化丁兰大格对象（核心构造函数）
        /// </summary>
        /// <param name="Value">输入的丁兰尺刻度值（单位：MM）</param>
        /// <remarks>
        /// 1. 刻度向下取整，统一计算精度
        /// 2. 通过刻度/单格尺寸计算所属大格循环倍数
        /// 3. 自动初始化所有大格属性、子格、吉凶数据
        /// 4. 传入负数不初始化任何数据
        /// </remarks>
        public DingLanGrid(double Value)
        {
            if (Value < 0) return;
            Value = Math.Floor(Value);
            double d = Math.Floor(Value / GridFixed);

            this.Value = Value;
            SetGrid((int)d);
        }
    }

    /// <summary>
    /// 丁兰尺细分小格计算实体类
    /// </summary>
    /// <remarks>
    /// 核心功能：解析丁兰尺最小细分刻度，每4个小格组成一个标准丁兰大格，
    /// 提供小格名称、刻度区间、父级绑定、按组获取同组4小格能力
    /// 丁兰尺单细分小格固定尺寸：9.695MM
    /// </remarks>
    private class DingLanSubGrid
    {
        /// <summary>
        /// 丁兰尺全部细分小格名称数组
        /// </summary>
        /// <value>共40个细分吉凶档位，循环匹配刻度，为丁兰尺最小刻度单元</value>
        public static readonly string[] DingLanSubGridNames = { "福星", "及第", "财旺", "登科", "口舌", "病临", "死绝", "灾至", "天德", "喜事", "进宝", "纳福", "失脱", "官鬼", "劫财", "无嗣", "大吉", "财旺", "益利", "天库", "富贵", "进宝", "横财", "顺科", "离乡", "死别", "退丁", "失财", "登科", "贵子", "添丁", "兴旺", "孤寡", "牢执", "公事", "退财", "迎福", "六合", "进宝", "财德" }; // 小格

        /// <summary>
        /// 当前小格所属的父级丁兰大格对象
        /// </summary>
        /// <value>可空，用于大小格层级绑定，读取父级大格吉凶信息</value>
        public DingLanGrid Parent { get; set; }

        /// <summary>
        /// 当前细分小格循环索引
        /// </summary>
        /// <value>对40个小格总数取模循环，对应小格名称数组下标</value>
        public int Index { get; private set; }

        /// <summary>
        /// 当前丁兰细分小格起始刻度（单位：MM，向下取整）
        /// </summary>
        public double StartValue { get; private set; }

        /// <summary>
        /// 当前丁兰细分小格结束刻度（单位：MM，向下取整）
        /// </summary>
        public double EndValue { get; private set; }

        /// <summary>
        /// 当前初始化使用的目标刻度值
        /// </summary>
        /// <remarks>默认-1为未赋值，未赋值时自动取当前小格起始刻度</remarks>
        public double Value { get; private set; } = -1;

        /// <summary>
        /// 当前小格对应的吉凶文字释义
        /// </summary>
        public string DingLanSubGridInfo { get; private set; } = "";

        /// <summary>
        /// 当前细分小格中文名称
        /// </summary>
        /// <value>取值自 DingLanSubGridNames 对应索引</value>
        public string SubGridName { get; private set; }

        /// <summary>
        /// 丁兰尺单细分小格固定标准刻度宽度
        /// </summary>
        /// <value>固定值 9.695MM，4个小格拼接为一个标准丁兰大格</value>
        public const double SubGridFixed = 9.695;

        /// <summary>
        /// 根据循环倍数初始化细分小格全部属性（私有核心初始化方法）
        /// </summary>
        /// <param name="Times">小格循环倍数，用于计算40格循环索引与刻度区间</param>
        /// <remarks>自动赋值索引、名称、起止刻度、目标值</remarks>
        private void SetSubGrid(int Times)
        {
            this.Index = Times % DingLanSubGrid.DingLanSubGridNames.Length;
            this.SubGridName = DingLanSubGrid.DingLanSubGridNames[this.Index];
            this.StartValue = Math.Floor(Times * SubGridFixed);
            this.EndValue = Math.Floor((Times + 1) * SubGridFixed);
            this.Value = this.Value < 0 ? StartValue : this.Value;
        }

        /// <summary>
        /// 获取当前刻度所属大格内的 4 个一组完整细分小格集合。
        /// </summary>
        /// <param name="value">目标丁兰尺长度刻度值（单位：毫米，MM）。</param>
        /// <param name="parent">归属的父级丁兰大格对象，若不传则默认为空（<see langword="null"/>）。</param>
        /// <returns>返回当前大格对应的连续 4 个丁兰细分小格列表。</returns>
        /// <remarks>
        /// 丁兰尺算法规则：每 4 个细分小格（分格）构成一个完整的大格单元。
        /// 该方法通过传入的任意刻度进行锚定位移，逆向推算并自动补齐当前组内的全部 4 个连续小格。
        /// </remarks>
        public static List<DingLanSubGrid> GetGroup(double value, DingLanGrid parent = null)
        {
            List<DingLanSubGrid> li = new List<DingLanSubGrid>();

            DingLanSubGrid LBSG = new DingLanSubGrid(value);

            double t = LBSG.Index - (LBSG.Index % 4) + 1;//取本组的第一个格
            double st = LBSG.EndValue - (((LBSG.Index % 4)) * SubGridFixed);
            int j = 0;
            for (double i = t; i < t + 4; i++)
            {
                var sg = new DingLanSubGrid(st + (j * SubGridFixed));
                sg.Parent = parent;
                li.Add(sg);
                j++;
            }

            return li;
        }


        /// <summary>
        /// 根据小格名称初始化丁兰细分小格对象
        /// </summary>
        /// <param name="SubGridName">细分小格中文名称</param>
        /// <remarks>传入无效名称则不初始化任何属性</remarks>
        public DingLanSubGrid(String SubGridName)
        {
            int index = Array.IndexOf(DingLanSubGridNames, SubGridName);
            if (index < 0) return;

            SetSubGrid(index);
        }

        /// <summary>
        /// 根据具体刻度值初始化丁兰细分小格对象（核心构造函数）
        /// </summary>
        /// <param name="Value">输入的丁兰尺刻度值（单位：MM）</param>
        /// <remarks>
        /// 1. 刻度向下取整，统一计算精度
        /// 2. 根据刻度计算所属小格循环倍数
        /// 3. 自动初始化小格名称、刻度区间、索引等所有属性
        /// 4. 传入负数不初始化任何数据
        /// </remarks>
        public DingLanSubGrid(double Value)
        {
            if (Value < 0) return;
            Value = Math.Floor(Value);
            double d = Math.Floor(Value / SubGridFixed);

            this.Value = Value;
            SetSubGrid((int)d);
        }
    }
}
