using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bonsaii.Models.GlobalStaticVaribles
{
    public class CirculateMethod
    {
        public string Id { get; set; }
        public string Description { get; set; }

        public const string Day = "每日提醒";
        public const string Week = "每周提醒";
        public const string Month = "每月提醒";

        public static SelectList GetCirculateMethod()
        {
            List<SelectListItem> list = new List<SelectListItem>()
            {
                new SelectListItem() { Value = (1).ToString(), Text = Day },
                new SelectListItem() { Value = (7).ToString(), Text = Week },
                //月份的天数要用DaysInMonth方法来动态的获取
                new SelectListItem() { Value = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month).ToString(), Text = Month },
            };
            return new SelectList(list, "Value", "Text");
        }
    }
}