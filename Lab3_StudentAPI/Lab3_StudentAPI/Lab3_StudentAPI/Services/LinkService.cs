using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Lab3_StudentAPI.DTO;

namespace Lab3_StudentAPI.Services
{
    public class LinkService : ILinkService
    {
        private readonly LinkGenerator _linkGenerator;

        public LinkService(LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
        }

        public List<Link> GetLinksForStudent(HttpContext httpContext, int id)
        {
            var format = httpContext.GetRouteValue("format")?.ToString();

            var links = new List<Link>
    {
        new Link(_linkGenerator.GetUriByAction(httpContext, "GetStudent", values: new { id, format }), "self", "GET"),
        new Link(_linkGenerator.GetUriByAction(httpContext, "UpdateStudent", values: new { id, format }), "update_student", "PUT"),
        new Link(_linkGenerator.GetUriByAction(httpContext, "DeleteStudent", values: new { id, format }), "delete_student", "DELETE")
    };

            return links;
        }

        public List<Link> GetLinksForStudents(HttpContext httpContext)
        {
            return new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(httpContext, "GetStudents"), "self", "GET"),
                new Link(_linkGenerator.GetUriByAction(httpContext, "CreateStudent"), "create_student", "POST")
            };
        }
    }
}
