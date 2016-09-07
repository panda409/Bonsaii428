using Bonsaii.Models;
using Bonsaii.Models.Checking_in;
using Bonsaii.Models.Works;
using BonsaiiModels.GlobalStaticVaribles;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Threading;
using BonsaiiCommon;
using System.Data.SqlClient;
namespace ConsoleTest
{
    class Program1
    {
        public BonsaiiDbContext db { get; set; }
        public Program1()
        {
            db = new BonsaiiDbContext("Data source = 211.149.199.42,1433;initial catalog = bonsaii0000000008;user id = sa;password = admin123@;");
        }

        public void TestDb()
        {
            try
            {
                string conn = "Data Source = 211.149.199.42,1433;Initial Catalog = Bonsaii0000000008;User ID = sa;Password = admin123@;";
                string sql = "insert into dbo.DeviceOriginalData(UserID,DeviceID,DateTime,Verify,Action,Remark,MDIN,DoorStatus,JobCode,Antipassback) " +
        "values(@UserID,@DeviceID,@DateTime,@Verify,@Action,@Remark,@MDIN,@DoorStatus,@JobCode,@Antipassback)";
                SqlConnection connection = new SqlConnection(conn);
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();
                command.Parameters.Add(new SqlParameter("@UserID", "ZDC119"));
                command.Parameters.Add(new SqlParameter("@DeviceID", 1));
                command.Parameters.Add(new SqlParameter("@DateTime", "2016-4-3"));
                command.Parameters.Add(new SqlParameter("@Verify", 2));
                command.Parameters.Add(new SqlParameter("@Action", 1));
                command.Parameters.Add(new SqlParameter("@Remark", "remark"));
                command.Parameters.Add(new SqlParameter("@MDIN", "mdintest"));
                command.Parameters.Add(new SqlParameter("@DoorStatus", 1));
                command.Parameters.Add(new SqlParameter("@JobCode", 1));
                command.Parameters.Add(new SqlParameter("@Antipassback", 1));


                int res = command.ExecuteNonQuery();
                Console.WriteLine(res);
            }
            catch (Exception e)
            {
                var tmp = e;
            }
        }
        public class A
        {
            public string name { get; set; }
            public int age { get; set; }
        }


        public string Test(ref A aa)
        {
            A tmp = new A();
            tmp.name = "Nicholoes";
            tmp.age = 24;
            aa = tmp;
            return "HELO";
        }
        public string TestB(A aa)
        {
            A tmp = new A();
            tmp.name = "HElen";
            tmp.age = 29;
            aa = tmp;
            return "HELO";
        }
        static void Main(string[] args)
        {
            //A my = new A();
            //new Program1().Test(ref my);
            //Console.WriteLine(my.name + "  " + my.age);
            //A my2 = new A();
            //new Program1().TestB(my2);
            //Console.WriteLine(my2.name + "  " + my2.age);
            //       HXComm.SampleUse mySample = new HXComm.SampleUse();
            //        mySample.Test("YXA6MfhwoG6mEeWbT0ef-BFVfw", "YXA6GUEzoaBTvze7xX3RmlJnF1wMtZA", "fuckaholic", "fuckaholic");
            //         new CheckingInManage().CalculateMonthSignInData("BZ00000147", 1);
            //         new CheckingInManage().TestCheckin();
            //      new CheckingInManage().CalculateDay();
            ///       new CheckingInManage().GenerateSomeDayForTest();
            //       Console.WriteLine(90 / 35);
            //      Console.WriteLine(90 % 35);
            //      float x = double(100)/double(3);

            //       BonsaiiDbContext db = new BonsaiiDbContext("Data Source = 211.149.199.42,1433;Initial Catalog = Bonsaii0000000008;User ID = sa;Password = admin123@;");
            //      DateTime date = new DateTime(2016,1,12);
            //       OnDutyApplies apply = db.OnDutyApplies.Where(p=>p.Da)

        //       new UDPServer().recvDataFromDeviceUdp();
        //    new Program1().TestDb();
        }
    }
}
