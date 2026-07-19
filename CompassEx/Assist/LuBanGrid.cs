using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace  CommLib
{
    

    public   class LuBanGrid
    {

        /// <summary>
        /// 鲁班大格名称
        /// </summary>
        public static  string[] LuBanGridName = { "财", "病", "离", "义", "官", "劫", "害", "吉" }; //鲁班大格



        /// <summary>
        /// 鲁班大格吉凶种颜色 （红吉，黑凶）
        /// </summary>
        public static Color[] LuBanGridColor = { Color.Red, Color.Black, Color.Black, Color.Red, Color.Red, Color.Black, Color.Black, Color.Red };

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
        public    string GridName;
        /// <summary>
        /// 大格每格距54MM
        /// </summary>
        public  const    double GridFixed = 53.625;

        /// <summary>
        /// 所在值（如果根据名字获得的，则值为起始值 )
        /// </summary>
        public double Value=-1;

        /// <summary>
        /// 所在的小格
        /// </summary>
        public LuBanSubGrid SG;

        /// <summary>
        /// 在这大格内的所有小格
        /// </summary>
        public List<LuBanSubGrid> Child=new List<LuBanSubGrid> ();

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
                for (int i = this.Index +1; i < LuBanGridName.Length ; i++)
                {
                    EndValue = Math.Floor ( EndValue + GridFixed)-1;
                    if (LuBanGridColor[i] == Color.Red)
                    {
                        LuBanGrid LBG = new LuBanGrid(EndValue);
                        rightGoodValue = LBG.StartValue +1 ;
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
        public double LeftGoodValue { get 
            {
                if (this.Value  < 0) return -1;
                if (this.GridColor == Color.Red) return -1;//如果是吉则不需要寻
                double EndValue = this.EndValue;
                for (int i=this.Index -1;i>=0; i--)
                {
                    EndValue = Math.Floor ( EndValue - GridFixed) ;
                    if ( LuBanGridColor[i]==Color.Red)
                    {
                        LuBanGrid LBG = new LuBanGrid(EndValue);
                        leftGoodValue = LBG.EndValue;
                        break;


                    }
                }
                return  leftGoodValue;
            } 

        }


        /// <summary>
        /// 根据索引设置类
        /// </summary>
        /// <param name="index"></param>
        public void SetGrid(int Times)
        {
          
            int index = Times % LuBanGridName.Length;
            this.Index = index;
            this.GridName = LuBanGrid.LuBanGridName[index];
            this.StartValue = Math.Floor(Times * GridFixed);
            this.EndValue = Math.Floor((Times + 1) * GridFixed) ;
            this.Value = this.Value < 0 ? StartValue : this.Value;
            this.GridColor = LuBanGridColor[index];
            this.SG = new LuBanSubGrid(this.Value);
            this.SG.Parent = this;
            this.Child = LuBanSubGrid.GetGroup(this.Value,this);
            //for (int i = index * 4; i < (index + 1) * 4; i++)
            //{
            //    LuBanSubGrid LBSG = new LuBanSubGrid(LuBanSubGrid.LuBanSubGridName [ i] );
            //    LBSG.Parent = this;
            //    this.Child.Add(LBSG);
            //}

        }

        
        /// <summary>
        /// 根据大格名称，创建大格
        /// </summary>
        /// <param name="GridName"></param>
        public LuBanGrid(String GridName)
        {
            int index = Array.IndexOf(LuBanGridName, GridName);
            if (index < 0) return;

            SetGrid(index);

        }

        /// <summary>
        /// 根据刻度值 创建大格类
        /// </summary>
        /// <param name="Value"></param>
        public LuBanGrid(double Value)
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


    public class LuBanSubGrid
    {

        /// <summary>
        /// 鲁班小格（用","分开)
        /// </summary>
        public static string[] LuBanSubGridName = { "财德","宝库","六合","迎福", "退财","公事","牢执","孤寡", "长库","劫财","官鬼","失脱", "添丁","益利","贵子","大吉", "顺科","横财","进益","富贵", "死别","退口","离乡","失财", "灾至","死绝","病临","口舌", "财至","登科","进宝","兴旺" }; //

        private  static string[] LuBanSubGridInfos = { "指在财，德善，功德方面有表现。", "比喻可得或储藏珍贵物品。", "合和美满。六合为天地四方。", "迎接福。福为幸福，利益。", "损财，破财之意。", "多指因公家的事如贪污受贿及案件官司等。", "指牢狱之灾。", "指有孤独寡居的行为。", "古有监狱之说。" , "破耗及耗损财。", "指有官煞引起之事。", "物品失落、人离散之意。", "古时生男孩叫添丁", "增加了财资利禄。", "日后能显贵的子嗣。", "吉祥吉利。", "顺利通过考试而获中。", "意外之财。", "收益进益。", "有财有势。", "即永别。", "指有孝服之事。", "背井离乡。", "财物损失或丢失。", "灾殃祸患到。" , "死得干干净净。", "疾病来临。", "争执争吵。", "即财到。", "考试被录取。", "招财进宝。", "兴盛旺盛。" };

        /// <summary>
        /// 父类
        /// </summary>
        public LuBanGrid Parent;

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
        public string LuBanSubGridInfo = "";

        /// <summary>
        /// 小格名称
        /// </summary>
        public string SubGridName;
        /// <summary>
        /// 小格每格距13.5MM
        /// </summary>
        public const  double SubGridFixed = 13.40625;




        /// <summary>
        /// 根据索引设置类
        /// </summary>
        /// <param name="index"></param>
        private void SetSubGrid(int Times)
        {

            this.Index = Times % LuBanSubGrid.LuBanSubGridName.Length;  
            this.SubGridName = LuBanSubGrid.LuBanSubGridName[this.Index];
            this.StartValue = Math.Floor ( Times * SubGridFixed);
            this.EndValue = Math.Floor((Times + 1) * SubGridFixed) ;
            this.Value = this.Value < 0 ? StartValue : this.Value;
            this.LuBanSubGridInfo = LuBanSubGridInfos[this.Index];
            


    }


        /// <summary>
        /// 获得本大格的4个小格（为一组）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static  List<LuBanSubGrid > GetGroup( double value , LuBanGrid parent =null )
        {
            List<LuBanSubGrid> li = new List<LuBanSubGrid>();

            LuBanSubGrid LBSG = new LuBanSubGrid(value);

            double t =LBSG.Index - (LBSG.Index % 4) +1;//取本组的第一个格
            double st = LBSG.EndValue  - (((LBSG.Index % 4) ) * SubGridFixed);
            int j = 0;
            for ( double i=t; i < t+ 4; i++)
            {
                var sg = new LuBanSubGrid(st + (j * SubGridFixed));
                sg.Parent = parent;

                li.Add( sg );
                j++;
            }

            return li;

        }

        /// <summary>
        /// 根据小格名称创建小格类
        /// </summary>
        /// <param name="GridName"></param>
        /// <param name="Mod">具体index 值（不取余)</param>
        public LuBanSubGrid(String SubGridName )
        {
            int index = Array.IndexOf(LuBanSubGridName, SubGridName);
            if (index < 0) return;
            
            SetSubGrid(index  );
            
        


        }
        /// <summary>
        /// 根据刻度值 创建小格类
        /// </summary>
        /// <param name="Value"></param>
        public LuBanSubGrid( double  Value )
        {

            if (Value < 0) return;
            Value = Math.Floor(Value);
            double   d = Math.Floor( Value /  SubGridFixed);
            // if (Value % SubGridFixed == 0 && Value > 0) //如果刚刚好，也算本格
            //{
            //    d -= 1;
            //}
            this.Value = Value;
          //  d = d % LuBanSubGrid.LuBanSubGridName.Length;
             
  
            
            SetSubGrid((int)d);
        }

    }


 
}
