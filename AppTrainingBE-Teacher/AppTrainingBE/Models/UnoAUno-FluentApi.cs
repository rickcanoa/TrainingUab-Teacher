namespace AppTrainingBETeacher.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;

        // Propiedad de navegación
        public UserProfile Profile { get; set; } = null!;
    }

    public class UserProfile
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }

        // Clave foránea y navegación inversa
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
