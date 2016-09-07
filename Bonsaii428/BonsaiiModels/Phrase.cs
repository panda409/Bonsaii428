using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bonsaii.Models
{
    [Table("Phrases")]
    public class Phrase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name="序号")]
        [Range(1,1000)]
        public int Sn { get; set; }
        [Required]
        [Display(Name ="场景")]
        public string PhraseScene { get; set; }
        [Required]
        [Display(Name = "内容")]
        public string PhraseContent { get; set; }
        [Display(Name = "录入时间")]
        public Nullable<DateTime> RecordTime { get; set; }
        [Display(Name = "录入人员")]
        public string RecordPerson { get; set; }
        [Display(Name = "更改时间")]
        public Nullable<DateTime> ChangeTime { get; set; }
        [Display(Name = "更改人员")]
        public string ChangePerson { get; set; }
        [Display(Name = "审核时间")]
        public Nullable<DateTime> AuditTime { get; set; }
        [Display(Name = "审核人员")]
        public string AuditPerson { get; set; }
    }
    [Table("PhraseScenes")]
    public class PhraseScene
    {
        [Key]
        [Required]
        [Display(Name ="序号")]
        public int SnS { get; set; }
        [Required]
        [Display(Name="场景名称")]
        [MaxLength(50,ErrorMessage ="字符长度应该小于50")]
        public string SceneName { get; set; }
    }
    
}