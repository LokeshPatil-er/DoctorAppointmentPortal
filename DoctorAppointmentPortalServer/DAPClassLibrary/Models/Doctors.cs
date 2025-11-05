using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPClassLibrary
{
    public class Doctors
    {
        public int DoctorId { get; set; }

        public int UserId { get; set; }

        [Required]
        public string Email { get; set; }

       
        public string Password { get; set; }

        [Required]
        public string RoleShortCode { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string ContactNo { get; set; }

        [Required]
        public int BloodGroupId { get; set; }

     
        public string BloodGroupName { get; set; }

        [Required]
        public int GenderId { get; set; }

        public string GenderName { get; set; }

        public int AddressId { get; set; }

        [Required]
        public string AddressLine1 { get; set; }

        [Required]
        public int TalukaId { get; set; }

        public string TalukaName { get; set; }

        [Required]
        public int DistrictId { get; set; }

        public string DistrictName { get; set; }

        [Required]
        public int StateId { get; set; }

        public string StateName { get; set; }

        [Required]
        public int CountryId { get; set; }

        public string CountryName { get; set; }

        [Required]
        public string Pincode { get; set; }

        [Required]
        public DateTime ExperienceStartDate { get; set; }

        [Required]
        public int ConsultancyFee { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [Required]
        public List<int> DoctorQulificationsIdList { get; set; }=new List<int>();

        [Required]
        public List<int> DoctorSpecializationsIdList { get; set; }= new List<int>();

        [Required]
        public List<DoctorAvailableSlots> DoctorAvailableSlots { get; set; }


        public List<Qualifications> DoctorQualificationsList { get; set; }=new List<Qualifications> { };

        public string DoctorQualification=> string.Join(", ", DoctorSpecializationsList);
        public List<Specializations> DoctorSpecializationsList { get; set; }=new List<Specializations> { };
        public string DoctorSpecialization=>string.Join(", ",DoctorSpecializationsList);





    }

 

   

    

}
