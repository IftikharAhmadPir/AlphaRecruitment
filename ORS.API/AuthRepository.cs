using ORS.API.Entities;
using ORS.API.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.AspNet.Identity.Owin;
using System.Net.Mail;
using System.IO;
using System.Net.Mime;

namespace ORS.API
{

    public class AuthRepository : IDisposable
    {
        private AuthContext _ctx;

        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        public AuthRepository()
        {
            _ctx = new AuthContext();
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_ctx));
            var roleStore = new RoleStore<IdentityRole>(_ctx);
            _roleManager = new RoleManager<IdentityRole>(roleStore);
        }
        public async Task<IdentityResult> ChangePassword(ORS.API.Controllers.ChangePasswordViewModel vm)
        {
            return await _userManager.ChangePasswordAsync(vm.UserId, vm.CurrentPassword, vm.Password);
        }
        public async Task<IdentityResult> RegisterUser(UserModel userModel)
        {
            if (userModel.UserRole == "company")
            {
                IdentityUser user = new Employer
                {
                    UserName = userModel.Email,
                    Email = userModel.Email
                };
                IdentityResult result = await _userManager.CreateAsync(user, userModel.Password);

                if (result.Succeeded)
                {
                    IdentityUser userinfo = await FindUser(userModel.Email, userModel.Password);
                    await _userManager.AddToRoleAsync(userinfo.Id, userModel.UserRole.ToString());
                }
                return result;
            }

            else if (userModel.UserRole == "jobseeker")
            {
                IdentityUser user = new Applicant
                {
                    UserName = userModel.Email,
                    Email = userModel.Email
                };
                IdentityResult result = await _userManager.CreateAsync(user, userModel.Password);
                if (result.Succeeded)
                {
                    IdentityUser userinfo = await FindUser(userModel.Email, userModel.Password);
                    await _userManager.AddToRoleAsync(userinfo.Id, userModel.UserRole.ToString());


                }

                return result;
            }
            else if (userModel.UserRole == "AppAdmin")
            {
                IdentityUser user = new Admin
                {
                    UserName = userModel.Email,
                    Email = userModel.Email
                };
                IdentityResult result = await _userManager.CreateAsync(user, userModel.Password);

                if (result.Succeeded)
                {
                    IdentityUser userinfo = await FindUser(userModel.Email, userModel.Password);
                    await _userManager.AddToRoleAsync(userinfo.Id, userModel.UserRole.ToString());
                }

                return result;
            }
            else
            {
                return null;
            }

            //await _roleManager.CreateAsync(new IdentityRole() { Name = "company" });

            //await _roleManager.CreateAsync(new IdentityRole() { Name = "jobseeker" });


        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await _userManager.FindAsync(userName, password);
            return user;
        }

        public IList<Claim> GetUserRole(ClaimsIdentity oAuthIdentity)
        {

            List<Claim> roles = oAuthIdentity.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();

            return roles;

        }

        public Client FindClient(string clientId)
        {
            var client = _ctx.Clients.Find(clientId);


            return client;
        }

        public async Task<bool> AddRefreshToken(RefreshToken token)
        {

            var existingToken = _ctx.RefreshTokens.Where(r => r.Subject == token.Subject && r.ClientId == token.ClientId).SingleOrDefault();

            if (existingToken != null)
            {
                var result = await RemoveRefreshToken(existingToken);
            }

            _ctx.RefreshTokens.Add(token);

            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _ctx.RefreshTokens.FindAsync(refreshTokenId);

            if (refreshToken != null)
            {
                _ctx.RefreshTokens.Remove(refreshToken);
                return await _ctx.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            _ctx.RefreshTokens.Remove(refreshToken);
            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _ctx.RefreshTokens.FindAsync(refreshTokenId);

            return refreshToken;
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            return _ctx.RefreshTokens.ToList();
        }

        public async Task<IdentityUser> FindAsync(UserLoginInfo loginInfo)
        {
            IdentityUser user = await _userManager.FindAsync(loginInfo);

            return user;
        }

        public async Task<IdentityResult> CreateAsync(IdentityUser user)
        {
            var result = await _userManager.CreateAsync(user);

            return result;
        }

        public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
        {
            var result = await _userManager.AddLoginAsync(userId, login);

            return result;
        }


        public async Task<string> GenerateEmailConfirmationTokenAsync(string userid)
        {
            var user = await _userManager.FindByIdAsync(userid);
            var provider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("ORS");
            _userManager.UserTokenProvider = new Microsoft.AspNet.Identity.Owin.DataProtectorTokenProvider<IdentityUser>(provider.Create("EmailConfirmation"));
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(userid);
            return token;
        }
        public Task SendEmailAsync(string userId, string subject, string body)
        {
            return _userManager.SendEmailAsync(userId, subject, body);

        }

        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string code)
        {
            var provider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("ORS");
            _userManager.UserTokenProvider = new Microsoft.AspNet.Identity.Owin.DataProtectorTokenProvider<IdentityUser>(provider.Create("EmailConfirmation"));
            IdentityResult result = await _userManager.ConfirmEmailAsync(userId, code);
            return result;
        }
        #region Password token and resetpassword
        public async Task<string> GeneratePasswordResetTokenAsync(string email)
        {
            IdentityUser user =  await _userManager.FindByEmailAsync(email);
            var provider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("ORS");
            _userManager.UserTokenProvider = new Microsoft.AspNet.Identity.Owin.DataProtectorTokenProvider<IdentityUser>(provider.Create("EmailConfirmation"));
            return await _userManager.GeneratePasswordResetTokenAsync(user.Id);
        }

        public async Task<IdentityResult> ResetPasswordAsync(string userid, string token, string newpassword)
        {
            var provider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("ORS");
            _userManager.UserTokenProvider = new Microsoft.AspNet.Identity.Owin.DataProtectorTokenProvider<IdentityUser>(provider.Create("EmailConfirmation"));
            return await _userManager.ResetPasswordAsync(userid,token,newpassword);
        }
        #endregion
        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }

        public async Task<ClaimsIdentity> CreateIdentityAsync(IdentityUser user, string authenticationType)
        {
            var result = await _userManager.CreateIdentityAsync(user, authenticationType);
            return result;
        }





    }
}