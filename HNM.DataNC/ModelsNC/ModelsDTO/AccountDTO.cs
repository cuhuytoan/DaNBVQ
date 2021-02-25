using HNM.DataNC.ModelsNC.ModelsUtility;
using System.ComponentModel.DataAnnotations;

namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    public class AccountDTO
    {

    }
    public class LoginDto : ModelBase
    {
        /// <summary>
        /// Require Email or Phone
        /// </summary>
        [Required(ErrorMessage = "Email hoặc số điện thoại không được để trống")]
        public string Email { get; set; }
        /// <summary>
        /// Require Password
        /// </summary>
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string Password { get; set; }
        public string Id { get; set; }

    }
    public class LoginTokenDTO
    {
        public string Email { get; set; }
        public string Jwt { get; set; }
    }
    public class LoginSocialDto : ModelBase
    {

        [Required]
        public string Email { get; set; }
        [Required]
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string Id { get; set; }
    }
    public class RegisterDto : ModelBase
    {
        /// <summary>
        /// Require Email or Phone
        /// </summary>
        [Required]
        public string Email { get; set; }
        /// <summary>
        /// Require Password
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
        public string Password { get; set; }
    }

    public class ChangePassDTO : ModelBase
    {
        /// <summary>
        /// Require Email or Phone
        /// </summary>
        [Required]
        public string Email { get; set; }
        /// <summary>
        /// Require Old Password
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
        public string OldPassword { get; set; }
        /// <summary>
        /// Require New Password
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
        public string NewPassword { get; set; }
    }

    public class ForgotPassworDTO : ModelBase
    {
        /// <summary>
        /// Require Email or Phone
        /// </summary>
        public string EmailOrPhone { get; set; }
        /// <summary>
        /// No need
        /// </summary>
        public string Code { get; set; }
    }

    public class ConfirmVerify : ModelBase
    {
        /// <summary>
        /// Require Email or Phone
        /// </summary>
        public string EmailOrPhone { get; set; }
        /// <summary>
        /// Require Code
        /// </summary>
        public string Code { get; set; }
    }
    public class SetPassDTO : ModelBase
    {
        /// <summary>
        /// Require Email or Phone
        /// </summary>
        [Required]
        public string EmailOrPhone { get; set; }
        /// <summary>
        /// Require Email or Phone Must be check Client First
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
        public string NewPassword { get; set; }
        /// <summary>
        /// Require Email or Phone
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
        public string ConfirmPassword { get; set; }
    }
    public class CheckExpireToken :ModelBaseStatus
    {
        public string Email { get; set; }
    }
}
