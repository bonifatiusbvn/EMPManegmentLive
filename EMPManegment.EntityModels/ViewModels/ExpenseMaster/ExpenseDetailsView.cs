using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.ExpenseMaster
{
    public class ExpenseDetailsView
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public int ExpenseType { get; set; }
        public string ExpenseTypeName { get; set; } = null!;

        public int? PaymentType { get; set; }
        public string PaymentTypeName { get; set; } = null!;

        public string BillNumber { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime Date { get; set; }

        public decimal TotalAmount { get; set; }

        public string? Image { get; set; }

        public string Account { get; set; } = null!;

        public bool IsPaid { get; set; }

        public bool? IsApproved { get; set; }

        public Guid? ApprovedBy { get; set; }

        public string? ApprovedByName { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? CreatedBy { get; set; }
    }

    public class ExpenseRequestModel
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public int ExpenseType { get; set; }

        public int PaymentType { get; set; }

        public string BillNumber { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime Date { get; set; }

        public decimal TotalAmount { get; set; }

        public IFormFile? Image { get; set; }

        public string Account { get; set; } = null!;

        public bool IsPaid { get; set; }

        public bool IsApproved { get; set; }

        public Guid? ApprovedBy { get; set; }

        public string? ApprovedByName { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? CreatedBy { get; set; }
    }

    public class ApprovedExpense
    {
        public Guid Id { get; set; }

        public bool IsApproved { get; set; }

        public Guid? ApprovedBy { get; set; }

        public string? ApprovedByName { get; set; }
        public DateTime ApprovedDate { get; set; }

    }
}
