using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bonsaii.Models.GlobalStaticVaribles
{
    public class BillSortMethod
    {

        /// <summary>
        /// 从数据库中获取到所有的单据类别，并转换为SelectList
        /// </summary>
        /// <param name="connString">某个企业的连接字符串</param>
        /// <returns>可用于DropDownList显示的SelectList</returns>
        public static SelectList GetBillSortMethod(string connString)
        {
            BonsaiiDbContext db = new BonsaiiDbContext(connString);
            List<BillSort> BillSorts = db.BillSorts.ToList();

            List<SelectListItem> list = new List<SelectListItem>();
            foreach (BillSort tmp in BillSorts)
            {
                list.Add(new SelectListItem()
                {
                    Value = tmp.Sort,
                    Text = tmp.Name + "-" + tmp.Sort
                });
            }
            return new SelectList(list, "Value", "Text");
        }
    }
}