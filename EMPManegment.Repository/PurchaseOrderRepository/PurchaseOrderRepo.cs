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
                        Poid = item.Opid,
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
            var POList = from a in Context.TblPurchaseOrders
                                             join b in Context.TblVendorMasters on a.VendorId equals b.Vid
                                             join c in Context.TblProductTypeMasters on a.ProductType equals c.Id
                                             select new OPMasterView
                                             {
                                                 Id = a.Id,
                                                 ProductId = a.ProductId,
                                                 Opid = a.Poid,
                                                 CompanyName = b.VendorCompany,
                                                 VendorId = a.VendorId,
                                                 ProductName = c.Type,
                                                 ProductShortDescription = a.ProductShortDescription,
                                                 Quantity = a.Quantity,
                                                 TotalAmount = a.TotalAmount,
                                                 OrderDate = a.OrderDate,
                                                 DeliveryDate = a.DeliveryDate,
                                                 CreatedOn = a.CreatedOn,
                                                 ProductType = a.ProductType,
                                                 Status = a.Status,
                                             };
            if (!string.IsNullOrEmpty(dataTable.sortColumn) && !string.IsNullOrEmpty(dataTable.sortColumnDir))
            {
                POList = POList.OrderBy(dataTable.sortColumn + " " + dataTable.sortColumnDir);
            }

            if (!string.IsNullOrEmpty(dataTable.searchValue))
            {
                POList = POList.Where(e => e.CompanyName.Contains(dataTable.searchValue) || e.ProductName.Contains(dataTable.searchValue) || e.ProductShortDescription.Contains(dataTable.searchValue));
            }
            int totalRecord = POList.Count();
            var cData = POList.Skip(dataTable.skip).Take(dataTable.pageSize).ToList();

            jsonData jsonData = new jsonData
            {
                draw = dataTable.draw,
                recordsFiltered = totalRecord,
                recordsTotal = totalRecord,
                data = cData
            };
            return jsonData;
        }
    }
}
