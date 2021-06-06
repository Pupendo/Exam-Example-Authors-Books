using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AuthorBlazor.Models;

namespace AuthorBlazor.Service
{
    public class BookService
    {
        private HttpClient client;
        private string uri = "https://localhost:5003";

        public BookService()
        {
            client = new HttpClient();
        }
        public async Task AddBookAsync(Book book, int id)
        {
            string authorJson = JsonSerializer.Serialize(book);
            HttpContent content = new StringContent(authorJson, Encoding.UTF8, "application/json");
            await client.PostAsync($"{uri}/books/{id}", content);
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            Task<string> stringAsync = client.GetStringAsync(uri + "/books");
                string message = await stringAsync;
            
                List<Book> books = JsonSerializer.Deserialize<List<Book>>(message, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                return books;
        }

        public async Task RemoveBook(int isbn)
        {
            await client.DeleteAsync($"{uri}/books/{isbn}");
        }
    }
}