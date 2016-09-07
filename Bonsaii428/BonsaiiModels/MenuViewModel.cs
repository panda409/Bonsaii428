using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonsaiiModels
{
    public class MenuViewModel
    {
        public string ParentName { get; set; }
        public string ParentClass { get; set; }
        public int ParentOrder { get; set; }
        public List<MenuNode> MenuNodes { get; set; }
        public MenuViewModel(string ParentName,string ParentClass,int ParentOrder)
        {
            this.ParentName = ParentName;
            this.ParentClass = ParentClass;
            this.ParentOrder = ParentOrder;
            MenuNodes = new List<MenuNode>();
        }
    }

    public class MenuNode
    {
        public int Id { get; set; }
        public string Name{get;set;}
        public string Href { get; set; }
        public bool IsShow { get; set; }
        public MenuNode(string Name, string Href, bool IsShow,int Id)
        {
            this.Id = Id;
            this.Name = Name;
            this.Href = Href;
            this.IsShow = IsShow;
        }
    }
}
