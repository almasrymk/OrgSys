namespace OrgSys.Controllers
{
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Entities;
    using Domain.Enums;
    using Domain.Shared; 
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    
    [Authorize]
    public class MainController<TDto, TCreate, TUpdate>(IConfiguration configuration, IMapper mapper) : Controller
        where TCreate : ICreateCommand<Domain.Shared.Result>
        where TUpdate : IUpdateCommand<Domain.Shared.Result>
        where TDto : BaseModel
    {
        string AreaName = "";
        string ControllerName = "";

        protected IConfiguration Configuration => configuration;

        protected HttpClient CreateClient() =>
            HttpContext.RequestServices.GetRequiredService<IHttpClientFactory>().CreateClient();

        public virtual async Task<HttpResponseMessage> ApiMethod(ApiMethodType apiMethodType, string NameActionAndParamenter, object Ob = null)
        {
            string ApiUrl = configuration["ApiUrl"];
            HttpClient httpClient = CreateClient();           
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

        public virtual async Task<List<TSubDto>> GetListApi<TSubDto>(string NameActionAndParamenter) where TSubDto : BaseModel
        {
            string ApiUrl = configuration["ApiUrl"];

            var ob = (List<TSubDto>)Activator.CreateInstance(typeof(List<TSubDto>));
            string ApiControllerName = typeof(TSubDto).Name.Replace("ModelView", "").Replace("Dto", "");
            HttpClient httpClient = CreateClient();
            HttpResponseMessage response = await httpClient.GetAsync($"{ApiUrl}/{ApiControllerName}/{NameActionAndParamenter}");
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<ResultCollection<TSubDto>>(data);
            if (res != null)
                ob = res.Response;
            return ob;
        }

        public virtual async Task<List<TSubDto>> GetListApi<TSubDto>(long TypeId = 0, long ParentId = 0, string TextSearch = "", int Page = 1, int PageSize = 25) where TSubDto : BaseModel
        {
            string ApiUrl = configuration["ApiUrl"];

            var ob = (List<TSubDto>)Activator.CreateInstance(typeof(List<TSubDto>));
            string ApiControllerName = typeof(TSubDto).Name.Replace("ModelView", "").Replace("Dto", "");
            HttpClient httpClient = CreateClient();
            HttpResponseMessage response = await httpClient.GetAsync($"{ApiUrl}/{ApiControllerName}/GetList?KeySearch={TextSearch}&TypeId={TypeId}&ParentId={ParentId}&Page={Page}&PageSize={PageSize}");
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<ResultCollection<TSubDto>>(data);
            if (res != null)
                ob = res.Response;
            return ob;
        }

        public virtual async Task<TSubDto> GetObApi<TSubDto>(string NameActionAndParamenter) where TSubDto : BaseModel
        {
            string ApiUrl = configuration["ApiUrl"];

            var ob = (TSubDto)Activator.CreateInstance(typeof(TSubDto));
            string ApiControllerName = typeof(TSubDto).Name.Replace("ModelView", "").Replace("Dto", "");
            HttpClient httpClient = CreateClient();
            HttpResponseMessage response = await httpClient.GetAsync($"{ApiUrl}/{ApiControllerName}/{NameActionAndParamenter}");
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<Result<TSubDto>>(data);
            if (res != null)
                ob = res.Response;
            return ob;
        }

        public virtual async Task<object> GetValueApi<TSubDto>(string NameActionAndParamenter, TSubDto Ob = null) where TSubDto : BaseModel
        {
            string ApiUrl = configuration["ApiUrl"];
            string ApiControllerName = typeof(TSubDto).Name.Replace("ModelView", "").Replace("Dto", "");
            HttpClient httpClient = CreateClient();
            HttpResponseMessage response = await httpClient.GetAsync($"{ApiUrl}/{ApiControllerName}/{NameActionAndParamenter}");
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<object>(data);
            return res;
        }

        public virtual async Task<ActionResult> Index(string search, long ParentId = 0, long TypeId = 0, int page = 1, int pageSize = 10, ResultStatus Status = ResultStatus.nothing, string MsgError = "")
        {
            if ("" + MsgError != "")
                ViewBag.message = MsgError;
            ViewBag.status = Status.ToString();
            ViewBag.pageNumber = page;
            ViewBag.ParentId = ParentId;
            ViewBag.TypeId = TypeId;
            await LoadViewBagIndex(ParentId, TypeId);
            //if (string.IsNullOrEmpty(search))
            //    search = "0";
            var response = await ApiMethod(ApiMethodType.Get, $"Search?KeySearch={search}&ParentId={ParentId}&TypeId={TypeId}&Page={page}&PageSize={pageSize}");
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            var dataList = JsonConvert.DeserializeObject<ResultPagination<TDto>>(data);

            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("List", dataList) : View("Index", dataList);
        }

        public virtual async Task<ActionResult> Save(long id = 0, long ParentId = 0, long TypeId = 0, ResultStatus status = ResultStatus.nothing, string MsgError = "")
        {
            ViewBag.TypeId = TypeId;
            var ob = (TDto)Activator.CreateInstance(typeof(TDto));
          
            var response = await ApiMethod(ApiMethodType.Get, $"GetById?Id={id}");
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<Result<TDto>>(data);
            if (res != null)
                ob = res.Response;

            ob.ParentId = ParentId;
            ob.TypeId = TypeId;
            ob = await InitializeData(ob);
            await LoadViewBag(ob);
            return View(ob);
        }

        [HttpPost]
        public virtual async Task<ActionResult> Save( TDto ob)
        {
            //return null;
            Domain.Shared.Result res = null;
            if (ModelState.IsValid)
            {
                ob = await FixData(ob);
                HttpResponseMessage response = null;
                if (ob.Id == 0)
                {
                    var CreateOb = mapper.Map<TCreate>(ob);
                    response = await ApiMethod(ApiMethodType.Post, $"Create", CreateOb);
                }
                else
                {
                    var UpdateOb = mapper.Map<TUpdate>(ob);
                    response = await ApiMethod(ApiMethodType.Put, $"Update", UpdateOb);
                }

                var data = await response.Content.ReadAsStringAsync();
                res = JsonConvert.DeserializeObject<Domain.Shared.Result>(data);

                if (response.IsSuccessStatusCode)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        return Ok(new { status = "success", id = ob.Id, url = "/" + AreaName + "/" + ControllerName + "?ParentId=" + ob.ParentId + "&TypeId=" + ob.TypeId + "&status=" + ResultStatus.success + "&MsgError=Success" });
                    return Redirect("/" + AreaName + "/" + ControllerName + "?ParentId=" + ob.ParentId + "&TypeId=" + ob.TypeId + "&status=" + ResultStatus.success + "&MsgError=Success");
                }
            }

            await LoadViewBag(ob);
            if (res != null)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return BadRequest(new { res.Errors });

                foreach (var item in res.Errors)
                    ModelState.AddModelError(item.Key, item.MessageError);
            }
            return View(ob);
        }

        public virtual async Task<Result> Delete(long id)
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
                        return new Result(HttpStatusCode.OK , null);

                }
            }
            catch (Exception ex)
            {
                return new Result(HttpStatusCode.InternalServerError, new List<Error> { new Error(ex.Message, "") });
            }
            return new Result(HttpStatusCode.BadRequest, new List<Error> { new Error("Error", "") } );
        }

        [HttpPost]
        public virtual async Task<Result> DeleteList(long[] ids, long ParentId = 0, long TypeId = 0)
        {
            try
            {
                if (ids != null && ids.Length > 0)
                {
                    var query = string.Join("&", ids.Select(i => $"ids={i}"));
                    var response = await ApiMethod(ApiMethodType.Delete, $"DeleteList?{query}");

                    if (response.IsSuccessStatusCode)
                        return new Domain.Shared.Result(HttpStatusCode.OK , null);
                }
            }
            catch (Exception ex)
            {
                return new Domain.Shared.Result(HttpStatusCode.InternalServerError, new List<Error> { new Error (ex.Message , "")  } );
            }
            return new Domain.Shared.Result(HttpStatusCode.BadRequest, new List<Error> { new Error("Error", "") } );
        }

        public virtual async Task LoadViewBag(TDto model)
        {

        }

        public virtual async Task LoadViewBagIndex(long ParentId = 0, long TypeId = 0)
        {

        }

        public virtual async Task<TDto> InitializeData(TDto ob)
        {
            return ob;
        }

        public virtual async Task<TDto> FixData(TDto ob)
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