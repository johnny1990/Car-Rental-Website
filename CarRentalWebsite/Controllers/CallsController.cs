using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarRentalWebsite.Database;
using CarRentalWebsite.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace CarRentalWebsite.Controllers
{
    [Authorize(Roles = "Admin, Client")]
    public class CallsController : Controller
    {
        private readonly DBContext _context;

        public CallsController(DBContext context)
        {
            _context = context;
        }

        // GET: Calls
        [HttpGet]
        public async Task<IActionResult> List()
        {
            return _context.Calls != null ?
                        View(await _context.Calls.ToListAsync()) :
                        Problem("Entity set 'DBContext.Calls'  is null.");
        }

        // GET: Calls/New
        public IActionResult New()
        {
            var UserName = User.Identity.Name.ToString();

            ViewData["KitsByUser"] = new SelectList(_context.Vehicles.Where(p => p.Owner == UserName && p.Validity == true), "Kit_Nr", "Kit_Nr");

            return View();
        }

        // POST: Calls/New
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New([Bind("Id,OwnerName,Kit_Number,Phone_Number,Date_And_Time,Call_Duration")] Call call)
        {
            if (ModelState.IsValid)
            {
                _context.Add(call);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(call);
        }

        // GET: Calls/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Calls == null)
            {
                return NotFound();
            }

            var call = await _context.Calls.FindAsync(id);
            if (call == null)
            {
                return NotFound();
            }
            return View(call);
        }

        // POST: Calls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OwnerName,Kit_Number,Phone_Number,Date_And_Time,Call_Duration")] Call call)
        {
            if (id != call.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(call);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CallExists(call.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(call);
        }

        // GET: Calls/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Calls == null)
            {
                return NotFound();
            }

            var call = await _context.Calls
                .FirstOrDefaultAsync(m => m.Id == id);
            if (call == null)
            {
                return NotFound();
            }

            return View(call);
        }

        // POST: Calls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Calls == null)
            {
                return Problem("Entity set 'DBContext.Calls'  is null.");
            }
            var call = await _context.Calls.FindAsync(id);
            if (call != null)
            {
                _context.Calls.Remove(call);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CallExists(int id)
        {
          return (_context.Calls?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
