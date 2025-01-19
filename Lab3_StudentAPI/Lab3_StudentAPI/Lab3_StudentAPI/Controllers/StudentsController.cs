using Lab3_StudentAPI.Data;
using Lab3_StudentAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab3_StudentAPI.Models;

[Route("api/[controller].{format::regex(json|xml)}")]
[ApiController]
public class StudentsController : ControllerBase
{
    private readonly StudentsContext _context;
    private readonly ILinkService _linkService;
    public StudentsController(StudentsContext context, ILinkService linkService)
    {
        _context = context;
        _linkService = linkService;
    }

    // GET: api/students.{format}
    [HttpGet]
    public IActionResult GetStudents([FromQuery] int? limit = null,
                                     [FromQuery] int? offset = null,
                                     [FromQuery] string sort = null,
                                     [FromQuery] int? minid = null,
                                     [FromQuery] int? maxid = null,
                                     [FromQuery] string like = null,
                                     [FromQuery] string columns = null,
                                     [FromQuery] string globalike = null,
                                     [FromRoute] string format = "json")
    {
        var query = _context.Students.AsQueryable();

        if (minid.HasValue)
            query = query.Where(s => s.ID >= minid.Value);

        if (maxid.HasValue)
            query = query.Where(s => s.ID <= maxid.Value);

        if (!string.IsNullOrEmpty(like))
            query = query.Where(s => EF.Functions.Like(s.Name, $"%{like}%"));

        if (!string.IsNullOrEmpty(globalike))
            query = query.Where(s => EF.Functions.Like(s.ID.ToString() + s.Name + s.Phone, $"%{globalike}%"));

        var totalStudents = query.Count();

        if (!string.IsNullOrEmpty(sort))
            query = query.OrderBy(s => s.Name);
        else
            query = query.OrderBy(s => s.ID);

        limit = limit.HasValue && limit > 0 ? limit : 10;
        offset = offset.HasValue && offset >= 0 ? offset : 0;  

        query = query.Skip(offset.Value).Take(limit.Value);

        var students = query.ToList();

        var result = students.Select(student =>
        {
            var studentDto = new StudentDto
            {
                ID = student.ID,
                Name = student.Name,
                Phone = student.Phone
            };

            studentDto.Links = _linkService.GetLinksForStudent(HttpContext, student.ID);

            if (!string.IsNullOrEmpty(columns))
            {
                var selectedFields = columns.Split(",").Select(c => c.Trim().ToLower()).ToArray();
                if (!selectedFields.Contains("id")) studentDto.ID = default;
                if (!selectedFields.Contains("name")) studentDto.Name = null;
                if (!selectedFields.Contains("phone")) studentDto.Phone = null;
            }

            return studentDto;
        }).ToList();

        var response = new StudentsResponseDto
        {
            total = totalStudents,
            Students = result,
            Links = new List<Link>()
        };

        var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var totalPages = (int)Math.Ceiling(totalStudents / (double)limit.Value);

        for (int page = 1; page <= totalPages; page++)
        {
            var pageOffset = (page - 1) * limit;
            var pageLink = $"{baseUrl}?limit={limit}&offset={pageOffset}&sort={sort}&minid={minid}&maxid={maxid}&like={like}&columns={columns}&globalike={globalike}&format={format}";
            response.Links.Add(new Link { Href = pageLink, Rel = page.ToString(), Method = "GET" });
        }

        if (format.ToLower() == "xml")
        {
            var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(StudentsResponseDto));
            using (var stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, response);
                return Content(stringWriter.ToString(), "application/xml");
            }
        }
        return Ok(response);
    }

    // GET: api/students.{format}/{id}
    [HttpGet("{id}")]
    public IActionResult GetStudent(int id, [FromRoute] string format = "json")
    {
        var student = _context.Students.Find(id);
        if (student == null)
        {
            var errorLinks = new List<Link>
        {
            new Link
            {
                Href = "http://localhost:5232/api/errors/404",
                Rel = "error_info",
                Method = "GET"
            }
        };

            return NotFound(new
            {
                message = "Student not found",
                links = errorLinks
            });
        }

        var studentDto = new StudentDto
        {
            ID = student.ID,
            Name = student.Name,
            Phone = student.Phone,
            Links = _linkService.GetLinksForStudent(HttpContext, student.ID)
        };

        if (format.ToLower() == "xml")
        {
            return new ObjectResult(studentDto)
            {
                ContentTypes = { "application/xml" }
            };
        }

        return Ok(studentDto);
    }

    // POST: api/students.{format}
    [HttpPost]
    public IActionResult CreateStudent([FromBody] StudentDto studentDto, [FromRoute] string format = "json")
    {
        if (studentDto == null)
        {
            return BadRequest(new
            {
                Message = "Invalid data",
                Links = _linkService.GetLinksForStudents(HttpContext)
            });
        }

        var student = new Student
        {
            Name = studentDto.Name,
            Phone = studentDto.Phone
        };

        _context.Students.Add(student);
        _context.SaveChanges();

        studentDto.ID = student.ID;
        studentDto.Links = _linkService.GetLinksForStudent(HttpContext, student.ID);

        if (format.ToLower() == "xml")
        {
            return new ObjectResult(studentDto)
            {
                ContentTypes = { "application/xml" }
            };
        }

        return CreatedAtAction(nameof(GetStudent), new { id = student.ID, format }, studentDto);
    }

    // PUT: api/students.{format}/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateStudent(int id, [FromBody] StudentDto studentDto, [FromRoute] string format = "json")
    {
        if (studentDto == null || id != studentDto.ID)
        {
            return BadRequest(new
            {
                Message = "Invalid data",
                Links = _linkService.GetLinksForStudents(HttpContext)
            });
        }

        var student = _context.Students.Find(id);
        if (student == null)
        {
            return NotFound(new
            {
                Message = "Student not found",
                Links = _linkService.GetLinksForStudents(HttpContext)
            });
        }

        student.Name = studentDto.Name;
        student.Phone = studentDto.Phone;
        _context.Students.Update(student);
        _context.SaveChanges();

        studentDto.Links = _linkService.GetLinksForStudent(HttpContext, student.ID);

        if (format.ToLower() == "xml")
        {
            return new ObjectResult(studentDto)
            {
                ContentTypes = { "application/xml" }
            };
        }

        return Ok(studentDto);
    }

    // DELETE: api/students.{format}/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteStudent(int id, [FromRoute] string format = "json")
    {
        var student = _context.Students.Find(id);
        if (student == null)
        {
            return NotFound(new
            {
                Message = "Student not found",
                Links = _linkService.GetLinksForStudents(HttpContext)
            });
        }

        _context.Students.Remove(student);
        _context.SaveChanges();

        if (format.ToLower() == "xml")
        {
            return NoContent();
        }

        return NoContent();
    }
}
