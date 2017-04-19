using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppManage
{
    public class AppType
    {
        private int id;
        private string name;

        public AppType() { }

        public AppType(int id,string name) {
            this.id = id; this.name = name;
        }
        public AppType(string name) {
            this.name = name;
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
        
        public string ToString(){
            return "AppType[id="+this.id+",name="+this.name+"]";
        }
    }
}
