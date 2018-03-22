using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace KernelAPI.Controllers
{
    [Produces("application/json")]
    [Route("")]
    public class DefaultController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(Directory.GetCurrentDirectory());
        }
    }
}