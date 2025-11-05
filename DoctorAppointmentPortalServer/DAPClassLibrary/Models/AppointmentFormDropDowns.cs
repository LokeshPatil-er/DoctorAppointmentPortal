using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPClassLibrary
{
    public class AppointmentFormDropDowns
    {
        public List<Specializations> SpecializationsList { get; set; }
        public List<Talukas> TalukasList { get; set; }
        public List<Districts> DistrictsList { get; set; }
        public List<States> StatesList { get; set; }
        public List<Countries> CountriesList { get; set; }
        public List<Genders> GendersList { get; set; }
        public List<BloodGroups> BloodGroupsList { get; set; }
    }
}
