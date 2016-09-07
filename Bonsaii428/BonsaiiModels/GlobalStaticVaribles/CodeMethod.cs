using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bonsaii.Models
{
    public class CodeMethod
    {
        public string Id { get; set; }
        public string Description { get; set; }

        public const string Day = "日编+流水";
        public const string Month = "月编+流水";
        public const string Serial = "流水";
        public const string Manual = "手动设置";
        public const string Five = "部门缩写+流水";

        public static List<CodeMethod> GetCodeMethod()
        {
            List<CodeMethod> list = new List<CodeMethod>();
            list.Add(new CodeMethod()
            {
                Id = CodeMethod.Day,
                Description = CodeMethod.Day
            });
            list.Add(new CodeMethod(){
                Id = CodeMethod.Month,
                Description = CodeMethod.Month
            });
            list.Add(new CodeMethod(){
                Id = CodeMethod.Serial,
                Description = CodeMethod.Serial
            });
            //list.Add(new CodeMethod(){
            //    Id = CodeMethod.Manual,
            //    Description = CodeMethod.Manual
            //});
            list.Add(new CodeMethod()
            {
                Id = CodeMethod.Five,
                Description = CodeMethod.Five
            });
            return list;
        }


        public static List<CodeMethod> GetBillType()
        {
            List<CodeMethod> list = new List<CodeMethod>();
            list.Add(new CodeMethod()
            {
                Id = CodeMethod.Day,
                Description = CodeMethod.Day
            });
            list.Add(new CodeMethod()
            {
                Id = CodeMethod.Month,
                Description = CodeMethod.Month
            });
            list.Add(new CodeMethod()
            {
                Id = CodeMethod.Serial,
                Description = CodeMethod.Serial
            });
            //list.Add(new CodeMethod(){
            //    Id = CodeMethod.Manual,
            //    Description = CodeMethod.Manual
            //});
            //list.Add(new CodeMethod()
            //{
            //    Id = CodeMethod.Five,
            //    Description = CodeMethod.Five
            //});
            return list;
        }
    }
}