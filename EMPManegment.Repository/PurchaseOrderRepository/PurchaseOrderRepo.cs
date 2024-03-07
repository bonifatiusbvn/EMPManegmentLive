using EMPManagment.API;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.EntityModels.ViewModels.POMaster;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
using EMPManegment.Inretface.Interface.PurchaseOrder;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Threading.Tasks;

namespace EMPManegment.Repository.PurchaseOrderRepository
{
    public class PurchaseOrderRepo : IPurchaseOrder
    {

        public PurchaseOrderRepo(BonifatiusEmployeesContext context)
        {
            Context = context;
        }

        public BonifatiusEmployeesContext Context { get; }

        public string CheckOPNo(string projectname)
        {
            try
            {
                var LastOrder = Context.TblOrderMasters.OrderByDescending(e => e.CreatedOn).FirstOrDefault();
                var currentYear = DateTime.Now.Year;
                var lastYear = currentYear - 1;

                string POId;
                if (LastOrder == null)
                {
                    POId = $"BTPL/OP/{projectname}/{lastYear % 100}-{currentYear % 100}-01";
                }
                else
                {
                    if (LastOrder.OrderId.Length >= 25)
                    {
                        int orderNumber = int.Parse(LastOrder.OrderId.Substring(24)) + 1;
                        POId = $"BTPL/OP/{projectname}/{lastYear % 100}-{currentYear % 100}-" + orderNumber.ToString("D3");
                    }
                    else
                    {
                        throw new Exception("OrderId does not have the expected format.");
                    }
                }
                return POId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserResponceModel> CreatePO(List<OPMasterView> CreatePO)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                foreach (var item in CreatePO)
                {
                    var purchaseorder = new TblPurchaseOrder()
                    {
                        Id = Guid.NewGuid(),
                        VendorId = item.VendorId,
                        Poid = item.POId,
                        CompanyName = item.CompanyName,
                        ProductName = item.ProductName,
                        ProductShortDescription = item.ProductShortDescription,
                        ProductId = item.ProductId,
                        ProductType = item.ProductType,
                        Quantity = item.Quantity,
                        OrderDate = item.OrderDate,
                        DeliveryDate = item.DeliveryDate,
                        Status = item.Status,
                        TotalAmount = item.TotalAmount,
                        CreatedBy = item.CreatedBy,
                        CreatedOn = DateTime.Now,
                    };
                    Context.TblPurchaseOrders.Add(purchaseorder);
                }

                await Context.SaveChangesAsync();
                response.Code = 200;
                response.Message = "Purchase Order Created successfully!";
            }
            catch (Exception ex)
            {
                response.Code = 500;
                response.Message = "Error creating orders: " + ex.Message;
            }
            return response;
        }

        public async Task<jsonData> GetPOList(DataTableRequstModel dataTable)
        {
            try
            {
                var data = await (from a in Context.TblPurchaseOrders
                                  join b in Context.TblVendorMasters on a.VendorId equals b.Vid
                                  join c in Context.TblProductTypeMasters on a.ProductType equals c.Id
                                  select new
                                  {
                                      Order = a,
                                      Vendor = b,
                                      ProductType = c,

                                  }).ToListAsync();

                var groupedPOList = data.GroupBy(x => x.Order.Poid)
                                        .Select(group => group.First())
                                        .Select(item => new OPMasterView
                                        {
                                            Id = item.Order.Id,
                                            ProductId = item.Order.ProductId,
                                            POId = item.Order.Poid,
                                            CompanyName = item.Vendor.VendorCompany,
                                            VendorAddress = item.Vendor.VendorAddress,
                                            VendorId = item.Order.VendorId,
                                            ProductName = item.ProductType.Type,
                                            Quantity = item.Order.Quantity,
                                            TotalAmount = item.Order.TotalAmount,
                                            OrderDate = item.Order.OrderDate,
                                            DeliveryDate = item.Order.DeliveryDate,
                                            CreatedOn = item.Order.CreatedOn,
                                            ProductType = item.Order.ProductType,
                                            Status = item.Order.Status
                                        });


                var flattenedData = groupedPOList.Skip(dataTable.skip)
                                                  .Take(dataTable.pageSize)
                                                  .ToList();

                int totalRecord = groupedPOList.Count();

                jsonData jsonData = new jsonData
                {
                    draw = dataTable.draw,
                    recordsFiltered = totalRecord,
                    recordsTotal = totalRecord,
                    data = flattenedData
                };

                return jsonData;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<POResponseModel> DisplayPODetails(string POId)
        {
            POResponseModel response = new POResponseModel();
            try
            {
                var PODetails = new List<OPMasterView>();
                var data = await (from a in Context.TblPurchaseOrders
                                  join b in Context.TblVendorMasters on a.VendorId equals b.Vid
                                  join c in Context.TblProductTypeMasters on a.ProductType equals c.Id
                                  where a.Poid == POId
                                  select new OPMasterView
                                  {
                                      Id = a.Id,
                                      ProductId = a.ProductId,
                                      POId = a.Poid,
                                      CompanyName = b.VendorCompany,
                                      VendorId = a.VendorId,
                                      VendorAddress = b.VendorAddress,
                                      ProductName = a.ProductName,
                                      ProductShortDescription = a.ProductShortDescription,
                                      Quantity = a.Quantity,
                                      TotalAmount = a.TotalAmount,
                                      OrderDate = a.OrderDate,
                                      DeliveryDate = a.DeliveryDate,
                                      CreatedOn = a.CreatedOn,
                                      ProductTypeName = c.Type,
                                      Status = a.Status,
                                  }).ToListAsync();
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        PODetails.Add(new OPMasterView()
                        {
                            Id = item.Id,
                            ProductId = item.ProductId,
                            POId = item.POId,
                            CompanyName = item.CompanyName,
                            VendorId = item.VendorId,
                            VendorAddress = item.VendorAddress,
                            ProductName = item.ProductName,
                            ProductTypeName = item.ProductTypeName,
                            ProductShortDescription = item.ProductShortDescription,
                            Quantity = item.Quantity,
                            TotalAmount = item.TotalAmount,
                            OrderDate = item.OrderDate,
                            DeliveryDate = item.DeliveryDate,
                            CreatedOn = item.CreatedOn,
                            ProductType = item.ProductType,
                            Status = item.Status,
                        });
                    }
                    response.Data = PODetails;
                    response.Code = 200;
                    response.Message = "Purchase Order Invoice Is Generated successfully";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
    }
}
