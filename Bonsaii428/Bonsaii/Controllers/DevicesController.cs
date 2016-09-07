using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bonsaii.Models;
using BonsaiiModels;
using BonsaiiModels.DeviceUser;
using System.Text.RegularExpressions;
using Riss.Devices;

namespace Bonsaii.Controllers
{
    public class DevicesController : BaseController
    {
        DeviceConnection deviceConnection = null;
        /// <summary>
        /// 针对具体的某型号的某一台设备测试连接
        /// </summary>
        /// <returns></returns>
        public JsonResult TestConnection()
        {
            string DeviceType = Request["DeviceType"];
            int DeviceID = int.Parse(Request["DeviceID"]);
            string CommKey = Request["CommKey"];
            int Port = int.Parse(Request["Port"]);
            string IP = Request["IP"];

            //针对基类进行操作
            BaseDevice device = SimpleDeviceFactory.createDevice(DeviceType, DeviceID, CommKey, Port, IP);
            if (device.TestConnection() == true)
                return packageJson(1, " 设备连接成功", null);
            else
                return packageJson(0, "设备连接失败！", null);
        }

        public JsonResult TestConnectionById()
        {
            int Id = int.Parse(Request["DId"]);
            Devices device = db.Devices.Find(Id);
            BaseDevice dev = SimpleDeviceFactory.createDevice(device.DeviceType, device.DeviceID, device.CommKey, device.Port, device.IP);
            if (dev.TestConnection() == true)
                return packageJson(1, " 设备连接成功", null);
            else
                return packageJson(0, "设备连接失败！", null);
        }

        // GET: Devices
        public ActionResult Index()
        {
            return View(db.Devices.ToList());
        }
        public ActionResult ConnError()
        {
            return View();
        }
        /// <summary>
        /// 获取到硬件设备中的用户信息。同时要初始化以后硬件操作要用到的Device和DeviceConnection对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult UserList(int id)
        {
            ViewBag.DeviceId = id;
            List<DeviceUserViewModel> deviceUser = new List<DeviceUserViewModel>();
            try
            {
                Device device = new Device();
                DeviceConnection deviceConnection = this.createDeviceConnection(id, ref device);
                //设备连接成功
                if (deviceConnection.Open() > 0)    //连接设备成功
                {
                    object extraProperty = new object();
                    object extraData = new object();
                    try
                    {
                        //                 bool result = deviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, extraData);
                        extraProperty = (UInt64)0;
                        bool result = deviceConnection.GetProperty(DeviceProperty.Enrolls, extraProperty, ref device, ref extraData);
                        if (false == result)
                        {
                            return RedirectToAction("ConnError");
                        }
                        List<User> userList = (List<User>)extraData;
                        foreach (User user in userList)         //遍历所有的注册用户，获取其用户名，我也不知道为什么User这个对象里不存放用户名这个字段
                        {
                            //      Enroll enroll = user.Enrolls[0];
                            User u = user;
                            result = deviceConnection.GetProperty(UserProperty.UserName, extraProperty, ref u, ref extraData);
                            string tmpDIN = u.DIN.ToString();
                            var tmpDepts = db.Departments.Where(p => p.DepartmentId.Equals(tmpDIN)).ToList();
                            string deptName = null;
                            if (tmpDepts.Count == 0)
                                deptName = "暂无部门";
                            else
                                deptName = tmpDepts[0].Name;
                            if (result)
                            {
                                deviceUser.Add(new DeviceUserViewModel()
                                {
                                    Name = u.UserName,
                                    PhysicalCardNumber = u.DIN.ToString(),
                                    DepartmentName = deptName
                                });
                            }
                            else
                            {
                                deviceUser.Add(new DeviceUserViewModel()
                                {
                                    Name = "未设定",
                                    PhysicalCardNumber = u.DIN.ToString(),
                                    DepartmentName = deptName
                                });
                            }
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                return View();
            }
            return View(deviceUser);
        }




        /// <summary>
        /// 根据设备Id，找到设备中没有员工姓名的那些记录，根据他们的PhycisalCardNumber,从Staffs中找到对应的员工姓名，并写入到机器中
        /// </summary>
        /// <param name="DeviceId"></param>
        /// <returns></returns>
        public ActionResult WriteUserToDevice(int DeviceId)
        {
            Device device = new Device();
            DeviceConnection deviceConnection = this.createDeviceConnection(DeviceId,ref device);
            //设备连接成功
            if (deviceConnection.Open() > 0)    //连接设备成功
            {
                try
                {
                    object extraProperty = new object();
                    object extraData = new object();
                    extraProperty = (UInt64)0;
                    bool result = deviceConnection.GetProperty(DeviceProperty.Enrolls, extraProperty, ref device, ref extraData);
                    if (false == result)
                    {
                        return RedirectToAction("ConnError");
                    }
                    List<User> userList = (List<User>)extraData;
                    foreach (User user in userList)         //遍历所有的注册用户，获取其用户名，我也不知道为什么User这个对象里不存放用户名这个字段
                    {
                        //      Enroll enroll = user.Enrolls[0];
                        User u = user;
                        //          u.DIN = (UInt64)1;
                        result = deviceConnection.GetProperty(UserProperty.UserName, extraProperty, ref u, ref extraData);
                        string tmpPCN = u.DIN.ToString();
                        u.UserName = db.Staffs.Where(p => p.PhysicalCardNumber.Equals(tmpPCN)).ToList().First().Name;
                        //将机器上的用户名称重置为员工档案中的名称
                       deviceConnection.SetProperty(UserProperty.UserName, extraProperty, user, extraData);


                    }

                }
                catch (Exception e)
                {

                }
            }
            return RedirectToAction("UserList", new { id = DeviceId });
        }


        public DeviceConnection createDeviceConnection(int id,ref Device mydevice)
        {
            Devices dbDevice = db.Devices.Find(id);
            Device device = new Device();
            device.DN = dbDevice.DeviceID;//deviceID;
            device.Password = dbDevice.CommKey;//this.CommKey;
            device.Model = dbDevice.DeviceType;
            device.ConnectionModel = 5;//等于5时才能正确加载ZD2911通讯模块

            device.IpAddress = dbDevice.IP;//this.IP;  //txt_IP.Text.Trim();
            device.IpPort = dbDevice.Port;//this.Port;    //(int)nud_Port.Value;
            device.CommunicationType = CommunicationType.Tcp;
            mydevice = device;
            return DeviceConnection.CreateConnection(ref device);
        }
        // GET: Devices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Devices devices = db.Devices.Find(id);
            if (devices == null)
            {
                return HttpNotFound();
            }
            return View(devices);
        }

        // GET: Devices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Devices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DeviceID,CommKey,IP,Port,DeviceType,DeviceName")] Devices devices)
        {
            if (ModelState.IsValid)
            {
                db.Devices.Add(devices);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(devices);
        }

        // GET: Devices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Devices devices = db.Devices.Find(id);
            if (devices == null)
            {
                return HttpNotFound();
            }
            return View(devices);
        }

        // POST: Devices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DeviceID,CommKey,IP,Port,DeviceType,DeviceName")] Devices devices)
        {
            if (ModelState.IsValid)
            {
                db.Entry(devices).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(devices);
        }

        // GET: Devices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Devices devices = db.Devices.Find(id);
            if (devices == null)
            {
                return HttpNotFound();
            }
            return View(devices);
        }

        // POST: Devices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Devices devices = db.Devices.Find(id);
            db.Devices.Remove(devices);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        /// <summary>
        /// 判断是否为合法的IP地址格式
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <returns>true：合法的IP地址，false：非法的IP地址</returns>
        public static bool IsCorrenctIP(string ip)
        {
            if (Regex.IsMatch(ip, "[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}"))
            {
                string[] ips = ip.Split('.');
                if (4 == ips.Length)
                {
                    if (Int32.Parse(ips[0]) < 256 && Int32.Parse(ips[1]) < 256
                        && Int32.Parse(ips[2]) < 256 && Int32.Parse(ips[3]) < 256)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 封装接口调用要返回的Json对象
        /// </summary>
        /// <param name="result">结果值,0代表请求失败，1是成功，-1表示APP用户还没有登录</param>
        /// <param name="msg">执行的结果信息</param>
        /// <param name="data">执行的结果数据</param>
        /// <returns></returns>
        public JsonResult packageJson(int result, string msg, object data)
        {
            return Json(new
            {
                Result = result,
                Msg = msg,
                Data = data
            }, JsonRequestBehavior.AllowGet);
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
