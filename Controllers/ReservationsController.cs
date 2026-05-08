using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Models;

namespace Hotel.Controllers;

public class ReservationsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ReservationsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Reservations
    public async Task<IActionResult> Index()
    {
        var reservations = await _context.Reservations
            .Include(r => r.Guest)
            .Include(r => r.Room)
            .ToListAsync();
        return View(reservations);
    }

    // GET: Reservations/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var reservation = await _context.Reservations
            .Include(r => r.Guest)
            .Include(r => r.Room)
            .FirstOrDefaultAsync(m => m.Id == id);
        
        if (reservation == null)
        {
            return NotFound();
        }

        return View(reservation);
    }

    // GET: Reservations/Create
    public IActionResult Create()
    {
        ViewData["GuestId"] = new SelectList(_context.Guests, "Id", "FullName");
        ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "RoomNumber");
        return View();
    }

    // POST: Reservations/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CheckInDate,CheckOutDate,TotalPrice,GuestId,RoomId")] Reservation reservation)
    {
        if (ModelState.IsValid)
        {
            _context.Add(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["GuestId"] = new SelectList(_context.Guests, "Id", "FullName", reservation.GuestId);
        ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "RoomNumber", reservation.RoomId);
        return View(reservation);
    }

    // GET: Reservations/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var reservation = await _context.Reservations.FindAsync(id);
        if (reservation == null)
        {
            return NotFound();
        }

        ViewData["GuestId"] = new SelectList(_context.Guests, "Id", "FullName", reservation.GuestId);
        ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "RoomNumber", reservation.RoomId);
        return View(reservation);
    }

    // POST: Reservations/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,CheckInDate,CheckOutDate,TotalPrice,GuestId,RoomId")] Reservation reservation)
    {
        if (id != reservation.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(reservation);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(reservation.Id))
                {
                    return NotFound();
                }
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["GuestId"] = new SelectList(_context.Guests, "Id", "FullName", reservation.GuestId);
        ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "RoomNumber", reservation.RoomId);
        return View(reservation);
    }

    // GET: Reservations/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var reservation = await _context.Reservations
            .Include(r => r.Guest)
            .Include(r => r.Room)
            .FirstOrDefaultAsync(m => m.Id == id);
        
        if (reservation == null)
        {
            return NotFound();
        }

        return View(reservation);
    }

    // POST: Reservations/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var reservation = await _context.Reservations.FindAsync(id);
        if (reservation != null)
        {
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool ReservationExists(int id)
    {
        return _context.Reservations.Any(e => e.Id == id);
    }
}

