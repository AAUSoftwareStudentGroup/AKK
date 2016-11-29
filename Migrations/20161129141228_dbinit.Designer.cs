using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using AKK.Models;

namespace AKK.Migrations
{
    [DbContext(typeof(MainDbContext))]
    [Migration("20161129141228_dbinit")]
    partial class dbinit
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("AKK.Models.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("MemberId");

                    b.Property<string>("Message");

                    b.Property<Guid>("RouteId");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.HasIndex("RouteId");

                    b.ToTable("Comments");
                });

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

            modelBuilder.Entity("AKK.Models.Rating", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("MemberId");

                    b.Property<int>("RatingValue");

                    b.Property<Guid>("RouteId");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.HasIndex("RouteId");

                    b.ToTable("Rating");
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

                    b.Property<string>("Note");

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

            modelBuilder.Entity("AKK.Models.Video", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FileUrl");

                    b.Property<Guid>("MemberId");

                    b.Property<Guid>("RouteId");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.HasIndex("RouteId");

                    b.ToTable("Videos");
                });

            modelBuilder.Entity("AKK.Models.Comment", b =>
                {
                    b.HasOne("AKK.Models.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AKK.Models.Route", "Route")
                        .WithMany("Comments")
                        .HasForeignKey("RouteId")
                        .OnDelete(DeleteBehavior.Cascade);
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
                    b.HasOne("AKK.Models.Route", "Route")
                        .WithOne("Image")
                        .HasForeignKey("AKK.Models.Image", "RouteId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AKK.Models.Rating", b =>
                {
                    b.HasOne("AKK.Models.Member", "Member")
                        .WithMany("Ratings")
                        .HasForeignKey("MemberId");

                    b.HasOne("AKK.Models.Route", "Route")
                        .WithMany("Ratings")
                        .HasForeignKey("RouteId")
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

            modelBuilder.Entity("AKK.Models.Video", b =>
                {
                    b.HasOne("AKK.Models.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AKK.Models.Route", "Route")
                        .WithMany("Videoes")
                        .HasForeignKey("RouteId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
