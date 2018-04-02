using Microsoft.AspNetCore.Mvc;

namespace KernelAPI.Controllers
{
    [Produces("application/json")]
    [Route("")]
    public class DefaultController : Controller
    {
        
        [HttpGet]
        public string Get()
        {
            return "Hello KernelApi! Latest tag test v1";
        }
       
    }
}