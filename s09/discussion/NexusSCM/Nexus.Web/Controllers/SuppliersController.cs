using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Nexus.Core;
using Nexus.Web.Data;
using System.Text.Json;

namespace Nexus.Web.Controllers
{
    public class SuppliersController : Controller
        
    {
        //private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        //public SuppliersController (ApplicationDbContext context)
        //{
        //    _context = context;
        //}
        public SuppliersController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            //var suppliers = await _context.Suppliers.ToListAsync();
            //return View(suppliers);
            var client = _httpClientFactory.CreateClient("NexusApiClient");
            var response = await client.GetAsync("api/suppliers");

            List<Supplier> suppliers = new();
            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                suppliers = await JsonSerializer.DeserializeAsync<List<Supplier>>(responseStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            return View(suppliers);
        }

        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id==null)
        //    {
        //        return NotFound();
        //    }
        //    var supplier = await _context.Suppliers.FirstOrDefaultAsync(m=>m.Id == id);
        //    if (supplier == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(supplier);
        //}

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Create(Supplier model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    _context.Suppliers.Add(model);
        //    _context.SaveChanges();

        //    return RedirectToAction("Index");
        //}


        [HttpPost]
        [ValidateAntiForgeryToken] // Prevents cross-site request forgery attacks
        public async Task<IActionResult> Create([Bind("Name,ContactPerson,Email")] Supplier supplier)
        {
            /*if (ModelState.IsValid)
            {
                _context.Add(supplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }*/

            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient("NexusApiClient");
                // retrieve the access token from login
                var token = HttpContext.Session.GetString("JWToken");
                Console.WriteLine(token);
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                // Sends a POST request at the URI "https://localhost:7072/api/suppliers" with the supplier model in the request boday as Json format and the bearer token in the autorization header
                var response = await client.PostAsJsonAsync("api/suppliers", supplier);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                } else
                {
                    ModelState.AddModelError(string.Empty, "Failed to create supplier. API returned an error.");
                }

            }

            return View(supplier);
        }


    }
}
