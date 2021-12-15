using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Utility
{
    public static class Extensions
    {
        //public static t Map<t>(this BaseModel ob) where t : BaseModel
        //{
        //    Assembly assembly = Assembly.Load("Entity");
        //    var type = assembly.GetType("Entity.MapperConfig");
        //    var profile = (Profile) Activator.CreateInstance(type);
        //    var config = type.GetProperty("config");
        //    MapperConfiguration mapperConfiguration = (MapperConfiguration) config.GetValue(profile);          
        //    var mapper = mapperConfiguration.CreateMapper();
        //    //var mapper = new MapperConfiguration(cfg => cfg.AddProfile(profile)).CreateMapper();           
        //    return mapper.Map<t>(ob);

        //    //Assembly assembly = Assembly.Load("AutoMapper");                                      
        //    //var type = assembly.GetType(typeof(Mapper).FullName);            
        //    //var mapper = Activator.CreateInstance<Mapper>();
        //    //return mapper.Map<t>(ob);
        //}
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

            AlternateView view = AlternateView.CreateAlternateViewFromString(Body, Encoding.UTF8, MediaTypeNames.Text.Html);
            LinkedResource resource = new LinkedResource(Path.GetFullPath("wwwroot/logos/Logo.png") , "image/png");
            resource.ContentId = "MyLogo";
            resource.ContentType.MediaType = MediaTypeNames.Image.Jpeg;
            resource.TransferEncoding = TransferEncoding.QuotedPrintable;
            resource.ContentType.Name = "Logo";
            resource.ContentLink = new Uri("cid:MyLogo");            
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