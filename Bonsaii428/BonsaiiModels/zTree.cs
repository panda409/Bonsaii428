using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bonsaii.Models
{
    public class zTree
    {
        public string id { get; set; }
        public string pid { get; set; }
        public string name { get; set; }
        public bool isParent { get; set; }
        public string url { get; set; }
        public string target { get; set; }
        public List<zTree> children { get; set; }
        public bool open { get; set; }

      //  public string flag { get; set; }

        public zTree()
        {
            this.children = new List<zTree>();
        }
    }
}