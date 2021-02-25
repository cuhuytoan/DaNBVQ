using HNM.DataNC.Models;
using HNM.DataNC.ModelsNC.ModelsDTO;
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
    public interface IAdvertisingRepository : IRepositoryBase<Advertising>
    {
        Task<IEnumerable<Advertising>> GetCarouselList();
        Task<IEnumerable<Advertising>> GetAdvertisingsByBlockName(string blockName);
        Task<IEnumerable<Advertising>> GetAdvertisingsByBlockId(int blockId);
        Task<IEnumerable<Advertising>> GetAdBlockByCate(int AdBlockID);
        Task<IEnumerable<Advertising>> GetAdvertisingsByListBlockId(string lstBlockId);
        Task<List<ProductHighLight_Search_Result>> GetSponsorProduct(int CategoryId,int? ProductTypeId, int Top);
        Task<List<SpAdMostView_CHT_Result>> GetBannerAdvertisingInChildPage(int AdMostViewType, int AdMostViewCate, int AdMostViewChildCate, int blockId);
        Task<List<QuoteAds>> GetLstQuoteAds();
        Task<bool> PostContactAds(ContactAdsDTO model);

    }
    public class AdvertisingRepository : RepositoryBase<Advertising>, IAdvertisingRepository
    {
        public AdvertisingRepository(HanomaContext HanomaContext) : base(HanomaContext)
        {
        }

        public async Task<IEnumerable<Advertising>> GetCarouselList()
        {
            var output = new List<Advertising>();
            try
            {
                 output = await HanomaContext.Advertising.Where(p => p.AdvertisingBlock_ID == 1 && p.Active == true).OrderBy(x => x.Sort).ToListAsync();
            }
            catch(Exception ex)
            {

            }
            return output;
        }

        public async Task<IEnumerable<Advertising>> GetAdvertisingsByBlockName(string blockName)
        {
            return await HanomaContext.Advertising
                .FromSqlInterpolated(
                    $"SELECT ad.* FROM dbo.Advertising AS ad WITH (NOLOCK) INNER JOIN dbo.AdvertisingBlock AS ab WITH(NOLOCK) ON ab.AdvertisingBlock_ID = ad.AdvertisingBlock_ID WHERE ab.Name = {blockName} AND ad.Active = 1 ORDER BY ad.Sort ASC")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Advertising>> GetAdvertisingsByBlockId(int blockId)
        {
            return await HanomaContext.Advertising
                .FromSqlInterpolated(
                    $"SELECT ad.* FROM dbo.Advertising AS ad WITH (NOLOCK) INNER JOIN dbo.AdvertisingBlock AS ab WITH (NOLOCK) ON ab.AdvertisingBlock_ID = ad.AdvertisingBlock_ID   WHERE ab.AdvertisingBlock_ID = {blockId} AND  ad.Active = 1 ORDER BY ad.Sort ASC")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Advertising>> GetAdBlockByCate(int AdBlockID)
        {
            return HanomaContext.Advertising.Where(p => p.AdvertisingBlock_ID == AdBlockID && p.Active == true).OrderByDescending(x => x.LastEditDate).ToList();
        }

        public async Task<IEnumerable<Advertising>> GetAdvertisingsByListBlockId(string lstBlockId)
        {
            var vcSql =
                $"SELECT ad.* FROM dbo.Advertising AS ad WITH (NOLOCK) INNER JOIN dbo.AdvertisingBlock AS ab WITH (NOLOCK) ON ab.AdvertisingBlock_ID = ad.AdvertisingBlock_ID   WHERE CONVERT(VARCHAR(50),ab.AdvertisingBlock_ID) in ({lstBlockId}) AND ab.IsMobile = 1 AND  ad.Active = 1 ORDER BY ad.Sort ASC";
            return await HanomaContext.Advertising
                .FromSqlRaw(vcSql)
                .AsNoTracking()
                .ToListAsync();

        }

      

        public async Task<List<ProductHighLight_Search_Result>> GetSponsorProduct(int CategoryId, int? ProductTypeId, int Top)
        {
            var pProductCategoryId = new SqlParameter("@ProductCategory_ID", CategoryId);
            var pProductTypeId = new SqlParameter("@ProductType_ID", ProductTypeId);

            var output = await HanomaContext.Set<ProductHighLight_Search_Result>()
                .FromSqlRaw($"EXECUTE dbo.ProductHighLight_Search " +
                $"@ProductCategory_ID = @ProductCategory_ID, " +
                $"@ProductType_ID = @ProductType_ID ", pProductCategoryId, pProductTypeId)
                .AsNoTracking()
                .ToListAsync();
            return output;
        }

        public async Task<List<SpAdMostView_CHT_Result>> GetBannerAdvertisingInChildPage(int AdMostViewType, int AdMostViewCate, int AdMostViewChildCate, int blockId)
        {
            try
            {
                var pAdMostViewType = new SqlParameter("@pAdMostViewType", AdMostViewType);
                var pAdMostViewCate = new SqlParameter("@pAdMostViewCate", AdMostViewCate);
                var pAdMostViewChildCate = new SqlParameter("@pAdMostViewCate2", AdMostViewChildCate);
                var pAdvertisingBlock_ID = new SqlParameter("@pAdvertisingBlock_ID", blockId);

                var output = await HanomaContext.Set<SpAdMostView_CHT_Result>()
                    .FromSqlRaw($"EXECUTE dbo.SpAdMostView_CHT " +
                    $"@pAdMostViewType = @pAdMostViewType, " +
                    $"@pAdMostViewCate = @pAdMostViewCate," +
                    $"@pAdMostViewCate2 = @pAdMostViewCate2," +
                    $"@pAdvertisingBlock_ID = @pAdvertisingBlock_ID ",
                    pAdMostViewType, pAdMostViewCate, pAdMostViewChildCate, pAdvertisingBlock_ID)
                    .AsNoTracking()
                    .ToListAsync();
                return output;
            }catch(Exception ex)
            { }
            return new List<SpAdMostView_CHT_Result>();
        }

        public async Task<List<QuoteAds>> GetLstQuoteAds()
        {
            return await HanomaContext.QuoteAds.ToListAsync();
        }

        public async Task<bool> PostContactAds(ContactAdsDTO model)
        {            
            try
            {
                var modelContact = new ContactAds();
                modelContact.Name = model.Name;
                modelContact.Address = model.Address;
                modelContact.Content = model.Content;
                modelContact.Mobile = model.Mobile;
                modelContact.Email = model.Email;
                modelContact.CreateDate = DateTime.Now;
                modelContact.LastEditDate = DateTime.Now;
                modelContact.Status = 1;

                HanomaContext.ContactAds.Add(modelContact);
                await HanomaContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
