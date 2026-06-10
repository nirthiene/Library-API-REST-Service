using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryApi.Data;
using LibraryApi.Models;

namespace LibraryApi.Controllers;

[ApiController]
[Route("copies")]
public class CopiesController : ControllerBase
{
    private readonly AppDbContext _db;

    public CopiesController(AppDbContext db) => _db= db;

    [HttpPost]
    public IActionResult Create(Copy copy)
    {
        //does book exist in db
        var bookExists = _db.Books.Any(b => b.Id == copy.BookId);
        if (!bookExists) return NotFound("Book with that Id not found.");

        _db.Copies.Add(copy);
        _db.SaveChanges();

        return Ok(copy);
    }

    //book copy delete
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var copy = _db.Copies.Find(id);
        if (copy == null) return NotFound();

        _db.Copies.Remove(copy);
        _db.SaveChanges();

        return NoContent();
    }
}