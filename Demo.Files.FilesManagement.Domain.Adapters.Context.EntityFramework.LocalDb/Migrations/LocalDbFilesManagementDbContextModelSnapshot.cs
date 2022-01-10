﻿// <auto-generated />
using System;
using Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.LocalDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.LocalDb.Migrations
{
    [DbContext(typeof(LocalDbFilesManagementDbContext))]
    partial class LocalDbFilesManagementDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.StorageAggregate.DirectoryData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.Property<long>("StorageId")
                        .HasColumnType("bigint")
                        .HasColumnName("storageId");

                    b.HasKey("Id");

                    b.HasIndex("StorageId");

                    b.ToTable("Directories");
                });

            modelBuilder.Entity("Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.StorageAggregate.FileData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("DirectoryId")
                        .HasColumnType("bigint")
                        .HasColumnName("directoryId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.Property<Guid>("PhysicalId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("physicalId");

                    b.Property<decimal>("Size")
                        .HasColumnType("decimal(20,0)")
                        .HasColumnName("size");

                    b.HasKey("Id");

                    b.HasIndex("DirectoryId");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.StorageAggregate.StorageData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<decimal>("LimitationsSingleFileSize")
                        .HasColumnType("decimal(20,0)")
                        .HasColumnName("limitations_singleFileSize");

                    b.Property<long>("LimitationsTotalFileCount")
                        .HasColumnType("bigint")
                        .HasColumnName("limitations_totalFileCount");

                    b.Property<decimal>("LimitationsTotalSpace")
                        .HasColumnType("decimal(20,0)")
                        .HasColumnName("limitations_totalSpace");

                    b.Property<decimal>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("decimal(20,0)")
                        .HasColumnName("version");

                    b.HasKey("Id");

                    b.HasIndex("Version");

                    b.ToTable("Storages");
                });

            modelBuilder.Entity("Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.StorageAggregate.DirectoryData", b =>
                {
                    b.HasOne("Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.StorageAggregate.StorageData", null)
                        .WithMany("Directories")
                        .HasForeignKey("StorageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.StorageAggregate.FileData", b =>
                {
                    b.HasOne("Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.StorageAggregate.DirectoryData", null)
                        .WithMany("Files")
                        .HasForeignKey("DirectoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.StorageAggregate.DirectoryData", b =>
                {
                    b.Navigation("Files");
                });

            modelBuilder.Entity("Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.StorageAggregate.StorageData", b =>
                {
                    b.Navigation("Directories");
                });
#pragma warning restore 612, 618
        }
    }
}