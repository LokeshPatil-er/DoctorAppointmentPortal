using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BCrypt.Net;
using DAPClassLibrary;
using DAPServerLibrary;
using Bcrypt = BCrypt.Net.BCrypt;

namespace DoctorAppointmentPortalServer.Controllers
{
    public class AccountController : ApiController
    {

        [HttpPost]
        public HttpResponseMessage Login(Users user)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                UsersOps objUsersOps=new UsersOps();
                if(user==null || String.IsNullOrEmpty(user.Email) || String.IsNullOrEmpty(user.Password) )
                {
                   return response = Request.CreateResponse(HttpStatusCode.BadRequest,new { success=false ,message= "Email and Password are required." });
                   
                }

                objUsersOps.Email = user.Email;

                if(!objUsersOps.LoadUser())
                {
                    return response = Request.CreateResponse(HttpStatusCode.NotFound,new { success=false,message= "Invalid credentials." });
                }

                if(!Bcrypt.Verify(user.Password,objUsersOps.Password))
                {
                    return response = Request.CreateResponse(HttpStatusCode.BadRequest,new { success=false, message="Password is incorrect"});
                }

                string jwtToken = TokenServices.TokenGenertor(user.Email, objUsersOps.RoleShortCode, objUsersOps.UserId);

                if (String.IsNullOrEmpty(jwtToken))
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
                }

             return   response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, token=jwtToken,message="Login succesfully." });

            }

            catch(Exception ex)
            {
                return response = Request.CreateResponse(HttpStatusCode.InternalServerError,new {success=false,message="Login failed"});
            }

        }
    }
}
