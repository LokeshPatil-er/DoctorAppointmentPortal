using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPClassLibrary
{
    public class DoctorTableListFilter
    {
        public List<int> DoctorSpecializationsIdList { get; set; } = new List<int>();
        public string DoctorSpecializationIds => String.Join(",", this.DoctorSpecializationsIdList);
        public string SearchDoctorName { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
