using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HaberdasheryModel.Data;
using HaberdasheryModel.Models;

namespace Proiect_Derscanu_Smaranda.Controllers
{
    public class FabricsController : Controller
    {
        private readonly HaberdasheryContext _context;

        public FabricsController(HaberdasheryContext context)
        {
            _context = context;
        }

        // GET: Fabrics
        public async Task<IActionResult> Index( string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TypeSortParm"] = String.IsNullOrEmpty(sortOrder) ? "type_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var fabrics = from b in _context.Fabrics
                        select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                fabrics = fabrics.Where(s => s.Type.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "type_desc":
                    fabrics = fabrics.OrderByDescending(b => b.Type);
                    break;
                case "Price":
                    fabrics = fabrics.OrderBy(b => b.Price);
                    break;
                case "price_desc":
                    fabrics = fabrics.OrderByDescending(b => b.Price);
                    break;
                default:
                    fabrics = fabrics.OrderBy(b => b.Type);
                    break;
            }
            int pageSize = 2;
            return View(await PaginatedList<Fabric>.CreateAsync(fabrics.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Fabrics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fabric = await _context.Fabrics
            .Include(s => s.Orders)
            .ThenInclude(e => e.Customer)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id);
            if (fabric == null)
            {
                return NotFound();
            }

            return View(fabric);
        }

        // GET: Fabrics/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fabrics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Type,Color,Price")] Fabric fabric)
        {
            try
            {
                if (ModelState.IsValid)
                {
                _context.Add(fabric);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex*/)
            {
                ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists ");
            }

            return View(fabric);
        }

        // GET: Fabrics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fabric = await _context.Fabrics.FindAsync(id);
            if (fabric == null)
            {
                return NotFound();
            }
            return View(fabric);
        }

        // POST: Fabrics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var fabricToUpdate = await _context.Fabrics.FirstOrDefaultAsync(s => s.ID == id);
            if (await TryUpdateModelAsync<Fabric>(
            fabricToUpdate,
            "",
            s => s.Color, s => s.Type, s => s.Price))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists");
                }
            }
            return View(fabricToUpdate);
        }

        // GET: Fabrics/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fabric = await _context.Fabrics
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (fabric == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                "Delete failed. Try again";
            }

            return View(fabric);
        }

        // POST: Fabrics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fabric = await _context.Fabrics.FindAsync(id);
            if (fabric == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
            _context.Fabrics.Remove(fabric);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {

                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool FabricExists(int id)
        {
            return _context.Fabrics.Any(e => e.ID == id);
        }
    }
}
