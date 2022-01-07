using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MyWeb.Controllers
{
    public abstract class MyControllerBase : ControllerBase
    {
        /// <summary>
        /// Convert <c>IdentityResult</c> to a json object and produce 422 response. 
        /// </summary>
        /// <param name="result">Identity result</param>
        /// <returns>
        /// UnprocessableEntity (422) response with error description.
        /// </returns>
        [NonAction]
        public UnprocessableEntityObjectResult UnprocessableEntity(IdentityResult result)
        {
            Dictionary<string, string> errorDescs = new();
            foreach (IdentityError error in result.Errors)
            {
                errorDescs.Add(error.Code, error.Description);
            }
            return UnprocessableEntity(
                new { traceId = HttpContext.TraceIdentifier, errors = errorDescs });
        }

        /// <summary>
        /// Produce 422 response. 
        /// </summary>
        /// <param name="errors">Data need to send</param>
        /// <returns>
        /// UnprocessableEntity (422) response with error description.
        /// </returns>
        [NonAction]
        public override UnprocessableEntityObjectResult UnprocessableEntity(Object errors)
        {
            return UnprocessableEntity(
                new { traceId = HttpContext.TraceIdentifier, errors = errors });
        }
    }
}
