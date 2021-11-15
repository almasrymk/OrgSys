using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;

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

        public static void SendEmail(string Email , string Sender, string Subject, string Body)
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
            mailMessage.To.Add(Email);
            Attachment oAttachment = new Attachment(@"wwwroot/logos/Logo.png");
            oAttachment.ContentId = "imgId";
            mailMessage.Attachments.Add(oAttachment);
            mailMessage.Body = Body;
            mailMessage.Subject = Subject;
            mailMessage.IsBodyHtml = true;
            client.Send(mailMessage);
        }
    }
}