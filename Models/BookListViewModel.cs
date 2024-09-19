namespace BookAPIDemo.Models
{
    public class BookListViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public List<AuthorViewModel> Authors { get; set; }

        public DateTime PublishDate { get; set; }

        public string CoverImage { get; set; }
    }
}
