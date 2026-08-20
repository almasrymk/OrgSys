#nullable enable annotations
using Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Http;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class HomeBaseController(IHttpClientFactory httpClientFactory) : Controller
{
    private readonly string LocalHost = "https://localhost:44300/";

    protected HttpClient CreateClient()
    {
        var client = httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(LocalHost);
        return client;
    }

    protected async Task<Result<T>?> GetAsync<T>(string url)
    {
        using var client = CreateClient();

        var response = await client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<Result<T>>();
    }

    protected async Task<Result<T>> PostAsync<T>(string url, object model)
    {
        using var client = CreateClient();

        var response = await client.PostAsJsonAsync(url, model);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<Result<T>>();
    }

    protected async Task<Result<T>?> PutAsync<T>(string url, object model)
    {
        using var client = CreateClient();

        var response = await client.PutAsJsonAsync(url, model);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<Result<T>>();
    }

    protected async Task<bool> DeleteAsync(string url)
    {
        using var client = CreateClient();

        var response = await client.DeleteAsync(url);

        return response.IsSuccessStatusCode;
    }
}