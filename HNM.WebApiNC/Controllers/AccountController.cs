using AutoMapper;
using HNM.DataNC.Models;
using HNM.DataNC.ModelsIdentity;
using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.DataNC.ModelsNC.ModelsUtility;
using HNM.RepositoriesNC.RepositoriesBase;
using HNM.WebApiNC.Logging;
using HNM.WebApiNC.Services;
using HNM.WebApiNC.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HNM.WebApiNC.Controllers
{

    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase, IDisposable
    {
        private readonly ILoggerManager _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly HanomaContext _hanomaContext;
        private IMapper _mapper;
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IRepositoryWrapper repositoryWrapper,
            IEmailSender emailSender,
            ISmsSender smsSender,
            ILoggerManager logger,
            HanomaContext hanomaContext,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _repositoryWrapper = repositoryWrapper;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = logger;
            _hanomaContext = hanomaContext;
            _mapper = mapper;
        }

        /// <summary>
        /// ApiLogin
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="password">Mật khẩu</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> Login([FromBody] LoginDto model)
        {
            //var username = principal.Identity.Name; //this is mapped to the Name claim by default

            var responseModel = new SumProfileResponseDTO();
            responseModel.Profile = new ProfileReponse();
            responseModel.ListRole = new List<ListRole>();
            responseModel.Profile.Email = model.Email;
            var inputUserName = model.Email;
            var inputEmail = model.Email;
            if (Util.IsPhoneNumber(model.Email))
            {
                model.Email = $"{model.Email}@hanoma.vn";
            }
            //Check input Email Pass
            if (String.IsNullOrEmpty(model.Email) || String.IsNullOrEmpty(model.Password))
            {
                responseModel.ErrorCode = "ACC006";
                responseModel.Message = ConstMessage.GetMsgConst("ACC006");
                return responseModel;
            }
            else
            {
                //Check exists User
                var user = await _userManager.FindByNameAsync(inputUserName);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(inputUserName, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        //var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                        var aspNetUserLogin =
                            _repositoryWrapper.AspNetUserProfiles.FirstOrDefault(p => p.UserId == user.Id);
                        responseModel.Profile = _mapper.Map<ProfileReponse>(user);
                        responseModel.Profile.FullName = aspNetUserLogin.FullName;
                        responseModel.Profile.RegType = aspNetUserLogin.RegType;
                        responseModel.Profile.AvartarFileName = aspNetUserLogin.AvatarUrl ?? "noimage.png";
                        responseModel.Profile.AvartarFullUrl =
                           _configuration["Cloud_Path"] + $"/user/avatar/original/{aspNetUserLogin.AvatarUrl ?? "noimage.png"}";

                        //List Role
                        var lstRole = await _repositoryWrapper.AspNetUsers.GetListRole(user.Id);
                        responseModel.ListRole = _mapper.Map<List<ListRole>>(lstRole);
                        //ProductBrandId
                        var userProfiler = await _repositoryWrapper.AspNetUserProfiles.FirstOrDefaultAsync(p => p.UserId == user.Id);
                        if (userProfiler != null)
                        {
                            responseModel.ProductBrandId = userProfiler.ProductBrand_ID ?? 0;
                            if (responseModel.ProductBrandId != 0)
                            {
                                var brand = await _repositoryWrapper.Brand.FirstOrDefaultAsync(p => p.ProductBrand_ID == responseModel.ProductBrandId);
                                if (brand != null)
                                {
                                    responseModel.ProductBrandTypeId = brand.ProductBrandType_ID?? 1;
                                    responseModel.ProductBrandYearJoin = (int)(DateTime.Now.Year - brand.CreateDate?.Year);
                                    responseModel.ReferralCode = brand.ReferralCode;
                                    responseModel.ProductBrandName = brand.Name;
                                    responseModel.ProductBrandAvatarUrl = _configuration["Cloud_Path"] + $"/productbrand/logo/original/{brand.Logo}";
                                }
                            }
                        }
                        else
                        {
                            responseModel.ProductBrandId = 0;
                        }
                        // responseModel.Role = lstRole.Count > 0 ? string.Join(",", lstRole) : "";
                        var jwt = await GenerateJwtToken(inputUserName, user);
                        responseModel.JWT = jwt.ToString();
                        responseModel.UserId = user.Id;
                        responseModel.ErrorCode = "00";
                        responseModel.Message = "Đăng nhập thành công";
                        //Total UnRead
                        responseModel.NumberFCMUnread = await _repositoryWrapper.FCMMessage.GetNumberFCMUnread(user.Id);
                        return responseModel;
                    }
                    // After register must verify
                    if (result.IsNotAllowed)
                    {
                        responseModel.ErrorCode = "ACC013";
                        responseModel.Message = ConstMessage.GetMsgConst("ACC013");
                        return responseModel;
                    }
                    //Yêu cầu xác  thực mỗi lần đăng nhập
                    if (result.RequiresTwoFactor)
                    {
                        responseModel.ErrorCode = "ACC013";
                        responseModel.Message = ConstMessage.GetMsgConst("ACC013");
                        return responseModel;
                    }
                    if (result.IsLockedOut)
                    {
                        responseModel.ErrorCode = "ACC011";
                        responseModel.Message = ConstMessage.GetMsgConst("ACC011");
                        return responseModel;
                    }
                    else
                    {
                        _logger.LogError($"[AccountController] Mật khẩu không đúng!");
                        responseModel.ErrorCode = "ACC007";
                        responseModel.Message = ConstMessage.GetMsgConst("ACC007");
                        return responseModel;
                    }

                }

                else
                {
                    _logger.LogError($"[AccountController] Tài khoản {model.Email} không tồn tại");
                    responseModel.ErrorCode = "ACC008";
                    responseModel.Message = ConstMessage.GetMsgConst("ACC008");
                    return responseModel;
                }
            }


        }

        [HttpPost]
        public async Task<object> LoginByJWT([FromBody] LoginTokenDTO model)
        {
            var responseModel = new SumProfileResponseDTO();
            responseModel.Profile = new ProfileReponse();
            responseModel.ListRole = new List<ListRole>();
            responseModel.Profile.Email = model.Email;
            if (model.Jwt == null)
            {
                responseModel.ErrorCode = "001";
                responseModel.Message = "Token không hợp lệ";
            }
            else
            {
                CheckExpireToken response = await GetPrincipalFromExpiredToken(model.Jwt);
                if (response.ErrorCode == "00")
                {
                    //Check exists User
                    var user = await _userManager.FindByNameAsync(response.Email);
                    if (user != null)
                    {
                        var aspNetUserLogin =
                          _repositoryWrapper.AspNetUserProfiles.FirstOrDefault(p => p.UserId == user.Id);
                        responseModel.Profile = _mapper.Map<ProfileReponse>(user);
                        responseModel.Profile.FullName = aspNetUserLogin.FullName;
                        responseModel.Profile.RegType = aspNetUserLogin.RegType;
                        responseModel.Profile.AvartarFileName = aspNetUserLogin.AvatarUrl ?? "noimage.png";
                        responseModel.Profile.AvartarFullUrl =
                           _configuration["Cloud_Path"] + $"/user/avatar/original/{aspNetUserLogin.AvatarUrl ?? "noimage.png"}";

                        //List Role
                        var lstRole = await _repositoryWrapper.AspNetUsers.GetListRole(user.Id);
                        responseModel.ListRole = _mapper.Map<List<ListRole>>(lstRole);
                        //ProductBrandId
                        var userProfiler = await _repositoryWrapper.AspNetUserProfiles.FirstOrDefaultAsync(p => p.UserId == user.Id);
                        if (userProfiler != null)
                        {
                            responseModel.ProductBrandId = userProfiler.ProductBrand_ID ?? 0;
                            if(responseModel.ProductBrandId != 0)
                            {
                                var brand = await _repositoryWrapper.Brand.FirstOrDefaultAsync(p => p.ProductBrand_ID == responseModel.ProductBrandId);
                                if(brand != null)
                                {
                                    responseModel.ProductBrandTypeId = brand.ProductBrandType_ID ?? 1;
                                    responseModel.ProductBrandYearJoin = (int)(DateTime.Now.Year - brand.CreateDate?.Year);
                                    responseModel.ReferralCode = brand.ReferralCode;
                                }    
                            }    
                        }
                        else
                        {
                            responseModel.ProductBrandId = 0;
                        }
                        
                        // responseModel.Role = lstRole.Count > 0 ? string.Join(",", lstRole) : "";
                        //var jwt = await GenerateJwtToken(model.Email, user);
                        responseModel.JWT = model.Jwt;
                        responseModel.UserId = user.Id;
                        responseModel.ErrorCode = "00";
                        responseModel.Message = "Đăng nhập thành công";
                        //Total UnRead
                        responseModel.NumberFCMUnread = await _repositoryWrapper.FCMMessage.GetNumberFCMUnread(user.Id);
                        return responseModel;
                    }
                }
                else if(response.ErrorCode == "003")
                {
                    //Call refresh token
                    var user = await _userManager.FindByNameAsync(model.Email);
                    if (user != null)
                    {
                        return await RefreshToken(model.Email, user);
                    }
                    else
                    {
                        responseModel.ErrorCode = response.ErrorCode;
                        responseModel.Message = response.Message;
                    }
                }
                else
                {
                    responseModel.ErrorCode = response.ErrorCode;
                    responseModel.Message = response.Message;
                }
            }
            return responseModel;
        }
        
        [HttpGet]
        public async Task<object> RefreshToken(string email)
        {
            var responseModel = new ModelBaseStatus();
            //Call refresh token
            var user = await _userManager.FindByNameAsync(email);
            if (user != null)
            {
                return await RefreshToken(email, user);           
            }
            else
            {
                responseModel.ErrorCode = "001";
                responseModel.Message = "Không tồn tại account";
            }
            return responseModel;
        }

        private async Task<object> RefreshToken(string email, ApplicationUser modelUser)
        {
            var responseModel = new SumProfileResponseDTO();
            responseModel.Profile = new ProfileReponse();
            responseModel.ListRole = new List<ListRole>();
            responseModel.Profile.Email = email;
            //Check exists User
            var user = await _userManager.FindByNameAsync(email);
            if (user != null)
            {
                var aspNetUserLogin =
                  _repositoryWrapper.AspNetUserProfiles.FirstOrDefault(p => p.UserId == user.Id);
                responseModel.Profile = _mapper.Map<ProfileReponse>(user);
                responseModel.Profile.FullName = aspNetUserLogin.FullName;
                responseModel.Profile.RegType = aspNetUserLogin.RegType;
                responseModel.Profile.AvartarFileName = aspNetUserLogin.AvatarUrl ?? "noimage.png";
                responseModel.Profile.AvartarFullUrl =
                   _configuration["Cloud_Path"] + $"/user/avatar/original/{aspNetUserLogin.AvatarUrl ?? "noimage.png"}";

                //List Role
                var lstRole = await _repositoryWrapper.AspNetUsers.GetListRole(user.Id);
                responseModel.ListRole = _mapper.Map<List<ListRole>>(lstRole);
                //ProductBrandId
                var userProfiler = await _repositoryWrapper.AspNetUserProfiles.FirstOrDefaultAsync(p => p.UserId == user.Id);
                if (userProfiler != null)
                {
                    responseModel.ProductBrandId = userProfiler.ProductBrand_ID ?? 0;
                }
                else
                {
                    responseModel.ProductBrandId = 0;
                }
                // responseModel.Role = lstRole.Count > 0 ? string.Join(",", lstRole) : "";
                var jwt = await GenerateJwtToken(email, modelUser);
                responseModel.JWT = jwt.ToString();
                responseModel.UserId = user.Id;
                responseModel.ErrorCode = "00";
                responseModel.Message = "Đăng nhập thành công";
                //Total UnRead
                responseModel.NumberFCMUnread = await _repositoryWrapper.FCMMessage.GetNumberFCMUnread(user.Id);
            }
            return responseModel;

        }
        private async Task<CheckExpireToken> GetPrincipalFromExpiredToken(string token)
         {
            var response = new CheckExpireToken();
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"])),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

                var jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    response.ErrorCode = "001";
                    response.Message = "Invalid Tokken";
                    //var ms = jwtSecurityToken.Claims;
                }
                else
                {
                    response.Email = jwtSecurityToken.Claims.ToList()[0].Value;
                    response.ErrorCode = "00";
                    response.Message = "Validate Tokken";
                }
                return response;
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(SecurityTokenExpiredException))
                {
                    response.ErrorCode = "003";
                    response.Message = "Token hết hạn";
                    return response;
                }
                else
                {
                    response.ErrorCode = "001";
                    response.Message = $"Lỗi : {ex.ToString()}";
                    return response;
                }
            }
        }

        private async Task<object> LoginBySocial([FromBody] LoginSocialDto model)
        {
            var responseModel = new LoginSocialDto();
            responseModel.Email = model.Email;
            responseModel.LoginProvider = model.LoginProvider;
            responseModel.ProviderKey = model.ProviderKey;
            if (Util.IsPhoneNumber(model.Email))
            {
                model.Email = $"{model.Email}@hanoma.vn";
            }
            //Check input Email Pass
            if (String.IsNullOrEmpty(model.Email))
            {
                responseModel.ErrorCode = "ACC006";
                responseModel.Message = ConstMessage.GetMsgConst("ACC006");
                return responseModel;
            }
            else
            {
                //Check exists User
                var userIDSocial =
                    _repositoryWrapper.AspNetUsers.CheckExistsUserSocial(model.LoginProvider, model.ProviderKey);
                if (userIDSocial != null)
                {
                    return LoginSocialReturn(userIDSocial, model);
                }
                else
                {
                    if (Util.IsPhoneNumber(model.Email))
                    {
                        model.Email = $"{model.Email}@hanoma.vn";
                    }
                    var user = new ApplicationUser
                    {
                        UserName = model.Email,
                        Email = model.Email
                    };
                    var result = await _userManager.CreateAsync(user);

                    if (result.Succeeded)
                    {

                        _logger.LogInfo($"[AccountController] Đăng ký thành công {model.Email} ");
                        try
                        {
                            _repositoryWrapper.AspNetUsers.CreateNewUserProfile(user.Id, user.Email, null, 1);
                            //Create Social 
                            _repositoryWrapper.AspNetUsers.CreateUserSocial(model.LoginProvider, model.ProviderKey, user.Id);
                            //return 
                            return LoginSocialReturn(user.Id, model);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"[Account Controller] {ex}");
                            return LoginSocialReturn(null, model);
                        }

                    }
                    else
                    {
                        _logger.LogError($"[Account Controller] Error with Create Account Social");
                        return LoginSocialReturn(null, model);
                    }
                }
            }
        }
        /// <summary>
        /// Forgot Password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> ForgotPassword([FromBody] ForgotPassworDTO model)
        {
            var responseModel = new ForgotPassworDTO();
            responseModel.EmailOrPhone = model.EmailOrPhone;

            if (Util.IsPhoneNumber(model.EmailOrPhone))
            {
                //var modelPhone = $"{model.EmailOrPhone}@hanoma.vn";
                var user = await _userManager.FindByNameAsync(model.EmailOrPhone);
                if (user == null)
                {
                    responseModel.ErrorCode = "ACC008";
                    responseModel.Message = ConstMessage.GetMsgConst("ACC008");
                    return responseModel;
                }

                var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.EmailOrPhone);
                await SendCode(Util.IsPhoneNumber(model.EmailOrPhone) ? "Phone" : "Email", model.EmailOrPhone, code);
                responseModel.ErrorCode = "00";
                responseModel.Message = "Đã gửi code xác nhận qua số điện thoại";
                return responseModel;
            }
            else
            {
                var user = await _userManager.FindByNameAsync(model.EmailOrPhone);

                if (user == null)
                {
                    responseModel.ErrorCode = "ACC008";
                    responseModel.Message = ConstMessage.GetMsgConst("ACC008");
                    return responseModel;
                }
                var codeMail = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.EmailOrPhone);
                await SendCode(Util.IsPhoneNumber(model.EmailOrPhone) ? "Phone" : "Email", model.EmailOrPhone, codeMail);
                //await _emailSender.SendEmailAsync(model.EmailOrPhone, "Mã xác thực lấy lại mật khẩu", $"Mã xác thực của bạn là:{codeMail}",_repositoryWrapper.AspNetUsers.setting());
                //Util.SendMail("",model.EmailOrPhone,"","Xác thực lấy lại mật khẩu",$"Mã xác thực của bạn là:{codeMail}",_repositoryWrapper.AspNetUsers.setting());
                responseModel.ErrorCode = "00";
                responseModel.Message = "Đã gửi code xác nhận qua email";
                return responseModel;
            }
        }
        /// <summary>
        /// Set Password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> SetPassword([FromBody] SetPassDTO model)
        {
            var responseModel = new SetPassDTO();
            responseModel.EmailOrPhone = model.EmailOrPhone;

            var user = await _userManager.FindByNameAsync(model.EmailOrPhone);
            if (user == null)
            {
                responseModel.ErrorCode = "ACC008";
                responseModel.Message = ConstMessage.GetMsgConst("ACC008");
                return responseModel;
            }


            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.NewPassword);
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                responseModel.ErrorCode = "00";
                responseModel.Message = "Cập nhật mật khẩu thành công";
                return responseModel;
            }
            else
            {
                responseModel.ErrorCode = "002";
                responseModel.Message = ConstMessage.GetMsgConst("002");
                return responseModel;
            }
        }
        [HttpGet]
        public async Task<object> VerifyCode(string EmailOrPhone, string code)
        {
            var responseModel = new ConfirmVerify();
            responseModel.EmailOrPhone = EmailOrPhone;
            responseModel.Code = code;
            var phoneNumEmail = EmailOrPhone;
            if (Util.IsPhoneNumber(EmailOrPhone))
            {
                phoneNumEmail = $"{EmailOrPhone}@hanoma.vn";
            }
            var user = await _userManager.FindByNameAsync(EmailOrPhone);
            if (user == null)
            {
                responseModel.ErrorCode = "ACC008";
                responseModel.Message = ConstMessage.GetMsgConst("ACC008");
                return responseModel;
            }

            if (!Util.IsPhoneNumber(EmailOrPhone))
            {
                var result = await _userManager.ChangePhoneNumberAsync(user, EmailOrPhone, code);                
                //var result = await _signInManager.TwoFactorSignInAsync("Email", code, false, false);

                if (result.Succeeded)
                {
                    //Upgrade Mail Confirm
                    var codeMail = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var result2 = await _userManager.ConfirmEmailAsync(user, codeMail);
                    var codePhone = await _userManager.GenerateChangePhoneNumberTokenAsync(user, "");
                    var result3 = await _userManager.ChangePhoneNumberAsync(user, "", codePhone);


                    //await _userManager.SetPhoneNumberAsync(user, "");
                    _repositoryWrapper.FCMMessage.PushNotificationToRabitMQ(new NotificationRabitMQModel
                    {
                        Type = "ONDEMAND",
                        NotificationCode = "DKTK",
                        ChannelSend = "ALL",
                        UsingTemplate = true,
                        UserId = user.Id,
                    });
                    responseModel.ErrorCode = "00";
                    responseModel.Message = "Verify Thành công";

                    return responseModel;
                }
                else
                {
                    //Set phone number empty
                    //await _userManager.SetPhoneNumberAsync(user, "");
                    responseModel.ErrorCode = "ACC012";
                    responseModel.Message = "Verify không thành công";
                    return responseModel;
                }
            }
            else
            {
                var result = await _userManager.ChangePhoneNumberAsync(user, EmailOrPhone, code);
                //var result = await _signInManager.TwoFactorSignInAsync("Email", code, false, false);
                //Upgrade Mail Confirm
                var codeMail = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var result2 = await _userManager.ConfirmEmailAsync(user, codeMail);

                if (result.Succeeded)
                {
                    _repositoryWrapper.FCMMessage.PushNotificationToRabitMQ(new NotificationRabitMQModel
                    {
                        Type = "ONDEMAND",
                        NotificationCode = "DKTK",
                        ChannelSend = "ALL",
                        UsingTemplate = true,
                        UserId = user.Id,
                    });
                    responseModel.ErrorCode = "00";
                    responseModel.Message = "Verify Thành công";
                    return responseModel;
                }
                else
                {
                    responseModel.ErrorCode = "ACC012";
                    responseModel.Message = "Verify không thành công";
                    return responseModel;
                }
            }
        }
        private async Task<LoginSocialDto> LoginSocialReturn(string userIDSocial, LoginSocialDto model)
        {
            var responseModel = new LoginSocialDto();
            responseModel.Email = model.Email;
            responseModel.LoginProvider = model.LoginProvider;
            responseModel.ProviderKey = model.ProviderKey;
            if (!String.IsNullOrEmpty(userIDSocial))
            {
                var appUser = _userManager.Users.SingleOrDefault(r => r.Id == userIDSocial);
                responseModel.Email = model.Email;
                var lstRole = await _userManager.GetRolesAsync(appUser);
                //responseModel.Role = lstRole.Count > 0 ? string.Join(",", lstRole) : "";
                var jwt = await GenerateJwtToken(model.Email, appUser);
                responseModel.JWT = jwt.ToString();
                responseModel.Id = appUser.Id;
                return responseModel;
            }
            else
            {
                responseModel.ErrorCode = "005";
                responseModel.Message = ConstMessage.GetMsgConst("005");
                return responseModel;

            }

        }

        [HttpPost]
        public async Task<object> Register([FromBody] RegisterDto model)
        {
            var reponseModel = new RegisterDto();
            reponseModel.Email = model.Email;
            var InputEmail = model.Email;
            var code = "";
            if (Util.IsPhoneNumber(model.Email))
            {
                InputEmail = $"{model.Email}@hanoma.vn";
            }
            //Check input Email Pass
            if (String.IsNullOrEmpty(model.Email) || String.IsNullOrEmpty(model.Password))
            {
                reponseModel.ErrorCode = "ACC006";
                reponseModel.Message = ConstMessage.GetMsgConst("ACC006");
                return reponseModel;
            }
            //Check exists User with not confirm
            if(_repositoryWrapper.AspNetUsers.CheckExistsUserNotConfirmed(model.Email))
            {
                //reponseModel.ErrorCode = "ACC013";
                //reponseModel.Message = ConstMessage.GetMsgConst("ACC013");
                //SentCode if not confirm

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = InputEmail
                };
                if (!Util.IsPhoneNumber(model.Email))
                {

                    code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.Email);
                }
                else
                {
                    code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.Email);
                }
                await SendCode(Util.IsPhoneNumber(model.Email) ? "Phone" : "Email", model.Email, code);
                reponseModel.ErrorCode = "00";
                reponseModel.Message = "Đã gửi code xác nhận";
                return reponseModel;
            }    
            //Check exists User
            if (_repositoryWrapper.AspNetUsers.CheckExistsUser(model.Email))
            {
                reponseModel.ErrorCode = "ACC009";
                reponseModel.Message = ConstMessage.GetMsgConst("ACC009");
                return reponseModel;
            }

            if (!Util.IsEmailOrPhone(model.Email))
            {
                _logger.LogError($"[AccountController] {model.Email}" + ConstMessage.GetMsgConst("ACC010"));
                reponseModel.ErrorCode = "ACC010";
                reponseModel.Message = ConstMessage.GetMsgConst("ACC010");
                return reponseModel;
            }
            else
            {

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = InputEmail
                };

                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    //Set EnableTwoFactorAuthentication
                    //await _userManager.SetTwoFactorEnabledAsync(user, true);

                    //Update Account Code
                    var userRegister = _repositoryWrapper.AspNetUsers.FirstOrDefault(x => x.Id.Equals(user.Id));
                    userRegister.AccountCode = $"84{model.Email}";
                    _hanomaContext.SaveChanges();
                    



                    _logger.LogInfo($"[AccountController] Đăng ký thành công {model.Email} ");
                    try
                    {
                        //Create User Profile
                        _repositoryWrapper.AspNetUsers.CreateNewUserProfile(user.Id, user.Email, null, 1);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"[Account Controller] {ex}");
                    }
                    if (!Util.IsPhoneNumber(model.Email))
                    {
                        
                        code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.Email);
                    }
                    else
                    {
                        code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.Email);
                    }

                    //Thay đổi luồng verify số điện thoại trước
                    await SendCode(Util.IsPhoneNumber(model.Email) ? "Phone" : "Email", model.Email, code);
                    reponseModel.Email = model.Email;
                    reponseModel.Password = model.Password;
                    reponseModel.ErrorCode = "00";
                    reponseModel.Message = "Đăng ký thành công";
                    return reponseModel;
                }

            }

            throw new ApplicationException("UNKNOWN_ERROR");
        }

        [HttpPost]
        public async Task<object> ResendCode([FromBody] ForgotPassworDTO model)
        {
            var responseModel = new ModelBase();
            var code = "";
            var user = await _userManager.FindByNameAsync(model.EmailOrPhone);
            if (user == null)
            {
                responseModel.ErrorCode = "ACC008";
                responseModel.Message = ConstMessage.GetMsgConst("ACC008");
                return responseModel;
            }
            if (!Util.IsPhoneNumber(model.EmailOrPhone))
            {
                code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.EmailOrPhone);
            }
            else
            {
                code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.EmailOrPhone);
            }


            await SendCode(Util.IsPhoneNumber(model.EmailOrPhone) ? "Phone" : "Email", model.EmailOrPhone, code);
            responseModel.ErrorCode = "00";
            responseModel.Message = "Đã gửi code xác nhận";
            return responseModel;
        }

        private async Task<object> SendCode(string provider, string emailOrPhone, string code)
        {

            if (provider == "Email")
            {
                await _emailSender.SendEmailAsync(emailOrPhone, "Mã xác thực lấy lại mật khẩu", $"Mã xác thực của bạn là:{code}", _repositoryWrapper.AspNetUsers.setting());
            }
            else if (provider == "Phone")
            {
                Util.SendSMS($"(hanoma.vn) Ma code xac thuc cua ban la {code}", emailOrPhone);
            }

            return NoContent();
        }
            

        [Authorize]
        [HttpPost]
        public async Task<object> ChangePassword([FromBody] ChangePassDTO model)
        {
            var responseModel = new ChangePassDTO();
            responseModel.Email = model.Email;
            var InputEmail = model.Email;
            if (Util.IsPhoneNumber(model.Email))
            {
                InputEmail = $"{model.Email}@hanoma.vn";
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Check input Email Pass
            if (String.IsNullOrEmpty(model.Email) || String.IsNullOrEmpty(model.NewPassword) || String.IsNullOrEmpty(model.OldPassword))
            {
                responseModel.ErrorCode = "ACC006";
                responseModel.Message = ConstMessage.GetMsgConst("ACC006");
                return responseModel;
            }
            //Check exists User
            //if (_repositoryWrapper.AspNetUsers.CheckExistsUser(InputEmail))
            //{
            //    responseModel.ErrorCode = "ACC009";
            //    responseModel.Message = ConstMessage.GetMsgConst("ACC009");
            //    return responseModel;
            //}            
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                responseModel.ErrorCode = "ACC008";
                responseModel.Message = ConstMessage.GetMsgConst("ACC008");
                return responseModel;
            }
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (changePasswordResult.Succeeded)
            {
                //Sent notification to rabitMQ
                await _repositoryWrapper.FCMMessage.PushNotificationToRabitMQ(new NotificationRabitMQModel
                {
                    Type = "ONDEMAND",
                    NotificationCode = "DMK",
                    ChannelSend = "ALL",
                    UsingTemplate = true,
                    UserId = user.Id                  
                });

                responseModel.ErrorCode = "00";
                responseModel.Message = "Thiết lập mật khẩu thành công";
                return Ok(responseModel);
            }
            else
            {
                responseModel.ErrorCode = "ACC015";
                responseModel.Message = ConstMessage.GetMsgConst("ACC015");
                return responseModel;
            }
        }
        private async Task<object> GenerateJwtToken(string email, ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));
            //var expires = DateTime.Now.AddSeconds(30);

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}