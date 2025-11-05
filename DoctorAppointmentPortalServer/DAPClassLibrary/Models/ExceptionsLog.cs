using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPClassLibrary
{
    public class ExceptionsLog
    {
        public int ExceptionLogId { get; set; }

        public string ExceptionMessage { get; set; }

        public string ExceptionStackTrace { get; set; }

        public string InnerException { get; set; }

        public string Source { get; set; }

        public string TargetSite { get; set; }

        public int UserId { get; set; }  

        public DateTime LoggedOn { get; set; }

        public string AdditionalInfo { get; set; }

        public bool IsResolved { get; set; }
    }
}
