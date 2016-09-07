using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonsaiiCommon
{
    class CommonTool
    {
        public const string ERROR_LOG_PATH = "D:\\error.log";
        /// <summary>
        /// 针对一个TimeSpan的时间，添加制定的分钟数，返回一个新的时间  
        /// </summary>
        /// <param name="time">原时间</param>
        /// <param name="minutes">添加的分钟数（可以是负数）</param>
        /// <returns>返回一个新的时间</returns>
        public TimeSpan AddMinutes(TimeSpan time, int minutes)
        {
            int minute = (time.Minutes + minutes) % 60;
            int hour = (time.Hours + (time.Minutes + minutes) / 60) % 24;
            return new TimeSpan(hour, minute, time.Seconds);
        }
        /// <summary>
        /// 针对一个TimeSpan的时间，减去制定的分钟数，返回一个新的时间
        /// </summary>
        /// <param name="time">原时间</param>
        /// <param name="minutes">减去的分钟数</param>
        /// <returns>返回一个新的时间</returns>
        public TimeSpan MinusMinutes(TimeSpan time, int minutes)
        {
            if (time.Minutes >= minutes)
                return new TimeSpan(time.Hours, time.Minutes - minutes, time.Seconds);
            else
            {
                int hours = time.Hours - 1 < 0 ? 23 : time.Hours - 1;
                return new TimeSpan(hours, time.Minutes + 60 - minutes, time.Seconds);
            }
        }

        /// <summary>
        /// 写入错误异常到日志文件当中
        /// </summary>
        /// <param name="ex"></param>
        public void WriteErrorLog(Exception ex)
        {
            StreamWriter fs = new StreamWriter(ERROR_LOG_PATH, true);
            fs.WriteLine("当前时间：" + DateTime.Now.ToString());
            fs.WriteLine("异常信息：" + ex.Message);
            fs.WriteLine("异常对象：" + ex.Source);
            fs.WriteLine("调用堆栈：\n" + ex.StackTrace.Trim());
            fs.WriteLine("触发方法：" + ex.TargetSite);
            fs.WriteLine();
            fs.Close();
        }

        public void WriteTestLog(string str)
        {
            StreamWriter fs = new StreamWriter("D:\\debug.log", true);
            fs.WriteLine("当前时间:" + DateTime.Now.ToString());
            fs.WriteLine("调试信息：" + str);
            fs.WriteLine();
            fs.Close();
        }
    }
}
