using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPClassLibrary
{
    public enum EmailScenario
    {
        AppointmentSubmitted=1,
        AppointmentAccepted=2,
        AppointmentRejected=3,
        AlternateSlot=4,
        AlternateSlotReject=5,
        AlternateSlotAccept=6
    }
}
