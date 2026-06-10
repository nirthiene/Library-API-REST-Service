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

	public BooksController(AppDbContext db)
	{
		_db = db;
	}

	[HttpGet]
	public IActionResult GetAll([FromQuery] int? authorId)
	{
		var query = _db.Books.Include(b => b.Author).AsQueryable();

		if (authorId.HasValue)
		{
			query = query.Where(b => b.AuthorId == authorId);
		}

		return Ok(query.ToList());
	}

	[HttpPost]
	public IActionResult Create(Book book)
	{
		_db.Books.Add(book);
		_db.SaveChanges();
		return Ok(book);
	}

	[HttpPut("{id}")]
	public IActionResult Update(int id, Book updatedBook)
	{
		var book = _db.Books.Find(id);
		if (book == null) return NotFound();

		// validation
		if (updatedBook.Year < 0) return BadRequest("Year can't be negative.");

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