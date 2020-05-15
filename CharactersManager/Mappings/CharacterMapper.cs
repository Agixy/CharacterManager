using AutoMapper;
using CharactersManager.Models;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CharactersManager.Mappings
{
    public class CharacterMapper
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg => {
                // This line ensures that internal properties are also mapped over.
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<MappingProfile>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        });

        public static IMapper Mapper => Lazy.Value;

        public Character MapToModel(CharacterViewModel characterViewModel)
        {            
            var character = Mapper.Map<Character>(characterViewModel);  

            

            return character;
        }

        public CharacterViewModel MapToView(Character character)
        {
            var characterViewModel = Mapper.Map<CharacterViewModel>(character);

            if(character.Relationships != null && character.Relationships != String.Empty)
            {
                foreach (var relationship in character.Relationships.Split(',').ToList())
                {
                    var array = relationship.Split('-').ToArray();
                    characterViewModel.Relationships.Add(new RelationshipViewModel() { CharacterId = character.Id, TargetRelationshipCharacterName = array[0], Type = array[1] });
                }
            }         

            return characterViewModel;
        }
    }
}
