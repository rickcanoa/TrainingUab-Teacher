using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = null!;

    // Relación 1:1
    public UserProfile Profile { get; set; } = null!;
}

public class UserProfile
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string FullName { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    // Clave foránea + relación inversa
    [ForeignKey("User")]
    public int UserId { get; set; }

    public User? User { get; set; }
}
