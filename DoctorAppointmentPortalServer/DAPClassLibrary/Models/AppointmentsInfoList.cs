using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPClassLibrary
{
    public class AppointmentsInfoList
    {
        public List<PatientAppointmentRequest> patientsAppointmentsList { get; set; }
        public int TotalRecored {  get; set; }
    }
}
