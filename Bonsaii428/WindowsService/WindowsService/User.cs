using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsService
{
    public class User
    {
        public int Id { get; set; }
        public String EventName { get; set; }
        public String MessageTitle { get; set; }
        public String MessageAlert { get; set; }
        public String MessageBody { get; set; }
        public String Receiver { get; set; }
        public String ReceiverName { get; set; }
        public String ReceiverTel { get; set; }
        public String ReceiverEmail { get; set; }
        public String ReceiverType { get; set; }
        public Boolean SendToApp { get; set; }
        public Boolean IsEmail { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? OnlyOneDate { get; set; }
        public TimeSpan? RemindDate { get; set; }
        public byte CirculateMethod { get; set; }
        public DateTime? EndDate { get; set; }
        public String SubscribeContent { get; set; }
        public Boolean IsAvailable { get; set; }    
    }
}
