using NotesApp.Api.DTOs;
using NotesApp.Api.Models;

namespace NotesApp.Api.Mappers;

public static class NoteMapper
{
    public static NoteResponseDto ToResponseDto(Note note)
    {
        return new NoteResponseDto
        {
            Id = note.Id,
            Title = note.Title,
            Content = note.Content,
            CreatedAt = note.CreatedAt,
            UpdatedAt = note.UpdatedAt,
            Folder = note.Folder,
            Tags = note.Tags ?? Enumerable.Empty<string>(),
            IsPinned = note.IsPinned
        };
    }
    
    public static IEnumerable<NoteResponseDto> ToResponseDtoList(IEnumerable<Note> notes)
    {
        return notes.Select(ToResponseDto);
    }
    
    public static Note FromCreateDto(CreateNoteDto dto)
    {
        return new Note
        {
            Title = dto.Title.Trim(),
            Content = dto.Content.Trim(),
            Folder = dto.Folder?.Trim(),
            Tags = dto.Tags ?? new List<string>(),
            IsPinned = dto.IsPinned,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
    
    public static void UpdateFromDto(Note note, UpdateNoteDto dto)
    {
        note.Title = dto.Title.Trim();
        note.Content = dto.Content.Trim();
        note.Folder = dto.Folder?.Trim();
        note.Tags = dto.Tags ?? new List<string>();
        note.IsPinned = dto.IsPinned;
        note.UpdatedAt = DateTime.UtcNow;
    }
}