using Microsoft.AspNetCore.Mvc;

namespace TodoAppAPI.Controllers;


[ApiController]
[Route("api/todo")]
public class TodoItemsController : ControllerBase
{
    private readonly IDbContext _context;
    private readonly ILogger<TodoItemsController> _logger;

    public TodoItemsController(IDbContext context, ILogger<TodoItemsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("item/{id:long}", Name = "GetItemById")]
    public ActionResult Get([FromRoute] long id)
    {
        TodoItem? todo = _context.GetById(id);
        return todo is not null ? Ok(todo) : NotFound();
    }

    [HttpGet("{dayNumber:int}", Name = "GetAllByDayNumber")]
    public ActionResult<IEnumerable<TodoItem>> Get([FromRoute] int dayNumber)
    {
        return Ok(_context.GetAllByDayNumber(dayNumber));
    }

    [HttpGet("today", Name = "GetAllForToday")]
    public ActionResult GetToday()
    {
        var daynum = DateOnly.FromDateTime(DateTime.Now).DayNumber;
        return RedirectToRoute("GetAllByDayNumber", new { dayNumber = daynum });
    }
}