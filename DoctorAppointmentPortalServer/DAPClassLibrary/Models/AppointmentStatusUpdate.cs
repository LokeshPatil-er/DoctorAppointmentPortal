using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPClassLibrary
{
    public class AppointmentStatusUpdate
    {
        [Required]
        public int AppointmentId { get; set; }

        [Required]
        public string NewAppointmentStatus { get; set; } 

        [Required]
        public int ActionId { get; set; }

        public int ModifiedBy { get; set; }
        public int PreferredSlotId { get; set; }
        public DateTime AlternateDate { get; set; }
        public string AlternateStartTime { get; set; }
        public string AlternateEndTime { get; set; }
    }
}
