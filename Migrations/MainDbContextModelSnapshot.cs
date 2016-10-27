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

            modelBuilder.Entity("AKK.Classes.Models.Route", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<uint>("ColorOfHolds");

                    b.Property<DateTime>("Date");

                    b.Property<int>("Grade");

                    b.Property<string>("Name");

                    b.Property<string>("SectionID");

                    b.HasKey("ID");

                    b.HasIndex("SectionID");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("AKK.Classes.Models.Section", b =>
                {
                    b.Property<string>("Name");

                    b.HasKey("Name");

                    b.ToTable("Sections");
                });

            modelBuilder.Entity("AKK.Classes.Models.Route", b =>
                {
                    b.HasOne("AKK.Classes.Models.Section", "Section")
                        .WithMany("Routes")
                        .HasForeignKey("SectionID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
