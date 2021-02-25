using HNM.DataNC.Models;
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
    public interface IDashBoardRepository : IRepositoryBase<AspNetUserProfiles>
    {
        Task<List<DashBoashProductBrand_Result>> GetDashBoard(string UserId);
        Task<List<GetDashBoard_Result>> GetDashBoardPharse2(string UserI, int? CustomerFilter, int? PostFilter, int? OrderFilter, int? FinanceFilter);
    }
    public class DashBoardRepository : RepositoryBase<AspNetUserProfiles>, IDashBoardRepository
    {
        public DashBoardRepository(HanomaContext HanomaContext) : base(HanomaContext)
        {
        }

        public async Task<List<DashBoashProductBrand_Result>> GetDashBoard(string UserId)
        {
            var userProfile = HanomaContext.AspNetUserProfiles.FirstOrDefault(p => p.UserId == UserId);
            if (userProfile != null && userProfile.ProductBrand_ID > 0)
            {
                try
                {
                    //init param
                    var Tag = new SqlParameter("@pTag", (object)DBNull.Value);
                    var ProductBrandID = new SqlParameter("@pProductBrandID", userProfile.ProductBrand_ID);

                    var result = await HanomaContext.Set<DashBoashProductBrand_Result>()
                    .FromSqlRaw($"EXECUTE dbo.DashBoashProductBrand "
                    + $"@pTag = @pTag, "
                    + $"@pProductBrandID = @pProductBrandID ", Tag, ProductBrandID)
                    .AsNoTracking()
                    .ToListAsync();
                    return result;
                }
                catch (Exception ex)
                {

                }
            }
            return new List<DashBoashProductBrand_Result>();


        }

        public async Task<List<GetDashBoard_Result>> GetDashBoardPharse2(string UserId, int? CustomerFilter, int? PostFilter, int? OrderFilter, int? FinanceFilter)
        {
            var userProfile = HanomaContext.AspNetUserProfiles.FirstOrDefault(p => p.UserId == UserId);
            if (userProfile != null && userProfile.ProductBrand_ID > 0)
            {
                try
                {
                    //init param                    
                    var pProductBrandID = new SqlParameter("@pProductBrandID", userProfile.ProductBrand_ID);
                    var pCustomerFilter = new SqlParameter("@pCustomerFilter", CustomerFilter ?? (object)DBNull.Value);
                    var pPostFilter = new SqlParameter("@pPostFilter", PostFilter ?? (object)DBNull.Value);
                    var pOrderFilter = new SqlParameter("@pOrderFilter", OrderFilter ?? (object)DBNull.Value);
                    var pFinanceFilter = new SqlParameter("@pFinanceFilter", FinanceFilter ?? (object)DBNull.Value);

                    var result = await HanomaContext.Set<GetDashBoard_Result>()
                    .FromSqlRaw($"EXECUTE dbo.GetDashBoard "
                    + $"@pProductBrandID = @pProductBrandID, "
                    + $"@pCustomerFilter = @pCustomerFilter, "
                    + $"@pPostFilter = @pPostFilter, "
                    + $"@pOrderFilter = @pOrderFilter, "
                    + $"@pFinanceFilter = @pFinanceFilter ", pProductBrandID, pCustomerFilter, pPostFilter, pOrderFilter, pFinanceFilter)
                    .AsNoTracking()
                    .ToListAsync();
                    return result;
                }
                catch (Exception ex)
                {

                }
            }
            return new List<GetDashBoard_Result>();
        }
    }
}
