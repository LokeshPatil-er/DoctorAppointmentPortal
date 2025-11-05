using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPClassLibrary
{
    public class DoctorAvailability
    {
        public List<DoctorAvailableSlots> AvailableSlots { get; set; }
        public List<Appointments> AcceptedAppointments { get; set; }

        public DoctorAvailability()
        {
            AvailableSlots = new List<DoctorAvailableSlots>();
            AcceptedAppointments = new List<Appointments>();
        }
    }
}
