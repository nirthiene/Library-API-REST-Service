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

    public AuthorsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_db.Authors.ToList());
    }

    [HttpPost]
    public IActionResult Create(Author author)
    {
        _db.Authors.Add(author);
        _db.SaveChanges();
        return Ok(author);
    }
}