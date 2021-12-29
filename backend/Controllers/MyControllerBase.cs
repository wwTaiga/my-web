using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MyWeb.Controllers
{
    public abstract class MyControllerBase : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        public ActionResult UnprocessableEntity(IdentityResult result)
        {
            Dictionary<string, string> errorDescs = new();
            foreach (IdentityError error in result.Errors)
            {
                errorDescs.Add(error.Code, error.Description);
            }
            return UnprocessableEntity(
                new { traceId = HttpContext.TraceIdentifier, errors = errorDescs });
        }
    }
}
