using HNM.RepositoriesWeb.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HNM.RepositoriesWeb.RepositoriesBase
{
    public interface IRepositoryWrapper
    {
        IAdvertisingRepository Advertising { get; }
        IArticleRepository Article { get; }
        IVideoRepository Video { get; }
        ILibraryRepository Library { get; }
        IProductCategoryRepository ProductCategory { get; }
        IProductRepository Product { get; }
        IServiceRepository Service { get; }
        ISettingRepository Setting { get; }
        ICurriculumVitaeRepository CurriculumVitae { get; }
        IRecruitmentRepository Recruitment { get; }
        IElasticRepository Elastic { get; }
        IProfileRepository Profile { get; }
        ISubscriptionRepository Subscription { get; }
        IManufactureRepository Manufacture { get; }
        IProductModelRepository ProductModel { get; }
        ILocationRepository Location { get; }
        IAccountRepository Account { get; }
        IProductBrandRepository ProductBrand { get; }
        IManagerDemandRepository ManagerDemand { get; }
        IShopmanProductRepository ShopmanProduct { get; }
        IUpdateStoreRepository UpdateStore { get; }

    }
}
