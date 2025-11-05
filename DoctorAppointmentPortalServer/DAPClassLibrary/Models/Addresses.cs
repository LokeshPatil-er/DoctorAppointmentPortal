using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPClassLibrary
{
    public class Addresses
    {
        public int AddressId { get; set; }
        public string AddressLine1 { get; set; }
        public string Pincode { get; set; }
        public int TalukaId { get; set; }
        public string TalukaName { get; set; }   
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
