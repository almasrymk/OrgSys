using Domain.Enums;
using Domain.Shared;
using Entity;
using iTextSharp.text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting.Internal;
using Newtonsoft.Json;
using Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace OrgSys.Controllers
{
    [Authorize]
    public class MainController<TDto>(IConfiguration configuration) : Controller where TDto : BaseModel
    {
        string AreaName = "";
        string ControllerName = "";

        public virtual async Task<HttpResponseMessage> ApiMethod(ApiMethodType apiMethodType, string NameActionAndParamenter, TDto Ob = null)
        {
            string ApiUrl = configuration["ApiUrl"];
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response;
            string ApiControllerName = typeof(TDto).Name.Replace("ModelView", "").Replace("Dto", "");
            switch (apiMethodType)
            {
                case ApiMethodType.Get:
                    return await httpClient.GetAsync($"{ApiUrl}/{ApiControllerName}/{NameActionAndParamenter}");
                case ApiMethodType.Post:
                    return await httpClient.PostAsJsonAsync($"{ApiUrl}/{ApiControllerName}/{NameActionAndParamenter}", Ob);
                case ApiMethodType.Put:
                    return await httpClient.PutAsJsonAsync($"{ApiUrl}/{ApiControllerName}/{NameActionAndParamenter}", Ob);
                case ApiMethodType.Delete:
                    return await httpClient.DeleteAsync($"{ApiUrl}/{ApiControllerName}/{NameActionAndParamenter}");
                default:
                    break;
            }
            return null;
        }


        [HttpGet]
        public virtual async Task<ActionResult> Index(string search, long ParentId = 0, long TypeId = 0, int page = 1, int pageSize = 10, ResultStatus Status = ResultStatus.nothing, string MsgError = "")
        {
            if ("" + MsgError != "")
                ViewBag.message = MsgError;
            ViewBag.status = Status.ToString();
            ViewBag.pageNumber = page;
            ViewBag.ParentId = ParentId;
            ViewBag.TypeId = TypeId;
            LoadViewBagIndex(ParentId, TypeId);
            //if (string.IsNullOrEmpty(search))
            //    search = "0";
            var response = await ApiMethod(ApiMethodType.Get, $"Search?KeySearch={search}&Page={page}&PageSize={pageSize}");
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            var dataList = JsonConvert.DeserializeObject<ResultPagination<TDto>>(data);

            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("List", dataList) : View("Index", dataList);
        }

        [HttpGet]
        public virtual async Task<ActionResult> Save(long id = 0, long ParentId = 0, long TypeId = 0, ResultStatus status = ResultStatus.nothing, string MsgError = "")
        {
            ViewBag.TypeId = TypeId;
            var ob = (TDto)Activator.CreateInstance(typeof(TDto));
            ob.ParentId = ParentId;
            ob.TypeId = TypeId;

            if (id > 0)
            {
                var response = await ApiMethod(ApiMethodType.Get, $"GetById?Id={id}");
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<Result<TDto>>(data);
                if (res != null)
                    ob = res.Response;
            }

            ob = InitializeData(ob);
            LoadViewBag(ob);
            return View(ob);
        }

        [HttpPost]
        public virtual async Task<ActionResult> Save(TDto ob)
        {
            if (ModelState.IsValid)
            {
               HttpResponseMessage response = null;
                if (ob.Id == 0)
                    response = await ApiMethod(ApiMethodType.Post, $"Create", ob);
                else
                    response = await ApiMethod(ApiMethodType.Put, $"Update", ob);

                if (response.IsSuccessStatusCode)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        return Ok(new { status = "success", id = ob.Id, url = "/" + AreaName + "/" + ControllerName + "?ParentId=" + ob.ParentId + "&TypeId=" + ob.TypeId + "&status=" + ResultStatus.success + "&MsgError=Success" });
                    return Redirect("/" + AreaName + "/" + ControllerName + "?ParentId=" + ob.ParentId + "&TypeId=" + ob.TypeId + "&status=" + ResultStatus.success + "&MsgError=Success");
                }
            }
            LoadViewBag(ob);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return BadRequest("Error");
            return View(ob);
        }

        [HttpGet]
        public virtual async Task<Utility.Result> Delete(long id)
        {            
            var ob = await ApiMethod(ApiMethodType.Get, $"GetById?Id={id}");
            ob.EnsureSuccessStatusCode();
            var data = await ob.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<Result<TDto>>(data);
            try
            {
                if (res.Response != null && res.Response.Id > 0)
                {
                    var response = await ApiMethod(ApiMethodType.Delete, $"Delete?Id={id}");

                    if (response.IsSuccessStatusCode)
                        return new Utility.Result();

                }
            }
            catch (Exception ex)
            {
                return new Utility.Result(HttpStatusCode.InternalServerError, ex.Message);
            }
            return new Utility.Result(HttpStatusCode.BadRequest, "Error");
        }

        [HttpPost]
        public virtual async Task<Utility.Result> DeleteList(long[] ids, long ParentId = 0, long TypeId = 0)
        {
            try
            {
                if (ids != null && ids.Length > 0)
                {                    
                    var query = string.Join("&", ids.Select(i => $"ids={i}"));
                    var response = await ApiMethod(ApiMethodType.Delete, $"DeleteList?{query}");

                    if (response.IsSuccessStatusCode)
                        return new Utility.Result();
                }
            }
            catch (Exception ex)
            {
                return new Utility.Result(HttpStatusCode.InternalServerError, ex.Message);
            }
            return new Utility.Result(HttpStatusCode.BadRequest, "Error");
        }

        public virtual void LoadViewBag(TDto model)
        {

        }

        public virtual void LoadViewBagIndex(long ParentId = 0, long TypeId = 0)
        {

        }

        public virtual TDto InitializeData(TDto ob)
        {
            return ob;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            AreaName = "" + context.RouteData.Values["area"];
            ControllerName = ControllerContext.ActionDescriptor.ControllerName;
            ViewBag.Page = "/" + context.RouteData.Values["area"] + "/" + ControllerContext.ActionDescriptor.ControllerName;
            ViewBag.area = AreaName;
            ViewBag.PageTitle = ControllerContext.ActionDescriptor.ControllerName;
            base.OnActionExecuting(context);
        }
    }
}