using EMPManagment.API;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.ExpenseMaster;
using EMPManegment.EntityModels.ViewModels.Invoice;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.Purchase_Request;
using EMPManegment.EntityModels.ViewModels.UserModels;
using EMPManegment.Inretface.Interface.PurchaseRequest;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;
using Microsoft.AspNetCore.Mvc;
using EMPManegment.EntityModels.ViewModels.PurchaseOrderModels;
#nullable disable

namespace EMPManegment.Repository.PurchaseRequestRepository
{
    public class PurchaseRequestRepo : IPurchaseRequest
    {
        public PurchaseRequestRepo(BonifatiusEmployeesContext context)
        {
            Context = context;
        }
        public BonifatiusEmployeesContext Context { get; }

        public async Task<ApiResponseModel> CreatePurchaseRequest(PurchaseRequestMasterView AddPurchaseRequest)
        {
            ApiResponseModel response = new ApiResponseModel();
            try
            {

                foreach (var item in AddPurchaseRequest.PRList)
                {

                    var PRDetailS = new TblPurchaseRequest()
                    {
                        PrId = Guid.NewGuid(),
                        UserId = item.UserId,
                        ProjectId = item.ProjectId,
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        ProductTypeId = item.ProductTypeId,
                        Quantity = item.Quantity,
                        IsApproved = false,
                        IsDeleted = false,
                        CreatedBy = item.CreatedBy,
                        CreatedOn = DateTime.Now,
                        PrNo = item.PrNo,
                    };
                    Context.TblPurchaseRequests.Add(PRDetailS);
                }
                await Context.SaveChangesAsync();
                response.code = (int)HttpStatusCode.OK;
                response.message = "PurchaseRequest successfully created..!";
            }
            catch (Exception ex)
            {
                response.code = 500;
                response.message = "Error creating purchaseRequest ";
            }
            return response;
        }

        public async Task<ApiResponseModel> DeletePurchaseRequest(string PrNo)
        {
            {
                ApiResponseModel response = new ApiResponseModel();
                var GetPRdata = Context.TblPurchaseRequests.Where(a => a.PrNo == PrNo).ToList();

                if (GetPRdata.Any())
                {
                    foreach (var pr in GetPRdata)
                    {
                            pr.IsDeleted = true;
                            response.message = "Purchase request with PR No. " + PrNo + " is deleted successfully."; 
                    }

                    Context.TblPurchaseRequests.UpdateRange(GetPRdata);
                    await Context.SaveChangesAsync();

                    response.code = 200;
                    response.data = GetPRdata;
                }
                else
                {
                    response.code = 404;
                    response.message = "Purchase request with PR No. " + PrNo + " not found.";
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
                                          PrNo = a.PrNo,
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
                model.message = "Purchase request updated successfully!";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }

        public string CheckPRNo()
        {
            try
            {
                var LastPr = Context.TblPurchaseRequests.OrderByDescending(e => e.CreatedOn).FirstOrDefault();
                var currentDate = DateTime.Now;

                int currentYear;
                int lastYear;
                if (currentDate.Month > 4)
                {

                    currentYear = currentDate.Year + 1;
                    lastYear = currentDate.Year;
                }
                else
                {

                    currentYear = currentDate.Year;
                    lastYear = currentDate.Year - 1;
                }

                string PurchaseRequestId;
                if (LastPr == null)
                {

                    PurchaseRequestId = $"BTPL/PR/{(lastYear % 100).ToString("D2")}-{(currentYear % 100).ToString("D2")}/001";
                }
                else
                {
                    if (LastPr.PrNo.Length >= 17)
                    {

                        int PrNumber = int.Parse(LastPr.PrNo.Substring(16)) + 1;
                        PurchaseRequestId = $"BTPL/PR/{(lastYear % 100).ToString("D2")}-{(currentYear % 100).ToString("D2")}/" + PrNumber.ToString("D3");
                    }
                    else
                    {
                        throw new Exception("PurchaseRequest id does not have the expected format.");
                    }
                }
                return PurchaseRequestId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<jsonData> GetPRList(DataTableRequstModel PRdataTable)
        {
            var purchaseRequestList = from a in Context.TblPurchaseRequests
                                      join b in Context.TblUsers on a.UserId equals b.Id
                                      join c in Context.TblProjectMasters on a.ProjectId equals c.ProjectId
                                      orderby a.CreatedOn descending
                                      where a.IsDeleted == false
                                      group new { a, b, c } by new { a.UserId, b.Image, b.UserName, FullName = b.FirstName + " " + b.LastName, a.PrNo } into userGroup
                                      select new PurchaseRequestModel
                                      {
                                          PrId = userGroup.Select(x => x.a.PrId).FirstOrDefault(),
                                          UserId = userGroup.Key.UserId,
                                          UserName = userGroup.Key.UserName,
                                          FullName = userGroup.Key.FullName,
                                          ProjectId = userGroup.Select(x => x.a.ProjectId).FirstOrDefault(),
                                          ProjectName = userGroup.Select(x => x.c.ProjectTitle).FirstOrDefault(),
                                          ProductId = userGroup.Select(x => x.a.ProductId).FirstOrDefault(),
                                          ProductName = userGroup.Select(x => x.a.ProductName).FirstOrDefault(),
                                          ProductTypeId = userGroup.Select(x => x.a.ProductTypeId).FirstOrDefault(),
                                          Quantity = userGroup.Select(x => x.a.Quantity).FirstOrDefault(),
                                          IsApproved = userGroup.Select(x => x.a.IsApproved).FirstOrDefault(),
                                          IsDeleted = userGroup.Select(x => x.a.IsDeleted).FirstOrDefault(),
                                          CreatedBy = userGroup.Select(x => x.a.CreatedBy).FirstOrDefault(),
                                          CreatedOn = userGroup.Select(x => x.a.CreatedOn).FirstOrDefault(),
                                          PrNo = userGroup.Key.PrNo,
                                      };

            if (!string.IsNullOrEmpty(PRdataTable.sortColumn) && !string.IsNullOrEmpty(PRdataTable.sortColumnDir))
            {
                purchaseRequestList = purchaseRequestList.OrderBy(PRdataTable.sortColumn + " " + PRdataTable.sortColumnDir);
            }
            if (!string.IsNullOrEmpty(PRdataTable.searchValue))
            {
                purchaseRequestList = purchaseRequestList.Where(e => e.ProjectName.Contains(PRdataTable.searchValue) || e.PrNo.Contains(PRdataTable.searchValue) || e.ProductName.Contains(PRdataTable.searchValue) || e.FullName.Contains(PRdataTable.searchValue));
            }
            int totalRecord = purchaseRequestList.Count();
            var cData = purchaseRequestList.Skip(PRdataTable.skip).Take(PRdataTable.pageSize).ToList();

            jsonData jsonData = new jsonData
            {
                draw = PRdataTable.draw,
                recordsFiltered = totalRecord,
                recordsTotal = totalRecord,
                data = cData
            };
            return jsonData;
        }

        public async Task<PurchaseRequestMasterView> PurchaseRequestDetailsByPrNo(string PrNo)
        {
            PurchaseRequestMasterView PRDetails = new PurchaseRequestMasterView();
            try
            {
                PRDetails = (from a in Context.TblPurchaseRequests.Where(a => a.PrNo == PrNo)
                                                     select new PurchaseRequestMasterView
                                                     {
                                                         PrNo = PrNo,
                                                     }).FirstOrDefault();
                List<PurchaseRequestModel> PRList = (from a in Context.TblPurchaseRequests.Where(a => a.PrNo == PRDetails.PrNo)
                                                               join b in Context.TblProductDetailsMasters on a.ProductId equals b.Id
                                                               join c in Context.TblProductTypeMasters on a.ProductTypeId equals c.Id
                                                               join d in Context.TblUsers on a.UserId equals d.Id
                                                               join e in Context.TblProjectMasters on a.ProjectId equals e.ProjectId
                                                               select new PurchaseRequestModel
                                                               {
                                                                   ProductTypeId = a.ProductTypeId,
                                                                   ProductName = a.ProductName,
                                                                   ProductId = a.ProductId,
                                                                   ProductTypeName = c.Type,
                                                                   UserId = a.UserId,
                                                                   FullName = d.FirstName + " " + d.LastName,
                                                                   ProjectId = a.ProjectId,
                                                                   ProjectName = e.ProjectTitle,
                                                                   Quantity = a.Quantity,
                                                                   Price = b.PerUnitPrice,
                                                                   //PerUnitWithGstprice = b.PerUnitWithGstprice,
                                                                   ProductImage = b.ProductImage,
                                                                   ProductTotal = b.PerUnitPrice * a.Quantity,
                                                               }).ToList();
                PRDetails.PRList = PRList; 
                return PRDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<UserResponceModel> ApproveUnapprovePR(string PrNo)
        {
            UserResponceModel response = new UserResponceModel();
            var GetPRdata = Context.TblPurchaseRequests.Where(a => a.PrNo == PrNo).ToList();

            if (GetPRdata.Any())
            {
                foreach (var pr in GetPRdata)
                {
                    if (pr.IsApproved == true)
                    {
                        pr.IsApproved = false;
                        response.Message = "Purchase request with PR No. " + PrNo + " is deactivated successfully.";
                    }
                    else
                    {
                        pr.IsApproved = true;
                        response.Message = "Purchase request with PR No. " + PrNo + " is activated successfully.";
                    }
                }

                Context.TblPurchaseRequests.UpdateRange(GetPRdata);
                await Context.SaveChangesAsync();

                response.Code = 200;
                response.Data = GetPRdata;
            }
            else
            {
                response.Code = 404;
                response.Message = "Purchase request with PR No. " + PrNo + " not found.";
            }

            return response;
        }
    }
}
