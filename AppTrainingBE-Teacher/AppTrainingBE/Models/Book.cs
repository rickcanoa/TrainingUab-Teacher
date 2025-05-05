namespace AppTrainingBETeacher.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }

        public ICollection<AuthorBook> AuthorBooks { get; set; }
    }
}
