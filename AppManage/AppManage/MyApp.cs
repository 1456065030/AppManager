using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppManage
{

    public class MyApp
    {
        private int count;
        private int id;
        private string name;
        private int typeId;
        private string type;
        private string path;
        private string showPath;
        private string pwd;
        private int click;
        
        public MyApp() { }
        public MyApp(int id, string name, int typeId, string path, string pwd, int click)
        {
            this.id = id;
            this.name = name;
            this.typeId = typeId;
            this.path = path;
            this.pwd = pwd;
            this.click = click;
        }
        public MyApp(int id, string name, int typeId, string path, string pwd)
        {
            this.id = id;
            this.name = name;
            this.typeId = typeId;
            this.path = path;
            this.pwd = pwd;
        }
        public MyApp(string name, int typeId, string path, string pwd)
        {
            this.name = name;
            this.typeId = typeId;
            this.path = path;
            this.pwd = pwd;
        }

        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int TypeId
        {
            get { return typeId; }
            set { typeId = value; }
        }

        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        public string Pwd
        {
            get { return pwd; }
            set { pwd = value; }
        }
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        public string ShowPath
        {
            get { return showPath; }
            set { showPath = value; }
        }

        public int Click
        {
            get { return click; }
            set { click = value; }
        } 

        public string ToString()
        {
            return "MyApp[id=" + this.id + ",name=" + this.name + ",typeId=" + this.typeId + ",type="+this.type+",path=" + this.path + ",pwd=" + this.pwd +",click="+this.click+ "]";
        }
    }
}
