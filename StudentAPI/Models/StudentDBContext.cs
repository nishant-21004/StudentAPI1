using System;
using System.Collections.Generic;   
using Microsoft.EntityFrameworkCore;
using Microsoft .Extensions.Configuration;
namespace StudentAPI.Models
{
    public class StudentDBContext:DbContext
    {
        public StudentDBContext(DbContextOptions<StudentDBContext> options):base(options)
        {
            
        }
        public DbSet<Student> Students { get; set; }
        //public object User { get; internal set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.Email)
                .IsUnique();
        }
    }

}
