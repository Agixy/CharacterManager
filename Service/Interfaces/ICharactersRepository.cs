using Microsoft.AspNetCore.Http;
using Service.Models;
using System.Collections.Generic;

namespace Service.Interfaces
{
    public interface ICharactersRepository
    {
        void AddCharacter(Character character);
        void DeleteCharacter(int characterId);
        IList<AlignmentChart> GetAlignmentChatrs();
        IList<Image> GetAllAvatars();
        IList<Breed> GetAllBreeds();
        IList<Character> GetAllCharacters();
        IList<Image> GetAllImages();
        Character GetCharacterById(int id);
        IList<Character> GetCharactersByBreed(int breedId);
        Image GetImageById(int imageId);
        IList<Image> GetImagesByCharacterId(int characterId);
        void AddImage(int characterId, IFormFileCollection fileList);
        IList<Orientation> GetOrientations();
        IList<TypeOfCharacter> GetTypesOfCharacter();
        void UpdateCharacter(Character character);
    }
}
