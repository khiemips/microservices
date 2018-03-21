using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace KernelAPI.Attributes
{
    public class GeneralExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            // Log
            context.Result = new JsonResult(new { context.Exception.Message, context.Exception.StackTrace });
            base.OnException(context);
        }
    }
}
