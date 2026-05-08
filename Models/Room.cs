using System.ComponentModel.DataAnnotations;

namespace Hotel.Models;

public class Room
{
    public int Id { get; set; }

    [Required]
    [MaxLength(10)]
    public string RoomNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Type { get; set; } = string.Empty; // Single, Double, Suite

    public decimal PricePerNight { get; set; }

    public int Capacity { get; set; }

    public bool IsAvailable { get; set; } = true;

    // 1 Room → Many Reservations
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    // Many Rooms ↔ Many Amenities
    public ICollection<Amenity> Amenities { get; set; } = new List<Amenity>();
}
