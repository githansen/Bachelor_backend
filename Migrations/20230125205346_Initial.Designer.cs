﻿// <auto-generated />
using System;
using Bachelor_backend.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Bachelorbackend.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20230125205346_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Bachelor_backend.DAL.DatabaseContext+TagForText", b =>
                {
                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.Property<int>("TextId")
                        .HasColumnType("int");

                    b.HasKey("TagId", "TextId");

                    b.HasIndex("TextId");

                    b.ToTable("TagForText");
                });

            modelBuilder.Entity("Bachelor_backend.Models.Audiofile", b =>
                {
                    b.Property<Guid>("UUID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Path")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TextId")
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

            modelBuilder.Entity("Bachelor_backend.Models.Text", b =>
                {
                    b.Property<int>("TextId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TextId"));

                    b.Property<string>("TextText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TextId");

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

                    b.Property<string>("NativeLanguage")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Bachelor_backend.DAL.DatabaseContext+TagForText", b =>
                {
                    b.HasOne("Bachelor_backend.Models.Tag", "tag")
                        .WithMany("texts")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bachelor_backend.Models.Text", "Text")
                        .WithMany("Tags")
                        .HasForeignKey("TextId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Text");

                    b.Navigation("tag");
                });

            modelBuilder.Entity("Bachelor_backend.Models.Audiofile", b =>
                {
                    b.HasOne("Bachelor_backend.Models.Text", "text")
                        .WithMany()
                        .HasForeignKey("TextId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bachelor_backend.Models.User", "user")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("text");

                    b.Navigation("user");
                });

            modelBuilder.Entity("Bachelor_backend.Models.Tag", b =>
                {
                    b.Navigation("texts");
                });

            modelBuilder.Entity("Bachelor_backend.Models.Text", b =>
                {
                    b.Navigation("Tags");
                });
#pragma warning restore 612, 618
        }
    }
}
