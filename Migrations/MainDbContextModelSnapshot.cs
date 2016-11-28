using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using AKK.Models;

namespace AKK.Migrations
{
    [DbContext(typeof(MainDbContext))]
    partial class MainDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("AKK.Models.Grade", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Difficulty");

                    b.Property<uint?>("HexColor");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Grades");
                });

            modelBuilder.Entity("AKK.Models.Hold", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ImageId");

                    b.Property<double>("Radius");

                    b.Property<double>("X");

                    b.Property<double>("Y");

                    b.HasKey("Id");

                    b.HasIndex("ImageId");

                    b.ToTable("Holds");
                });

            modelBuilder.Entity("AKK.Models.Image", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FileUrl");

                    b.Property<uint>("Height");

                    b.Property<Guid>("RouteId");

                    b.Property<uint>("Width");

                    b.HasKey("Id");

                    b.HasIndex("RouteId")
                        .IsUnique();

                    b.ToTable("Images");
                });

            modelBuilder.Entity("AKK.Models.Member", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DisplayName");

                    b.Property<bool>("IsAdmin");

                    b.Property<string>("Password");

                    b.Property<string>("Token");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("AKK.Models.Route", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<Guid>("GradeId");

                    b.Property<uint?>("HexColorOfHolds");

                    b.Property<uint?>("HexColorOfTape");

                    b.Property<Guid>("MemberId");

                    b.Property<string>("Name");

                    b.Property<bool>("PendingDeletion");

                    b.Property<Guid>("SectionId");

                    b.HasKey("Id");

                    b.HasIndex("GradeId");

                    b.HasIndex("MemberId");

                    b.HasIndex("SectionId");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("AKK.Models.Section", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Sections");
                });

            modelBuilder.Entity("AKK.Models.Hold", b =>
                {
                    b.HasOne("AKK.Models.Image", "Image")
                        .WithMany("Holds")
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AKK.Models.Image", b =>
                {
                    b.HasOne("AKK.Models.Route")
                        .WithOne("Image")
                        .HasForeignKey("AKK.Models.Image", "RouteId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AKK.Models.Route", b =>
                {
                    b.HasOne("AKK.Models.Grade", "Grade")
                        .WithMany("Routes")
                        .HasForeignKey("GradeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AKK.Models.Member", "Member")
                        .WithMany("Routes")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AKK.Models.Section", "Section")
                        .WithMany("Routes")
                        .HasForeignKey("SectionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
