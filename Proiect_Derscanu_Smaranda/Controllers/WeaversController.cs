using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HaberdasheryModel.Data;
using HaberdasheryModel.Models;
using Proiect_Derscanu_Smaranda.Models;
using Proiect_Derscanu_Smaranda.Models.HaberdasheryViewModels;

namespace Proiect_Derscanu_Smaranda.Controllers
{
    public class WeaversController : Controller
    {
        private readonly HaberdasheryContext _context;

        public WeaversController(HaberdasheryContext context)
        {
            _context = context;
        }

        // GET: Weavers
        public async Task<IActionResult> Index(int? id, int? fabricID)
        {
            var viewModel = new WeaverIndexData();
            viewModel.Weavers = await _context.Weavers
            .Include(i => i.FabricWeaved)
            .ThenInclude(i => i.Fabric)
            .ThenInclude(i => i.Orders)
            .ThenInclude(i => i.Customer)
            .AsNoTracking()
            .OrderBy(i => i.WeaverName)
            .ToListAsync();
            if (id != null)
            {
                ViewData["WeaverID"] = id.Value;
                Weaver weaver = viewModel.Weavers.Where(
                i => i.ID == id.Value).Single();
                viewModel.Fabrics = weaver.FabricWeaved.Select(s => s.Fabric);
            }
            if (fabricID != null)
            {
                ViewData["FabricID"] = fabricID.Value;
                viewModel.Orders = viewModel.Fabrics.Where(
                x => x.ID == fabricID).Single().Orders;
            }
            return View(viewModel);
        }

        // GET: Weavers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weaver = await _context.Weavers
                .FirstOrDefaultAsync(m => m.ID == id);
            if (weaver == null)
            {
                return NotFound();
            }

            return View(weaver);
        }

        // GET: Weavers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Weavers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,WeaverName,Specialty")] Weaver weaver)
        {
            if (ModelState.IsValid)
            {
                _context.Add(weaver);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(weaver);
        }

        // GET: Weavers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var weaver = await _context.Weavers
            .Include(i => i.FabricWeaved).ThenInclude(i => i.Fabric)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id);
            if (weaver == null)
            {
                return NotFound();
            }
            PopulateFabricWeavedData(weaver);
            return View(weaver);

        }
        private void PopulateFabricWeavedData(Weaver weaver)
        {
            var allFabrics = _context.Fabrics;
            var weaverFabrics = new HashSet<int>(weaver.FabricWeaved.Select(c => c.FabricID));
            var viewModel = new List<FabricWeavedData>();
            foreach (var fabric in allFabrics)
            {
                viewModel.Add(new FabricWeavedData
                {
                    FabricID = fabric.ID,
                    Type = fabric.Type,
                    IsWeaved = weaverFabrics.Contains(fabric.ID)
                });
            }
            ViewData["Fabrics"] = viewModel;
        }

        // POST: Weavers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedFabrics)
        {
            if (id == null)
            {
                return NotFound();
            }
            var weaverToUpdate = await _context.Weavers
            .Include(i => i.FabricWeaved)
            .ThenInclude(i => i.Fabric)
            .FirstOrDefaultAsync(m => m.ID == id);
            if (await TryUpdateModelAsync<Weaver>(
            weaverToUpdate,
            "",
            i => i.WeaverName, i => i.Specialty))
            {
                UpdateWeavedFabrics(selectedFabrics, weaverToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {

                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, ");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdateWeavedFabrics(selectedFabrics, weaverToUpdate);
            PopulateFabricWeavedData(weaverToUpdate);
            return View(weaverToUpdate);
        }
        private void UpdateWeavedFabrics(string[] selectedFabrics, Weaver weaverToUpdate)
        {
            if (selectedFabrics == null)
            {
                weaverToUpdate.FabricWeaved = new List<FabricWeaved>();
                return;
            }
            var selectedFabricsHS = new HashSet<string>(selectedFabrics);
            var weavedFabrics = new HashSet<int>
            (weaverToUpdate.FabricWeaved.Select(c => c.Fabric.ID));
            foreach (var fabric in _context.Fabrics)
            {
                if (selectedFabricsHS.Contains(fabric.ID.ToString()))
                {
                    if (!weavedFabrics.Contains(fabric.ID))
                    {
                        weaverToUpdate.FabricWeaved.Add(new FabricWeaved
                        {
                            WeaverID =
                       weaverToUpdate.ID,
                            FabricID = fabric.ID
                        });
                    }
                }
                else
                {
                    if (weavedFabrics.Contains(fabric.ID))
                    {
                        FabricWeaved fabricToRemove = weaverToUpdate.FabricWeaved.FirstOrDefault(i
                       => i.FabricID == fabric.ID);
                        _context.Remove(fabricToRemove);
                    }
                }
            }
        }

        // GET: Weavers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weaver = await _context.Weavers
                .FirstOrDefaultAsync(m => m.ID == id);
            if (weaver == null)
            {
                return NotFound();
            }

            return View(weaver);
        }

        // POST: Weavers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var weaver = await _context.Weavers.FindAsync(id);
            _context.Weavers.Remove(weaver);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WeaverExists(int id)
        {
            return _context.Weavers.Any(e => e.ID == id);
        }
    }
}
