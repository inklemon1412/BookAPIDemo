using System.ComponentModel.DataAnnotations;

namespace BookAPIDemo.Items
{
    public class Author
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public ICollection<Book> Books { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ModifiedDate { get; set; }
    }
}
