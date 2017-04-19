using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppManage
{
    public class Applications
    {
        private int id;
        private string name;
        private string path;
        private string image;
        private string style;
        private float size;
        private string color;

        public Applications() { }
        public Applications(int id,string name,string path,string image,string style,float size,string color) {
            this.id = id;
            this.name = name;
            this.path = path;
            this.image = image;
            this.style = style;
            this.size = size;
            this.color = color;
        }
        public Applications(string name, string path, string image, string style, float size, string color)
        {
            this.name = name;
            this.path = path;
            this.image = image;
            this.style = style;
            this.size = size;
            this.color = color;
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

        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        public string Image
        {
            get { return image; }
            set { image = value; }
        }

        public string Style
        {
            get { return style; }
            set { style = value; }
        }
        public float Size
        {
            get { return size; }
            set { size = value; }
        }

        public string Color
        {
            get { return color; }
            set { color = value; }
        }

        public string ToString()
        {
            return "Application[id=" + this.id + ",name=" + this.name + ",path=" + this.path + ",image=" + this.image + ",style=" + this.style + ",size=" + this.size + ",color=" + this.color + "]";
        }
    }
}
