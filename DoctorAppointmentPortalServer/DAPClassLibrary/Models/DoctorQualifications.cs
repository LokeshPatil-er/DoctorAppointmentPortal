using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPClassLibrary
{
    public class DoctorQualifications
    {
        public int DoctorQualificationId { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }         

        public int QualificationId { get; set; }
        public string QualificationName { get; set; } 

        public string Description { get; set; }
        public bool IsActive { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
