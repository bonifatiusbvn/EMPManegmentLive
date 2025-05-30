using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.AGGridModels
{
    public class AGGridRequestModel
    {
        public int StartRow { get; set; }
        public int PageSize { get; set; }
        public List<FilterModel> filters { get; set; }
        public List<SortModel> SortModel { get; set; }
        public string SearchValue { get; set; }
        public string SearchType { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? Month { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public string? UserFilter { get; set; }
        public string? ProductTypeFilter { get; set; }
        public Guid? CompanyFilter { get; set; }
        public Guid? VendorFilter { get; set; }
        public Guid? ProjectFilter { get; set; }
        public int? PaymentType { get; set; }
        public Guid? UserId { get; set; }
        public Guid? RoleId { get; set; }
        public Guid? VendorFilter { get; set; }
        public string? ProjectFilter { get; set; }
        public string? TaskTypeFilter { get; set; }
        public string? TaskStatusFilter { get; set; }
        public string? FilterType { get; set; }
        public bool? Approvefilter { get; set; }
        public bool? UnapproveFilter { get; set; }
        public string? AccountFilter { get; set; }
    }

    public class SortModel
    {
        public string ColId { get; set; }
        public string Sort { get; set; }
    }

    public class FilterModel
    {
        public string ColId { get; set; }
        public string FilterValue { get; set; }
    }

    public class AGGridResponseModel<T>
    {
        public List<T> Data { get; set; }
        public int RecordsTotal { get; set; }

        public string ErrorMessage { get; set; }

    }
}
