﻿// <auto-generated />
using System;
using EMS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EMS.Migrations
{
    [DbContext(typeof(EMSDbContext))]
    [Migration("20241206143245_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("EMS.Models.Attendance", b =>
                {
                    b.Property<int>("AttendanceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("AttendanceID"));

                    b.Property<TimeOnly>("CheckInTime")
                        .HasColumnType("time(6)");

                    b.Property<TimeOnly>("CheckOutTime")
                        .HasColumnType("time(6)");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<int>("EmpID")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<TimeSpan>("TotalDuration")
                        .HasColumnType("time(6)");

                    b.HasKey("AttendanceID");

                    b.ToTable("Attendances");
                });

            modelBuilder.Entity("EMS.Models.Banks", b =>
                {
                    b.Property<int>("BankId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("BankId"));

                    b.Property<string>("BankName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("BankId");

                    b.ToTable("Banks");
                });

            modelBuilder.Entity("EMS.Models.Department", b =>
                {
                    b.Property<int>("DeptID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("DeptID"));

                    b.Property<string>("DeptName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("DeptID");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("EMS.Models.Designation", b =>
                {
                    b.Property<int>("DesignationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("DesignationID"));

                    b.Property<string>("DesignationName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("DesignationID");

                    b.ToTable("Designations");
                });

            modelBuilder.Entity("EMS.Models.EmpAuth", b =>
                {
                    b.Property<int>("EmpAuthID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("EmpAuthID"));

                    b.Property<int>("EmpID")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("EmpAuthID");

                    b.ToTable("EmpAuths");
                });

            modelBuilder.Entity("EMS.Models.EmpBank", b =>
                {
                    b.Property<int>("EmpBankID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("EmpBankID"));

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("BankName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("EmpID")
                        .HasColumnType("int");

                    b.Property<string>("IFSCCode")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("EmpBankID");

                    b.ToTable("EmpBanks");
                });

            modelBuilder.Entity("EMS.Models.Employee", b =>
                {
                    b.Property<int>("EmpID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("EmpID"));

                    b.Property<string>("AadhaarNumber")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("CAddress")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("CDistrict")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("CPinCode")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateOnly>("DOB")
                        .HasColumnType("date");

                    b.Property<DateOnly>("DOJ")
                        .HasColumnType("date");

                    b.Property<int>("DepartmentID")
                        .HasColumnType("int");

                    b.Property<int>("Designation")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("FatherName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("MaritalStatus")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Mobile1")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Mobile2")
                        .HasColumnType("longtext");

                    b.Property<string>("MotherName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("NickName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PAddress")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PDistrict")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PPinCode")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PanNumber")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Qualification")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("RoleID")
                        .HasColumnType("int");

                    b.Property<int>("WorkingLocation")
                        .HasColumnType("int");

                    b.Property<bool>("isActive")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("EmpID");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("EMS.Models.LnABalance", b =>
                {
                    b.Property<int>("LnABID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("LnABID"));

                    b.Property<double>("ClosingBalance")
                        .HasColumnType("double");

                    b.Property<int>("EmpID")
                        .HasColumnType("int");

                    b.Property<string>("MonthYear")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("varchar(7)");

                    b.Property<double>("OpeningBalance")
                        .HasColumnType("double");

                    b.Property<double>("TotalDeduction")
                        .HasColumnType("double");

                    b.HasKey("LnABID");

                    b.HasIndex("EmpID", "MonthYear", "TotalDeduction")
                        .IsUnique();

                    b.ToTable("LnABalances");
                });

            modelBuilder.Entity("EMS.Models.LoansandAdvance", b =>
                {
                    b.Property<int>("LnAID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("LnAID"));

                    b.Property<double>("AmountGiven")
                        .HasColumnType("double");

                    b.Property<double>("DeductionAmount")
                        .HasColumnType("double");

                    b.Property<int>("DeductionFrequencyID")
                        .HasColumnType("int");

                    b.Property<DateOnly>("DeductionStartDate")
                        .HasColumnType("date");

                    b.Property<int>("EmpID")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeductionComplete")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateOnly>("LnADate")
                        .HasColumnType("date");

                    b.Property<string>("MonthYear")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("isActive")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("LnAID");

                    b.ToTable("LoansandAdvances");
                });

            modelBuilder.Entity("EMS.Models.Location", b =>
                {
                    b.Property<int>("LocationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("LocationID"));

                    b.Property<string>("LocationName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("LocationID");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("EMS.Models.Payroll", b =>
                {
                    b.Property<int>("PrID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("PrID"));

                    b.Property<double>("BasicSalary")
                        .HasColumnType("double");

                    b.Property<double>("EPF")
                        .HasColumnType("double");

                    b.Property<double>("ESI")
                        .HasColumnType("double");

                    b.Property<int>("EmpID")
                        .HasColumnType("int");

                    b.Property<double>("HI")
                        .HasColumnType("double");

                    b.Property<double>("Increament")
                        .HasColumnType("double");

                    b.Property<DateOnly?>("IncreamentDate")
                        .HasColumnType("date");

                    b.Property<string>("MonthYear")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PayrollType")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<double>("RD")
                        .HasColumnType("double");

                    b.HasKey("PrID");

                    b.ToTable("Payrolls");
                });

            modelBuilder.Entity("EMS.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("EMS.Models.Uploads", b =>
                {
                    b.Property<int>("UPID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("UPID"));

                    b.Property<string>("AadharPath")
                        .HasColumnType("longtext");

                    b.Property<int>("EmpID")
                        .HasColumnType("int");

                    b.Property<string>("ImagePath")
                        .HasColumnType("longtext");

                    b.Property<string>("PanPath")
                        .HasColumnType("longtext");

                    b.Property<string>("PassbookPath")
                        .HasColumnType("longtext");

                    b.HasKey("UPID");

                    b.ToTable("Uploads");
                });
#pragma warning restore 612, 618
        }
    }
}