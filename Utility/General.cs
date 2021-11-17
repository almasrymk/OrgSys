using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Utility
{
    public static class Extensions
    {

    }

    public static class General
    {
        //public static HttpContext HttpContext { get; set; }
        public static string _Schema { get; set; }

        public static ISession _session { get; set; }

        public static void SetSchema(this ISession session, string value)
        {
            _session = session;
            session.Set("Schema", JsonSerializer.SerializeToUtf8Bytes(value));
        }

        public static string GetSchema()
        {
            if (_session != null)
            {
                _session.TryGetValue("Schema", out byte[] value);
                return value == null ? "" : JsonSerializer.Deserialize<string>(value);
            }
            return "";
        }

        //public static void SetSchema(string Schema)
        //{
        //    _Schema = Schema;
        //    //HttpContext.Session.Remove("Schema");
        //    //HttpContext.Session.Set("Schema", Encoding.ASCII.GetBytes(Schema));
        //}

        //public static void SetConfiguration(IConfiguration configuration)
        //{
        //    BinaryFormatter bf = new BinaryFormatter();
        //    MemoryStream ms = new MemoryStream();
        //    bf.Serialize(ms, configuration);
        //    HttpContext.Session.Set("Configuration", ms.ToArray());
        //}

        //public static string GetSchema()
        //{
        //    //if (HttpContext == null || HttpContext.Session == null)
        //    //    return "";
        //    //byte[] obs = null;
        //    //HttpContext.Session.TryGetValue("Schema", out obs);
        //    //if (obs == null)
        //    //    return "";
        //    //return Encoding.ASCII.GetString(obs);
        //    return _Schema;
        //}

        //public static IConfiguration GetConfiguration()
        //{
        //    if (HttpContext == null || HttpContext.Session == null)
        //        return null;
        //    byte[] obs = null;
        //    HttpContext.Session.TryGetValue("Configuration", out obs);
        //    if (obs == null)
        //        return null;
        //    BinaryFormatter bf = new BinaryFormatter();
        //    MemoryStream ms = new MemoryStream(obs);
        //    object obj = bf.Deserialize(ms);
        //    return (IConfiguration)obj;
        //}

        public static void SendEmail(string Email , string Sender, string Subject, string Body )
        {
            SmtpClient client = new SmtpClient();
            client.Host = "mail.organizersys.com";
            client.Port = 8889;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("info@organizersys.com", "Testg@83");
            client.EnableSsl = false;
            client.Timeout = 50000;
          
            MailMessage mailMessage = null;           
            mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("info@organizersys.com", Sender);
            mailMessage.To.Add(new MailAddress(Email, Sender));
            //Attachment oAttachment = new Attachment(@"wwwroot/logos/Logo.png");
            //oAttachment.ContentId = "MyImage";
            //mailMessage.Attachments.Add(oAttachment);

            AlternateView view = AlternateView.CreateAlternateViewFromString(Body , null, MediaTypeNames.Text.Html);
            LinkedResource resource = new LinkedResource(Path.GetFullPath("wwwroot/logos/Logo.png"));
            resource.ContentId = "Image1";
            view.LinkedResources.Add(resource);
            mailMessage.AlternateViews.Add(view);
            //mailMessage.Body = Body;
            mailMessage.Subject = Subject;
            mailMessage.IsBodyHtml = true;            
            client.Send(mailMessage);
        }

        public static async Task<string> RenderViewAsync<TModel>(Controller controller, string viewName, TModel model, bool partial = false)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = controller.ControllerContext.ActionDescriptor.ActionName;
            }

            controller.ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                ViewEngineResult viewResult = viewEngine.FindView(controller.ControllerContext, viewName, !partial);

                if (viewResult.Success == false)
                {
                    return $"A view with the name {viewName} could not be found";
                }

                ViewContext viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }
    }
}