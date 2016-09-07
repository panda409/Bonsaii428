using Bonsaii.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ConsoleTest
{
    class CounterManager
    {
        public BonsaiiDbContext db { get; set; }
        public SystemDbContext sysdb { get; set; }
        public List<string> ConnStrings { get; set; }

        public CounterManager()
        {
            sysdb = new SystemDbContext("Data Source = 211.149.199.42,1433;Initial Catalog = BonsaiiSystem;User ID = sa;Password = admin123@;");
            db = new BonsaiiDbContext("Data Source = 211.149.199.42,1433;Initial Catalog = Bonsaii0000000008;User ID = sa;Password = admin123@;");
            ConnStrings = sysdb.Users.Where(p => p.IsRoot == true).Select(p => p.ConnectionString).ToList();
        }


        /// <summary>
        /// 负责单据生成时的Counter的清零,日清零
        /// </summary>
        public void SetZeroEachDay()
        {
            //便利每一个公司
            foreach (string tmpConn in ConnStrings)
            {
                //对于不同的企业，分别初始化不同的DbContext,来操作不同的企业数据库
                using (db = new BonsaiiDbContext(tmpConn))
                {
                    List<BillPropertyModels> list = db.BillProperties.Where(p => p.CodeMethod.Equals(CodeMethod.Day)).ToList();
                    foreach (BillPropertyModels tmp in list)
                    {
                        tmp.Count = 1;
                        db.Entry(tmp).State = EntityState.Modified;
                    }
                }
            }
        }
        /// <summary>
        /// 负责单据生成时的Counter的清零,月清零
        /// </summary>
        public void SetZeroEachMonth()
        {
            //便利每一个公司
            foreach (string tmpConn in ConnStrings)
            {
                //对于不同的企业，分别初始化不同的DbContext,来操作不同的企业数据库
                using (db = new BonsaiiDbContext(tmpConn))
                {
                    List<BillPropertyModels> list = db.BillProperties.Where(p => p.CodeMethod.Equals(CodeMethod.Month)).ToList();
                    foreach (BillPropertyModels tmp in list)
                    {
                        tmp.Count = 1;
                        db.Entry(tmp).State = EntityState.Modified;
                    }
                }
            }
        }



        /// <summary>
        /// 根据单据类型编号，生成单号
        /// </summary>
        /// <param name="BillTypeNumber">单据类型编号</param>
        /// <returns>单号</returns>
        public string GenerateBillNumber(string BillTypeNumber)
        {
            BillPropertyModels tmp = db.BillProperties.Where(p => p.Type == BillTypeNumber).Single();
            string date = DateTime.Now.ToString("yyyyMMdd");
            //为流水号补充零
            string SerialNumber = AddZero(tmp.Count, tmp.SerialNumber);
            //更新单号的计数值
            tmp.Count++;
            db.Entry(tmp).State = EntityState.Modified;
            db.SaveChanges();

            switch (tmp.CodeMethod)
            {
                case CodeMethod.Day:
                    return DateTime.Now.ToString("yyyyMMdd").ToString() + SerialNumber;
                case CodeMethod.Month:
                    return DateTime.Now.ToString("yyyyMM").ToString() + SerialNumber;
                case CodeMethod.Serial:
                    return tmp.Code.Substring(0, 10 - tmp.SerialNumber) + SerialNumber;
                default:
                    return "";
            }
        }

        /// <summary>
        /// 将SerialNumber补零凑够length长度的数值
        /// </summary>
        /// <param name="SerialNumber"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public string AddZero(int SerialNumber, int length)
        {
            string tmp = SerialNumber.ToString();
            while (tmp.Length != length)
                tmp = tmp.Insert(0, "0");
            return tmp;
        }
         
    }
}
