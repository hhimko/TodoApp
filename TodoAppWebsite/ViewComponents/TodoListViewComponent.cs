using Microsoft.AspNetCore.Mvc;

namespace TodoAppWebsite.ViewComponents;


public class TodoListViewComponent : ViewComponent
{
    private readonly IHttpClientFactory _httpClient;


    public TodoListViewComponent(IHttpClientFactory client)
    {
        _httpClient = client;
    }

    public async Task<IViewComponentResult> InvokeAsync()
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

        return View(todoItems.ToList());
    }
}
