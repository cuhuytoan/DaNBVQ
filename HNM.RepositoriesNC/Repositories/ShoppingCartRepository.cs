using AutoMapper;
using HNM.DataNC.Models;
using HNM.DataNC.ModelsFilter;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.DataNC.ModelsNC.ModelsUtility;
using HNM.DataNC.ModelsStore;
using HNM.RepositoriesNC.RepositoriesBase;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HNM.RepositoriesNC.Repositories
{
    public interface IShoppingCartRepository :IRepositoryBase<ShoppingCartDetail>
    {
        Task<List<DeliveryAddressDTO>> GetLstDeliveryAddress(string userId);
        Task<bool> PostDeliveryAddress(DeliveryAddressDTO model);
        Task<bool> DeleteDeliveryAddress(int Id,string UserId);
        Task<List<ShoppingCartDetail>> PostShopingCart(List<PostShoppingCart> model);
        Task<bool> PostShopingCartAction(ShoppingCartDetailDTO model);
        Task<List<HistoryShopingCart_Result>> GetHistoryShopingCart(HistoryShoppingCart model);
        Task<HistoryShopingCart_Result> GetDetailShoppingCart(string ShopingCartCode);
        Task<CountHistoryShopingCart_Result> GetCountHistoryShopingCart(CountHistoryShoppingCart model);

    }
    public class ShoppingCartRepository: RepositoryBase<ShoppingCartDetail>,IShoppingCartRepository
    {        
        public ShoppingCartRepository(HanomaContext HanomaContext) : base(HanomaContext)
        {
        }

        public async Task<bool> DeleteDeliveryAddress(int Id,string UserId)
        {
            var pUserId = Guid.Parse(UserId);
            var item = await HanomaContext.DeliveryAddress.FirstOrDefaultAsync(p => p.CreateBy == pUserId && p.Id == Id);
            if(item == null)
            {
                return false;
            }
            else
            {
                HanomaContext.DeliveryAddress.Remove(item);
                await HanomaContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<CountHistoryShopingCart_Result> GetCountHistoryShopingCart(CountHistoryShoppingCart model)
        {
            var pUserId = new SqlParameter("@UserId", model.UserId ?? (object)DBNull.Value);
            var pProductBrandId = new SqlParameter("@ProductBrandId", model.ProductBrandId ?? (object)DBNull.Value);
            var pFromDate = new SqlParameter("@FromDate", model.FromDate ?? (object)DBNull.Value);
            var pToDate = new SqlParameter("@ToDate", model.ToDate ?? (object)DBNull.Value);
            var pShopingCartCode = new SqlParameter("@ShopingCartCode", model.ShopingCartCode ?? (object)DBNull.Value);
            try
            {
                var output = await HanomaContext.Set<CountHistoryShopingCart_Result>()
                    .FromSqlRaw($"EXECUTE dbo.CountHistoryShopingCart " +
                    $"@UserId = @UserId, " +
                    $"@ProductBrandId = @ProductBrandId, " +
                    $"@ShopingCartCode = @ShopingCartCode, " +
                    $"@FromDate = @FromDate, " +
                    $"@ToDate = @ToDate ",
                    pUserId, pProductBrandId, pShopingCartCode, pFromDate, pToDate
                    )
                    .AsNoTracking()
                    .ToListAsync();

                return output.FirstOrDefault();
            }
            catch (Exception ex)
            {

            }

            return new CountHistoryShopingCart_Result();
        }

        public async Task<HistoryShopingCart_Result> GetDetailShoppingCart(string ShopingCartCode)
        {
            var pUserId = new SqlParameter("@UserId", (object)DBNull.Value);
            var pProductBrandId = new SqlParameter("@ProductBrandId",(object)DBNull.Value);
            var pShopingCartCode = new SqlParameter("@ShopingCartCode", ShopingCartCode);
            var pShopingCartMasterId = new SqlParameter("@ShopingCartMasterId", (object)DBNull.Value);
            var pShopingCartDetailId = new SqlParameter("@ShopingCartDetailId", (object)DBNull.Value);
            var pStatusCart = new SqlParameter("@StatusCart", (object)DBNull.Value);
            var pPageSize = new SqlParameter("@PageSize", 1);
            var pCurrentPage = new SqlParameter("@CurrentPage", 1);
            var pFromDate = new SqlParameter("@FromDate", (object)DBNull.Value);
            var pToDate = new SqlParameter("@ToDate",  (object)DBNull.Value);

            try
            {
                var output = await HanomaContext.Set<HistoryShopingCart_Result>()
                    .FromSqlRaw($"EXECUTE dbo.HistoryShopingCart " +
                    $"@UserId = @UserId, " +
                    $"@ProductBrandId = @ProductBrandId, " +
                    $"@ShopingCartCode = @ShopingCartCode, " +
                    $"@ShopingCartMasterId = @ShopingCartMasterId, " +
                    $"@ShopingCartDetailId = @ShopingCartDetailId, " +
                    $"@StatusCart = @StatusCart, " +
                    $"@FromDate = @FromDate, " +
                    $"@ToDate = @ToDate, " +
                    $"@PageSize = @PageSize, " +
                    $"@CurrentPage = @CurrentPage ",
                    pUserId, pProductBrandId, pShopingCartCode, pShopingCartMasterId, pShopingCartDetailId,
                    pStatusCart, pFromDate, pToDate, pPageSize, pCurrentPage
                    )
                    .AsNoTracking()
                    .ToListAsync();

                return output.FirstOrDefault();
            }
            catch (Exception ex)
            {

            }

            return new HistoryShopingCart_Result();
        }

        public async Task<List<HistoryShopingCart_Result>> GetHistoryShopingCart(HistoryShoppingCart model)
        {
           
            var pUserId = new SqlParameter("@UserId", model.UserId ?? (object)DBNull.Value);
            var pProductBrandId = new SqlParameter("@ProductBrandId", model.ProductBrandId ?? (object)DBNull.Value);
            var pShopingCartCode = new SqlParameter("@ShopingCartCode", model.ShopingCartCode ?? (object)DBNull.Value);
            var pShopingCartMasterId = new SqlParameter("@ShopingCartMasterId", model.ShopingCartMasterId ?? (object)DBNull.Value);
            var pShopingCartDetailId = new SqlParameter("@ShopingCartDetailId", model.ShopingCartDetailId ?? (object)DBNull.Value);
            var pStatusCart = new SqlParameter("@StatusCart", model.StatusCart ?? (object)DBNull.Value);
            var pFromDate = new SqlParameter("@FromDate", model.FromDate ?? (object)DBNull.Value);
            var pToDate = new SqlParameter("@ToDate", model.ToDate ?? (object)DBNull.Value);
            var pPageSize = new SqlParameter("@PageSize", model.PageSize ?? 1);
            var pCurrentPage = new SqlParameter("@CurrentPage", model.CurrentPage ?? 20);

            try
            {
                var output = await HanomaContext.Set<HistoryShopingCart_Result>()
                    .FromSqlRaw($"EXECUTE dbo.HistoryShopingCart " +
                    $"@UserId = @UserId, " +
                    $"@ProductBrandId = @ProductBrandId, " +
                    $"@ShopingCartCode = @ShopingCartCode, " +
                    $"@ShopingCartMasterId = @ShopingCartMasterId, " +
                    $"@ShopingCartDetailId = @ShopingCartDetailId, " +
                    $"@StatusCart = @StatusCart, " +
                    $"@FromDate = @FromDate, " +
                    $"@ToDate = @ToDate, " +
                    $"@PageSize = @PageSize, " +
                    $"@CurrentPage = @CurrentPage ",
                    pUserId, pProductBrandId, pShopingCartCode, pShopingCartMasterId, pShopingCartDetailId,
                    pStatusCart,pFromDate,pToDate, pPageSize, pCurrentPage
                    )
                    .AsNoTracking()
                    .ToListAsync();

                return output;
            }
            catch(Exception ex)
            {

            }

            return new List<HistoryShopingCart_Result>();
        }

        public async Task<List<DeliveryAddressDTO>> GetLstDeliveryAddress(string userId)
        {
            var pUserId = Guid.Parse(userId);
            var output = new List<DeliveryAddressDTO>();
            try
            {
                var result = await HanomaContext.DeliveryAddress.Where(p => p.UserId == pUserId).OrderByDescending(x => x.IsDefault).ToListAsync();
                if(result != null)
                {
                    foreach(var p in result)
                    {
                        DeliveryAddressDTO item = new DeliveryAddressDTO();
                        item.Id = p.Id;
                        item.UserId = userId;
                        item.UserName = p.UserName;
                        item.PhoneNumber = p.PhoneNumber;
                        item.LocationId = p.LocationId;
                        item.DistrictId = p.DistrictId;
                        item.Address = p.Address;
                        item.IsDefault = p.IsDefault;
                        item.LocationName =  HanomaContext.Location.FirstOrDefault(x => x.Location_ID == p.LocationId)?.Name;
                        item.DistrictName = HanomaContext.District.FirstOrDefault(x => x.Id == p.DistrictId)?.Name;
                        output.Add(item);
                    }
                }    
            }
            catch (Exception ex)
            {

            }
            return output;
        }

        public async Task<bool> PostDeliveryAddress(DeliveryAddressDTO model)
        {
            try
            {                
                if(model !=null)
                {
                    var pUserId = Guid.Parse(model.UserId);
                    var item = await HanomaContext.DeliveryAddress.FirstOrDefaultAsync(p => p.Id == model.Id && p.CreateBy == pUserId);
                    if (item == null)
                    {
                        //Add Delivery
                        var addItem = new DeliveryAddress();
                        addItem.UserId = pUserId;
                        addItem.UserName = model.UserName;
                        addItem.PhoneNumber = model.PhoneNumber;
                        addItem.LocationId = model.LocationId;
                        addItem.DistrictId = model.DistrictId;
                        addItem.Address = model.Address;
                        addItem.IsDefault = model.IsDefault;
                        addItem.CreateBy = pUserId;
                        addItem.CreateDate = DateTime.Now;
                        addItem.LastEditBy = pUserId;
                        addItem.LastEditDate = DateTime.Now;
                        HanomaContext.DeliveryAddress.Add(addItem);
                        await HanomaContext.SaveChangesAsync();
                        //Update Default
                        if(model.IsDefault == true)
                        {
                            var lstItem = await HanomaContext.DeliveryAddress.Where(p => p.CreateBy == pUserId).ToListAsync();
                            foreach(var p in lstItem)
                            {
                                if (p.Id == addItem.Id) continue;
                                var pItem = await HanomaContext.DeliveryAddress.FirstOrDefaultAsync(x => x.Id == p.Id);
                                pItem.IsDefault = false;
                                await HanomaContext.SaveChangesAsync();
                            }    

                        }    
                    }
                    else
                    {
                        //Update                         
                        item.UserName = model.UserName;
                        item.PhoneNumber = model.PhoneNumber;
                        item.LocationId = model.LocationId;
                        item.DistrictId = model.DistrictId;
                        item.Address = model.Address;
                        item.IsDefault = model.IsDefault;                        
                        item.LastEditBy = pUserId;
                        item.LastEditDate = DateTime.Now;                        
                        await HanomaContext.SaveChangesAsync();
                        //Update Default
                        if (model.IsDefault == true)
                        {
                            var lstItem = await HanomaContext.DeliveryAddress.Where(p => p.CreateBy == pUserId).ToListAsync();
                            foreach (var p in lstItem)
                            {
                                if (p.Id == model.Id) continue;
                                var pItem = await HanomaContext.DeliveryAddress.FirstOrDefaultAsync(x => x.Id == p.Id);
                                pItem.IsDefault = false;
                                await HanomaContext.SaveChangesAsync();
                            }

                        }
                    }
                }    
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }

        public async Task<List<ShoppingCartDetail>> PostShopingCart(List<PostShoppingCart> model)
        {
            var output = new List<ShoppingCartDetail>();
            try
            {
                if(model != null)
                {
                    foreach(var item in model)
                    {
                        ShoppingCartMaster itemMaster = new ShoppingCartMaster();
                        itemMaster.UserId = Guid.Parse(item.UserId);
                        itemMaster.ProductBrandId = item.ProductBrandID;
                        itemMaster.DeliveryAddress = item.DeliveryAddress;
                        itemMaster.TotalAmount = item.TotalAmmout;
                        itemMaster.CreateBy = Guid.Parse(item.UserId);
                        itemMaster.CreateDate = DateTime.Now;
                        itemMaster.LastEditBy = Guid.Parse(item.UserId);
                        itemMaster.LastEditDate = DateTime.Now;
                        HanomaContext.ShoppingCartMaster.Add(itemMaster);
                        await HanomaContext.SaveChangesAsync();

                        
                        foreach (var p in item.LstShopCartDetail)
                        {
                            
                            var lastId = await HanomaContext.ShoppingCartDetail.OrderByDescending(p => p.Id).FirstOrDefaultAsync();
                            ShoppingCartDetail itemDetail = new ShoppingCartDetail();
                            itemDetail.ShoppingCartMasterId = itemMaster.Id;
                            itemDetail.ShopingCartCode = "HNM" + String.Format("{0:D7}", lastId == null ? 1 : lastId.Id + 1);
                            itemDetail.ProductId = p.ProductId;
                            var itemProduct = await HanomaContext.Product.FindAsync(p.ProductId);
                            if(itemProduct !=null) // Get price from server
                            {
                                itemDetail.Price = p.Price;
                            }                           
                            itemDetail.Qty = p.Qty;                            
                            itemDetail.DisCount = p.Discount;
                            itemDetail.Remark = p.Remark;
                            itemDetail.StatusCart = 1; // Waiting confirm
                            itemDetail.CreateBy = Guid.Parse(item.UserId);
                            itemDetail.CreateDate = DateTime.Now;
                            itemDetail.LastEditBy = Guid.Parse(item.UserId);
                            itemDetail.LastEditDate = DateTime.Now;
                            HanomaContext.ShoppingCartDetail.Add(itemDetail);
                            await HanomaContext.SaveChangesAsync();
                            //
                            output.Add(itemDetail);
                        }
                    }    
                    
                }
                else
                {
                    return output;
                }
            }
            catch(Exception ex)
            {
                return output;
            }
            return output;
        }

        public async Task<bool> PostShopingCartAction(ShoppingCartDetailDTO model)
        {
            var item = HanomaContext.ShoppingCartDetail.Find(model.Id);
            if(item !=null)
            {
                item.StatusCart = model.StatusCart;
                item.ReasonCancel = model.ReasonCancel; // Lý do hủy
                item.IsCancelByBuyer = model.IsCancelByBuyer;
                item.LastEditBy = Guid.Parse(model.UserId);
                item.LastEditDate = DateTime.Now;
                await HanomaContext.SaveChangesAsync();
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}
