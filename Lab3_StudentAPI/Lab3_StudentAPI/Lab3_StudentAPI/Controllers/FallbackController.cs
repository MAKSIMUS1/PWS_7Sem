using Microsoft.AspNetCore.Mvc;

namespace Lab3_StudentAPI.Controllers
{
    [ApiController]
    public class FallbackController : ControllerBase
    {
        [Route("{*url}", Order = 999)]
        public IActionResult HandleFallback()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documentation.html"), "text/html");
        }
    }
}
