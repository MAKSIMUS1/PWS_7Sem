using Lab3_StudentAPI.DTO;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

public interface ILinkService
{
    List<Link> GetLinksForStudent(HttpContext httpContext, int id);
    List<Link> GetLinksForStudents(HttpContext httpContext);
}
