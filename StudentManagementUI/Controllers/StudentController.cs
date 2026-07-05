using Microsoft.AspNetCore.Mvc;
using StudentManagementUI.Models;
using StudentMVC.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace StudentManagementUI.Controllers
{
    public class StudentController : Controller
    {
        private readonly HttpClient _httpClient;


    public StudentController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44314/Api/");
        }

        private bool SetToken()
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
                return false;

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            return true;
        }

        // GET: Student List
        public async Task<IActionResult> Index()
        {
            if (!SetToken())
                return RedirectToAction("Login", "Auth");

            var response = await _httpClient.GetFromJsonAsync<StudentResponse>("Student");

            if (response == null)
            {
                return View(new List<StudentViewModel>());
            }

            return View(response.Data);
        }

        // GET: Create
        [HttpGet]
        public IActionResult Create()
        {
            if (!SetToken())
                return RedirectToAction("Login", "Auth");

            return View();
        }

        // POST: Create
        [HttpPost]
        public async Task<IActionResult> Create(StudentViewModel student)
        {
            if (!SetToken())
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
                return View(student);

            var response = await _httpClient
                .PostAsJsonAsync("Student", student);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            return View(student);
        }

        // GET: Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!SetToken())
                return RedirectToAction("Login", "Auth");

            var student = await _httpClient
                .GetFromJsonAsync<StudentViewModel>($"Student/{id}");

            if (student == null)
                return NotFound();

            return View(student);
        }

        // POST: Edit
        [HttpPost]
        public async Task<IActionResult> Edit(int id, StudentViewModel student)
        {
            if (!SetToken())
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
                return View(student);

            var response = await _httpClient
                .PutAsJsonAsync($"Student/{id}", student);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            return View(student);
        }

        // GET: Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!SetToken())
                return RedirectToAction("Login", "Auth");

            var student = await _httpClient
                .GetFromJsonAsync<StudentViewModel>($"Student/{id}");

            if (student == null)
                return NotFound();

            return View(student);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!SetToken())
                return RedirectToAction("Login", "Auth");

            await _httpClient.DeleteAsync($"Student/{id}");

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (!SetToken())
                return RedirectToAction("Login", "Auth");

            var student = await _httpClient.GetFromJsonAsync<StudentViewModel>($"Student/{id}");

            if (student == null)
                return NotFound();

            return View(student);
        }
    }


}
