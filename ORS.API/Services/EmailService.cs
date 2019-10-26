using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

public class EmailService
{
    #region Sending Mails
    public static void SendMail(string code, string email, HttpPostedFileBase fileuploader = null)
    {
        string filebody = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Templates/") + "confirmation" + ".cshtml");
        filebody = filebody.Replace("$$name$$", email);
        filebody = filebody.Replace("$$code$$", code);
        LinkedResource logo = new LinkedResource(HttpContext.Current.Server.MapPath("~/Templates/") + "logo" + ".png");
        logo.ContentId = "companylogo";
        AlternateView avHtml = AlternateView.CreateAlternateViewFromString(filebody, null, MediaTypeNames.Text.Html);
        avHtml.LinkedResources.Add(logo);
        using (MailMessage mail = new MailMessage(System.Configuration.ConfigurationManager.AppSettings["UserID"], email))
        {
            mail.AlternateViews.Add(avHtml);
            mail.Subject = "Please Confirm Your Account";
            mail.Body = "Test attachment";
            mail.Body = filebody;

            if (fileuploader != null)
            {
                string filename = Path.GetFileName(fileuploader.FileName);
                mail.Attachments.Add(new Attachment(fileuploader.InputStream, filename));
            }
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(
                System.Configuration.ConfigurationManager.AppSettings["UserID"],
                System.Configuration.ConfigurationManager.AppSettings["Password"]
                );
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }


    }
    public static void SendResetPasswordMail(string code, string tomail, HttpPostedFileBase fileuploader = null)
    {
        string filebody = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Templates/") + "resetpassword" + ".cshtml");
        filebody = filebody.Replace("$$name$$", tomail);
        //filebody = filebody.Replace("$$name$$", "Waseem Ahmad");
        filebody = filebody.Replace("$$code$$", code);
        LinkedResource logo = new LinkedResource(HttpContext.Current.Server.MapPath("~/Templates/") + "logo" + ".png");
        logo.ContentId = "companylogo";
        AlternateView avHtml = AlternateView.CreateAlternateViewFromString(filebody, null, MediaTypeNames.Text.Html);
        avHtml.LinkedResources.Add(logo);
        using (MailMessage mail = new MailMessage(System.Configuration.ConfigurationManager.AppSettings["UserID"], tomail))
        {
            mail.AlternateViews.Add(avHtml);
            mail.Subject = "Password Reset";
            mail.Body = "Test attachment";
            mail.Body = filebody;

            if (fileuploader != null)
            {
                string filename = Path.GetFileName(fileuploader.FileName);
                mail.Attachments.Add(new Attachment(fileuploader.InputStream, filename));
            }
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(
                System.Configuration.ConfigurationManager.AppSettings["UserID"],
                System.Configuration.ConfigurationManager.AppSettings["Password"]
                );
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }


    }
    public static void SendTempraryPasswordMail(string tomail, string password, HttpPostedFileBase fileuploader = null)
    {
        string filebody = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Templates/") + "temprarypassword" + ".cshtml");
        filebody = filebody.Replace("$$name$$", tomail);
        //filebody = filebody.Replace("$$name$$", "Waseem Ahmad");
        filebody = filebody.Replace("$$password$$", password);
        LinkedResource logo = new LinkedResource(HttpContext.Current.Server.MapPath("~/Templates/") + "logo" + ".png");
        logo.ContentId = "companylogo";
        AlternateView avHtml = AlternateView.CreateAlternateViewFromString(filebody, null, MediaTypeNames.Text.Html);
        avHtml.LinkedResources.Add(logo);
        using (MailMessage mail = new MailMessage(System.Configuration.ConfigurationManager.AppSettings["UserID"], tomail))
        {
            mail.AlternateViews.Add(avHtml);
            mail.Subject = "Temprary Password Generated";
            mail.Body = "Test attachment";
            mail.Body = filebody;

            if (fileuploader != null)
            {
                string filename = Path.GetFileName(fileuploader.FileName);
                mail.Attachments.Add(new Attachment(fileuploader.InputStream, filename));
            }
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(
                System.Configuration.ConfigurationManager.AppSettings["UserID"],
                System.Configuration.ConfigurationManager.AppSettings["Password"]
                );
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }


    }
    #endregion
}