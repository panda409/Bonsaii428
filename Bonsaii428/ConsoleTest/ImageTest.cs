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
using System.Configuration;
using System.Timers;
using ConsoleTest;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using Bonsaii.Models;
using Bonsaii.Models.Audit;
using System.Reflection;
namespace ConsoleTest
{
    class ImageTest
    {
            //if (FileData != null)
            //{
            //    staff.HeadType = FileData.ContentType;//获取图片类型
            //    staff.Head = new byte[FileData.ContentLength];//新建一个长度等于图片大小的二进制地址
            //    FileData.InputStream.Read(staff.Head, 0, FileData.ContentLength);//将image读取到Logo中

            //}
        public void GenerateImage()
        {
            BonsaiiDbContext db = new BonsaiiDbContext("Data Source = 211.149.199.42,1433;Initial Catalog = Bonsaii0000000008;User ID = sa;Password = admin123@;");
            Staff staff = db.Staffs.Where(p => p.StaffNumber.Equals("RSXZ000119")).Single();
            MemoryStream ms = new MemoryStream((byte[])staff.Head);
            byte[] image = ms.ToArray();
            FileStream fs = File.OpenWrite("D://test.png");
            fs.Write(image, 0, image.Length);
            fs.Close();
        }

     

    }
}
