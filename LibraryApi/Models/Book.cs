using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace LibraryApi.Models;

public class Book
{

    [JsonPropertyName("id")]
    public long Id { get; set; } //int -> long
    [Required]
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("year")]
    public int Year { get; set; }

    [JsonPropertyName("authorId")]
    public int AuthorId { get; set; }
    public Author? Author { get; set; }
    [JsonIgnore]
    public ICollection<Copy> Copies { get; set; } = new List<Copy>();
}