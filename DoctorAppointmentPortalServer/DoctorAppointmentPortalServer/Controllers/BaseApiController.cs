using DAPClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Mvc;

namespace DoctorAppointmentPortalServer.Controllers
{

 
    public class BaseApiController : ApiController
    {

        protected ClaimsIdentity claimsIdentity
        {
            get
            {
                return (ClaimsIdentity)User.Identity;
            }
        }

        protected int UserId
        {
            get
            {
                var userIdClaim = claimsIdentity.FindFirst("userId");
                return userIdClaim != null ? Convert.ToInt32(userIdClaim.Value) : 0;
            }
        }

        protected string Email
        {
            get
            {
                var emailClaim = claimsIdentity.FindFirst("email");
                return emailClaim != null ? emailClaim.Value : string.Empty;
            }
        }

        protected string Role
        {
            get
            {
                var roleClaim = claimsIdentity.FindFirst("role");
                return roleClaim != null ? roleClaim.Value : string.Empty;
            }
        }

        protected int DoctorId
        {
            get
            {
                var doctorIdClaim = claimsIdentity.FindFirst("doctorId");
                return doctorIdClaim != null ? Convert.ToInt32(doctorIdClaim.Value) : 0;
            }
        }
    }
}

