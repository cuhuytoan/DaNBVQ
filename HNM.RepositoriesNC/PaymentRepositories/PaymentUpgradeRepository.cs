using HNM.DataNC.Models;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.DataNC.ModelsPayment;
using HNM.RepositoriesNC.PaymentRepositoriesBase;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNM.RepositoriesNC.PaymentRepositories
{
    public interface IPaymentUpgradeRepository : IPaymentRepositoryBase<OrderDetail>
    {
        Task<OrderDetail> UpgradeBrand(UpgrageBrandPackageDTO model, string productBrandName, string Phone,bool isSameType);
        Task<OrderDetail> GetOrderDetailByOrderCode(string OrderCode);
        Task<Order> GetOrderMasterByOrderCode(string OrderCode);
        Task<PaymentExpVat> PostPaymentVat(PostPaymentVAT model);
        Task UploadImage(string fileName, string OrderCode);
        Task<Order> FinishUpgradeBrand(string orderCode, int PaymentStatusId);
        Task<PaymentLog> SavePaymentLog(PaymentLog model);
        Task<Order> UpgradePaymentType(string orderCode, int PaymentTypeId);
        Task<Order> GetOrderByBrandId(int ProductBrandId, int ServiceId);
        //Upgrade gian hàng từ vừa đến lớn
        Task<OrderDetail> UpgradeBrandVTL(UpgrageBrandPackageDTO model, string productBrandName, string Phone,int? MonthRemain, int? MonthPrice, decimal totalDeduct);
        //Get GetOrderRemain by ProductBrandID
        Task<OrderRemain> GetOrderRemain(int ProductBrandId);
        Task<List<OrderDetail>> GetListOrderDetailByBrandId(int ProductBrandId);
        Task<OrderDetail> GetCurrentOrderDetail(int ProductBrandId, int ServiceId);
        Task<List<OrderHistory>> GetListHistoryByBrandId(int ProductBrandId);
        Task<OrderDetail> ProcessOldBrand(UpgrageBrandPackageDTO model, string productBrandName, string Phone);
        Task<OrderDTO> DeleteOrderByOrderCode(string OrderCode);
    }
    public class PaymentUpgradeRepository : PaymentRepositoryBase<OrderDetail>, IPaymentUpgradeRepository
    {
        public PaymentUpgradeRepository(PaymentContext PaymentContext) : base(PaymentContext)
        {

        }

        public async Task<Order> FinishUpgradeBrand(string orderCode, int PaymentStatusId)
        {
            var output = new Order();
            var item = await PaymentContext.Order.FirstOrDefaultAsync(p => p.OrderCode == orderCode);
            if(item != null)
            {
                item.PaymentStatus_ID = PaymentStatusId;             
                PaymentContext.SaveChanges();
            }
            return output;
        }

        public async Task<List<OrderDetail>> GetListOrderDetailByBrandId(int ProductBrandId)
        {
            return await PaymentContext.OrderDetail.Where(p => p.ProductBrand_ID == ProductBrandId).OrderByDescending(x => x.EndDate).ToListAsync();
        }

        public async Task<Order> GetOrderByBrandId(int ProductBrandId, int ServiceId)
        {            
            return await PaymentContext.Order.OrderByDescending(x => x.LastEditDate).FirstOrDefaultAsync(p => p.ProductBrand_ID == ProductBrandId && p.Service_ID == ServiceId);             
        }

        public async Task<OrderDetail> GetOrderDetailByOrderCode(string OrderCode)
        {
            var output = new OrderDetail();
            try
            {
                output = await PaymentContext.OrderDetail.FirstOrDefaultAsync(p => p.OrderCode == OrderCode);
            }
            catch(Exception ex)
            {
                return output;
            }
            return output;
        }

        public async Task<OrderRemain> GetOrderRemain(int ProductBrandId)
        {
            var output = new OrderRemain();
            var orderMasterItems = await PaymentContext.Order.OrderByDescending(x => x.LastEditDate).FirstOrDefaultAsync(p => p.ProductBrand_ID == ProductBrandId && p.PaymentStatus_ID == 1);
            var orderDetailItems = await PaymentContext.OrderDetail.OrderByDescending(x => x.EndDate).FirstOrDefaultAsync(p => p.Order_ID == orderMasterItems.Order_ID);

            if (orderMasterItems != null && orderDetailItems !=null)
            {
                var diffMonth = (int)(orderDetailItems.EndDate?.Month - DateTime.Now.Month) + 12 * (orderDetailItems.EndDate?.Year - DateTime.Now.Year);
                output.MonthRemain = diffMonth ?? 0;
                output.MonthPrice = PaymentContext.Services.FirstOrDefault(p => p.ServiceId == orderMasterItems.Service_ID)?.PricePerMonth ?? 0;
                output.TotalDeduct = output.MonthRemain * output.MonthPrice;
            }
            return output;
        }

        public async Task<Order> GetOrderMasterByOrderCode(string OrderCode)
        {
            var output = new Order();
            try
            {
                output = await PaymentContext.Order.FirstOrDefaultAsync(p => p.OrderCode == OrderCode);
            }
            catch (Exception ex)
            {
                return output;
            }
            return output;
        }

        public async Task<PaymentExpVat> PostPaymentVat(PostPaymentVAT model)
        {
            var output = new PaymentExpVat();
            try
            {
                output.OrderCode = model.OrderCode;
                output.CompanyName = model.CompanyName;
                output.TaxCode = model.TaxCode;
                output.BuyerName = model.BuyerName;
                output.CompanyAddress = model.CompanyAddress;
                output.ReceiveBillAddress = model.ReceiveBillAddress;
                output.CreateBy = Guid.Parse(model.UserId);
                output.CreateDate = DateTime.Now;
                output.LastEditBy = Guid.Parse(model.UserId);
                output.LastEditDate = DateTime.Now;
                output.Email = model.Email;
                PaymentContext.PaymentExpVat.Add(output);
                await PaymentContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return output;
        }

        public async Task<PaymentLog> SavePaymentLog(PaymentLog model)
        {            
            if(model != null)
            {
                PaymentContext.PaymentLog.Add(model);
                PaymentContext.SaveChanges();
            }
            return model;
        }

        public async Task<OrderDetail> UpgradeBrand(UpgrageBrandPackageDTO model, string productBrandName, string Phone, bool isSameType)
        {
            var orderMaster = new Order();
            var orderDetail = new OrderDetail();
            try
            {
                var packageServices = await PaymentContext.Services.SingleOrDefaultAsync(p => p.ServiceId == model.ServiceId);
                var packageMonth = await PaymentContext.DiscountConfig.SingleOrDefaultAsync(p => p.Discount_ID == model.Discount_ID);
                var countOrderMaster = (PaymentContext.Order.Count() + 1).ToString();
                var countOrderDetail = (PaymentContext.OrderDetail.Count() + 1).ToString();                
                //Save OrderMAster 
                orderMaster.Order_ID = Guid.NewGuid().ToString();
                orderMaster.OrderCode = "NCGH" + (model.ServiceId == 2 ? "V" : "L") + Phone + countOrderMaster;
                orderMaster.Service_ID = model.ServiceId;
                orderMaster.DisCount = packageServices.PricePerMonth * packageMonth.Month * packageMonth.Percent_Discount / 100;
                orderMaster.Sum = packageServices.PricePerMonth * packageMonth.Month - orderMaster.DisCount;
                orderMaster.VAT = orderMaster.Sum * 10 / 100;
                orderMaster.Discount_ID = model.Discount_ID;
                orderMaster.Total = orderMaster.Sum + orderMaster.VAT;
                orderMaster.ProductBrand_ID = model.ProductBrandId;
                orderMaster.ProductBrand_ID = model.ProductBrandId;
                orderMaster.ProductBrandName = productBrandName;                

                orderMaster.CreateBy = model.UserId;
                orderMaster.CreateDate = DateTime.Now;
                orderMaster.LastEditBy = model.UserId;
                orderMaster.LastEditDate = DateTime.Now;
                orderMaster.PaymentStatus_ID = 0; // Đang chờ thanh toán

                PaymentContext.Order.Add(orderMaster);
                PaymentContext.SaveChanges();

                //Save Order Detail
                orderDetail.OrderDetail_ID = Guid.NewGuid().ToString();
                orderDetail.Order_ID = orderMaster.Order_ID;
                orderDetail.OrderCode = "NCGH" + (model.ServiceId == 2 ? "V" : "L") + Phone + countOrderMaster;
                orderDetail.ProductName = packageServices.ServiceName;
                orderDetail.ProductBrand_ID = model.ProductBrandId;
                orderDetail.ProductName = productBrandName;
                orderDetail.Quatity = 1;
                orderDetail.Price = packageServices.PricePerMonth * packageMonth.Month;
                orderDetail.DisCount = orderDetail.Price * packageMonth.Percent_Discount / 100;
                orderDetail.Sum = orderDetail.Price - orderDetail.DisCount;
                orderDetail.VAT = orderDetail.Sum * 10 / 100;
                orderDetail.Total = orderDetail.Sum + orderDetail.VAT;
                orderDetail.Description = model.ServiceId == 2 ? "Nâng cấp gian hàng lên quy mô \"Nhà cung cấp vừa\"" : "Nâng cấp gian hàng lên quy mô \"Nhà cung cấp lớn\"";
                orderDetail.StartDate = DateTime.Now;
                if(!isSameType)
                {
                    orderDetail.EndDate = DateTime.Now.AddMonths(packageMonth.Month ?? 0);
                }
                else
                {
                    var currentOrderMaster = await PaymentContext.Order.OrderByDescending(x => x.LastEditDate).FirstOrDefaultAsync(p => p.ProductBrand_ID == model.ProductBrandId && p.PaymentStatus_ID == 1 && p.Service_ID == model.ServiceId);
                    var currentOrderDetail = new OrderDetail();
                    if (currentOrderMaster != null)
                    {
                        currentOrderDetail = await PaymentContext.OrderDetail.FirstOrDefaultAsync(p => p.OrderCode == currentOrderMaster.OrderCode);
                        if(currentOrderDetail !=null)
                        {
                            orderDetail.EndDate = currentOrderDetail.EndDate?.AddMonths(packageMonth.Month ?? 0);
                        }    
                    }
                    
                }
                

                PaymentContext.OrderDetail.Add(orderDetail);
                PaymentContext.SaveChanges();

                
            }catch(Exception ex)
            {

            }

            return orderDetail;

        }
        /// <summary>
        /// Upgrade gian hàng từ vừa lên lớn
        /// </summary>
        /// <param name="model"></param>
        /// <param name="productBrandName"></param>
        /// <param name="Phone"></param>
        /// <returns></returns>
        public async Task<OrderDetail> UpgradeBrandVTL(UpgrageBrandPackageDTO model, string productBrandName, string Phone, int? MonthRemain, int? MonthPrice, decimal totalDeduct)
        {
            var orderMaster = new Order();
            var orderDetail = new OrderDetail();
            try
            {
                var packageServices = await PaymentContext.Services.SingleOrDefaultAsync(p => p.ServiceId == model.ServiceId);
                var packageMonth = await PaymentContext.DiscountConfig.SingleOrDefaultAsync(p => p.Discount_ID == model.Discount_ID);
                var countOrderMaster = (PaymentContext.Order.Count() + 1).ToString();
                var countOrderDetail = (PaymentContext.OrderDetail.Count() + 1).ToString();

                var currentOrderMaster = await PaymentContext.Order.OrderByDescending(x => x.LastEditDate).FirstOrDefaultAsync(p => p.ProductBrand_ID == model.ProductBrandId && p.PaymentStatus_ID == 1);
                var currentOrderDetail = new OrderDetail();
                if ( currentOrderMaster != null)
                {
                    currentOrderDetail = await PaymentContext.OrderDetail.FirstOrDefaultAsync(p => p.OrderCode == currentOrderMaster.OrderCode);
                }

           

                //Save OrderMAster 
                orderMaster.Order_ID = Guid.NewGuid().ToString();
                orderMaster.OrderCode = "NCGH" + (model.ServiceId == 2 ? "V" : "L") + Phone + countOrderMaster;
                orderMaster.Service_ID = model.ServiceId;
                orderMaster.DisCount = packageServices.PricePerMonth * packageMonth.Month * packageMonth.Percent_Discount / 100;
                orderMaster.Sum = packageServices.PricePerMonth * packageMonth.Month - orderMaster.DisCount;
                orderMaster.VAT = orderMaster.Sum * 10 / 100;
                orderMaster.Discount_ID = model.Discount_ID;
                orderMaster.Total = ((packageServices.PricePerMonth * packageMonth.Month) -(packageServices.PricePerMonth * packageMonth.Month * packageMonth.Percent_Discount /100) - totalDeduct)
                    + ((packageServices.PricePerMonth * packageMonth.Month) - (packageServices.PricePerMonth * packageMonth.Month * packageMonth.Percent_Discount / 100) - totalDeduct) * 10 / 100;
                orderMaster.ProductBrand_ID = model.ProductBrandId;
                orderMaster.ProductBrand_ID = model.ProductBrandId;
                orderMaster.ProductBrandName = productBrandName;

                orderMaster.CreateBy = model.UserId;
                orderMaster.CreateDate = DateTime.Now;
                orderMaster.LastEditBy = model.UserId;
                orderMaster.LastEditDate = DateTime.Now;
                orderMaster.PaymentStatus_ID = 0; // Đang chờ thanh toán

                PaymentContext.Order.Add(orderMaster);
                PaymentContext.SaveChanges();


                //Save Order Detail
                orderDetail.OrderDetail_ID = Guid.NewGuid().ToString();
                orderDetail.Order_ID = orderMaster.Order_ID;
                orderDetail.OrderCode = "NCGH" + (model.ServiceId == 2 ? "V" : "L") + Phone + countOrderMaster;
                orderDetail.ProductName = packageServices.ServiceName;
                orderDetail.ProductBrand_ID = model.ProductBrandId;
                orderDetail.ProductName = productBrandName;
                orderDetail.Quatity = 1;
                orderDetail.Price = packageServices.PricePerMonth * packageMonth.Month;
                orderDetail.DisCount = orderDetail.Price * packageMonth.Percent_Discount / 100;
                orderDetail.Sum = orderDetail.Price - orderDetail.DisCount;
                orderDetail.VAT = orderDetail.Sum * 10 / 100;
                orderDetail.Total = (orderDetail.Sum - totalDeduct ) + (orderDetail.Sum - totalDeduct) * 10/ 100;
                orderDetail.Description = model.ServiceId == 2 ? "Nâng cấp gian hàng lên quy mô \"Nhà cung cấp vừa\"" : "Nâng cấp gian hàng lên quy mô \"Nhà cung cấp lớn\"";
                orderDetail.StartDate = DateTime.Now;
                orderDetail.EndDate = DateTime.Now.AddMonths(packageMonth.Month ?? 0);
                orderDetail.MonthRemain = MonthRemain;
                orderDetail.TotalDeduct = totalDeduct;

                PaymentContext.OrderDetail.Add(orderDetail);
                PaymentContext.SaveChanges();

               

            }
            catch (Exception ex)
            {

            }

            return orderDetail;
        }

        public async Task<Order> UpgradePaymentType(string orderCode,int PaymentTypeId)
        {
            var orderMaster = await PaymentContext.Order.FirstOrDefaultAsync(p => p.OrderCode == orderCode);
            if(orderMaster != null)
            {
                orderMaster.PaymentType_ID = PaymentTypeId;
                PaymentContext.SaveChanges();
            }
            return orderMaster;
        }

        public async Task UploadImage(string fileName, string OrderCode)
        {
            var orderItem = await PaymentContext.OrderDetail.FirstOrDefaultAsync(p => p.OrderCode == OrderCode);
            if(orderItem != null)
            {
                orderItem.OrderImage = fileName;
                await PaymentContext.SaveChangesAsync();
            }    
        }

        public async Task<List<OrderHistory>> GetListHistoryByBrandId(int ProductBrandId)
        {
            var output = new List<OrderHistory>();
            var result = await (from a in PaymentContext.Order
                          join b in PaymentContext.OrderDetail on a.Order_ID equals b.Order_ID
                          where a.ProductBrand_ID == ProductBrandId
                          select new { a, b }).ToListAsync();
            foreach(var p in result)
            {
                OrderHistory orHis = new OrderHistory();
                orHis.OrderDetail_ID = p.b.OrderDetail_ID;
                orHis.Order_ID = p.b.Order_ID;
                orHis.ProductName = p.b.ProductName;
                orHis.ProductBrand_ID = p.b.ProductBrand_ID;
                orHis.ProductBrandName = p.b.ProductBrandName;
                orHis.Unit = p.b.Unit;
                orHis.Quatity = p.b.Quatity;
                orHis.Price = p.b.Price;
                orHis.VAT = p.b.VAT;
                orHis.Sum = p.b.Sum;
                orHis.DisCount = p.b.DisCount;
                orHis.Total = p.b.Total;
                orHis.Description = p.b.Description;
                orHis.StartDate = p.b.StartDate;
                orHis.EndDate = p.b.EndDate;
                orHis.OrderCode = p.b.OrderCode;
                orHis.OrderImage = p.b.OrderImage;
                orHis.PaymentStatus_ID = p.a.PaymentStatus_ID ??0;
                orHis.PaymentType_ID = p.a.PaymentType_ID ??0;
                orHis.LastEditDate = p.a.LastEditDate;
                output.Add(orHis);
            }
            return output.OrderByDescending(x => x.LastEditDate).ToList();
        }

        public async Task<OrderDetail> GetCurrentOrderDetail(int ProductBrandId, int ServiceId)
        {
            var output = new OrderDetail();
            var orderMaster = await PaymentContext.Order.OrderByDescending(x=>x.LastEditDate).FirstOrDefaultAsync(p => p.ProductBrand_ID == ProductBrandId && p.Service_ID == ServiceId);
            if(orderMaster != null)
            {
                output = await PaymentContext.OrderDetail.FirstOrDefaultAsync(p => p.OrderCode == orderMaster.OrderCode);
            }
            return output;
        }

        public async Task<OrderDetail> ProcessOldBrand(UpgrageBrandPackageDTO model, string productBrandName, string Phone)
        {
            var orderMaster = new Order();
            var orderDetail = new OrderDetail();
            try
            {
                var packageServices = await PaymentContext.Services.SingleOrDefaultAsync(p => p.ServiceId == model.ServiceId);
                //var packageMonth = await PaymentContext.DiscountConfig.SingleOrDefaultAsync(p => p.Discount_ID == model.Discount_ID);
                var countOrderMaster = (PaymentContext.Order.Count() + 1).ToString();
                string OrderCode = "NCGH";
                if(model.ServiceId == 2)
                {
                    OrderCode = OrderCode + "V" + Phone + countOrderMaster;
                }    
                else if(model.ServiceId == 3)
                {

                }    
                //Save OrderMAster 
                orderMaster.Order_ID = Guid.NewGuid().ToString();
                orderMaster.OrderCode = "NCGH" + (model.ServiceId == 2 ? "V" : "L") + Phone + countOrderMaster;
                orderMaster.Service_ID = model.ServiceId;
                orderMaster.DisCount = 0;
                orderMaster.Sum = 0;
                orderMaster.VAT = 0;
                orderMaster.Discount_ID = 0;
                orderMaster.Total = 0;
                orderMaster.ProductBrand_ID = model.ProductBrandId;                
                orderMaster.ProductBrandName = productBrandName;

                orderMaster.CreateBy = model.UserId;
                orderMaster.CreateDate = DateTime.Now;
                orderMaster.LastEditBy = model.UserId;
                orderMaster.LastEditDate = DateTime.Now;
                orderMaster.PaymentStatus_ID = 1; // Nâng cấp thành công

                PaymentContext.Order.Add(orderMaster);
                PaymentContext.SaveChanges();

                //Save Order Detail
                orderDetail.OrderDetail_ID = Guid.NewGuid().ToString();
                orderDetail.Order_ID = orderMaster.Order_ID;
                orderDetail.OrderCode = "NCGH" + (model.ServiceId == 2 ? "V" : "L") + Phone + countOrderMaster;
                orderDetail.ProductName = packageServices.ServiceName;
                orderDetail.ProductBrand_ID = model.ProductBrandId;
                orderDetail.ProductName = productBrandName;
                orderDetail.Quatity = 1;
                orderDetail.Price = 0;
                orderDetail.DisCount = 0;
                orderDetail.Sum = 0;
                orderDetail.VAT = 0;
                orderDetail.Total = 0;
                orderDetail.Description = model.ServiceId == 2 ? "Miễn phí : Nâng cấp gian hàng lên quy mô \"Nhà cung cấp vừa\"" : "Nâng cấp gian hàng lên quy mô \"Nhà cung cấp lớn\"";
                orderDetail.StartDate = DateTime.Now;
                orderDetail.EndDate = new DateTime(2020, 12, 31);

                PaymentContext.OrderDetail.Add(orderDetail);
                PaymentContext.SaveChanges();


            }
            catch (Exception ex)
            {

            }

            return orderDetail;
        }

        public async Task<OrderDTO> DeleteOrderByOrderCode(string OrderCode)
        {
            var orderMaster = await PaymentContext.Order.FirstOrDefaultAsync(p => p.OrderCode == OrderCode);
            if(orderMaster != null)
            {
                PaymentContext.Order.Remove(orderMaster);
                await  PaymentContext.SaveChangesAsync();
            }    
            var orderDetail = await PaymentContext.OrderDetail.FirstOrDefaultAsync(p => p.OrderCode == OrderCode);
            if (orderDetail != null)
            {
                PaymentContext.OrderDetail.Remove(orderDetail);
                await PaymentContext.SaveChangesAsync();
            }
            return new OrderDTO();
        }
    }
}
