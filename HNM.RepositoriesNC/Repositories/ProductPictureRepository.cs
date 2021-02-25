using HNM.DataNC.Models;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.DataNC.ModelsNC.ModelsUtility;
using HNM.RepositoriesNC.RepositoriesBase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HNM.RepositoriesNC.Repositories
{
    public interface IProductPictureRepository : IRepositoryBase<ProductPicture>
    {
        Task<IEnumerable<ProductPictureDTO>> GetByProductId(int productId);
        Task<IEnumerable<ProductPictureDTO>> ShopmanGetByProductId(int productId);
        Task<List<DeleteImageProductPicture>> GetListDeleteProdPicture(int productId);
    }
    public class ProductPictureRepository : RepositoryBase<ProductPicture>, IProductPictureRepository
    {
        public ProductPictureRepository(HanomaContext HanomaContext) : base(HanomaContext)
        {
        }
        public async Task<IEnumerable<ProductPictureDTO>> GetByProductId(int productId)
        {
            var productPicture = await HanomaContext.ProductPicture.Where(p => p.Product_ID == productId).AsNoTracking().ToListAsync();            
            var product = HanomaContext.Product.FirstOrDefault(x => x.Product_ID == productId);
            var prodPicture = new List<ProductPictureDTO>();
            //Add sub Image
            foreach(var p in productPicture)
            {
                prodPicture.Add(new ProductPictureDTO
                {
                    ProductPicture_ID = p.ProductPicture_ID,
                    Image = p.Image,
                    UrlImage = Utils.CloudPath() + $"/productpicture/mainimages/small/{p.Image}"
                });
            }
            //Add main Image
            prodPicture.Add(new ProductPictureDTO
            {
                ProductPicture_ID = new Guid(),
                Image = product.Image,
                UrlImage = Utils.CloudPath() + $"/product/mainimages/small/{product.Image}"
            });

            return prodPicture;
        }

        public async Task<List<DeleteImageProductPicture>> GetListDeleteProdPicture(int productId)
        {
            var output = new List<DeleteImageProductPicture>();
            var prodPicture = await HanomaContext.ProductPicture.Where(p => p.Product_ID == productId).ToListAsync();
            if(prodPicture.Count > 0)
            {
                foreach(var p in prodPicture)
                {
                    DeleteImageProductPicture item = new DeleteImageProductPicture();
                    item.ProductPicture_ID = p.ProductPicture_ID.ToString();
                    output.Add(item);
                }
            }
            return output;
        }

        public async Task<IEnumerable<ProductPictureDTO>> ShopmanGetByProductId(int productId)
        {
            var productPicture = await HanomaContext.ProductPicture.Where(p => p.Product_ID == productId).AsNoTracking().ToListAsync();
            var product = HanomaContext.Product.FirstOrDefault(x => x.Product_ID == productId);
            var prodPicture = new List<ProductPictureDTO>();
            //Add sub Image
            foreach (var p in productPicture)
            {
                prodPicture.Add(new ProductPictureDTO
                {
                    ProductPicture_ID = p.ProductPicture_ID,
                    Image = p.Image,
                    UrlImage = Utils.CloudPath() + $"/productpicture/mainimages/small/{p.Image}"
                });
            }
            return prodPicture;
        }
    }
}
