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
    public IActionResult GetAll() => Ok(_db.Books.ToList());

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var book = _db.Books.Include(b => b.Author).FirstOrDefault(b=> b.Id==id);
        //var book = _db.Books.Find(id);
        return book == null ? NotFound() : Ok(book);
    }

    [HttpPost]
    public IActionResult Create(Book book)
    {
        if (!IsValidBook(book))
            return BadRequest();

        var author = _db.Authors.Find(book.AuthorId);
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
    public IActionResult Update(int id, Book updatedBook)
    {
        var book = _db.Books.Find(id);
        if (book == null) return NotFound();

        book.Title = updatedBook.Title;
        book.Year = updatedBook.Year;
        book.AuthorId = updatedBook.AuthorId;

        _db.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var book = _db.Books.Find(id);
        if (book == null) return NotFound();

        _db.Books.Remove(book);
        _db.SaveChanges();
        return NoContent();
    }
}