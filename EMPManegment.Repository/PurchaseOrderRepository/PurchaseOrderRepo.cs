using Azure;
using EMPManagment.API;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.ExpenseMaster;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
using EMPManegment.EntityModels.ViewModels.PurchaseOrderModels;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.EntityModels.ViewModels.VendorModels;
using EMPManegment.Inretface.Interface.OrderDetails;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
#nullable disable
namespace EMPManegment.Repository.OrderRepository
{
    public class PurchaseOrderRepo : IPurchaseOrderDetails
    {
        public PurchaseOrderRepo(BonifatiusEmployeesContext context)
        {
            Context = context;
        }

        public BonifatiusEmployeesContext Context { get; }


        public async Task<UserResponceModel> CreatePurchaseOrder(PurchaseOrderDetailView CreatePurchaseOrder)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var ordermodel = new TblPurchaseOrderMaster()
                {
                    Id = Guid.NewGuid(),
                    OrderId = "Order_" + CreatePurchaseOrder.OrderId,
                    Type = CreatePurchaseOrder.Type,
                    CompanyName = CreatePurchaseOrder.CompanyName,
                    VendorId = CreatePurchaseOrder.VendorId,
                    ProductType = CreatePurchaseOrder.Product,
                    Quantity = CreatePurchaseOrder.Quantity,
                    AmountPerUnit = CreatePurchaseOrder.AmountPerUnit,
                    TotalAmount = CreatePurchaseOrder.TotalAmount,
                    OrderDate = CreatePurchaseOrder.OrderDate,
                    DeliveryDate = CreatePurchaseOrder.DeliveryDate,
                    PaymentMethod = CreatePurchaseOrder.PaymentMethod,
                    DeliveryStatus = CreatePurchaseOrder.DeliveryStatus,
                    CreatedOn = DateTime.Now,
                    CreatedBy = CreatePurchaseOrder.CreatedBy,
                };
                response.Code = 200;
                response.Message = "Order created successfully!";
                Context.TblPurchaseOrderMasters.Add(ordermodel);
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                response.Code = 404;
                response.Message = "Error in creating order.";
            }
            return response;
        }

        public async Task<IEnumerable<PurchaseOrderDetailView>> GetPurchaseOrderList()
        {
            try
            {
                var data = await (from a in Context.TblPurchaseOrderMasters
                                  join b in Context.TblVendorMasters on a.VendorId equals b.Vid
                                  join c in Context.TblProductTypeMasters on a.ProductType equals c.Id
                                  join d in Context.TblPaymentMethodTypes on a.PaymentMethod equals d.Id
                                  where a.IsDeleted != true
                                  select new
                                  {
                                      Order = a,
                                      Vendor = b,
                                      ProductType = c,
                                      PaymentMethod = d,
                                      CreatedOn = a.CreatedOn,
                                  }).ToListAsync();

                var orderList = data.GroupBy(x => x.Order.OrderId)
                                    .Select(group => group.First())
                                    .OrderByDescending(item => item.CreatedOn)
                                    .Select(item => new PurchaseOrderDetailView
                                    {
                                        Id = item.Order.Id,
                                        OrderId = item.Order.OrderId,
                                        ProductId = item.Order.ProductId,
                                        CompanyName = item.Vendor.VendorCompany,
                                        VendorId = item.Order.VendorId,
                                        ProductName = item.ProductType.Type,
                                        Quantity = item.Order.Quantity,
                                        OrderDate = item.Order.OrderDate,
                                        TotalAmount = item.Order.TotalAmount,
                                        AmountPerUnit = item.Order.AmountPerUnit,
                                        PaymentMethod = item.Order.PaymentMethod,
                                        PaymentMethodName = item.PaymentMethod.PaymentMethod,
                                        DeliveryStatus = item.Order.DeliveryStatus,
                                        DeliveryDate = item.Order.DeliveryDate,
                                        CreatedOn = item.Order.CreatedOn,
                                    });

                return orderList.ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<PurchaseOrderDetailView>> GetPurchaseOrderDetailsByStatus(string DeliveryStatus)
        {
            try
            {
                var data = await (from a in Context.TblPurchaseOrderMasters
                                  join b in Context.TblVendorMasters on a.VendorId equals b.Vid
                                  join c in Context.TblProductTypeMasters on a.ProductType equals c.Id
                                  join d in Context.TblPaymentMethodTypes on a.PaymentMethod equals d.Id
                                  where a.IsDeleted != true && a.DeliveryStatus == DeliveryStatus
                                  select new
                                  {
                                      Order = a,
                                      Vendor = b,
                                      ProductType = c,
                                      PaymentMethod = d
                                  }).ToListAsync();

                var orderList = data.Select(item => new PurchaseOrderDetailView
                {
                    Id = item.Order.Id,
                    OrderId = item.Order.OrderId,
                    ProductId = item.Order.ProductId,
                    CompanyName = item.Vendor.VendorCompany,
                    VendorId = item.Order.VendorId,
                    ProductName = item.ProductType.Type,
                    Quantity = item.Order.Quantity,
                    OrderDate = item.Order.OrderDate,
                    TotalAmount = item.Order.TotalAmount,
                    AmountPerUnit = item.Order.AmountPerUnit,
                    PaymentMethod = item.Order.PaymentMethod,
                    PaymentMethodName = item.PaymentMethod.PaymentMethod,
                    DeliveryStatus = item.Order.DeliveryStatus,
                    DeliveryDate = item.Order.DeliveryDate,
                    CreatedOn = item.Order.CreatedOn,
                }).ToList();

                return orderList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public string CheckPurchaseOrder(string projectname)
        {
            try
            {
                var LastOrder = Context.TblPurchaseOrderMasters.OrderByDescending(e => e.CreatedOn).FirstOrDefault();
                var currentYear = DateTime.Now.Year;
                var lastYear = currentYear - 1;

                int startIndex = projectname.IndexOf('(');
                int endIndex = projectname.IndexOf(')');

                var Projectsubparts = projectname.Substring(startIndex + 1, endIndex - startIndex - 1);
                string UserOrderId;
                if (LastOrder == null)
                {
                    UserOrderId = $"BTPL/PO/{Projectsubparts}/{lastYear % 100}-{currentYear % 100}-001";
                }
                else
                {
                    if (LastOrder.OrderId.Length >= 25)
                    {
                        int orderNumber = int.Parse(LastOrder.OrderId.Substring(24)) + 1;
                        UserOrderId = $"BTPL/PO/{Projectsubparts}/{lastYear % 100}-{currentYear % 100}-" + orderNumber.ToString("D3");
                    }
                    else
                    {
                        throw new Exception("OrderId does not have the expected format.");
                    }
                }
                return UserOrderId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<ApiResponseModel> InsertMultiplePurchaseOrder(PurchaseOrderMasterView InsertPurchaseOrder)
        {
            ApiResponseModel response = new ApiResponseModel();
            try
            {
                var PurchaseOrder = new TblPurchaseOrderMaster()
                {
                    Id = Guid.NewGuid(),
                    ProductId = InsertPurchaseOrder.ProductId,
                    OrderId = InsertPurchaseOrder.OrderId,
                    Type = InsertPurchaseOrder.Type,
                    VendorId = InsertPurchaseOrder.VendorId,
                    CompanyName = InsertPurchaseOrder.CompanyName,
                    ProductName = InsertPurchaseOrder.ProductName,
                    ProductShortDescription = InsertPurchaseOrder.ProductShortDescription,
                    ProjectId = InsertPurchaseOrder.ProjectId,
                    ProductType = InsertPurchaseOrder.ProductType,
                    Quantity = InsertPurchaseOrder.Quantity,
                    GstPerUnit = InsertPurchaseOrder.GstPerUnit,
                    TotalGst = InsertPurchaseOrder.TotalGst,
                    AmountPerUnit = InsertPurchaseOrder.AmountPerUnit,
                    SubTotal = InsertPurchaseOrder.SubTotal,
                    TotalAmount = InsertPurchaseOrder.TotalAmount,
                    DeliveryDate = InsertPurchaseOrder.DeliveryDate,
                    OrderDate = InsertPurchaseOrder.OrderDate,
                    OrderStatus = InsertPurchaseOrder.OrderStatus,
                    PaymentMethod = InsertPurchaseOrder.PaymentMethod,
                    PaymentStatus = InsertPurchaseOrder.PaymentStatus,
                    DeliveryStatus = InsertPurchaseOrder.DeliveryStatus,
                    IsDeleted = false,
                    CreatedBy = InsertPurchaseOrder.CreatedBy,
                    CreatedOn = DateTime.Now,                   
                };
                Context.TblPurchaseOrderMasters.Add(PurchaseOrder);

                foreach (var item in InsertPurchaseOrder.ProductList)
                {
                    var PurchaseOrderDetail = new TblPurchaseOrderDetail()
                    {
                        PorefId = PurchaseOrder.Id,
                        ProductId = item.ProductId,
                        Product = item.Product,
                        ProductType = item.ProductType,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        Discount = item.Discount,
                        Gst = item.Gst,
                        ProductTotal = item.ProductTotal,
                        IsDeleted = false,
                        CreatedBy = InsertPurchaseOrder.CreatedBy,
                        CreatedOn = DateTime.Now,
                    };
                    Context.TblPurchaseOrderDetails.Add(PurchaseOrderDetail);
                }
                
                    var PurchaseAddress = new TblPodeliveryAddress()
                    {
                        Poid = PurchaseOrder.Id,
                        Address = InsertPurchaseOrder.Address,
                        IsDeleted = false
                    };

                    Context.TblPodeliveryAddresses.Add(PurchaseAddress);

                await Context.SaveChangesAsync();
                response.code = (int)HttpStatusCode.OK;
                response.message = "Purchase order successfully inserted.";
            }
            catch (Exception ex)
            {
                response.code = 400;
                response.message = "Error in creating purchase orders.";
            }
            return response;
        }

        public async Task<PurchaseOrderMasterView> GetPurchaseOrderDetailsByOrderId(string OrderId)
        {
            PurchaseOrderMasterView orderDetails = new PurchaseOrderMasterView();
            try
            {
                orderDetails = (from a in Context.TblPurchaseOrderMasters.Where(x => x.OrderId == OrderId)
                                join b in Context.TblVendorMasters on a.VendorId equals b.Vid
                                join d in Context.TblCompanyMasters on a.CompanyName equals d.CompnyName
                                join e in Context.TblCities on d.City equals e.Id
                                join f in Context.TblStates on d.State equals f.Id
                                join Vst in Context.TblStates on b.VendorState equals Vst.Id
                                join Vct in Context.TblCities on b.VendorCity equals Vct.Id
                                join g in Context.TblPodeliveryAddresses on a.Id equals g.Poid
                                join h in Context.TblPaymentMethodTypes on a.PaymentMethod equals h.Id
                                join i in Context.TblPaymentTypes on a.PaymentStatus equals i.Id
                                select new PurchaseOrderMasterView
                                {
                                    Id = a.Id,
                                    OrderId = a.OrderId,
                                    VendorId = a.VendorId,
                                    Type = a.Type,
                                    CompanyName = a.CompanyName,
                                    CompanyFullAddress = d.Address + "," + e.City + "," + f.State,
                                    CompanyGstNumber = d.Gst,
                                    VendorName = b.VendorFirstName + " " + b.VendorLastName,
                                    VendorFullAddress = b.VendorAddress + "," + Vct.City + "," + Vst.State + "-" + b.VendorPinCode,
                                    VendorContact = b.VendorContact,
                                    VendorEmail = b.VendorCompanyEmail,
                                    VendorAccountHolderName = b.VendorAccountHolderName,
                                    VendorBankAccountNo = b.VendorBankAccountNo,
                                    SubTotal = a.SubTotal,
                                    TotalGst = a.TotalGst,
                                    ProductShortDescription = a.ProductShortDescription,
                                    OrderDate = a.OrderDate,
                                    TotalAmount = a.TotalAmount,
                                    AmountPerUnit = a.AmountPerUnit,
                                    PaymentMethod = a.PaymentMethod,
                                    PaymentMethodName = h.PaymentMethod,
                                    PaymentStatus = a.PaymentStatus,
                                    PaymentStatusName=i.Type,
                                    DeliveryStatus = a.DeliveryStatus,
                                    DeliveryDate = a.DeliveryDate,
                                    CreatedOn = a.CreatedOn,
                                    Address = g.Address,
                                }).FirstOrDefault();
                List<PurchaseOrderDetailsModel> productList = (from a in Context.TblPurchaseOrderDetails.Where(a => a.PorefId == orderDetails.Id)
                                                               join b in Context.TblProductTypeMasters on a.ProductType equals b.Id
                                                               select new PurchaseOrderDetailsModel
                                                               {
                                                                   ProductId = a.ProductId,
                                                                   Product = a.Product,
                                                                   ProductType = a.ProductType,
                                                                   ProductTotal = a.ProductTotal,
                                                                   Quantity = a.Quantity,
                                                                   ProductTypeName = b.Type,
                                                                   Price = a.Price,
                                                                   Gst = a.Gst,
                                                               }).ToList();
                orderDetails.ProductList = productList;
                return orderDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<PaymentMethodView>> GetAllPaymentMethod()
        {
            try
            {
                IEnumerable<PaymentMethodView> paymentMethod = Context.TblPaymentMethodTypes.ToList().Select(a => new PaymentMethodView
                {
                    Id = a.Id,
                    PaymentMethod = a.PaymentMethod,
                });
                return paymentMethod;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UpdatePurchaseOrderView> EditPurchaseOrderDetails(Guid Id)
        {
            try
            {
                var OrderDetails = new UpdatePurchaseOrderView();
                var data = Context.TblPurchaseOrderMasters.Where(x => x.Id == Id).SingleOrDefault();
                if (data != null)
                {

                    OrderDetails = new UpdatePurchaseOrderView()
                    {
                        Id = data.Id,
                        OrderId = data.OrderId,
                        OrderDate = data.OrderDate,
                        CompanyName = data.CompanyName,
                        ProductName = data.ProductName,
                        ProductShortDescription = data.ProductShortDescription,
                        TotalAmount = data.TotalAmount,
                        PaymentMethod = data.PaymentMethod,
                        DeliveryStatus = data.DeliveryStatus,
                        OrderStatus = data.OrderStatus,
                    };
                }

                return OrderDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserResponceModel> UpdatePurchaseOrderDetails(UpdatePurchaseOrderView UpdatePurchaseorder)
        {
            UserResponceModel model = new UserResponceModel();
            var orderdetails = Context.TblPurchaseOrderMasters.Where(e => e.Id == UpdatePurchaseorder.Id).FirstOrDefault();
            try
            {
                if (orderdetails != null)
                {
                    orderdetails.Id = UpdatePurchaseorder.Id;
                    orderdetails.OrderDate = UpdatePurchaseorder.OrderDate;
                    orderdetails.CompanyName = UpdatePurchaseorder.CompanyName;
                    orderdetails.ProductName = UpdatePurchaseorder.ProductName;
                    orderdetails.TotalAmount = UpdatePurchaseorder.TotalAmount;
                    orderdetails.PaymentMethod = UpdatePurchaseorder.PaymentMethod;
                    orderdetails.DeliveryStatus = UpdatePurchaseorder.DeliveryStatus;
                    orderdetails.OrderStatus = UpdatePurchaseorder.OrderStatus;
                    orderdetails.UpdatedBy=UpdatePurchaseorder.UpdatedBy;
                    orderdetails.UpdatedOn=DateTime.Now;
                }
                Context.TblPurchaseOrderMasters.Update(orderdetails);
                Context.SaveChanges();
                model.Code = 200;
                model.Message = "Order details updated successfully!";
            }
            catch (Exception ex)
            {
                model.Code = 400;
                model.Message = "Error in updating order details.";
            }
            return model;
        }

        public async Task<UserResponceModel> DeletePurchaseOrderDetails(Guid Id)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
               
                var GetOrderdata = Context.TblPurchaseOrderMasters.Where(a => a.Id == Id).FirstOrDefault();
                var PODetails = Context.TblPurchaseOrderDetails.Where(a => a.PorefId == Id).ToList();
                var POAddress = Context.TblPodeliveryAddresses.Where(a => a.Poid == Id).FirstOrDefault();

                if(GetOrderdata != null)
                {
                    GetOrderdata.IsDeleted = true;
                    Context.TblPurchaseOrderMasters.Update(GetOrderdata);
                    if (PODetails.Any() || POAddress != null)
                    {
                        foreach (var PODData in PODetails)
                        {
                            PODData.IsDeleted = true;
                            Context.TblPurchaseOrderDetails.Update(PODData);
                        }

                        POAddress.IsDeleted = true;
                        Context.TblPodeliveryAddresses.Update(POAddress);

                        Context.SaveChanges();

                        response.Code = 200;
                        response.Message = "Purchase order details are successfully deleted.";
                    }
                    else
                    {
                        response.Code = 404;
                        response.Message = "No related records found to delete";
                    }
                }
                else
                {
                    response.Code = 404;
                    response.Message = "No related records found to delete";
                }                                  
            }
            catch (Exception ex)
            {
                response.Code = 400;
                response.Message = "Error in deleting purchase order.";
            }
            return response;
        }
    }
}
