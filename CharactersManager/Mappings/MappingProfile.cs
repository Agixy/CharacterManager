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
            CreateMap<CharacterViewModel, Character>().ForMember(dest => dest.Relationships, m => m.MapFrom(src => String.Join(",", src.Relationships)));
            CreateMap<OriginViewModel, Origin>();
            CreateMap<PersonalityViewModel, Personality>();
            CreateMap<AppearanceViewModel, Appearance>();          


            CreateMap<Character, CharacterViewModel>().ForMember(x => x.Relationships, opt => opt.MapFrom(character => MapCharacterRelationships(character)));
            CreateMap<Origin, OriginViewModel>();
            CreateMap<Personality, PersonalityViewModel>();
            CreateMap<Appearance, AppearanceViewModel>();
            CreateMap<Breed, string>().ConvertUsing(r => r.Name);     
            CreateMap<Image, ImageViewModel>().ForMember(dest => dest.ImageData, opt => opt.MapFrom(src => string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(src.ImageData))));
        }

        public List<RelationshipViewModel> MapCharacterRelationships(Character character)
        {
            var result = new List<RelationshipViewModel>();

            if (character.Relationships != null && character.Relationships != String.Empty)
            {
                foreach (var relationship in character.Relationships.Split(',').ToList())
                {
                    var array = relationship.Split('-').ToArray();
                    result.Add(new RelationshipViewModel() { CharacterId = character.Id, TargetRelationshipCharacterName = array[0], Type = array[1] });
                }
            }

            return result;
        }
    }
}
