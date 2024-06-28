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

    public virtual DbSet<TblCompanyMaster> TblCompanyMasters { get; set; }

    public virtual DbSet<TblCountry> TblCountries { get; set; }

    public virtual DbSet<TblCreditDebitMaster> TblCreditDebitMasters { get; set; }

    public virtual DbSet<TblDepartment> TblDepartments { get; set; }

    public virtual DbSet<TblDocumentMaster> TblDocumentMasters { get; set; }

    public virtual DbSet<TblExpenseMaster> TblExpenseMasters { get; set; }

    public virtual DbSet<TblExpenseType> TblExpenseTypes { get; set; }

    public virtual DbSet<TblForm> TblForms { get; set; }

    public virtual DbSet<TblInvoice> TblInvoices { get; set; }

    public virtual DbSet<TblInvoiceDetail> TblInvoiceDetails { get; set; }

    public virtual DbSet<TblInvoiceTypeMaster> TblInvoiceTypeMasters { get; set; }

    public virtual DbSet<TblManualInvoice> TblManualInvoices { get; set; }

    public virtual DbSet<TblManualInvoiceDetail> TblManualInvoiceDetails { get; set; }

    public virtual DbSet<TblPageMaster> TblPageMasters { get; set; }

    public virtual DbSet<TblPaymentMethodType> TblPaymentMethodTypes { get; set; }

    public virtual DbSet<TblPaymentType> TblPaymentTypes { get; set; }

    public virtual DbSet<TblPodeliveryAddress> TblPodeliveryAddresses { get; set; }

    public virtual DbSet<TblProductDetailsMaster> TblProductDetailsMasters { get; set; }

    public virtual DbSet<TblProductTypeMaster> TblProductTypeMasters { get; set; }

    public virtual DbSet<TblProjectDocument> TblProjectDocuments { get; set; }

    public virtual DbSet<TblProjectMaster> TblProjectMasters { get; set; }

    public virtual DbSet<TblProjectMember> TblProjectMembers { get; set; }

    public virtual DbSet<TblPurchaseOrderDetail> TblPurchaseOrderDetails { get; set; }

    public virtual DbSet<TblPurchaseOrderMaster> TblPurchaseOrderMasters { get; set; }

    public virtual DbSet<TblPurchaseRequest> TblPurchaseRequests { get; set; }

    public virtual DbSet<TblQuestion> TblQuestions { get; set; }

    public virtual DbSet<TblRoleMaster> TblRoleMasters { get; set; }

    public virtual DbSet<TblRolewiseFormPermission> TblRolewiseFormPermissions { get; set; }

    public virtual DbSet<TblSalarySlip> TblSalarySlips { get; set; }

    public virtual DbSet<TblState> TblStates { get; set; }

    public virtual DbSet<TblTaskDetail> TblTaskDetails { get; set; }

    public virtual DbSet<TblTaskMaster> TblTaskMasters { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    public virtual DbSet<TblUserDocument> TblUserDocuments { get; set; }

    public virtual DbSet<TblUserFormPermission> TblUserFormPermissions { get; set; }

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
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

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
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.CompnyName).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("date");
            entity.Property(e => e.Gst)
                .HasMaxLength(20)
                .HasColumnName("GST");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblCountry>(entity =>
        {
            entity.ToTable("tblCountry");

            entity.Property(e => e.Country).HasMaxLength(30);
            entity.Property(e => e.CountryCode).HasMaxLength(10);
        });

        modelBuilder.Entity<TblCreditDebitMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tblPaymentDetailMaster");

            entity.ToTable("tblCreditDebitMaster");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.CreditDebitAmount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.InvoiceNo).HasMaxLength(100);
            entity.Property(e => e.PendingAmount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Type).HasMaxLength(10);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.PaymentMethodNavigation).WithMany(p => p.TblCreditDebitMasters)
                .HasForeignKey(d => d.PaymentMethod)
                .HasConstraintName("FK_tblCreditDebitMaster_tblPaymentMethodType");

            entity.HasOne(d => d.PaymentTypeNavigation).WithMany(p => p.TblCreditDebitMasters)
                .HasForeignKey(d => d.PaymentType)
                .HasConstraintName("FK_tblCreditDebitMaster_tblPaymentType");

            entity.HasOne(d => d.Vendor).WithMany(p => p.TblCreditDebitMasters)
                .HasForeignKey(d => d.VendorId)
                .HasConstraintName("FK_tblCreditDebitMaster_tblVendor_Master");
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
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblExpenseMaster>(entity =>
        {
            entity.ToTable("tblExpenseMaster");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Account).HasMaxLength(20);
            entity.Property(e => e.ApprovedByName).HasMaxLength(50);
            entity.Property(e => e.ApprovedDate).HasColumnType("datetime");
            entity.Property(e => e.BillNumber).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.TotalAmount).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.ExpenseTypeNavigation).WithMany(p => p.TblExpenseMasters)
                .HasForeignKey(d => d.ExpenseType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblExpenseMaster_tblExpenseType");

            entity.HasOne(d => d.PaymentTypeNavigation).WithMany(p => p.TblExpenseMasters)
                .HasForeignKey(d => d.PaymentType)
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
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblForm>(entity =>
        {
            entity.HasKey(e => e.FormId);

            entity.ToTable("tblForm");

            entity.Property(e => e.Action).HasMaxLength(50);
            entity.Property(e => e.Controller).HasMaxLength(50);
            entity.Property(e => e.FormGroup).HasMaxLength(50);
            entity.Property(e => e.FormName).HasMaxLength(50);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
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
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.DispatchThrough).HasMaxLength(20);
            entity.Property(e => e.Igst)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("IGST");
            entity.Property(e => e.InvoiceDate).HasColumnType("date");
            entity.Property(e => e.InvoiceNo).HasMaxLength(100);
            entity.Property(e => e.InvoiceType).HasMaxLength(20);
            entity.Property(e => e.OrderId).HasMaxLength(50);
            entity.Property(e => e.RoundOff).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Sgst)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("SGST");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TotalAmount).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.TotalDiscount).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.TotalGst)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("TotalGST");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Company).WithMany(p => p.TblInvoices)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK_tblInvoices_tblCompanyMaster");

            entity.HasOne(d => d.PaymentMethodNavigation).WithMany(p => p.TblInvoices)
                .HasForeignKey(d => d.PaymentMethod)
                .HasConstraintName("FK_tblInvoices_tblPaymentMethodType");

            entity.HasOne(d => d.PaymentStatusNavigation).WithMany(p => p.TblInvoices)
                .HasForeignKey(d => d.PaymentStatus)
                .HasConstraintName("FK_tblInvoices_tblPaymentType");

            entity.HasOne(d => d.Project).WithMany(p => p.TblInvoices)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK_tblInvoices_tblProjectMaster");
        });

        modelBuilder.Entity<TblInvoiceDetail>(entity =>
        {
            entity.ToTable("tblInvoiceDetails");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DiscountAmount).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.DiscountPer).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.GstAmount).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.GstPer).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Hsn).HasColumnName("HSN");
            entity.Property(e => e.Price).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Product).HasMaxLength(100);
            entity.Property(e => e.ProductTotal).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Quantity).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.InvoiceRef).WithMany(p => p.TblInvoiceDetails)
                .HasForeignKey(d => d.InvoiceRefId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblInvoiceDetails_tblInvoices");
        });

        modelBuilder.Entity<TblInvoiceTypeMaster>(entity =>
        {
            entity.ToTable("tblInvoiceTypeMaster");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.InvoiceType).HasMaxLength(10);
        });

        modelBuilder.Entity<TblManualInvoice>(entity =>
        {
            entity.ToTable("tblManualInvoices");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.BuyesOrderDate).HasColumnType("date");
            entity.Property(e => e.BuyesOrderNo).HasMaxLength(100);
            entity.Property(e => e.Cgst)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("CGST");
            entity.Property(e => e.CompanyGstNo).HasMaxLength(50);
            entity.Property(e => e.CompanyName).HasMaxLength(150);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DispatchThrough).HasMaxLength(20);
            entity.Property(e => e.Igst)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("IGST");
            entity.Property(e => e.InvoiceDate).HasColumnType("date");
            entity.Property(e => e.InvoiceNo).HasMaxLength(100);
            entity.Property(e => e.RoundOff).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Sgst)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("SGST");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TotalAmount).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.TotalGst)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("TotalGST");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.VendorGstNo).HasMaxLength(50);
            entity.Property(e => e.VendorName).HasMaxLength(150);
            entity.Property(e => e.VendorPhoneNo).HasMaxLength(12);
        });

        modelBuilder.Entity<TblManualInvoiceDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tblManualInvoiceDetails_1");

            entity.ToTable("tblManualInvoiceDetails");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Discount).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.DiscountPercent).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Gst).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.GstAmount).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Hsn).HasColumnName("HSN");
            entity.Property(e => e.Price).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Product).HasMaxLength(100);
            entity.Property(e => e.ProductTotal).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Quantity).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblPageMaster>(entity =>
        {
            entity.ToTable("tblPageMaster");

            entity.Property(e => e.PageName).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.TblPageMasters)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_tblPageMaster_tblUsers");
        });

        modelBuilder.Entity<TblPaymentMethodType>(entity =>
        {
            entity.ToTable("tblPaymentMethodType");

            entity.Property(e => e.PaymentMethod).HasMaxLength(15);
        });

        modelBuilder.Entity<TblPaymentType>(entity =>
        {
            entity.ToTable("tblPaymentType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Type).HasMaxLength(30);
        });

        modelBuilder.Entity<TblPodeliveryAddress>(entity =>
        {
            entity.HasKey(e => e.Aid).HasName("PK_PODeliveryAddress");

            entity.ToTable("tblPODeliveryAddress");

            entity.Property(e => e.Aid).HasColumnName("AId");
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.Poid).HasColumnName("POId");
        });

        modelBuilder.Entity<TblProductDetailsMaster>(entity =>
        {
            entity.ToTable("tblProductDetailsMaster");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.GstAmount).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.GstPercentage).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Hsn).HasColumnName("HSN");
            entity.Property(e => e.PerUnitPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.ProductShortDescription).HasMaxLength(100);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblProductTypeMaster>(entity =>
        {
            entity.ToTable("tblProductTypeMaster");

            entity.Property(e => e.Type).HasMaxLength(20);
        });

        modelBuilder.Entity<TblProjectDocument>(entity =>
        {
            entity.ToTable("tblProjectDocuments");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreadetOn).HasColumnType("datetime");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

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
            entity.Property(e => e.Area).HasMaxLength(250);
            entity.Property(e => e.BuildingName).HasMaxLength(100);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.PinCode).HasMaxLength(7);
            entity.Property(e => e.ProjectDeadline).HasColumnType("date");
            entity.Property(e => e.ProjectEndDate).HasColumnType("date");
            entity.Property(e => e.ProjectHead).HasMaxLength(50);
            entity.Property(e => e.ProjectImage).HasMaxLength(500);
            entity.Property(e => e.ProjectPath).HasMaxLength(500);
            entity.Property(e => e.ProjectPriority).HasMaxLength(10);
            entity.Property(e => e.ProjectStartDate).HasColumnType("date");
            entity.Property(e => e.ProjectStatus).HasMaxLength(10);
            entity.Property(e => e.ProjectTitle).HasMaxLength(50);
            entity.Property(e => e.ProjectType).HasMaxLength(20);
            entity.Property(e => e.ShortName).HasMaxLength(50);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblProjectMember>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tblProjectDetails");

            entity.ToTable("tblProjectMembers");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedOn).HasColumnType("date");
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.ProjectTitle).HasMaxLength(50);
            entity.Property(e => e.ProjectType).HasMaxLength(20);
            entity.Property(e => e.StartDate).HasColumnType("date");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.UserRole).HasMaxLength(20);

            entity.HasOne(d => d.Project).WithMany(p => p.TblProjectMembers)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK_tblProjectDetails_tblProjectMaster");

            entity.HasOne(d => d.User).WithMany(p => p.TblProjectMembers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_tblProjectDetails_tblUsers");
        });

        modelBuilder.Entity<TblPurchaseOrderDetail>(entity =>
        {
            entity.ToTable("tblPurchaseOrderDetails");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Gstamount)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("GSTAmount");
            entity.Property(e => e.Gstper)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("GSTPer");
            entity.Property(e => e.Hsn).HasColumnName("HSN");
            entity.Property(e => e.PorefId).HasColumnName("PORefId");
            entity.Property(e => e.Price).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.ProductTotal).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Quantity).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Poref).WithMany(p => p.TblPurchaseOrderDetails)
                .HasForeignKey(d => d.PorefId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblPurchaseOrderDetails_tblPurchaseOrderMaster1");

            entity.HasOne(d => d.Product).WithMany(p => p.TblPurchaseOrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblPurchaseOrderDetails_tblPurchaseOrderMaster");

            entity.HasOne(d => d.ProductTypeNavigation).WithMany(p => p.TblPurchaseOrderDetails)
                .HasForeignKey(d => d.ProductType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblPurchaseOrderDetails_tblProductDetailsMaster");
        });

        modelBuilder.Entity<TblPurchaseOrderMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_OrderMaster");

            entity.ToTable("tblPurchaseOrderMaster");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DeliveryDate).HasColumnType("date");
            entity.Property(e => e.DeliveryStatus).HasMaxLength(50);
            entity.Property(e => e.OrderDate).HasColumnType("date");
            entity.Property(e => e.OrderId).HasMaxLength(50);
            entity.Property(e => e.OrderStatus).HasMaxLength(20);
            entity.Property(e => e.SubTotal).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TotalGst).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.PaymentMethodNavigation).WithMany(p => p.TblPurchaseOrderMasters)
                .HasForeignKey(d => d.PaymentMethod)
                .HasConstraintName("FK_tblOrderMaster_tblPaymentMethodType");

            entity.HasOne(d => d.PaymentStatusNavigation).WithMany(p => p.TblPurchaseOrderMasters)
                .HasForeignKey(d => d.PaymentStatus)
                .HasConstraintName("FK_tblPurchaseOrderMaster_tblPaymentType");
        });

        modelBuilder.Entity<TblPurchaseRequest>(entity =>
        {
            entity.HasKey(e => e.PrId);

            entity.ToTable("tblPurchaseRequest");

            entity.Property(e => e.PrId).ValueGeneratedNever();
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.PrDate).HasColumnType("date");
            entity.Property(e => e.PrNo).HasMaxLength(50);
            entity.Property(e => e.ProductName).HasMaxLength(250);
            entity.Property(e => e.Quantity).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Product).WithMany(p => p.TblPurchaseRequests)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_tblPurchaseRequest_tblProductDetailsMaster");

            entity.HasOne(d => d.ProductType).WithMany(p => p.TblPurchaseRequests)
                .HasForeignKey(d => d.ProductTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblPurchaseRequest_tblProductTypeMaster");

            entity.HasOne(d => d.Project).WithMany(p => p.TblPurchaseRequests)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblPurchaseRequest_tblProjectMaster");

            entity.HasOne(d => d.User).WithMany(p => p.TblPurchaseRequests)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblPurchaseRequest_tblUsers");
        });

        modelBuilder.Entity<TblQuestion>(entity =>
        {
            entity.ToTable("tblQuestion");
        });

        modelBuilder.Entity<TblRoleMaster>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK_tblRoleMaster_1");

            entity.ToTable("tblRoleMaster");

            entity.Property(e => e.RoleId).ValueGeneratedNever();
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Role).HasMaxLength(20);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblRolewiseFormPermission>(entity =>
        {
            entity.ToTable("tblRolewiseFormPermission");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Form).WithMany(p => p.TblRolewiseFormPermissions)
                .HasForeignKey(d => d.FormId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblRolewiseFormPermission_tblForm1");
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
                .HasConstraintName("FK_tblState_tblCountry");
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
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Project).WithMany(p => p.TblTaskDetails)
                .HasForeignKey(d => d.ProjectId)
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
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
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

            entity.HasOne(d => d.Role).WithMany(p => p.TblUsers)
                .HasForeignKey(d => d.RoleId)
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
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.DocumentType).WithMany(p => p.TblUserDocuments)
                .HasForeignKey(d => d.DocumentTypeId)
                .HasConstraintName("FK_tblUserDocuments_tblDocumentMaster");

            entity.HasOne(d => d.User).WithMany(p => p.TblUserDocuments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_tblUserDocuments_tblUsers");
        });

        modelBuilder.Entity<TblUserFormPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tblUserFormPermissions_1");

            entity.ToTable("tblUserFormPermissions");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Form).WithMany(p => p.TblUserFormPermissions)
                .HasForeignKey(d => d.FormId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblUserFormPermissions_tblForm");

            entity.HasOne(d => d.User).WithMany(p => p.TblUserFormPermissions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblUserFormPermissions_tblUsers");
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
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
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
