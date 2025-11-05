using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPClassLibrary
{
    public class AppointmentsListFilters
    {
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public string AppointmentStatus { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; }
        public string PatientName { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime FromDate { get; set; }
        public int SpecializationId { get; set; }
    }
}
