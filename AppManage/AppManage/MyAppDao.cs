using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppManage
{
    public class MyAppDao
    {
        private static string nodeName = "MyApp";
        private static string appName = "App";

        public static List<MyApp> read() {
            List<object[]> list = XmlDao.read(nodeName);
            List<MyApp> appList = new List<MyApp>();
            foreach (object[] item in list)
            {
                try
                {
                    int id = int.Parse(item[0] + "");
                    string name = item[1] + "";
                    int type = int.Parse(item[2] + "");
                    string path = item[3] + "";
                    string pwd = item[4] + "";
                    int click = 0;
                    try
                    {
                        click = int.Parse(item[5] + "");
                    }
                    catch { }
                    MyApp app = new MyApp(id,name,type,path,pwd,click);
                    appList.Add(app);
                }
                catch { continue; }
                
            }
            return appList;
        }

        public static bool add(MyApp app)
        {
            createBootNode();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (app.Id != 0)
            {
                dic.Add("id", app.Id + "");
            }
            else {
                dic.Add("id", (getMaxId() + 1) + "");
            }
            dic.Add("name",app.Name);
            dic.Add("type",app.TypeId+"");
            dic.Add("path",app.Path);
            dic.Add("pwd",app.Pwd);
            dic.Add("click",app.Click+"");
            return XmlDao.add(nodeName, appName, dic);
        }

        public static bool update(MyApp app) {
            if (app.Id <= 0) {
                return false;
            }
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (!BeanUtil.isNull(app.Name)) {
                dic.Add("name",app.Name);
            }
            if (app.TypeId != 0) {
                dic.Add("type",app.TypeId+"");
            }
            if (!BeanUtil.isNull(app.Path)) {
                dic.Add("path",app.Path);
            }
            dic.Add("pwd", app.Pwd);
            return XmlDao.update(nodeName, app.Id, dic);
        }

        public static bool click(int appId)
        {
            if (appId <= 0) return false;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("click","++");
            return XmlDao.updateAndCreate(nodeName, appId, dic);
        }

        public static bool delete(int id) {
            return XmlDao.delete(nodeName, id);
        }

        public static int getMaxId() {
            return XmlDao.getMaxId(nodeName);
        }
        public static bool createBootNode()
        {
            return XmlDao.createBootNode(nodeName);
        }
    }
}
