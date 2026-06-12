using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryApi.Data;
using LibraryApi.Models;

namespace LibraryApi.Controllers;

[ApiController]
[Route("authors")]
public class AuthorsController : ControllerBase
{
    private readonly AppDbContext _db;

    public AuthorsController(AppDbContext db) => _db = db;

    [HttpGet]
    public IActionResult GetAll() => Ok(_db.Authors.ToList());

    [HttpPost]
    public IActionResult Create(Author author)
    {
        _db.Authors.Add(author);
        _db.SaveChanges();
        return CreatedAtAction(nameof(GetAll), new { id = author.Id }, author);
        //return StatusCode(201, author);
        //return Ok(author); //200 - Ok
    }
    [HttpPut("{id}")]
    public IActionResult Update(long id, Author author)
    {
        var existingAuthor = _db.Authors.Find(id);

        if (existingAuthor == null)
        {
            return NotFound();
        }

        existingAuthor.FirstName = author.FirstName;
        existingAuthor.LastName = author.LastName;

        _db.SaveChanges();

        return NoContent();
    }
    [HttpGet("{id}")]
    public IActionResult GetById(long id)
    {
        var author = _db.Authors.Find(id);

        if (author == null)
            return NotFound();

        return Ok(author);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
        var author = _db.Authors.Find(id);

        if (author == null)
            return NotFound();

        _db.Authors.Remove(author);
        _db.SaveChanges();

        return NoContent(); // 204
    }


}