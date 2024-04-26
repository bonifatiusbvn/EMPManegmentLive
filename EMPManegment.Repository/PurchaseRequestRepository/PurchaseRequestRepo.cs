using EMPManagment.API;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.ExpenseMaster;
using EMPManegment.EntityModels.ViewModels.Invoice;
using EMPManegment.EntityModels.ViewModels.Purchase_Request;
using EMPManegment.Inretface.Interface.PurchaseRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Repository.PurchaseRequestRepository
{
    public class PurchaseRequestRepo : IPurchaseRequest
    {
        public PurchaseRequestRepo(BonifatiusEmployeesContext context)
        {
            Context = context;
        }
        public BonifatiusEmployeesContext Context { get; }

        public async Task<ApiResponseModel> AddPurchaseRequestDetails(PurchaseRequestModel PurchaseRequestDetails)
        {
            ApiResponseModel response = new ApiResponseModel();
            try
            {
                var PurchaseRequest = new TblPurchaseRequest()
                {
                    PrId = Guid.NewGuid(),
                    UserId = PurchaseRequestDetails.UserId,
                    ProjectId = PurchaseRequestDetails.ProjectId,
                    ProductId = PurchaseRequestDetails.ProductId,
                    ProductName = PurchaseRequestDetails.ProductName,
                    ProductTypeId = PurchaseRequestDetails.ProductTypeId,
                    Quantity = PurchaseRequestDetails.Quantity,
                    IsApproved = PurchaseRequestDetails.IsApproved,
                    IsDeleted = false,
                    CreatedBy = PurchaseRequestDetails.CreatedBy,
                    CreatedOn = DateTime.Now,
                };
                response.code = (int)HttpStatusCode.OK;
                response.message = "PurchaseRequest successfully created..!";
                Context.TblPurchaseRequests.Add(PurchaseRequest);
                Context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        public async Task<PurchaseRequestModel> GetPurchaseRequestDetailsById(Guid PrId)
        {
            PurchaseRequestModel purchaseRequestList = new PurchaseRequestModel();
            try
            {
                purchaseRequestList = (from a in Context.TblPurchaseRequests.Where(x => x.PrId == PrId)
                                       join b in Context.TblUsers on a.UserId equals b.Id
                                       join c in Context.TblProjectMasters on a.ProjectId equals c.ProjectId
                                       select new PurchaseRequestModel
                                       {
                                           PrId = a.PrId,
                                           UserId = a.UserId,
                                           UserName = b.UserName,
                                           ProjectId = a.ProjectId,
                                           ProductId = a.ProductId,
                                           ProjectName = c.ProjectTitle,
                                           ProductName = a.ProductName,
                                           ProductTypeId = a.ProductTypeId,
                                           Quantity = a.Quantity,
                                           IsApproved = a.IsApproved,
                                           CreatedBy = a.CreatedBy,
                                           CreatedOn = a.CreatedOn,
                                       }).First();
                return purchaseRequestList;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IEnumerable<PurchaseRequestModel>> GetPurchaseRequestList()
        {
            try
            {
                var purchaseRequest = from a in Context.TblPurchaseRequests
                                          join b in Context.TblUsers on a.UserId equals b.Id
                                          join c in Context.TblProjectMasters on a.ProjectId equals c.ProjectId
                                          select new PurchaseRequestModel
                                          {
                                              PrId = a.PrId,
                                              UserId = a.UserId,
                                              UserName = b.UserName,
                                              ProjectId = a.ProjectId,
                                              ProjectName = c.ProjectTitle,
                                              ProductId = a.ProductId,
                                              ProductName = a.ProductName,
                                              ProductTypeId = a.ProductTypeId,
                                              Quantity = a.Quantity,
                                              IsApproved = a.IsApproved,
                                              IsDeleted = a.IsDeleted,
                                              CreatedBy = a.CreatedBy,
                                              CreatedOn = a.CreatedOn,
                                          };
                return purchaseRequest;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
