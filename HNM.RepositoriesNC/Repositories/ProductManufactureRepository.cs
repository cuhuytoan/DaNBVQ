using HNM.DataNC.Models;
using HNM.RepositoriesNC.RepositoriesBase;

namespace HNM.RepositoriesNC.Repositories
{
    public interface IProductManufactureRepository : IRepositoryBase<ProductManufacture>
    {
        //Task<IEnumerable<ProductPicture>> GetByProductId(int productId);
    }
    public class ProductManufactureRepository : RepositoryBase<ProductManufacture>, IProductManufactureRepository
    {
        public ProductManufactureRepository(HanomaContext HanomaContext) : base(HanomaContext)
        {
        }
        //public async Task<IEnumerable<ProductPicture>> GetByProductId(int productId)
        //{
        //    return await HanomaContext.ProductPicture.Where(p => p.Product_ID == productId).AsNoTracking().ToListAsync();
        //}
    }
}
