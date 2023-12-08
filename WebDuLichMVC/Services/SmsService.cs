using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WebDuLichMVC.Services
{
    public class SmsService
    {
        public static string SendSMSGet() {
            String result;
            string apiKey = "xxx";
            string numbers = "+61xx";
            string message = "Thank for creating an account with Vietnam Travel";
            string sender = "Ho Duc Vu";
            String url = "https://api.txtlocal.com/send/?apikey=" + apiKey + "&numbers=" + numbers + "&message=" + message + "&sender=" + sender;

            StreamWriter myWriter = null;
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);

            objRequest.Method = "POST";
            objRequest.ContentLength = Encoding.UTF8.GetByteCount(url);
            // coding msg: key1=value1&key2=value2&key3=value3
            objRequest.ContentType =  "application/x-www-form-urlencoded";
            try
            {
                myWriter = new StreamWriter(objRequest.GetRequestStream());
                myWriter.Write(url);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                myWriter.Close();
            }
            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                sr.Close();
            }
            return result;
        }

        public static string SendSMSPost() {
            String msg = HttpUtility.UrlEncode("THank for creating an account with Vietnam Travel");
            using (var wb = new WebClient())
            {
                byte[] response = wb.UploadValues("https://api.txtlocal.com/send/", new System.Collections.Specialized.NameValueCollection() {
                {"apikey" , "xxx"},
                {"numbers" , "+61xxx"},
                {"message" , msg},
                {"sender" , "Ho Duc Vu"}
                });
                string result = Encoding.UTF8.GetString(response);
                return result;
            }
        }
    }
}