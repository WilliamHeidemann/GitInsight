﻿// <auto-generated />
using GitInsight.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GitInsight.Infrastructure.Migrations
{
    [DbContext(typeof(PersistentStorageContext))]
    partial class PersistentStorageContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GitInsight.Infrastructure.DbCommit", b =>
                {
                    b.Property<string>("SHA")
                        .HasColumnType("text");

                    b.Property<int>("RepoId")
                        .HasColumnType("integer");

                    b.Property<string>("AuthorName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Date")
                        .IsRequired()
                        .HasColumnType("character varying(48)");

                    b.HasKey("SHA", "RepoId");

                    b.ToTable("Commits");
                });

            modelBuilder.Entity("GitInsight.Infrastructure.DbRepository", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NewestCommitSHA")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("FilePath")
                        .IsUnique();

                    b.ToTable("Repositories");
                });
#pragma warning restore 612, 618
        }
    }
}
