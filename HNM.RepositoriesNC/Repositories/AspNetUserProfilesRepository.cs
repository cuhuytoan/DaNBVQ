using HNM.DataNC.Models;
using HNM.RepositoriesNC.RepositoriesBase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace HNM.RepositoriesNC.Repositories
{
    public interface IAspNetUserProfilesRepository : IRepositoryBase<AspNetUserProfiles>
    {
        void UpdateProfilers(AspNetUserProfiles model);
        void UpdateAvatar(AspNetUserProfiles model);
        Task<object> UpdateProductBrandToZero(string userId);
    }
    public class AspNetUserProfilesRepository : RepositoryBase<AspNetUserProfiles>, IAspNetUserProfilesRepository
    {
        public AspNetUserProfilesRepository(HanomaContext HanomaContext) : base(HanomaContext)
        {
        }

        public void UpdateAvatar(AspNetUserProfiles model)
        {

        }

        public async Task<object> UpdateProductBrandToZero(string userId)
        {
            var pUserId = Guid.Parse(userId);
            try
            {                
                var deleteProdBrand =  await HanomaContext.ProductBrand.FirstOrDefaultAsync(p => p.CreateBy == pUserId);
                if(deleteProdBrand !=null)
                {
                    HanomaContext.ProductBrand.RemoveRange(deleteProdBrand);
                    HanomaContext.SaveChanges();
                }
                var updateAspNetUserProfiles = await HanomaContext.AspNetUserProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
                if(updateAspNetUserProfiles !=null)
                {
                    updateAspNetUserProfiles.ProductBrand_ID = 0;
                    HanomaContext.SaveChanges();
                }                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new object();
        }

        public void UpdateProfilers(AspNetUserProfiles model)
        {
            Update(model);
        }
    }
}
