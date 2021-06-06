using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AuthorBlazor.Models;

namespace AuthorBlazor.Service
{
    public class AuthorService
    {
        private HttpClient client;
        private string uri = "https://localhost:5003";

        public AuthorService()
        {
            client = new HttpClient();
        }
        
        public async Task AddAdultAsync(Author author)
        {
            string authorJson = JsonSerializer.Serialize(author);
            HttpContent content = new StringContent(authorJson, Encoding.UTF8, "application/json");
            await client.PostAsync(uri + "/authors", content);
        }

        public async Task<List<Author>> GetAuthorsAsync()
        {
            Task<string> stringAsync = client.GetStringAsync(uri + "/authors");
            string message = await stringAsync;
            
            List<Author> authors = JsonSerializer.Deserialize<List<Author>>(message, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            return authors;
        }
    }
}