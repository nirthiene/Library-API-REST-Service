using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace LibraryApi.Models;

public class Copy
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("copies")]
    public int Copies { get; set; }
    [JsonPropertyName("bookId")]
    public int BookId { get; set; }
    public Book? Book { get; set; }
}