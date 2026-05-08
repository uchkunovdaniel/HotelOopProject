using System.ComponentModel.DataAnnotations;

namespace Hotel.Models;

public class Guest
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    // 1 Guest → Many Reservations
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
