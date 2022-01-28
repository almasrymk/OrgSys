using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Web.Http;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Utility
{
    public class Result : IHttpActionResult
    {
        public Result(HttpStatusCode StatusCode = HttpStatusCode.OK, string Message = "")
        {
            this.StatusCode = StatusCode;
            this.Status = StatusCode == HttpStatusCode.OK;
            this.ErrorMessage = Message;
        }

        public Result(object _Data)
        {
            this.Data = _Data;
            this.StatusCode = HttpStatusCode.OK;
            this.Status = true;
            this.ErrorMessage = "";
        }

        private HttpStatusCode StatusCode { get; set; }
        public bool Status { get; set; }
        public string ErrorMessage { get; set; }
        public object Data { get; set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(StatusCode);            
            response.Content = new  StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");
            return Task.FromResult(response);
        }
    }
}