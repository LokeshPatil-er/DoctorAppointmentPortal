using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace DAPClassLibrary
{
    public class DoctorsOps
    {
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public string Email {  get; set; }
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
        public string Pincode { get; set; }
        public int YearOfExperience { get; set; }
        public int ConsultancyFee { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }


        private Database db;

        public DoctorsOps()
        {
            db = DatabaseFactory.CreateDatabase("constr");
        }

        public DoctorsOps(int doctorId)
        {
            db = DatabaseFactory.CreateDatabase("constr");
            this.DoctorId=doctorId;
        }

        public bool InsertOrUpdateDoctor(Doctors doctorModel)
        {
            try
            {
                DbCommand dbCommand = db.GetStoredProcCommand("dap_doctorInsertOrUpdate");



                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }


    }
}
