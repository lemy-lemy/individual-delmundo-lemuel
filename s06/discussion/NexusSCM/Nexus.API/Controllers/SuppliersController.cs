using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nexus.API.Data;
using Nexus.Core;

namespace Nexus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SuppliersController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetSuppliers()
        {
            //throw new Exception("This is a deliberate test exception!");
            return await _context.Suppliers.ToListAsync();
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<Supplier>> GetSupplier(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return Ok(supplier);
        }


        [HttpPost]
        public async Task<ActionResult<IEnumerable<Supplier>>> PostSupplier(Supplier supplier)
        {
            if (supplier == null)
            {
                return BadRequest();
            }

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(PostSupplier), new { id = supplier.Id }, supplier);

        }
    }
}
