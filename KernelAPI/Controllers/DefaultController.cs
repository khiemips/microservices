using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace KernelAPI.Controllers
{
    [Produces("application/json")]
    [Route("")]
    public class DefaultController : Controller
    {
        
        [HttpGet]
        public string Get()
        {
            return "Hello KernelApi!";
        }
       
    }
}