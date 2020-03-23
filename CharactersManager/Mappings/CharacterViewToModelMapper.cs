using AutoMapper;
using CharactersManager.Models;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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

         
    }
}
