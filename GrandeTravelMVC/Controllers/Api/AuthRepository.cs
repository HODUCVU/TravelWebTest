﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrandeTravelMVC.ViewModels;
using GrandeTravelMVC.Services;

namespace GrandeTravelMVC.Controllers.Api
{
    public class AuthRepository : IDisposable
    {
        private MyDbContext _ctx;

        private UserManager<IdentityUser> _userManager;

        //public AuthRepository()
        //{
        //    _ctx = new MyDbContext();
        //    _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_ctx));
        //}

        public async Task<IdentityResult> RegisterUser(AccountRegisterViewModel userModel)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = userModel.UserName
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);

            return result;
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await _userManager.FindAsync(userName, password);

            return user;
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();
        }
    }
}
