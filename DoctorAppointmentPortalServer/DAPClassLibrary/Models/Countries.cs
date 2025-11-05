using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPClassLibrary
{
    public class Countries
    {
        public int CountryId {  get; set; }
        public string CountryName { get; set; }
        public bool IsActive {  get; set; }
        public int? CreatedBy {  get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy {  get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
