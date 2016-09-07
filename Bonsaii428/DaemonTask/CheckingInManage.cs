//using Bonsaii.Models;
//using Bonsaii.Models.Checking_in;
//using Bonsaii.Models.Works;
//using BonsaiiModels.Checking_in;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ConsoleTest
//{
//    class CheckingInManage
//    {
//        public BonsaiiDbContext db { get; set; }
//        public SystemDbContext sysdb { get; set; }

//        public List<string> ConnStrings { get; set; }

//        //定义公共的工具函数
//        public CommonTool Tools { get; set; }

//        //无参数构造函数
//        public CheckingInManage()
//        {
//            sysdb = new SystemDbContext("Data Source = 211.149.199.42,1433;Initial Catalog = BonsaiiSystem;User ID = sa;Password = admin123@;");
//            Tools = new CommonTool();
//            ConnStrings = sysdb.Users.Where(p => p.IsRoot == true).Select(p => p.ConnectionString).ToList();
//        }



//        //生成每天所需要的打卡表和日考勤报表
//        public void GenerateEachDay()
//        {
//            DateTime CurrentDate = DateTime.Now;
//            foreach (string tmpConn in ConnStrings)
//            {
//                //对于不同的企业，分别初始化不同的DbContext,来操作不同的企业数据库
//                using (db = new BonsaiiDbContext(tmpConn))
//                {
//                    //按照排班生成最基本的打卡表和日考勤报表
//                    this.GenerateSignInCardStatusAndEveryDaySign(CurrentDate);

//                    this.Vacatedell(CurrentDate);

//                    this.CheckOvertimeApplies(CurrentDate);
//                }
//            }
//        }


//        //生成每天所需要的基本打卡表和日考勤报表
//        public void GenerateSignInCardStatusAndEveryDaySign(DateTime CurrentDate)
//        {
//            //找到所有需要打卡的员工号
//            List<string> StaffNumberList = db.Staffs.Where(p => p.FreeCard == false).Select(p => p.StaffNumber).ToList();
//            foreach (string sn in StaffNumberList)
//            {
//                //找到这个员工的所有班次
//                //写入信息到打卡表
//                int WorksId = db.WorkManages.Where(p => p.StaffNumber.Equals(sn) && p.Date.Equals(CurrentDate)).Select(p => p.WorksId).Single();
//                List<WorkTimes> WorkTimeList = db.WorkTimes.Where(p => p.WorksId == WorksId).ToList();
//                foreach (WorkTimes tmp in WorkTimeList)
//                {
//                    //添加上班时间点
//                    db.SignInCardStatus.Add(new SignInCardStatus()
//                    {
//                        StaffNumber = sn,
//                        WorkDate = CurrentDate,
//                        NeedWorkTime = tmp.StartTime,
//                        NeedStartTime = Tools.MinusMinutes(tmp.StartTime, tmp.AheadMinutes),
//                        NeedEndTime = Tools.AddMinutes(tmp.StartTime, tmp.LateMinutes),
//                        Type = "上班",
//                        ComeLateMinutes = 0,
//                        LeaveEarlyMinutes = 0

//                    });
//                    //添加下班时间点
//                    db.SignInCardStatus.Add(new SignInCardStatus()
//                    {
//                        StaffNumber = sn,
//                        WorkDate = CurrentDate,
//                        NeedWorkTime = tmp.EndTime,
//                        NeedStartTime = Tools.MinusMinutes(tmp.EndTime, tmp.LeaveEarlyMinutes),
//                        NeedEndTime = Tools.AddMinutes(tmp.EndTime, tmp.BackMinutes),
//                        Type = "下班",
//                        ComeLateMinutes = 0,
//                        LeaveEarlyMinutes = 0
//                    });
//                    db.SaveChanges();
//                }

//                //生成日考勤报表
//                db.EveryDaySignInDates.Add(new EveryDaySignInDate(sn, WorksId, CurrentDate));
//                db.SaveChanges();
//            }
//        }


//        /// <summary>
//        /// 处理某一天员工的请假情况
//        /// </summary>
//        /// <param name="CurrentDate">某一天的日期</param>
//        public void Vacatedell(DateTime CurrentDate)
//        {
//            //获取请假表中未读的请假申请,条件为IsRead，当前员工号和，，以列表形式输出；
//            List<VacateApplies> VacateApplies = db.VacateApplies.Where(p => p.IsRead == false && DbFunctions.TruncateTime(p.StartDateTime) <= CurrentDate && DbFunctions.TruncateTime(p.EndDateTime) >= CurrentDate).ToList();
//            // 遍历请假申请表，从未读的请假申请中获取员工号，请假开始时间，请假结束时间，请假日期；
//            foreach (VacateApplies va in VacateApplies)
//            {
//                //获取日考勤报表中的当前时间;
//                EveryDaySignInDate everydaysignindate = db.EveryDaySignInDates.Where(p => p.Date == CurrentDate && p.StaffNumber == va.StaffNumber).Single();



//                List<SignInCardStatus> signincardstatus = db.SignInCardStatus.Where(p1 => p1.StaffNumber == va.StaffNumber && p1.WorkDate == CurrentDate).ToList();
//                foreach (SignInCardStatus sica in signincardstatus)
//                {
//                    //遍历日考勤报表，获得NeedWorksTime,如果NeedWorksTime在请假表之间，便直接在打卡表中删除此点。
//                    if (sica.NeedWorkTime >= va.StartDateTime.TimeOfDay && sica.NeedWorkTime <= va.EndDateTime.TimeOfDay)
//                    {
//                        db.SignInCardStatus.Remove(sica);
//                        db.SaveChanges();
//                    }
//                    //向日考勤表报中写入数据，具体属性是添加请假类型和请假时数。
//                    string BillType = va.BillType;

//                    string BillTypeName = db.BillProperties.Where(p => p.Type == BillType).Single().TypeName;
//                    // 向日考勤报表中添加请假类型
//                    everydaysignindate.VacateType = BillTypeName;

//                    //向日考勤报表中添加请假时数
//                    //根据排班表和请假表中一样员工号获得员工的排班ID。根据排班ID获得Ｗorks中的TotaoWorkHours
//                    WorkManages WorkManages = db.WorkManages.Where(p1 => p1.StaffNumber == va.StaffNumber && p1.Date == CurrentDate).Single();

//                    int? totalhours = (from works in db.Works
//                                       where works.Id == WorkManages.WorksId
//                                       select works.TotalWorkHours).Single();
//                    //        根据请假单类型代码是不是年假调休班确定是HolidayHours还是VacateHours。
//                    //          获取请假表中请假时数 ，与Works表中规定的工作时数做判断，判断是否大于规定工作时间，如果大于则表示请假一整天，将请假时间赋值为规定工作时间
//                    if (BillType.Equals("3209"))
//                    {
//                        if (va.Hours >= totalhours)
//                        {
//                            everydaysignindate.HolidayHours = totalhours;
//                            everydaysignindate.VacateHours = 0;
//                        }
//                        else
//                        {
//                            everydaysignindate.HolidayHours = va.Hours;
//                            everydaysignindate.HolidayHours = 0;
//                        }
//                    }
//                    else
//                    {
//                        if (va.Hours >= totalhours)
//                        {
//                            everydaysignindate.VacateHours = totalhours;
//                            everydaysignindate.HolidayHours = 0;
//                        }
//                        else
//                        {
//                            everydaysignindate.VacateHours = va.Hours;
//                            everydaysignindate.HolidayHours = 0;

//                        }
//                    }
//                }
//                //      保存所做编辑
//                db.Entry(everydaysignindate).State = EntityState.Modified;
//                db.SaveChanges();

//            }
//        }



//        /// <summary>
//        /// 处理出差的情况
//        /// </summary>
//        /// <param name="CurrentDate"></param>
//        public void CheckEvectionApplies(DateTime CurrentDate)
//        {
//            List<EvectionApplies> list = db.EvectionApplies.Where(p => DbFunctions.TruncateTime(p.StartDateTime) <= CurrentDate && DbFunctions.TruncateTime(p.EndDateTime) >= CurrentDate).ToList();
//            bool flag = true;
//            try
//            {
//                //这一类单据是否需要打卡
//                flag = db.BillProperties.Where(p => p.Type.Equals("3101")).Single().NeedSignIn;
//            }
//            catch (Exception e)
//            {
//                Tools.WriteErrorLog(e);
//                return;
//            }
//            //遍历在CurrentDate内的所有出差申请
//            foreach (EvectionApplies tmp in list)
//            {
//                EveryDaySignInDate EveryDaySignIn = null;
//                try
//                {
//                    EveryDaySignIn = db.EveryDaySignInDates.Where(p => p.StaffNumber.Equals(tmp.StaffNumber) && p.Date == CurrentDate).Single();
//                }
//                catch (Exception e)
//                {
//                    Tools.WriteErrorLog(e);
//                    return;
//                }

//                //标记是出差考勤的情况
//                EveryDaySignIn.IsOnEvection = true;
//                //要打卡
//                if (flag)
//                {

//                }
//                //不用打卡,就直接删除打卡点就行
//                else
//                {
//                    List<SignInCardStatus> tmpList = db.SignInCardStatus.Where(p => p.StaffNumber.Equals(tmp.StaffNumber) && p.WorkDate == CurrentDate).ToList();
//                    db.SignInCardStatus.RemoveRange(tmpList);
//                }

//                //直接写入日考勤报表的工作时间
//                WorkManages work = db.WorkManages.Where(p => p.StaffNumber.Equals(tmp.StaffNumber) && p.Date == CurrentDate).Single();
//                EveryDaySignIn.WorkHours = db.Works.Where(p => p.Id == work.WorksId).Single().TotalWorkHours;

//                db.Entry(EveryDaySignIn).State = EntityState.Modified;
//                db.SaveChanges();   //保存更改
//            }
//        }

//        //判断某一天的假期情况
//        public bool IsHoliday(string StaffNumber,DateTime CurrentDate)
//        {
//            return false;
//        }





//    }
//}
