using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Utility
{
    public static class Extensions
    {

    }

    public static class General
    {
        public static HttpContext HttpContext { get; set; }
        public static void SetSchema(string Schema)
        {
            HttpContext.Session.Set("Schema", Encoding.ASCII.GetBytes(Schema));
        }

        public static void SetConfiguration(IConfiguration configuration)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, configuration);
            HttpContext.Session.Set("Configuration", ms.ToArray());
        }

        public static string GetSchema()
        {
            if (HttpContext == null || HttpContext.Session == null)
                return "";
            byte[] obs = null;
            HttpContext.Session.TryGetValue("Schema", out obs);
            if (obs == null)
                return "";
            return Encoding.ASCII.GetString(obs);
        }

        public static IConfiguration GetConfiguration()
        {
            if (HttpContext == null || HttpContext.Session == null)
                return null;
            byte[] obs = null;
            HttpContext.Session.TryGetValue("Configuration", out obs);
            if (obs == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(obs);
            object obj = bf.Deserialize(ms);
            return (IConfiguration)obj;
        }
    }
}