using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPClassLibrary
{
    public class DoctorTableList
    {
        public List<Doctors> Doctors { get; set; }
        public int TotalRecords { get; set; }

        public DoctorTableList()
        {
            Doctors = new List<Doctors>();
            TotalRecords = 0;
        }
    }
}
