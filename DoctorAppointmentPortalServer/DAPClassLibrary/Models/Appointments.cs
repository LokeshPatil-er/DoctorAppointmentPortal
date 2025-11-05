using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPClassLibrary
{
    public class Appointments
    {
        [Key]
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }

        [Required(ErrorMessage = "DoctorId is required.")]
        public int DoctorId { get; set; }
        public string DoctorFirstName { get; set; }
        public string DoctorLastName { get; set; }

        [Required(ErrorMessage = "Reason for appointment is required.")]
        public string ReasonOfAppointment { get; set; }

        public List<string> DoctorSpecializations { get; set; }

        public string MedicalHistory { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string ApprovedStartTime { get; set; }
        public string ApprovedEndTime { get; set; }
        public int AppointmentStatusId { get; set; }
        public string AppointmentStatus { get; set; }
        public bool IsActive { get; set; } = true;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }

    }
}
