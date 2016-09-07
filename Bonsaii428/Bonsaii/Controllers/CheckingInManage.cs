using Bonsaii;
using Bonsaii.Models;
using Bonsaii.Models.Checking_in;
using Bonsaii.Models.Works;
using BonsaiiModels;
using BonsaiiModels.GlobalStaticVaribles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsaii
{
   public class CheckingInManage
    {
       public static BonsaiiDbContext db { get; set; }
       public static SystemDbContext sysdb { get; set; }
       public static List<string> ConnStrings { get; set; }
        //定义公共的工具函数
       public static CommonTool Tools { get; set; }

        //一些和考勤相关的参数
        /// <summary>
        /// 加班打卡提前分钟
        /// </summary>
       public static int OvertimeAheadMinutes { get; set; }
        /// <summary>
        /// 加班打卡推后分钟
        /// </summary>
       public static int OvertimeBackMinutes { get; set; }
        /// <summary>
        /// 出差打卡提前分钟
        /// </summary>
       public static int EvectionAheadMinutes { get; set; }
        /// <summary>
        /// 出差打卡推后分钟
        /// </summary>
       public static int EvectionBackMinutes { get; set; }
        /// <summary>
        /// 迟到分钟数
        /// </summary>
       public static int LateMinutes { get; set; }
        /// <summary>
        /// 迟到计小时值
        /// </summary>
       public static int LateToHour { get; set; }
        /// <summary>
        /// 早退分钟数
        /// </summary>
       public static int EarlyMinutes { get; set; }
        /// <summary>
        /// 早退计小时值
        /// </summary>
       public static int EarlyToHour { get; set; }
        /// <summary>
        /// 出差是否打卡
        /// </summary>
       public static bool EvectionNeedSignIn { get; set; }
        /// <summary>
        /// 加班是否打卡
        /// </summary>
       public static bool OvertimeNeedSignIn { get; set; }
        /// <summary>
        /// 值班是否需要打卡
        /// </summary>
        public static bool OnDutyNeedSignIn { get; set; }
        /// <summary>
        /// 值班打卡的提前分钟数
        /// </summary>
        public static int OnDutyAheadMinutes { get; set; }
        /// <summary>
        /// 值班打卡的推后分钟数
        /// </summary>
        public static int OnDutyBackMinutes { get; set; }
        /// <summary>
        /// 旷职分钟数
        /// </summary>
        public static int AbsentismMinutes { get; set; }
        /// <summary>
        /// 旷职时数
        /// </summary>
        public static int AbsentismHour { get; set; }
        //无参数构造函数
        public CheckingInManage()
        {
            //这里面初始化db和sysdb仅仅是为了针对某个企业测试函数逻辑，实际用的时候，要循环所有的企业
            db = new BonsaiiDbContext("Data Source = 211.149.199.42,1433;Initial Catalog = Bonsaii0000000008;User ID = sa;Password = admin123@;");
            sysdb = new SystemDbContext("Data Source = 211.149.199.42,1433;Initial Catalog = BonsaiiSystem;User ID = sa;Password = admin123@;");
            Tools = new CommonTool();
            ConnStrings = sysdb.Users.Where(p => p.IsRoot == true).Select(p => p.ConnectionString).ToList();
            OvertimeAheadMinutes = db.CheckingInParams.Find(1).Value;
            OvertimeBackMinutes = db.CheckingInParams.Find(2).Value;
            EvectionAheadMinutes = db.CheckingInParams.Find(3).Value;
            EvectionBackMinutes = db.CheckingInParams.Find(4).Value;
            LateMinutes = db.CheckingInParams.Find(5).Value;
            LateToHour = db.CheckingInParams.Find(6).Value;
            EarlyMinutes = db.CheckingInParams.Find(7).Value;
            EarlyToHour = db.CheckingInParams.Find(8).Value;

            EvectionNeedSignIn = db.CheckingInParamsBools.Find(1).Value;
            OvertimeNeedSignIn = db.CheckingInParamsBools.Find(2).Value;
            OnDutyNeedSignIn = db.CheckingInParamsBools.Find(3).Value;
            AbsentismMinutes = db.CheckingInParams.Find(9).Value;
            AbsentismHour = db.CheckingInParams.Find(10).Value;

            OnDutyAheadMinutes = db.CheckingInParams.Find(11).Value;
            OnDutyBackMinutes = db.CheckingInParams.Find(12).Value;
        }

        /*-------------------------------------------------------------------------下面高能预警！一大坨测试函数！！！！！-----------------------------------------------------------------------------------------------------------*/
        public void GenerateMonth()
        {
            int month = 15;
            DateTime dt = new DateTime(2016, 4, 21);
            for (int i = 0; i < month; i++)
            {
                DateTime CurrentDate = dt.AddDays(i);
                Console.WriteLine(CurrentDate);
                GenerateSignInCardStatusAndEveryDaySign(CurrentDate);
                CheckVacateApplies(CurrentDate);
                CheckEvectionApplies(CurrentDate);
                CheckOvertimeApplies(CurrentDate);
            }
        }

        //生成某天所需要的打卡表和日考勤报表，仅仅是为了测试
        public void GenerateSomeDayForTest()
        {
            DateTime CurrentDate = new DateTime(2016, 3, 14);
            //按照排班生成最基本的打卡表和日考勤报表
            GenerateSignInCardStatusAndEveryDaySign(CurrentDate);

    //        CheckVacateApplies(CurrentDate);

    //        CheckEvectionApplies(CurrentDate);

    //        CheckOvertimeApplies(CurrentDate);

            CheckOnDutyApplies(CurrentDate);

            CheckDaysOffApplies(CurrentDate);

        }

        //计算某一天的日考勤报表,主要是根据打卡情况，计算各种数据
        //public void CalculateDay()
        //{
        //    DateTime currentDate = new DateTime(2016, 3, 12);
        //    CalculateChargeCard(currentDate);
        //    CalculateEvection(currentDate);
        //    CalculateOvertime(currentDate);
        //    //只有针对值班需要打卡的情况才要计算
        //    if (OnDutyNeedSignIn)
        //       CalculateOnDuty(currentDate);
        //    //请假不需要打卡，也就不用计算了
        //    List<string> staffNumbers = db.Staffs.Select(p=>p.StaffNumber).ToList();
        //    foreach (string tmpStaffNumber in staffNumbers)
        //        CalculateEveryDaySignInData(currentDate, tmpStaffNumber);
        //}
        //计算某一天的日考勤报表,主要是根据打卡情况，计算各种数据
        public static void CalculateDay(DateTime currentDate)
        {
            //DateTime currentDate = new DateTime(2016, 3, 12);
            CalculateChargeCard(currentDate);
            CalculateEvection(currentDate);
            CalculateOvertime(currentDate);
            //只有针对值班需要打卡的情况才要计算
            if (OnDutyNeedSignIn)
                CalculateOnDuty(currentDate);
            //请假不需要打卡，也就不用计算了
           // CalculateEveryDaySignInData(currentDate, "PHGL000228");
            //List<string> staffNumbers = db.Staffs.Select(p => p.StaffNumber).ToList();
            //foreach (string tmpStaffNumber in staffNumbers)
            //   CalculateEveryDaySignInData(currentDate, tmpStaffNumber);
        }

       //计算当前员工的今日考勤情况
        public void CalculateStaffDay(string staffNumber, DateTime dateTime)
        { 
            Staff staff=(from s in db.Staffs where s.StaffNumber==staffNumber select s).FirstOrDefault();           
            var data = db.DeviceOriginalDatas.Where(d =>dateTime<=d.DateTime&&d.UserID.Equals(staff.PhysicalCardNumber)).ToList();
            foreach (var temp in data)
            {
               TimeSpan time=temp.DateTime.TimeOfDay;
               SignInCardStatus sign = db.SignInCardStatus.Where(s => s.NeedStartTime < time && s.NeedEndTime > time && s.SignInTime == null&&s.WorkDate==dateTime).FirstOrDefault();
                if (sign == null)
                {
                    continue;
                }
                if (sign.NeedWorkTime>=time&&sign.Type=="上班")
                {
                    sign.SignInTime = time;
                }
                else if (sign.NeedWorkTime < time && sign.Type == "上班")
                {
                    sign.SignInTime = time;
                    sign.ComeLateMinutes = (time - sign.NeedWorkTime).Minutes;
                }
                else if (sign.NeedWorkTime > time && sign.Type == "下班")
                {
                    sign.SignInTime = time;
                    sign.LeaveEarlyMinutes = (sign.NeedWorkTime - time).Minutes;
                }
                else if (sign.NeedWorkTime <= time && sign.Type == "下班")
                {
                    sign.SignInTime = time;                   
                }
                db.SaveChanges();
            }
        
        }

        /*-------------------------------------------------------------------------上面高能预警！一大坨测试函数！！！！！-----------------------------------------------------------------------------------------------------------*/

        //生成每天所需要的打卡表和日考勤报表
        public void GenerateEachDay()
        {
            //    DateTime CurrentDate = DateTime.Now;
            DateTime CurrentDate = new DateTime(2016, 3, 11);
            //        foreach (string tmpConn in ConnStrings)
            //       {
            //对于不同的企业，分别初始化不同的DbContext,来操作不同的企业数据库
            //     using (db = new BonsaiiDbContext("Data Source = 211.149.199.42,1433;Initial Catalog = Bonsaii0000000008;User ID = sa;Password = admin123@;"))
            //         {
            //按照排班生成最基本的打卡表和日考勤报表
            GenerateSignInCardStatusAndEveryDaySign(CurrentDate);

            CheckVacateApplies(CurrentDate);

            CheckEvectionApplies(CurrentDate);

            CheckOvertimeApplies(CurrentDate);

            CheckOnDutyApplies(CurrentDate);
            //            }
            //        }
        }




        //生成每天所需要的基本打卡表和日考勤报表
        public void GenerateSignInCardStatusAndEveryDaySign(DateTime CurrentDate)
        {
            List<HolidayTables> holidays = db.HolidayTables.Where(p => p.Date == CurrentDate).ToList();
            //找到每一个员工在CurrentDate这一天所有的WorkTimes
            var StaffWorks = (from x in db.Staffs.Where(p => p.FreeCard == false)
                              join p in db.WorkManages.Where(p => p.Date == CurrentDate.Date) on x.StaffNumber equals p.StaffNumber
                              join n in db.WorkTimes on p.WorksId equals n.WorksId
                              select new
                              {
                                  x.StaffNumber,
                                  x.Name,
                                  p.Date,
                                  p.WorksId,
                                  n.StartTime,
                                  n.EndTime,
                                  n.AheadMinutes,
                                  n.LateMinutes,
                                  n.LeaveEarlyMinutes,
                                  n.BackMinutes
                              }).ToList();

            foreach (var tmp in StaffWorks)
            {
                HolidayTables tmpHoliday = null;
                if (holidays.Count != 0)
                {
                    try
                    {
                        tmpHoliday = holidays.Where(p => p.StaffNumber.Equals(tmp.StaffNumber)).Single();
                    }
                    catch (Exception e)
                    {
                        Tools.WriteErrorLog(e);
                        break;
                    }
                }

                if (tmpHoliday != null && tmpHoliday.StartHour <= tmp.StartTime.Hours && tmpHoliday.EndHour >= tmp.StartTime.Hours)
                    continue;
                //写入信息到打卡表
                //添加上班时间点
                db.SignInCardStatus.Add(new SignInCardStatus()
                {
                    StaffNumber = tmp.StaffNumber,
                    WorkDate = CurrentDate,
                    NeedWorkTime = tmp.StartTime,
                    NeedStartTime = Tools.MinusMinutes(tmp.StartTime, tmp.AheadMinutes),
                    NeedEndTime = Tools.AddMinutes(tmp.StartTime, tmp.LateMinutes),
                    Type = "上班",
                    ComeLateMinutes = 0,
                    LeaveEarlyMinutes = 0

                });
                //添加下班时间点
                db.SignInCardStatus.Add(new SignInCardStatus()
                {
                    StaffNumber = tmp.StaffNumber,
                    WorkDate = CurrentDate,
                    NeedWorkTime = tmp.EndTime,
                    NeedStartTime = Tools.MinusMinutes(tmp.EndTime, tmp.LeaveEarlyMinutes),
                    NeedEndTime = Tools.AddMinutes(tmp.EndTime, tmp.BackMinutes),
                    Type = "下班",
                    ComeLateMinutes = 0,
                    LeaveEarlyMinutes = 0
                });
                db.SaveChanges();
            }
            var Staffs = StaffWorks.Select(p => new { p.StaffNumber, p.WorksId }).Distinct().ToList();
            foreach (var tmp in Staffs)
            {
                //生成日考勤报表
                db.EveryDaySignInDates.Add(new EveryDaySignInDate(tmp.StaffNumber, tmp.WorksId, CurrentDate));
                db.SaveChanges();
            }
        }

        /// <summary>
        /// 检查加班申请表，是否有员工在某一天需要加班，有的话进行处理
        /// </summary>
        /// <param name="CurrentDate">传递的参数是日期！不带时间的日期</param>
        public void CheckOvertimeApplies(DateTime CurrentDate)
        {
            //获取今天的假日类型
            List<HolidayTables> holidays = db.HolidayTables.Where(p => p.Date == CurrentDate).ToList();
            string HolidayType = "正常加班";
            if (holidays.Count != 0)
                HolidayType = holidays[0].Type + "加班";
            List<OvertimeApplies> list = db.OvertimeApplies.Where(p => p.Date == CurrentDate).ToList();

            List<EveryDaySignInDate> tmpEveryDay = db.EveryDaySignInDates.Where(p => p.Date == CurrentDate).ToList();
            foreach (OvertimeApplies tmp in list)
            {
                EveryDaySignInDate EveryDay = null;
                try
                {
                    //获取到日考勤报表的对象
                    EveryDay = tmpEveryDay.Where(p => p.StaffNumber.Equals(tmp.StaffNumber)).Single();
                }
                catch (Exception e)
                {
                    Tools.WriteErrorLog(e);
                    return;
                }

                if (OvertimeNeedSignIn)
                {
                    //插入打卡时间到打卡表           
                    db.SignInCardStatus.Add(new SignInCardStatus()
                    {
                        StaffNumber = tmp.StaffNumber,
                        WorkDate = CurrentDate,
                        NeedWorkTime = tmp.StartDateTime.TimeOfDay,
                        NeedStartTime = Tools.MinusMinutes(tmp.StartDateTime.TimeOfDay, OvertimeAheadMinutes),
                        NeedEndTime = Tools.AddMinutes(tmp.StartDateTime.TimeOfDay, OvertimeBackMinutes),
                        Type = "上班",
                        ComeLateMinutes = 0,
                        LeaveEarlyMinutes = 0,
                        SignInType = SignInType.OVERTIME,
                    });
                    db.SignInCardStatus.Add(new SignInCardStatus()
                    {
                        StaffNumber = tmp.StaffNumber,
                        WorkDate = CurrentDate,
                        NeedWorkTime = tmp.EndDateTime.TimeOfDay,
                        NeedStartTime = Tools.MinusMinutes(tmp.EndDateTime.TimeOfDay, OvertimeAheadMinutes),
                        NeedEndTime = Tools.AddMinutes(tmp.EndDateTime.TimeOfDay, OvertimeBackMinutes),
                        Type = "下班",
                        ComeLateMinutes = 0,
                        LeaveEarlyMinutes = 0,
                        SignInType = SignInType.OVERTIME,
                    });
                }

                //申请加班总时数
                //更新这个员工的日考勤报表的信息
                EveryDay.WorkOvertimeHours = tmp.Hours;//OvertimeWorkHours;
                EveryDay.OvertimeType = HolidayType;
                db.Entry(EveryDay).State = EntityState.Modified;
                //标记这个加班申请已经处理过
                tmp.IsRead = true;
                db.Entry(tmp).State = EntityState.Modified;
                db.SaveChanges();
            }
        }


        /// <summary>
        /// 检查值班申请表，是否有员工在某一天申请值班，有的话进行处理
        /// </summary>
        /// <param name="CurrentDate"></param>
        public void CheckOnDutyApplies(DateTime CurrentDate)
        {
            List<OnDutyApplies> list = db.OnDutyApplies.Where(p => p.Date == CurrentDate).ToList();

            List<EveryDaySignInDate> tmpEveryDay = db.EveryDaySignInDates.Where(p => p.Date == CurrentDate).ToList();
            foreach (OnDutyApplies tmp in list)
            {
                //无论打卡还是不打卡，都要搞到OnDutyHours这个对象，或者插入一条新的，或者读取一条已经存在的（StaffNumber是主键)

                OnDutyHours tmpOnDutyHours = db.OnDutyHours.Find(tmp.StaffNumber);

                //需要打卡就插入打卡表,OnDutyHours的计算要放在CalculateOnDutyApplies这个函数中
                if (OnDutyNeedSignIn)
                {
                    //插入打卡时间到打卡表           
                    db.SignInCardStatus.Add(new SignInCardStatus()
                    {
                        StaffNumber = tmp.StaffNumber,
                        WorkDate = CurrentDate,
                        NeedWorkTime = tmp.StartDateTime.TimeOfDay,
                        NeedStartTime = Tools.MinusMinutes(tmp.StartDateTime.TimeOfDay, OvertimeAheadMinutes),
                        NeedEndTime = Tools.AddMinutes(tmp.StartDateTime.TimeOfDay, OvertimeBackMinutes),
                        Type = "上班",
                        ComeLateMinutes = 0,
                        LeaveEarlyMinutes = 0,
                        SignInType = SignInType.ONDUTY,
                    });
                    db.SignInCardStatus.Add(new SignInCardStatus()
                    {
                        StaffNumber = tmp.StaffNumber,
                        WorkDate = CurrentDate,
                        NeedWorkTime = tmp.EndDateTime.TimeOfDay,
                        NeedStartTime = Tools.MinusMinutes(tmp.EndDateTime.TimeOfDay, OvertimeAheadMinutes),
                        NeedEndTime = Tools.AddMinutes(tmp.EndDateTime.TimeOfDay, OvertimeBackMinutes),
                        Type = "下班",
                        ComeLateMinutes = 0,
                        LeaveEarlyMinutes = 0,
                        SignInType = SignInType.ONDUTY,
                    });

                    //针对打卡的情况，如果不存在OnDutyHours，就插入；如果存在，那就不用管，在最后的计算中会根据打卡情况计算AvailableHours值
                    if (tmpOnDutyHours == null)
                        db.OnDutyHours.Add(new OnDutyHours()
                        {
                            StaffNumber = tmp.StaffNumber,
                            AvailableHours = 0
                        });
                }       
                else
                {

                    if (tmpOnDutyHours == null)
                    {
                        db.OnDutyHours.Add(new OnDutyHours()
                        {
                            StaffNumber = tmp.StaffNumber,
                            AvailableHours = tmp.Hours ?? 0
                        });
                    }
                    else
                    {
                        tmpOnDutyHours.AvailableHours += tmp.Hours ?? 0;
                        db.Entry(tmpOnDutyHours).State = EntityState.Modified;                 
                    }
                }
                //标记这个值班申请已经处理过
                tmp.IsRead = true;
                db.Entry(tmp).State = EntityState.Modified;
                db.SaveChanges();
            }
        }


        /// <summary>
        /// 检查调休申请表，修改打卡表（其实就是删除在调休范围内的打卡表）,修改日考勤报表（正班工时要加上调休的时数！）
        /// </summary>
        /// <param name="CurrentDate"></param>
        public void CheckDaysOffApplies(DateTime CurrentDate)
        {
            //获取当前日期中的所有调休申请
            List<DaysOffApplies> daysOffApplies = db.DaysOffApplies.Where(p => p.Date == CurrentDate && p.IsRead == false).ToList();
            List<SignInCardStatus> signInCardStatus = (from x in db.SignInCardStatus
                                                       where x.WorkDate == CurrentDate
                                                       join y in db.DaysOffApplies on x.StaffNumber equals y.StaffNumber
                                                       where y.IsRead == false
                                                       select x).ToList();
            List<EveryDaySignInDate> everyDaySignInDateList = (from x in db.DaysOffApplies
                                                               where x.Date == CurrentDate && x.IsRead == false
                                                               join y in db.EveryDaySignInDates on x.StaffNumber equals y.StaffNumber
                                                               select y).ToList();
            foreach (DaysOffApplies tmp in daysOffApplies)
            {
                try
                {
                    TimeSpan tmpStartTime = new TimeSpan(tmp.StartDateTime.Hour, tmp.StartDateTime.Minute, 0);
                    TimeSpan tmpEndTime = new TimeSpan(tmp.EndDateTime.Hour, tmp.EndDateTime.Minute, 0);
                    //找到在调休范围内的打卡时间点，删除它们
                    List<SignInCardStatus> tmpSignInCardStatus = signInCardStatus.Where(p => p.StaffNumber.Equals(tmp.StaffNumber) && p.NeedWorkTime <= tmpEndTime && p.NeedWorkTime >= tmpStartTime).ToList();
                    //删除那些额外的时间点
                    db.SignInCardStatus.RemoveRange(tmpSignInCardStatus);

                    //日考勤报表当中的工时数要加上调休的时数
                    EveryDaySignInDate tmpEveryDaySignInDate = everyDaySignInDateList.Where(p => p.StaffNumber.Equals(tmp.StaffNumber)).Single();
                    tmpEveryDaySignInDate.WorkHours += tmp.Hours;

                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Tools.WriteErrorLog(e);
                    return;
                }
            }
        }

        /// <summary>
        /// 处理出差的情况
        /// </summary>
        /// <param name="CurrentDate"></param>
        public void CheckEvectionApplies(DateTime CurrentDate)
        {
            List<EvectionApplies> list = db.EvectionApplies.Where(p => DbFunctions.TruncateTime(p.StartDateTime) <= CurrentDate && DbFunctions.TruncateTime(p.EndDateTime) >= CurrentDate).ToList();
            List<EveryDaySignInDate> tmpEveryDay = db.EveryDaySignInDates.Where(p => p.Date == CurrentDate).ToList();
            //遍历在CurrentDate内的所有出差申请
            foreach (EvectionApplies tmp in list)
            {
                EveryDaySignInDate EveryDaySignIn = null;
                try
                {
                    EveryDaySignIn = tmpEveryDay.Where(p => p.StaffNumber.Equals(tmp.StaffNumber)).Single();
                }
                catch (Exception e)
                {
                    Tools.WriteErrorLog(e);
                    return;
                }

                //标记是出差考勤的情况
                EveryDaySignIn.IsOnEvection = true;

                /**
                 * 获取在出差时间段内的所有打卡时间点，有两种处理方式：
                 * 1、出差需要打卡：更改这些打卡点的SignInType值
                 * 2、出差不需要打卡，直接删除这些打卡点就行
                 * */
                List<SignInCardStatus> tmpList = db.SignInCardStatus.Where(p => p.StaffNumber.Equals(tmp.StaffNumber) && p.NeedWorkTime >= tmp.StartDateTime.TimeOfDay && p.NeedWorkTime <= tmp.EndDateTime.TimeOfDay).ToList();

                //需要打卡
                if (EvectionNeedSignIn)
                {
                    //更改打卡的类型
                    foreach (SignInCardStatus tmpSignIn in tmpList)
                    {
                        tmpSignIn.SignInType = SignInType.EVECTION;
                        tmpSignIn.NeedStartTime = Tools.MinusMinutes(tmpSignIn.NeedWorkTime, EvectionAheadMinutes);
                        tmpSignIn.NeedEndTime = Tools.AddMinutes(tmpSignIn.NeedWorkTime, EvectionBackMinutes);
                    }
                }
                //不用打卡,就直接删除打卡点就行
                else
                {
                    //直接写入日考勤报表的工作时间
                    WorkManages work = db.WorkManages.Where(p => p.StaffNumber.Equals(tmp.StaffNumber) && p.Date == CurrentDate).Single();
                    EveryDaySignIn.WorkHours = db.Works.Where(p => p.Id == work.WorksId).Single().TotalWorkHours;
                    db.SignInCardStatus.RemoveRange(tmpList);
                }
                db.Entry(EveryDaySignIn).State = EntityState.Modified;
                if (tmp.EndDateTime.Date == CurrentDate)
                {
                    tmp.IsRead = true;
                    db.Entry(tmp).State = EntityState.Modified;
                }
                db.SaveChanges();   //保存更改
            }
        }


        /// <summary>
        /// 处理员工请假情况
        /// </summary>
        /// <param name="CurrentDate">当前时间</param>
        public void CheckVacateApplies(DateTime CurrentDate)
        {
            //获取请假表中未读的请假申请,条件为IsRead，当前员工号和，，以列表形式输出；
            List<VacateApplies> VacateApplies = db.VacateApplies.Where(p => p.IsRead == false && DbFunctions.TruncateTime(p.StartDateTime) <= CurrentDate && DbFunctions.TruncateTime(p.EndDateTime) >= CurrentDate && p.AuditStatus == 3).ToList();

            List<EveryDaySignInDate> tmpEveryDay = db.EveryDaySignInDates.Where(p => p.Date == CurrentDate).ToList();
            // 遍历请假申请表，从未读的请假申请中获取员工号，请假开始时间，请假结束时间，请假日期；
            foreach (VacateApplies va in VacateApplies)
            {
                //获取请假表中StartDateTime ,EndDateTime ,和CurrentDate做比较
                //获取日考勤报表中的当前时间;
                EveryDaySignInDate everydaysignindate = null;
                try
                {
                    everydaysignindate = tmpEveryDay.Where(p => p.Date == CurrentDate && p.StaffNumber == va.StaffNumber).Single();
                }
                catch (Exception e)
                {
                    Tools.WriteErrorLog(e);
                    return;
                }
                List<SignInCardStatus> signincardstatus = db.SignInCardStatus.Where(p1 => p1.StaffNumber == va.StaffNumber && p1.WorkDate == CurrentDate).ToList();
                foreach (SignInCardStatus sica in signincardstatus)
                {
                    //遍历打卡表，获得NeedWorksTime,如果NeedWorksTime在请假表之间，便直接在打卡表中删除此点。
                    if (sica.NeedWorkTime >= va.StartDateTime.TimeOfDay && sica.NeedWorkTime <= va.EndDateTime.TimeOfDay)
                    {
                        db.SignInCardStatus.Remove(sica);
                        db.SaveChanges();
                    }
                    //向日考勤表报中写入数据，具体属性是添加请假类型和请假时数。
                    string BillType = va.BillType;
                    string BillTypeName = null;
                    try
                    {
                        BillTypeName = db.BillProperties.Where(p => p.Type == BillType).Single().TypeName;
                    }
                    catch (Exception e)
                    {
                        Tools.WriteErrorLog(e);
                        return;
                    }
                    // 向日考勤报表中添加请假类型
                    everydaysignindate.VacateType = BillTypeName;
                    //向日考勤报表中添加请假时数
                    //根据排班表和请假表中一样员工号获得员工的排班ID。根据排班ID获得Ｗorks中的TotalWorkHours  
                    //          获取请假表中请假时数 ，与Works表中规定的工作时数做判断，判断是否大于规定工作时间，如果大于则表示请假一整天，将请假时间赋值为规定工作时间
                    float Hours = CalcuteVacateHours(va.StaffNumber, va.StartDateTime, va.EndDateTime, CurrentDate);
                    everydaysignindate.VacateHours = Hours;

                }
                //      保存所做编辑
                db.Entry(everydaysignindate).State = EntityState.Modified;
                db.SaveChanges();
                //设置请假表中的IsRead属性。如果请假结束时间  < CurrentDate,那么将IsRead属性设为ture
                if (va.EndDateTime <= CurrentDate)
                    va.IsRead = true;
                db.Entry(va).State = EntityState.Modified;
                db.SaveChanges();
            }
        }





        /// <summary>
        /// 根据员工号和当前的日期，判断出员工当前日期的请假小时数
        /// </summary>
        /// <param name="StaffNumber">员工号</param>
        /// <param name="Start">员工申请请假的开始日期</param>
        /// <param name="End">员工申请请假的结束日期</param>
        /// <param name="CurrentDate">当前的日期</param>
        /// <returns>小时数</returns>
        public float CalcuteVacateHours(string StaffNumber, DateTime Start, DateTime End, DateTime CurrentDate)
        {
            //从请假条中获取请假开始日期，请假结束日期
            DateTime StartDate = Start.Date;
            DateTime EndDate = End.Date;
            //获取打卡表中NeedWorkTime;获取第一条和最后一条记录
            TimeSpan WorkStartTime = db.SignInCardStatus.Where(p => p.StaffNumber.Equals(StaffNumber) && p.WorkDate == CurrentDate).Select(p => p.NeedWorkTime).First();
            TimeSpan WorkEndTime = db.SignInCardStatus.Where(p => p.StaffNumber.Equals(StaffNumber) && p.WorkDate == CurrentDate).Select(p => p.NeedWorkTime).Last();
            //获取请假条中请假开始时间，请假结束时间
            //默认为请假开始时间，请假结束时间
            TimeSpan StartTime = StartDate.TimeOfDay;
            TimeSpan EndTime = EndDate.TimeOfDay;
            //默认是今天 ， 今天
            //判断 小于今天，今天
            if (StartDate < CurrentDate && EndDate == CurrentDate)
                StartTime = WorkStartTime;
            //判断 今天 ， 大于今天
            else if (EndDate > CurrentDate && StartDate == CurrentDate)
                EndTime = WorkEndTime;
            //介于几天之中全天请假情况
            else
            {
                StartTime = WorkStartTime;
                EndTime = WorkEndTime;
            }
            //比较神奇和复杂 看不懂 只能意会不得言传
            float Hours = (EndTime - StartTime).Hours + (float)(EndTime - StartTime).Minutes / 60;
            List<TimeSpan> times = db.SignInCardStatus.Where(p => p.StaffNumber.Equals(StaffNumber) && p.WorkDate == CurrentDate).Select(p => p.NeedWorkTime).ToList();
            for (int i = 1; i < times.Count - 1; i += 2)
                if (times[i].CompareTo(StartTime) >= 0 && times[i + 1].CompareTo(EndTime) <= 0)
                    Hours -= (times[i + 1] - times[i]).Hours + (float)(times[i + 1] - times[i]).Minutes / 60;
            return Hours;
        }

        //读取原始打卡数据，写入打卡表
        public void SignIn(DateTime CurrentDate)
        {
            //获取所有未处理的原始考勤记录
            List<OriginalDataofCheckOn> SignDateTime = db.OriginalDataofCheckOn.Where(p => p.IsRead == false && p.Date == CurrentDate).ToList();
            foreach (OriginalDataofCheckOn OriginalData in SignDateTime)
            {
                //找到针对这一条原始记录的员工所对应的所有打卡时间点
                List<SignInCardStatus> SignInCardStatusList = db.SignInCardStatus.Where(p => p.StaffNumber.Equals(OriginalData.StaffNumber)).ToList();

                foreach (SignInCardStatus tmp in SignInCardStatusList)
                {
                    //打卡时间落在时间点
                    if (OriginalData.Time >= tmp.NeedStartTime && OriginalData.Time <= tmp.NeedEndTime)
                    {
                        if (tmp.SignInTime == null || OriginalData.Time < tmp.SignInTime)
                        {
                            tmp.SignInTime = OriginalData.Time;
                            //上班时间，超过正常上班时间点，算迟到
                            if (tmp.Type.Equals("上班") && OriginalData.Time > tmp.NeedWorkTime)
                            {
                                tmp.ComeLateMinutes = (OriginalData.Time - tmp.NeedWorkTime).Minutes + (OriginalData.Time - tmp.NeedWorkTime).Hours * 60;
                                //       tmp.ComeLateMinutes = tmp.ComeLateMinutes / LateMinutes * 60 + tmp.ComeLateMinutes / LateToHour;
                            }
                            //下班时间，早于正常上班时间点，算早退
                            if (tmp.Type.Equals("下班") && OriginalData.Time < tmp.NeedWorkTime)
                            {
                                tmp.LeaveEarlyMinutes = (tmp.NeedWorkTime - OriginalData.Time).Minutes + (tmp.NeedWorkTime - OriginalData.Time).Hours * 60;
                                //       tmp.LeaveEarlyMinutes = tmp.LeaveEarlyMinutes / EarlyMinutes * 60 + tmp.LeaveEarlyMinutes / EarlyToHour;
                            }
                            db.Entry(tmp).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        break;
                    }
                }
            }

        }




        /// <summary>
        /// 读取机器数据文件，将机器数据写入考勤原始数据表,这个函数的解析文件的逻辑是与硬件数据格式相关的
        /// </summary>
        /// <param name="filepath">机器数据文件路径</param>
        public void WriteOriginalDataIntoSignInCardStatus(string filepath)
        {
            BonsaiiDbContext db = new BonsaiiDbContext("data source = 211.149.199.42,1433;initial catalog = bonsaii0000000008;user id = sa;password = admin123@;");
            StreamReader sr = System.IO.File.OpenText(@"d:\wj1.txt");
            string nextline;
            string strvalues = string.Empty;
            while ((nextline = sr.ReadLine()) != null)
            {
                string[] array = nextline.Split(new char[2] { '|', ' ' });
                OriginalDataofCheckOn tmp = new OriginalDataofCheckOn();
                tmp.PhysicalCardNumber = array[0];
                tmp.MachineNumber = array[1];
                tmp.Date = Convert.ToDateTime(array[2]);
                DateTime tmpDateTime = Convert.ToDateTime(array[3]);
                tmp.Time = new TimeSpan(tmpDateTime.Hour, tmpDateTime.Minute, tmpDateTime.Second);
                tmp.OriginalData = nextline;
                var tmpstaff = (from x in db.Staffs
                                where x.PhysicalCardNumber == tmp.PhysicalCardNumber
                                select new
                                {
                                    DepartmentName = x.Department,
                                    StaffName = x.Name,
                                    StaffNumber = x.StaffNumber
                                }).Single();
                tmp.DepartmentName = tmpstaff.DepartmentName;
                tmp.StaffNumber = tmpstaff.StaffNumber;
                tmp.StaffName = tmpstaff.StaffName;

                db.OriginalDataofCheckOn.Add(tmp);
                db.SaveChanges();
            }
        }



        /// <summary>
        /// 计算日考勤报表；将打卡表中 正班时间， 迟到总分钟数，早退总分钟数写入日考勤报表；这个仅仅考虑正班情况
        /// </summary>下员工号</param>
        public static void CalculateEveryDaySignInData(DateTime CurrentDate, string StaffNumber)
        {
        /// <param name="CurrentDate">日考勤报表中的时间</param>
        /// <param name="StaffNumber">同一时间
            //先处理正常上班的情况
            List<SignInCardStatus> SignInCardStatus = db.SignInCardStatus.Where(p => p.IsRead == false && p.StaffNumber == StaffNumber && p.WorkDate == CurrentDate && p.SignInType == SignInType.NORMALWORK).OrderBy(p => p.NeedWorkTime).ToList();
            EveryDaySignInDate everydaysignindate = null;
            WorkManages WorkManages = null;
            List<EveryDaySignInDate> EveryDays = db.EveryDaySignInDates.Where(p => p.Date == CurrentDate).ToList();
            List<WorkManages> Works = db.WorkManages.Where(p => p.Date == CurrentDate).ToList();
            try
            {
                everydaysignindate = EveryDays.Where(p => p.StaffNumber.Equals(StaffNumber)).First();
                WorkManages = Works.Where(p => p.StaffNumber == StaffNumber && p.Date == CurrentDate).Single();
                int totalhours = (from works in db.Works
                                  where works.Id == WorkManages.WorksId
                                  select works.TotalWorkHours).Single();
                everydaysignindate.WorkHours = totalhours;
                int totalearlyminutes = 0;
                int totallateminutes = 0;
                for (int i = 0; i < SignInCardStatus.Count; i++)
                {
                    //只要没有打卡，暂时只按照旷职处理
                    if (SignInCardStatus[i].SignInTime == null && SignInCardStatus[i].Type == "上班")
                        everydaysignindate.AbsenteeismHours += SignInCardStatus[i + 1].NeedWorkTime.Hours - SignInCardStatus[i].NeedWorkTime.Hours;
                    //最后一个判断条件的作用是：避免上下班都没有打卡被记两次旷职的情况，实际只记一次
                    if (SignInCardStatus[i].SignInTime == null && SignInCardStatus[i].Type == "下班" && SignInCardStatus[i - 1].SignInTime != null)
                        everydaysignindate.AbsenteeismHours += SignInCardStatus[i].NeedWorkTime.Hours - SignInCardStatus[i - 1].NeedWorkTime.Hours;

                    totallateminutes = totallateminutes + SignInCardStatus[i].ComeLateMinutes;
                    totalearlyminutes = totalearlyminutes + SignInCardStatus[i].LeaveEarlyMinutes;
                }
                totallateminutes = CalculateLateMinutes(totallateminutes);
                totalearlyminutes = CalculateEarlyMinutes(totalearlyminutes);
                everydaysignindate.TotalComeLateMinutes = totallateminutes;
                everydaysignindate.TotalLeaveEarlyMinutes = totalearlyminutes;
                everydaysignindate.AbsenteeismHours += CalculateAbsentHours(totallateminutes, totalearlyminutes);
                everydaysignindate.WorkHours -= everydaysignindate.AbsenteeismHours;
                db.Entry(everydaysignindate).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Tools.WriteErrorLog(e);
                return;
            }
        }

        /// <summary>
        /// 计算日考勤报表中的出差情况，其实和处理正班的情况是一样的，只是SignInType的值不同而已
        /// </summary>
        /// <param name="CurrentDate">日考勤报表中的时间</param>
        /// <param name="StaffNumber">同一时间下员工号</param>
        public static void CalculateEvection(DateTime CurrentDate)
        {
            
            List<SignInCardStatus> SignInCardStatus = db.SignInCardStatus.Where(p => p.IsRead == false && p.WorkDate == CurrentDate && p.SignInType == SignInType.EVECTION).OrderBy(p => p.NeedWorkTime).ToList();

            List<string> staffNumbers = SignInCardStatus.Select(p => p.StaffNumber).Distinct().ToList();
            WorkManages WorkManages = null;
            List<EveryDaySignInDate> EveryDays = db.EveryDaySignInDates.Where(p => p.Date == CurrentDate).ToList();
            List<WorkManages> Works = db.WorkManages.Where(p => p.Date == CurrentDate).ToList();
            foreach (string tmpStaffNumber in staffNumbers)
            {                    
                EveryDaySignInDate everydaysignindate = null;
                try
                {
                    everydaysignindate = EveryDays.Where(p => p.StaffNumber.Equals(tmpStaffNumber)).First();
                    WorkManages = Works.Where(p => p.StaffNumber == tmpStaffNumber && p.Date == CurrentDate).Single();
                    int totalhours = (from works in db.Works
                                      where works.Id == WorkManages.WorksId
                                      select works.TotalWorkHours).Single();
                    everydaysignindate.WorkHours = totalhours;
                    int totalearlyminutes = 0;
                    int totallateminutes = 0;
                    for (int i = 0; i < SignInCardStatus.Count; i++)
                    {
                        //只要没有打卡，暂时只按照旷职处理
                        if (SignInCardStatus[i].SignInTime == null && SignInCardStatus[i].Type == "上班")
                            everydaysignindate.AbsenteeismHours += SignInCardStatus[i + 1].NeedWorkTime.Hours - SignInCardStatus[i].NeedWorkTime.Hours;
                        //最后一个判断条件的作用是：避免上下班都没有打卡被记两次旷职的情况，实际只记一次
                        if (SignInCardStatus[i].SignInTime == null && SignInCardStatus[i].Type == "下班" && SignInCardStatus[i - 1].SignInTime != null)
                            everydaysignindate.AbsenteeismHours += SignInCardStatus[i].NeedWorkTime.Hours - SignInCardStatus[i - 1].NeedWorkTime.Hours;

                        totallateminutes = totallateminutes + SignInCardStatus[i].ComeLateMinutes;
                        totalearlyminutes = totalearlyminutes + SignInCardStatus[i].LeaveEarlyMinutes;
                    }
                    totallateminutes = CalculateLateMinutes(totallateminutes);
                    totalearlyminutes = CalculateEarlyMinutes(totalearlyminutes);
                    everydaysignindate.TotalComeLateMinutes = totallateminutes;
                    everydaysignindate.TotalLeaveEarlyMinutes = totalearlyminutes;
                    everydaysignindate.AbsenteeismHours += CalculateAbsentHours(totallateminutes, totalearlyminutes);
                    everydaysignindate.WorkHours -= everydaysignindate.AbsenteeismHours;
                }
                catch (Exception e)
                {
                    Tools.WriteErrorLog(e);
                    return;
                }
                db.Entry(everydaysignindate).State = EntityState.Modified;
                db.SaveChanges();
            }
        }



        //根据打卡表中的SignInType,单独处理加班的打卡情况
        public static void CalculateOvertime(DateTime CurrentDate)
        {
            List<SignInCardStatus> SignInCards = db.SignInCardStatus.Where(p => p.IsRead == false && p.WorkDate == CurrentDate && p.SignInType == SignInType.OVERTIME).ToList();
            List<string> staffNumbers = SignInCards.Select(p => p.StaffNumber).Distinct().ToList();
            List<EveryDaySignInDate> EveryDays = db.EveryDaySignInDates.Where(p => p.Date == CurrentDate).ToList();
            foreach (string StaffNumber in staffNumbers)
            {
                //处理加班打卡的情况
                List<SignInCardStatus> SignInCardStatus = SignInCards.Where(p => p.StaffNumber.Equals(StaffNumber)).ToList();
                EveryDaySignInDate everydaysignindate = null;
                try
                {
                    everydaysignindate = EveryDays.Where(p => p.StaffNumber.Equals(StaffNumber)).First();
                    int totalearlyminutes = 0;
                    int totallateminutes = 0;
                    //加班暂时不做未打卡算旷职的处理
                    foreach (SignInCardStatus sc in SignInCardStatus)
                    {
                        totallateminutes += sc.ComeLateMinutes;
                        totalearlyminutes += sc.LeaveEarlyMinutes;
                    }
                    totallateminutes = CalculateLateMinutes(totallateminutes);
                    totalearlyminutes = CalculateEarlyMinutes(totalearlyminutes);
                    everydaysignindate.TotalComeLateMinutes += totallateminutes;
                    everydaysignindate.TotalLeaveEarlyMinutes += totalearlyminutes;
                    double tmpAbsentHours = CalculateAbsentHours(totallateminutes, totalearlyminutes);
                    everydaysignindate.AbsenteeismHours += tmpAbsentHours;
                    everydaysignindate.TotalWorkOvertimeHours -= tmpAbsentHours;
                }
                catch (Exception e)
                {
                    Tools.WriteErrorLog(e);
                    return;
                }
                db.Entry(everydaysignindate).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        /**
         * 关于调休有一个地方需要注意：
         *调休的小时数并不会计算到工时当中，仅仅在OnDutyHours这个表中记录一个员工总的值班时数
         * 当某一个员工选择某一天调休的时候，将用他OnDutyHours中的值班数来确定最大的调休小时数
         * 而选择调休的这一天中，他调休的小时数仍会计算到他这一天的工时当中！这也是值班数不算做工时的原因！
         * */
        //根据打卡表中的SignInType,单独处理值班的打卡情况
        //如果系统参数设置了值班需要打卡才执行这个函数！ 
        public static void CalculateOnDuty(DateTime CurrentDate)
        {
            List<SignInCardStatus> SignInCards = db.SignInCardStatus.Where(p => p.IsRead == false && p.WorkDate == CurrentDate && p.SignInType == SignInType.ONDUTY).ToList();
            List<string> staffNumbers = SignInCards.Select(p => p.StaffNumber).Distinct().ToList();
     //       List<EveryDaySignInDate> EveryDays = db.EveryDaySignInDates.Where(p => p.Date == CurrentDate).ToList();
            foreach (string StaffNumber in staffNumbers)
            {
                //找到某一个人今天有值班，针对值班的打卡情况
                //处理值班打卡的情况
                List<SignInCardStatus> SignInCardStatus = SignInCards.Where(p => p.StaffNumber.Equals(StaffNumber)).ToList();
                try
                {
                    //找到这个人的值班申请
                    OnDutyApplies onDutyApply = db.OnDutyApplies.Where(p => p.StaffNumber.Equals(StaffNumber) && p.Date == CurrentDate).Single();
             //       EveryDaySignInDate everydaysignindate = null;
                    try
                    {
                        //         everydaysignindate = EveryDays.Where(p => p.StaffNumber.Equals(StaffNumber)).First();
                        int totalearlyminutes = 0;
                        int totallateminutes = 0;
                        //加班暂时不做未打卡算旷职的处理
                        foreach (SignInCardStatus sc in SignInCardStatus)
                        {
                            totallateminutes += sc.ComeLateMinutes;
                            totalearlyminutes += sc.LeaveEarlyMinutes;
                        }
                        totallateminutes = CalculateLateMinutes(totallateminutes);
                        totalearlyminutes = CalculateEarlyMinutes(totalearlyminutes);


                        //找到这个员工的值班时数情况，如果没有，则要在表中插入这个员工
                        OnDutyHours onDutyHours = db.OnDutyHours.Find(StaffNumber);
                        double totalHours = onDutyApply.Hours - CalculateAbsentHours(totallateminutes, totalearlyminutes) ?? 0;
                        //找到这个员工的值班时数，更新它
                        if (onDutyHours != null)
                        {
                            onDutyHours.AvailableHours += totalHours;
                            db.Entry(onDutyHours).State = EntityState.Modified;
                        }

                        db.SaveChanges();
                        // everydaysignindate.TotalComeLateMinutes += totallateminutes;
                        // everydaysignindate.TotalLeaveEarlyMinutes += totalearlyminutes;
                        // double tmpAbsentHours = CalculateAbsentHours(totallateminutes, totalearlyminutes);
                        // everydaysignindate.AbsenteeismHours += tmpAbsentHours;
                        //everydaysignindate.TotalWorkOvertimeHours -= tmpAbsentHours;
                        //                      db.Entry(everydaysignindate).State = EntityState.Modified;
                        //                      db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Tools.WriteErrorLog(e);
                        return;
                    }
                }
                catch (Exception e)
                {
                    Tools.WriteErrorLog(e);
                    return;
                }

            }
        }




        /// <summary>
        /// 根据考勤参数中设置的LateMinutes以及LateToHour，计算系统认为的迟到时间(统一按照分钟计算）
        /// </summary>
        /// <param name="LateMinutes">实际的迟到时间</param>
        /// <returns>系统根据LateMinutes以及LateToHour计算出的迟到时间(统一按照分钟计算）</returns>
        public static int CalculateLateMinutes(int LateMinutes)
        {
            return (LateMinutes / LateMinutes * LateToHour * 60 + LateMinutes % LateMinutes);
        }


        /// <summary>
        /// 根据考勤参数中设置的EarlyMinutes以及EarlyToHour，计算系统认为的早退时间(统一按照分钟计算）
        /// </summary>
        /// <param name="EarlyMinutes">实际的早退时间</param>
        /// <returns>系统根据EarlyMinutes以及EarlyToHour计算出的早退时间(统一按照分钟计算）</returns>
        public static int CalculateEarlyMinutes(int EarlyMinutes)
        {
            return (EarlyMinutes / EarlyMinutes * EarlyToHour * 60 + EarlyMinutes % EarlyMinutes);
        }

        /// <summary>
        /// 根据考勤参数设置，计算由于迟到和早退产生的旷职时数
        /// </summary>
        /// <param name="LateMinutes">迟到分钟数</param>
        /// <param name="EarlyMinutes">早退分钟数</param>
        /// <returns></returns>
        public static double CalculateAbsentHours(int a, int b)
        {
            double LateMinutes = a;
            double EarlyMinutes = b;
            double tmp = (LateMinutes / AbsentismMinutes * AbsentismHour + (LateMinutes % AbsentismMinutes) / 60) + (EarlyMinutes / AbsentismMinutes * AbsentismHour + (EarlyMinutes % AbsentismMinutes) / 60);
            return tmp;
        }


        /// <summary>
        /// 检查所有的签卡申请并处理
        /// </summary>
        /// 
        public static void CalculateChargeCard(DateTime CurrentDate)
        {
            /**
             * 签卡的处理逻辑目前为：
             * 1、首先获取所有未处理的签卡请求
             * 2、将签卡的时间点，插入最为接近的打卡表中的SignInTime
             * 3、调用函数，重新计算日考勤报表
             * 注意：处理签卡的流程是和当前日期无关的！签卡很可能是补签之前任意一天的内容 
             * */

            //只筛选未处理和审核通过的签卡申请 
            //var test = db.ChargeCardApplies.ToList();
            //int ii = test.Count;
            //var Cards2 = (from cc in db.ChargeCardApplies where cc.IsRead == false && cc.AuditStatus == 3 select cc).ToList();
            //List<ChargeCardApplies> Cards1 = (from cc in db.ChargeCardApplies where cc.IsRead == false && cc.AuditStatus == 3 select cc).ToList();
            List<ChargeCardApplies> Cards = db.ChargeCardApplies.Where(p => p.IsRead == false && p.AuditStatus == 3).ToList();
            foreach (ChargeCardApplies tmp in Cards)
            {
                List<SignInCardStatus> SignIns = db.SignInCardStatus.Where(p => p.WorkDate == tmp.DateTime.Date && p.StaffNumber.Equals(tmp.StaffNumber)).OrderBy(p => p.NeedWorkTime).ToList();
                //遍历某个人，这一天的所有签卡时间，找到最符合的那个签卡时间点
                for (int i = 0; i < SignIns.Count; i++)
                    if (SignIns[i].NeedWorkTime.Hours == tmp.DateTime.Hour)
                    {
                        SignIns[i].SignInTime = tmp.DateTime.TimeOfDay;
                        db.Entry(SignIns[i]).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                //重算日考勤报表
                ReCalculateEveryDaySignIn(tmp.Date, tmp.StaffNumber);
                //加班需要打卡，则重算
                if (OvertimeNeedSignIn)
                    ReCalculateOvertime(tmp.Date, tmp.StaffNumber);
                if (EvectionNeedSignIn)
                    ReCalculateEvection(tmp.Date, tmp.StaffNumber);
            }
        }

        /// <summary>
        /// 针对签卡过程，重新计算日考勤报表
        /// </summary>
        public static void ReCalculateEveryDaySignIn(DateTime CurrentDate, string StaffNumber)
        {
            //先处理正常上班的情况
            List<SignInCardStatus> SignInCardStatus = db.SignInCardStatus.Where(p => p.StaffNumber == StaffNumber && p.WorkDate == CurrentDate && p.SignInType == SignInType.NORMALWORK).OrderBy(p => p.NeedWorkTime).ToList();
            EveryDaySignInDate everydaysignindate = null;
            WorkManages WorkManages = null;
            List<EveryDaySignInDate> EveryDays = db.EveryDaySignInDates.Where(p => p.Date == CurrentDate).ToList();
            List<WorkManages> Works = db.WorkManages.Where(p => p.Date == CurrentDate).ToList();
            try
            {
                everydaysignindate = EveryDays.Where(p => p.StaffNumber.Equals(StaffNumber)).First();
                WorkManages = Works.Where(p => p.StaffNumber == StaffNumber && p.Date == CurrentDate).Single();
                int totalhours = (from works in db.Works
                                  where works.Id == WorkManages.WorksId
                                  select works.TotalWorkHours).Single();
                everydaysignindate.WorkHours = totalhours;
                int totalearlyminutes = 0;
                int totallateminutes = 0;
                //清零旷职时数
                everydaysignindate.AbsenteeismHours = 0;
                for (int i = 0; i < SignInCardStatus.Count; i++)
                {
                    //只要没有打卡，暂时只按照旷职处理
                    if (SignInCardStatus[i].SignInTime == null && SignInCardStatus[i].Type == "上班")
                        everydaysignindate.AbsenteeismHours += SignInCardStatus[i + 1].NeedWorkTime.Hours - SignInCardStatus[i].NeedWorkTime.Hours;
                    //最后一个判断条件的作用是：避免上下班都没有打卡被记两次旷职的情况，实际只记一次
                    if (SignInCardStatus[i].SignInTime == null && SignInCardStatus[i].Type == "下班" && SignInCardStatus[i - 1].SignInTime != null)
                        everydaysignindate.AbsenteeismHours += SignInCardStatus[i].NeedWorkTime.Hours - SignInCardStatus[i - 1].NeedWorkTime.Hours;

                    totallateminutes = totallateminutes + SignInCardStatus[i].ComeLateMinutes;
                    totalearlyminutes = totalearlyminutes + SignInCardStatus[i].LeaveEarlyMinutes;
                }
                totallateminutes = CalculateLateMinutes(totallateminutes);
                totalearlyminutes = CalculateEarlyMinutes(totalearlyminutes);
                everydaysignindate.TotalComeLateMinutes = totallateminutes;
                everydaysignindate.TotalLeaveEarlyMinutes = totalearlyminutes;
                everydaysignindate.AbsenteeismHours += CalculateAbsentHours(totallateminutes, totalearlyminutes);
                everydaysignindate.WorkHours -= everydaysignindate.AbsenteeismHours;
            }
            catch (Exception e)
            {
                Tools.WriteErrorLog(e);
                return;
            }
            db.Entry(everydaysignindate).State = EntityState.Modified;
            db.SaveChanges();
        }

        /// <summary>
        /// 针对加班打卡，重新计算日考勤报表
        /// </summary>
        /// <param name="CurrentDate"></param>
        public static void ReCalculateOvertime(DateTime CurrentDate, string StaffNumber)
        {
            List<SignInCardStatus> SignInCards = db.SignInCardStatus.Where(p => p.WorkDate == CurrentDate && p.SignInType == SignInType.OVERTIME).ToList();
            List<EveryDaySignInDate> EveryDays = db.EveryDaySignInDates.Where(p => p.Date == CurrentDate).ToList();

            //处理加班打卡的情况
            List<SignInCardStatus> SignInCardStatus = SignInCards.Where(p => p.StaffNumber.Equals(StaffNumber)).ToList();
            EveryDaySignInDate everydaysignindate = null;
            try
            {
                everydaysignindate = EveryDays.Where(p => p.StaffNumber.Equals(StaffNumber)).First();
                int totalearlyminutes = 0;
                int totallateminutes = 0;
                //加班暂时不做未打卡算旷职的处理
                foreach (SignInCardStatus sc in SignInCardStatus)
                {
                    totallateminutes += sc.ComeLateMinutes;
                    totalearlyminutes += sc.LeaveEarlyMinutes;
                }
                totallateminutes = CalculateLateMinutes(totallateminutes);
                totalearlyminutes = CalculateEarlyMinutes(totalearlyminutes);
                everydaysignindate.TotalComeLateMinutes += totallateminutes;
                everydaysignindate.TotalLeaveEarlyMinutes += totalearlyminutes;
                double tmpAbsentHours = CalculateAbsentHours(totallateminutes, totalearlyminutes);
                everydaysignindate.AbsenteeismHours += tmpAbsentHours;
                everydaysignindate.TotalWorkOvertimeHours -= tmpAbsentHours;
            }
            catch (Exception e)
            {
                Tools.WriteErrorLog(e);
                return;
            }
            db.Entry(everydaysignindate).State = EntityState.Modified;
            db.SaveChanges();

        }

        /// <summary>
        /// 针对出差打卡，重新计算日考勤
        /// </summary>
        /// <param name="CurrentDate"></param>
        /// <param name="StaffNumber"></param>
        public static void ReCalculateEvection(DateTime CurrentDate, string StaffNumber)
        {
            List<SignInCardStatus> SignInCardStatus = db.SignInCardStatus.Where(p => p.StaffNumber == StaffNumber && p.WorkDate == CurrentDate && p.SignInType == SignInType.EVECTION).OrderBy(p => p.NeedWorkTime).ToList();
            EveryDaySignInDate everydaysignindate = null;
            WorkManages WorkManages = null;
            List<EveryDaySignInDate> EveryDays = db.EveryDaySignInDates.Where(p => p.Date == CurrentDate).ToList();
            List<WorkManages> Works = db.WorkManages.Where(p => p.Date == CurrentDate).ToList();
            try
            {
                everydaysignindate = EveryDays.Where(p => p.StaffNumber.Equals(StaffNumber)).First();
                WorkManages = Works.Where(p => p.StaffNumber == StaffNumber && p.Date == CurrentDate).Single();
                int totalhours = (from works in db.Works
                                  where works.Id == WorkManages.WorksId
                                  select works.TotalWorkHours).Single();
                everydaysignindate.WorkHours = totalhours;
                int totalearlyminutes = 0;
                int totallateminutes = 0;
                for (int i = 0; i < SignInCardStatus.Count; i++)
                {
                    //只要没有打卡，暂时只按照旷职处理
                    if (SignInCardStatus[i].SignInTime == null && SignInCardStatus[i].Type == "上班")
                        everydaysignindate.AbsenteeismHours += SignInCardStatus[i + 1].NeedWorkTime.Hours - SignInCardStatus[i].NeedWorkTime.Hours;
                    //最后一个判断条件的作用是：避免上下班都没有打卡被记两次旷职的情况，实际只记一次
                    if (SignInCardStatus[i].SignInTime == null && SignInCardStatus[i].Type == "下班" && SignInCardStatus[i - 1].SignInTime != null)
                        everydaysignindate.AbsenteeismHours += SignInCardStatus[i].NeedWorkTime.Hours - SignInCardStatus[i - 1].NeedWorkTime.Hours;

                    totallateminutes = totallateminutes + SignInCardStatus[i].ComeLateMinutes;
                    totalearlyminutes = totalearlyminutes + SignInCardStatus[i].LeaveEarlyMinutes;
                }
                totallateminutes = CalculateLateMinutes(totallateminutes);
                totalearlyminutes = CalculateEarlyMinutes(totalearlyminutes);
                everydaysignindate.TotalComeLateMinutes = totallateminutes;
                everydaysignindate.TotalLeaveEarlyMinutes = totalearlyminutes;
                everydaysignindate.AbsenteeismHours += CalculateAbsentHours(totallateminutes, totalearlyminutes);
                everydaysignindate.WorkHours -= everydaysignindate.AbsenteeismHours;
            }
            catch (Exception e)
            {
                Tools.WriteErrorLog(e);
                return;
            }
            db.Entry(everydaysignindate).State = EntityState.Modified;
            db.SaveChanges();
        }





        /// <summary>
        /// 计算月考勤报表
        /// </summary>
        public void CalculateMonthSignInData(string StaffNumber, int Month)
        {
            DateTime dt = DateTime.Now;
            /**
             * 获取本月最后一天的方法是从网上找到，比较巧妙
             * */
            DateTime dt_First = dt.AddDays(-(dt.Day) + 1).Date;
            //将本月月数+1  
            DateTime dt2 = dt.AddMonths(1);
            //本月最后一天时间  
            DateTime dt_Last = dt2.AddDays(-(dt.Day)).Date;

            MonthSignIn msi = new MonthSignIn();
            //找到一个人一个月的所有日考勤报表
            List<EveryDaySignInDate> Months = db.EveryDaySignInDates.Where(p => DbFunctions.TruncateTime(p.Date) <= dt_Last && DbFunctions.TruncateTime(p.Date) >= dt_First).ToList();
            foreach (EveryDaySignInDate tmp in Months)
            {
                msi.NormalWorkHours += tmp.WorkHours;
                msi.OvertimeApplyHours += tmp.WorkOvertimeHours;

                if (tmp.TotalWorkOvertimeHours != 0)
                {
                    switch (tmp.OvertimeType)
                    {
                        case "正常加班": msi.NormalWorkHours = tmp.TotalWorkOvertimeHours; break;
                        case "双休加班": msi.WeekendOvertimeHours = tmp.TotalWorkOvertimeHours; break;
                        case "节假日加班": msi.HolidayOvertimeHours = tmp.TotalWorkOvertimeHours; break;
                        default: msi.OtherOvertimeHours = tmp.TotalWorkOvertimeHours; break;
                    }
                    msi.TotalOvertimeHours = tmp.TotalWorkOvertimeHours;
                }


                msi.ComeLateMinutes += tmp.TotalComeLateMinutes;
                if (tmp.TotalComeLateMinutes != 0)
                    msi.ComeLateTimes += 1;
                msi.LeaveEarlyMinutes += tmp.TotalLeaveEarlyMinutes;
                if (tmp.TotalLeaveEarlyMinutes != 0)
                    msi.LeaveEarlyTimes += 1;

                msi.AbsentHours = tmp.AbsenteeismHours;
                if (tmp.AbsenteeismHours != 0)
                    msi.AbsentTimes += 1;

                msi.VacateHours += tmp.VacateHours;
                if (tmp.VacateHours != 0)
                    msi.VacateTimes += 1;
            }
            //确定签卡次数
            msi.ChargeCardTimes = db.ChargeCardApplies.Where(p => p.StaffNumber.Equals(StaffNumber) && DbFunctions.TruncateTime(p.Date) <= dt_Last && DbFunctions.TruncateTime(p.Date) >= dt_First).ToList().Count;
            msi.NormalWorkDays = Convert.ToInt32(msi.NormalWorkHours / 24);
            msi.VacateDays = Convert.ToInt32(msi.VacateHours / 24);

            msi.StaffNumber = StaffNumber;
            msi.Date = DateTime.Now;

            try
            {
                db.MonthSignIns.Add(msi);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                var tmp = e;
            }
        }
    }
}
