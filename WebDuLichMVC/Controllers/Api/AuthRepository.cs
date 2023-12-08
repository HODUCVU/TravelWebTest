using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
// using  Microsoft.AspNetCore.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebDuLichMVC.Services;
using WebDuLichMVC.ViewModels;

namespace WebDuLichMVC.Controllers.Api
{
    public class AuthRepository : IDisposable
    {
        private MyDbContext _context;
        private UserManager<IdentityUser> _userManager;
        // public AuthRepository()
        // {
        //     _context = new MyDbContext();
        //     // _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_context));  
        // }
        
        public async Task<IdentityResult> RegisterUser(AccountRegisterViewModel userModel)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = userModel.UserName
            };
            var result = await _userManager.CreateAsync(user, userModel.Password);
            return result;
        }

        public async Task<IdentityUser> FindUser(string username, string password)
        {
            IdentityUser user = await _userManager.FindAsync(username, password);
            return user;
        }
        public void Dispose()
        {
            _context.Dispose();
            _userManager.Dispose();
        }
    }
}