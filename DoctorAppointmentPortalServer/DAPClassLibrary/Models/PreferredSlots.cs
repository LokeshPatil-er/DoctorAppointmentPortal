using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPClassLibrary
{
    public class PreferredSlots
    {
        public int PreferredSlotId { get; set; }
        public DateTime PreferredDate { get; set; }
        public string PreferredStartTime { get; set; }
        public string PreferredEndTime { get; set; }
        public bool IsApproved { get; set; }
        public bool IsAlternateSlot { get; set; }
        public bool IsActive { get; set; }

    }
}
