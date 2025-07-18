using Microsoft.AspNetCore.Mvc;
using Nexus.Core.Dtos;
using System.Text.Json;

namespace Nexus.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        
        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid) return View(loginDto);

            var client = _httpClientFactory.CreateClient("NexusApiClient");
            // This send a POST request to the URI "https://localhost:7072/api/auth/login" with the email and password from the LoginDto modela in the request body as Json format
            var response = await client.PostAsJsonAsync("api/auth/login", loginDto);

            if (response.IsSuccessStatusCode)
            {
                // Read AsString waits for the content property to load copmpletely before retrieving data
                var jsonString = await response.Content.ReadAsStringAsync();
                // converts the stringified JSON into C# object
                var tokenObj = JsonDocument.Parse(jsonString);
                // retrieve the access token from the proerty "token"
                var token = tokenObj.RootElement.GetProperty("token").GetString();

                // store the access token retrieved from the "login" endpoint into the session state with the vproperty name "JWToken"
                HttpContext.Session.SetString("JWToken", token);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(loginDto);
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
