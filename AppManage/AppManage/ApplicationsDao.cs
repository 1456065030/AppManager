using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppManage
{
    public class ApplicationsDao
    {
        private static string nodeName = "Applications";
        private static string appName = "Application";

        public static List<Applications> read()
        {
            List<object[]> list = XmlDao.read(nodeName);
            List<Applications> appList = new List<Applications>();
            foreach (object[] item in list)
            {
                try
                {
                    int id = int.Parse(item[0] + "");
                    string name = item[1] + "";
                    string path = item[2] + "";
                    string image= item[3] + "";
                    string style= item[4] + "";
                    float size = 20f;
                    try
                    {
                        size = float.Parse(item[5] + "");
                    }
                    catch { }
                    string color= item[6] + "";
                    appList.Add(new Applications(id, name, path, image, style, size, color));
                }
                catch { continue; }

            }
            return appList;
        }

        public static bool add(Applications app)
        {
            createBootNode();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (app.Id != 0)
            {
                dic.Add("id", app.Id + "");
            }
            else
            {
                dic.Add("id", (getMaxId() + 1) + "");
            }
            //id name path image style size color
            dic.Add("name", app.Name);
            dic.Add("path", app.Path);
            dic.Add("image", app.Image);
            dic.Add("style", app.Style);
            dic.Add("size", app.Size+"");
            dic.Add("color", app.Color);
            return XmlDao.add(nodeName, appName, dic);
        }

        public static bool update(Applications app)
        {
            if (app.Id <= 0)
            {
                return false;
            }
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("name", app.Name);
            dic.Add("path", app.Path);
            dic.Add("image", app.Image);
            dic.Add("style", app.Style);
            dic.Add("size", app.Size + "");
            dic.Add("color", app.Color);
            return XmlDao.update(nodeName, app.Id, dic);
        }

        public static bool delete(int id)
        {
            return XmlDao.delete(nodeName, id);
        }

        public static int getMaxId()
        {
            return XmlDao.getMaxId(nodeName);
        }
        public static bool createBootNode() {
            return XmlDao.createBootNode(nodeName);
        }
    }
}
