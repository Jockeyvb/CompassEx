



using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace  CommLib
{


    public class DingLanGrid
    {

        /// <summary>
        /// 丁兰大格名称
        /// </summary>
        public static string[] DingLanGridName = { "丁", "害", "旺", "苦", "义", "官", "死", "兴", "失", "财" }; //丁兰大格

        public static string[] DingLanGridInfos = { "生子,添丁。", "遇小人,伤害,灾害,病灾", "进财,旺盛。", "悲苦,损财,生病", "厚利,而行财运", "升官,进财利", "痼疾,重病,意外伤亡", "蒸蒸日上", "失败致破产", "财源滚滚" };

        /// <summary>
        /// 丁兰大格吉凶种颜色 （红吉，黑凶）
        /// </summary>
        public static Color[] DingLanGridColor = { Color.Red, Color.Black,Color.Red , Color.Black, Color.Red, Color.Red, Color.Black,Color.Red , Color.Black, Color.Red };

        /// <summary>
        /// 所在大格的吉凶种颜色 （红吉，黑凶）
        /// </summary>
        public Color GridColor;

        /// <summary>
        /// 相关索引 
        /// </summary>
        public int Index;
        /// <summary>
        /// 起始值
        /// </summary>
        public double StartValue;
        /// <summary>
        /// 结束值 
        /// </summary>
        public double EndValue;

        /// <summary>
        /// 大格名称
        /// </summary>
        public string GridName;
        /// <summary>
        /// 大格每格距54MM
        /// </summary>
        public const double GridFixed = 38.78;

        /// <summary>
        /// 所在值（如果根据名字获得的，则值为起始值 )
        /// </summary>
        public double Value = -1;

        /// <summary>
        /// 所在的小格
        /// </summary>
        public DingLanSubGrid SG;

        /// <summary>
        /// 　大格说明
        /// </summary>
        public string DingLanGridInfo = "";



        /// <summary>
        /// 在这大格内的所有小格
        /// </summary>
        public List<DingLanSubGrid> Child = new List<DingLanSubGrid>();

        private double rightGoodValue;




        /// <summary>
        /// 比本值要大的吉值
        /// </summary>
        public double RightGoodValue
        {
            get
            {
                if (this.Value < 0) return -1;
                if (this.GridColor == Color.Red) return -1;//如果是吉则不需要寻
                double EndValue = this.EndValue;
                for (int i = this.Index + 1; i < DingLanGridName.Length; i++)
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




        private double leftGoodValue;
        /// <summary>
        /// 比本值要少的吉值
        /// </summary>
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
        /// 根据索引设置类
        /// </summary>
        /// <param name="index"></param>
        public void SetGrid(int Times)
        {

            int index = Times % DingLanGridName.Length;
            this.Index = index;
            this.GridName = DingLanGrid.DingLanGridName[index];
            this.StartValue = Math.Floor(Times * GridFixed);
            this.EndValue = Math.Floor((Times + 1) * GridFixed);
            this.Value = this.Value < 0 ? StartValue : this.Value;
            this.GridColor = DingLanGridColor[index];
            this.SG = new DingLanSubGrid(this.Value);
            this.SG.Parent = this;
            this.DingLanGridInfo = DingLanGridInfos[this.Index];
            this.Child = DingLanSubGrid.GetGroup(this.Value, this);
            
            //for (int i = index * 4; i < (index + 1) * 4; i++)
            //{
            //    DingLanSubGrid LBSG = new DingLanSubGrid(DingLanSubGrid.DingLanSubGridName [ i] );
            //    LBSG.Parent = this;
            //    this.Child.Add(LBSG);
            //}

        }


        /// <summary>
        /// 根据大格名称，创建大格
        /// </summary>
        /// <param name="GridName"></param>
        public DingLanGrid(String GridName)
        {
            int index = Array.IndexOf(DingLanGridName, GridName);
            if (index < 0) return;

            SetGrid(index);

        }

        /// <summary>
        /// 根据刻度值 创建大格类
        /// </summary>
        /// <param name="Value"></param>
        public DingLanGrid(double Value)
        {
            if (Value < 0) return;
            Value = Math.Floor(Value);
            var a = Value / GridFixed;
            double d = Math.Floor(Value / GridFixed);
            //if (Value % GridFixed == 0 && Value > 0)//如果刚刚好，也算本格
            //{
            //    d -= 1;
            //}



            this.Value = Value;

            SetGrid((int)d);
        }



    }


    public class DingLanSubGrid
    {

        /// <summary>
        /// 丁兰小格（用","分开)
        /// </summary>
        public static string[] DingLanSubGridName = { "福星", "及第", "财旺", "登科", "口舌", "病临", "死绝", "灾至", "天德", "喜事", "进宝", "纳福", "失脱", "官鬼", "劫财", "无嗣", "大吉", "财旺", "益利", "天库", "富贵", "进宝", "横财", "顺科", "离乡", "死别", "退丁", "失财", "登科", "贵子", "添丁", "兴旺", "孤寡", "牢执", "公事", "退财", "迎福", "六合", "进宝", "财德" }; // 小格

   　
        /// <summary>
        /// 父类
        /// </summary>
        public DingLanGrid Parent;

        /// <summary>
        /// 相关索引 
        /// </summary>
        public int Index;
        /// <summary>
        /// 起始值
        /// </summary>
        public double StartValue;
        /// <summary>
        /// 结束值 
        /// </summary>
        public double EndValue;

        /// <summary>
        /// 返回实现值 （如果未设置，则返回起始值 )
        /// </summary>
        public double Value = -1;

        /// <summary>
        /// 每小格的说明
        /// </summary>
        public string DingLanSubGridInfo = "";

        /// <summary>
        /// 小格名称
        /// </summary>
        public string SubGridName;
        /// <summary>
        /// 小格每格距13.5MM
        /// </summary>
        public const double SubGridFixed = 9.695;




        /// <summary>
        /// 根据索引设置类
        /// </summary>
        /// <param name="index"></param>
        private void SetSubGrid(int Times)
        {

            this.Index = Times % DingLanSubGrid.DingLanSubGridName.Length;
            this.SubGridName = DingLanSubGrid.DingLanSubGridName[this.Index];
            this.StartValue = Math.Floor(Times * SubGridFixed);
            this.EndValue = Math.Floor((Times + 1) * SubGridFixed);
            this.Value = this.Value < 0 ? StartValue : this.Value;
           



        }


        /// <summary>
        /// 获得本大格的4个小格（为一组）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
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
        /// 根据小格名称创建小格类
        /// </summary>
        /// <param name="GridName"></param>
        /// <param name="Mod">具体index 值（不取余)</param>
        public DingLanSubGrid(String SubGridName)
        {
            int index = Array.IndexOf(DingLanSubGridName, SubGridName);
            if (index < 0) return;

            SetSubGrid(index);




        }
        /// <summary>
        /// 根据刻度值 创建小格类
        /// </summary>
        /// <param name="Value"></param>
        public DingLanSubGrid(double Value)
        {

            if (Value < 0) return;
            Value = Math.Floor(Value);
            double d = Math.Floor(Value / SubGridFixed);
            // if (Value % SubGridFixed == 0 && Value > 0) //如果刚刚好，也算本格
            //{
            //    d -= 1;
            //}
            this.Value = Value;
            //  d = d % DingLanSubGrid.DingLanSubGridName.Length;



            SetSubGrid((int)d);
        }

    }



}

