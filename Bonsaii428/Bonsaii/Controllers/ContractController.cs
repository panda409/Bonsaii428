using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bonsaii.Models;
using System.IO;
using System.Configuration;
using Bonsaii.Models.Audit;

namespace Bonsaii.Controllers
{
    /// <summary>
    /// 判断文件是否为空
    /// </summary>
    public static class HasFiles
    {
        public static bool HasFile(this HttpPostedFileBase file)
        {
            return (file != null && file.ContentLength > 0) ? true : false;
        }
    }
    public class ContractController : BaseController
    {
        //private BonsaiiDbContext db = new BonsaiiDbContext();
        public byte AuditApplicationContract(Contract contract)//(string BillTypeNumber,int id)
        {
            /*访问单据性质，看是否是自动审核*/
            var item = (from p in db.BillProperties where p.Type == contract.BillTypeNumber select p).ToList().FirstOrDefault();

            if (item.IsAutoAudit == 1)//自动审核是1
            { //如果为0 代表不能自动审核 如果为1  代表可以自动审核
                return 3;//代表自动审核
            }

            if (item.IsAutoAudit == 2)//手动审核是2
            {
                //手动审核,也写到db.AditApplications这个表中但是不走process？
                return 6;//手动审核

            }

            if (item.IsAutoAudit == 3)
            { //如果不自动审核，就要走人工审核流程。即，把信息写入db.AditApplications这个表中
                AuditApplication auditApplication = new AuditApplication();
                auditApplication.BType = item.Type;
                auditApplication.TypeName = item.TypeName;
                auditApplication.CreateDate = DateTime.Now;

                var template = (from p in db.AuditTemplates
                                where (
                                    (contract.BillTypeNumber== p.BType) && (DateTime.Now >= p.StartTime) && (DateTime.Now < p.ExpireTime)
                                    )
                                select p).ToList().FirstOrDefault();

                if (template != null)//如果没有用于审批的模板 那么这里没法运行
                {
                    var tmpBillType = (from p in db.BillProperties where p.Type == contract.BillTypeNumber select p.TypeFullName).ToList().Single();
                    Staff staff = db.Staffs.Where(c => c.StaffNumber.Equals(contract.SecondParty)).ToList().Single();
                    string departmentName = (from p in db.Departments where p.DepartmentId == contract.SecondParty select p.Name).SingleOrDefault();
                    auditApplication.BNumber = contract.BillNumber;
                    auditApplication.TId = template.Id;
                    auditApplication.Creator = this.UserName;
                    auditApplication.CreatorName=this.Name;
                    auditApplication.State = 0;//代表等待审核
                    auditApplication.Info =
                "单据名称：" + tmpBillType + ";" +
                "工       号：" + contract.SecondParty + ";" +
                "姓       名：" + contract.SecondParty + ";" +
                "所在部门：" + departmentName + ";" +
                "性       别：" + staff.Gender + ";" +
                "职       位：" + staff.Position + ";" +
                "用工性质：" + staff.WorkProperty + ";" +
                "合同标题："+contract.ContractObject+";"+
                "合同编号："+contract.ContractNumber+";"+
                 "签订时间：" + contract.SignDate+ ";" +
                 "到期时间：" + contract.DueDate + ";" +
                 "合同金额：" + contract.Amount+ ";"+
                 "备注：" + contract.Remark+ ";"+
               
                    db.AuditApplications.Add(auditApplication);
                    db.SaveChanges();

                    AuditStep step = db.AuditSteps.Find(template.FirstStepSId);
                    AuditProcess auditProcess = new AuditProcess();
                    auditProcess.AId = auditApplication.Id;
                    auditProcess.SId = step.SId;
                    auditProcess.TId = step.TId;
                    auditProcess.BType = auditApplication.BType;
                    auditProcess.BNumber = auditApplication.BNumber;
                    auditProcess.TypeName = auditApplication.TypeName;
                    auditProcess.Info = auditApplication.Info;
                    //auditProcess.Info = auditApplication.Info + "提交人员：" + auditApplication.CreatorName + "-" + auditApplication.Creator + ";" +
                    //    "提交日期：" + auditApplication.CreateDate + ";";
                    auditProcess.AuditDate = DateTime.Now;
                    auditProcess.CreateDate = auditApplication.CreateDate;
                    auditProcess.Result = 0;//待审
                    auditProcess.DeadlineDate = DateTime.Now.AddDays(step.Days);//记录一下该节点最晚的审核时间；
                    auditProcess.Approver = step.Approver;
                    db.AuditProcesses.Add(auditProcess);

                    db.SaveChanges();
                    return 0;//待审
                }
                else
                {
                    return 7;//待审(未能进入审核流程)
                }
            }
            return 0;//待审
        }
        public ActionResult ManualAudit(int? id, int flag)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract contract = db.Contracts.Find(id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            //手动审批；这部分是自己给自己审批
            //需要对原表做出的修改
            try
            {
                if (flag == 1)
                {
                    //通过审批
                    contract.AuditStatus = 3;
                }
                else
                {
                    //不通过审批
                    contract.AuditStatus = 4;

                }
                contract.AuditPerson = this.UserName;
                contract.AuditTime = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        public ActionResult Submit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract contract = db.Contracts.Find(id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            //提交审批
            byte status = AuditApplicationContract(contract);
            //需要对原表做出的修改
            contract.AuditStatus = status;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ContractDetails(string staffNumber) {
            var name = (from p in db.Staffs where p.StaffNumber == staffNumber&& p.ArchiveTag != true select p.Name).ToList().Single();
            var contract = (from c in db.Contracts where c.SecondParty == staffNumber&&c.AuditStatus==3
                            join s in db.Staffs on c.SecondParty equals s.StaffNumber
                            join bp in db.BillProperties on c.BillTypeNumber equals bp.Type
                            join d in db.Departments on s.Department equals d.DepartmentId
                            into gc    /*左联：显示所有员工表的字段*/
                            from d in gc.DefaultIfEmpty()
                            select new ContractViewModel { 
                                AuditStatus = c.AuditStatus, Id = c.Id, StaffNumber = c.SecondParty, 
                                BillTypeNumber = c.BillTypeNumber, BillTypeName = bp.TypeName, BillNumber = c.BillNumber, 
                                ContractNumber = c.ContractNumber, FirstParty = this.CompanyFullName, SecondParty = s.Name, 
                                Department = d.Name, SignDate = c.SignDate, DueDate = c.DueDate, Amount = c.Amount, ContractObject = c.ContractObject, ContractAttachmentName = c.ContractAttachmentName, 
                                Remark = c.Remark 
                            }).ToList();
            ViewBag.name = name;
           ViewBag.contract = contract;
            return View();
        }

        [Authorize(Roles = "Admin,Contract_Index")]
        // GET: Contract
        public ActionResult Index()
        {
            var recordList = (from p in db.ReserveFields
                              join q in db.TableNameContrasts
                              on p.TableNameId equals q.Id
                              where q.TableName == "Contracts" && p.Status == true
                              select p).ToList();
            ViewBag.recordList = recordList;
            var pp = (from saf in db.ContractReserves
                      join rf in db.ReserveFields on saf.FieldId equals rf.Id where rf.Status == true
                      select new ContractViewModel
                      {
                          Id = saf.Number,
                          Description = rf.Description,
                          Value = saf.Value
                      }).ToList();
            ViewBag.List = pp;
            var contract = (from c in db.Contracts
                            join s in db.Staffs on c.SecondParty equals s.StaffNumber
                            join bp in db.BillProperties on c.BillTypeNumber equals bp.Type
                            join d in db.Departments on s.Department equals d.DepartmentId
                            into gc    /*左联：显示所有员工表的字段*/
                            from d in gc.DefaultIfEmpty()
                            select new ContractViewModel {AuditStatus=c.AuditStatus,Id=c.Id,StaffNumber=c.SecondParty,BillTypeNumber=c.BillTypeNumber,BillTypeName=bp.TypeName,BillNumber=c.BillNumber,ContractNumber=c.ContractNumber,FirstParty=this.CompanyFullName,SecondParty=s.Name,Department=d.Name,SignDate=c.SignDate,DueDate=c.DueDate,Amount=c.Amount,ContractObject=c.ContractObject,ContractAttachmentName=c.ContractAttachmentName,Remark=c.Remark}).ToList();
            //List<Contract> Contract = (from p in db.Contracts select p).ToList();
            foreach (var tmp in contract)
            {
                //tmp.AuditStatus = (from p in db.AuditApplications where tmp.BillTypeNumber == p.BType && tmp.BillNumber == p.BNumber select p.State).ToList().FirstOrDefault();
                tmp.AuditStatusName = db.States.Find(tmp.AuditStatus).Description;
            }
            
            return View(contract.OrderByDescending(c=>c.Id));
        }
        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        //public FileContentResult GetImage(int Id)
        //{
        //    //var fileinfo = db.Contracts.Find(Id);
        //    //return File(fileinfo.FileUrl, fileinfo.MimeType, fileinfo.FileName);
        //    Contract com = db.Contracts.FirstOrDefault(p => p.Id == Id);
        //    if (com != null)
        //    {
        //        return File(com.ContractAttachment, com.ContractAttachmentType);//File方法直接将二进制转化为指定类型了。
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        // GET: Contract/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract contract = db.Contracts.Find(id); 
            if (contract == null)
            {
                return HttpNotFound();
            }
            ContractViewModel contractViewModel = new ContractViewModel();
            contractViewModel.Id = contract.Id;
            contractViewModel.BillTypeNumber = contract.BillTypeNumber;
            contractViewModel.BillNumber = contract.BillNumber;
            contractViewModel.StaffNumber=contract.SecondParty;
            contractViewModel.ContractNumber = contract.ContractNumber;
            contractViewModel.FirstParty = this.CompanyFullName;
            contractViewModel.SignDate = contract.SignDate;
            contractViewModel.DueDate = contract.DueDate;
            contractViewModel.Amount=contract.Amount;
            contractViewModel.ContractObject=contract.ContractObject;
            contractViewModel.Remark=contract.Remark;
            contractViewModel.ContractAttachmentName = contract.ContractAttachmentName;
            contractViewModel.AuditStatusName = db.States.Find(contract.AuditStatus).Description;
            contractViewModel.AuditStatus = contract.AuditStatus;
            var staffs = (from s in db.Staffs
                          join d in db.Departments on s.Department equals d.DepartmentId
                          where s.StaffNumber == contract.SecondParty
                          select new { Name = s.Name, Department = d.Name }).ToList();
            var billTypeNumber = db.BillProperties.Where(bp => bp.Type.Equals(contract.BillTypeNumber));
            foreach (var billTypeNumber1 in billTypeNumber)
            {
                contractViewModel.BillTypeName = billTypeNumber1.TypeName;
            }
           
            foreach (var temp in staffs)
            {
                contractViewModel.SecondParty = temp.Name;
                contractViewModel.Department = temp.Department;
            }
           
            /*表预留字段*/
            var fieldValueList = (from cr in db.ContractReserves
                                  join rf in db.ReserveFields on cr.FieldId equals rf.Id
                                  where cr.Number == id && rf.Status== true
                                  select new ContractViewModel { Id = cr.Number, Description = rf.Description, Value = cr.Value }).ToList();
            ViewBag.fieldValueList = fieldValueList;
            return View(contractViewModel);
        }
         [Authorize(Roles = "Admin,Contract_Create")]
        // GET: Contract/Create
        public ActionResult Create()
        {
        
           ViewBag.company = this.CompanyFullName;          
            var fieldList = (from p in db.ReserveFields
                             join q in db.TableNameContrasts
                            on p.TableNameId equals q.Id
                             where q.TableName == "Contracts" && p.Status == true
                            select p).ToList();
            ViewBag.fieldList = fieldList;
            return View();
        }

        // POST: Contract/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contract contract1, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (HasFiles.HasFile(file)) {
                    string miniType = file.ContentType;
                    Stream fileStream = file.InputStream;
                    string path = AppDomain.CurrentDomain.BaseDirectory + "files\\";
                    string filename = Path.GetFileName(file.FileName);
                    file.SaveAs(Path.Combine(path, filename));
                    contract1.ContractAttachmentType = miniType;
                    contract1.ContractAttachmentName = filename;
                    contract1.ContractAttachmentUrl = Path.Combine(path, filename);
                }
                
                contract1.RecordTime = DateTime.Now;
                contract1.RecordPerson = base.Name;
                byte status = AuditApplicationContract(contract1);  
                //需要对原表做出的修改
                contract1.AuditStatus = status;
                db.Contracts.Add(contract1);//存储到数据库
                db.SaveChanges();
                //没有找到该单据的审批模板
                if (status == 7)
                {
                    ViewBag.alertMessage = true;
                    ContractViewModel contract = new ContractViewModel();
                    contract.BillTypeNumber = contract1.BillTypeNumber;
                    contract.BillNumber = contract1.BillNumber;
                    contract.Department = Request["Department"];
                    contract.ContractNumber = contract1.ContractNumber;
                    contract.SecondParty = contract1.SecondParty;
                    contract.SignDate = contract1.SignDate;
                    contract.DueDate = contract1.DueDate;
                    contract.Amount = contract1.Amount;
                    contract.ContractObject = contract1.ContractObject;
                    contract.Remark = contract1.Remark;
                    
                    ViewBag.company = this.CompanyFullName;
                    var fieldList1 = (from p in db.ReserveFields
                                      join q in db.TableNameContrasts
                                          on p.TableNameId equals q.Id
                                      where q.TableName == "Contracts" && p.Status == true
                                      select p).ToList();
                    ViewBag.fieldList = fieldList1;
                    return View(contract);
                }
              
                var fieldList = (from p in db.ReserveFields
                                 join q in db.TableNameContrasts
                                     on p.TableNameId equals q.Id
                                 where q.TableName == "Contracts" && p.Status == true
                                 select p).ToList();
                ViewBag.fieldList = fieldList;

                /*遍历，保存员工基本信息预留字段*/
                foreach (var temp in fieldList)
                {
                    ContractReserve cr = new ContractReserve();
                    cr.Number = contract1.Id;
                    cr.FieldId = temp.Id;
                    cr.Value = Request[temp.FieldName];
                    /*占位，为了在Index中显示整齐的格式*/
                    if (cr.Value == null) cr.Value = " ";
                    db.ContractReserves.Add(cr);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.company = this.CompanyFullName;
                var fieldList = (from p in db.ReserveFields
                                 join q in db.TableNameContrasts
                                     on p.TableNameId equals q.Id
                                 where q.TableName == "Contracts" && p.Status == true
                                 select p).ToList();
                ViewBag.fieldList = fieldList;
                ContractViewModel contract = new ContractViewModel();
                contract.BillTypeNumber = contract1.BillTypeNumber;
                contract.BillNumber = contract1.BillNumber;
                contract.Department = Request["Department"];
                contract.ContractNumber = contract1.ContractNumber;
                contract.SecondParty = contract1.SecondParty;
                contract.SignDate = contract1.SignDate;
                contract.DueDate = contract1.DueDate;
                contract.Amount = contract1.Amount;
                contract.ContractObject = contract1.ContractObject;
                contract.Remark = contract1.Remark;
                //contract.AuditPerson = contract1.AuditPerson;
                //contract.AuditStatusName = contract1.AuditStatusName;
                //contract.AuditStatus = contract1.AuditStatus;
                return View(contract);

            }

        }
        public FilePathResult Download(int id)
        {
            var fileinfo = db.Contracts.Find(id);
            return File(fileinfo.ContractAttachmentUrl, fileinfo.ContractAttachmentType, fileinfo.ContractAttachmentName);
        }
                [Authorize(Roles = "Admin,Contract_Edit")]
        // GET: Contract/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract contract = db.Contracts.Find(id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            ContractViewModel contractViewModel = new ContractViewModel();
            contractViewModel.Id = contract.Id;
            contractViewModel.BillTypeNumber = contract.BillTypeNumber;
            contractViewModel.BillNumber = contract.BillNumber;
            contractViewModel.StaffNumber = contract.SecondParty;
            contractViewModel.ContractNumber = contract.ContractNumber;
            contractViewModel.FirstParty = this.CompanyFullName;
            contractViewModel.SignDate = contract.SignDate;
            contractViewModel.DueDate = contract.DueDate;
            contractViewModel.Amount = contract.Amount;
            contractViewModel.ContractObject = contract.ContractObject;
            contractViewModel.Remark = contract.Remark;
            contractViewModel.ContractAttachmentName = contract.ContractAttachmentName;
            //contractViewModel.ContractAttachmentUrl = contract.ContractAttachmentUrl;
            //contractViewModel.ContractAttachmentType = contract.ContractAttachmentType;
            //contractViewModel.RecordTime = contract.RecordTime;
            var staffs = (from s in db.Staffs
                          join d in db.Departments on s.Department equals d.DepartmentId
                          where s.StaffNumber == contract.SecondParty
                          select new { Name = s.Name, Department = d.Name }).ToList();
            var billTypeNumber = db.BillProperties.Where(bp => bp.Type.Equals(contract.BillTypeNumber));
            foreach (var billTypeNumber1 in billTypeNumber)
            {
                contractViewModel.BillTypeName = billTypeNumber1.TypeName;
            }

            foreach (var temp in staffs)
            {
                contractViewModel.SecondParty = temp.Name;
                contractViewModel.Department = temp.Department;
            }
            /*查找表预留字段*/
            var fieldList = (from cr in db.ContractReserves
                             join rf in db.ReserveFields on cr.FieldId equals rf.Id
                             where cr.Number == id && rf.Status == true
                             select new ContractViewModel { Description = rf.Description, Value = cr.Value }).ToList();
            ViewBag.fieldList = fieldList;

            return View(contractViewModel);
        }

        // POST: Contract/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ContractViewModel contract,HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    //先原删除文件
                    FileInfo file1 = new FileInfo(Server.MapPath("/files/" + contract.ContractAttachmentName));  //指定文件路径          
                    if (file1.Exists)//判断文件是否存在
                    {
                        file1.Attributes = FileAttributes.Normal;//将文件属性设置为普通,比方说只读文件设置为普通
                        file1.Delete();//删除文件
                    } 
                    //增加新文件
                    string miniType = file.ContentType;
                    Stream fileStream = file.InputStream;
                    string path = AppDomain.CurrentDomain.BaseDirectory + "files\\";
                    string filename = Path.GetFileName(file.FileName);
                    file.SaveAs(Path.Combine(path, filename));


                    contract.ContractAttachmentType = miniType;
                    contract.ContractAttachmentName = filename;
                    contract.ContractAttachmentUrl = Path.Combine(path, filename); 
                    Contract c = db.Contracts.Find(contract.Id);
                    c.ContractAttachmentName = contract.ContractAttachmentName;
                    c.ContractAttachmentType = contract.ContractAttachmentType;
                    c.ContractAttachmentUrl = contract.ContractAttachmentUrl;
                    c.SignDate = contract.SignDate;
                    c.DueDate = contract.DueDate;
                    c.Amount = contract.Amount;
                    c.ContractObject = contract.ContractObject;
                    c.ChangePerson = base.Name;
                    c.ChangeTime = DateTime.Now;
                    /*查找预留字段(value)*/
                    var fieldValueList = (from cr in db.ContractReserves
                                          join rf in db.ReserveFields on cr.FieldId equals rf.Id
                                          where cr.Number == contract.Id && rf.Status == true
                                          select new ContractViewModel { Id = cr.Id, Description = rf.Description, Value = cr.Value }).ToList();
                    /*给预留字段赋值*/
                    foreach (var temp in fieldValueList)
                    {
                        ContractReserve cr = db.ContractReserves.Find(temp.Id);
                        cr.Value = Request[temp.Description];
                        db.SaveChanges();
                    }
                   
                    //db.Entry(contract).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    /*查找预留字段(value)*/
                    var fieldValueList = (from cr in db.ContractReserves
                                          join rf in db.ReserveFields on cr.FieldId equals rf.Id
                                          where cr.Number == contract.Id && rf.Status == true
                                          select new ContractViewModel { Id = cr.Id, Description = rf.Description, Value = cr.Value }).ToList();
                    /*给预留字段赋值*/
                    foreach (var temp in fieldValueList)
                    {
                        ContractReserve cr = db.ContractReserves.Find(temp.Id);
                        cr.Value = Request[temp.Description];
                        db.SaveChanges();
                    }
                    Contract c = db.Contracts.Find(contract.Id);
                    //c.ContractAttachmentName = contract.ContractAttachmentName;
                    c.ContractNumber = contract.ContractNumber;
                    c.ChangePerson = base.Name;
                    c.ChangeTime = DateTime.Now;
                    c.SignDate = contract.SignDate;
                    c.DueDate = contract.DueDate;
                    c.Amount = contract.Amount;
                    c.ContractObject = contract.ContractObject;
                    c.Remark = contract.Remark;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            /*查找表预留字段*/
            var fieldList = (from cr in db.ContractReserves
                             join rf in db.ReserveFields on cr.FieldId equals rf.Id
                             where cr.Number == contract.Id && rf.Status == true
                             select new ContractViewModel { Id = cr.Id, Description = rf.Description, Value = cr.Value }).ToList();
            ViewBag.fieldList = fieldList;
            return View(contract);
        }

                [Authorize(Roles = "Admin,Contract_Delete")]
        // GET: Contract/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract contract = db.Contracts.Find(id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            ContractViewModel contractViewModel = new ContractViewModel();
            contractViewModel.AuditStatusName = db.States.Find(contractViewModel.AuditStatus).Description;
            contractViewModel.Id = contract.Id;
            contractViewModel.BillTypeNumber = contract.BillTypeNumber;
            contractViewModel.BillNumber = contract.BillNumber;
            contractViewModel.StaffNumber = contract.SecondParty;
            contractViewModel.ContractNumber = contract.ContractNumber;
            contractViewModel.FirstParty = this.CompanyFullName;
            contractViewModel.SignDate = contract.SignDate;
            contractViewModel.DueDate = contract.DueDate;
            contractViewModel.Amount = contract.Amount;
            contractViewModel.ContractObject = contract.ContractObject;
            contractViewModel.Remark = contract.Remark;
            contractViewModel.ContractAttachmentName = contract.ContractAttachmentName;
            contractViewModel.AuditStatus = contract.AuditStatus;
            contractViewModel.AuditStatusName = contract.AuditStatusName;
            contractViewModel.AuditPerson = contract.AuditPerson;
            var staffs = (from s in db.Staffs
                          join d in db.Departments on s.Department equals d.DepartmentId
                          where s.StaffNumber == contract.SecondParty
                          select new { Name = s.Name, Department = d.Name }).ToList();
            var billTypeNumber = db.BillProperties.Where(bp => bp.Type.Equals(contract.BillTypeNumber));
            foreach (var billTypeNumber1 in billTypeNumber)
            {
                contractViewModel.BillTypeName = billTypeNumber1.TypeName;
            }

            foreach (var temp in staffs)
            {
                contractViewModel.SecondParty = temp.Name;
                contractViewModel.Department = temp.Department;
            }
            /*查表预留字段(value)*/
            var fieldValueList = (from cr in db.ContractReserves
                                  join rf in db.ReserveFields on cr.FieldId equals rf.Id
                                  where cr.Number == id && rf.Status == true
                                  select new ContractViewModel { Id = cr.Number, Description = rf.Description, Value = cr.Value }).ToList();
            ViewBag.fieldValueList = fieldValueList;
            return View(contractViewModel);
        }

        // POST: Contract/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            /*Step1：删除预留字段*/
            var item = (from cr in db.ContractReserves
                        where cr.Number == id
                        select new ContractViewModel { Id = cr.Id }).ToList();
            foreach (var temp in item)
            {
                ContractReserve cr = db.ContractReserves.Find(temp.Id);
                db.ContractReserves.Remove(cr);

            }
            db.SaveChanges();

            /*Step2：删除固定字段*/
            Contract contract = db.Contracts.Find(id);
            FileInfo file = new FileInfo(Server.MapPath("/files/" + contract.ContractAttachmentName));  //指定文件路径          
            if (file.Exists)//判断文件是否存在
            {
                file.Attributes = FileAttributes.Normal;//将文件属性设置为普通,比方说只读文件设置为普通
                file.Delete();//删除文件
            } 
            db.Contracts.Remove(contract);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        /*实现单据类别搜索：显示单据类别编号和单据类别名称*/
        [HttpPost]
        public JsonResult BillTypeNumberSearch()
        {
            try
            {
                var items = (from b in db.BillProperties
                             where b.BillSort == "27"
                             select new
                             {
                                 text = b.Type + " " + b.TypeName,
                                 id = b.Type
                             }).ToList();

                return Json(items);
            }
            catch (Exception e) { return Json(new { success = false, msg = e.Message }); }


        }
        //可以提取成公共方法
        [HttpPost]
        public JsonResult BillTypeNumber(string number)
        {
            string temp = Generate.GenerateBillNumber(number, this.ConnectionString);
            try
            {
                var items = (from p in db.BillProperties
                             where p.Type.Contains(number) || p.TypeName.Contains(number)
                             select new
                             {
                                 billNumber = temp
                             }).ToList();

                return Json(items);
            }
            catch (Exception e) { return Json(new { success = false, msg = e.Message }); }


        }
        public JsonResult StaffNumberSearch()
        {
            try
            {
                var items = (from s in db.Staffs
                             where s.ArchiveTag != true && s.AuditStatus == 3
                             select new
                             {
                                 text = s.StaffNumber+" "+s.Name,
                                 id = s.StaffNumber
                             }).ToList();

                return Json(items);
            }
            catch (Exception e) { return Json(new { success = false, msg = e.Message }); }


        }
        [HttpPost]
        /*员工工号搜索 返回Json对象*/
        public JsonResult StaffContent(string number)
        {
            //var department = (from s in db.Staffs
            //                         join d in db.Departments on s.Department equals d.DepartmentId
            //                         where s.StaffNumber == number
            //                         select new { DepartmentAbbr=d.DepartmentAbbr }).SingleOrDefault();
            //string temp = Generate.GenerateContractNumber(department.DepartmentAbbr, this.ConnectionString);
            try
            {             
                var items = (from p in db.Staffs 
                             join d in db.Departments on p.Department equals d.DepartmentId
                             where p.StaffNumber==number
                             select new {  department=d.Name}).ToList();
                return Json(items);
            }
            catch (Exception e) { return Json(new { success = false, msg = e.Message }); }
        }
           
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
