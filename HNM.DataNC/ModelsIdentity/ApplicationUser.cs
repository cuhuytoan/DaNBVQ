using Microsoft.AspNetCore.Identity;


namespace HNM.DataNC.ModelsIdentity
{
    public class ApplicationUser : IdentityUser
    {
        //public Nullable<bool> IsEnabled { get; set; }
        //public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        //{
        //    // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
        //    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

        //    identity.AddClaim(new Claim(ClaimTypes.Name, this.UserName));
        //    // Add custom user claims here
        //    return identity;
        //}
        //public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        //{
        //    // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
        //    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, "", authenticationType);

        //    identity.AddClaim(new Claim(ClaimTypes.Name, this.UserName));

        //    // Add custom user claims here
        //    return identity;
        //}

    }

    public class ApplicationRole : IdentityRole
    {

    }
}
