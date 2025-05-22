namespace AppTrainingBETeacher.DTOs
{
    public class StudentDto
    {
        public string FullName { get; set; } = null!;
        public List<int> CourseIds { get; set; } = new();
    }
}
