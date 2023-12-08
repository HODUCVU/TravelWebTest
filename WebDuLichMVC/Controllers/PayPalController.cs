using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebDuLichMVC.Services;

namespace WebDuLichMVC.Controllers
{
    public class PayPalController : Controller
    {  
        public IActionResult CreatePayment()
        {
            var payment = PayPalPaymentService.CreatePayment("string", "sale");
            return Redirect(payment.GetApprovalUrl());
        }
        public IActionResult PaymentCancelled()
        {
            return RedirectToAction("Error");
        }
        public IActionResult PaymentSuccessful(string paymentId, string token, string PayerID)
        {
            var payment = PayPalPaymentService.ExecutePayment(paymentId, PayerID);
            return View();
        }
    }
}