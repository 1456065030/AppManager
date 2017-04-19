using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppManage
{
    public class AppTypeDao
    {
        private static string nodeName = "AppType";
        private static string appName = "Type";

        public static List<AppType> read()
        {
            List<object[]> list = XmlDao.read(nodeName);
            List<AppType> typeList = new List<AppType>();
            foreach (object[] item in list)
            {
                try
                {
                    int id = int.Parse(item[0] + "");
                    string name = item[1] + "";
                    AppType type = new AppType(id, name);
                    typeList.Add(type);
                }
                catch { continue; }

            }
            return typeList;
        }

        public static bool add(AppType type)
        {
            createBootNode();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (type.Id != 0)
            {
                dic.Add("id", type.Id + "");
            }
            else
            {
                dic.Add("id", (getMaxId()+1) + "");
            }
            dic.Add("name", type.Name);
            return XmlDao.add(nodeName, appName, dic);
        }

        public static bool update(AppType type)
        {
            if (type.Id <= 0)
            {
                return false;
            }
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (!BeanUtil.isNull(type.Name))
            {
                dic.Add("name", type.Name);
            }

            return XmlDao.update(nodeName, type.Id, dic);
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
