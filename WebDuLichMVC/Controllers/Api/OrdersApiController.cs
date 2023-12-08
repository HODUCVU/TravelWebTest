using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PayPal.Api;

namespace WebDuLichMVC.Controllers.Api
{
    [RoutePrefix("api/Orders")]
    public class OrdersController : ApiController
    {
       [Authorize]
       [System.Web.Http.Route("")]
       public IHttpActionResult Get()
       {
            return Ok(Order.CreateOrders());
       }
    }
    #region Helpers
    public class Order
    {
        public int OrderID { get; set; }
        public string CustomerName { get; set; }
        public string ShipperCity { get; set; }
        public Boolean IsShipped { get; set; }
        public static List<Order> CreateOrders()
        {
            List<Order> OrderList = new List<Order>
            {
                new Order {OrderID = 10248, CustomerName = "Nguyen Van Nam", ShipperCity = "Da Nang", IsShipped = true },
                new Order {OrderID = 10249, CustomerName = "Hoang Minh Tuan", ShipperCity = "Ha Noi", IsShipped = false},
                new Order {OrderID = 10250,CustomerName = "Nguyen Cong Tuan", ShipperCity = "Ho Chi Minh", IsShipped = false },
                new Order {OrderID = 10251,CustomerName = "Tran Chi Phuong", ShipperCity = "Hue", IsShipped = false},
                new Order {OrderID = 10252,CustomerName = "Hoang Anh Gia Lai", ShipperCity = "Da Lat", IsShipped = true}
            };
            return OrderList;
        }
    }
    #endregion
}