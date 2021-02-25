using AutoMapper;
using HNM.DataNC.ElasticModels;
using HNM.DataNC.Models;
using HNM.DataNC.ModelsIdentity;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.DataNC.ModelsStore;
using HNM.DataNC.ModelsPayment;
using HNM.DataNC.ModelsFilter;

namespace HNM.WebApiNC.AutoMap
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
            DestinationMemberNamingConvention = new PascalCaseNamingConvention();
            ReplaceMemberName("_ID", "Id");

            //FCM Message
            CreateMap<FCMClient, FCMClientDTO>().ReverseMap();
            CreateMap<FCMMessage, FCMMessageDTO>().ReverseMap();

            //Advertising
            CreateMap<AdvertisingDTO, Advertising>().ReverseMap();
            CreateMap<AdvertisingCarouselDTO, Advertising>().ReverseMap()
                .ForMember(dest =>
                    dest.ParamId,
                    opt => opt.MapFrom(src => src.ParameterId));
            CreateMap<AdvertisingMobileDTO, Advertising>().ReverseMap();
            CreateMap<AdvertisingDTO, SpAdMostView_CHT_Result>().ReverseMap();
            CreateMap<AdvertisingCarouselDTO, SpAdMostView_CHT_Result>().ReverseMap();
            //Profile
            CreateMap<AspNetUserProfiles, ProfilersDTO>().ReverseMap();
            CreateMap<ApplicationUser, ProfileReponse>().ReverseMap();
            CreateMap<AspNetRoles, ListRole>().ReverseMap();
            CreateMap<AspNetUserProfiles, UpdateAvatarDTO>().ReverseMap();

            // Product Category
            CreateMap<ProductCategory, ProductCategoryDTO>().ReverseMap();
            CreateMap<GetMenuProdCateByUser_Result, ProductCategoryDTO>().ReverseMap();


            CreateMap<Menu, HomeMenuDTO>().ReverseMap();
            CreateMap<GetMenuStatusByUser_Result, HomeMenuDTO>().ReverseMap();


            // Recruitment
            
            CreateMap<RecruitmentTop, RecruitmentDTO>().ReverseMap();
            CreateMap<Recruitment_Search_Result, RecruitmentSearchResultDTO>().ReverseMap();
            CreateMap<RecruitmentSearchResultDTO, Recruitment_Search_V2_Result>().ReverseMap();
            

            // Resume
            CreateMap<Resume_Search_Result, ResumeDTO>().ReverseMap();
            CreateMap<Resume_Search_Result, ResumeSearchResultDTO>().ReverseMap();

            //Product Brand
            CreateMap<ProductBrand, BrandDTO>().ReverseMap();
            CreateMap<ProductBrand_Search_Result, ProductBrandSearchDTO>().ReverseMap();
            CreateMap<BrandElastic, ProductBrandSearchDTO>().ReverseMap();
            CreateMap<ProductBrandSearchDetailDTO, ProductBrand>().ReverseMap();
            CreateMap<PostProductBrand, ProductBrand>().ReverseMap();

            //Product
            CreateMap<Product_PreProcess_ByCateId_Result, ProductItemByCateIdDTO>().ReverseMap();
            CreateMap<Product_Search_Result, ProductSearchResultDTO>().ReverseMap();
            CreateMap<ProductElastic, ProductSearchResultDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, ProductMostViewDTO>().ReverseMap();
            CreateMap<ProductSearchResultDTO, Accesories_Search_Result>().ReverseMap();
            CreateMap<ProductSearchResultDTO, Materials_Search_Result>().ReverseMap();
            CreateMap<ProductSearchResultDTO, Service_Search_Result>().ReverseMap();
            CreateMap<Product, ProductShopManDTO>().ReverseMap();
            CreateMap<ProductShopManFilter, ProductFilter>().ReverseMap();
            CreateMap<ProductHighLight_Search_Result, ProductSearchResultDTO>().ReverseMap();
            CreateMap<TimeLinePostDTO, TimeLinePost_Result>().ReverseMap();
            //ProductPicture
            CreateMap<ProductPicture, ProductPictureDTO>().ReverseMap();
            //Article
            CreateMap<Article, ArticleDTO>().ReverseMap();
            CreateMap<ArticleCategoryDTO, ArticleCategory>().ReverseMap();
            CreateMap<ArticleVoiceDTO, ArticleVoice>().ReverseMap();
            CreateMap<ArticleShortInfo, GetArticleHome_Result>().ReverseMap();
            CreateMap<ArticleShortInfo, Article>().ReverseMap();
            CreateMap<ArticleShortInfo, Article_Search_Result>().ReverseMap();
            CreateMap<VideoShortInfo, Article_Search_Result>().ReverseMap();
            // Location
            CreateMap<LocationDTO, Location>().ReverseMap();
            CreateMap<DistrictDTO, District>().ReverseMap();

            //Manufacture
            CreateMap<ProductManufacture_SearchByCategory_Result_DTO, ProductManufacture_SearchByCategory_Result>().ReverseMap();
            CreateMap<ProductManufacture_SearchByCategory_Result_DTO, ProductManufacture>().ReverseMap();

            //Product Model
            CreateMap<ProductModel_SearchByCategory_Result_DTO, ProductModel_SearchByCategory_Result>().ReverseMap();
            CreateMap<ProductModel, ProductModelDTO>().ReverseMap();
            CreateMap<ProductModel_SearchByCategory_Result_DTO, ProductModel>().ReverseMap();
            //Recruitment Category
            

            // Page Layout
            CreateMap<ProductCategoryHighLightDTO, PageLayout>().ReverseMap();

            // keyword 
            CreateMap<KeywordDTO, MetaKeyword>().ReverseMap();

            // IntroScreen
            

            //Library
            CreateMap<LibraryDTO, Library>().ReverseMap();
            CreateMap<PostLibrary, Library>().ReverseMap();

            //Library Category
            CreateMap<LibraryCategoryDTO, LibraryCategory>().ReverseMap();

            //PartNumber 
            CreateMap<PatchNumberDTO, PatchNumber_SearchByCategory_Result>().ReverseMap();

            // CurriculumnVitae
            
            CreateMap<CVSearchDTO, CV_Search_Result>().ReverseMap();
            

            //DashBoard
            CreateMap<DashBoashProductBrandDTO, GetDashBoard_Result>().ReverseMap();
            CreateMap<DashBoardBrand, GetDashBoard_Result>().ReverseMap();
            CreateMap<DashBoardCustomer, GetDashBoard_Result>().ReverseMap();
            CreateMap<DashBoardPost, GetDashBoard_Result>().ReverseMap();
            CreateMap<DashBoardOrder, GetDashBoard_Result>().ReverseMap();
            CreateMap<DashBoardFinance, GetDashBoard_Result>().ReverseMap();
            //CareerCategory
            CreateMap<CareerCategoryDTO, CareerCategory>().ReverseMap();

            //Country
            CreateMap<CountryDTO, Country>().ReverseMap();

            //Unit
            CreateMap<UnitDTO, Unit>().ReverseMap();

            //Accessories
            

            //Payment
            CreateMap<ServicesDTO, HNM.DataNC.ModelsPayment.Services>().ReverseMap();
            CreateMap<DiscountConfigDTO, DiscountConfig>().ReverseMap();
            CreateMap<OrderDetailDTO, OrderDetail>().ReverseMap();
            CreateMap<PostPaymentVAT, PaymentExpVat>().ReverseMap();

            //Video Category
            CreateMap<VideoCategoryDTO, VideoCategory>().ReverseMap();
            //Meta Keyword
            CreateMap<MetaKeywordDTO, MetaKeyword>().ReverseMap();

            CreateMap<SubscriptionDTO, Subscription>().ReverseMap();
            
        }

    }
}

