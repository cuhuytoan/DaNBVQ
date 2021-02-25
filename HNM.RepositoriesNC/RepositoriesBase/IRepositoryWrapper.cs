using HNM.RepositoriesNC.Repositories;
using System.Threading.Tasks;

namespace HNM.RepositoriesNC.RepositoriesBase
{
    public interface IRepositoryWrapper
    {
        IFCMClientRepository FCMClient { get; }
        IAdvertisingRepository Advertising { get; }
        IAccountRepository AspNetUsers { get; }
        IAspNetUserProfilesRepository AspNetUserProfiles { get; }
        IFCMMessageForTokkenRepository FCMMessageForTokken { get; }
        IFCMMessageRepository FCMMessage { get; }
        IProductRepository Product { get; }
        IProductPictureRepository ProductPicture { get; }
        IProductModelRepository ProductModel { get; }
        IProductManufactureRepository ProductManufacture { get; }
        IProductCategoryRepository ProductCategory { get; }
        IMenuRepository Menu { get; }        
        IBrandRepository Brand { get; }
        IArticleCategoryRepository ArticleCategory { get; }
        IElasticRepository Elastic { get; }
        IArticleRepository Article { get; }
        ILocationRepository Location { get; }
        IManufactureRepository Manufacture { get; }
        
        IPageLayoutRepository PageLayout { get; }
        //IIntroRepository IntroScreen { get; }
        IKeywordRepository Keyword { get; }
        ILibraryRepository Library { get; }
        ILibraryCategoryRepository LibraryCategory { get; }
        //ICurriculumVitaeRepository CurriculumVitae { get; }
        IDashBoardRepository DashBoard { get; }
        
        ICountryRepository Country { get; }
        IUnitRepository Unit { get; }
        ISettingRepository Setting { get; }
        ISubscriptionRepository Subscription { get; }
        IShoppingCartRepository ShoppingCart { get; }
        void Save();
        Task<int> SaveChangesAsync();

    }
}
