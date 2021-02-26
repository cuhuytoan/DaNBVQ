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
    public interface IProductCategoryRepository : IRepositoryBase<ProductCategory>
    {
        Task<IEnumerable<ProductCategory>> GetTopCateByParentId(string keyword,int? parentId);
        Task<IEnumerable<ProductCategory>> GetTopCateByParentIdTakeTop(int parentId, int Top);
        Task<IEnumerable<ProductCategory>> GetProdCateByBrand(int? ProductBrandID);
        Task<IEnumerable<ProductCategoryTwoLevelDTO>> GetListProductCategoryTwoLevelDTO(string keyword, int? ProductCategoryId);
        Task<IEnumerable<GetMenuProdCateByUser_Result>> GetMenuProdCateByUser(string UserId,  int? productTypeId, int? statusTypeId, int parentId = 656);

        Task<IEnumerable<ProductCategory>> GetAllProductcategory();


    }

    public class ProductCategoryRepository : RepositoryBase<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(HanomaContext HanomaContext) : base(HanomaContext)
        {
        }

        public async Task<IEnumerable<ProductCategory>> GetTopCateByParentId(string keyword,int? parentId)
        {          
            if(!string.IsNullOrEmpty(keyword))
            {
                return await HanomaContext.ProductCategory.Where(x => x.Parent_ID == parentId && x.Name.Contains(keyword)).OrderBy(x => x.Sort)
                .AsNoTracking()
                .ToListAsync();
            }
            else
            {
                return await HanomaContext.ProductCategory.Where(x => x.Parent_ID == parentId ).OrderBy(x => x.Sort)
                .AsNoTracking()
                .ToListAsync();
            }
            
        }
        public async Task<IEnumerable<ProductCategory>> GetTopCateByParentIdTakeTop(int parentId, int Top)
        {
            return await HanomaContext.ProductCategory.Where(x => x.Parent_ID == parentId).OrderBy(x => x.ProductCategory_ID).Take(Top)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductCategory>> GetProdCateByBrand(int? ProductBrandID)
        {
            var result = HanomaContext.Product.Where(p => p.ProductBrand_ID == ProductBrandID)
                .Select(x => x.ProductCategory_ID)
                .Distinct()
                .ToList();
            return await HanomaContext.ProductCategory.Where(p => result.Contains(p.ProductCategory_ID))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductCategoryTwoLevelDTO>> GetListProductCategoryTwoLevelDTO(string keyword, int? ProductCategoryId)
        {
            if (String.IsNullOrEmpty(keyword))
            {
                keyword = "";
            }
            var output = new List<ProductCategoryTwoLevelDTO>();
            var prodCateID = ProductCategoryId ?? 654;
            var lstParent = await HanomaContext.ProductCategory.Where(p => p.Parent_ID == prodCateID).OrderBy(x=>x.Sort).ToListAsync();
            foreach (var item in lstParent)
            {
                ProductCategoryTwoLevelDTO prodItem = new ProductCategoryTwoLevelDTO();
                prodItem.ParentCategoryName = item.Name;
                prodItem.ParentCategoryUrl = item.URL;
                prodItem.data = await HanomaContext.ProductCategory.Where(x => x.Parent_ID == item.ProductCategory_ID && x.Name.Contains(keyword)).OrderBy(x => x.Sort).ToListAsync();
                output.Add(prodItem);
            }
            return output;
        }

        public async Task<IEnumerable<GetMenuProdCateByUser_Result>> GetMenuProdCateByUser(string UserId, int? productTypeId, int? statusTypeId, int parentId = 656)
        {
            var pUserId = new SqlParameter("@UserId", UserId ?? (object)DBNull.Value);
            var pParenId = new SqlParameter("@ParenId", parentId);
            var pProductTypeId = new SqlParameter("@ProductTypeId", productTypeId ?? (object)DBNull.Value);
            var pStatusTypeId = new SqlParameter("@StatusTypeId", statusTypeId ?? (object)DBNull.Value);
            try
            {
                var output = await HanomaContext.Set<GetMenuProdCateByUser_Result>()
                    .FromSqlRaw($"EXECUTE GetMenuProdCateByUser @UserId = @UserId, "
                    + $"@ParenId = @ParenId, "
                    + $"@ProductTypeId = @ProductTypeId, "
                    + $"@StatusTypeId = @StatusTypeId ",
                    pUserId, pParenId, pProductTypeId, pStatusTypeId)
                    .AsNoTracking()
                    .ToListAsync();
                return output;
            }
            catch(Exception ex)
            {
                return new List<GetMenuProdCateByUser_Result>();
            }
        }

        public async Task<IEnumerable<ProductCategory>> GetAllProductcategory()
        {
            return await HanomaContext.ProductCategory.Where(p => p.Parent_ID == null).OrderBy(x => x.Sort)
               .AsNoTracking()
               .ToListAsync();
        }
    }
}
