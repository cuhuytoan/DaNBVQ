using HNM.DataNC.Models;
using HNM.RepositoriesNC.RepositoriesBase;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HNM.RepositoriesNC.Repositories
{
    public interface IKeywordRepository : IRepositoryBase<MetaKeyword>
    {
        Task<IEnumerable<MetaKeyword>> GetSuggestKeyword(int topX = 10, string key = "");
    }
    public class KeywordRepository : RepositoryBase<MetaKeyword>, IKeywordRepository
    {
        public KeywordRepository(HanomaContext HanomaContext) : base(HanomaContext)
        {
        }

        public async Task<IEnumerable<MetaKeyword>> GetSuggestKeyword(int topX = 10, string key = "")
        {
            if (string.IsNullOrEmpty(key))
                return GetMetaKeywordDefault();
            return await HanomaContext.MetaKeyword

                .FromSqlRaw($"SELECT TOP {topX} * FROM dbo.MetaKeyword WITH (NOLOCK) WHERE Type IS NULL AND Name LIKE N'{key}%'")
                .AsNoTracking()
                .ToListAsync();
        }
        public List<MetaKeyword> GetMetaKeywordDefault()
        {
            var res = new List<MetaKeyword>();
            res.AddRange(new[] {
                new MetaKeyword { MetaKeyword_ID = 1, Name = "Máy xúc"},
                new MetaKeyword { MetaKeyword_ID = 2, Name = "Máy hàn"},
                new MetaKeyword { MetaKeyword_ID = 3, Name = "Máy ủi"},
                new MetaKeyword { MetaKeyword_ID = 4, Name = "Máy tiện"},
                new MetaKeyword { MetaKeyword_ID = 5, Name = "Xe cứu hộ"},
                new MetaKeyword { MetaKeyword_ID = 6, Name = "Xe tải"},
                new MetaKeyword { MetaKeyword_ID = 7, Name = "Xe ben"},
            });
            return res;
        }
    }
}
