﻿// <auto-generated />
using ConsoleAppLearnEFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ConsoleAppLearnEFCore.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AuthorBook", b =>
                {
                    b.Property<int>("BookAuthorsId")
                        .HasColumnType("int");

                    b.Property<int>("BookAuthorsId1")
                        .HasColumnType("int");

                    b.HasKey("BookAuthorsId", "BookAuthorsId1");

                    b.HasIndex("BookAuthorsId1");

                    b.ToTable("AuthorBook");
                });

            modelBuilder.Entity("BookSection", b =>
                {
                    b.Property<int>("BookSectionsId")
                        .HasColumnType("int");

                    b.Property<int>("BookSectionsId1")
                        .HasColumnType("int");

                    b.HasKey("BookSectionsId", "BookSectionsId1");

                    b.HasIndex("BookSectionsId1");

                    b.ToTable("BookSection");
                });

            modelBuilder.Entity("ConsoleAppLearnEFCore.Model.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("ConsoleAppLearnEFCore.Model.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<int>("Pages")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("ConsoleAppLearnEFCore.Model.Section", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Sections");
                });

            modelBuilder.Entity("AuthorBook", b =>
                {
                    b.HasOne("ConsoleAppLearnEFCore.Model.Author", null)
                        .WithMany()
                        .HasForeignKey("BookAuthorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ConsoleAppLearnEFCore.Model.Book", null)
                        .WithMany()
                        .HasForeignKey("BookAuthorsId1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BookSection", b =>
                {
                    b.HasOne("ConsoleAppLearnEFCore.Model.Book", null)
                        .WithMany()
                        .HasForeignKey("BookSectionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ConsoleAppLearnEFCore.Model.Section", null)
                        .WithMany()
                        .HasForeignKey("BookSectionsId1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
