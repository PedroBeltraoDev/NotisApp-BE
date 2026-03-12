using AutoMapper;
using NotesApp.Api.DTOs;
using NotesApp.Api.Models;
using NotesApp.Api.Repositories;

namespace NotesApp.Api.Services;

public class NoteService : INoteService
{
    private readonly INoteRepository _repository;
    private readonly ILogger<NoteService> _logger;
    private readonly IMapper _mapper;

    public NoteService(
        INoteRepository repository, 
        ILogger<NoteService> logger,
        IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Note> GetByIdAsync(int id)
    {
        _logger.LogInformation("Buscando nota com ID {NoteId}", id);
        
        var note = await _repository.GetByIdAsync(id);
        
        if (note == null)
        {
            _logger.LogWarning("Nota com ID {NoteId} não encontrada", id);
            throw new KeyNotFoundException($"Nota com ID {id} não encontrada");
        }
        
        return note;
    }

    public async Task<IEnumerable<Note>> GetAllAsync()
    {
        _logger.LogInformation("Buscando todas as notas");
        return await _repository.GetAllAsync();
    }

    public async Task<IEnumerable<Note>> GetFilteredAsync(string? folder, string? tag)
    {
        _logger.LogInformation("Filtrando notas - Pasta: {Folder}, Tag: {Tag}", folder, tag);
        
        if (!string.IsNullOrEmpty(folder) && !string.IsNullOrEmpty(tag))
        {
            var notes = await _repository.GetByFolderAsync(folder);
            return notes.Where(n => n.Tags != null && n.Tags.Contains(tag));
        }
        
        if (!string.IsNullOrEmpty(folder))
            return await _repository.GetByFolderAsync(folder);
        
        if (!string.IsNullOrEmpty(tag))
            return await _repository.GetByTagAsync(tag);
        
        return await GetAllAsync();
    }

    public async Task<Note> CreateAsync(CreateNoteDto dto)
    {
        _logger.LogInformation("Criando nova nota: {Title}", dto.Title);
        
        await ValidateNoteData(dto.Title, dto.Content);
        
        var note = _mapper.Map<Note>(dto);
        note.CreatedAt = DateTime.UtcNow;
        note.UpdatedAt = DateTime.UtcNow;
        
        var created = await _repository.CreateAsync(note);
        
        _logger.LogInformation("Nota criada com sucesso - ID: {NoteId}", created.Id);
        return created;
    }

    public async Task<Note> UpdateAsync(UpdateNoteDto dto)
    {
        _logger.LogInformation("Atualizando nota ID: {NoteId}", dto.Id);
        
        var existing = await GetByIdAsync(dto.Id);
        await ValidateNoteData(dto.Title, dto.Content);
        
        _mapper.Map(dto, existing);
        existing.UpdatedAt = DateTime.UtcNow;
        
        await _repository.UpdateAsync(existing);
        
        _logger.LogInformation("Nota atualizada com sucesso - ID: {NoteId}", dto.Id);
        return existing;
    }

    public async Task DeleteAsync(int id)
    {
        _logger.LogInformation("Excluindo nota ID: {NoteId}", id);
        
        await GetByIdAsync(id); //Valida se existe
        await _repository.DeleteAsync(id);
        
        _logger.LogInformation("Nota excluída com sucesso - ID: {NoteId}", id);
    }

    public async Task<IEnumerable<string>> GetDistinctFoldersAsync()
    {
        return await _repository.GetDistinctFoldersAsync();
    }

    public async Task<IEnumerable<string>> GetDistinctTagsAsync()
    {
        return await _repository.GetDistinctTagsAsync();
    }

    private async Task ValidateNoteData(string title, string content)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Título é obrigatório", nameof(title));
        
        if (title.Length < 3 || title.Length > 200)
            throw new ArgumentException("Título deve ter entre 3 e 200 caracteres", nameof(title));
        
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Conteúdo é obrigatório", nameof(content));
        
        if (content.Length < 10 || content.Length > 1000)
            throw new ArgumentException("Conteúdo deve ter entre 10 e 1000 caracteres", nameof(content));
    }
}