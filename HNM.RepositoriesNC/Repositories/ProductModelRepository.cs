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
    public interface IProductModelRepository : IRepositoryBase<ProductModel>
    {
        Task<IEnumerable<ProductModel_SearchByCategory_Result>> SearchProductModel(int? productCategoryId, int? productTypeId, int? manufactoryId, string search);
        Task<ProductModel> PostProductModel(ProductModel model, string UserId);
        Task<IEnumerable<ProductModel>> ShopmanSearchProductModel(int? productCategoryId, int? productTypeId, int? manufactoryId, string search);
    }
    public class ProductModelRepository : RepositoryBase<ProductModel>, IProductModelRepository
    {
        public ProductModelRepository(HanomaContext HanomaContext) : base(HanomaContext)
        {
        }

        public async Task<ProductModel> PostProductModel(ProductModel model, string UserId)
        {
            try
            {
                var pUser = Guid.Parse(UserId);
                model.Name = model.Name.ToUpper();
                model.CreateBy = pUser;
                model.LastEditBy = pUser;
                model.CreateDate = DateTime.Now;
                model.LastEditDate = DateTime.Now;
                model.IsEnable = true;
                model.StatusType = 1; // mới thêm
                HanomaContext.ProductModel.Add(model);
                HanomaContext.SaveChanges();

            }
            catch (Exception ex)
            {
                model.ProductModel_ID = 0;
                throw ex;
            }
            return model;
        }

        public async Task<IEnumerable<ProductModel_SearchByCategory_Result>> SearchProductModel(int? productCategoryId, int? productTypeId, int? manufactoryId, string search)
        {
            var ProductCategory_ID = new SqlParameter("@ProductCategory_ID", productCategoryId ?? (object)DBNull.Value);
            var ProductType_ID = new SqlParameter("@ProductType_ID", productTypeId ?? (object)DBNull.Value);
            var ProductManufacture_ID = new SqlParameter("@ProductManufacture_ID", manufactoryId ?? (object)DBNull.Value);
            var ItemCount = new SqlParameter("@ItemCount", "")
            {
                Direction = System.Data.ParameterDirection.Output,
                DbType = System.Data.DbType.Int32
            };

            var output = await HanomaContext.Set<ProductModel_SearchByCategory_Result>()
                .FromSqlRaw($"EXECUTE dbo.ProductModel_SearchByCategory @ProductCategory_ID = @ProductCategory_ID, @ProductType_ID = @ProductType_ID, @ProductManufacture_ID = @ProductManufacture_ID, @ItemCount =  @ItemCount OUTPUT",
                ProductCategory_ID, ProductType_ID, ProductManufacture_ID, ItemCount)
                .AsNoTracking()
                .ToListAsync();
            if (!String.IsNullOrEmpty(search))
            {
                output = output.Where(x => x.Name.Contains(search)).ToList();
            }
            return output;
        }

        public async Task<IEnumerable<ProductModel>> ShopmanSearchProductModel(int? productCategoryId, int? productTypeId, int? manufactoryId, string search)
        {

            return await HanomaContext.ProductModel.Where(p => p.ProductCategory_ID == productCategoryId && p.ProductManufacture_ID == manufactoryId && p.Name.Contains(search)).ToListAsync();

        }
    }
}
