using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
namespace Bonsaii.Models
{
    [Table("BillProperties")]
    public partial class BillPropertyModels
    {
        public int Id { get; set; }

        [Display(Name = "�������")]
        public string BillSort { get; set; }

        [Display(Name = "��������")]
        public string Type { get; set; }
        [Required]
        [Display(Name = "������������")]
        [StringLength(50)]
        public string TypeName { get; set; }
        [Display(Name = "��������ȫ��")]
        [StringLength(50)]
        public string TypeFullName { get; set; }

        [Display(Name = "���ݱ��뷽ʽ")]
        public string CodeMethod { get; set; }

        [StringLength(10)]
        [Display(Name = "������ʽ")]
        public string Code { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public int Day { get; set; }

        public int SerialNumber { get; set; }
        [Display(Name = "��˷�ʽ")]
        public int IsAutoAudit { get; set; }
        [Display(Name = "����������")]
        public bool? IsApprove { get; set; }
        [Display(Name = "�����޶������û�")]
        public bool IsLimitInput { get; set; }
        [Display(Name = "���Ӽ���")]
        public bool IsAscOrDesc { get; set; }

        public int Count { get; set; }
        [Display(Name = "¼��ʱ��")]
        public Nullable<DateTime> RecordTime { get; set; }
        [Display(Name = "¼����Ա")]
        public string RecordPerson { get; set; }
        [Display(Name = "����ʱ��")]
        public Nullable<DateTime> ChangeTime { get; set; }
        [Display(Name = "������Ա")]
        public string ChangePerson { get; set; }
        [Display(Name = "���ʱ��")]
        public Nullable<DateTime> AuditTime { get; set; }
        [Display(Name = "�����Ա")]
        public string AuditPerson { get; set; }

        [Display(Name = "�Ƿ���Ҫ��")]
        public bool NeedSignIn { get; set; }
    }
}
