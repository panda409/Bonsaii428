using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BacklogService
{
    public class User
    {
        public int Id { get; set; }
        public String AcciName { get; set; }
        public String MessTitle { get; set; }
        public String MessContent { get; set; }
        public String Recipient { get; set; }
        public String Name { get; set; }
        public String TelNum { get; set; }
        public String EmailAddr { get; set; }
        public Boolean Type { get; set; }
        public Boolean SendMess { get; set; }
        public Boolean Email { get; set; }
        public DateTime? StartTime { get; set; }
        public TimeSpan? RemindTime { get; set; }
        public byte Cycle { get; set; }
        public DateTime? QuitTime { get; set; }
        public Boolean IsUse { get; set; }
        public DateTime? OnlyOneDate { get; set; }
    }
}
