using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using AKK.Classes.Models;

namespace Akk.Migrations
{
    [DbContext(typeof(MainDbContext))]
    partial class MainDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("AKK.Classes.Models.Route", b =>
                {
                    b.Property<Guid>("RouteId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<uint>("ColorOfHolds");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("Grade");

                    b.Property<string>("Name");

                    b.Property<Guid>("SectionId");

                    b.HasKey("RouteId");

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
                    b.HasOne("AKK.Classes.Models.Section", "Section")
                        .WithMany("Routes")
                        .HasForeignKey("SectionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
