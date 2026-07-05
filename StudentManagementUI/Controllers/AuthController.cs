using Microsoft.AspNetCore.Mvc;
using StudentManagementUI.Models;
using System.Net.Http.Json;

namespace StudentManagementUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _httpClient;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44314/api/");
        }

        // ================= LOGIN =================

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _httpClient.PostAsJsonAsync("Auth/Login", model);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Invalid Email or Password";
                return View(model);
            }

            var result = await response.Content.ReadFromJsonAsync<Response>();

            if (result == null || string.IsNullOrEmpty(result.Token))
            {
                ViewBag.Error = "Token not received";
                return View(model);
            }

            HttpContext.Session.SetString("JWToken", result.Token);

            return RedirectToAction("Index", "Student");
        }

        // ================= REGISTER =================

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Register model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _httpClient.PostAsJsonAsync("Auth/Register", model);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Registration Successful";
                return RedirectToAction("Login");
            }

            var error = await response.Content.ReadAsStringAsync();

            ViewBag.Error = error;

            return View(model);
        }

        // ================= LOGOUT =================

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}