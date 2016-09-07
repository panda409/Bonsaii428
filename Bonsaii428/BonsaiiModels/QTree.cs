using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bonsaii.Controllers
{
    public class QTree
    {
        public int id;
        public String url;
        public String text;
        public bool check= false;
        public List<QTree> children;
    }
}
