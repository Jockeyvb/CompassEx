// // Copyright (c) 2026 [Jockeyvb]. All rights reserved.
// // 
// // This file is part of [CompassEx].
// // [CompassEx] is free software: you can redistribute it and/or modify
// // it under the terms of the GNU Affero General Public License as published by
// // the Free Software Foundation, either version 3 of the License, or
// // (at your option) any later version.
// //
// // For commercial use, you must obtain a commercial license from the author.
// // Contact: [Jockeyvb@gmail.com/微信:Jockeyvb1]
//

using CompassEx.Comm;
using CompassEx.Gua;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace CompassEx
{
    /// <summary>
    /// 三元罗盘类。
    /// </summary>
    /// <remarks>
    /// 本类继承自 <see cref="CompassEx"/>，主要用于处理三元罗盘体系下的先天六十四卦（天盘）与后天六十四卦（地盘）的度数映射与卦象计算。
    /// </remarks>
    public class C3Y : CompassEx
    {

        /// <summary>
        /// 罗盘六十四卦的单卦度数常量。
        /// </summary>
        /// <value>
        /// 默认值为 <c>5.625</c> 度。计算公式为：360度 / 64卦。
        /// </value>
        public const double CompassGuaDegree = 5.625;

        /// <summary>
        /// 每一爻（Gua Yao）所占用的罗盘度数常量。
        /// </summary>
        /// <value>
        /// 默认值约为 <c>0.9375</c> 度。计算公式为：单卦度数 <see cref="CompassGuaDegree"/> / 6爻。
        /// </value>
        /// <remarks>
        /// 本常量用于在罗盘排盘计算时，进一步精细定位当前度数处于某卦的哪一爻（初爻至上爻）。
        /// </remarks>
        public const double GuaYaosDegree = CompassGuaDegree / 6;



        /// <summary>
        /// 获取当前罗盘度数对应的先天 64 卦对象（天盘）。
        /// </summary>
        /// <value>
        /// 返回一个 <see cref="GuaClass"/> 对象，表示当前罗盘度数在天盘上对应的先天 64 卦象。
        /// </value>
        public GuaClass BeforGua { get; protected set; }

        /// <summary>
        /// 先天 64 卦对象字典缓存（天盘）。
        /// </summary>
        /// <value>
        /// 键为 <see cref="CompassRangEX"/> 范围，值为对应的 <see cref="GuaClass"/> 卦象对象。
        /// </value>
        /// <remarks>
        /// <para><b>性能说明：</b></para>
        /// <para>使用字典缓存所有 64 卦对象，避免每次计算时重复创建新对象，提高执行性能。系统启动时需要优先初始化此字典。</para>
        /// <para><b>排列规则：</b></para>
        /// <para>罗盘上先天排行从午为乾1、兑2、离3、震4（阳仪），巽5、坎6、艮7、坤8（阴仪）。</para>
        /// <para><b>度数映射：</b></para>
        /// <para>罗盘上子坤（360度）至午乾（180度）。从坤 0 度至天风姤 180 度，乾左为顺 180 度至地雷复 360 度。内卦为卦宫，外卦相荡而成。</para>
        /// </remarks>
        [JsonIgnore]
        public static Dictionary<CompassRangEX, GuaClass> CBeforeGuas;

        /// <summary>
        /// 后天 64 卦对象字典缓存（地盘）。
        /// </summary>
        /// <value>
        /// 键为 <see cref="CompassRangEX"/> 范围，值为对应的 <see cref="GuaClass"/> 卦象对象。
        /// </value>
        /// <remarks>
        /// <para><b>性能说明：</b></para>
        /// <para>使用字典缓存所有 64 卦对象，避免每次计算时重复创建新对象，提高执行性能。系统启动时需要优先初始化此字典。</para>
        /// <para><b>排列规则：</b></para>
        /// <para>罗盘上后天排行从子为乾9、兑4、离3、震8（阳仪），巽2、坎7、艮6、坤1（阴仪）。</para>
        /// <para><b>度数映射：</b></para>
        /// <para>罗盘上子乾（360度）至午坤（180度）。从乾 0 度至雷地豫 180 度，坤左为顺 180 度至地雷复 360 度。外卦为卦宫，内卦相荡而成。</para>
        /// </remarks>
        [JsonIgnore]
        public static Dictionary<CompassRangEX, GuaClass> CAfterGuas;

        /// <summary>
        /// 初始化 <see cref="C3Y"/> 类的新实例。
        /// </summary>
        /// <param name="Degreen">当前罗盘的初始度数（角度值值）。</param>
        /// <remarks>
        /// 构造函数在初始化时，会自动计算并填充当前度数对应的地盘后天卦（通过调用 <c>GetAfterGuaSub()</c>）以及天盘先天卦（通过调用 <c>GetCBeforeGua()</c>）。
        /// </remarks>
        public C3Y(double Degreen) : base(Degreen)
        {
            this.AfterGuaSub = GetAfterGuaSub();
            this.BeforGua = GetCBeforeGua();
        }


        /// <summary>
        /// 获取指定先天（天盘）64 卦卦名在罗盘上对应的度数范围对象。
        /// </summary>
        /// <param name="Name">要查询的先天 64 卦的完整卦名。</param>
        /// <returns>返回对应的 <see cref="CompassRangEX"/> 度数范围对象；若在缓存中未匹配到该卦名，则返回 <c>null</c>。</returns>
        public static CompassRangEX? GetCBeforeGuaDegree(string Name)
        {
            foreach (var kv in C3Y.CBeforeGuas)
            {
                if (kv.Value.Name == Name)
                {
                    return kv.Key;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取指定后天（地盘）64 卦卦名在罗盘上对应的度数范围对象。
        /// </summary>
        /// <param name="Name">要查询的后天 64 卦的完整卦名。</param>
        /// <returns>返回对应的 <see cref="CompassRangEX"/> 度数范围对象；若在缓存中未匹配到该卦名，则返回 <c>null</c>。</returns>
        public static CompassRangEX? GetCAfterGuaDegree(string Name)
        {
            foreach (var kv in CAfterGuas)
            {
                if (kv.Value.Name == Name)
                {
                    return kv.Key;
                }
            }
            return null;
        }

        /// <summary>
        /// 根据当前罗盘指向的度数，匹配并获取对应的先天 64 卦对象。
        /// </summary>
        /// <returns>返回当前度数所在的 <see cref="GuaClass"/> 先天 64 卦对象；若未找到匹配的范围则返回 <c>null</c>。</returns>
        public GuaClass? GetCBeforeGua()
        {
            foreach (var kv in CBeforeGuas)
            {
                if (kv.Key.IsInRange(this.degree))
                {
                    return kv.Value;
                }
            }
            return null;
        }



        /// <summary>
        /// 从数据源加载并初始化罗盘上的所有后天 64 卦（地盘）全局静态缓存字典。
        /// </summary>
        /// <remarks> 
        /// <para>该方法应在程序启动或系统初始化阶段优先执行。</para>
        /// </remarks>
        public static void LoadAllCAfterGuas()
        {
            CAfterGuas = C3Y.GetAllCAfterGuas();
        }


        /// <summary>
        /// 从数据源加载并初始化罗盘上的所有先天 64 卦（天盘）全局静态缓存字典。
        /// </summary>
        /// <remarks>
        /// <para><b>排列规则：</b>卦象按照顺时针方向在罗盘上排布，从坤卦开始，每个卦位占据 5.625 度（逆时针荡卦推演），共计 64 个卦象。</para>
        /// <para>该方法应在程序启动或系统初始化阶段优先执行。</para>
        /// </remarks>
        public static void LoadAllCBeforeGuas()
        {
            CBeforeGuas = GetAllBeforGuas();
        }


        /// <summary>
        ///加载并初始化罗盘上的所有后天64卦（地盘）对象，按照顺时针方向排列，从坤卦开始，每5.625度一个卦逆时针，共64个卦
        /// </summary>
        /// <returns></returns>
        public static Dictionary<CompassRangEX, GuaClass> GetAllCAfterGuas()
        {


            //罗盘上后天排行从子为乾9、兑4、离3、震8（阳仪），巽2、坎7、艮6、坤1午（阴仪）
            //实际是按先天卦排名索引，乾1、兑2、离3、震4（阳仪），巽5、坎6、艮7、坤8午（阴仪）
            //并把乾(360度）置上卦为卦宫，下卦相荡而成
            double baseDegree = 360;//罗盘360-5.625=354.375，则354.375至360度为坤卦
            double dEnd = baseDegree;
            Dictionary<CompassRangEX, GuaClass> dc = new Dictionary<CompassRangEX, GuaClass>();
            //--------------------阳仪32卦------------------
            for (int i = 0; i < 4; i++) //至震
            {
                // GuaSubClass gu = GuaSubClass.GetGuaSub(i, true); //上卦 
                string sN = GuaSubClass.BeforeGuaSubNames[i]; //按先天创建
                GuaSubClass gu = GuaSubClass.GetGuaSub(GuaSubClass.AfterGuaSubNames.IndexOf(sN), false);


                for (int j = 0; j < 8; j++)//按1-8卦相荡(下卦）顺
                {



                    //GuaSubClass gd = GuaSubClass.GetGuaSub(j, false); //下卦 
                    sN = GuaSubClass.BeforeGuaSubNames[j]; //按先天创建
                    GuaSubClass gd = GuaSubClass.GetGuaSub(GuaSubClass.AfterGuaSubNames.IndexOf(sN), true);


                    List<int> iYao = new List<int>();
                    iYao.AddRange(gd.Yaos.Select(x => x.Value));//相荡
                    iYao.AddRange(gu.Yaos.Select(x => x.Value));//卦宫

                    GuaClass g = GuaClass.GetGuaClass(iYao.ToArray()); //根据六爻数获得64卦对象

                    CompassRangEX rang = new CompassRangEX(dEnd - CompassGuaDegree, dEnd); //范围
                    dEnd = dEnd - CompassGuaDegree;
                    dc.Add(rang, g);  //范围对象作为key，卦对象作为value添加到字典中
                }

            }
            //--------------------阳仪32卦------------------

            //---------------------阴仪32卦----------------------------------

            for (int i = 7; i > 3; i--)
            {
                //  GuaSubClass gu = GuaSubClass.GetGuaSub(i, true); //上卦 
                string sN = GuaSubClass.BeforeGuaSubNames[i]; //按先天创建
                GuaSubClass gu = GuaSubClass.GetGuaSub(GuaSubClass.AfterGuaSubNames.IndexOf(sN), false);


                for (int j = 7; j > -1; j--)//按8-1卦相荡(下卦）逆
                {
                    //GuaSubClass gd = GuaSubClass.GetGuaSub(j, false); //下卦
                    sN = GuaSubClass.BeforeGuaSubNames[j]; //按先天创建
                    GuaSubClass gd = GuaSubClass.GetGuaSub(GuaSubClass.AfterGuaSubNames.IndexOf(sN), true);


                    List<int> iYao = new List<int>();
                    iYao.AddRange((IEnumerable<int>)gd.Yaos.Select(x => x.Value));
                    iYao.AddRange((IEnumerable<int>)gu.Yaos.Select(x => x.Value));
                    GuaClass g = GuaClass.GetGuaClass(iYao.ToArray()); //根据六爻数获得64卦对象
                    CompassRangEX rang = new CompassRangEX(dEnd - CompassGuaDegree, dEnd); //范围
                    dEnd = dEnd - CompassGuaDegree;
                    dc.Add(rang, g);  //范围对象作为key，卦对象作为value添加到字典中
                }
            }

            //---------------------阴仪32卦----------------------------------

            return dc;

        }






        /// <summary>
        ///加载罗盘上的所有先天64卦（天盘）对象，按照顺时针方向排列，从坤卦开始，每5.625度一个卦逆时针，共64个卦
        /// </summary>
        /// <returns></returns>
        public static Dictionary<CompassRangEX, GuaClass> GetAllBeforGuas()
        {
            double baseDegree = 360;//罗盘360-5.625=354.375，则354.375至360度为坤卦
            double dEnd = baseDegree;
            Dictionary<CompassRangEX, GuaClass> dc = new Dictionary<CompassRangEX, GuaClass>();
            //--------------------//阴从右边道相通------------------
            for (int i = 7; i > 3; i--)
            {
                //   GuaSubClass gd = GuaSubClass.GetGuaSub(i, true); //下卦（从坤右边转相荡至巽）
                string sN = GuaSubClass.BeforeGuaSubNames[i]; //按先天创建
                GuaSubClass gd = GuaSubClass.GetGuaSub(GuaSubClass.AfterGuaSubNames.IndexOf(sN), true);




                for (int j = 7; j > -1; j--)
                {
                    //GuaSubClass gu = GuaSubClass.GetGuaSub(j, false); //上卦（从坤右边转相荡至乾）
                    sN = GuaSubClass.BeforeGuaSubNames[j]; //按先天创建
                    GuaSubClass gu = GuaSubClass.GetGuaSub(GuaSubClass.AfterGuaSubNames.IndexOf(sN), false);


                    List<int> iYao = new List<int>();
                    iYao.AddRange((IEnumerable<int>)gd.Yaos.Select(x => x.Value));
                    iYao.AddRange((IEnumerable<int>)gu.Yaos.Select(x => x.Value));
                    GuaClass g = GuaClass.GetGuaClass(iYao.ToArray()); //根据六爻数获得64卦对象

                    CompassRangEX rang = new CompassRangEX(dEnd - CompassGuaDegree, dEnd); //范围
                    dEnd = dEnd - CompassGuaDegree;
                    dc.Add(rang, g);  //范围对象作为key，卦对象作为value添加到字典中
                }

            }
            //--------------------//阴从右边道相通------------------

            //---------------------阳从左边团团转----------------------------------

            for (int i = 0; i < 4; i++)
            {
                //   GuaSubClass gd = GuaSubClass.GetGuaSub(i, true); //下卦（从乾左边转相荡至震）
                string sN = GuaSubClass.BeforeGuaSubNames[i]; //按先天创建
                GuaSubClass gd = GuaSubClass.GetGuaSub(GuaSubClass.AfterGuaSubNames.IndexOf(sN), true);


                for (int j = 0; j < 8; j++)
                {
                    //  GuaSubClass gu = GuaSubClass.GetGuaSub(j, false); //上卦（从乾左边转相荡至坤）
                    sN = GuaSubClass.BeforeGuaSubNames[j]; //按先天创建
                    GuaSubClass gu = GuaSubClass.GetGuaSub(GuaSubClass.AfterGuaSubNames.IndexOf(sN), false);


                    List<int> iYao = new List<int>();
                    iYao.AddRange((IEnumerable<int>)gd.Yaos.Select(x => x.Value));
                    iYao.AddRange((IEnumerable<int>)gu.Yaos.Select(x => x.Value));
                    GuaClass g = GuaClass.GetGuaClass(iYao.ToArray()); //根据六爻数获得64卦对象
                    CompassRangEX rang = new CompassRangEX(dEnd - CompassGuaDegree, dEnd); //范围
                    dEnd = dEnd - CompassGuaDegree;
                    dc.Add(rang, g);  //范围对象作为key，卦对象作为value添加到字典中
                }
            }

            //---------------------阳从左边团团转----------------------------------

            return dc;

        }


    }
}
