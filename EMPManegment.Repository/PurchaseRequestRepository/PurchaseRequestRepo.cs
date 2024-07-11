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
using Microsoft.EntityFrameworkCore;
using Azure;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using Microsoft.AspNetCore.Http.HttpResults;
using EMPManegment.EntityModels.Common;
using EMPManegment.EntityModels.ViewModels.ManualInvoice;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
#nullable disable

namespace EMPManegment.Repository.PurchaseRequestRepository
{
    public class PurchaseRequestRepo : IPurchaseRequest
    {
        public PurchaseRequestRepo(BonifatiusEmployeesContext context, IConfiguration configuration)
        {
            Context = context;
            Configuration = configuration;
        }
        public BonifatiusEmployeesContext Context { get; }
        public IConfiguration Configuration { get; }

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
                        Date=DateTime.Now,
                        PrDate=item.PrDate,
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
                response.message = "Error in creating purchase request.";
            }
            return response;
        }

        public async Task<ApiResponseModel> DeletePurchaseRequest(string PrNo)
        {
            ApiResponseModel response = new ApiResponseModel();
            try
            {
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
            }
            catch
            {
                response.code = 400;
                response.message = "Error in deleting purchase request.";
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
                                           PrNo = a.PrNo,
                                           UserId = a.UserId,
                                           UserName = b.UserName,
                                           ProjectId = a.ProjectId,
                                           ProductId = a.ProductId,
                                           ProjectName = c.ProjectTitle,
                                           ProductName = a.ProductName,
                                           ProductTypeId = a.ProductTypeId,
                                           ProductTypeName=a.ProductName,
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

        public async Task<ApiResponseModel> UpdatePurchaseRequestDetails(PurchaseRequestMasterView UpdatePurchaseRequest)
        {
            ApiResponseModel response = new ApiResponseModel();
            try
            {
                foreach (var item in UpdatePurchaseRequest.PRList)
                {
                    var existingPurchaseRequest = Context.TblPurchaseRequests.FirstOrDefault(e => e.PrNo == UpdatePurchaseRequest.PrNo && e.ProductId == item.ProductId);

                    if (existingPurchaseRequest != null)
                    {
                        existingPurchaseRequest.UserId = item.UserId;
                        existingPurchaseRequest.ProjectId = item.ProjectId;
                        existingPurchaseRequest.ProductId = item.ProductId;
                        existingPurchaseRequest.ProductName = item.ProductName;
                        existingPurchaseRequest.ProductTypeId = item.ProductTypeId;
                        existingPurchaseRequest.Quantity = item.Quantity;
                        existingPurchaseRequest.PrNo = item.PrNo;
                        existingPurchaseRequest.UpdatedBy = item.UpdatedBy;
                        existingPurchaseRequest.UpdatedOn = DateTime.Now;
                        existingPurchaseRequest.Date = DateTime.Now;
                        existingPurchaseRequest.PrDate = item.PrDate;

                        Context.TblPurchaseRequests.Update(existingPurchaseRequest);
                    }
                    else
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
                            Date = DateTime.Now,
                            PrDate=item.PrDate,
                        };
                        Context.TblPurchaseRequests.Add(PRDetailS);
                    }
                }

                var deletedPurchaseRequest = UpdatePurchaseRequest.PRList.Select(a => a.ProductId).ToList();

                var PurchaseRequestToRemove = Context.TblPurchaseRequests
                    .Where(e => e.PrNo == UpdatePurchaseRequest.PrNo && !deletedPurchaseRequest.Contains(e.ProductId))
                    .ToList();

                Context.TblPurchaseRequests.RemoveRange(PurchaseRequestToRemove);
                await Context.SaveChangesAsync();
                response.code = (int)HttpStatusCode.OK;
                response.message = "PurchaseRequest updated successfully.";
            }
            catch (Exception ex)
            {
                response.code = 500;
                response.message = "Error updating PR: " + ex.Message;
            }
            return response;
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
                                          Date = userGroup.Select(x => x.a.Date).FirstOrDefault()
                                      };
            purchaseRequestList = purchaseRequestList.OrderByDescending(pr => pr.Date);

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

            try
            {
                string dbConnectionStr = Configuration.GetConnectionString("EMPDbconn");
                var sqlPar = new SqlParameter[]
                {
                   new SqlParameter("@PrNo", PrNo),
                };
                var PR = DbHelper.GetDataSet("[GetPRDetailsByPRId]", System.Data.CommandType.StoredProcedure, sqlPar, dbConnectionStr);

                PurchaseRequestMasterView PRDetails = new PurchaseRequestMasterView();

                if (PR != null && PR.Tables.Count > 0)
                {
                    if (PR.Tables[0].Rows.Count > 0)
                    {
                        PRDetails.PrNo = PR.Tables[0].Rows[0]["PrNo"]?.ToString();
                        PRDetails.PrDate = PR.Tables[0].Rows[0]["PrDate"] != DBNull.Value ? (DateTime)PR.Tables[0].Rows[0]["PrDate"] : DateTime.MinValue;
                        PRDetails.ProjectId = PR.Tables[0].Rows[0]["ProjectId"] != DBNull.Value ? (Guid)PR.Tables[0].Rows[0]["ProjectId"] : Guid.Empty;
                    }

                    PRDetails.PRList = new List<PurchaseRequestModel>();

                    foreach (DataRow row in PR.Tables[1].Rows)
                    {
                        var PRList = new PurchaseRequestModel
                        {
                            PrId = row["PrId"] != DBNull.Value ? (Guid)row["PrId"] : Guid.Empty,
                            ProductTypeId = row["ProductTypeId"] != DBNull.Value ? (int)row["ProductTypeId"] : 0,
                            ProductTypeName = row["ProductTypeName"]?.ToString(),
                            ProductId = row["ProductId"] != DBNull.Value ? (Guid)row["ProductId"] : Guid.Empty,
                            ProductName = row["ProductName"]?.ToString(),
                            UserId = row["UserId"] != DBNull.Value ? (Guid)row["UserId"] : Guid.Empty,
                            FullName = row["FullName"]?.ToString(),
                            ProjectId = row["ProjectId"] != DBNull.Value ? (Guid)row["ProjectId"] : Guid.Empty,
                            ProjectName = row["ProjectName"]?.ToString(),
                            Quantity = row["Quantity"] != DBNull.Value ? (decimal)row["Quantity"] : 0,
                            Price = row["Price"] != DBNull.Value ? (decimal)row["Price"] : 0m,
                            GstAmount = row["GstAmount"] != DBNull.Value ? (decimal)row["GstAmount"] : 0m,
                            ProductImage = row["ProductImage"]?.ToString(),
                            ProductTotal = row["ProductTotal"] != DBNull.Value ? (decimal)row["ProductTotal"] : 0m,
                            ProductDescription = row["ProductDescription"]?.ToString(),
                        };

                        PRDetails.PRList.Add(PRList);
                    }
                }

                return PRDetails;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching PR details", ex);
            }
        }
        public async Task<UserResponceModel> ApproveUnapprovePR(List<string> PrNo)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                foreach (var item in PrNo)
                {
                    if (item != "")
                    {
                        var prList = await Context.TblPurchaseRequests.Where(a => a.PrNo == item).ToListAsync();
                        if (prList.Any())
                        {
                            foreach (var pr in prList)
                            {
                                pr.IsApproved = true;
                                response.Message = "Purchase request is approved successfully.";
                                response.Code = 200;
                                Context.TblPurchaseRequests.Update(pr);
                            }
                            await Context.SaveChangesAsync();
                        }
                        else
                        {
                            response.Message = "Purchase request is already approved!";
                            response.Code = 404;
                        }
                    }
                    else
                    {
                        response.Message = "Please select purchase request!";
                        response.Code = 404;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error in approve - unapprove purchase request.";
                response.Code = 400;
            }
            return response;
        }

        public async Task<List<PurchaseRequestModel>> ProductDetailsById(Guid ProductId)
        {
            try
            {
                var productDetails = new List<PurchaseRequestModel>();
                var data = await(from a in Context.TblProductDetailsMasters.Where(x => x.Id == ProductId)
                                 join b in Context.TblProductTypeMasters on a.ProductType equals b.Id
                                 select new PurchaseRequestModel
                                 {
                                     ProductTypeId = b.Id,
                                     ProductDescription = a.ProductDescription,
                                     ProductName = a.ProductName,
                                     ProductImage = a.ProductImage,
                                     ProductId = a.Id,
                                     Price = a.PerUnitPrice,
                                     GstAmount = a.GstAmount,
                                     ProductTypeName = b.Type,
                                 }).ToListAsync();
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        productDetails.Add(new PurchaseRequestModel()
                        {
                            ProductId = item.ProductId,
                            ProductTypeId = item.ProductTypeId,
                            ProductDescription = item.ProductDescription,
                            ProductName = item.ProductName,
                            ProductImage = item.ProductImage,
                            Price = item.Price,
                            GstAmount = item.GstAmount,
                            ProductTypeName = item.ProductTypeName,
                        });
                    }
                }
                return productDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
