using HNM.DataNC.Models;
using HNM.DataNC.ModelsFilter;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.DataNC.ModelsStore;
using HNM.RepositoriesNC.RepositoriesBase;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HNM.RepositoriesNC.Repositories
{
    public interface IRecruitmentRepository : IRepositoryBase<Recruitment>
    {
        Task<IEnumerable<RecruitmentTop>> GetTopRecruitment(int topX);
        Task<IEnumerable<Recruitment_Search_V2_Result>> GetListRecruitmentPagging(RecruitmentFilter filter);
        Task<SentContactRec> SentContactRecruitment(int RecruitmentId, string Name, string Phone, string Email, string Content);
        Task<RecruitmentTop> GetRecruitmentDetails(int Recruitment_ID);
        Task<IEnumerable<Recruitment_Search_V2_Result>> SearchRecruitment(BaseFilter filter);
        Task<IEnumerable<RecruitmentPicture>> GetLstPicture(int Id);
        Task<int> PostRecruitment(Recruitment model, string UserId);
        bool CanEditRec(int Id, string userId);
        Task ActionInRec(int Id, string Action);
        Task DeleteRecIllustrationImages(List<DeleteImageRecPicture> model);
        Task<List<DeleteImageRecPicture>> GetListDeleteRecPicture(int Id);
    }
    public class RecruitmentRepository : RepositoryBase<Recruitment>, IRecruitmentRepository
    {
        public RecruitmentRepository(HanomaContext HanomaContext) : base(HanomaContext)
        {
        }

        public async Task<IEnumerable<RecruitmentTop>> GetTopRecruitment(int topX)
        {

            return await HanomaContext.Set<RecruitmentTop>()
                .FromSqlRaw($"SELECT TOP {topX} R.*,B.Name AS LocationName,ISNULL(D.Image,'noimage.png') AS LogoBrand, E.MinorName AS JobTypeName, F.MinorName AS SalaryTypeName FROM dbo.Recruitment AS R WITH (NOLOCK)" +
                $" LEFT JOIN dbo.Location AS B WITH(NOLOCK) ON B.Location_Id = R.Location_Id" +
                $" LEFT JOIN dbo.ProductBrand AS D WITH(NOLOCK) ON R.ProductBrand_ID = D.ProductBrand_ID" +
                $" LEFT JOIN dbo.Minor AS E ON E.MajorID = 1 AND E.MinorID = R.JobType" +
                $" LEFT JOIN dbo.Minor AS F ON F.MajorID = 2 AND F.MinorID = R.SalaryType" +
                $" ORDER BY R.LastEditDate DESC")
                .AsNoTracking().ToListAsync();

        }
        public async Task<IEnumerable<Recruitment_Search_V2_Result>> GetListRecruitmentPagging(RecruitmentFilter filter)
        {
            var Keyword = new SqlParameter("@Keyword", filter.Keyword ?? (object)DBNull.Value);
            var StatusType_ID = new SqlParameter("@StatusType_ID", filter.StatusTypeId);
            var PageSize = new SqlParameter("@PageSize", filter.PageSize);
            var CurrentPage = new SqlParameter("@CurrentPage", filter.Page);
            var RecruitmentCategoryId = new SqlParameter("@RecruitmentCategory_ID", filter.RecruitmentCategoryId ?? (object)DBNull.Value);
            var ProductCategoryId = new SqlParameter("@ProductCategoryID", filter.ProductCategoryId ?? (object)DBNull.Value);
            var Experience = new SqlParameter("@Experience", filter.Experience ?? (object)DBNull.Value);
            var FromPrice = new SqlParameter("@FromPrice", filter.FromPrice ?? (object)DBNull.Value);
            var ToPrice = new SqlParameter("@ToPrice", filter.ToPrice ?? (object)DBNull.Value);
            var LocationId = new SqlParameter("@Location_ID", filter.LocationId ?? (object)DBNull.Value);

            try
            {
                var output = await HanomaContext.Set<Recruitment_Search_V2_Result>()
                    .FromSqlRaw($"EXECUTE dbo.Recruitment_Search_V2 @Keyword = @Keyword," +
                    $"@RecruitmentCategory_ID = @RecruitmentCategory_ID , " +
                    $"@StatusType_ID = @StatusType_ID, " +
                    $"@PageSize = @PageSize, " +
                    $"@CurrentPage = @CurrentPage, "
                    + $"@ProductCategoryID = @ProductCategoryID, "
                    + $"@Experience = @Experience, "
                    + $"@FromPrice = @FromPrice, "
                    + $"@ToPrice = @ToPrice, "
                    + $"@Location_ID = @Location_ID "
                    , Keyword, RecruitmentCategoryId, StatusType_ID, PageSize, CurrentPage,
                    ProductCategoryId, Experience, FromPrice, ToPrice, LocationId)
                    .AsNoTracking()
                    .ToListAsync();
                return output;
            }
            catch (Exception ex)
            {

            }
            return new List<Recruitment_Search_V2_Result>();
        }

        public async Task<SentContactRec> SentContactRecruitment(int RecruitmentId, string Name, string Phone, string Email, string Content)
        {
            var result = new SentContactRec();
            var recruitment = HanomaContext.Recruitment.FirstOrDefault(x => x.Recruitment_ID == RecruitmentId);
            string MailBody = "";
            WebClient client = new WebClient();
            Stream stream = client.OpenRead("https://hanoma.vn/Content/Emailtemplate/contact-to-employer.html");
            using (StreamReader reader =
                new StreamReader(stream))
            {
                MailBody = reader.ReadToEnd();
            }

            MailBody = MailBody.Replace("{CompanyName}", recruitment.CompanyName);
            MailBody = MailBody.Replace("{RecName}", recruitment.Name);
            MailBody = MailBody.Replace("{RecURL}", recruitment.URL);
            MailBody = MailBody.Replace("{CustName}", Name);
            MailBody = MailBody.Replace("{CustPhone}", Phone);
            MailBody = MailBody.Replace("{CustEmail}", Email);
            MailBody = MailBody.Replace("{CustContent}", Content);

            result.MailBody = MailBody;
            result.ContactEmail = recruitment.ContactEmail;
            result.ContactName = recruitment.ContactName;

            return result;
        }

        public async Task<RecruitmentTop> GetRecruitmentDetails(int Recruitment_ID)
        {
            var RecruitmentID = new SqlParameter("@Recruitment_ID", Recruitment_ID);

            try
            {
                return await HanomaContext.Set<RecruitmentTop>()
                  .FromSqlRaw($"SELECT R.*,B.Name AS LocationName,ISNULL(D.Image,'noimage.png') AS LogoBrand, E.MinorName AS JobTypeName, F.MinorName AS SalaryTypeName FROM dbo.Recruitment AS R WITH (NOLOCK)" +
                  $" LEFT JOIN dbo.Location AS B WITH(NOLOCK) ON B.Location_Id = R.Location_Id" +
                  $" LEFT JOIN dbo.ProductBrand AS D WITH(NOLOCK) ON R.ProductBrand_ID = D.ProductBrand_ID" +
                  $" LEFT JOIN dbo.Minor AS E ON E.MajorID = 1 AND E.MinorID = R.JobType" +
                  $" LEFT JOIN dbo.Minor AS F ON F.MajorID = 2 AND F.MinorID = R.SalaryType" +
                  $" WHERE R.Recruitment_ID = @Recruitment_ID", RecruitmentID)
                  .AsNoTracking().SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {

            }
            return new RecruitmentTop();
        }

        public async Task<IEnumerable<Recruitment_Search_V2_Result>> SearchRecruitment(BaseFilter filter)
        {
            var Keyword = new SqlParameter("@Keyword", filter.Keyword ?? (object)DBNull.Value);
            var StatusType_ID = new SqlParameter("@StatusType_ID", filter.StatusTypeId);
            var PageSize = new SqlParameter("@PageSize", filter.PageSize);
            var CurrentPage = new SqlParameter("@CurrentPage", filter.Page);
            try
            {
                var output = await HanomaContext.Set<Recruitment_Search_V2_Result>()
                    .FromSqlRaw($"EXECUTE dbo.Recruitment_Search_V2 @Keyword = @Keyword," +                    
                    $"@StatusType_ID = @StatusType_ID, " +
                    $"@PageSize = @PageSize, " +
                    $"@CurrentPage = @CurrentPage "                  
                    , Keyword, StatusType_ID, PageSize, CurrentPage)  
                    .AsNoTracking()
                    .ToListAsync();
                return output;
            }
            catch (Exception ex)
            {

            }
            return new List<Recruitment_Search_V2_Result>();
        }

        public async Task<IEnumerable<RecruitmentPicture>> GetLstPicture(int Id)
        {
            return await HanomaContext.RecruitmentPicture.Where(p => p.Recruitment_ID == Id).ToListAsync();
        }

        public async Task<int> PostRecruitment(Recruitment model, string UserId)
        {
            try
            {
                var pUserId = Guid.Parse(UserId);
                var userInfo = HanomaContext.AspNetUserProfiles.FirstOrDefault(p => p.UserId == UserId);
                if(model !=null)
                {
                    var recSave = HanomaContext.Recruitment.Find(model.Recruitment_ID);
                    if(recSave == null) //Addnew
                    {
                        model.LastEditDate = DateTime.Now;
                        model.CreateDate = DateTime.Now;
                        model.LastEditBy = pUserId;
                        model.CreateBy = pUserId;
                        model.StatusType_ID = 2;
                        HanomaContext.Recruitment.Add(model);
                        await HanomaContext.SaveChangesAsync();
                        //Create Urrl
                        CreateRecUrl(model.Recruitment_ID);
                    }
                    else // Update
                    {
                        recSave.RecruitmentCategory_ID = model.RecruitmentCategory_ID;
                        recSave.Name = model.Name;
                        recSave.Description = model.Description;
                        recSave.Content = model.Content;
                        recSave.PriceFrom = model.PriceFrom;
                        recSave.PriceTo = model.PriceTo;
                        recSave.LastEditDate = DateTime.Now;
                        recSave.LastEditBy = pUserId;
                        recSave.CompanyName = model.CompanyName;
                        recSave.ContactName = model.ContactName;
                        recSave.ContactPhone = model.ContactPhone;
                        recSave.ContactAddress = model.ContactAddress;
                        recSave.Location_ID = model.Location_ID;
                        recSave.ContactEmail = model.ContactEmail;
                        recSave.StatusType_ID = 2;
                        recSave.CompanyBusiness = model.CompanyBusiness;
                        recSave.RecruimentNumber = model.RecruimentNumber;
                        recSave.RequireExp = model.RequireExp;
                        recSave.JobType = model.JobType;
                        recSave.SalaryType = model.SalaryType;
                        recSave.AddressOfCV = model.AddressOfCV;
                        recSave.ContactPersonOfCV = model.ContactPersonOfCV;
                        recSave.PhonePersonOfCV = model.PhonePersonOfCV;
                        recSave.EmailPersonOfCV = model.EmailPersonOfCV;
                        recSave.CompanyWebsite = model.CompanyWebsite;
                        recSave.ProductCateRelate = model.ProductCateRelate;
                        recSave.ProductCateChildRelate = model.ProductCateChildRelate;
                        recSave.DeadlineOfCV = model.DeadlineOfCV;
                        recSave.RequireOfCV = model.RequireOfCV;
                        HanomaContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return model.Recruitment_ID;
        }

        public async Task CreateRecUrl(int Id)
        {
            try
            {
                var currentRec = await HanomaContext.Recruitment.FirstOrDefaultAsync(p => p.Recruitment_ID == Id);
                if(currentRec !=null)
                {
                    currentRec.URL = "tuyen-dung-" + FormatURL(currentRec.Name) + "-" +
                                        currentRec.Recruitment_ID.ToString();
                    await HanomaContext.SaveChangesAsync();
                }                    
            }
            catch (Exception ex)
            {

            }
        }

        public bool CanEditRec(int Id, string userId)
        {
            if (String.IsNullOrEmpty(userId)) return false;
            var product = HanomaContext.Recruitment.FirstOrDefault(x => x.Recruitment_ID == Id);

            if (product.CreateBy == new Guid(userId) || userId == "06dfe050-4407-4492-b1fd-656ccb8a82b6" || userId == "a4243721-e234-4efc-aec7-048117444572")
            {
                return true;
            }

            return false;
        }

        public async Task ActionInRec(int Id, string Action)
        {
            var result = await HanomaContext.Recruitment.FirstOrDefaultAsync(p => p.Recruitment_ID == Id);
            if (result != null)
            {
                result.StatusType_ID = -1; // hủy đăng
                await HanomaContext.SaveChangesAsync();
            }
        }

        public async Task DeleteRecIllustrationImages(List<DeleteImageRecPicture> model)
        {       
            foreach (var p in model)
            {
                var pictureItem = await HanomaContext.RecruitmentPicture.FirstOrDefaultAsync(x => x.RecruitmentPicture_ID.ToString() == p.RecruitmentPicture_ID);
                if(pictureItem != null)
                {
                    HanomaContext.RecruitmentPicture.Remove(pictureItem);
                    await HanomaContext.SaveChangesAsync();
                }   
            }
        }

        public async Task<List<DeleteImageRecPicture>> GetListDeleteRecPicture(int Id)
        {
            var output = new List<DeleteImageRecPicture>();
            var prodPicture = await HanomaContext.RecruitmentPicture.Where(p => p.Recruitment_ID == Id).ToListAsync();
            if (prodPicture.Count > 0)
            {
                foreach (var p in prodPicture)
                {
                    DeleteImageRecPicture item = new DeleteImageRecPicture();
                    item.RecruitmentPicture_ID = p.RecruitmentPicture_ID.ToString();
                    output.Add(item);
                }
            }
            return output;
        }
    }
}
