using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EMPManagment.API;

public partial class BonifatiusEmployeesContext : DbContext
{
    public BonifatiusEmployeesContext()
    {
    }

    public BonifatiusEmployeesContext(DbContextOptions<BonifatiusEmployeesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblAttendance> TblAttendances { get; set; }

    public virtual DbSet<TblCity> TblCities { get; set; }

    public virtual DbSet<TblCountry> TblCountries { get; set; }

    public virtual DbSet<TblDepartment> TblDepartments { get; set; }

    public virtual DbSet<TblDocumentMaster> TblDocumentMasters { get; set; }

    public virtual DbSet<TblPageMaster> TblPageMasters { get; set; }

    public virtual DbSet<TblProjectMaster> TblProjectMasters { get; set; }

    public virtual DbSet<TblQuestion> TblQuestions { get; set; }

    public virtual DbSet<TblSalarySlip> TblSalarySlips { get; set; }

    public virtual DbSet<TblState> TblStates { get; set; }

    public virtual DbSet<TblTaskDetail> TblTaskDetails { get; set; }

    public virtual DbSet<TblTaskMaster> TblTaskMasters { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    public virtual DbSet<TblUserDocument> TblUserDocuments { get; set; }

    public virtual DbSet<TblVendorMaster> TblVendorMasters { get; set; }

    public virtual DbSet<TblVendorType> TblVendorTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblAttendance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbl_BoniAttendance");

            entity.ToTable("tblAttendance");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Intime)
                .HasColumnType("datetime")
                .HasColumnName("INTime");
            entity.Property(e => e.OutTime).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.TblAttendances)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblAttendance_tblUsers");
        });

        modelBuilder.Entity<TblCity>(entity =>
        {
            entity.ToTable("tblCity");

            entity.HasIndex(e => e.StateId, "IX_tblCity_StateId");

            entity.Property(e => e.City).HasMaxLength(50);

            entity.HasOne(d => d.State).WithMany(p => p.TblCities)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblCity_tblState");
        });

        modelBuilder.Entity<TblCountry>(entity =>
        {
            entity.ToTable("tblCountry");

            entity.Property(e => e.Country).HasMaxLength(30);
        });

        modelBuilder.Entity<TblDepartment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbl_BoniDepartment");

            entity.ToTable("tblDepartment");

            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Department).HasMaxLength(50);
        });

        modelBuilder.Entity<TblDocumentMaster>(entity =>
        {
            entity.ToTable("tblDocumentMaster");

            entity.Property(e => e.DocumentType).HasMaxLength(20);
        });

        modelBuilder.Entity<TblPageMaster>(entity =>
        {
            entity.ToTable("tblPageMaster");

            entity.Property(e => e.PageName).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.TblPageMasters)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_tblPageMaster_tblUsers");
        });

        modelBuilder.Entity<TblProjectMaster>(entity =>
        {
            entity.HasKey(e => e.ProjectId);

            entity.ToTable("tblProjectMaster");

            entity.Property(e => e.ProjectId).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.ProjectDeadline).HasColumnType("date");
            entity.Property(e => e.ProjectEndDate).HasColumnType("date");
            entity.Property(e => e.ProjectHead).HasMaxLength(50);
            entity.Property(e => e.ProjectLocation).HasMaxLength(50);
            entity.Property(e => e.ProjectPriority).HasMaxLength(10);
            entity.Property(e => e.ProjectStartDate).HasColumnType("date");
            entity.Property(e => e.ProjectStatus).HasMaxLength(10);
            entity.Property(e => e.ProjectTitle).HasMaxLength(50);
            entity.Property(e => e.ProjectType).HasMaxLength(20);
        });

        modelBuilder.Entity<TblQuestion>(entity =>
        {
            entity.ToTable("tblQuestion");
        });

        modelBuilder.Entity<TblSalarySlip>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tblSalarySlip");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Month).HasColumnType("date");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblSalarySlip_tblUsers");
        });

        modelBuilder.Entity<TblState>(entity =>
        {
            entity.ToTable("tblState");

            entity.HasIndex(e => e.CountryId, "IX_tblState_CountryId");

            entity.Property(e => e.State).HasMaxLength(50);

            entity.HasOne(d => d.Country).WithMany(p => p.TblStates)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblState_tblCountry1");
        });

        modelBuilder.Entity<TblTaskDetail>(entity =>
        {
            entity.ToTable("tblTaskDetails");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.TaskDate).HasColumnType("datetime");
            entity.Property(e => e.TaskDetails).HasMaxLength(150);
            entity.Property(e => e.TaskEndDate).HasColumnType("datetime");
            entity.Property(e => e.TaskStatus).HasMaxLength(50);
            entity.Property(e => e.TaskTitle).HasMaxLength(50);

            entity.HasOne(d => d.TaskTypeNavigation).WithMany(p => p.TblTaskDetails)
                .HasForeignKey(d => d.TaskType)
                .HasConstraintName("FK_tblTaskDetails_tblTaskMaster");

            entity.HasOne(d => d.User).WithMany(p => p.TblTaskDetails)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_tblTaskDetails_tblUsers");
        });

        modelBuilder.Entity<TblTaskMaster>(entity =>
        {
            entity.ToTable("tblTaskMaster");

            entity.Property(e => e.TaskType).HasMaxLength(20);
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.ToTable("tblUsers");

            entity.HasIndex(e => e.CityId, "IX_tblUsers_CityId");

            entity.HasIndex(e => e.CountryId, "IX_tblUsers_CountryId");

            entity.HasIndex(e => e.DepartmentId, "IX_tblUsers_DepartmentId");

            entity.HasIndex(e => e.QuestionId, "IX_tblUsers_QuestionId");

            entity.HasIndex(e => e.StateId, "IX_tblUsers_StateId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(80);
            entity.Property(e => e.Answer).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DateOfBirth).HasColumnType("date");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(30);
            entity.Property(e => e.Gender).HasMaxLength(8);
            entity.Property(e => e.JoiningDate).HasColumnType("date");
            entity.Property(e => e.LastLoginDate).HasColumnType("datetime");
            entity.Property(e => e.LastName).HasMaxLength(30);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.Pincode).HasMaxLength(20);
            entity.Property(e => e.UserName).HasMaxLength(20);

            entity.HasOne(d => d.City).WithMany(p => p.TblUsers)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK_tblUsers_tblCity");

            entity.HasOne(d => d.Country).WithMany(p => p.TblUsers)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK_tblUsers_tblCountry");

            entity.HasOne(d => d.Department).WithMany(p => p.TblUsers)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK_tbl_BoniUsers_tbl_Departmen");

            entity.HasOne(d => d.Question).WithMany(p => p.TblUsers)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK_tblUsers_tblQuestion");

            entity.HasOne(d => d.State).WithMany(p => p.TblUsers)
                .HasForeignKey(d => d.StateId)
                .HasConstraintName("FK_tblUsers_tblState");
        });

        modelBuilder.Entity<TblUserDocument>(entity =>
        {
            entity.ToTable("tblUserDocuments");

            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.DocumentType).WithMany(p => p.TblUserDocuments)
                .HasForeignKey(d => d.DocumentTypeId)
                .HasConstraintName("FK_tblUserDocuments_tblDocumentMaster");

            entity.HasOne(d => d.User).WithMany(p => p.TblUserDocuments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_tblUserDocuments_tblUsers");
        });

        modelBuilder.Entity<TblVendorMaster>(entity =>
        {
            entity.ToTable("tblVendor_Master");

            entity.Property(e => e.CreatedBy).HasMaxLength(10);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.VendorAddress).HasMaxLength(100);
            entity.Property(e => e.VendorBankAccountNo).HasMaxLength(50);
            entity.Property(e => e.VendorBankIfsc)
                .HasMaxLength(50)
                .HasColumnName("VendorBankIFSC");
            entity.Property(e => e.VendorBankName).HasMaxLength(50);
            entity.Property(e => e.VendorEmail).HasMaxLength(50);
            entity.Property(e => e.VendorGstnumber)
                .HasMaxLength(50)
                .HasColumnName("VendorGSTNumber");
            entity.Property(e => e.VendorName).HasMaxLength(50);
            entity.Property(e => e.VendorPhone).HasMaxLength(50);

            entity.HasOne(d => d.VendorType).WithMany(p => p.TblVendorMasters)
                .HasForeignKey(d => d.VendorTypeId)
                .HasConstraintName("FK_tblVendor_Master_tblVendorType");
        });

        modelBuilder.Entity<TblVendorType>(entity =>
        {
            entity.ToTable("tblVendorType");

            entity.Property(e => e.VendorType).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
