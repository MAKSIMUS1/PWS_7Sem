namespace Lab3_StudentAPI.DTO
{
    public class StudentsResponseDto
    {
        public int total { get; set; }
        public List<StudentDto>? Students { get; set; }
        public List<Link> Links { get; set; } = new List<Link>();
    }
}
