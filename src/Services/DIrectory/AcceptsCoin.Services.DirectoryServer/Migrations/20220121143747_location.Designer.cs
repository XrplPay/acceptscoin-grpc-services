﻿// <auto-generated />
using System;
using AcceptsCoin.Services.DirectoryServer.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace AcceptsCoin.Services.DirectoryServer.Migrations
{
    [DbContext(typeof(AcceptsCoinDirectoryDbContext))]
    [Migration("20220121143747_location")]
    partial class location
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasPostgresExtension("postgis")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("AcceptsCoin.Services.DirectoryServer.Domain.Models.Business", b =>
                {
                    b.Property<Guid>("BusinessId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid");

                    b.Property<string>("ContactNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FaceBook")
                        .HasColumnType("text");

                    b.Property<string>("Instagram")
                        .HasColumnType("text");

                    b.Property<double>("Latitude")
                        .HasColumnType("double precision");

                    b.Property<Point>("Location")
                        .HasColumnType("geography (point)");

                    b.Property<string>("Logo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Longitude")
                        .HasColumnType("double precision");

                    b.Property<string>("Manager")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OfferedServices")
                        .HasColumnType("text");

                    b.Property<string>("Owner")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("PartnerId")
                        .HasColumnType("uuid");

                    b.Property<bool>("Published")
                        .HasColumnType("boolean");

                    b.Property<string>("Twitter")
                        .HasColumnType("text");

                    b.Property<Guid?>("UpdatedById")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("Verified")
                        .HasColumnType("boolean");

                    b.Property<string>("WebSiteUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("BusinessId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("PartnerId");

                    b.HasIndex("UpdatedById");

                    b.ToTable("Businesses");
                });

            modelBuilder.Entity("AcceptsCoin.Services.DirectoryServer.Domain.Models.BusinessGallery", b =>
                {
                    b.Property<Guid>("BusinessGalleryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("BusinessId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Extension")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Published")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("UpdatedById")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("BusinessGalleryId");

                    b.HasIndex("BusinessId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("UpdatedById");

                    b.ToTable("BusinessGalleries");
                });

            modelBuilder.Entity("AcceptsCoin.Services.DirectoryServer.Domain.Models.BusinessTag", b =>
                {
                    b.Property<Guid>("BusinessId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TagId")
                        .HasColumnType("uuid");

                    b.HasKey("BusinessId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("BusinessTags");
                });

            modelBuilder.Entity("AcceptsCoin.Services.DirectoryServer.Domain.Models.Category", b =>
                {
                    b.Property<Guid>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            CategoryId = new Guid("711425a4-07b9-4396-bb33-942a73ba6354")
                        },
                        new
                        {
                            CategoryId = new Guid("ba626277-b49c-4e2b-9410-16ca496f278d")
                        });
                });

            modelBuilder.Entity("AcceptsCoin.Services.DirectoryServer.Domain.Models.Partner", b =>
                {
                    b.Property<Guid>("PartnerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("PartnerId");

                    b.ToTable("Partners");

                    b.HasData(
                        new
                        {
                            PartnerId = new Guid("bff3b2dd-e89d-46fc-a868-aab93a3efbbe")
                        });
                });

            modelBuilder.Entity("AcceptsCoin.Services.DirectoryServer.Domain.Models.Review", b =>
                {
                    b.Property<Guid>("ReviewId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("BusinessId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Published")
                        .HasColumnType("boolean");

                    b.Property<int>("Rate")
                        .HasColumnType("integer");

                    b.Property<Guid?>("UpdatedById")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("ReviewId");

                    b.HasIndex("BusinessId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("UpdatedById");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("AcceptsCoin.Services.DirectoryServer.Domain.Models.Tag", b =>
                {
                    b.Property<Guid>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("TagId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("AcceptsCoin.Services.DirectoryServer.Domain.Models.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = new Guid("999bb90f-3167-4f81-83bb-0c76d1d3ace5"),
                            Email = "info@acceptscoin.com",
                            Name = "Super Admin"
                        });
                });

            modelBuilder.Entity("AcceptsCoin.Services.DirectoryServer.Domain.Models.Business", b =>
                {
                    b.HasOne("AcceptsCoin.Services.DirectoryServer.Domain.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AcceptsCoin.Services.DirectoryServer.Domain.Models.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AcceptsCoin.Services.DirectoryServer.Domain.Models.Partner", "Partner")
                        .WithMany()
                        .HasForeignKey("PartnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AcceptsCoin.Services.DirectoryServer.Domain.Models.User", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");

                    b.Navigation("Category");

                    b.Navigation("CreatedBy");

                    b.Navigation("Partner");

                    b.Navigation("UpdatedBy");
                });

            modelBuilder.Entity("AcceptsCoin.Services.DirectoryServer.Domain.Models.BusinessGallery", b =>
                {
                    b.HasOne("AcceptsCoin.Services.DirectoryServer.Domain.Models.Business", "Business")
                        .WithMany("BusinessGalleries")
                        .HasForeignKey("BusinessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AcceptsCoin.Services.DirectoryServer.Domain.Models.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AcceptsCoin.Services.DirectoryServer.Domain.Models.User", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");

                    b.Navigation("Business");

                    b.Navigation("CreatedBy");

                    b.Navigation("UpdatedBy");
                });

            modelBuilder.Entity("AcceptsCoin.Services.DirectoryServer.Domain.Models.BusinessTag", b =>
                {
                    b.HasOne("AcceptsCoin.Services.DirectoryServer.Domain.Models.Business", "Business")
                        .WithMany("BusinessTags")
                        .HasForeignKey("BusinessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AcceptsCoin.Services.DirectoryServer.Domain.Models.Tag", "Tag")
                        .WithMany("BusinessTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Business");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("AcceptsCoin.Services.DirectoryServer.Domain.Models.Review", b =>
                {
                    b.HasOne("AcceptsCoin.Services.DirectoryServer.Domain.Models.Business", "Business")
                        .WithMany("BusinessReviews")
                        .HasForeignKey("BusinessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AcceptsCoin.Services.DirectoryServer.Domain.Models.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AcceptsCoin.Services.DirectoryServer.Domain.Models.User", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");

                    b.Navigation("Business");

                    b.Navigation("CreatedBy");

                    b.Navigation("UpdatedBy");
                });

            modelBuilder.Entity("AcceptsCoin.Services.DirectoryServer.Domain.Models.Business", b =>
                {
                    b.Navigation("BusinessGalleries");

                    b.Navigation("BusinessReviews");

                    b.Navigation("BusinessTags");
                });

            modelBuilder.Entity("AcceptsCoin.Services.DirectoryServer.Domain.Models.Tag", b =>
                {
                    b.Navigation("BusinessTags");
                });
#pragma warning restore 612, 618
        }
    }
}
