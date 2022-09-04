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
    public ActionResult<TodoItem> Get([FromRoute] long id)
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

    [HttpPost("item", Name = "PostItem")]
    public ActionResult<TodoItem> Post(TodoItem item)
    {
        TodoItem todo = _context.Insert(item);

        return Ok(todo);
    }

    [HttpPut("item/{id:long}", Name = "PutItemById")]
    public ActionResult<TodoItem> Put([FromRoute] long id, [FromBody] TodoItem item)
    {
        if (id != item.Id)
            return BadRequest("Route and request body id mishmash");

        TodoItem? todo = _context.Update(item);
        return todo is not null ? Ok(todo) : NotFound();
    }

    [HttpDelete("item/{id:long}", Name = "DeleteItemById")]
    public ActionResult Delete([FromRoute] long id)
    {
        return _context.Delete(id) ? NoContent() : NotFound();
    }
}
