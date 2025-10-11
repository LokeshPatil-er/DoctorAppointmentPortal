using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPServerLibrary
{
    public class Roles
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string ShortCode { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
