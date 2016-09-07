namespace Bonsaii.Models.Checking_in
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EvectionAppliesViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Ա����")]
        public string StaffNumber { get; set; }
        [Display(Name = "Ա������")]
        public string StaffName { get; set; }
        [Display(Name = "��������")]
        public string BillTypeName { get; set; }

        [Display(Name = "��������")]
        public string DepartmentName { get; set; }

        [Display(Name = "��ʼʱ��")]
        public DateTime StartDateTime { get; set; }

         [Display(Name = "����ʱ��")]
        public DateTime EndDateTime { get; set; }

         [Display(Name = "��������")]
        public int Days { get; set; }

         [Display(Name = "���״̬")]
         public string AuditStatusName { get; set; }
    }
}
