using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace TodoAppWebsite.Controllers;


[Controller]
public class TodoController : Controller
{
    private readonly IHttpClientFactory _httpClient;
    private readonly ILogger<TodoController> _logger;
    private readonly IDateTimeProvider _dateTimeProvider;


    public TodoController(IHttpClientFactory httpClient, ILogger<TodoController> logger, IDateTimeProvider dateTime)
    {
        _httpClient = httpClient;
        _logger = logger;
        _dateTimeProvider = dateTime;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(TodoItem item)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var client = _httpClient.CreateClient("Todo");

        var response = await client.PostAsJsonAsync("api/todo/item", item);
        if (!response.IsSuccessStatusCode)
            return StatusCode(StatusCodes.Status500InternalServerError, "Connecion to API failed");

        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> TodoListPartial(int? hoveredTodoId)
    {
        var todoClient = _httpClient.CreateClient("Todo");
        var model = await todoClient.GetAsyncFromAPI<IEnumerable<TodoItem>>("api/todo/today");

        ViewBag.HoveredTodoId = hoveredTodoId;
        return PartialView("_TodoListPartial", model ?? Enumerable.Empty<TodoItem>());
    }

    [HttpPost]
    public async Task<IActionResult> SchedulePartial(int? hoveredTodoId)
    {
        var todoClient = _httpClient.CreateClient("Todo");
        var model = await todoClient.GetAsyncFromAPI<IEnumerable<TodoItem>>("api/todo/today");

        ViewBag.HoveredTodoId = hoveredTodoId;
        return PartialView("_SchedulePartial", new ScheduleViewModel(model ?? Enumerable.Empty<TodoItem>(), _dateTimeProvider));
    }

    [HttpPost]
    public async Task<bool> ChangeDoneStateAjax(int? id)
    {
        if (id is null)
            return false;

        var todoClient = _httpClient.CreateClient("Todo");
        var item = await todoClient.GetAsyncFromAPI<TodoItem>($"api/todo/item/{id}");
        if (item is null)
            return false;

        TodoItem updated = new(item.Id, item.Description, item.DayNumber, !item.Done, item.ScheduledTime);
        
        var response = await todoClient.PutAsJsonAsync($"api/todo/item/{id}", updated);
        return response.IsSuccessStatusCode;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
