using HNM.DataNC.Models;
using HNM.DataNC.ModelsStore;
using HNM.RepositoriesNC.RepositoriesBase;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HNM.RepositoriesNC.Repositories
{
    public interface IManufactureRepository : IRepositoryBase<ProductManufacture>
    {
        Task<IEnumerable<ProductManufacture_SearchByCategory_Result>> SearchManufacture(int? ProductCategory_ID, int? ProductType_ID, string text = "");
        Task<IEnumerable<ProductManufacture>> GetShopmanListManufacture(string keyword);
    }
    public class ManufactureRepository : RepositoryBase<ProductManufacture>, IManufactureRepository
    {
        public ManufactureRepository(HanomaContext HanomaContext) : base(HanomaContext)
        {
        }

        public async Task<IEnumerable<ProductManufacture_SearchByCategory_Result>> SearchManufacture(int? productCategoryId, int? productTypeId, string text = "")
        {
            var ProductCategory_ID = new SqlParameter("@ProductCategory_ID", productCategoryId ?? (object)DBNull.Value);
            var ProductType_ID = new SqlParameter("@ProductType_ID", productTypeId ?? (object)DBNull.Value);
            var ItemCount = new SqlParameter("@ItemCount", "")
            {
                Direction = System.Data.ParameterDirection.Output,
                DbType = System.Data.DbType.Int32
            };

            var output = await HanomaContext.Set<ProductManufacture_SearchByCategory_Result>()
                .FromSqlRaw($"EXECUTE dbo.ProductManufacture_SearchByCategory @ProductCategory_ID = @ProductCategory_ID, @ProductType_ID = @ProductType_ID, @ItemCount =  @ItemCount OUTPUT", ProductCategory_ID, ProductType_ID, ItemCount)
                .AsNoTracking()
                .ToListAsync();
            if (!String.IsNullOrEmpty(text))
            {
                output = output.Where(x => x.Name.Contains(text)).ToList();
            }
            return output;
        }
        public async Task<IEnumerable<ProductManufacture>> GetShopmanListManufacture(string keyword)
        {
            return await HanomaContext.ProductManufacture.Where(p => p.Name.Contains(keyword)).ToListAsync();
        }
    }
}
