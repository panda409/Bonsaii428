using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonsaiiModels
{

    [Table("CompanyPushes")]
    public class CompanyPush
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "推送群体")]
        public string Target { get; set; }
        [Required]
        [Display(Name = "推送内容")]
        public string TagContent { get; set; }
        [Required]
        [Display(Name = "推送目标")]
        public string Tag { get; set; }
        [Display(Name = "推送的时间")]
        public DateTime RecordTime { get; set; }
        [Display(Name = "推送账号")]
        public string RecordPerson { get; set; }
        [Display(Name = "推送人")]
        public string PersonName { get; set; }
        [Required]
        [Display(Name = "推送主题")]
        public string TagTitle { get; set; }
        [Required]
        [Display(Name = "推送类型")]
        public string Type { get; set; }
        [Display(Name = "推送主题图片")]
        public string Url { get; set; }

    }
}
