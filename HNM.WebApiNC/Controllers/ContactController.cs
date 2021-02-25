using AutoMapper;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.RepositoriesNC.RepositoriesBase;
using HNM.WebApiNC.Logging;
using HNM.WebApiNC.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HNM.WebApiNC.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    // [Authorize]
    [ApiController]
    public class ContactController : ControllerBase, IDisposable
    {
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;
        public ContactController(IRepositoryWrapper repoWrapper, ILoggerManager logger, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<object> ContactWithSeller([FromBody] ContactSellerDTO model)
        {
            var responseModel = new ContactSellerResultDTO();
            //var checkEmail = Util.IsEmail(model.Email);
            //if (!checkEmail)
            //{
            //    responseModel.ErrorCode = "01";
            //    responseModel.Message = "Email không hợp lệ";
            //    return responseModel;
            //}
            try
            {

                var product = _repoWrapper.Product.FirstOrDefault(x => x.Product_ID == model.ProductId);
                var productBrand = _repoWrapper.Brand.FirstOrDefault(x => x.ProductBrand_ID == product.ProductBrand_ID);
                var productUrl = String.Format("{0}/{1}", "https://hanoma.vn", product.FullURL);
                var productbrandName = productBrand?.Name != null ? productBrand.Name : product.SaleContactName;
                IDictionary<string, string> map = new Dictionary<string, string>()
                    {
                       {"#name#",productbrandName},
                       {"#product_url#",productUrl},
                       {"#product_name#",product.Name},
                       {"#customer_name#",model.Name},
                       {"#customer_phone#", model.Phone},
                       {"#customer_email#", model.Email ?? ""},
                       {"#customer_content#",model.Infomation},
                       {"#email_cc#","ducpq@hanoma.vn"}
                    };
                var file = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "EmailTemplates", "contact-to-sale.html");
                string MailBody = System.IO.File.ReadAllText(file);
                var regex = new Regex(String.Join("|", map.Keys));
                var MailBodyRes = regex.Replace(MailBody, m => map[m.Value]);
                try
                {
                    Util.SendMail(model.Name, product.SaleEmail, product.SaleName, "Có khách hàng quan tâm đến sản phẩm của bạn", MailBodyRes, _repoWrapper.AspNetUsers.setting());
                }
                catch (Exception ex)
                {
                    responseModel.ErrorCode = "01";
                    responseModel.Message = "Gửi email không thành công.";
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"ContactWithSeller: " + ex.ToString());
            }
            responseModel.ErrorCode = "00";
            responseModel.Message = "Gửi email thành công.";
            return responseModel;

        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}