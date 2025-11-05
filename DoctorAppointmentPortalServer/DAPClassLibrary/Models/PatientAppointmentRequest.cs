using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPClassLibrary
{
    public class PatientAppointmentRequest
    {
        public Patients Patient { get; set; }
        public Appointments Appointment { get; set; }
    }
}
