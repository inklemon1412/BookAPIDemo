using AutoMapper;
using BookAPIDemo.Data;
using BookAPIDemo.Items;
using BookAPIDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookAPIDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly BookDbContext _context;

        private readonly IMapper _mapper;

        public AuthorController(BookDbContext context, IMapper mapper)
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
                var authorCount = _context.Author.Count();
                var authorList = _mapper.Map<List<AuthorViewModel>>(_context.Author.Skip(pageIndex * pageSize).Take(pageSize).ToList());

                response.Status = true;
                response.Message = "Success";
                response.Data = new { Author = authorList, Count = authorCount };

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
        public IActionResult GetAuthorById(int id)
        {
            BaseResponseModel response = new BaseResponseModel();

            try
            {

                var author = _context.Author.Where(x => x.Id == id).FirstOrDefault();

                if (author == null)
                {
                    response.Status = false;
                    response.Message = "Failed to find any record of this item";

                    return BadRequest(response);
                }

                var authorData = _mapper.Map<AuthorDetailsViewModel>(author);

                response.Status = true;
                response.Message = "Success";
                response.Data = authorData;


                return Ok(response);

            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Something went wrong";

                return BadRequest(response);
            }

        }

        [HttpGet]
        [Route("Search/{searchText}")]
        public IActionResult Get(string searchText)
        {
            BaseResponseModel response = new BaseResponseModel();

            try
            {
                var searchAuthor = _context.Author.Where(x => x.Name.Contains(searchText)).Select(x => new
                {
                    x.Id,
                    x.Name
                }).ToList();

                response.Status = true;
                response.Message = "Success";
                response.Data = searchAuthor;


                return Ok(response);

            }
            catch
            {
                response.Status = false;
                response.Message = "Something went wrong";

                return BadRequest(response);
            }
        }

        [HttpPost]
        public IActionResult Post(AuthorViewModel model)
        {
            BaseResponseModel response = new BaseResponseModel();

            try
            {
                if (ModelState.IsValid)
                {

                    var postedModel = new Author()
                    {
                        Name = model.Name
                        

                    };

                    _context.Author.Add(postedModel);
                    _context.SaveChanges();

                    model.Id = postedModel.Id;


                    response.Status = true;
                    response.Message = "Created successfully!";
                    response.Data = model;

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
        public IActionResult Put(AuthorViewModel model)
        {
            BaseResponseModel response = new BaseResponseModel();

            try
            {
                if (!ModelState.IsValid)
                {
                    var postedModel = _mapper.Map<Author>(model);

                    if (model.Id <= 0)
                    {
                        response.Status = false;
                        response.Message = "Invalid author record";

                        return BadRequest(response);
                    }

                    var authorDetails = _context.Author.Where(x => x.Id == model.Id).AsNoTracking().FirstOrDefault();
                    if (authorDetails == null)
                    {
                        response.Status = false;
                        response.Message = "Invalid author record";

                        return BadRequest(response);
                    }

                    _context.Author.Update(postedModel);
                    _context.SaveChanges();

                    response.Status = true;
                    response.Message = "Updated successfully!";
                    response.Data = postedModel;

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
                var author = _context.Author.Where(x => x.Id == id).FirstOrDefault();
                if (author == null)
                {
                    response.Status = false;
                    response.Message = "Invalid author record!";

                    return BadRequest(response);
                }

                _context.Author.Remove(author);
                _context.SaveChanges();

                response.Status = true;
                response.Message = "Author deleted successfully";

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Something went wrong!";

                return BadRequest(response);

            }
        }
    }
}
