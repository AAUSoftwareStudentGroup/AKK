using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using AKK.Classes.Models;

namespace AKK.Migrations
{
    [DbContext(typeof(MainDbContext))]
    [Migration("20161108140214_modelnames")]
    partial class modelnames
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("AKK.Classes.Models.Grade", b =>
                {
                    b.Property<Guid>("GradeId")
                        .ValueGeneratedOnAdd();

                    b.Property<uint?>("ColorDB");

                    b.Property<int>("Difficulty");

                    b.HasKey("GradeId");

                    b.ToTable("Grades");
                });

            modelBuilder.Entity("AKK.Classes.Models.Route", b =>
                {
                    b.Property<Guid>("RouteId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<uint?>("ColorOfHoldsDB");

                    b.Property<uint?>("ColorOfTapeDB");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<Guid>("GradeId");

                    b.Property<string>("Name");

                    b.Property<bool>("PendingDeletion");

                    b.Property<Guid>("SectionId");

                    b.HasKey("RouteId");

                    b.HasIndex("GradeId");

                    b.HasIndex("SectionId");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("AKK.Classes.Models.Section", b =>
                {
                    b.Property<Guid>("SectionId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("SectionId");

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
                        .HasForeignKey("SectionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
