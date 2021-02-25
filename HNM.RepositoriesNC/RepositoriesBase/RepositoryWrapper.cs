using HNM.DataNC.Models;
using HNM.RepositoriesNC.Repositories;
using System.Threading.Tasks;

namespace HNM.RepositoriesNC.RepositoriesBase
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private HanomaContext _hanomaContext;
        private IFCMClientRepository _fCMClient;
        private IAdvertisingRepository _advertising;
        private IAccountRepository _accountRepository;
        private IAspNetUserProfilesRepository _aspNetUserProfilesRepository;
        private IFCMMessageForTokkenRepository _fCMMessageForTokkenRepository;
        private IFCMMessageRepository _fCMMessageRepository;
        private IProductRepository _productRepository;
        private IProductPictureRepository _productPictureRepository;
        private IProductCategoryRepository _productCategoryRepository;
        private IProductModelRepository _productModelRepository;
        private IProductManufactureRepository _productManufactureRepository;
        private IMenuRepository _menuRepository;
        private IKeywordRepository _keywordRepository;       
        private IBrandRepository _brandRepository;
        private IArticleCategoryRepository _articleCategoryRepository;
        private IElasticRepository _elasticRepository;
        private IArticleRepository _articleRepository;
        private ILocationRepository _locationRepository;
        private IManufactureRepository _manufactureRepository;        
        private IPageLayoutRepository _pageLayoutRepository;        
        private ILibraryRepository _libraryRepository;
        private ILibraryCategoryRepository _libraryCategoryRepository;        
        private IDashBoardRepository _dashBoardRepository;       
        private ICountryRepository _countryRepository;
        private IUnitRepository _unitRepository;
        private ISettingRepository _settingRepository;
        private ISubscriptionRepository _subscription;
        private IShoppingCartRepository _shoppingCart;

        public IShoppingCartRepository ShoppingCart
        {
            get
            {
                if (_shoppingCart == null)
                {
                    _shoppingCart = new ShoppingCartRepository(_hanomaContext);
                }

                return _shoppingCart;
            }
        }

        public ISettingRepository Setting
        {
            get
            {
                if (_settingRepository == null)
                {
                    _settingRepository = new SettingRepository(_hanomaContext);
                }

                return _settingRepository;
            }
        }
        public IFCMClientRepository FCMClient
        {
            get
            {
                if (_fCMClient == null)
                {
                    _fCMClient = new FCMClientRepository(_hanomaContext);
                }

                return _fCMClient;
            }
        }
        public IAdvertisingRepository Advertising
        {
            get
            {
                if (_advertising == null)
                {
                    _advertising = new AdvertisingRepository(_hanomaContext);
                }

                return _advertising;
            }
        }
        public IAccountRepository AspNetUsers
        {
            get
            {
                if (_accountRepository == null)
                {
                    _accountRepository = new AccountRepository(_hanomaContext);
                }

                return _accountRepository;
            }
        }

        public IAspNetUserProfilesRepository AspNetUserProfiles
        {
            get
            {
                if (_aspNetUserProfilesRepository == null)
                {
                    _aspNetUserProfilesRepository = new AspNetUserProfilesRepository(_hanomaContext);
                }

                return _aspNetUserProfilesRepository;
            }
        }

        public IFCMMessageForTokkenRepository FCMMessageForTokken
        {
            get
            {
                if (_fCMMessageForTokkenRepository == null)
                {
                    _fCMMessageForTokkenRepository = new FCMMessageForTokkenRepository(_hanomaContext);
                }

                return _fCMMessageForTokkenRepository;
            }
        }
        public IFCMMessageRepository FCMMessage
        {
            get
            {
                if (_fCMMessageRepository == null)
                {
                    _fCMMessageRepository = new FCMMessageRepository(_hanomaContext);
                }

                return _fCMMessageRepository;
            }
        }

        public IProductRepository Product
        {
            get
            {
                if (_productRepository == null)
                {
                    _productRepository = new ProductRepository(_hanomaContext);
                }

                return _productRepository;
            }
        }
        public IProductPictureRepository ProductPicture
        {
            get
            {
                if (_productPictureRepository == null)
                {
                    _productPictureRepository = new ProductPictureRepository(_hanomaContext);
                }

                return _productPictureRepository;
            }
        }
        public IProductCategoryRepository ProductCategory
        {
            get
            {
                if (_productCategoryRepository == null)
                {
                    _productCategoryRepository = new ProductCategoryRepository(_hanomaContext);
                }

                return _productCategoryRepository;
            }
        }
        public IMenuRepository Menu
        {
            get
            {
                if (_menuRepository == null)
                {
                    _menuRepository = new MenuRepository(_hanomaContext);
                }

                return _menuRepository;
            }
        }
        public IKeywordRepository Keyword
        {
            get
            {
                if (_keywordRepository == null)
                {
                    _keywordRepository = new KeywordRepository(_hanomaContext);
                }

                return _keywordRepository;
            }
        }
        
     
        public IBrandRepository Brand
        {
            get
            {
                if (_brandRepository == null)
                {
                    _brandRepository = new BrandRepository(_hanomaContext);
                }

                return _brandRepository;
            }
        }
        public IProductModelRepository ProductModel
        {
            get
            {
                if (_productModelRepository == null)
                {
                    _productModelRepository = new ProductModelRepository(_hanomaContext);
                }

                return _productModelRepository;
            }
        }
        public IProductManufactureRepository ProductManufacture
        {
            get
            {
                if (_productManufactureRepository == null)
                {
                    _productManufactureRepository = new ProductManufactureRepository(_hanomaContext);
                }

                return _productManufactureRepository;
            }
        }

        public IArticleCategoryRepository ArticleCategory
        {
            get
            {
                if (_articleCategoryRepository == null)
                {
                    _articleCategoryRepository = new ArticleCategoryRepository(_hanomaContext);
                }

                return _articleCategoryRepository;

            }
        }

        public IArticleRepository Article
        {
            get
            {
                if (_articleRepository == null)
                {
                    _articleRepository = new ArticleRepository(_hanomaContext);
                }

                return _articleRepository;

            }
        }
        public ILocationRepository Location
        {
            get
            {
                if (_locationRepository == null)
                {
                    _locationRepository = new LocationRepository(_hanomaContext);
                }

                return _locationRepository;

            }
        }
        public IManufactureRepository Manufacture
        {
            get
            {
                if (_manufactureRepository == null)
                {
                    _manufactureRepository = new ManufactureRepository(_hanomaContext);
                }

                return _manufactureRepository;

            }
        }
     
        public IPageLayoutRepository PageLayout
        {
            get
            {
                if (_pageLayoutRepository == null)
                {
                    _pageLayoutRepository = new PageLayoutRepository(_hanomaContext);
                }

                return _pageLayoutRepository;

            }
        }
    
        public IElasticRepository Elastic
        {
            get
            {
                if (_elasticRepository == null)
                {
                    _elasticRepository = new ElasticRepository();
                }

                return _elasticRepository;
            }
        }

        public ILibraryRepository Library
        {
            get
            {
                if (_libraryRepository == null)
                {
                    _libraryRepository = new LibraryRepository(_hanomaContext);
                }
                return _libraryRepository;
            }
        }
        public ILibraryCategoryRepository LibraryCategory
        {
            get
            {
                if (_libraryCategoryRepository == null)
                {
                    _libraryCategoryRepository = new LibraryCategoryRepository(_hanomaContext);
                }
                return _libraryCategoryRepository;
            }
        }
       
        public IDashBoardRepository DashBoard
        {
            get
            {
                if (_dashBoardRepository == null)
                {
                    _dashBoardRepository = new DashBoardRepository(_hanomaContext);
                }
                return _dashBoardRepository;
            }
        }
      

        public ICountryRepository Country
        {
            get
            {
                if (_countryRepository == null)
                {
                    _countryRepository = new CountryRepository(_hanomaContext);
                }
                return _countryRepository;
            }
        }
        public IUnitRepository Unit
        {
            get
            {
                if (_unitRepository == null)
                {
                    _unitRepository = new UnitRepository(_hanomaContext);
                }
                return _unitRepository;
            }
        }
        public ISubscriptionRepository Subscription
        {
            get
            {
                if (_subscription == null)
                {
                    _subscription = new SubscriptionRepository(_hanomaContext);
                }
                return _subscription;
            }
        }
        public RepositoryWrapper(HanomaContext HanomaContext)
        {
            _hanomaContext = HanomaContext;
        }

        public void Save()
        {
            _hanomaContext.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _hanomaContext.SaveChangesAsync();
        }
    }
}
