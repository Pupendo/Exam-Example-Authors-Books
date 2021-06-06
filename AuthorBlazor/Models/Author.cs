using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AuthorBlazor.Models
{
    public class Author
    {
        public int Id { get; set; }
        [Required, MaxLength(15)]
        public string FirstName { get; set; }
        [Required, MaxLength(15)]
        public string LastName { get; set; }
        public List<Book> Books { get; set; }
        
        
    }
}