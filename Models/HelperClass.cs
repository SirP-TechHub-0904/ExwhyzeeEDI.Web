using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ExwhyzeeEDI.Web.Helper;
using Microsoft.EntityFrameworkCore;

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
                if (cent != null)
                {
                    response = cent.SchoolName;
                }
                return response;
            }
        }

        public async static Task<string> SendSMS(string messages, string phonenumber)
        {
            messages = messages.Replace("0", "O");
            string response = "";
            try
            {
                string username = EnvLoader.Get("SMS_USERNAME", "");
                string password = EnvLoader.Get("SMS_PASSWORD", "");
                string sender = EnvLoader.Get("SMS_SENDER", "EDI");

                string encodedPhone = WebUtility.UrlEncode(phonenumber);
                string encodedMsg = WebUtility.UrlEncode(messages);

                string apiUrl = $"http://account.kudisms.net/api/?username={username}&password={password}&message={encodedMsg}&sender={sender}&mobiles={encodedPhone}";

                using (var httpClient = new System.Net.Http.HttpClient())
                {
                    response = await httpClient.GetStringAsync(apiUrl);
                }
            }
            catch (Exception)
            {
                // Silently catch network or sms errors
            }

            return response;
        }

        public async static Task<string> SendMail(string mailaddress, string message)
        {
            string response = "";
            try
            {
                string host = EnvLoader.Get("SMTP_HOST", "mail.edi.exwhyzee.ng");
                int port = int.TryParse(EnvLoader.Get("SMTP_PORT", "587"), out int p) ? p : 587;
                string user = EnvLoader.Get("SMTP_USER", "noreply@edi.exwhyzee.ng");
                string pass = EnvLoader.Get("SMTP_PASS", "");
                string from = EnvLoader.Get("SMTP_FROM", user);

                string templatePath = AppPaths.MapPath("~/Views/Data/Success.html");
                string templateContent = "";
                if (File.Exists(templatePath))
                {
                    templateContent = await File.ReadAllTextAsync(templatePath);
                }

                // Replacements
                string body = templateContent
                    .Replace("{bHead}", "EXWHYZEE ENTREPRENEURSHIP DEVELOPMENT INSTITUTE")
                    .Replace("{bbody}", "MISSION: By the year 2025, and beyond. We must have created atleast 1000 entrepreneurs in the South East and Nation atlarge who are self sustained, This we intend to do by providing world class entrepreneurship training, mentorship, providing access to continous funding and building global market linkage.")
                    .Replace("{doyou}", "New Participant")
                    .Replace("{doyoubody}", "The Easternpreneurs is a forum and platform put together by project consultants, portfolio business managers, commercial law consultants and investment consultants to provide a network for entrepreneurs, commercial and business agencies and companies for interaction, business roundtable, investors exchange platform and development of international trade and business connections.")
                    .Replace("{activate}", message)
                    .Replace("{smalltitle}", "Benefits of E.D.I");

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from);
                mail.To.Add(mailaddress);
                mail.Subject = "Welcome to EXWHYZEE EDI";
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient(host, port))
                {
                    smtp.Credentials = new NetworkCredential(user, pass);
                    smtp.EnableSsl = false; // Adjust based on host provider configuration
                    await smtp.SendMailAsync(mail);
                }

                response = "OK";
            }
            catch (Exception)
            {
                // Silently catch email sending errors to avoid blocking user registration
            }

            return response;
        }

        public static bool checkPayment(int id, string userid)
        {
            using (var db = new ApplicationDbContext())
            {
                var sds = db.UserPrograms.FirstOrDefault(x => x.Id == id && x.UserId == userid);
                if (sds == null) return false;
                return sds.PaymentStatus == Models.Entities.Enum.UserProgramPaymentStatus.Successful;
            }
        }

        public static string GetfullName(string userid)
        {
            using (var db = new ApplicationDbContext())
            {
                var sds = db.Profiles.FirstOrDefault(x => x.UserId == userid);
                if (sds == null) return "";
                return sds.Surname + " " + sds.FirstName + " " + sds.OtherName;
            }
        }

        public static string GetSchool(int? id)
        {
            using (var db = new ApplicationDbContext())
            {
                var sds = db.Schools.FirstOrDefault(x => x.Id == id);
                if (sds == null) return "";
                return sds.SchoolName;
            }
        }

        public static string ResolveImageUrl(string? imagePath, string defaultFolder = "Uploads")
        {
            if (string.IsNullOrWhiteSpace(imagePath)) return "";
            if (imagePath.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || imagePath.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                return imagePath;
            }

            var cloudBase = EnvLoader.Get("CLOUDINARY_BASE_URL", "");
            if (!string.IsNullOrEmpty(cloudBase))
            {
                var cleanBase = cloudBase.TrimEnd('/') + "/";
                var cleanFolder = defaultFolder.Trim('/');
                var cleanFile = imagePath.TrimStart('/');
                return $"{cleanBase}{cleanFolder}/{cleanFile}";
            }

            return $"/{defaultFolder.Trim('/')}/{imagePath.TrimStart('/')}";
        }
    }
}