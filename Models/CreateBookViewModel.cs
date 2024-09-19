using BookAPIDemo.Items;
using System.ComponentModel.DataAnnotations;

namespace BookAPIDemo.Models
{
    public class CreateBookViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Name of the book is required!")]
        public string Title { get; set; }

        public string Description { get; set; }
        public List <int> Authors { get; set; }

        public DateTime PublishDate { get; set; }

        public string CoverImage { get; set; }

       
    }
}
