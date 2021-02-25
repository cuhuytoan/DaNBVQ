using HNM.DataNC.ModelsNC.ModelsUtility;
using System;
using System.ComponentModel.DataAnnotations;

namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    public class ContactDTO
    {
    }
    public class ContactSellerDTO
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Yêu cầu Product Id")]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Họ tên liên hệ không được để trống.")]
        [MinLength(6, ErrorMessage = "Họ tên tối thiểu phải có 6 ký tự")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Số điện thoại liên hệ không được để trống.")]
        [RegularExpression(@"^([0-9]{1,12})$", ErrorMessage = "Số điện thoại không đúng định dạng.")]
        public string Phone { get; set; }
        public string Infomation { get; set; }

        public double? Price { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
    }
    public class ContactSellerResultDTO : ModelBaseStatus
    {
    }
}
