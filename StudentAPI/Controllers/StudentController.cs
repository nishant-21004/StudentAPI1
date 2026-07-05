using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAPI.Models;
using StudentAPI.Services;

namespace StudentAPI.Controllers
{
    [Authorize(Roles  = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentDBContext _context;
        private readonly StudentServices _studentServices;


    public StudentController(
        StudentDBContext context,
        StudentServices studentServices)
        {
            _context = context;
            _studentServices = studentServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
      string? name,
      string? email,
      string? gender,
      string? phone,
      string? address,
      int pageNumber = 1,
      int pageSize = 10)
        {
            var query = _context.Students.AsQueryable();

            // Name Filter (FirstName + LastName)
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(s =>
                    s.FirstName.Contains(name) ||
                    s.LastName.Contains(name) ||
                    (s.FirstName + " " + s.LastName).Contains(name));
            }

            // Email Filter
            if (!string.IsNullOrWhiteSpace(email))
            {
                query = query.Where(s => s.Email.Contains(email));
            }

            // Gender Filter
            if (!string.IsNullOrWhiteSpace(gender))
            {
                query = query.Where(s => s.Gender == gender);
            }

            // Phone Filter
            if (!string.IsNullOrWhiteSpace(phone))
            {
                query = query.Where(s => s.Phone.Contains(phone));
            }

            // Address Filter
            if (!string.IsNullOrWhiteSpace(address))
            {
                query = query.Where(s => s.Address.Contains(address));
            }

            // Total records after filtering
            var totalRecords = await query.CountAsync();

            // Pagination
            var students = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize),
                Data = students
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
                return NotFound("Student not found");

            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> Insert(Student student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();

            return Ok(student);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Student student)
        {
            var existingStudent = await _context.Students.FindAsync(id);

            if (existingStudent == null)
                return NotFound("Student not found");

            existingStudent.FirstName = student.FirstName;
            existingStudent.LastName = student.LastName;
            existingStudent.Email = student.Email;
            existingStudent.DOB = student.DOB;

            await _context.SaveChangesAsync();

            return Ok(existingStudent);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
                return NotFound("Student not found");

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return Ok("Student deleted successfully");
        }

        [HttpGet("export")]
        public async Task<IActionResult> Export()
        {
            var fileBytes = await _studentServices.ExportStudentsAsync();

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Students.xlsx");
        }
    }


}
