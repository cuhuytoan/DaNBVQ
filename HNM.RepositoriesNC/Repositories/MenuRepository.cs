using HNM.DataNC.Models;
using HNM.DataNC.ModelsStore;
using HNM.RepositoriesNC.RepositoriesBase;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HNM.RepositoriesNC.Repositories
{
    public interface IMenuRepository : IRepositoryBase<Menu>
    {
        Task<IEnumerable<Menu>> GetMenus();
        Task<IEnumerable<Menu>> GetMenusAccount(int ParentId);
        Task<IEnumerable<Menu>> GetMenusByParentId(int ParentId);
        Task<IEnumerable<GetMenuStatusByUser_Result>> GetMenusStatusByUser(string UserId, int? ProductTypeId);
    }
    public class MenuRepository : RepositoryBase<Menu>, IMenuRepository
    {
        public MenuRepository(HanomaContext HanomaContext) : base(HanomaContext)
        {
        }

        public async Task<IEnumerable<Menu>> GetMenus()
        {
            try
            {
                return await HanomaContext.Menu
                    .FromSqlRaw($"select * from dbo.Menu with (nolock) where Parent_ID Is null and Active = 1 order by Sort asc")
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {

            }
            return new List<Menu>();
        }

        public async Task<IEnumerable<Menu>> GetMenusAccount(int ParentId)
        {
            try
            {
                return await HanomaContext.Menu
                    .FromSqlRaw($"select * from  dbo.Menu with (nolock) where  Parent_ID = {ParentId} and Active = 1  order by Sort asc")
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {

            }
            return new List<Menu>();
        }

        public async Task<IEnumerable<Menu>> GetMenusByParentId(int ParentId)
        {
            try
            {
                return await HanomaContext.Menu
                   .FromSqlRaw($"select * from dbo.Menu with (nolock) where  Parent_ID = {ParentId} and Active = 1 order by Sort asc")
                   .AsNoTracking()
                   .ToListAsync();
            }
            catch (Exception ex)
            {

            }
            return new List<Menu>();
        }

        public async Task<IEnumerable<GetMenuStatusByUser_Result>> GetMenusStatusByUser(string UserId, int? ProductTypeId)
        {
            var pUserId = new SqlParameter("@UserId", UserId ?? (object)DBNull.Value);            
            var pProductTypeId = new SqlParameter("@ProductTypeId", ProductTypeId ?? (object)DBNull.Value);
            
            try
            {
                var output = await HanomaContext.Set<GetMenuStatusByUser_Result>()
                    .FromSqlRaw($"EXECUTE GetMenuStatusByUser @UserId = @UserId, "                 
                    + $"@ProductTypeId = @ProductTypeId ",                  
                    pUserId, pProductTypeId)
                    .AsNoTracking()
                    .ToListAsync();
                return output;
            }
            catch (Exception ex)
            {
                return new List<GetMenuStatusByUser_Result>();
            }
        }
    }
}
