﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WeeklyFuelPricesLoader.Database;

#nullable disable

namespace WeeklyFuelPricesLoader.Migrations
{
    [DbContext(typeof(FuelPricesContext))]
    [Migration("20230420171638_InitialScaffold")]
    partial class InitialScaffold
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WeeklyFuelPricesLoader.Database.FuelPrice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Period")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Value")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("Period")
                        .IsUnique();

                    b.ToTable("FuelPrices");
                });
#pragma warning restore 612, 618
        }
    }
}
