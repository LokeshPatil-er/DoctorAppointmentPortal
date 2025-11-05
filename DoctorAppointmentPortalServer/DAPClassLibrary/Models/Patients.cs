using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DAPClassLibrary
{
    public class Patients
    {
        
        public int PatientId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string ContactNo { get; set; }
        [Required]
        public int GenderId { get; set; }
        public string Gender { get; set; }
        [Required]
        public int BloodGroupId { get; set; }

        public string BloodGroup {  get; set; }
        [Required]
        public string AddressLine1 {get;set;}
        [Required]
        public string Pincode { get; set; }
        [Required]
        public int TalukaId { get; set; }
        public string Taluka  { get;set; }
        [Required]
        public int DistrictId { get; set; }
        [Required]
        public int StateId { get; set; }
        [Required]
        public int CountryId { get; set; }
        [Required]
        public List<PreferredSlots> PreferredSlotsList { get; set; }
        public List<ReportFiles> UploadReports { get; set; }
        public string ProviderName { get; set; }
        public string PolicyNumber { get; set; }
        public string PolicyName { get; set; }
        public DateTime? ValidTill { get; set; }

    }

}
