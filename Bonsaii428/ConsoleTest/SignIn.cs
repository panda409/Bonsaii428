using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest
{
    class SignIn
    {
        //真正的执行打卡签到的所有流程,每天都会执行
        public void SignInFlow()
        {
            //生成每天需要的报表  Generate();            A时间点执行
                
            //过滤请假的情况               A时间点执行

            //过滤加班的情况               A时间点执行

            //过滤出差的情况               A时间点执行 

            //上面的三个其实是并列的三种情况

            //计算日考勤报表的函数，实际上就是读取打卡表来更改日考勤报表         B时间点执行
            
        }
    }
}
