using Elasticsearch.Net;
using HNM.DataNC.ElasticModels;
using HNM.RepositoriesNC.Elastic;
using Nest;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HNM.RepositoriesNC.Repositories
{
    public interface IElasticRepository
    {
        Task<ProductsList> GetTopXProducts(int from = 0, int size = 6);
        Task<ProductsList> SearchProducts(int from = 0, int size = 20, string query = "");
        Task<ProductsList> SearchProductBrand(int page = 0, int pageSize = 20, string query = "");
        Task<BrandListElastic> SearchProductBrandPagination(int page = 0, int pageSize = 20, string query = "");
    }
    public class ElasticRepository : ElasticConnection, IElasticRepository
    {
        public static string PRODUCT_INDEX = "products";
        public static string BRAND_INDEX = "brands";
        public ElasticRepository()
        {
        }
        public async Task<ProductsList> GetTopXProducts(int from = 0, int size = 6)
        {
            var productsList = new ProductsList();
            try
            {
                var searchResponse = _elaticClient.Search<ProductElastic>(s => s
               .Index(PRODUCT_INDEX)
               .From(from)
               .Size(size)
               .Query(q => q.MatchAll()
                   )
                );
                var searchResponseWithIds = searchResponse.Hits.Select(h =>
                {
                    h.Source.Id = h.Id;
                    return h.Source;
                }).ToList();
                productsList.Products = searchResponse.Documents.ToList();
                return productsList;
            }
            catch (Exception ex)
            {

            }
            return productsList;

        }
        public async Task<ProductsList> SearchProducts(int page = 0, int pageSize = 20, string query = "")
        {
            //var exep = 1 / (page - page);
            var productsList = new ProductsList();

            var searchResponse = _elaticClient.Search<ProductElastic>(s => s
            .Index(PRODUCT_INDEX)
            .From((page - 1) * pageSize)
            .Size(pageSize)
              .Query(q => q
                     .Bool(b => b
                          .Must(m => m
                               .Match(mu => mu
                                 .Field("statusType_ID").Query("4")
                                 )
                               )
                          .Must(sh => sh.MultiMatch(c => c
                                         .Fields(f => f.Field("name^10").Field("metaKeywords").Field("saleName").Field("salePhone").Field("content").Field("partNumber"))
                                         .Operator(Operator.And)
                                         .Query(query)
                                         )
                                 )
                          .Filter(f => f
                                       .Bool(bf => bf
                                        .Must(mn1 => mn1
                                         .Term(t1 => t1
                                          .Field("statusType_ID").Value(4)
                                         )
                                        )
                                       )
                                      )
                          )


             )
         );
            var json = _elaticClient.RequestResponseSerializer.SerializeToString(searchResponse.ApiCall.RequestBodyInBytes);
            //var countTotal = _elaticClient.Count<ProductElastic>(s => s
            //.Index(PRODUCT_INDEX)
            // .Query(q => q
            //            .Bool(b => b
            //                 .Must(m => m
            //                      .Match(mu => mu
            //                        .Field("statusType_ID").Query("4")
            //                        )
            //                      )
            //                 .Must(sh => sh.MultiMatch(c => c
            //                                .Fields(f => f.Field("name^10").Field("metaKeywords").Field("saleName").Field("salePhone").Field("content"))
            //                                .Operator(Operator.And)
            //                                .Query(query)
            //                                )
            //                        )
            //                 )
            //    )
            //);
            var searchResponseWithIds = searchResponse.Hits.Select(h =>
            {
                h.Source.Id = h.Id;
                return h.Source;
            }).ToList();
            var products = searchResponse.Documents.ToList();

            var totalRecord = searchResponse.Total;
            var totalPage = (totalRecord - 1) / pageSize + 1;
            productsList.Products = products;
            productsList.TotalRecord = totalRecord;
            productsList.TotalPage = totalPage;
            productsList.CurrentPage = page;

            return productsList;

        }
        public async Task<ProductsList> SearchProductBrand(int page = 0, int pageSize = 20, string query = "")
        {
            var from = (page - 1) * pageSize;
            var productsList = new ProductsList();
            try
            {
                var searchResponse = _elaticClient.Search<ProductElastic>(s => s
                                            .Index(PRODUCT_INDEX)
                                            .Size(0)
                                            .Sort(sr => sr.Field(srf => srf.Field("_score")))
                                            .Query(q => q
                                            .Bool(b => b
                                             .Must(m => m
                                              .Match(mu => mu
                                               .Field("statusType_ID").Query("4")
                                              )
                                             )
                                             .Must(sh => sh
                                              .MultiMatch(c => c
                                               .Fields(f => f
                                                .Field("name^10").Field("metaKeywords").Field("saleName").Field("salePhone").Field("content").Field("productBrandName^15"))
                                               .Operator(Operator.And)
                                               .Query(query)
                                              )
                                             )
                                             .Filter(f => f
                                              .Bool(bf => bf
                                               .MustNot(mn1 => mn1
                                                .Term(t1 => t1
                                                 .Field("productType_ID").Value(2)
                                                )
                                               )
                                               .MustNot(mn2 => mn2
                                                .Term(t2 => t2
                                                 .Field("productBrand_ID").Value(0)
                                                )
                                               )
                                              )
                                             )
                                            )

                                            )
                                            .Aggregations(a => a
                                                .Terms("product_brand_group", term => term
                                                .Field("productBrand_ID")
                                                .Size(10000)
                                                .Aggregations(a2 => a2
                                                .TopHits("tophits", tophit => tophit
                                                .Source().Size(1)
                                                )
                                                .BucketSort("bucket_truncate", bs => bs.From(from).Size(pageSize))
                                                )

                                                )


                                            )
                                              );
                var searchResponseWithIds = searchResponse.Hits.Select(h =>
                {
                    h.Source.Id = h.Id;
                    return h.Source;
                }).ToList();

                var products = searchResponse.Documents.ToList();
                var tess = searchResponse.Aggregations.Terms("product_brand_group").Buckets;
                foreach (var bucket in searchResponse.Aggregations.Terms("product_brand_group").Buckets)
                {
                    var hit = bucket.TopHits("tophits");
                    var d = hit.Documents<ProductElastic>().FirstOrDefault();
                    productsList.Products.Add(d);
                }

                var totalRecord = searchResponse.Total;
                var totalPage = (totalRecord - 1) / pageSize + 1;
                productsList.Products = products;
                productsList.TotalRecord = totalRecord;
                productsList.TotalPage = totalPage;
                productsList.CurrentPage = page;

                return productsList;
            }
            catch (Exception ex)
            {

            }
            return productsList;

        }
        public async Task<BrandListElastic> SearchProductBrandPagination(int page = 0, int pageSize = 20, string query = "")
        {
            //var exep = 1 / (page - page);
            var from = (page - 1) * pageSize;
            var brandListElastic = new BrandListElastic();
            var searchResponse = _elaticClient.Search<BrandElastic>(s => s
                 .Index(BRAND_INDEX)
                 .From((page - 1) * pageSize)
                 .Size(pageSize)
                  .Query(q => q
                             .Bool(b => b
                                  .Must(sh => sh.MultiMatch(c => c
                                                 .Fields(f => f.Field("name^3").Field("mobile").Field("email"))
                                                 .Operator(Operator.And)
                                                 .Query(query)
                                                 )
                                         )
                                  )
                     )
                 );
            var searchResponseWithIds = searchResponse.Hits.Select(h =>
            {
                h.Source.Id = h.Id;
                return h.Source;
            }).ToList();

            var brands = searchResponse.Documents.ToList();


            var totalRecord = searchResponse.Total;
            var totalPage = (totalRecord - 1) / pageSize + 1;
            brandListElastic.Brands = brands;
            brandListElastic.TotalRecord = totalRecord;
            brandListElastic.TotalPage = totalPage;
            brandListElastic.CurrentPage = page;
            return brandListElastic;


        }
    }
}
