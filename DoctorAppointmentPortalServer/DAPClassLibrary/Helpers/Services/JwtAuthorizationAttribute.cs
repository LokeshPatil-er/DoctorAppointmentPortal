using DAPClassLibrary.Helpers.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace DAPClassLibrary
{
    public class JwtAuthorizationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                var authHeader = actionContext.Request.Headers.Authorization;

                if (authHeader == null || authHeader.Scheme != "Bearer")
                {
                    actionContext.Response = actionContext.Request.CreateResponse(
                        HttpStatusCode.Unauthorized,
                        new { message = "Missing or invalid Authorization header." });
                    return;
                }

                var token = authHeader.Parameter;

                if (string.IsNullOrEmpty(token))
                {
                    actionContext.Response = actionContext.Request.CreateResponse(
                        HttpStatusCode.Unauthorized,
                        new { message = "Token is missing." });
                    return;
                }

                var principal = TokenServices.TokenVerify(token);

                if (principal == null)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(
                        HttpStatusCode.Unauthorized,
                        new { message = "Invalid or expired token." });
                    return;
                }

                actionContext.RequestContext.Principal = principal;
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(JwtAuthorizationAttribute), nameof(OnAuthorization));

             

             
                actionContext.Response = actionContext.Request.CreateResponse(
                    HttpStatusCode.InternalServerError,
                    new { message = "Token validation failed.", details = ex.Message });
            }
        }
    }
}
