﻿// <auto-generated />
using System;
using Bachelor_backend.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Bachelorbackend.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Bachelor_backend.Models.Audiofile", b =>
                {
                    b.Property<Guid>("UUID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Path")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TextId")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("UUID");

                    b.HasIndex("TextId");

                    b.HasIndex("UserId");

                    b.ToTable("Audiofiles");
                });

            modelBuilder.Entity("Bachelor_backend.Models.Tag", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TagId"));

                    b.Property<string>("TagText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TagId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Bachelor_backend.Models.TargetGroup", b =>
                {
                    b.Property<int>("Targetid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Targetid"));

                    b.Property<string>("AgeGroups")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Dialects")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Genders")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Languages")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Targetid");

                    b.ToTable("TargetGroups");
                });

            modelBuilder.Entity("Bachelor_backend.Models.Text", b =>
                {
                    b.Property<int>("TextId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TextId"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<int?>("TargetGroupTargetid")
                        .HasColumnType("int");

                    b.Property<string>("TextText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TextId");

                    b.HasIndex("TargetGroupTargetid");

                    b.ToTable("Texts");
                });

            modelBuilder.Entity("Bachelor_backend.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("AgeGroup")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Dialect")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NativeLanguage")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TagText", b =>
                {
                    b.Property<int>("TagsTagId")
                        .HasColumnType("int");

                    b.Property<int>("TextsTextId")
                        .HasColumnType("int");

                    b.HasKey("TagsTagId", "TextsTextId");

                    b.HasIndex("TextsTextId");

                    b.ToTable("TagsForTexts", (string)null);
                });

            modelBuilder.Entity("Bachelor_backend.Models.Audiofile", b =>
                {
                    b.HasOne("Bachelor_backend.Models.Text", "Text")
                        .WithMany()
                        .HasForeignKey("TextId");

                    b.HasOne("Bachelor_backend.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Text");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Bachelor_backend.Models.Text", b =>
                {
                    b.HasOne("Bachelor_backend.Models.TargetGroup", "TargetGroup")
                        .WithMany()
                        .HasForeignKey("TargetGroupTargetid");

                    b.Navigation("TargetGroup");
                });

            modelBuilder.Entity("TagText", b =>
                {
                    b.HasOne("Bachelor_backend.Models.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsTagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bachelor_backend.Models.Text", null)
                        .WithMany()
                        .HasForeignKey("TextsTextId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
