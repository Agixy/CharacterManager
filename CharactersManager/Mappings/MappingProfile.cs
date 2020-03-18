using AutoMapper;
using CharactersManager.Models;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharactersManager.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<CharacterViewModel, Character>();
            CreateMap<OriginViewModel, Origin>();
            CreateMap<PersonalityViewModel, Personality>();
            CreateMap<AppearanceViewModel, Appearance>();


            CreateMap<Character, CharacterViewModel>();
            CreateMap<Origin, OriginViewModel>();
            CreateMap<Personality, PersonalityViewModel>();
            CreateMap<Appearance, AppearanceViewModel>();
            CreateMap<Breed, string>().ConvertUsing(r => r.Name);
            CreateMap<Book, string>().ConvertUsing(a => a.Title);

            CreateMap<Relationship, RelationshipViewModel>()
                    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                    .ForMember(dest => dest.TargetRelationshipCharacterName, opt => opt.MapFrom(src => src.TargetRelationshipCharacter.Name + " " + src.TargetRelationshipCharacter.Surname))
                    .ForMember(dest => dest.TargetRelationshipCharacterId, opt => opt.MapFrom(src => src.TargetRelationshipCharacter.Id));
        }
    }
}
