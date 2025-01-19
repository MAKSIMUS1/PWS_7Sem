using Lab3_StudentAPI.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Lab3_StudentAPI.Controllers
{
    [ApiController]
    [Route("api/errors")]
    public class ErrorsController : ControllerBase
    {
        [HttpGet("{code}")]
        public IActionResult GetErrorInfo(int code)
        {
            var errorInfo = GetErrorDetails(code);
            if (errorInfo == null)
            {
                return NotFound(new { message = "Error code not found." });
            }

            return Ok(new
            {
                error_code = code,
                error_description = errorInfo.Description,
                links = new List<Link>
            {
                new Link { Href = "/error" + code + ".html", Rel = "self", Method = "GET" },
                new Link { Href = "/documentation.html", Rel = "api_documentation", Method = "GET" }
            }
            });
        }

        private ErrorInfo GetErrorDetails(int code)
        {
            var errorDictionary = new Dictionary<int, ErrorInfo>
        {
            { 404, new ErrorInfo { Description = "Not Found: The resource could not be located." } },
            { 500, new ErrorInfo { Description = "Internal Server Error: Something went wrong on the server." } },
            // Another cods
        };

            errorDictionary.TryGetValue(code, out ErrorInfo errorInfo);
            return errorInfo;
        }

        public class ErrorInfo
        {
            public string Description { get; set; }
        }
    }

}
