using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblUser
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public string Email { get; set; } = null!;

    public bool? EmailConfirmed { get; set; }

    public DateTime? JoiningDate { get; set; }

    public string? Designation { get; set; }

    public string? Address { get; set; }

    public int? CityId { get; set; }

    public int? StateId { get; set; }

    public int? CountryId { get; set; }

    public string? Pincode { get; set; }

    public byte[] PasswordHash { get; set; } = null!;

    public byte[] PasswordSalt { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public bool? PhoneNumberConfirmed { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? LastLoginDate { get; set; }

    public Guid? RoleId { get; set; }

    public int? QuestionId { get; set; }

    public string? Answer { get; set; }

    public string? Image { get; set; }

    public int? DepartmentId { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual TblCity? City { get; set; }

    public virtual TblCountry? Country { get; set; }

    public virtual TblDepartment? Department { get; set; }

    public virtual TblQuestion? Question { get; set; }

    public virtual TblRoleMaster? Role { get; set; }

    public virtual TblState? State { get; set; }

    public virtual ICollection<TblAttendance> TblAttendances { get; set; } = new List<TblAttendance>();

    public virtual ICollection<TblExpenseMaster> TblExpenseMasters { get; set; } = new List<TblExpenseMaster>();

    public virtual ICollection<TblPageMaster> TblPageMasters { get; set; } = new List<TblPageMaster>();

    public virtual ICollection<TblProjectDocument> TblProjectDocuments { get; set; } = new List<TblProjectDocument>();

    public virtual ICollection<TblProjectMember> TblProjectMembers { get; set; } = new List<TblProjectMember>();

    public virtual ICollection<TblPurchaseRequest> TblPurchaseRequests { get; set; } = new List<TblPurchaseRequest>();

    public virtual ICollection<TblTaskDetail> TblTaskDetails { get; set; } = new List<TblTaskDetail>();

    public virtual ICollection<TblUserDocument> TblUserDocuments { get; set; } = new List<TblUserDocument>();

    public virtual ICollection<TblUserFormPermission> TblUserFormPermissions { get; set; } = new List<TblUserFormPermission>();
}
