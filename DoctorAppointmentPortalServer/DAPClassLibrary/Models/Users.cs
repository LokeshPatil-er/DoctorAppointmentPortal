using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPServerLibrary
{
    public class Users
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }    
        public int RoleId { get; set; }
        public string RoleShortCode { get; set; }
        public string Role { get; set; }
        public bool IsFirstLogin { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }



    }
}
