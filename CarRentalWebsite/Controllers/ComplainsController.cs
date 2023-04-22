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
    public class ComplainsController : Controller
    {

        private readonly DBContext _context;

        public ComplainsController(DBContext context)
        {
            _context = context;
        }

        // GET: Complains
        [Authorize(Roles = "Admin, Client")]
        public async Task<IActionResult> List()
        {
              return _context.Complains != null ? 
                          View(await _context.Complains.ToListAsync()) :
                          Problem("Entity set 'DBContext.Complains'  is null.");
        }



        // GET: Complains/New
        [Authorize(Roles = "Admin, Client")]
        public IActionResult New()
        {
            var UserName  = User.Identity.Name.ToString();

            ViewData["KitsByUser"] = new SelectList(_context.Vehicles.Where(p => p.Owner == UserName && p.Validity == true), "Kit_Nr", "Kit_Nr");
            return View();
        }

        // POST: Complains/New
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New([Bind("Id,Owner,Kit_Number,Message,Message_Date")] Complain complain)
        {
            if (ModelState.IsValid)
            {
                _context.Add(complain);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(complain);
        }

        // GET: Complains/Edit/5
        [Authorize(Roles = "Admin, Client")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Complains == null)
            {
                return NotFound();
            }

            var complain = await _context.Complains.FindAsync(id);
            if (complain == null)
            {
                return NotFound();
            }
            return View(complain);
        }

        // POST: Complains/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Client")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Owner,Kit_Number,Message,Message_Date")] Complain complain)
        {
            if (id != complain.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(complain);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComplainExists(complain.Id))
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
            return View(complain);
        }

        // GET: Complains/Delete/5
        [Authorize(Roles = "Admin, Client")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Complains == null)
            {
                return NotFound();
            }

            var complain = await _context.Complains
                .FirstOrDefaultAsync(m => m.Id == id);
            if (complain == null)
            {
                return NotFound();
            }

            return View(complain);
        }

        // POST: Complains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Complains == null)
            {
                return Problem("Entity set 'DBContext.Complains'  is null.");
            }
            var complain = await _context.Complains.FindAsync(id);
            if (complain != null)
            {
                _context.Complains.Remove(complain);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComplainExists(int id)
        {
          return (_context.Complains?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
