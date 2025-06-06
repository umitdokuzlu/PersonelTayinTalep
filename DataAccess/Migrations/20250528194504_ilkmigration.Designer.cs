﻿// <auto-generated />
using System;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccess.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250528194504_ilkmigration")]
    partial class ilkmigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Entities.Concrete.Adliye", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Ad")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Adliyeler");
                });

            modelBuilder.Entity("Entities.Concrete.Personel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Adi")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SicilNo")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Soyadi")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Unvan")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Personeller");
                });

            modelBuilder.Entity("Entities.Concrete.TayinTalep", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Aciklama")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("BasvuruTarihi")
                        .HasColumnType("datetime2");

                    b.Property<int>("PersonelId")
                        .HasColumnType("int");

                    b.Property<string>("TalepTuru")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("PersonelId");

                    b.ToTable("TayinTalepleri");
                });

            modelBuilder.Entity("Entities.Concrete.TayinTalepTercih", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AdliyeId")
                        .HasColumnType("int");

                    b.Property<int>("Sira")
                        .HasColumnType("int");

                    b.Property<int>("TayinTalepId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AdliyeId");

                    b.HasIndex("TayinTalepId");

                    b.ToTable("TayinTalepTercihleri");
                });

            modelBuilder.Entity("Entities.Concrete.TayinTalep", b =>
                {
                    b.HasOne("Entities.Concrete.Personel", "Personel")
                        .WithMany("TayinTalepleri")
                        .HasForeignKey("PersonelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Personel");
                });

            modelBuilder.Entity("Entities.Concrete.TayinTalepTercih", b =>
                {
                    b.HasOne("Entities.Concrete.Adliye", "Adliye")
                        .WithMany("TayinTalepTercihler")
                        .HasForeignKey("AdliyeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Entities.Concrete.TayinTalep", "TayinTalep")
                        .WithMany("Tercihler")
                        .HasForeignKey("TayinTalepId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Adliye");

                    b.Navigation("TayinTalep");
                });

            modelBuilder.Entity("Entities.Concrete.Adliye", b =>
                {
                    b.Navigation("TayinTalepTercihler");
                });

            modelBuilder.Entity("Entities.Concrete.Personel", b =>
                {
                    b.Navigation("TayinTalepleri");
                });

            modelBuilder.Entity("Entities.Concrete.TayinTalep", b =>
                {
                    b.Navigation("Tercihler");
                });
#pragma warning restore 612, 618
        }
    }
}
