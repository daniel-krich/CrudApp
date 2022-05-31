using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrudHW.Entities;

namespace CrudHW.Data
{
    public class DataContext : DbContext
    {
#nullable disable
        public DbSet<StudentEntity> Students { get; set; }
        public DbSet<StudentCourseEntity> StudentsCourses { get; set; }
        public DbSet<CourseEntity> Courses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptions)
        {
            base.OnConfiguring(dbContextOptions);

            dbContextOptions.UseLazyLoadingProxies().UseSqlServer(@"data source=localhost;initial catalog=mydotnetdb;Trusted_Connection=true;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StudentEntity>()
                .HasMany(x => x.Courses)
                .WithOne(x => x.Student)
                .HasForeignKey(x => x.StudentId);

            modelBuilder.Entity<StudentEntity>().HasData(
                new StudentEntity
                {
                    Id = 1,
                    FirstName = "Daniel",
                    LastName = "Krich",
                    Age = 22
                },
                new StudentEntity
                {
                    Id = 2,
                    FirstName = "David",
                    LastName = "Saimon",
                    Age = 30
                },
                new StudentEntity
                {
                    Id = 3,
                    FirstName = "Galit",
                    LastName = "Yardeni",
                    Age = 25
                }
            );

            modelBuilder.Entity<CourseEntity>().HasData(
                new CourseEntity
                {
                    Id = 1,
                    Name = "Gaming"
                },
                new CourseEntity
                {
                    Id = 2,
                    Name = "Math"
                },
                new CourseEntity
                {
                    Id = 3,
                    Name = "Programming"
                },
                new CourseEntity
                {
                    Id = 4,
                    Name = "Painting"
                }
            );

            modelBuilder.Entity<StudentCourseEntity>().HasData(
                new StudentCourseEntity
                {
                    Id = 1,
                    CourseId = 3,
                    StudentId = 1,
                    Grade = 100
                },
                new StudentCourseEntity
                {
                    Id = 2,
                    CourseId = 1,
                    StudentId = 1,
                    Grade = 100
                }
            );
        }
    }
}
