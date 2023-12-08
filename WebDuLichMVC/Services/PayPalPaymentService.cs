using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PayPal.Api;

namespace WebDuLichMVC.Services
{
    public class PayPalPaymentService
    {
        public static Payment CreatePayment(string baseUrl, string intent)
        {
            // Pass in a `APIContext` object to authenticate 
            // the call and to send a unique request id 
            var apiContext = PayPalConfiguration.GetAPIContext();

            var payment = new Payment()
            {
                intent = intent, // sale or authorize
                payer = new Payer() {payment_method = "paypal"},
                transactions = GetTransactionsList(),
                redirect_urls = GetReturnUrls(baseUrl, intent)
            };
            var createPayment = payment.Create(apiContext);
            return createPayment;
        }
        private static List<Transaction> GetTransactionsList()
        {
            // A transaction defines the contract of a payment
            // what is the payment for and who is fulfilling it. 
            var transactionList = new List<Transaction>();

            // Create API requires a list of transaction
            transactionList.Add(new Transaction() 
            {
                description = "Book a Phu Quoc tour",    
                invoice_number = GetRandomInvoiceNumber(),
                amount = new Amount()
                {
                    currency = "VND",
                    total = "2.000.000",
                    details = new Details()
                    {
                        tax = "200.000",
                        shipping = "400.000",
                        subtotal = "1.400.000"
                    }
                },
                item_list = new ItemList()
                {
                    items = new List<Item>()
                    {
                        new Item()
                        {
                            name = "Tour",
                            currency = "VND",
                            price = "280.000",
                            quantity = "5",
                            sku = "sku"
                        }
                    }
                }
            });
            return transactionList;
        }
        private static RedirectUrls GetReturnUrls(string baseUrl, string intent)
        {
            var returnUrls = intent == "sale" ? "/Home/PaymentSuccessful" : "/Home/AuthorizeSuccessful";

            // These URLs will determine how the user is redirected from PayPal 
            return new RedirectUrls()
            {
                cancel_url = baseUrl + "/Home/PaymentCancelled",
                return_url = baseUrl + returnUrls
            };
        }
        public static Payment ExecutePayment(string paymentId, string payerId) 
        {
            // truyền mã định danh vào
            var apiContext = PayPalConfiguration.GetAPIContext();
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            var payment = new Payment() 
            {
                id = paymentId
            };

            // Execute payment
            var executedPayment = payment.Execute(apiContext,paymentExecution);
            return executedPayment;
        }
        public static Capture CapturePayment(string paymentId) {
            // truyền mã định danh vào
            var apiContext = PayPalConfiguration.GetAPIContext();
            var payment = Payment.Get(apiContext, paymentId);
            var auth = payment.transactions[0].related_resources[0].authorization;

            var capture = new Capture()
            {
                amount = new Amount()
                {
                    currency = "VND",
                    // total = payment.transactions[0].amount.total
                    total = "100.000"
                },
                is_final_capture = true
            };
            var responseCapture = auth.Capture(apiContext, capture);
            return responseCapture;
        }

        public static string GetRandomInvoiceNumber()
        {
            return new Random().Next(999999).ToString();
        }
    }
}