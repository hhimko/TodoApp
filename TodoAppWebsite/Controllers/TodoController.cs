using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using TodoAppWebsite.ViewComponents;

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
    public async Task<bool?> ChangeDoneStateAjax(int? id)
    {
        if (id is null)
            return null;

        TodoItem? todoItem = null;

        var client = _httpClient.CreateClient("Todo");
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/todo/item/{id}");

        var response = await client.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<TodoItem>();
            if (result is not null)
                todoItem = result;
        }
        return todoItem?.Done;
    }

    public IActionResult TodoListVC()
    {
        return ViewComponent(typeof(TodoListViewComponent));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
