using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace StudentAPI.Models
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } 
        [Required]
        [MaxLength(50)]
        public string  LastName { get; set; }
        [Required]
        public string  Email { get; set; }
        [Required]
        public DateTime DOB { get; set; }
        public string? Gender { get; set; } 
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? IsActive { get; set; }= "true";


    }
}
