using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPClassLibrary
{
    public class Doctors
    {
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RoleShortCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ContactNo { get; set; }
        public int BloodGroupId { get; set; }
        public string BloodGroupName { get; set; }
        public int GenderId { get; set; }
        public string GenderName { get; set; }
        public int AddressId { get; set; }
        public string AddressLine1 { get; set; }
        public int TalukaId { get; set; }
        public string TalukaName { get; set; }
        public string Pincode { get; set; }
        public int YearOfExperience { get; set; }
        public int ConsultancyFee { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public List<int> DoctorQulificationsIdList { get; set; }
        public List<int> DoctorSpecializationsIdList { get; set; }
        public List<AvailableSlots> DoctorAvailableSlots { get; set; }
        public List<Countries> CountriesList { get; set; }
        public List<States> StatesList { get; set; }
        public List<Districts> DistrictsList { get; set; }
        public List<Talukas> TalukasList { get; set; }
        public List<BloodGroups> BloodGroupsList { get; set; }
        public List<Genders> GendersList { get; set; }
        public List<Qualifications> QualificationsList { get; set; }
        public List<Specializations> SpecializationsList { get; set; }

    }

    public class AvailableSlots
    {
        public int AvaliableSlotId { get; set; }
        public string dayOfWeek { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool IsAvailable { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
