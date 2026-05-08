using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Models;

namespace Hotel.Controllers;

public class RoomsController : Controller
{
    private readonly ApplicationDbContext _context;

    public RoomsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Rooms
    public async Task<IActionResult> Index()
    {
        return View(await _context.Rooms.Include(r => r.Amenities).ToListAsync());
    }

    // GET: Rooms/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var room = await _context.Rooms
            .Include(r => r.Amenities)
            .Include(r => r.Reservations)
            .FirstOrDefaultAsync(m => m.Id == id);
        
        if (room == null)
        {
            return NotFound();
        }

        return View(room);
    }

    // GET: Rooms/Create
    public IActionResult Create()
    {
        ViewData["Amenities"] = _context.Amenities.ToList();
        return View();
    }

    // POST: Rooms/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("RoomNumber,Type,PricePerNight,Capacity,IsAvailable")] Room room, int[] amenityIds)
    {
        if (ModelState.IsValid)
        {
            _context.Add(room);
            await _context.SaveChangesAsync();

            // Add selected amenities
            if (amenityIds.Length > 0)
            {
                foreach (var amenityId in amenityIds)
                {
                    var amenity = await _context.Amenities.FindAsync(amenityId);
                    if (amenity != null)
                    {
                        room.Amenities.Add(amenity);
                    }
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["Amenities"] = _context.Amenities.ToList();
        return View(room);
    }

    // GET: Rooms/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var room = await _context.Rooms.Include(r => r.Amenities).FirstOrDefaultAsync(r => r.Id == id);
        if (room == null)
        {
            return NotFound();
        }

        ViewData["Amenities"] = _context.Amenities.ToList();
        ViewData["SelectedAmenities"] = room.Amenities.Select(a => a.Id).ToList();
        return View(room);
    }

    // POST: Rooms/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,RoomNumber,Type,PricePerNight,Capacity,IsAvailable")] Room room, int[] amenityIds)
    {
        if (id != room.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var existingRoom = await _context.Rooms.Include(r => r.Amenities).FirstOrDefaultAsync(r => r.Id == id);
                if (existingRoom == null)
                {
                    return NotFound();
                }

                existingRoom.RoomNumber = room.RoomNumber;
                existingRoom.Type = room.Type;
                existingRoom.PricePerNight = room.PricePerNight;
                existingRoom.Capacity = room.Capacity;
                existingRoom.IsAvailable = room.IsAvailable;

                // Update amenities
                existingRoom.Amenities.Clear();
                foreach (var amenityId in amenityIds)
                {
                    var amenity = await _context.Amenities.FindAsync(amenityId);
                    if (amenity != null)
                    {
                        existingRoom.Amenities.Add(amenity);
                    }
                }

                _context.Update(existingRoom);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(room.Id))
                {
                    return NotFound();
                }
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["Amenities"] = _context.Amenities.ToList();
        ViewData["SelectedAmenities"] = amenityIds.ToList();
        return View(room);
    }

    // GET: Rooms/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var room = await _context.Rooms
            .Include(r => r.Amenities)
            .FirstOrDefaultAsync(m => m.Id == id);
        
        if (room == null)
        {
            return NotFound();
        }

        return View(room);
    }

    // POST: Rooms/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room != null)
        {
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool RoomExists(int id)
    {
        return _context.Rooms.Any(e => e.Id == id);
    }
}

