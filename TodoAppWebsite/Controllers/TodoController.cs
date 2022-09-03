using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;

namespace TodoAppWebsite.Controllers;


public class TodoController : Controller
{
    private readonly IHttpClientFactory _httpClient;
    private readonly ILogger<TodoController> _logger;


    public TodoController(IHttpClientFactory httpClient, ILogger<TodoController> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> TodoListPartial()
    {
        var client = _httpClient.CreateClient("Todo");
        var request = new HttpRequestMessage(HttpMethod.Get, "api/todo/today");

        var response = await client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            return StatusCode(StatusCodes.Status500InternalServerError, "Connecion to API failed");

        IEnumerable<TodoItem>? result;
        try
        {
            result = await response.Content.ReadFromJsonAsync<IEnumerable<TodoItem>>();
        } 
        catch (JsonException e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }

        return PartialView("_TodoListPartial", result ?? Enumerable.Empty<TodoItem>());
    }

    [HttpPost]
    public async Task<bool> ChangeDoneStateAjax(int? id)
    {
        if (id is null)
            return false;

        var client = _httpClient.CreateClient("Todo");
        TodoItem? item = await client.GetFromJsonAsync<TodoItem>($"api/todo/item/{id}");
        if (item is null)
            return false;

        TodoItem updated = new(item.Id, item.Name, item.DayNumber, !item.Done, item.ScheduledTime);
        var response = await client.PutAsJsonAsync<TodoItem>($"api/todo/item/{id}", updated);

        return response.IsSuccessStatusCode;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
