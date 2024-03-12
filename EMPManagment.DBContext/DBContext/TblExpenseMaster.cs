using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblExpenseMaster
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public int ExpenseType { get; set; }

    public int? PaymentType { get; set; }

    public string BillNumber { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime Date { get; set; }

    public decimal TotalAmount { get; set; }

    public string? Image { get; set; }

    public string Account { get; set; } = null!;

    public bool IsPaid { get; set; }

    public bool IsApproved { get; set; }
    public DateTime ApprovedDate { get; set; }

    public Guid? ApprovedBy { get; set; }

    public string? ApprovedByName { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public virtual TblExpenseType ExpenseTypeNavigation { get; set; } = null!;

    public virtual TblPaymentType? PaymentTypeNavigation { get; set; }

    public virtual TblUser User { get; set; } = null!;
}
