using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryApi.Data;
using LibraryApi.Models;

namespace LibraryApi.Controllers;

[ApiController]
[Route("books")]
public class BooksController : ControllerBase
{
    private readonly AppDbContext _db;

    public BooksController(AppDbContext db) => _db = db;

    private bool IsValidBook(Book book)
    {
        return !string.IsNullOrWhiteSpace(book.Title)
               && book.Year > 0
               && _db.Authors.Any(a => a.Id == book.AuthorId);
    }

    [HttpGet]
    public IActionResult GetAll([FromQuery] long? authorId)
    {
        var query = _db.Books
            .Include(b => b.Author)
            .AsQueryable();

        if (authorId.HasValue)
        {
            query = query.Where(b => b.AuthorId == authorId.Value);
        }

        var books = query
            .Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Year = b.Year,
                Author = new AuthorDto
                {
                    Id = b.Author.Id,
                    FirstName = b.Author.FirstName,
                    LastName = b.Author.LastName
                }
            })
            .ToList();

        return Ok(books);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(long id)
    {
        var book = _db.Books
            .Include(b => b.Author)
            .Where(b => b.Id == id)
            .Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Year = b.Year,
                Author = b.Author == null ? null : new AuthorDto
                {
                    Id = b.Author.Id,
                    FirstName = b.Author.FirstName,
                    LastName = b.Author.LastName
                }
            })
            .FirstOrDefault();
        return book == null ? NotFound() : Ok(book);
    }

    [HttpPost]
    public IActionResult Create(Book book)
    {
        if (!IsValidBook(book))
            return BadRequest();

        var author = _db.Authors.Find((long)book.AuthorId);
        book.Author = author;

        _db.Books.Add(book);
        _db.SaveChanges();

        return CreatedAtAction(
            nameof(GetById),
            new { id = book.Id },
            book
        );
    }

    [HttpPut("{id}")]
    public IActionResult Update(long id, Book updatedBook)
    {
        var book = _db.Books.Find(id);
        if (book == null) return NotFound();

        if (!int.TryParse(updatedBook.Year.ToString(), out int yearValue) || yearValue < 0)
        {
            return BadRequest("Invalid year.");
        }
        var authorExists = _db.Authors.Any(a => a.Id == updatedBook.AuthorId);
        if (!authorExists)
        {
            return BadRequest("Author does not exist.");
        }

        book.Title = updatedBook.Title;
        book.Year = updatedBook.Year;
        book.AuthorId = updatedBook.AuthorId;

        _db.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
        var book = _db.Books.Find(id);
        if (book == null) return NotFound();

        _db.Books.Remove(book);
        _db.SaveChanges();
        return NoContent();
    }
}