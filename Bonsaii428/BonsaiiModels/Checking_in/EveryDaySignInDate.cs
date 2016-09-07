namespace Bonsaii.Models.Checking_in
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EveryDaySignInDate")]
    public partial class EveryDaySignInDate
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Week { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }              //�տ��ڱ��������

        [Required]
        [StringLength(50)]
        public string StaffNumber { get; set; }         //Ա����

        public int WorksId { get; set; }                    //���Id�������ҵ�Ա����Ӧ�İ��ʱ�����Ϣ��WorkTimes)
        public double WorkHours { get; set; }              //����ʱ���������㿼�ڵ�ʱ���ɴ򿨱�������

        public double? WorkDays { get; set; }       //������������ʱ������

        public double NormalWorkOvertimeHours { get; set; }    //�����Ӱ�ʱ��,����ɴ򿨱�������

        public double WeekendWorkOvertimeHours { get; set; }   //˫�ݼӰ�ʱ��������ɴ򿨱�������

        [StringLength(50)]
        public string VacateType { get; set; }                          //������ͣ�����ٵ��ĵ����ã�Ԥ���ɵ�ʱ���ȡ��ٵ����ӵ����á�

        public double VacateHours { get; set; }                           //���ʱ����Ԥ���ɵ�ʱ���ȡ��ٵ���á�

        public double HolidayHours { get; set; }                          //�ݼ�ʱ����Ԥ���ɵ�ʱ���ȡ��ٵ���á�

        public double TotalWorkOvertimeHours { get; set; }         //�Ӱ���ʱ���������㿼�ڵ�ʱ���ɴ򿨱�����

        public int TotalComeLateMinutes { get; set; }            //�ٵ��ܷ������������㿼�ڵ�ʱ���ɴ򿨱�����

        public int TotalLeaveEarlyMinutes { get; set; }             //�����ܷ������������㿼�ڵ�ʱ���ɴ򿨱�����

        public double AbsenteeismHours { get; set; }                    //��ְʱ���������㿼�ڵ�ʱ���ɴ򿨱�����

        [StringLength(20)]
        public string AuditStatus { get; set; }                            //���״̬����ʱ������

        [StringLength(50)]
        public string StaffConfirm { get; set; }                        //��ʱ������

        public bool IsNightWork { get; set; }                         //�Ƿ�ҹ�࣬��ʱ������

        public double WorkOvertimeHours { get; set; }             //����Ӱ���ʱ�������ֵ�ӼӰ����뵥�л�ȡ��

        [StringLength(50)]
        public string OriginalSignInData { get; set; }              //��Դ�ڻ�����ԭʼ������,�������ʱ��д��

        [StringLength(255)]
        public string Remark { get; set; }


        public bool IsOnEvection { get; set; }

        public string OvertimeType { get; set; }

        //define a constructor with parameterless
        public EveryDaySignInDate()
        {
        }
        public EveryDaySignInDate(string StaffNumber, int WorksId,DateTime CurrentDate)
        {
            this.Week = DateTime.Now.DayOfWeek.ToString();
            this.Date = CurrentDate;
            this.StaffNumber = StaffNumber;
            this.WorksId = WorksId;
            this.WorkHours = 0;
            this.NormalWorkOvertimeHours = 0;
            this.WeekendWorkOvertimeHours = 0;
            this.TotalComeLateMinutes = 0;
            this.TotalWorkOvertimeHours = 0;
            this.TotalLeaveEarlyMinutes = 0;
            this.AbsenteeismHours = 0;
            this.WorkOvertimeHours = 0;
            this.IsOnEvection = false;
            this.IsNightWork = false;
            this.VacateHours = 0;
        }
    }
}