using AutoMapper;
using NotesApp.Api.DTOs;
using NotesApp.Api.Models;

namespace NotesApp.Api.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateNoteDto, Note>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
            
        CreateMap<UpdateNoteDto, Note>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
            
        CreateMap<Note, NoteResponseDto>();
    }
}