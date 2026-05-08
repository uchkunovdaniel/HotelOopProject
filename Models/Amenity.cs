using System.ComponentModel.DataAnnotations;

namespace Hotel.Models;

public class Amenity
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(250)]
    public string Description { get; set; } = string.Empty;

    // Many Amenities ↔ Many Rooms
    public ICollection<Room> Rooms { get; set; } = new List<Room>();
}
