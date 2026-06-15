using Microsoft.EntityFrameworkCore;
using StudentAPI.Models;
using StudentAPI.Services;
using Xunit;

namespace StudentAPI.Tests.Services
{
    public class StudentServiceTests
    {
        [Fact]
        public async Task ExportStudentsAsync_ReturnsExcelFile()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<StudentDBContext>()
                .UseInMemoryDatabase(databaseName: "StudentTestDb")
                .Options;

            using var context = new StudentDBContext(options);

            context.Students.Add(new Student
            {
                Id = 1,
                FirstName = "Anup",
                LastName = "K Maurya",
                Email = "anup@gmail.com",
                DOB = new DateTime(2004/01/01)
            });

            await context.SaveChangesAsync();

            var service = new StudentServices(context);

            // Act
            var result = await service.ExportStudentsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Length > 0);
        }
    }
}