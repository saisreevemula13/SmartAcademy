namespace Application.DTO
{
    public class StudentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<CourseDTO> Courses { get; set; } = new();
        // You can add Role or CreatedAt if needed
    }
}
