using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonsaiiModels.GlobalStaticVaribles
{
     public class SignInType
    {
        //普通上班打卡类型
        public static readonly int NORMALWORK = 0;
        //加班打卡类型
        public static readonly int OVERTIME = 1;
         //出差打卡类型
        public static readonly int EVECTION = 2;
        //值班打卡类型
        public static readonly int ONDUTY = 3;
    }
}
