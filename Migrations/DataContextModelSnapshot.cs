﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using CrudHW.Data;

#nullable disable

namespace CrudHW.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ProjectPreps.Entities.CourseEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Courses", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Gaming"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Math"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Programming"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Painting"
                        });
                });

            modelBuilder.Entity("ProjectPreps.Entities.StudentCourseEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("Grade")
                        .HasColumnType("int");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("StudentId");

                    b.ToTable("StudentsCourses", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CourseId = 3,
                            Grade = 100,
                            StudentId = 1
                        },
                        new
                        {
                            Id = 2,
                            CourseId = 1,
                            Grade = 100,
                            StudentId = 1
                        });
                });

            modelBuilder.Entity("ProjectPreps.Entities.StudentEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("LastName")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Students", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Age = 22,
                            FirstName = "Daniel",
                            LastName = "Krich"
                        },
                        new
                        {
                            Id = 2,
                            Age = 30,
                            FirstName = "David",
                            LastName = "Saimon"
                        },
                        new
                        {
                            Id = 3,
                            Age = 25,
                            FirstName = "Galit",
                            LastName = "Yardeni"
                        });
                });

            modelBuilder.Entity("ProjectPreps.Entities.StudentCourseEntity", b =>
                {
                    b.HasOne("ProjectPreps.Entities.CourseEntity", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectPreps.Entities.StudentEntity", "Student")
                        .WithMany("Courses")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("ProjectPreps.Entities.StudentEntity", b =>
                {
                    b.Navigation("Courses");
                });
#pragma warning restore 612, 618
        }
    }
}
