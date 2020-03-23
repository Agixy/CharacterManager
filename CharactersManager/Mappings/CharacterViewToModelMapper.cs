using AutoMapper;
using CharactersManager.Models;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Service;

namespace CharactersManager.Mappings
{
    public class CharacterViewToModelMapper
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

        //public CharacterViewModel MapToView(Character character)
        //{
        //    var characterViewModel = Mapper.Map<CharacterViewModel>(character);

        //    characterViewModel.Images = 

        //    return character;
        //}

        //public IList<ImageViewModel> MapImagesToView(IList<Image> viewModelList)
        //{
        //    var result = new List<ImageViewModel>();
        //    if (viewModelList != null)
        //    {
        //        foreach (var relationship in viewModelList)
        //        {
        //            result.Add(new Relationship()
        //            {
        //                Type = relationship.Type,
        //                TargetRelationshipCharacter = allCharacters.FirstOrDefault(ch => ch.Id == relationship.TargetRelationshipCharacterId),
        //                CharacterId = relationship.CharacterId
        //            });
        //        }
        //    }

        //    return result;
        //}


        public List<Character> GetAllCharacters()
        {
            using (var context = new CharacterDbContext())
            {
                return context.Characters.ToList();
            }
        }

        public IList<Relationship> MapRelationshipsToModel(IList<RelationshipViewModel> viewModelList, IList<Character> allCharacters)
        {
            var result = new List<Relationship>();
            if(viewModelList != null)
            {
                foreach (var relationship in viewModelList)
                {
                    result.Add(new Relationship()
                    {
                        Type = relationship.Type,
                        TargetRelationshipCharacter = allCharacters.FirstOrDefault(ch => ch.Id == relationship.TargetRelationshipCharacterId),
                        CharacterId = relationship.CharacterId
                    });
                }
            }

            return result;
        }     
    }
}
