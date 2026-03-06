using System.ComponentModel.DataAnnotations;

namespace NotesApp.Api.DTOs;

public class UpdateNoteDto
{
    [Required]
    public int Id { get; set; }
    
    [Required]
    [MinLength(3)]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [MinLength(10)]
    [MaxLength(1000)]
    public string Content { get; set; } = string.Empty;
    
    public string? Folder { get; set; }
    
    public List<string> Tags { get; set; } = new();
    
    public bool IsPinned { get; set; }
}