using System.ComponentModel.DataAnnotations;

namespace Hotel.Models;

public class Reservation
{
    public int Id { get; set; }

    public DateTime CheckInDate { get; set; }

    public DateTime CheckOutDate { get; set; }

    public decimal TotalPrice { get; set; }

    // Foreign keys
    public int GuestId { get; set; }
    public Guest Guest { get; set; } = null!;

    public int RoomId { get; set; }
    public Room Room { get; set; } = null!;
}
