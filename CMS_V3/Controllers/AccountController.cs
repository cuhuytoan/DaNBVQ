using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using CMS_V3.ViewModel;
using HNM.DataNC.Models;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.RepositoriesWeb.RepositoriesBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using HNM.CommonNC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json.Linq;
using HNM.DataNC.ModelsNC.ModelsUtility;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace CMS_V3.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;
        private IDistributedCache _cache;        
        public AccountController(ILogger<AccountController> logger, IHttpClientFactory clientFactory, IMapper mapper, IRepositoryWrapper repoWrapper, IDistributedCache cache)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _mapper = mapper;
            _repoWrapper = repoWrapper;
            _cache = cache;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login(string returnUrl = "/")
        {            
            return PartialView("Login",new LoginDto());
        }

        public IActionResult RequireLogin(string returnUrl = "/")
        {
            ViewData["showLogin"] = "1";
            ViewData["returnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]        
        public async Task<JsonResult> Login([FromBody]LoginDto model,string returnUrl = "/")
        {
            var output = new SumProfileResponseDTO();
            try
            {                    
                output = await _repoWrapper.Account.Login(model);                    
                if (output.ErrorCode == "00")
                {                        
                    Response.Cookies.Append("JWT", JsonConvert.SerializeObject(output));
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, output.Profile.UserName),
                    new Claim("UserInfomation",JsonConvert.SerializeObject(output)),
                    new Claim("access_token", output.JWT)
                };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        //AllowRefresh = <bool>,
                        // Refreshing the authentication session should be allowed.

                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(10),
                        // The time at which the authentication ticket expires. A 
                        // value set here overrides the ExpireTimeSpan option of 
                        // CookieAuthenticationOptions set with AddCookie.

                        IsPersistent = true,
                        // Whether the authentication session is persisted across 
                        // multiple requests. When used with cookies, controls
                        // whether the cookie's lifetime is absolute (matching the
                        // lifetime of the authentication ticket) or session-based.

                        //IssuedUtc = <DateTimeOffset>,
                        // The time at which the authentication ticket was issued.

                        //RedirectUri = <string>
                        // The full path or absolute URI to be used as an http 
                        // redirect response value.
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);
                }
                else
                {
                    _logger.LogInformation($"Account Controller - Login Fail {JsonConvert.SerializeObject(model)}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountController - Login {ex.ToString()}");
            }
            return Json(JsonConvert.SerializeObject(output));
        }

        
        public IActionResult Register()
        {
            return PartialView("Register");
        }
        public IActionResult ForgotPassword()
        {
            return PartialView("Forgot");
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromBody]ForgotPassworDTO model)
        {
            var output = new ForgotPassworDTO();
            try
            {
                output = await _repoWrapper.Account.ForgotPassword(model);
                if (output.ErrorCode == "00")
                {
                    _logger.LogInformation($"Account Controller - ForgotPassword {JsonConvert.SerializeObject(model)}");
                    return Json(JsonConvert.SerializeObject(output));
                }
                else
                {
                    _logger.LogInformation($"Account Controller - ForgotPassword {JsonConvert.SerializeObject(model)}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountController - ForgotPassword {ex.ToString()}");
            }
            //output.ErrorCode = "00";
            return Json(JsonConvert.SerializeObject(output));
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var output = new RegisterDto();
            try
            {
                output = await _repoWrapper.Account.Register(model);
                if (output.ErrorCode == "00")
                {
                    _logger.LogInformation($"Account Controller - Đã gửi code xác nhận {model.Email}");
                    return Json(JsonConvert.SerializeObject(output));
                }
                else
                {
                    _logger.LogInformation($"Account Controller - Register Fail {JsonConvert.SerializeObject(model)}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountController - Register {ex.ToString()}");
            }
            //output.ErrorCode = "00";
            return Json(JsonConvert.SerializeObject(output));

        }

        [HttpGet]
        public async Task<IActionResult> VerifyOTP(string EmailOrPhone, string code)
        {
            var output = new ConfirmVerify();
            try
            {
                output = await _repoWrapper.Account.VerifyCode(EmailOrPhone, code);
                if (output.ErrorCode == "00")
                {
                    _logger.LogInformation($"Account Controller - verify thành công {EmailOrPhone} - code {code}");
                    return Json(JsonConvert.SerializeObject(output));
                }
                else
                {
                    _logger.LogInformation($"Account Controller -  verify không thành công {EmailOrPhone} - code {code}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountController - VerifyOTP {ex.ToString()}");
            }
            //output.ErrorCode = "00";
            return Json(JsonConvert.SerializeObject(output));
        }

        [HttpPost]
        public async Task<IActionResult> ResendCode([FromBody] ForgotPassworDTO model)
        {
            var output = new ModelBase();
            try
            {
                output = await _repoWrapper.Account.ResendCode(model);
                if (output.ErrorCode == "00")
                {
                    _logger.LogInformation($"Account Controller - ResendCode thành công {JsonConvert.SerializeObject(model)}");
                    return Json(JsonConvert.SerializeObject(output));
                }
                else
                {
                    _logger.LogInformation($"Account Controller -  ResendCode không thành công {JsonConvert.SerializeObject(model)}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountController - ResendCode {ex.ToString()}");
            }
            // output.ErrorCode = "00";
            return Json(JsonConvert.SerializeObject(output));
        }

        [HttpPost]
        public async Task<IActionResult> SetPassword([FromBody] SetPassDTO model)
        {
            var output = new SetPassDTO();
            try
            {
                output = await _repoWrapper.Account.SetPassword(model);
                if (output.ErrorCode == "00")
                {
                    _logger.LogInformation($"Account Controller - SetPassword thành công {JsonConvert.SerializeObject(model)}");
                    return Json(JsonConvert.SerializeObject(output));
                }
                else
                {
                    _logger.LogInformation($"Account Controller -  SetPassword không thành công {JsonConvert.SerializeObject(model)}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountController - SetPassword {ex.ToString()}");
            }
            //output.ErrorCode = "00";
            return Json(JsonConvert.SerializeObject(output));

        }

        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("JWT");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
      
        
    }
}
