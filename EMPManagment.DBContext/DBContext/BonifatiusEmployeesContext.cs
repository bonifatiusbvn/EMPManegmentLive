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

    public virtual DbSet<OrderMaster> OrderMasters { get; set; }

    public virtual DbSet<TblAttendance> TblAttendances { get; set; }

    public virtual DbSet<TblCity> TblCities { get; set; }

    public virtual DbSet<TblCompanyMaster> TblCompanyMasters { get; set; }

    public virtual DbSet<TblCountry> TblCountries { get; set; }

    public virtual DbSet<TblDepartment> TblDepartments { get; set; }

    public virtual DbSet<TblDocumentMaster> TblDocumentMasters { get; set; }

    public virtual DbSet<TblExpenseMaster> TblExpenseMasters { get; set; }

    public virtual DbSet<TblExpenseType> TblExpenseTypes { get; set; }

    public virtual DbSet<TblInvoice> TblInvoices { get; set; }

    public virtual DbSet<TblInvoiceTypeMaster> TblInvoiceTypeMasters { get; set; }

    public virtual DbSet<TblPageMaster> TblPageMasters { get; set; }

    public virtual DbSet<TblPaymentType> TblPaymentTypes { get; set; }

    public virtual DbSet<TblProductDetailsMaster> TblProductDetailsMasters { get; set; }

    public virtual DbSet<TblProductTypeMaster> TblProductTypeMasters { get; set; }

    public virtual DbSet<TblProjectDetail> TblProjectDetails { get; set; }

    public virtual DbSet<TblProjectDocument> TblProjectDocuments { get; set; }

    public virtual DbSet<TblProjectMaster> TblProjectMasters { get; set; }

    public virtual DbSet<TblQuestion> TblQuestions { get; set; }

    public virtual DbSet<TblRoleMaster> TblRoleMasters { get; set; }

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
        modelBuilder.Entity<OrderMaster>(entity =>
        {
            entity.ToTable("OrderMaster");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.CompanyName).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DeliveryDate).HasColumnType("date");
            entity.Property(e => e.DeliveryStatus).HasMaxLength(50);
            entity.Property(e => e.OrderDate).HasColumnType("date");
            entity.Property(e => e.OrderId).HasMaxLength(50);
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.PaymentStatus).HasMaxLength(50);
            entity.Property(e => e.ProductName).HasMaxLength(50);
            entity.Property(e => e.ProductShortDescription).HasMaxLength(50);
            entity.Property(e => e.Quantity).HasMaxLength(50);
            entity.Property(e => e.Total).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Type).HasMaxLength(20);

            entity.HasOne(d => d.Product).WithMany(p => p.OrderMasters)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_OrderMaster_tblProductDetailsMaster");

            entity.HasOne(d => d.Project).WithMany(p => p.OrderMasters)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK_OrderMaster_tblProjectMaster");
        });

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

        modelBuilder.Entity<TblCompanyMaster>(entity =>
        {
            entity.ToTable("tblCompanyMaster");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.CompnyName).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("date");
            entity.Property(e => e.Gst)
                .HasMaxLength(20)
                .HasColumnName("GST");
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

        modelBuilder.Entity<TblExpenseMaster>(entity =>
        {
            entity.ToTable("tblExpenseMaster");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Account).HasMaxLength(20);
            entity.Property(e => e.ApprovedByName).HasMaxLength(50);
            entity.Property(e => e.BillNumber).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Image).HasMaxLength(50);
            entity.Property(e => e.TotalAmount).HasColumnType("numeric(18, 2)");

            entity.HasOne(d => d.ExpenseTypeNavigation).WithMany(p => p.TblExpenseMasters)
                .HasForeignKey(d => d.ExpenseType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblExpenseMaster_tblExpenseType");

            entity.HasOne(d => d.PaymentTypeNavigation).WithMany(p => p.TblExpenseMasters)
                .HasForeignKey(d => d.PaymentType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblExpenseMaster_tblPaymentType");

            entity.HasOne(d => d.User).WithMany(p => p.TblExpenseMasters)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblExpenseMaster_tblUsers1");
        });

        modelBuilder.Entity<TblExpenseType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ExpenseType");

            entity.ToTable("tblExpenseType");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Type).HasMaxLength(30);
        });

        modelBuilder.Entity<TblInvoice>(entity =>
        {
            entity.ToTable("tblInvoices");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.BuyesOrderDate).HasColumnType("date");
            entity.Property(e => e.BuyesOrderNo).HasMaxLength(100);
            entity.Property(e => e.Cgst)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("CGST");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Destination).HasMaxLength(50);
            entity.Property(e => e.DispatchThrough).HasMaxLength(20);
            entity.Property(e => e.Igst)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("IGST");
            entity.Property(e => e.InvoiceDate).HasColumnType("date");
            entity.Property(e => e.InvoiceNo).HasMaxLength(100);
            entity.Property(e => e.OrderId).HasMaxLength(50);
            entity.Property(e => e.Sgst)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("SGST");
            entity.Property(e => e.TotalAmount).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.TotalGst)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("TotalGST");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblInvoiceTypeMaster>(entity =>
        {
            entity.ToTable("tblInvoiceTypeMaster");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InvoiceType).HasMaxLength(10);
        });

        modelBuilder.Entity<TblPageMaster>(entity =>
        {
            entity.ToTable("tblPageMaster");

            entity.Property(e => e.PageName).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.TblPageMasters)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_tblPageMaster_tblUsers");
        });

        modelBuilder.Entity<TblPaymentType>(entity =>
        {
            entity.ToTable("tblPaymentType");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Type).HasMaxLength(30);
        });

        modelBuilder.Entity<TblProductDetailsMaster>(entity =>
        {
            entity.ToTable("tblProductDetailsMaster");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Gst)
                .HasColumnType("numeric(3, 2)")
                .HasColumnName("GST");
            entity.Property(e => e.Hsn).HasColumnName("HSN");
            entity.Property(e => e.PerUnitPrice).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.PerUnitWithGstprice)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("PerUnitWithGSTPrice");
            entity.Property(e => e.ProductImage).HasMaxLength(50);
            entity.Property(e => e.ProductName).HasMaxLength(50);
            entity.Property(e => e.ProductShortDescription).HasMaxLength(50);
            entity.Property(e => e.ProductStocks).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblProductTypeMaster>(entity =>
        {
            entity.ToTable("tblProductTypeMaster");

            entity.Property(e => e.Type).HasMaxLength(20);
        });

        modelBuilder.Entity<TblProjectDetail>(entity =>
        {
            entity.ToTable("tblProjectDetails");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedOn).HasColumnType("date");
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.ProjectTitle).HasMaxLength(50);
            entity.Property(e => e.ProjectType).HasMaxLength(20);
            entity.Property(e => e.StartDate).HasColumnType("date");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.UserRole).HasMaxLength(20);

            entity.HasOne(d => d.Project).WithMany(p => p.TblProjectDetails)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK_tblProjectDetails_tblProjectMaster");

            entity.HasOne(d => d.User).WithMany(p => p.TblProjectDetails)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_tblProjectDetails_tblUsers");
        });

        modelBuilder.Entity<TblProjectDocument>(entity =>
        {
            entity.ToTable("tblProjectDocuments");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreadetOn).HasColumnType("datetime");
            entity.Property(e => e.Date).HasColumnType("date");

            entity.HasOne(d => d.Project).WithMany(p => p.TblProjectDocuments)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblProjectDocuments_tblProjectMaster");

            entity.HasOne(d => d.User).WithMany(p => p.TblProjectDocuments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblProjectDocuments_tblUsers");
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

        modelBuilder.Entity<TblRoleMaster>(entity =>
        {
            entity.ToTable("tblRoleMaster");

            entity.Property(e => e.Role).HasMaxLength(20);
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
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.IsCompleted).HasMaxLength(20);
            entity.Property(e => e.TaskDate).HasColumnType("datetime");
            entity.Property(e => e.TaskEndDate).HasColumnType("datetime");
            entity.Property(e => e.TaskStatus).HasMaxLength(20);
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
            entity.Property(e => e.Designation).HasMaxLength(20);
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

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.TblUsers)
                .HasForeignKey(d => d.Role)
                .HasConstraintName("FK_tblUsers_tblRoleMaster");

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
            entity.HasKey(e => e.Vid).HasName("PK_tblVendor_Master_1");

            entity.ToTable("tblVendor_Master");

            entity.Property(e => e.Vid)
                .ValueGeneratedNever()
                .HasColumnName("VId");
            entity.Property(e => e.CreatedBy).HasMaxLength(10);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.VendorAccountHolderName).HasMaxLength(20);
            entity.Property(e => e.VendorAddress).HasMaxLength(100);
            entity.Property(e => e.VendorBankAccountNo).HasMaxLength(50);
            entity.Property(e => e.VendorBankBranch).HasMaxLength(50);
            entity.Property(e => e.VendorBankIfsc)
                .HasMaxLength(50)
                .HasColumnName("VendorBankIFSC");
            entity.Property(e => e.VendorBankName).HasMaxLength(50);
            entity.Property(e => e.VendorCompany).HasMaxLength(50);
            entity.Property(e => e.VendorCompanyEmail).HasMaxLength(50);
            entity.Property(e => e.VendorCompanyLogo).HasMaxLength(15);
            entity.Property(e => e.VendorCompanyNumber).HasMaxLength(15);
            entity.Property(e => e.VendorCompanyType).HasMaxLength(20);
            entity.Property(e => e.VendorContact).HasMaxLength(12);
            entity.Property(e => e.VendorEmail).HasMaxLength(50);
            entity.Property(e => e.VendorFirstName).HasMaxLength(20);
            entity.Property(e => e.VendorGstnumber)
                .HasMaxLength(50)
                .HasColumnName("VendorGSTNumber");
            entity.Property(e => e.VendorLastName).HasMaxLength(20);
            entity.Property(e => e.VendorPhone).HasMaxLength(15);
            entity.Property(e => e.VendorPinCode).HasMaxLength(10);

            entity.HasOne(d => d.VendorCityNavigation).WithMany(p => p.TblVendorMasters)
                .HasForeignKey(d => d.VendorCity)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblVendor_Master_tblCity");

            entity.HasOne(d => d.VendorCountryNavigation).WithMany(p => p.TblVendorMasters)
                .HasForeignKey(d => d.VendorCountry)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblVendor_Master_tblCountry");

            entity.HasOne(d => d.VendorStateNavigation).WithMany(p => p.TblVendorMasters)
                .HasForeignKey(d => d.VendorState)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblVendor_Master_tblState");

            entity.HasOne(d => d.VendorType).WithMany(p => p.TblVendorMasters)
                .HasForeignKey(d => d.VendorTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblVendor_Master_tblVendorType");
        });

        modelBuilder.Entity<TblVendorType>(entity =>
        {
            entity.ToTable("tblVendorType");

            entity.Property(e => e.VendorName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
