using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppManage
{
    public class Other
    {
        private int id;
        //年月日
        private string yyr;
        //注册码
        private string zc;
        //排序
        private int sort;

        public Other() { }
        public Other(int id,string yyr,string zc) {
            this.id = id; this.yyr = yyr; this.zc = zc;
        }
        public Other(int id, string yyr, string zc,int sort)
        {
            this.id = id; this.yyr = yyr; this.zc = zc; this.sort = sort;
        }
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        
        public string Yyr
        {
            get { return yyr; }
            set { yyr = value; }
        }
       
        public string Zc
        {
            get { return zc; }
            set { zc = value; }
        }

        public int Sort
        {
            get { return sort; }
            set { sort = value; }
        }

        #region 过期and未注册?
        public static bool isGuoQiAndNoZc(List<Other> list,int date) {
            try
            {
                if (list[0].zc.Trim().Equals(BeanUtil.zcm)) {
                    return false;
                }
                string yyr = list[0].yyr;
                if (BeanUtil.isNull(yyr))
                    return true;
                if (yyr.IndexOf(":") < 0)
                    return true;
                string[] yyrs = yyr.Split(':');
                if (yyrs[0].Length == 2) yyrs[0] = "20" + yyrs[0];
                int y = int.Parse(yyrs[0]);
                int m = int.Parse(yyrs[1]);
                int d = int.Parse(yyrs[2]);
                DateTime dt = new DateTime(y, m, d);
                DateTime sj = DateTime.Now;
                int days = (sj - dt).Days;
                if (days < 0) return true;
                if (days >= date) return true;
                else return false;
            }
            catch (Exception e) { return true; }
        }
        #endregion

        public string ToString() {
            return "Other[id=" + this.id + ",yyr=" + this.yyr + ",zc=" + this.zc + ",sort="+this.sort+"]";
        }
    }
}
