using ApiNN.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiNN
{
    public class SmsMessage
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
        public string ReportId { get; set; }
        public MessageStatus Status { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
