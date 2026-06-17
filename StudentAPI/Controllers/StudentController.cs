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
    int pageNumber = 1,
    int pageSize = 10)
        {
            var totalRecords = await _context.Students.CountAsync();

            var students = await _context.Students
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
