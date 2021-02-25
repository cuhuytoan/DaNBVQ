using HNM.DataNC.Models;
using HNM.RepositoriesNC.RepositoriesBase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HNM.RepositoriesNC.Repositories
{
    public interface IAccountRepository : IRepositoryBase<AspNetUsers>
    {
        bool CheckExistsUser(string emailOrPhone);
        bool CheckExistsUserNotConfirmed(string emailOrPhone);
        void CreateNewUserProfile(string userId, string Email, string Phone = null, int? accountType = 1);
        string CheckExistsUserSocial(string LoginProvider, string ProviderKey);
        void CreateUserSocial(string LoginProvider, string ProviderKey, string UserId);
        Setting setting();
        Task<List<AspNetRoles>> GetListRole(string UserId);
        AspNetUserLogins GetAspNetUserLogins(string UserId);

    }
    public class AccountRepository : RepositoryBase<AspNetUsers>, IAccountRepository
    {
        public AccountRepository(HanomaContext HanomaContext) : base(HanomaContext)
        {
        }

        public bool CheckExistsUser(string emailOrPhone)
        {
            var result = HanomaContext.AspNetUsers.FirstOrDefault(p => p.NormalizedUserName == emailOrPhone.ToUpper() || p.UserName == emailOrPhone);
            if (result != null) return true;
            return false;
        }
        public string CheckExistsUserSocial(string LoginProvider, string ProviderKey)
        {
            var result = HanomaContext.AspNetUserLogins.FirstOrDefault(p =>
                p.LoginProvider == LoginProvider && p.ProviderKey == ProviderKey);
            return result?.UserId;
        }

        public void CreateUserSocial(string LoginProvider, string ProviderKey, string UserId)
        {
            var checkExist = HanomaContext.AspNetUserLogins.FirstOrDefault(p =>
                p.LoginProvider == LoginProvider && p.ProviderKey == ProviderKey);
            if (checkExist == null)
            {
                AspNetUserLogins item = new AspNetUserLogins();
                item.LoginProvider = LoginProvider;
                item.ProviderKey = ProviderKey;
                item.UserId = UserId;
                HanomaContext.AspNetUserLogins.Add(item);
                HanomaContext.SaveChangesAsync();
            }

        }

        public Setting setting()
        {
            return HanomaContext.Setting.FirstOrDefault(p => p.Setting_ID == 1);
        }

        public Task<List<AspNetRoles>> GetListRole(string UserId)
        {

            return HanomaContext.AspNetRoles
                .FromSqlRaw(
                    $"SELECT B.* FROM AspnetUserRoles A JOIN AspNetRoles B ON A.RoleId = B.Id WHERE A.UserId = '{UserId}'")
                .AsNoTracking()
                .ToListAsync();
        }

        public AspNetUserLogins GetAspNetUserLogins(string UserId)
        {
            return HanomaContext.AspNetUserLogins.FirstOrDefault(p => p.UserId == UserId);
        }

        public void CreateNewUserProfile(string userId, string Email, string Phone = null, int? accountType = 1)
        {
            try
            {

                var profileExist = HanomaContext.AspNetUserProfiles.FirstOrDefault(x => x.UserId.Equals(userId));
                if (profileExist != null) return;


                var regtype = String.IsNullOrEmpty(Phone) ? "Email" : "Phone";
                if (regtype == "Email")
                {
                    //SendEmailToCEO(userId, Email);
                }
                else
                {
                    //SendEmailToCEO(userId, Phone);
                }

                var roles = HanomaContext.AspNetRoles.ToList();
                var productBrand_ID = 0;

                HanomaContext.AspNetUserRoles.Add(new AspNetUserRoles()
                {
                    UserId = userId,
                    RoleId = roles.FirstOrDefault(x => x.Name.Equals("Thành viên")).Id
                });


                var profilers = new AspNetUserProfiles
                {
                    UserId = userId,
                    //FullName = Email,
                    Phone = Phone,
                    AccountType = 1, //accountType ?? 1,
                    ProductBrand_ID = productBrand_ID,
                    RegType = regtype,
                    RegisterDate = DateTime.Now,
                    LastActivityDate = DateTime.Now,
                    Rank = 1,
                    Location_ID = -1
                };

                var profile = HanomaContext.AspNetUserProfiles.Add(profilers);
                HanomaContext.SaveChanges();

            }
            catch (Exception exception)
            {

                throw exception;
            }
        }

        public bool CheckExistsUserNotConfirmed(string emailOrPhone)
        {
            var result = HanomaContext.AspNetUsers.FirstOrDefault(p => (p.NormalizedUserName == emailOrPhone.ToUpper() || p.UserName == emailOrPhone) && p.EmailConfirmed == false && p.PhoneNumberConfirmed == false );
            if (result != null) return true;
            return false;
        }
    }
}
