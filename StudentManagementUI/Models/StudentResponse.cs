using StudentMVC.Models;

namespace StudentManagementUI.Models
{
    public class StudentResponse
    {
        public int TotalRecords { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

        public List<StudentViewModel> Data { get; set; } = new();
    }
}
