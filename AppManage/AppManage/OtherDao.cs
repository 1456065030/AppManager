using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppManage
{
    public class OtherDao
    {
        private static string nodeName = "Other";
        private static string appName = "other";

        public static List<Other> read()
        {
            List<object[]> list = XmlDao.read(nodeName);
            List<Other> otherList = new List<Other>();
            foreach (object[] item in list)
            {
                try
                {
                    int id = int.Parse(item[0] + "");
                    string name = item[1] + "";
                    string zc = item[2]+"";
                    int sort = 1;
                    try
                    {
                        sort=int.Parse(item[3] + "");
                    }
                    catch { }
                    Other o = new Other(id, name, zc, sort);
                    otherList.Add(o);
                }
                catch { continue; }

            }
            return otherList;
        }

        public static bool add(Other other)
        {
            createBootNode();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (other.Id != 0)
            {
                dic.Add("id", other.Id + "");
            }
            else
            {
                dic.Add("id", (getMaxId() + 1) + "");
            }
            dic.Add("yyr", other.Yyr);
            dic.Add("zc", other.Zc);
            dic.Add("sort","1");
            return XmlDao.add(nodeName, appName, dic);
        }

        public static bool update(Other other)
        {
            if (other.Id <= 0)
            {
                return false;
            }
            Dictionary<string, string> dic = new Dictionary<string, string>();

            dic.Add("zc", other.Zc);

            return XmlDao.update(nodeName, other.Id, dic);
        }

        public static bool sortUpdate(int otherId,int sort){
            if(otherId<=0)return false;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("sort", sort.ToString());
            return XmlDao.updateAndCreate(nodeName, otherId, dic);
        }
        
        public static bool delete(int id)
        {
            return XmlDao.delete(nodeName, id);
        }

        public static int getMaxId()
        {
            return XmlDao.getMaxId(nodeName);
        }
        public static bool createBootNode()
        {
            return XmlDao.createBootNode(nodeName);
        }
    }
}
