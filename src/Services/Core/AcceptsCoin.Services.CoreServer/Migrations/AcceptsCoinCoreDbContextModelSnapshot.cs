﻿// <auto-generated />
using System;
using AcceptsCoin.Services.CoreServer.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace AcceptsCoin.Services.CoreServer.Migrations
{
    [DbContext(typeof(AcceptsCoinCoreDbContext))]
    partial class AcceptsCoinCoreDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasPostgresExtension("postgis")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("AcceptsCoin.Services.CoreServer.Domain.Models.Category", b =>
                {
                    b.Property<Guid>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Icon")
                        .HasColumnType("text");

                    b.Property<string>("Logo")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Priority")
                        .HasColumnType("integer");

                    b.Property<bool>("Published")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("UpdatedById")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("CategoryId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("UpdatedById");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            CategoryId = new Guid("711425a4-07b9-4396-bb33-942a73ba6354"),
                            CreatedById = new Guid("999bb90f-3167-4f81-83bb-0c76d1d3ace5"),
                            CreatedDate = new DateTime(2022, 1, 11, 15, 56, 53, 925, DateTimeKind.Local).AddTicks(8290),
                            Deleted = false,
                            Icon = "",
                            Logo = "",
                            Name = "Food",
                            Priority = 1,
                            Published = true
                        },
                        new
                        {
                            CategoryId = new Guid("ba626277-b49c-4e2b-9410-16ca496f278d"),
                            CreatedById = new Guid("999bb90f-3167-4f81-83bb-0c76d1d3ace5"),
                            CreatedDate = new DateTime(2022, 1, 11, 15, 56, 53, 934, DateTimeKind.Local).AddTicks(6900),
                            Deleted = false,
                            Icon = "",
                            Logo = "",
                            Name = "Car",
                            Priority = 2,
                            Published = true
                        });
                });

            modelBuilder.Entity("AcceptsCoin.Services.CoreServer.Domain.Models.Language", b =>
                {
                    b.Property<Guid>("LanguageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Icon")
                        .HasColumnType("text");

                    b.Property<string>("Logo")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Priority")
                        .HasColumnType("integer");

                    b.Property<bool>("Published")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("UpdatedById")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("LanguageId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("UpdatedById");

                    b.ToTable("Languages");

                    b.HasData(
                        new
                        {
                            LanguageId = new Guid("9934b846-e3f1-406a-9207-04926e553d1b"),
                            Code = "en",
                            CreatedById = new Guid("999bb90f-3167-4f81-83bb-0c76d1d3ace5"),
                            CreatedDate = new DateTime(2022, 1, 11, 15, 56, 53, 935, DateTimeKind.Local).AddTicks(2350),
                            Deleted = false,
                            Icon = "",
                            Logo = "",
                            Name = "ENGLISH",
                            Priority = 0,
                            Published = true
                        });
                });

            modelBuilder.Entity("AcceptsCoin.Services.CoreServer.Domain.Models.Partner", b =>
                {
                    b.Property<Guid>("PartnerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ApiKey")
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

                    b.Property<Guid>("LanguageId")
                        .HasColumnType("uuid");

                    b.Property<string>("Logo")
                        .HasColumnType("text");

                    b.Property<string>("Manager")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Owner")
                        .HasColumnType("text");

                    b.Property<bool>("Published")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("UpdatedById")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("WebSiteUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("PartnerId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("LanguageId");

                    b.HasIndex("UpdatedById");

                    b.ToTable("Partners");

                    b.HasData(
                        new
                        {
                            PartnerId = new Guid("bff3b2dd-e89d-46fc-a868-aab93a3efbbe"),
                            ApiKey = new Guid("a96b2fa5-fbf2-4173-92eb-c5c9ce77e7dc"),
                            ContactNumber = "0000",
                            CreatedById = new Guid("999bb90f-3167-4f81-83bb-0c76d1d3ace5"),
                            CreatedDate = new DateTime(2022, 1, 11, 15, 56, 53, 935, DateTimeKind.Local).AddTicks(6580),
                            Deleted = false,
                            Email = "info@acceptscoin.com",
                            LanguageId = new Guid("9934b846-e3f1-406a-9207-04926e553d1b"),
                            Logo = "",
                            Name = "Accepts Coin",
                            Published = true,
                            WebSiteUrl = "https://acceptscoin.com"
                        });
                });

            modelBuilder.Entity("AcceptsCoin.Services.CoreServer.Domain.Models.PartnerCategory", b =>
                {
                    b.Property<Guid>("PartnerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("Published")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("UpdatedById")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("PartnerId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("UpdatedById");

                    b.ToTable("PartnerCategories");
                });

            modelBuilder.Entity("AcceptsCoin.Services.CoreServer.Domain.Models.PartnerToken", b =>
                {
                    b.Property<Guid>("PartnerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TokenId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("Published")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("UpdatedById")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("PartnerId", "TokenId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("TokenId");

                    b.HasIndex("UpdatedById");

                    b.ToTable("PartnerTokens");
                });

            modelBuilder.Entity("AcceptsCoin.Services.CoreServer.Domain.Models.Tag", b =>
                {
                    b.Property<Guid>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("Published")
                        .HasColumnType("boolean");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("UpdatedById")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("TagId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("UpdatedById");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("AcceptsCoin.Services.CoreServer.Domain.Models.Token", b =>
                {
                    b.Property<Guid>("TokenId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("Published")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("UpdatedById")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("TokenId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("UpdatedById");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("AcceptsCoin.Services.CoreServer.Domain.Models.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = new Guid("999bb90f-3167-4f81-83bb-0c76d1d3ace5")
                        });
                });

            modelBuilder.Entity("AcceptsCoin.Services.CoreServer.Domain.Models.Category", b =>
                {
                    b.HasOne("AcceptsCoin.Services.CoreServer.Domain.Models.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AcceptsCoin.Services.CoreServer.Domain.Models.User", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");

                    b.Navigation("CreatedBy");

                    b.Navigation("UpdatedBy");
                });

            modelBuilder.Entity("AcceptsCoin.Services.CoreServer.Domain.Models.Language", b =>
                {
                    b.HasOne("AcceptsCoin.Services.CoreServer.Domain.Models.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AcceptsCoin.Services.CoreServer.Domain.Models.User", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");

                    b.Navigation("CreatedBy");

                    b.Navigation("UpdatedBy");
                });

            modelBuilder.Entity("AcceptsCoin.Services.CoreServer.Domain.Models.Partner", b =>
                {
                    b.HasOne("AcceptsCoin.Services.CoreServer.Domain.Models.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AcceptsCoin.Services.CoreServer.Domain.Models.Language", "Language")
                        .WithMany("Partners")
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AcceptsCoin.Services.CoreServer.Domain.Models.User", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");

                    b.Navigation("CreatedBy");

                    b.Navigation("Language");

                    b.Navigation("UpdatedBy");
                });

            modelBuilder.Entity("AcceptsCoin.Services.CoreServer.Domain.Models.PartnerCategory", b =>
                {
                    b.HasOne("AcceptsCoin.Services.CoreServer.Domain.Models.Category", "Category")
                        .WithMany("PartnerCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AcceptsCoin.Services.CoreServer.Domain.Models.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AcceptsCoin.Services.CoreServer.Domain.Models.Partner", "Partner")
                        .WithMany("PartnerCategories")
                        .HasForeignKey("PartnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AcceptsCoin.Services.CoreServer.Domain.Models.User", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");

                    b.Navigation("Category");

                    b.Navigation("CreatedBy");

                    b.Navigation("Partner");

                    b.Navigation("UpdatedBy");
                });

            modelBuilder.Entity("AcceptsCoin.Services.CoreServer.Domain.Models.PartnerToken", b =>
                {
                    b.HasOne("AcceptsCoin.Services.CoreServer.Domain.Models.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AcceptsCoin.Services.CoreServer.Domain.Models.Partner", "Partner")
                        .WithMany("PartnerTokens")
                        .HasForeignKey("PartnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AcceptsCoin.Services.CoreServer.Domain.Models.Token", "Token")
                        .WithMany("PartnerTokens")
                        .HasForeignKey("TokenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AcceptsCoin.Services.CoreServer.Domain.Models.User", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");

                    b.Navigation("CreatedBy");

                    b.Navigation("Partner");

                    b.Navigation("Token");

                    b.Navigation("UpdatedBy");
                });

            modelBuilder.Entity("AcceptsCoin.Services.CoreServer.Domain.Models.Tag", b =>
                {
                    b.HasOne("AcceptsCoin.Services.CoreServer.Domain.Models.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AcceptsCoin.Services.CoreServer.Domain.Models.User", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");

                    b.Navigation("CreatedBy");

                    b.Navigation("UpdatedBy");
                });

            modelBuilder.Entity("AcceptsCoin.Services.CoreServer.Domain.Models.Token", b =>
                {
                    b.HasOne("AcceptsCoin.Services.CoreServer.Domain.Models.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AcceptsCoin.Services.CoreServer.Domain.Models.User", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");

                    b.Navigation("CreatedBy");

                    b.Navigation("UpdatedBy");
                });

            modelBuilder.Entity("AcceptsCoin.Services.CoreServer.Domain.Models.Category", b =>
                {
                    b.Navigation("PartnerCategories");
                });

            modelBuilder.Entity("AcceptsCoin.Services.CoreServer.Domain.Models.Language", b =>
                {
                    b.Navigation("Partners");
                });

            modelBuilder.Entity("AcceptsCoin.Services.CoreServer.Domain.Models.Partner", b =>
                {
                    b.Navigation("PartnerCategories");

                    b.Navigation("PartnerTokens");
                });

            modelBuilder.Entity("AcceptsCoin.Services.CoreServer.Domain.Models.Token", b =>
                {
                    b.Navigation("PartnerTokens");
                });
#pragma warning restore 612, 618
        }
    }
}
