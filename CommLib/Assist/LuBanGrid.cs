using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace CommLib
{

    /// <summary>
    /// 鲁班尺大格计算实体类
    /// </summary>
    /// <remarks>
    /// 核心功能：根据输入刻度值/格位索引，解析鲁班尺八大基础格位（财、病、离、义、官、劫、害、吉），
    /// 自动匹配格位吉凶颜色、起止刻度、所属小格，同时提供左右相邻最近吉值查询能力
    /// 鲁班尺单大格固定尺寸：53.625MM，8个大格为一组循环往复
    /// </remarks>
    public class LuBanGrid
    {
        /// <summary>
        /// 鲁班尺八大基础大格名称数组（固定顺序：财、病、离、义、官、劫、害、吉）
        /// </summary>
        /// <value>固定8个基础格位名称，循环遍历计算格位索引</value>
        public static readonly string[] LuBanGridNames = { "财", "病", "离", "义", "官", "劫", "害", "吉" };

        /// <summary>
        /// 鲁班尺八大格对应吉凶颜色数组
        /// </summary>
        /// <value>Red=吉格，Black=凶格，与 LuBanGridNames 索引一一对应</value>
        public static readonly Color[] LuBanGridColor = { Color.Red, Color.Black, Color.Black, Color.Red, Color.Red, Color.Black, Color.Black, Color.Red };

        /// <summary>
        /// 当前大格的吉凶展示颜色
        /// </summary>
        /// <value>根据当前格位索引自动匹配，红为吉、黑为凶</value>
        public Color GridColor { get; private set; }

        /// <summary>
        /// 当前大格在八大格组中的索引（0-7 循环）
        /// </summary>
        /// <value>对应 LuBanGridNames、LuBanGridColor 数组下标</value>
        public int Index { get; private set; }

        /// <summary>
        /// 当前鲁班大格的起始刻度（单位：MM，向下取整）
        /// </summary>
        public double StartValue { get; private set; }

        /// <summary>
        /// 当前鲁班大格的结束刻度（单位：MM，向下取整）
        /// </summary>
        public double EndValue { get; private set; }

        /// <summary>
        /// 当前大格的中文名称
        /// </summary>
        /// <value>取值于 LuBanGridNames 对应索引名称</value>
        public string GridName { get; private set; }

        /// <summary>
        /// 鲁班尺单一大格固定标准尺寸
        /// </summary>
        /// <value>固定值 53.625MM，为鲁班尺标准单格刻度宽度</value>
        public const double GridFixed = 53.625;

        /// <summary>
        /// 当前传入的目标刻度值（单位：MM）
        /// </summary>
        /// <remarks>默认初始值 -1 代表未赋值；根据名称初始化时，值默认取当前格起始值</remarks>
        public double Value { get; private set; } = -1;

        /// <summary>
        /// 当前刻度归属的鲁班尺最小细分格实体
        /// </summary>
        /// <value>基于当前 Value 解析对应的子格，绑定父级大格对象</value>
        public LuBanSubGrid SG { get; private set; }

        /// <summary>
        /// 当前大格下包含的所有鲁班尺细分小格集合
        /// </summary>
        /// <value>自动加载当前大格对应的全部子格数据</value>
        public List<LuBanSubGrid> Child { get; private set; } = new List<LuBanSubGrid>();

        /// <summary>
        /// 私有字段：缓存右侧最近吉值（大于当前刻度的最小吉格起始值）
        /// </summary>
        private double rightGoodValue;

        /// <summary>
        /// 获取【大于当前刻度】的最近吉祥刻度值（向右寻吉）
        /// </summary>
        /// <returns>
        /// -1：未赋值 / 当前格本身为吉格，无需寻吉；
        /// 有效值：右侧最近吉格的起始刻度值
        /// </returns>
        /// <remarks>仅当前格为凶格时生效，从当前下一格开始向后遍历吉格，取首个吉格起始值</remarks>
        public double RightGoodValue
        {
            get
            {
                if (this.Value < 0) return -1;
                if (this.GridColor == Color.Red) return -1;// 当前为吉格，无需查找右侧吉值

                double endVal = this.EndValue;
                // 从当前下一格开始向后遍历所有大格
                for (int i = this.Index + 1; i < LuBanGridNames.Length; i++)
                {
                    endVal = Math.Floor(endVal + GridFixed) - 1;
                    // 匹配首个吉色格位
                    if (LuBanGridColor[i] == Color.Red)
                    {
                        LuBanGrid lbg = new LuBanGrid(endVal);
                        rightGoodValue = lbg.StartValue + 1;
                        break;
                    }
                }
                return rightGoodValue;
            }
        }

        /// <summary>
        /// 私有字段：缓存左侧最近吉值（小于当前刻度的最大吉格结束值）
        /// </summary>
        private double leftGoodValue;

        /// <summary>
        /// 获取【小于当前刻度】的最近吉祥刻度值（向左寻吉）
        /// </summary>
        /// <returns>
        /// -1：未赋值 / 当前格本身为吉格，无需寻吉；
        /// 有效值：左侧最近吉格的结束刻度值
        /// </returns>
        /// <remarks>仅当前格为凶格时生效，从当前上一格开始向前遍历吉格，取首个吉格结束值</remarks>
        public double LeftGoodValue
        {
            get
            {
                if (this.Value < 0) return -1;
                if (this.GridColor == Color.Red) return -1;// 当前为吉格，无需查找左侧吉值

                double endVal = this.EndValue;
                // 从当前上一格开始向前遍历所有大格
                for (int i = this.Index - 1; i >= 0; i--)
                {
                    endVal = Math.Floor(endVal - GridFixed);
                    // 匹配首个吉色格位
                    if (LuBanGridColor[i] == Color.Red)
                    {
                        LuBanGrid lbg = new LuBanGrid(endVal);
                        leftGoodValue = lbg.EndValue;
                        break;
                    }
                }
                return leftGoodValue;
            }
        }

        /// <summary>
        /// 根据循环倍数索引，初始化当前大格所有属性
        /// </summary>
        /// <param name="Times">格位循环倍数，用于计算8格循环后的实际索引和刻度区间</param>
        /// <remarks>
        /// 1. 对8格取模得到当前基础格索引
        /// 2. 自动计算当前格起止刻度、名称、吉凶颜色
        /// 3. 绑定当前归属细分小格、加载当前大格下所有子格数据
        /// </remarks>
        public void SetGrid(int Times)
        {
            // 8大格循环取模，获取基础格索引
            int index = Times % LuBanGridNames.Length;
            this.Index = index;
            this.GridName = LuBanGrid.LuBanGridNames[index];
            // 计算当前格起止刻度（向下取整，贴合鲁班尺刻度规则）
            this.StartValue = Math.Floor(Times * GridFixed);
            this.EndValue = Math.Floor((Times + 1) * GridFixed);
            // 未手动赋值时，默认取值为格起始刻度
            this.Value = this.Value < 0 ? StartValue : this.Value;
            this.GridColor = LuBanGridColor[index];
            // 绑定当前刻度对应的细分小格
            this.SG = new LuBanSubGrid(this.Value);
            this.SG.Parent = this;
            // 加载当前大格下所有细分小格子集
            this.Child = LuBanSubGrid.GetGroup(this.Value, this);
        }

        /// <summary>
        /// 通过【大格名称】初始化鲁班大格对象
        /// </summary>
        /// <param name="GridName">八大格名称（财/病/离/义/官/劫/害/吉）</param>
        /// <remarks>传入无效名称则不初始化任何属性</remarks>
        public LuBanGrid(string GridName)
        {
            // 根据名称匹配索引
            int index = Array.IndexOf(LuBanGridNames, GridName);
            if (index < 0) return;

            // 根据索引初始化格位数据
            SetGrid(index);
        }

        /// <summary>
        /// 通过【具体刻度值】初始化鲁班大格对象（核心构造函数）
        /// </summary>
        /// <param name="Value">输入的鲁班尺刻度值（单位：MM）</param>
        /// <remarks>
        /// 1. 刻度值向下取整，适配鲁班尺整数刻度规则
        /// 2. 通过刻度/单格尺寸计算所属格位循环倍数
        /// 3. 自动初始化当前格所有属性、子格、吉凶数据
        /// 4. 传入负数不初始化任何数据
        /// </remarks>
        public LuBanGrid(double Value)
        {
            if (Value < 0) return;
            // 刻度向下取整，统一计算精度
            Value = Math.Floor(Value);
            // 计算当前刻度归属的大格循环倍数
            double d = Math.Floor(Value / GridFixed);

            this.Value = Value;
            // 根据倍数初始化格位信息
            SetGrid((int)d);
        }
    }

    /// <summary>
    /// 鲁班尺细分小格计算实体类
    /// </summary>
    /// <remarks>
    /// 核心功能：用于解析鲁班尺最小细分刻度，每4个小格组成一个鲁班大格，
    /// 提供小格名称、吉凶释义、刻度区间、所属父级大格，支持按刻度/名称初始化、按组获取同组4小格数据
    /// 鲁班尺单小格固定尺寸：13.40625MM
    /// </remarks>
    public class LuBanSubGrid
    {
        /// <summary>
        /// 鲁班尺全部细分小格名称数组（共32个细分吉凶档位，循环匹配）
        /// </summary>
        /// <remarks>存储所有鲁班尺最小格位名称，与 LuBanSubGridInfos 索引一一对应</remarks>
        public static readonly string[] LuBanSubGridNames = {
        "财德", "宝库", "六合", "迎福",
        "退财", "公事", "牢执", "孤寡",
        "长库", "劫财", "官鬼", "失脱",
        "添丁", "益利", "贵子", "大吉",
        "顺科", "横财", "进益", "富贵",
        "死别", "退口", "离乡", "失财",
        "灾至", "死绝", "病临", "口舌",
        "财至", "登科", "进宝", "兴旺"
    };

        /// <summary>
        /// 鲁班尺细分小格释义说明数组
        /// </summary>
        /// <remarks>存储每个小格对应的吉凶文字解释，与 LuBanSubGridNames 索引一一对应</remarks>
        private static readonly string[] LuBanSubGridInfos = {
        "指在财，德善，功德方面有表现。",
        "比喻可得或储藏珍贵物品。",
        "合和美满。六合为天地四方。",
        "迎接福。福为幸福，利益。",
        "损财，破财之意。",
        "多指因公家的事如贪污受贿及案件官司等。",
        "指牢狱之灾。",
        "指有孤独寡居的行为。",
        "古有监狱之说。",
        "破耗及耗损财。",
        "指有官煞引起之事。",
        "物品失落、人离散之意。",
        "古时生男孩叫添丁",
        "增加了财资利禄。",
        "日后能显贵的子嗣。",
        "吉祥吉利。",
        "顺利通过考试而获中。",
        "意外之财。",
        "收益进益。",
        "有财有势。",
        "即永别。",
        "指有孝服之事。",
        "背井离乡。",
        "财物损失或丢失。",
        "灾殃祸患到。",
        "死得干干净净。",
        "疾病来临。",
        "争执争吵。",
        "即财到。",
        "考试被录取。",
        "招财进宝。",
        "兴盛旺盛。"
    };

        /// <summary>
        /// 当前小格所属的父级鲁班大格对象
        /// </summary>
        /// <value>可空，用于绑定大小格层级关系，获取父级大格信息</value>
        public LuBanGrid Parent { get; set; }

        /// <summary>
        /// 当前细分小格在32格序列中的循环索引
        /// </summary>
        /// <value>对32取模循环，对应名称、释义数组下标</value>
        public int Index { get; private set; }

        /// <summary>
        /// 当前鲁班细分小格起始刻度（单位：MM，向下取整）
        /// </summary>
        public double StartValue { get; private set; }

        /// <summary>
        /// 当前鲁班细分小格结束刻度（单位：MM，向下取整）
        /// </summary>
        public double EndValue { get; private set; }

        /// <summary>
        /// 当前初始化使用的目标刻度值
        /// </summary>
        /// <value>默认-1为未赋值，未赋值时自动取当前小格起始刻度</value>
        public double Value { get; private set; } = -1;

        /// <summary>
        /// 当前小格对应的吉凶文字释义
        /// </summary>
        public string LuBanSubGridInfo { get; private set; } = "";

        /// <summary>
        /// 当前细分小格中文名称
        /// </summary>
        /// <value>取值自 LuBanSubGridNames 对应索引</value>
        public string SubGridName { get; private set; }

        /// <summary>
        /// 鲁班尺单细分小格固定标准尺寸
        /// </summary>
        /// <remarks>固定值 13.40625MM，4个小格拼接为一个大格 53.625MM</remarks>
        public const double SubGridFixed = 13.40625;


        /// <summary>
        /// 根据循环倍数初始化当前细分小格全部属性（私有核心初始化方法）
        /// </summary>
        /// <param name="Times">小格循环倍数，用于计算32格循环索引与刻度区间&lt;/param&gt;
        /// <remarks>自动赋值索引、名称、释义、起止刻度、目标值</remarks>
        private void SetSubGrid(int Times)
        {
            this.Index = Times % LuBanSubGrid.LuBanSubGridNames.Length;
            this.SubGridName = LuBanSubGrid.LuBanSubGridNames[this.Index];
            this.StartValue = Math.Floor(Times * SubGridFixed);
            this.EndValue = Math.Floor((Times + 1) * SubGridFixed);
            this.Value = this.Value < 0 ? StartValue : this.Value;
            this.LuBanSubGridInfo = LuBanSubGridInfos[this.Index];
        }

        /// <summary>
        /// 获取当前刻度所属大格内的 4 个一组完整细分小格集合。
        /// </summary>
        /// <param name="value">目标鲁班尺长度刻度值（单位：毫米，MM）。</param>
        /// <param name="parent">归属的父级鲁班大格对象，若不传则默认为空（<see langword="null"/>）。</param>
        /// <returns>返回当前大格对应的连续 4 个鲁班细分小格列表。</returns>
        /// <remarks>
        /// 鲁班尺算法规则：每 4 个细分小格（分格）构成一个完整的大格单元。<br/>
        /// 该方法通过传入的任意刻度进行锚定位移，逆向推算并自动补齐当前组内的全部 4 个连续小格。
        /// </remarks>
        public static List<LuBanSubGrid> GetGroup(double value, LuBanGrid parent = null)
        {
            List<LuBanSubGrid> li = new List<LuBanSubGrid>();

            LuBanSubGrid LBSG = new LuBanSubGrid(value);

            // 计算本组4格的起始序号
            double t = LBSG.Index - (LBSG.Index % 4) + 1;
            // 计算本组4格的起始刻度
            double st = LBSG.EndValue - (((LBSG.Index % 4)) * SubGridFixed);
            int j = 0;
            // 循环生成当前大格内连续4个小格
            for (double i = t; i < t + 4; i++)
            {
                var sg = new LuBanSubGrid(st + (j * SubGridFixed));
                sg.Parent = parent;
                li.Add(sg);
                j++;
            }

            return li;
        }


        /// <summary>
        /// 根据小格中文名称初始化鲁班细分小格对象。
        /// </summary>
        /// <param name="SubGridName">要匹配的鲁班尺 32 种细分小格名称（例如“大吉”、“财德”）。</param>
        /// <remarks>
        /// 1. 方法会检索传入名称在 <see cref="LuBanSubGridNames"/> 数组中的位置索引。<br/>
        /// 2. 若传入无效或不存在的名称（找不到对应索引），则触发健壮性拦截，提前终止后续的所有初始化配置。
        /// </remarks>
        public LuBanSubGrid(string SubGridName)
        {
            int index = Array.IndexOf(LuBanSubGridNames, SubGridName);
            if (index < 0) return;

            SetSubGrid(index);
        }


        /// <summary>
        /// 根据具体的物理刻度值初始化鲁班细分小格对象。
        /// </summary>
        /// <param name="Value">输入的鲁班尺刻度值（单位：毫米，MM）。</param>
        /// <remarks>
        /// 1. 刻度值会自动向下取整（<see cref="Math.Floor(double)"/>），确保系统计算精度统一。<br/>
        /// 2. 根据取整后的刻度与每格固定尺寸（<see cref="SubGridFixed"/>）计算出所属的小格累计步数。<br/>
        /// 3. 自动匹配并加载对应的循环小格中文名称、吉凶断语释义、理论刻度区间及循环索引。<br/>
        /// 4. 传入负数刻度属非法输入，将直接拦截并提前终止后续的所有初始化配置。
        /// </remarks>
        public LuBanSubGrid(double Value)
        {
            if (Value < 0) return;
            Value = Math.Floor(Value);
            double d = Math.Floor(Value / SubGridFixed);

            this.Value = Value;

            SetSubGrid((int)d);
        }

    }


}
