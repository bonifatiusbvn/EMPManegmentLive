using EMPManagment.API;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.ExpenseMaster;
using EMPManegment.EntityModels.ViewModels.Invoice;
using EMPManegment.EntityModels.ViewModels.Models;
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

        public async Task<ApiResponseModel> AddPurchaseRequestDetails(PurchaseRequestModel AddPurchaseRequest)
        {
            ApiResponseModel response = new ApiResponseModel();
            try
            {
                var purchaseRequest = new TblPurchaseRequest()
                {
                    PrId = Guid.NewGuid(),
                    UserId = AddPurchaseRequest.UserId,
                    ProjectId = AddPurchaseRequest.ProjectId,
                    ProductId = AddPurchaseRequest.ProductId,
                    ProductName = AddPurchaseRequest.ProductName,
                    ProductTypeId = AddPurchaseRequest.ProductTypeId,
                    Quantity = AddPurchaseRequest.Quantity,
                    IsApproved = AddPurchaseRequest.IsApproved,
                    IsDeleted = false,
                    CreatedBy = AddPurchaseRequest.CreatedBy,
                    CreatedOn = DateTime.Now,
                };
                response.code = (int)HttpStatusCode.OK;
                response.message = "PurchaseRequest successfully created..!";
                Context.TblPurchaseRequests.Add(purchaseRequest);
                Context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        public async Task<ApiResponseModel> DeletePurchaseRequestDetails(Guid PrId)
        {
            {
                ApiResponseModel response = new ApiResponseModel();
                var PurchaseRequest = Context.TblPurchaseRequests.Where(a => a.PrId == PrId).FirstOrDefault();

                if (PurchaseRequest != null)
                {
                    PurchaseRequest.IsDeleted = true;
                    Context.TblPurchaseRequests.Update(PurchaseRequest);
                    Context.SaveChanges();
                    response.code = 200;
                    response.data = PurchaseRequest;
                    response.message = "PurchaseRequest is Deleted Successfully";
                }
                return response;
            }
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

        public async Task<ApiResponseModel> UpdatePurchaseRequestDetails(PurchaseRequestModel UpdatePurchaseRequest)
        {
            ApiResponseModel model = new ApiResponseModel();
            var purchaseRequest = Context.TblPurchaseRequests.Where(e => e.PrId == UpdatePurchaseRequest.PrId).FirstOrDefault();
            try
            {
                if (purchaseRequest != null)
                {
                    purchaseRequest.PrId = UpdatePurchaseRequest.PrId;
                    purchaseRequest.UserId = UpdatePurchaseRequest.UserId;
                    purchaseRequest.ProjectId = UpdatePurchaseRequest.ProjectId;
                    purchaseRequest.ProductId = UpdatePurchaseRequest.ProductId;
                    purchaseRequest.ProductName = UpdatePurchaseRequest.ProductName;
                    purchaseRequest.ProductTypeId = UpdatePurchaseRequest.ProductTypeId;
                    purchaseRequest.Quantity = UpdatePurchaseRequest.Quantity;
                    purchaseRequest.IsApproved = UpdatePurchaseRequest.IsApproved;
                }
                Context.TblPurchaseRequests.Update(purchaseRequest);
                Context.SaveChanges();
                model.code = 200;
                model.message = "Purchase Request Updated Successfully!";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }
    }
}
