using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using StudentAPI.Models;

namespace StudentAPI.Services
{
    public class StudentServices
    {
        private readonly StudentDBContext _context;

        public StudentServices(StudentDBContext context)
        {
            _context = context;
        }

        public async Task<byte[]> ExportStudentsAsync()
        {
            var students = await _context.Students.ToListAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Students");

            worksheet.Cell(1, 1).Value = "Id";
            worksheet.Cell(1, 2).Value = "Name";
            worksheet.Cell(1, 3).Value = "Email";
            worksheet.Cell(1, 4).Value = "DOB";
            

            int row = 2;

            foreach (var student in students)
            {
                worksheet.Cell(row, 1).Value = student.Id;
                worksheet.Cell(row, 2).Value = $"{student.FirstName} {student.LastName}";
                worksheet.Cell(row, 3).Value = student.Email;
                worksheet.Cell(row, 4).Value = student.DOB;

                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return stream.ToArray();
        }
    }
}