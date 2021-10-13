﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReserveringSysteem.Database;

namespace ReserveringSysteem.Database.Migrations
{
    [DbContext(typeof(ReserveringContext))]
    [Migration("20211013083849_AddTelefoonBedrijf")]
    partial class AddTelefoonBedrijf
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ReserveringSysteem.Database.Models.BedrijfsModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Adress")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Afdeling")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("BTWNummer")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("KVKNummer")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Naam")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("PostCode")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("TelefoonNummer")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("ID")
                        .HasName("PK_Bedrijven");

                    b.ToTable("bedrijven");
                });

            modelBuilder.Entity("ReserveringSysteem.Database.Models.ReserveringsModel", b =>
                {
                    b.Property<int>("ReserveringID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AantalPersonen")
                        .HasColumnType("int");

                    b.Property<int>("BedrijfID")
                        .HasColumnType("int");

                    b.Property<int>("ID")
                        .HasColumnType("int");

                    b.Property<string>("NaamReserverende")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("Tafel")
                        .HasColumnType("int");

                    b.Property<string>("TelefoonNummer")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Tijd")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("ReserveringID")
                        .HasName("PK_Reserveringen");

                    b.HasIndex("BedrijfID")
                        .IsUnique();

                    b.HasIndex("ID");

                    b.ToTable("reserveringen");
                });

            modelBuilder.Entity("ReserveringSysteem.Database.Models.VestigingsModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("MaxPersonen")
                        .HasColumnType("int");

                    b.Property<int>("MaxTafels")
                        .HasColumnType("int");

                    b.Property<string>("Naam")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<TimeSpan>("OpeningsTijd")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("SluitingsTijd")
                        .HasColumnType("time");

                    b.HasKey("ID")
                        .HasName("PK_Vestiging");

                    b.ToTable("vestigingen");
                });

            modelBuilder.Entity("ReserveringSysteem.Database.Models.ReserveringsModel", b =>
                {
                    b.HasOne("ReserveringSysteem.Database.Models.BedrijfsModel", "Bedrijf")
                        .WithOne("Reservering")
                        .HasForeignKey("ReserveringSysteem.Database.Models.ReserveringsModel", "BedrijfID")
                        .HasConstraintName("FK__reservering_bedrijf_id__bedrijf_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ReserveringSysteem.Database.Models.VestigingsModel", "Vestiging")
                        .WithMany("Reservering")
                        .HasForeignKey("ID")
                        .HasConstraintName("FK__reservering_id__vestiging_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bedrijf");

                    b.Navigation("Vestiging");
                });

            modelBuilder.Entity("ReserveringSysteem.Database.Models.BedrijfsModel", b =>
                {
                    b.Navigation("Reservering");
                });

            modelBuilder.Entity("ReserveringSysteem.Database.Models.VestigingsModel", b =>
                {
                    b.Navigation("Reservering");
                });
#pragma warning restore 612, 618
        }
    }
}
