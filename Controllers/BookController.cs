using AutoMapper;
using BookAPIDemo.Data;
using BookAPIDemo.Items;
using BookAPIDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace BookAPIDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookDbContext _context;
        private readonly IMapper _mapper;

        public BookController(BookDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]

        public IActionResult Get(int pageIndex = 0, int pageSize = 10)
        {
            BaseResponseModel response = new BaseResponseModel();
            
            try
            {
                var bookCount = _context.Book.Count();
                var bookList = _mapper.Map<List<BookListViewModel>>( _context.Book.Include(x => x.Authors).Skip(pageIndex * pageSize).Take(pageSize).ToList());

                response.Status = true;
                response.Message = "Success";
                response.Data = new { Books = bookList, Count = bookCount };

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Something went wrong";

                return BadRequest(response);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetBookById(int id)
        {
            BaseResponseModel response = new BaseResponseModel();

            try
            {

                var book = _context.Book.Include(x => x.Authors).Where(x => x.Id == id).FirstOrDefault();

                if (book == null)
                {
                    response.Status = false;
                    response.Message = "Failed to find any record of this item";

                    return BadRequest(response);
                }

                var bookData = _mapper.Map<BookDetailsViewModel>(book);

                response.Status = true;
                response.Message = "Success";
                response.Data = bookData;


                return Ok(response);

            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Something went wrong";

                return BadRequest(response);
            }

        }

        [HttpPost]
        public IActionResult Post(CreateBookViewModel model)
        {
            BaseResponseModel response = new BaseResponseModel();

            try
            {
                if (ModelState.IsValid)
                {
                    var authors = _context.Author.Where(x => model.Authors.Contains(x.Id)).ToList();

                    if (authors.Count != model.Authors.Count)
                    {
                        response.Status = false;
                        response.Message = "Invalid author assigned";

                        return BadRequest(response);
                    }

                    var postedModel = _mapper.Map<Book>(model);
                    postedModel.Authors = authors;
                    

                    _context.Book.Add(postedModel);
                    _context.SaveChanges();

                    var responseData = _mapper.Map<BookDetailsViewModel>(postedModel);

                    response.Status = true;
                    response.Message = "Created successfully!";
                    response.Data = responseData;

                    return Ok(response);
                }
                else
                {
                    response.Status = false;
                    response.Message = "Validation failed";
                    response.Data = ModelState;

                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Something went wrong";

                return BadRequest(response);
            }
        }

        [HttpPut]
        public IActionResult Put(CreateBookViewModel model)
        {
            BaseResponseModel response = new BaseResponseModel();

            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Id <= 0)
                    {
                        response.Status = false;
                        response.Message = "Invalid book record";

                        return BadRequest(response);
                    }

                    var authors = _context.Author.Where(x => model.Authors.Contains(x.Id)).ToList();

                    if (authors.Count != model.Authors.Count)
                    {
                        response.Status = false;
                        response.Message = "Invalid author assigned!";

                        return BadRequest(response);
                    }

                    var bookDetails = _context.Book.Include(x => x.Authors).Where(x => x.Id == model.Id).FirstOrDefault();

                    if (bookDetails == null)
                    {
                        response.Status = false;
                        response.Message = "Invalid book record!";

                        return BadRequest(response);
                    }

                    bookDetails.CoverImage = model.CoverImage;
                    bookDetails.Description = model.Description;
                    bookDetails.PublishDate = model.PublishDate;
                    bookDetails.Title = model.Title;

                    var removedAuthors = bookDetails.Authors.Where(x => !model.Authors.Contains(x.Id)).ToList();

                    foreach (var author in removedAuthors)
                    {
                        bookDetails.Authors.Remove(author);
                    }

                    var addedAuthors = authors.Except(bookDetails.Authors).ToList();
                    foreach (var author in addedAuthors)
                    {
                        bookDetails.Authors.Add(author);
                    }

                    _context.SaveChanges();

                    var responseData = new BookDetailsViewModel
                    {
                        Id = bookDetails.Id,
                        Title = bookDetails.Title,
                        Authors = bookDetails.Authors.Select(y => new AuthorViewModel
                        {
                            Id = y.Id,
                            Name = y.Name,
                           

                        }).ToList(),
                        CoverImage = bookDetails.CoverImage,
                        PublishDate = bookDetails.PublishDate,
                        Description = bookDetails.Description
                    };

                    response.Status = true;
                    response.Message = "Created successfully!";
                    response.Data = responseData;

                    return Ok(response);
                }
                else
                {
                    response.Status = false;
                    response.Message = "Validation failed";
                    response.Data = ModelState;

                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Something went wrong";

                return BadRequest(response);
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            BaseResponseModel response = new BaseResponseModel();

            try
            {
                var book = _context.Book.Where(x => x.Id == id).FirstOrDefault();
                if (book == null)
                {
                    response.Status = false;
                    response.Message = "Invalid book record!";

                    return BadRequest(response);
                }

                _context.Book.Remove(book);
                _context.SaveChanges();

                response.Status = true;
                response.Message = "Book deleted successfully";

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Something went wrong!";

                return BadRequest(response);
            }
        }

        [HttpPost]
        [Route("upload-book-cover")]
        public async Task<IActionResult> UploadBookCover(IFormFile imageFile)
        {
            try
            {
                var filename = ContentDispositionHeaderValue.Parse(imageFile.ContentDisposition).FileName.TrimStart('\"').TrimEnd('\"');
                string newPath = @"D:\to-delete";

                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }

                string[] allowedImageExtensions = new string[] { ".jpg", ".jpeg", ".png"};

                if (!allowedImageExtensions.Contains(Path.GetExtension(filename)))
                {
                    return BadRequest(new BaseResponseModel
                    {
                        Status = false,
                        Message = "Only .jpg, .jpeg and .png files allowed!"
                    }); 
                }

                string newFileName = Guid.NewGuid() + Path.GetExtension(filename);
                string fullFilePath = Path.Combine(newPath, newFileName);

                using (var stream = new FileStream(fullFilePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                return Ok(new { ProfileImage = $"{HttpContext.Request.Scheme} ://{HttpContext.Request.Host}/StaticFiles/{newFileName}" });
            }
            catch(Exception ex)
            {
                return BadRequest(new BaseResponseModel
                {
                    Status = false,
                    Message = "Error Occured!"
                });
            }
        }
    }
}
