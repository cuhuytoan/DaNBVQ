using AutoMapper;
using HNM.RepositoriesWeb.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace HNM.RepositoriesWeb.RepositoriesBase
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private static HttpClient _client;        
        private static IMapper _mapper;
        private static IHttpContextAccessor _httpContext;
        private IAdvertisingRepository _advertising;
        private IArticleRepository _article;
        private IVideoRepository _video;
        private ILibraryRepository _library;
        private IProductCategoryRepository _productCategory;
        private IProductRepository _product;
        private IServiceRepository _service;
        private ISettingRepository _setting;
        private ICurriculumVitaeRepository _curriculumVitae;
        private IRecruitmentRepository _recruitment;
        private IElasticRepository _elasticRepository;
        private IProfileRepository _profile;
        private ISubscriptionRepository _subscription;
        private IManufactureRepository _manufacture;
        private IProductModelRepository _productModel;
        private ILocationRepository _location;
        private IAccountRepository _account;
        private IProductBrandRepository _productBrand;
        private IManagerDemandRepository _managerDemand;
        private IShopmanProductRepository _shopmanProduct;
        private IUpdateStoreRepository _updateStore;


        public RepositoryWrapper(HttpClient client, IMapper mapper, IHttpContextAccessor httpContext)
        {
            _client = client;
            _httpContext = httpContext;
            _mapper = mapper;
        }

        public IAccountRepository Account
        {
            get
            {
                if (_account == null)
                {
                    _account = new AccountRepository(_client, _mapper);
                }
                return _account;
            }

        }

        public IAdvertisingRepository Advertising
        {
            get
            {
                if(_advertising == null)
                {
                    _advertising = new AdvertisingRepository(_client, _mapper);
                }
                return _advertising;
            }
            
        }

        public IArticleRepository Article
        {
            get
            {
                if (_article == null)
                {
                    _article = new ArticleRepository(_client, _mapper);
                }
                return _article;
            }
        }

        public IVideoRepository Video
        {
            get
            {
                if(_video == null)
                {
                    _video = new VideoRepository(_client, _mapper);
                }
                return _video;
            }
        }

        public ILibraryRepository Library
        {
            get
            {
                if (_library == null)
                {
                    _library = new LibraryRepository(_client, _mapper, _httpContext);
                }
                return _library;
            }
        }

        public IProductCategoryRepository ProductCategory
        {
            get
            {
                if (_productCategory == null)
                {
                    _productCategory = new ProductCategoryRepository(_client, _mapper);
                }
                return _productCategory;
            }
        }

        public IProductRepository Product
        {
            get
            {
                if (_product == null)
                {
                    _product = new ProductRepository(_client, _mapper, _httpContext);
                }
                return _product;
            }
        } 
        
        public IServiceRepository Service
        {
            get
            {
                if (_service == null)
                {
                    _service = new ServiceRepository(_client, _mapper, _httpContext);
                }
                return _service;
            }
        }

        public ISettingRepository Setting
        {
            get
            {
                if (_setting == null)
                {
                    _setting = new SettingRepository(_client, _mapper);
                }
                return _setting;
            }
        } 
        
        public ICurriculumVitaeRepository CurriculumVitae
        {
            get
            {
                if (_curriculumVitae == null)
                {
                    _curriculumVitae = new CurriculumVitaeRepository(_client, _mapper);
                }
                return _curriculumVitae;
            }
        }
        
        public IRecruitmentRepository Recruitment
        {
            get
            {
                if (_recruitment == null)
                {
                    _recruitment = new RecruitmentRepository(_client, _mapper);
                }
                return _recruitment;
            }
        }
        public IElasticRepository Elastic
        {
            get
            {
                if (_elasticRepository == null)
                {
                    _elasticRepository = new ElasticRepository(_client, _mapper);
                }

                return _elasticRepository;
            }
        }
         public IProfileRepository Profile
        {
            get
            {
                if(_profile == null)
                {
                    _profile = new ProfileRepository(_client, _mapper, _httpContext);
                }
                return _profile;
            }
        }
        public ISubscriptionRepository Subscription
        {
            get
            {
                if (_subscription == null)
                {
                    _subscription = new SubscriptionRepository(_client, _mapper);
                }
                return _subscription;
            }
        }
        public IManufactureRepository Manufacture
        {
            get
            {
                if (_manufacture == null)
                {
                    _manufacture = new ManufactureRepository(_client, _mapper);
                }
                return _manufacture;
            }
        }
        public IProductModelRepository ProductModel
        {
            get
            {
                if (_productModel == null)
                {
                    _productModel = new ProductModelRepository(_client, _mapper);
                }
                return _productModel;
            }
        }
        public ILocationRepository Location
        {
            get
            {
                if (_location == null)
                {
                    _location = new LocationRepository(_client, _mapper);
                }
                return _location;
            }
        }

        public IProductBrandRepository ProductBrand
        {
            get
            {
                if (_productBrand == null)
                {
                    _productBrand = new ProductBrandRepository(_client, _mapper,_httpContext);
                }
                return _productBrand;
            }
        }

        public IManagerDemandRepository ManagerDemand
        {
            get
            {
                if (_managerDemand == null)
                {
                    _managerDemand = new ManagerDemandRepository(_client, _mapper, _httpContext);
                }
                return _managerDemand;
            }
        }
        
        public IShopmanProductRepository ShopmanProduct
        {
            get
            {
                if (_shopmanProduct == null)
                {
                    _shopmanProduct = new ShopmanProductRepository(_client, _mapper, _httpContext);
                }
                return _shopmanProduct;
            }
        }

        public IUpdateStoreRepository UpdateStore
        {
            get
            {
                if (_updateStore == null)
                {
                    _updateStore = new UpdateStoreRepository(_client, _mapper, _httpContext);
                }
                return _updateStore;
            }
        }
    }
}
