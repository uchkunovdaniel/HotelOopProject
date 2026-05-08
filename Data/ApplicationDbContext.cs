using Hotel.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Guest> Guests { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Amenity> Amenities { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Many-to-many: Room ↔ Amenity
        builder.Entity<Room>()
            .HasMany(r => r.Amenities)
            .WithMany(a => a.Rooms)
            .UsingEntity(j => j.ToTable("RoomAmenities"));

        builder.Entity<Room>()
            .Property(r => r.PricePerNight)
            .HasPrecision(18, 2);

        builder.Entity<Reservation>()
            .Property(r => r.TotalPrice)
            .HasPrecision(18, 2);

        // 1-to-many: Guest → Reservations
        builder.Entity<Reservation>()
            .HasOne(r => r.Guest)
            .WithMany(g => g.Reservations)
            .HasForeignKey(r => r.GuestId);

        // 1-to-many: Room → Reservations
        builder.Entity<Reservation>()
            .HasOne(r => r.Room)
            .WithMany(rm => rm.Reservations)
            .HasForeignKey(r => r.RoomId);

        // Seed Amenities
        builder.Entity<Amenity>().HasData(
            new Amenity { Id = 1, Name = "Wi-Fi", Description = "Безплатен безжичен интернет" },
            new Amenity { Id = 2, Name = "Климатик", Description = "Климатизация в стаята" },
            new Amenity { Id = 3, Name = "Мини бар", Description = "Мини бар с напитки" },
            new Amenity { Id = 4, Name = "Сейф", Description = "Сейф за ценности" }
        );

        // Seed Rooms
        builder.Entity<Room>().HasData(
            new Room { Id = 1, RoomNumber = "101", Type = "Единична", PricePerNight = 80m, Capacity = 1, IsAvailable = true },
            new Room { Id = 2, RoomNumber = "202", Type = "Двойна", PricePerNight = 120m, Capacity = 2, IsAvailable = true },
            new Room { Id = 3, RoomNumber = "303", Type = "Апартамент", PricePerNight = 250m, Capacity = 4, IsAvailable = false }
        );

        // Seed many-to-many join table
        builder.Entity("AmenityRoom").HasData(
            new { AmenitiesId = 1, RoomsId = 1 },
            new { AmenitiesId = 2, RoomsId = 1 },
            new { AmenitiesId = 1, RoomsId = 2 },
            new { AmenitiesId = 2, RoomsId = 2 },
            new { AmenitiesId = 3, RoomsId = 2 },
            new { AmenitiesId = 1, RoomsId = 3 },
            new { AmenitiesId = 2, RoomsId = 3 },
            new { AmenitiesId = 3, RoomsId = 3 },
            new { AmenitiesId = 4, RoomsId = 3 }
        );

        // Seed Guests
        builder.Entity<Guest>().HasData(
            new Guest { Id = 1, FullName = "Иван Петров", Email = "ivan@example.com", Phone = "+359888111222" },
            new Guest { Id = 2, FullName = "Мария Иванова", Email = "maria@example.com", Phone = "+359888333444" }
        );

        // Seed Reservations
        builder.Entity<Reservation>().HasData(
            new Reservation { Id = 1, CheckInDate = new DateTime(2026, 6, 1), CheckOutDate = new DateTime(2026, 6, 5), TotalPrice = 320m, GuestId = 1, RoomId = 1 },
            new Reservation { Id = 2, CheckInDate = new DateTime(2026, 7, 10), CheckOutDate = new DateTime(2026, 7, 15), TotalPrice = 600m, GuestId = 2, RoomId = 2 },
            new Reservation { Id = 3, CheckInDate = new DateTime(2026, 8, 1), CheckOutDate = new DateTime(2026, 8, 3), TotalPrice = 500m, GuestId = 1, RoomId = 3 }
        );
    }
}
