﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PartWebApp2.Data;

namespace PartWebApp2.Migrations
{
    [DbContext(typeof(PartyWebAppContext))]
    [Migration("20210727153237_newUpdateModels")]
    partial class newUpdateModels
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PartWebApp2.Models.Area", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Area");
                });

            modelBuilder.Entity("PartWebApp2.Models.Club", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("LocationId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Club");
                });

            modelBuilder.Entity("PartWebApp2.Models.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Genre");
                });

            modelBuilder.Entity("PartWebApp2.Models.Party", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ProducerId")
                        .HasColumnType("int");

                    b.Property<int>("areaId")
                        .HasColumnType("int");

                    b.Property<int>("clubId")
                        .HasColumnType("int");

                    b.Property<DateTime>("eventDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("genreId")
                        .HasColumnType("int");

                    b.Property<int>("maxCapacity")
                        .HasColumnType("int");

                    b.Property<int>("minimalAge")
                        .HasColumnType("int");

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("price")
                        .HasColumnType("float");

                    b.Property<DateTime>("startTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("ticketsPurchased")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("areaId");

                    b.HasIndex("clubId");

                    b.HasIndex("genreId");

                    b.ToTable("Party");
                });

            modelBuilder.Entity("PartWebApp2.Models.PartyImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("PartyId")
                        .HasColumnType("int");

                    b.Property<string>("imageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PartyId")
                        .IsUnique();

                    b.ToTable("PartyImage");
                });

            modelBuilder.Entity("PartWebApp2.Models.Performer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("SpotifyId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Performer");
                });

            modelBuilder.Entity("PartWebApp2.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTime>("birthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("firstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("lastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("PartyPerformer", b =>
                {
                    b.Property<int>("partiesId")
                        .HasColumnType("int");

                    b.Property<int>("performersId")
                        .HasColumnType("int");

                    b.HasKey("partiesId", "performersId");

                    b.HasIndex("performersId");

                    b.ToTable("PartyPerformer");
                });

            modelBuilder.Entity("PartyUser", b =>
                {
                    b.Property<int>("partiesId")
                        .HasColumnType("int");

                    b.Property<int>("usersId")
                        .HasColumnType("int");

                    b.HasKey("partiesId", "usersId");

                    b.HasIndex("usersId");

                    b.ToTable("PartyUser");
                });

            modelBuilder.Entity("PartWebApp2.Models.Party", b =>
                {
                    b.HasOne("PartWebApp2.Models.Area", "area")
                        .WithMany("Parties")
                        .HasForeignKey("areaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PartWebApp2.Models.Club", "club")
                        .WithMany("Parties")
                        .HasForeignKey("clubId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PartWebApp2.Models.Genre", "genre")
                        .WithMany("parties")
                        .HasForeignKey("genreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("area");

                    b.Navigation("club");

                    b.Navigation("genre");
                });

            modelBuilder.Entity("PartWebApp2.Models.PartyImage", b =>
                {
                    b.HasOne("PartWebApp2.Models.Party", "Party")
                        .WithOne("partyImage")
                        .HasForeignKey("PartWebApp2.Models.PartyImage", "PartyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Party");
                });

            modelBuilder.Entity("PartyPerformer", b =>
                {
                    b.HasOne("PartWebApp2.Models.Party", null)
                        .WithMany()
                        .HasForeignKey("partiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PartWebApp2.Models.Performer", null)
                        .WithMany()
                        .HasForeignKey("performersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PartyUser", b =>
                {
                    b.HasOne("PartWebApp2.Models.Party", null)
                        .WithMany()
                        .HasForeignKey("partiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PartWebApp2.Models.User", null)
                        .WithMany()
                        .HasForeignKey("usersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PartWebApp2.Models.Area", b =>
                {
                    b.Navigation("Parties");
                });

            modelBuilder.Entity("PartWebApp2.Models.Club", b =>
                {
                    b.Navigation("Parties");
                });

            modelBuilder.Entity("PartWebApp2.Models.Genre", b =>
                {
                    b.Navigation("parties");
                });

            modelBuilder.Entity("PartWebApp2.Models.Party", b =>
                {
                    b.Navigation("partyImage");
                });
#pragma warning restore 612, 618
        }
    }
}
