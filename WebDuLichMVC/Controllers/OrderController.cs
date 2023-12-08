using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebDuLichMVC.Models;
using WebDuLichMVC.Services;
using WebDuLichMVC.ViewModels;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace WebDuLichMVC.Controllers
{
    public class OrderController : Controller
    {
        private Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _userManagerService;
        private IDataService<Package> _packageDataService;
        private IDataService<Order> _orderDataService;
        public OrderController(Microsoft.AspNetCore.Identity.UserManager<IdentityUser> UserManager,
                            IDataService<Package> packageService,
                            IDataService<Order> orderService)
        {
            _userManagerService = UserManager;
            _packageDataService = packageService;
            _orderDataService = orderService;
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        public IActionResult Create()
        {
            int packageId = int.Parse(TempData["packageId"].ToString());
            Package package = _packageDataService.GetSignle(p => p.PackageId == packageId);
            OrderCreateViewModel vm = new OrderCreateViewModel {
                PackageId = package.PackageId,
                Name = package.Name,
                Quantity = 1,
                Location = package.Location,
                Price = package.Price,
                Description = package.Description,
                Picture = package.Picture
            };
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Create(OrderCreateViewModel vm)
        {
            IdentityUser user = await _userManagerService.FindByNameAsync(User.Identity.Name);
            if(ModelState.IsValid)
            {
                Order order = new Order{
                    Date = DateTime.Now,
                    PackageName = vm.Name,
                    PackageId = vm.PackageId,
                    Quantity = vm.Quantity,
                    TotalPrice = vm.Quantity * vm.Price,
                    UserId = user.Id
                };
                _orderDataService.Create(order);
                
                return RedirectToAction("Details", "Order", new{id = order.OrderId});
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        public IActionResult Details(int id)
        {
            Order order = _orderDataService.GetSignle(o => o.OrderId == id);
            OrderDetailsViewModel vm = new OrderDetailsViewModel {
                OrderId = order.OrderId,
                PackageName = order.PackageName,
                Quantity = order.Quantity,
                TotalPrice = order.TotalPrice,
                Date = order.Date
            };
            return View(vm);
        }
        [HttpGet]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> DetailsPast()
        {
            IdentityUser user = await _userManagerService.FindByNameAsync(User.Identity.Name);
            IEnumerable<Order> orders = _orderDataService.Query(o => o.UserId == user.Id);

            OrderDetailsPastViewModel vm = new OrderDetailsPastViewModel {
                Orders = orders,
                Count = orders.Count()
            };
            return View(vm);
        }
    }
}