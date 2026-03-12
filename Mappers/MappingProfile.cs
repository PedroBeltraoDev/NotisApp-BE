using AutoMapper;
using NotesApp.Api.DTOs;
using NotesApp.Api.Models;

namespace NotesApp.Api.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateNoteDto, Note>();
            CreateMap<UpdateNoteDto, Note>();
            CreateMap<Note, NoteResponseDto>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags ?? new List<string>()));
        }
    }
}