﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TranslaterServiceDL.Context;

#nullable disable

namespace TranslaterServiceDL.Migrations
{
    [DbContext(typeof(TranslaterContext))]
    partial class TranslaterContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.17")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TranslaterServiceDL.Models.Keyword", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("character varying(25)")
                        .HasColumnName("Value");

                    b.HasKey("Id");

                    b.HasIndex("Value")
                        .IsUnique();

                    b.ToTable("Keyword", (string)null);
                });

            modelBuilder.Entity("TranslaterServiceDL.Models.Language", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("character varying(25)")
                        .HasColumnName("Name");

                    b.HasKey("Id");

                    b.ToTable("Language", (string)null);
                });

            modelBuilder.Entity("TranslaterServiceDL.Models.Translation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("KeywordId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("LanguageId")
                        .HasColumnType("uuid");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("KeywordId");

                    b.HasIndex("LanguageId");

                    b.ToTable("Translation", (string)null);
                });

            modelBuilder.Entity("TranslaterServiceDL.Models.Translation", b =>
                {
                    b.HasOne("TranslaterServiceDL.Models.Keyword", "Keyword")
                        .WithMany("Translations")
                        .HasForeignKey("KeywordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TranslaterServiceDL.Models.Language", "Language")
                        .WithMany("Translations")
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Keyword");

                    b.Navigation("Language");
                });

            modelBuilder.Entity("TranslaterServiceDL.Models.Keyword", b =>
                {
                    b.Navigation("Translations");
                });

            modelBuilder.Entity("TranslaterServiceDL.Models.Language", b =>
                {
                    b.Navigation("Translations");
                });
#pragma warning restore 612, 618
        }
    }
}
