namespace BookAPIDemo.Items
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }
        public ICollection <Author> Authors { get; set; }

        public DateTime PublishDate { get; set; }

        public string CoverImage { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ModifiedDate { get; set; }
    }
}
