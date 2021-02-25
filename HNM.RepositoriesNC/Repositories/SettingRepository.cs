using HNM.DataNC.Models;
using HNM.RepositoriesNC.RepositoriesBase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace HNM.RepositoriesNC.Repositories
{
    public interface ISettingRepository : IRepositoryBase<Setting>
    {
        Task<Setting> GetSetting();
    }
    public class SettingRepository : RepositoryBase<Setting>, ISettingRepository
    {
        public SettingRepository(HanomaContext HanomaContext) : base(HanomaContext)
        {
        }

        public async Task<Setting> GetSetting()
        {
            return await HanomaContext.Setting.FirstOrDefaultAsync();
        }
    }
}
