﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PeopleForPeople.Models;

#nullable disable

namespace PeopleForPeople.Migrations
{
    [DbContext(typeof(MyContext))]
    [Migration("20221001084210_PeopleForPeopleMigration1")]
    partial class PeopleForPeopleMigration1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("PeopleForPeople.Models.Case", b =>
                {
                    b.Property<int>("CaseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FamilySurname")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Tittle")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("CaseId");

                    b.HasIndex("UserId");

                    b.ToTable("Cases");
                });

            modelBuilder.Entity("PeopleForPeople.Models.Chat", b =>
                {
                    b.Property<int>("ChatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CaseId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ChatId");

                    b.HasIndex("CaseId");

                    b.HasIndex("UserId");

                    b.ToTable("Chats");
                });

            modelBuilder.Entity("PeopleForPeople.Models.Message", b =>
                {
                    b.Property<string>("MessageId")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("CaseId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("MessageContent")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("MessageId");

                    b.HasIndex("CaseId");

                    b.HasIndex("UserId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("PeopleForPeople.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("CreatedCaseUserId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("UserId");

                    b.HasIndex("CreatedCaseUserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PeopleForPeople.Models.Case", b =>
                {
                    b.HasOne("PeopleForPeople.Models.User", "Creator")
                        .WithMany("Cases")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("PeopleForPeople.Models.Chat", b =>
                {
                    b.HasOne("PeopleForPeople.Models.Case", "ConnectedWith")
                        .WithMany()
                        .HasForeignKey("CaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PeopleForPeople.Models.User", "User")
                        .WithMany("Chats")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ConnectedWith");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PeopleForPeople.Models.Message", b =>
                {
                    b.HasOne("PeopleForPeople.Models.Case", "ConnectedTo")
                        .WithMany()
                        .HasForeignKey("CaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PeopleForPeople.Models.User", "Creator")
                        .WithMany("CreatedMessages")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ConnectedTo");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("PeopleForPeople.Models.User", b =>
                {
                    b.HasOne("PeopleForPeople.Models.User", "CreatedCase")
                        .WithMany()
                        .HasForeignKey("CreatedCaseUserId");

                    b.Navigation("CreatedCase");
                });

            modelBuilder.Entity("PeopleForPeople.Models.User", b =>
                {
                    b.Navigation("Cases");

                    b.Navigation("Chats");

                    b.Navigation("CreatedMessages");
                });
#pragma warning restore 612, 618
        }
    }
}
