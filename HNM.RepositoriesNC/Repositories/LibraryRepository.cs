using HNM.DataNC.Models;
using HNM.DataNC.ModelsFilter;
using HNM.DataNC.ModelsNC.ModelsDTO;
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
    public interface ILibraryRepository : IRepositoryBase<Library>
    {
        Task<IEnumerable<Library_Search_By_Cate_Result>> GetListLibraryPagging(LibraryFilter filter);
        Library GetLibDetail(int LibraryId);
        Task<IEnumerable<Library>> GetRelateLibrary(int LibraryCategoryId, int LibraryId);
        Task<int> PostLibrary(Library model, ImageUploadDTO imgLogo, string UserId);
        Task UpdateImgLogoLibrary(string fileName, int LibraryId);
    }
    public class LibraryRepository : RepositoryBase<Library>, ILibraryRepository
    {
        public LibraryRepository(HanomaContext HanomaContext) : base(HanomaContext)
        {
        }

        public Library GetLibDetail(int LibraryId)
        {
            return HanomaContext.Library.FirstOrDefault(p => p.Library_ID == LibraryId);
        }

        public async Task<IEnumerable<Library_Search_By_Cate_Result>> GetListLibraryPagging(LibraryFilter filter)
        {
            try
            {
                var Keyword = new SqlParameter("@Keyword", filter.Keyword ?? (object)DBNull.Value);
                var StatusType_ID = new SqlParameter("@StatusType_ID", filter.StatusTypeId);
                var PageSize = new SqlParameter("@PageSize", filter.PageSize);
                var CurrentPage = new SqlParameter("@CurrentPage", filter.Page ?? 1);
                var LibraryCategory_ID = new SqlParameter("@LibraryCategory_ID", filter.LibraryCategoryID ?? (object)DBNull.Value);
                var ProductCategoryID = new SqlParameter("@ProductCategoryID", filter.ProductCategoryID ?? (object)DBNull.Value);
                var ManufactoriesID = new SqlParameter("@ManufactoriesID", filter.ManufactoriesID ?? (object)DBNull.Value);
                var ModelID = new SqlParameter("@ModelID", filter.ModelID ?? (object)DBNull.Value);
                var ProductCategoryID2 = new SqlParameter("@ProductCategoryID2", filter.ProductCategoryID2 ?? (object)DBNull.Value);
                var CreateBy = new SqlParameter("@CreateBy", filter.CreateBy ?? (object)DBNull.Value);

                var output = await HanomaContext.Set<Library_Search_By_Cate_Result>()
                    .FromSqlRaw($"EXECUTE dbo.Library_Search_By_Cate "
                    + $"@Keyword = @Keyword, "
                    + $"@StatusType_ID = @StatusType_ID, "
                    + $"@PageSize = @PageSize, "
                    + $"@CurrentPage = @CurrentPage, "
                    + $"@LibraryCategory_ID = @LibraryCategory_ID, "
                    + $"@ProductCategoryID = @ProductCategoryID, "
                    + $"@ManufactoriesID = @ManufactoriesID, "
                    + $"@ModelID = @ModelID, "
                    + $"@ProductCategoryID2 = @ProductCategoryID2, "
                    + $"@CreateBy = @CreateBy ",
                    Keyword, StatusType_ID, PageSize, CurrentPage, LibraryCategory_ID, ProductCategoryID,
                    ManufactoriesID, ModelID, ProductCategoryID2,CreateBy)
                    .AsNoTracking()
                    .ToListAsync();
                return output;
            }
            catch(Exception ex)
            {
                return new List<Library_Search_By_Cate_Result>();
            }
        }

        public async Task<IEnumerable<Library>> GetRelateLibrary(int LibraryCategoryId, int LibraryId)
        {
            return await HanomaContext.Library.Where(p => p.LibraryCategory_ID == LibraryCategoryId
            && p.Library_ID != LibraryId && p.StatusType_ID == 4).OrderByDescending(x => x.LastEditDate).Take(5).ToListAsync();
        }

        public async Task<int> PostLibrary(Library model, ImageUploadDTO imgLogo, string UserId)
        {

            var pUserId = Guid.Parse(UserId);
            if (model != null)
            {
                var libSave = HanomaContext.Library.Find(model.Library_ID);
                if (libSave == null)
                {
                    var libraryNew = new Library
                    {
                        LibraryCategory_ID = model.LibraryCategory_ID,
                        LibraryType_ID = model.LibraryType_ID,
                        Title = model.Title,
                        SubTitle = model.SubTitle,
                        Image = model.Image,
                        Description = model.Description,
                        Content = model.Content,
                        Author = model.Author,
                        CreateDate = DateTime.Now,
                        Counter = model.Counter,
                        Active = true,
                        URL = null,
                        CreateBy = new Guid(UserId),
                        LastEditBy = new Guid(UserId),
                        LastEditDate = model.LastEditDate,
                        Tag = model.Tag,
                        Keyword = model.Keyword,
                        AllowComment = model.AllowComment,
                        MetaTitle = model.MetaTitle,
                        MetaDescription = model.MetaDescription,
                        AuthorPhone = model.AuthorPhone,
                        MetaKeywords = model.MetaKeywords,
                        AuthorEmail = model.AuthorEmail,
                        AuthorJob = model.AuthorJob,
                        AuthorCompany = model.AuthorCompany,
                        ProductCategoryID = model.ProductCategoryID,
                        ManufactoryID = model.ManufactoryID,
                        ModelID = model.ModelID,
                        StatusType_ID = 2,
                    };
                    try
                    {
                        HanomaContext.Library.Add(libraryNew);
                        await HanomaContext.SaveChangesAsync();

                        UpdateUrl(libraryNew.Library_ID);
                        return libraryNew.Library_ID;
                    }
                    catch (Exception ex)
                    {
                        libraryNew.Library_ID = 0;
                        return libraryNew.Library_ID;
                    }
                }
                else
                {
                    try
                    {
                        var urlNew = FormatURL(model.Title) + "-" + model.Library_ID.ToString();

                        libSave.LibraryCategory_ID = model.LibraryCategory_ID;
                        libSave.LibraryType_ID = model.LibraryType_ID;
                        libSave.Title = model.Title;
                        libSave.SubTitle = model.SubTitle;
                        libSave.Image = model.Image;
                        libSave.Description = model.Description;
                        libSave.Content = model.Content;
                        libSave.Author = model.Author;
                        libSave.CreateDate = DateTime.Now;
                        libSave.Counter = model.Counter;
                        libSave.Active = true;
                        libSave.URL = null;
                        libSave.CreateBy = new Guid(UserId);
                        libSave.LastEditBy = new Guid(UserId);
                        libSave.LastEditDate = model.LastEditDate;
                        libSave.Tag = model.Tag;
                        libSave.Keyword = model.Keyword;
                        libSave.AllowComment = model.AllowComment;
                        libSave.MetaTitle = model.MetaTitle;
                        libSave.MetaDescription = model.MetaDescription;
                        libSave.AuthorPhone = model.AuthorPhone;
                        libSave.MetaKeywords = model.MetaKeywords;
                        libSave.AuthorEmail = model.AuthorEmail;
                        libSave.AuthorJob = model.AuthorJob;
                        libSave.AuthorCompany = model.AuthorCompany;
                        libSave.ProductCategoryID = model.ProductCategoryID;
                        libSave.ManufactoryID = model.ManufactoryID;
                        libSave.ModelID = model.ModelID;
                        libSave.StatusType_ID = -1;
                        await HanomaContext.SaveChangesAsync();

                        return model.Library_ID;
                    }
                    catch (Exception ex)
                    {
                        return 0;
                    }
                }

            }
            return model.Library_ID;

        }

        public void UpdateUrl(int libraryId)
        {
            try
            {
                var lib = HanomaContext.Library.FirstOrDefault(x => x.Library_ID == libraryId);
                var urlNew = FormatURL(lib.Title) + "-" + lib.Library_ID.ToString();
                lib.URL = urlNew;
                HanomaContext.SaveChanges();
            }
            catch (Exception exception)
            {
                //log.Error(exception);

            }

        }

        public async Task UpdateImgLogoLibrary(string fileName, int LibraryId)
        {
            var lib = HanomaContext.Library.Find(LibraryId);
            if (lib != null)
            {
                lib.Image = fileName;
                HanomaContext.SaveChanges();
            }
        }
    }
}
