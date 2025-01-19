namespace Lab3_StudentAPI.DTO
{
    public class StudentDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public List<Link> Links { get; set; } = new List<Link>();
    }
}
