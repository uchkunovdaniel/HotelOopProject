using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Models;

namespace Hotel.Controllers;

public class AmenitiesController : Controller
{
    private readonly ApplicationDbContext _context;

    public AmenitiesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Amenities
    public async Task<IActionResult> Index()
    {
        return View(await _context.Amenities.ToListAsync());
    }

    // GET: Amenities/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var amenity = await _context.Amenities
            .Include(a => a.Rooms)
            .FirstOrDefaultAsync(m => m.Id == id);
        
        if (amenity == null)
        {
            return NotFound();
        }

        return View(amenity);
    }

    // GET: Amenities/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Amenities/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Description")] Amenity amenity)
    {
        if (ModelState.IsValid)
        {
            _context.Add(amenity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(amenity);
    }

    // GET: Amenities/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var amenity = await _context.Amenities.FindAsync(id);
        if (amenity == null)
        {
            return NotFound();
        }

        return View(amenity);
    }

    // POST: Amenities/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Amenity amenity)
    {
        if (id != amenity.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(amenity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AmenityExists(amenity.Id))
                {
                    return NotFound();
                }
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(amenity);
    }

    // GET: Amenities/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var amenity = await _context.Amenities.FirstOrDefaultAsync(m => m.Id == id);
        
        if (amenity == null)
        {
            return NotFound();
        }

        return View(amenity);
    }

    // POST: Amenities/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var amenity = await _context.Amenities.FindAsync(id);
        if (amenity != null)
        {
            _context.Amenities.Remove(amenity);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool AmenityExists(int id)
    {
        return _context.Amenities.Any(e => e.Id == id);
    }
}

