using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebDuLichMVC.Models;
using WebDuLichMVC.Services;
using WebDuLichMVC.ViewModels;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;
namespace WebDuLichMVC.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<IdentityUser> _userManagerService;
        private SignInManager<IdentityUser> _signinManagerService;
        private RoleManager<IdentityRole> _roleManagerService;
        private IDataService<CustomerProfile> _custProfileDataService;
        private IDataService<ProviderProfile> _provProfileDataService;
        private IDataService<Location> _locationDataService;
        private IDataService<Package> _packageDataService;
        private IDataService<Order> _orderDataService;
        private IWebHostEnvironment _environment;

        public AccountController(UserManager<IdentityUser> userManager,
                                SignInManager<IdentityUser> signInManager,
                                RoleManager<IdentityRole> roleManager,
                                IDataService<CustomerProfile> custProfileService,
                                IDataService<ProviderProfile> provProfileService,
                                IDataService<Location> locationService,
                                IDataService<Package> packageService,
                                IDataService<Order> orderService,
                                IWebHostEnvironment environment)
        {
              _userManagerService = userManager;
            _signinManagerService = signInManager;
            _roleManagerService = roleManager;
            _custProfileDataService = custProfileService;
            _provProfileDataService = provProfileService;
            _locationDataService = locationService;
            _packageDataService = packageService;
            _orderDataService = orderService;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AccountRegisterViewModel vm)
        {
            if(ModelState.IsValid)
            {
                IdentityUser identityUser = new IdentityUser(vm.UserName);
                IdentityResult identityResult = await _userManagerService.CreateAsync(identityUser, vm.Password);
                identityUser.Email = vm.UserName;
                identityUser.PhoneNumber = vm.Mobile;

                if(identityResult.Succeeded)
                {
                    if(vm.IsProvider == false)
                    {
                        if(await _roleManagerService.FindByIdAsync("Customer") != null)
                        {
                            await _userManagerService.AddToRoleAsync(identityUser, "Customer");  //***
                        }
                        identityUser = await _userManagerService.FindByNameAsync(vm.UserName);
                        CustomerProfile profile = new CustomerProfile {
                            Picture = "profilepicgen.png",
                            UserId = identityUser.Id
                        };
                        _custProfileDataService.Create(profile);
                        return RedirectToAction("Index", "Home");
                    }
                    if(vm.IsProvider == true)
                    {
                        if( await _roleManagerService.FindByIdAsync("Provider") != null)
                        {
                            await _userManagerService.AddToRoleAsync(identityUser, "Provider");
                        }
                        identityUser = await _userManagerService.FindByNameAsync(vm.UserName);
                        ProviderProfile profile = new ProviderProfile {
                            CompanyLogo = "profilepicgen.png",
                            UserId = identityUser.Id
                        };
                        _provProfileDataService.Create(profile);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else 
                {
                    foreach(var item in identityResult.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(vm);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "")
        {
            AccountLoginViewModel vm = new AccountLoginViewModel
            {
                ReturnUrl = returnUrl
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AccountLoginViewModel vm)
        {
            if(ModelState.IsValid)
            {
                var result = await _signinManagerService.PasswordSignInAsync(vm.UserName, vm.Password, vm.RememberMe, false);
                if(result.Succeeded)
                {
                    if(!string.IsNullOrEmpty(vm.ReturnUrl))
                    {
                        return Redirect(vm.ReturnUrl);
                    }
                    else{
                        return RedirectToAction("Index", "Home");
                    }
                }
                else 
                {
                    ModelState.AddModelError("", "Email or Password incorrect");
                }
            }
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signinManagerService.SignOutAsync();
            return RedirectToAction("Index", " Home");
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateCustProfile()
        {
            IdentityUser user = await _userManagerService.FindByNameAsync(User.Identity.Name);
            CustomerProfile profile = _custProfileDataService.GetSignle(s => s.UserId == user.Id);
            IEnumerable<Order> orders = _orderDataService.Query(o => o.UserId == user.Id);
            AccountUpdateCustProfileViewModel vm = new AccountUpdateCustProfileViewModel {
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Phone = user.PhoneNumber,
                Address = profile.Address,
                Picture = profile.Picture
            };
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateCustProfile(AccountUpdateCustProfileViewModel vm, IFormFile file)
        {
            if(ModelState.IsValid)
            {
                IdentityUser user = await _userManagerService.FindByNameAsync(User.Identity.Name);
                CustomerProfile profile = _custProfileDataService.GetSignle(s => s.UserId == user.Id);

                profile.UserId = user.Id;
                profile.FirstName = vm.FirstName;
                profile.LastName = vm.LastName;
                profile.Phone = vm.Phone;
                profile.Address = vm.Address;

                if(file != null)
                {
                    var serverPath = Path.Combine(_environment.WebRootPath, "uploads/custprofile");
                    Directory.CreateDirectory(Path.Combine(serverPath,User.Identity.Name));
                    string filename = FileNameHelper.GetNameFormatted(Path.GetFileName(file.FileName));
                    using(var fileStream = new FileStream(Path.Combine(serverPath, User.Identity.Name, filename), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    profile.Picture = User.Identity.Name + "/" + filename;
                }
                _custProfileDataService.Update(profile);
                
                //user.UserName = vm.UserName;
                //user.Email = vm.UserName;
                await _userManagerService.UpdateAsync(user);
            }
            return View(vm);
        }

        //Provider
        [HttpGet]
        [Authorize(Roles = "Provider")]
        public async Task<IActionResult> UpdateProvProfile(int id) //**
        {
            IdentityUser user = await _userManagerService.FindByNameAsync(User.Identity.Name);
            ProviderProfile profile = _provProfileDataService.GetSignle(s => s.UserId == user.Id);
            IEnumerable<Location> locations = _locationDataService.GetAll();
            IEnumerable<Package> packages = _packageDataService.Query(p =>p.UserId == user.Id);
            AccountUpdateProvProfileViewModel vm = new AccountUpdateProvProfileViewModel {
                CompanyName = profile.CompanyName,
                Website = profile.Website,
                Phone = user.PhoneNumber,
                Address = profile.Address,
                CompanyLogo = profile.CompanyLogo
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Provider")]
        public async Task<IActionResult> UpdateProvProfile(AccountUpdateProvProfileViewModel vm, IFormFile file)
        {
            if(ModelState.IsValid)
            {
                IdentityUser user = await _userManagerService.FindByNameAsync(User.Identity.Name);
                ProviderProfile profile = _provProfileDataService.GetSignle(s => s.UserId == user.Id);

                profile.UserId = user.Id;
                profile.CompanyName = vm.CompanyName;
                profile.Website = vm.Website;
                profile.Phone = vm.Phone;
                profile.Address = vm.Address;
                if(file != null)
                {
                    var serverPath = Path.Combine(_environment.WebRootPath, "uploads/provprofile");
                    Directory.CreateDirectory(Path.Combine(serverPath, User.Identity.Name));
                    string fileName = FileNameHelper.GetNameFormatted(Path.GetFileName(file.FileName));
                    using (var fileStream = new FileStream(Path.Combine(serverPath, User.Identity.Name, fileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    profile.CompanyLogo = User.Identity.Name + "/" + fileName;
                }
                _provProfileDataService.Update(profile);

                await _userManagerService.UpdateAsync(user);
                return RedirectToAction("Index", "Home");
            }
            return View(vm);
        }

        // cooki refushe
        [HttpGet]
        public IActionResult Denied()
        {
            return View();
        }
        //Admin page
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddRole()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRole(AccountAddRoleViewModel vm)
        {
            if(ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole(vm.Name);
                Microsoft.AspNetCore.Identity.IdentityResult result = await _roleManagerService.CreateAsync(role);
                
                if(result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach(var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(vm);
        }
    }
}