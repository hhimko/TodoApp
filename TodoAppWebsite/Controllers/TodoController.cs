using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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

    public async Task<IActionResult> Index()
    {
        IEnumerable<TodoItem> todoItems = Enumerable.Empty<TodoItem>();

        var client = _httpClient.CreateClient("Todo");
        var request = new HttpRequestMessage(HttpMethod.Get, "api/todo/today");

        var response = await client.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<TodoItem>>();
            if (result is not null)
                todoItems = result;
        }

        return View(new TodoViewModel() { TodoItems = todoItems.ToList() });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}