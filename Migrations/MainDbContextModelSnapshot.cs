using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using AKK.Classes.Models;

namespace AKK.Migrations
{
    [DbContext(typeof(MainDbContext))]
    partial class MainDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("AKK.Classes.Models.Grade", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<uint?>("ColorDb");

                    b.Property<int>("Difficulty");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Grades");
                });

            modelBuilder.Entity("AKK.Classes.Models.Route", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<uint?>("ColorOfHoldsDb");

                    b.Property<uint?>("ColorOfTapeDb");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<Guid>("GradeId");

                    b.Property<string>("Name");

                    b.Property<bool>("PendingDeletion");

                    b.Property<Guid>("Id");

                    b.HasKey("Id");

                    b.HasIndex("GradeId");

                    b.HasIndex("Id");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("AKK.Classes.Models.Section", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Sections");
                });

            modelBuilder.Entity("AKK.Classes.Models.Route", b =>
                {
                    b.HasOne("AKK.Classes.Models.Grade", "Grade")
                        .WithMany("Routes")
                        .HasForeignKey("GradeId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("AKK.Classes.Models.Section", "Section")
                        .WithMany("Routes")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
