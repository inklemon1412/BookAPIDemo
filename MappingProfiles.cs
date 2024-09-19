using AutoMapper;
using BookAPIDemo.Items;
using BookAPIDemo.Models;

namespace BookAPIDemo
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<Book, BookListViewModel>();
            CreateMap<Book, BookDetailsViewModel>();
            CreateMap<BookListViewModel, Book>();
            CreateMap<CreateBookViewModel, Book>().ForMember(x => x.Authors, y => y.Ignore());

            CreateMap<Author, AuthorViewModel>();
            CreateMap<Author, AuthorDetailsViewModel>();
            CreateMap<AuthorViewModel, Author>();
        }
    }
}
