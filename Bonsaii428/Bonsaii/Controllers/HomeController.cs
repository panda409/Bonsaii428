using Bonsaii.Models;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Data;
using NPOI.XSSF.UserModel;
using System.Data.Entity.Validation;
using BonsaiiModels;

namespace Bonsaii.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult SettingBG() {
            //SystemDbContext db = new SystemDbContext();
            string ips = Request.UserHostAddress;
            return View();
        }
        public ActionResult Switch() { return View(); }
        public ActionResult WillComeSoon() { return View(); }

        /// <summary>
        /// 显示用户勾选中的菜单
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexMain() {
            List<Menu> Menus = db.Menus.Where(p=>p.IsShow==true).OrderBy(p => p.ParentOrder).ToList();
            List<MenuViewModel> menuViewModel = new List<MenuViewModel>();
            menuViewModel.Add(new MenuViewModel(Menus[0].ParentName, Menus[0].ParentClass,Menus[0].ParentOrder));
            int j = 0;
            menuViewModel[j].MenuNodes.Add(new MenuNode(Menus[0].Name, Menus[0].Href, Menus[0].IsShow,Menus[0].Id));
            for (int i = 1; i < Menus.Count; i++)
            {
                if (!Menus[i].ParentOrder.Equals(Menus[i - 1].ParentOrder))
                {
                    menuViewModel.Add(new MenuViewModel(Menus[i].ParentName, Menus[i].ParentClass, Menus[0].ParentOrder));
                    j++;
                }
                menuViewModel[j].MenuNodes.Add(new MenuNode(Menus[i].Name, Menus[i].Href, Menus[i].IsShow,Menus[i].Id));
            }
            /*Panda Add SystemMessage alert*/
            SystemDbContext db2 = new SystemDbContext();

            var count = (from p in db2.SystemMessages where (p.CompanyId) == this.CompanyId && (p.UserName == this.UserName) && (p.IsRead == false) select p).ToList();
            ViewBag.Mess = count.Count();
            return View(menuViewModel);
        }

        //private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles="abc")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult CreateExcel()
        {
            HSSFWorkbook wk = new HSSFWorkbook();
            ISheet tb = wk.CreateSheet("MyFirst");
            //创建一行，此行为第二行:创建行是从0开始
            IRow row = tb.CreateRow(1);
            ICell cell;
            for (int i = 0; i < 20; i++)
            {
                cell = row.CreateCell(i);  //在第二行中创建单元格
                cell.SetCellValue("This is a test");//循环往第二行的单元格中添加数据
            }
//            ISheet tb = wk.CreateSheet("MyFirst");
//            HSSFPatriarch patr = sheet.CreateDrawingPatriarch();
//            HSSFComment comment1 = patr.CreateComment(new HSSFClientAnchor(0, 0, 0, 0, 1, 2, 4, 4));
//            comment1.String = new HSSFRichTextString("Hello World");
//            comment1.Author = "NPOI Team"
//;
//            cell.CellComment = comment1;

            FileStream fs = new FileStream(@"f:/test.xls", FileMode.Create);
            wk.Write(fs);
            fs.Close();
            return View();
        }

        //读取xls文件
        private ActionResult ReadExcel()
        {
            StringBuilder sbr = new StringBuilder();
            FileStream fs = new FileStream(@"f:/test.xls",FileMode.Open);//打开myxls.xls文件
            
                HSSFWorkbook wk = new HSSFWorkbook(fs);   //把xls文件中的数据写入wk中
                for (int i = 0; i < wk.NumberOfSheets; i++)  //NumberOfSheets是myxls.xls中总共的表数
                {
                    ISheet sheet = wk.GetSheetAt(i);   //读取当前表数据
                    for (int j = 0; j <= sheet.LastRowNum; j++)  //LastRowNum 是当前表的总行数
                    {
                        IRow row = sheet.GetRow(j);  //读取当前行数据
                        if (row != null)
                        {
                            sbr.Append("-------------------------------------\r\n"); //读取行与行之间的提示界限
                            for (int k = 0; k <= row.LastCellNum; k++)  //LastCellNum 是当前行的总列数
                            {
                                ICell cell = row.GetCell(k);  //当前表格
                                if (cell != null)
                                {
                                    sbr.Append(cell.ToString());   //获取表格中的数据并转换为字符串类型
                                }
                            }
                        }
                    }
                }
            
            sbr.ToString();
            StreamWriter wr = new StreamWriter(new FileStream(@"f:/myText.txt", FileMode.Append));  //把读取xls文件的数据写入myText.txt文件中
            
                wr.Write(sbr.ToString());
                wr.Flush();

                return View();
        }
        /// <summary>
        /// 批量导出本校第一批派位学生
        /// </summary>
        /// <returns></returns>
        public FileResult ExportStu2()
        {
            // schoolname = "401";
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //获取list数据
            List<Staff> listRainInfo = db.Staffs.ToList();
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("电脑号");
            row1.CreateCell(1).SetCellValue("姓名");
            //将数据逐步写入sheet1各个行
            for (int i = 0; i < listRainInfo.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(listRainInfo[i].StaffNumber.ToString());
                rowtemp.CreateCell(1).SetCellValue(listRainInfo[i].Name.ToString());
            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "第一批电脑派位生名册.xls");
        }
        /// <summary>
        /// Excel导入
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public DataTable ImportExcelFile1(string filePath)//excel表格信息的读入数据库
        {
            IWorkbook workbook = null;
            #region//初始化信息           
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                if (filePath.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new XSSFWorkbook(fs);
                else if (filePath.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(fs);                             
            }
            catch (Exception e)
            {
                throw e;
            }
            #endregion
           ISheet sheet = workbook.GetSheetAt(0);
                DataTable table = new DataTable();
                IRow headerRow = sheet.GetRow(0);//第一行为标题行
                int cellCount = headerRow.LastCellNum;//LastCellNum = PhysicalNumberOfCells
                int rowCount = sheet.LastRowNum;//LastRowNum = PhysicalNumberOfRows - 1

                //handling header.
                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                    table.Columns.Add(column);
                }
                for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
                {
                    IRow row = sheet.GetRow(i);
                    DataRow dataRow = table.NewRow();

                    if (row != null)
                    {
                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            if (row.GetCell(j) != null)
                                dataRow[j] = GetCellValue(row.GetCell(j));
                        }
                    }

                    table.Rows.Add(dataRow);
                }
                return table;
            

        }
        public ActionResult ImportExcelFile()
        {
            HttpPostedFileBase file = Request.Files["file"]; 
            string fileName = Path.Combine(Request.MapPath("~/Tmp"), Path.GetFileName(file.FileName));
              string FileType = ".xls,.xlsx";//定义上传文件的类型字符串
              if (!FileType.Contains(fileName))
             {
                 ModelState.AddModelError("", "文件类型不对，只能导入xls和xlsx格式的文件");
                 return RedirectToAction("Index");
             }
           
            file.SaveAs(fileName);
            string BillTypeNumber = "2603";
            var item = (from p in db.BillProperties where p.Type == "2603" select p.TypeName).FirstOrDefault();
            string BillTypeName = item;
            /*生成单号*/
            string str = Generate.GenerateBillNumber(BillTypeNumber, this.ConnectionString);
            /*生成单号*/
            string BillNumber = str;
            DataTable list = new DataTable();
            list = ImportExcelFile1(fileName);
            for (int i = 0; i < list.Rows.Count; i++)//得到excel的所有行
            {
             
                BillStaffMapping mapping = new BillStaffMapping
                {
                    BillType = BillTypeNumber,
                    BillNumber = BillNumber,
                    StaffNumber = list.Rows[i][0].ToString(),  //从外部Excel表格获取员工工号
                    TelPhone=list.Rows[i][2].ToString(),
                    Email=list.Rows[i][3].ToString()

                };
                db.BillStaffMappings.Add(mapping); db.SaveChanges();

               

              
            } 
         


          return RedirectToAction("Index");
              
        }
        /// <summary>
        /// 根据Excel列类型获取列的值
        /// </summary>
        /// <param name="cell">Excel列</param>
        /// <returns></returns>
        private static string GetCellValue(ICell cell)
        {
            if (cell == null)
                return string.Empty;
            switch (cell.CellType)
            {
                case CellType.Blank:
                    return string.Empty;
                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();
                case CellType.Error:
                    return cell.ErrorCellValue.ToString();
                case CellType.Numeric:
                case CellType.Unknown:
                default:
                    return cell.ToString();//This is a trick to get the correct value of the cell. NumericCellValue will return a numeric value no matter the cell value is a date or a number
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Formula:
                    try
                    {
                        HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                        e.EvaluateInCell(cell);
                        return cell.ToString();
                    }
                    catch
                    {
                        return cell.NumericCellValue.ToString();
                    }
            }
        }

        public ActionResult SystemSetting()
        {
            return View();
        }
        
    }
}