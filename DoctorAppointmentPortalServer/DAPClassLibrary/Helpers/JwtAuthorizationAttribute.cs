using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace DAPClassLibrary
{
    public class JwtAuthorizationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //base.OnAuthorization(actionContext);
            try
            {
                // Check if the Authorization header is present
                var authHeader = actionContext.Request.Headers.Authorization;

                if (authHeader == null || authHeader.Scheme != "Bearer")
                {
                    actionContext.Response = actionContext.Request.CreateResponse(
                        HttpStatusCode.Unauthorized,
                        new { message = "Missing or invalid Authorization header." });
                    return;
                }

                // Get the token string
                var token = authHeader.Parameter;

                if (string.IsNullOrEmpty(token))
                {
                    actionContext.Response = actionContext.Request.CreateResponse(
                        HttpStatusCode.Unauthorized,
                        new { message = "Token is missing." });
                    return;
                }

                // Validate the token
                var principal = TokenServices.TokenVerify(token);

                if (principal == null)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(
                        HttpStatusCode.Unauthorized,
                        new { message = "Invalid or expired token." });
                    return;
                }

                // Optionally set the current user principal
                actionContext.RequestContext.Principal = principal;
            }
            catch (Exception ex)
            {
                actionContext.Response = actionContext.Request.CreateResponse(
                    HttpStatusCode.InternalServerError,
                    new { message = "Token validation failed.", details = ex.Message });
            }
        }
    }
}
