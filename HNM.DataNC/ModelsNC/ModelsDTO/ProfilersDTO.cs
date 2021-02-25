using HNM.DataNC.ModelsNC.ModelsUtility;
using System;
using System.Collections.Generic;

namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    public class ProfilersDTO : ModelBase
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string AvatarUrl { get; set; }
        public string FullAvatarUrl => Utils.CloudPath() + $"/user/avatar/original/{AvatarUrl ?? "noimage.png"}";
        public bool? Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
    public class UpdateAvatarDTO : ModelBase
    {
        public string UserId { get; set; }
        public string AvatarUrl { get; set; }
        public string FullAvatarUrl => Utils.CloudPath() + $"/user/avatar/original/{AvatarUrl ?? "noimage.png"}";
    }
    public class SumProfileResponseDTO : ModelBase
    {
        /// <summary>
        /// Profile respose
        /// </summary>
        public ProfileReponse Profile { get; set; }
        /// <summary>
        /// List Role response
        /// </summary>
        public List<ListRole> ListRole { get; set; }
        public int ProductBrandId { get; set; }
        //ProductBrandType
        public int ProductBrandTypeId { get; set; }
        public int ProductBrandYearJoin { get; set; }
        public string ProductBrandAvatarUrl { get; set; }
        public string ProductBrandName { get; set; }
        public string ReferralCode { get; set; }
        public FCMNumberUnread NumberFCMUnread { get; set; }
    }

    public class ProfileReponse
    {
        public string Id { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Phone
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Phone Confirm Or not
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
        public bool? IsEnabled { get; set; }
        public string ConcurrencyStamp { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public string NormalizedEmail { get; set; }
        public string NormalizedUserName { get; set; }
        /// <summary>
        /// Email Confirm Or not
        /// </summary>
        public string EmailConfirmed { get; set; }

        public string FullName { get; set; }
        public string Gender { get; set; }
        /// <summary>
        /// Avatar File Name
        /// </summary>
        public string AvartarFileName { get; set; }
        /// <summary>
        /// Avatar Full Url
        /// </summary>
        public string AvartarFullUrl { get; set; }
        /// <summary>
        /// RegType Registration account Email Or Phone
        /// </summary>
        public string RegType { get; set; }
        public string AccountCode { get; set; }
        public string RegisterDate { get; set; }        
    }
    public class ListRole
    {
        /// <summary>
        /// Role ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Role Name
        /// </summary>
        public string Name { get; set; }
    }
}
