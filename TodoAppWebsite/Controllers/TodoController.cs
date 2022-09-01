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