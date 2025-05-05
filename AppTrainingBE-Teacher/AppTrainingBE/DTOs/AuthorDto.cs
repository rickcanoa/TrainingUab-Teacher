namespace AppTrainingBETeacher.DTOs
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> BookIds { get; set; }
    }

}
