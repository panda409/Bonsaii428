using Bonsaii.Models;
using Bonsaii.Models.Checking_in;
using Bonsaii.Models.Works;
using BonsaiiModels;
using BonsaiiModels.GlobalStaticVaribles;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace ConsoleTest
    {
        class Tester
        {
            public BonsaiiDbContext db { get; set; }
            public SystemDbContext sysdb { get; set; }
            public List<string> ConnStrings { get; set; }
            //定义公共的工具函数
            public CommonTool Tools { get; set; }

            //一些和考勤相关的参数
            /// <summary>
            /// 加班打卡提前分钟
            /// </summary>
            public int OvertimeAheadMinutes { get; set; }
            /// <summary>
            /// 加班打卡推后分钟
            /// </summary>
            public int OvertimeBackMinutes { get; set; }
            /// <summary>
            /// 出差打卡提前分钟
            /// </summary>
            public int EvectionAheadMinutes { get; set; }
            /// <summary>
            /// 出差打卡推后分钟
            /// </summary>
            public int EvectionBackMinutes { get; set; }
            /// <summary>
            /// 迟到分钟数
            /// </summary>
            public int LateMinutes { get; set; }
            /// <summary>
            /// 迟到计小时值
            /// </summary>
            public int LateToHour { get; set; }
            /// <summary>
            /// 早退分钟数
            /// </summary>
            public int EarlyMinutes { get; set; }
            /// <summary>
            /// 早退计小时值
            /// </summary>
            public int EarlyToHour { get; set; }
            /// <summary>
            /// 出差是否打卡
            /// </summary>
            public bool EvectionNeedSignIn { get; set; }
            /// <summary>
            /// 加班是否打卡
            /// </summary>
            public bool OvertimeNeedSignIn { get; set; }
            /// <summary>
            /// 旷职分钟数
            /// </summary>
            public int AbsentismMinutes { get; set; }
            /// <summary>
            /// 旷职时数
            /// </summary>
            public int AbsentismHour { get; set; }
            //无参数构造函数
            public Tester()
            {
                db = new BonsaiiDbContext("Data Source = 211.149.199.42,1433;Initial Catalog = Bonsaii0000000008;User ID = sa;Password = admin123@;");
                sysdb = new SystemDbContext("Data Source = 211.149.199.42,1433;Initial Catalog = BonsaiiSystem;User ID = sa;Password = admin123@;");
                Tools = new CommonTool();
                ConnStrings = sysdb.Users.Where(p => p.IsRoot == true).Select(p => p.ConnectionString).ToList();
                this.OvertimeAheadMinutes = db.CheckingInParams.Find(1).Value;
                this.OvertimeBackMinutes = db.CheckingInParams.Find(2).Value;
                this.EvectionAheadMinutes = db.CheckingInParams.Find(3).Value;
                this.EvectionBackMinutes = db.CheckingInParams.Find(4).Value;
                this.LateMinutes = db.CheckingInParams.Find(5).Value;
                this.LateToHour = db.CheckingInParams.Find(6).Value;
                this.EarlyMinutes = db.CheckingInParams.Find(7).Value;
                this.EarlyToHour = db.CheckingInParams.Find(8).Value;

                this.EvectionNeedSignIn = db.CheckingInParamsBools.Find(1).Value;
                this.OvertimeNeedSignIn = db.CheckingInParamsBools.Find(2).Value;

                this.AbsentismMinutes = db.CheckingInParams.Find(9).Value;
                this.AbsentismHour = db.CheckingInParams.Find(10).Value;
            }



            //生成每天所需要的打卡表和日考勤报表
            public void GenerateEachDay()
            {
                //    DateTime CurrentDate = DateTime.Now;
                DateTime CurrentDate = new DateTime(2015, 12, 31);
                foreach (string tmpConn in ConnStrings)
                {
                    //对于不同的企业，分别初始化不同的DbContext,来操作不同的企业数据库
                    using (db = new BonsaiiDbContext(tmpConn))
                    {
                        //按照排班生成最基本的打卡表和日考勤报表
                        this.GenerateSignInCardStatusAndEveryDaySign(CurrentDate);

                        //          this.Vacatedell(CurrentDate);

                        //          this.CheckOvertimeApplies(CurrentDate);
                    }
                }
            }

            public void GenerateMonty()
            {
                using (db = new BonsaiiDbContext("Data Source = 211.149.199.42,1433;Initial Catalog = Bonsaii0000000008;User ID = sa;Password = admin123@;"))
                {
                    for (int i = 1; i < 31; i++)
                        this.GenerateSignInCardStatusAndEveryDaySign(new DateTime(2016, 1, i));

                    //          this.Vacatedell(CurrentDate);

                    //          this.CheckOvertimeApplies(CurrentDate);
                }
            }


            //各种测试函数
            public void TestCheckin()
            {
                DateTime date = new DateTime(2016, 1, 3);
                this.CalculateEveryDaySignInData(date, "PHGL000106");
            }

            /// <summary>
            /// 针对某一个员工，生成某一天的日考勤报表和打卡表
            /// </summary>
            /// <param name="StaffNumber"></param>
            /// <param name="CurrentDate"></param>
            public void GenerateForEachPerson(string StaffNumber, DateTime CurrentDate)
            {
                List<HolidayTables> holidays = db.HolidayTables.Where(p => p.Date == CurrentDate).ToList();
                //找到每一个员工在CurrentDate这一天所有的WorkTimes
                var StaffWorks = (from x in db.Staffs.Where(p => p.FreeCard == false && p.StaffNumber.Equals(StaffNumber))
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
                    HolidayType = holidays[0].Type;
                List<OvertimeApplies> list = db.OvertimeApplies.Where(p => p.StartDateTime.Date == CurrentDate).ToList();

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
                            SignInType = 1,
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
                            SignInType = 1,
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
            /// 处理出差的情况
            /// </summary>
            /// <param name="CurrentDate"></param>
            public void CheckEvectionApplies(DateTime CurrentDate)
            {
                List<EvectionApplies> list = db.EvectionApplies.Where(p => DbFunctions.TruncateTime(p.StartDateTime) <= CurrentDate && DbFunctions.TruncateTime(p.EndDateTime) >= CurrentDate).ToList();
                bool NeedSignIn = db.CheckingInParamsBools.Find(1).Value;

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
                    if (NeedSignIn)
                    {
                        //更改打卡的类型
                        foreach (SignInCardStatus tmpSignIn in tmpList)
                            tmpSignIn.SignInType = 2;
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
            public void VacateDell(DateTime CurrentDate)
            {
                BonsaiiDbContext db = new BonsaiiDbContext("Data Source = 211.149.199.42,1433;Initial Catalog = Bonsaii0000000008;User ID = sa;Password = admin123@;");
                SystemDbContext sysDb = new SystemDbContext("Data Source = 211.149.199.42,1433;Initial Catalog = BonsaiiSystem;User ID = sa;Password = admin123@;");
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
            /// 计算日考勤报表；将打卡表中 正班时间， 迟到总分钟数，早退总分钟数写入日考勤报表；这个仅仅考虑正班情况
            /// </summary>
            /// <param name="CurrentDate">日考勤报表中的时间</param>
            /// <param name="StaffNumber">同一时间下员工号</param>
            public void CalculateEveryDaySignInData(DateTime CurrentDate, string StaffNumber)
            {
                //先处理正常上班的情况
                List<SignInCardStatus> SignInCardStatus = db.SignInCardStatus.Where(p => p.IsRead == false && p.StaffNumber == StaffNumber && p.WorkDate == CurrentDate && p.SignInType == 0).OrderBy(p => p.NeedWorkTime).ToList();
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
                    totallateminutes = this.CalculateLateMinutes(totallateminutes);
                    totalearlyminutes = this.CalculateEarlyMinutes(totalearlyminutes);
                    everydaysignindate.TotalComeLateMinutes = totallateminutes;
                    everydaysignindate.TotalLeaveEarlyMinutes = totalearlyminutes;
                    everydaysignindate.AbsenteeismHours += this.CalculateAbsentHours(totallateminutes, totalearlyminutes);
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
            /// 计算日考勤报表中的出差情况，其实和处理正班的情况是一样的，只是SignInType的值不同而已
            /// </summary>
            /// <param name="CurrentDate">日考勤报表中的时间</param>
            /// <param name="StaffNumber">同一时间下员工号</param>
            public void CalculateEvection(DateTime CurrentDate, string StaffNumber)
            {
                //先处理正常上班的情况
                List<SignInCardStatus> SignInCardStatus = db.SignInCardStatus.Where(p => p.IsRead == false && p.StaffNumber == StaffNumber && p.WorkDate == CurrentDate && p.SignInType == 2).OrderBy(p => p.NeedWorkTime).ToList();
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
                    totallateminutes = this.CalculateLateMinutes(totallateminutes);
                    totalearlyminutes = this.CalculateEarlyMinutes(totalearlyminutes);
                    everydaysignindate.TotalComeLateMinutes = totallateminutes;
                    everydaysignindate.TotalLeaveEarlyMinutes = totalearlyminutes;
                    everydaysignindate.AbsenteeismHours += this.CalculateAbsentHours(totallateminutes, totalearlyminutes);
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



            //根据打卡表中的SignInType,单独处理加班的打卡情况
            public void CalculateOvertime(DateTime CurrentDate)
            {
                List<SignInCardStatus> SignInCards = db.SignInCardStatus.Where(p => p.IsRead == false && p.WorkDate == CurrentDate && p.SignInType == 1).ToList();
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
                        totallateminutes = this.CalculateLateMinutes(totallateminutes);
                        totalearlyminutes = this.CalculateEarlyMinutes(totalearlyminutes);
                        everydaysignindate.TotalComeLateMinutes += totallateminutes;
                        everydaysignindate.TotalLeaveEarlyMinutes += totalearlyminutes;
                        double tmpAbsentHours = this.CalculateAbsentHours(totallateminutes, totalearlyminutes);
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


            /// <summary>
            /// 根据考勤参数中设置的LateMinutes以及LateToHour，计算系统认为的迟到时间(统一按照分钟计算）
            /// </summary>
            /// <param name="LateMinutes">实际的迟到时间</param>
            /// <returns>系统根据LateMinutes以及LateToHour计算出的迟到时间(统一按照分钟计算）</returns>
            public int CalculateLateMinutes(int LateMinutes)
            {
                return (LateMinutes / this.LateMinutes * this.LateToHour * 60 + LateMinutes % this.LateMinutes);
            }


            /// <summary>
            /// 根据考勤参数中设置的EarlyMinutes以及EarlyToHour，计算系统认为的早退时间(统一按照分钟计算）
            /// </summary>
            /// <param name="EarlyMinutes">实际的早退时间</param>
            /// <returns>系统根据EarlyMinutes以及EarlyToHour计算出的早退时间(统一按照分钟计算）</returns>
            public int CalculateEarlyMinutes(int EarlyMinutes)
            {
                return (EarlyMinutes / this.EarlyMinutes * this.EarlyToHour * 60 + EarlyMinutes % this.EarlyMinutes);
            }

            /// <summary>
            /// 根据考勤参数设置，计算由于迟到和早退产生的旷职时数
            /// </summary>
            /// <param name="LateMinutes">迟到分钟数</param>
            /// <param name="EarlyMinutes">早退分钟数</param>
            /// <returns></returns>
            public double CalculateAbsentHours(int a, int b)
            {
                double LateMinutes = a;
                double EarlyMinutes = b;
                double tmp = (LateMinutes / this.AbsentismMinutes * this.AbsentismHour + (LateMinutes % this.AbsentismMinutes) / 60) + (EarlyMinutes / this.AbsentismMinutes * this.AbsentismHour + (EarlyMinutes % this.AbsentismMinutes) / 60);
                return tmp;
            }
        }
    }
