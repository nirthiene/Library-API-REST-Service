using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace LibraryApi.Models;

public class Book
{

    [JsonPropertyName("id")]
    public long Id { get; set; }
    [Required]
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("year")]
    public int Year { get; set; }

    [JsonPropertyName("authorId")]
    public int AuthorId { get; set; }
    public Author? Author { get; set; }
}
public class BookDto
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    [JsonPropertyName("year")]
    public int Year { get; set; }
    [JsonPropertyName("author")]
    public AuthorDto? Author { get; set; }
}
