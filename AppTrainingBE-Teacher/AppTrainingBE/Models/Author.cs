﻿namespace AppTrainingBETeacher.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }

        public ICollection<AuthorBook> AuthorBooks { get; set; }
    }
}
