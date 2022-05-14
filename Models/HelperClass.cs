using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;

namespace ExwhyzeeEDI.Web.Models
{
    public static class HelperClass
    {

      

        public static string CentreName(int? id)
        {
           
            string response = "";
            using (var db = new ApplicationDbContext())
            {
                var cent = db.Schools.FirstOrDefault(x => x.Id == id);
                if(cent != null)
                {
                    response = cent.SchoolName;
                }

                // response = "ok";
                return response;
            }
        }


        public async static Task<string> SendSMS(string messages, string phonenumber)
        {
            messages = messages.Replace("0", "O");
            string response = "";
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    var getApi = "http://account.kudisms.net/api/?username=ponwuka123@gmail.com&password=sms@123&message=@@message@@&sender=@@sender@@&mobiles=@@recipient@@";
                    string apiSending = getApi.Replace("@@sender@@", "E D I").Replace("@@recipient@@", HttpUtility.UrlEncode(phonenumber)).Replace("@@message@@", HttpUtility.UrlEncode(messages));

                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(apiSending);
                    httpWebRequest.Method = "GET";
                    httpWebRequest.ContentType = "application/json";

                    //getting the respounce from the request
                    HttpWebResponse httpWebResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync();
                    Stream responseStream = httpWebResponse.GetResponseStream();
                    StreamReader streamReader = new StreamReader(responseStream);
                    response = await streamReader.ReadToEndAsync();

                }
                catch (Exception c)
                {

                }

                if (response.ToUpper().Contains("OK") || response.ToUpper().Contains("1701"))
                {

                }

                // response = "ok";
                return response;
            }
        }

        //mail

        public async static Task<string> SendMail(string mailaddress, string message)
        {
           
            string response = "";
            using (var db = new ApplicationDbContext())
            {
                try
                {
                  
                    MailMessage mail = new MailMessage();

                    //set the addresses 
                    mail.From = new MailAddress("noreply@edi.exwhyzee.ng"); //IMPORTANT: This must be same as your smtp authentication address.

                    //
                    //set the content Server.MapPath("~/status.txt")
                    string AppPath = HttpContext.Current.Request.PhysicalApplicationPath;
                    //StreamReader sr = new StreamReader(AppPath + "../Views/Account/HtmlPage1.html");
                    StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath("~/Views/Data/Success.html"));

                    mail.Body = sr.ReadToEnd();
                    sr.Close();

                    MailDefinition md = new MailDefinition();
                    md.From = "noreply@edi.exwhyzee.ng";
                    md.IsBodyHtml = true;
                    md.Subject = "Welcome to EXWHYZEE EDI";

                    //
                    ListDictionary replacements = new ListDictionary();
                    ///
                    replacements.Add("{bHead}", "EXWHYZEE ENTREPRENEURSHIP DEVELOPMENT INSTITUTE");
                    replacements.Add("{bbody}", "MISSION: By the year 2025, and beyond. We must have created atleast 1000 entrepreneurs in the South East and Nation atlarge who are self sustained, This we intend to do by providing world class entrepreneurship training, mentorship, providing access to continous funding and building global market linkage.");
                    replacements.Add("{doyou}", "New Participant");
                    //replacements.Add("{subjectt}", "Welcome");
                    replacements.Add("{doyoubody}", "The Easternpreneurs is a forum and platform put together by project consultants, portfolio business managers, commercial law consultants and investment consultants to provide a network for entrepreneurs, commercial and business agencies and companies for interaction, business roundtable, investors exchange platform and development of international trade and business connections.");
                    ///

                    replacements.Add("{activate}", message);
                    replacements.Add("{smalltitle}", "Benefits of E.D.I");

                    mail = md.CreateMailMessage(mailaddress, replacements, mail.Body, new System.Web.UI.Control());



                    //send the message 
                    SmtpClient smtp = new SmtpClient("mail.edi.exwhyzee.ng");

                    //IMPORANT:  Your smtp login email MUST be same as your FROM address. 
                    NetworkCredential Credentials = new NetworkCredential("noreply@edi.exwhyzee.ng", "ahambuPeter@123");
                    smtp.Credentials = Credentials;
                    smtp.Send(mail);
                }
                catch (Exception ex)
                {

                    //TempData["mssg"] = "Mail not Sent. Try Again.";
                }
                // response = "ok";
                return response;
            }
        }


       
    }
}