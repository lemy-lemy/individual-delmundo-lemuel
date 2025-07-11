using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nexus.Core;
using Nexus.Web.Data;

namespace Nexus.Web.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuppliersController (ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var suppliers = await _context.Suppliers.ToListAsync();
            return View(suppliers);        
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var supplier = await _context.Suppliers.FirstOrDefaultAsync(m=>m.Id == id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Supplier model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _context.Suppliers.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
