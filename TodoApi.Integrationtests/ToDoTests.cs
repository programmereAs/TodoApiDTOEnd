using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using FluentAssertions;
using Newtonsoft.Json;
using TodoApi.Models;
using Xunit;

namespace TodoApi.Integrationtests
{
    public class ToDoTests: IClassFixture<IntegrationTestWebApplicationFactory>
    {
        private readonly HttpClient _client;
        public ToDoTests(IntegrationTestWebApplicationFactory fixture)
        {
            _client = fixture.CreateClient();
        }

        [Fact]
        public async Task TestThat_TodoItems_CanBeCreated()
        {

            //POST
            var message = new TodoItemDTO{IsComplete = false, Name = "DotheFirstTest"};
            var item = await _client.PostAsJsonAsync<TodoItemDTO>("api/ToDoItems", message);
            item.EnsureSuccessStatusCode();
            var actual = await item.Content.ReadFromJsonAsync<TodoItemDTO>();
            actual.IsComplete.Should().BeFalse();
            actual.Name.Should().Be(message.Name);

            //GET 
            var readItem = await _client.GetFromJsonAsync<TodoItem>($"api/TodoItems/{actual.Id}");
            readItem?.Name.Should().Be(message.Name);
            

        }
    }
}
