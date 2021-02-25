using HNM.DataNC.Models;
using HNM.DataNC.ModelsFilter;
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
    public interface ICurriculumVitaeRepository : IRepositoryBase<CurriculumVitae>
    {
        Task<IEnumerable<CurriculumVitae>> GetTopCV(int? TopX);
        Task<IEnumerable<CV_Search_Result>> GetListCV(CVFilter filter);
        Task<IEnumerable<CurriculumVitae>> GetRelateCurriculumVitae(int? CareerCategoryId, int CurriculumId);
        Task<int> PostCV(CurriculumVitae model,  string UserId);
        Task<IEnumerable<CVPicture>> GetLstPicture(int Id);
        Task DeleteCVIllustrationImages(List<DeleteImageCVPicture> model);
        bool CanEditCV(int Id, string userId);
        Task ActionInCV(int Id, string Action);
        Task<List<DeleteImageCVPicture>> GetListDeleteCVPicture(int Id);
    }
    public class CurriculumVitaeRepository : RepositoryBase<CurriculumVitae>, ICurriculumVitaeRepository
    {
        public CurriculumVitaeRepository(HanomaContext HanomaContext) : base(HanomaContext)
        {

        }

        public async Task<IEnumerable<CV_Search_Result>> GetListCV(CVFilter filter)
        {
            var Keyword = new SqlParameter("@Keyword", String.IsNullOrEmpty(filter.Keyword) ? (object)DBNull.Value : filter.Keyword);
            var CareerCategoryId = new SqlParameter("@CareerCategoryId", filter.CareerCategoryId ?? (object)DBNull.Value);
            var ProductCategoryId = new SqlParameter("@ProductCategoryId", filter.ProductCategoryId ?? (object)DBNull.Value);
            var ProvinceName = new SqlParameter("@ProvinceName", filter.ProvinceName ?? (object)DBNull.Value);
            var YearOfExprience = new SqlParameter("@YearOfExprience", filter.YearOfExprience ?? (object)DBNull.Value);
            var MinSalary = new SqlParameter("@MinSalary", filter.MinSalary ?? (object)DBNull.Value);
            var MaxSalary = new SqlParameter("@MaxSalary", filter.MaxSalary ?? (object)DBNull.Value);
            var StatusTypeId = new SqlParameter("@StatusTypeId", 4);
            var PageSize = new SqlParameter("@PageSize", filter.PageSize);
            var CurrentPage = new SqlParameter("@CurrentPage", filter.Page ?? (object)DBNull.Value);

            try
            {
                var output = await HanomaContext.Set<CV_Search_Result>()
                    .FromSqlRaw($"EXECUTE dbo.CV_Search "
                    + $"@Keyword = @Keyword, "
                    + $"@CareerCategoryId = @CareerCategoryId, "
                    + $"@ProductCategoryId = @ProductCategoryId, "
                    + $"@ProvinceName = @ProvinceName, "
                    + $"@YearOfExprience = @YearOfExprience, "
                    + $"@MinSalary = @MinSalary, "
                    + $"@MaxSalary = @MaxSalary, "
                    + $"@StatusTypeId = @StatusTypeId, "
                    + $"@PageSize = @PageSize, "
                    + $"@CurrentPage = @CurrentPage ",
                    Keyword, CareerCategoryId, ProductCategoryId, ProvinceName, YearOfExprience, MinSalary, MaxSalary, StatusTypeId, PageSize, CurrentPage
                    ).AsNoTracking()
                    .ToListAsync();

                return output;
            }
            catch (Exception ex)
            {
                return new List<CV_Search_Result>();
            }
        }

        public async Task<IEnumerable<CurriculumVitae>> GetRelateCurriculumVitae(int? CareerCategoryId, int CurriculumId)
        {
            return await HanomaContext.CurriculumVitae.Where(p => p.CareerCategoryId == CareerCategoryId && p.Id != CurriculumId && p.StatusTypeId == 4).OrderByDescending(x => x.LastEditDate).Take(12).ToListAsync();
        }

        public async Task<IEnumerable<CurriculumVitae>> GetTopCV(int? TopX)
        {
            var output = HanomaContext.CurriculumVitae.Where(x => x.StatusTypeId == 4)
                .OrderByDescending(p => p.LastEditDate)
                .Take(TopX ?? 20)
                .ToList();                
            return output;
        }

        public async Task<int> PostCV(CurriculumVitae model, string UserId)
        {
            try
            {
                var pUserId = Guid.Parse(UserId);
                var userInfo = HanomaContext.AspNetUserProfiles.FirstOrDefault(p => p.UserId == UserId);
                if(model !=null)
                {
                    var CVSave = HanomaContext.CurriculumVitae.Find(model.Id);
                    if(CVSave == null) // Add new 
                    {
                        model.LastEditDate = DateTime.Now;
                        model.CreateDate = DateTime.Now;
                        model.LastEditBy = pUserId;
                        model.CreateBy = pUserId;
                        model.StatusTypeId = 2;
                        HanomaContext.CurriculumVitae.Add(model);
                        await HanomaContext.SaveChangesAsync();
                        //CreateUrl
                        await CreateCVURL(model.Id);
                    }    
                    else // Update
                    {
                        CVSave.FullName = model.FullName;
                        CVSave.YearOfBirth = model.YearOfBirth;
                        CVSave.DOB = model.DOB;
                        CVSave.ProductCateRelate = model.ProductCateRelate;
                        CVSave.Gender = model.Gender;
                        CVSave.ExprienceDescription = model.ExprienceDescription;
                        CVSave.PlaceOfBirth = model.PlaceOfBirth;
                        CVSave.HomeTown = model.HomeTown;
                        CVSave.Phone = model.Phone;
                        CVSave.Email = model.Email;
                        CVSave.Certificate = model.Certificate;                        
                        CVSave.MainOccupation = model.MainOccupation;
                        CVSave.CareerCategoryId = model.CareerCategoryId;
                        CVSave.Degree = model.Degree;
                        CVSave.YearsOfExperience = model.YearsOfExperience;
                        CVSave.TitleName = model.TitleName;
                        CVSave.Salary = model.Salary;
                        CVSave.TypeOfWork = model.TypeOfWork;
                        CVSave.IntroInfomation = model.IntroInfomation;
                        CVSave.LocalWork = model.LocalWork;
                        CVSave.LocationWorkId = model.LocationWorkId;
                        HanomaContext.SaveChanges();
    }
                }   
                
            }
            catch(Exception ex)
            {
                model.Id = 0;
                throw ex;
            }
            return model.Id;
        }

        public async Task CreateCVURL(int Id)
        {
            try
            {
                var currentRec = await HanomaContext.CurriculumVitae.FirstOrDefaultAsync(p => p.Id == Id);
                if (currentRec != null)
                {
                    currentRec.URL = "ho-so-" + FormatURL(currentRec.FullName) + "-" +
                                        currentRec.Id.ToString();
                    await HanomaContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public bool CanEditCV(int Id, string userId)
        {
            if (String.IsNullOrEmpty(userId)) return false;
            var product = HanomaContext.CurriculumVitae.FirstOrDefault(x => x.Id == Id);

            if (product.CreateBy == new Guid(userId) || userId == "06dfe050-4407-4492-b1fd-656ccb8a82b6" || userId == "a4243721-e234-4efc-aec7-048117444572")
            {
                return true;
            }

            return false;
        }

        public async Task DeleteCVIllustrationImages(List<DeleteImageCVPicture> model)
        {
            foreach (var p in model)
            {
                var itemPicture = await HanomaContext.CVPicture.FirstOrDefaultAsync(x => x.CVPicture_ID.ToString() == p.CVPicture_ID);
                if(itemPicture !=null)
                {
                    HanomaContext.CVPicture.Remove(itemPicture);
                    await HanomaContext.SaveChangesAsync();
                }    
            }

        }

        public async Task<IEnumerable<CVPicture>> GetLstPicture(int Id)
        {
            return await HanomaContext.CVPicture.Where(p => p.Recruitment_ID == Id).ToListAsync();
        }

        public async Task ActionInCV(int Id, string Action)
        {
            var result = await HanomaContext.CurriculumVitae.FirstOrDefaultAsync(p => p.Id == Id);
            if(result !=null)
            {
                result.StatusTypeId = -1; // hủy đăng
                await HanomaContext.SaveChangesAsync();
            }    

        }

        public async Task<List<DeleteImageCVPicture>> GetListDeleteCVPicture(int Id)
        {
            var output = new List<DeleteImageCVPicture>();
            var prodPicture = await HanomaContext.CVPicture.Where(p => p.Recruitment_ID == Id).ToListAsync();
            if (prodPicture.Count > 0)
            {
                foreach (var p in prodPicture)
                {
                    DeleteImageCVPicture item = new DeleteImageCVPicture();
                    item.CVPicture_ID = p.CVPicture_ID.ToString();
                    output.Add(item);
                }
            }
            return output;
        }
    }
}
