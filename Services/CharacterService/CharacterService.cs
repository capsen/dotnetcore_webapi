using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet.Dtos;
using dotnet.Dtos.Character;

namespace dotnet.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
         private static List<Character> characters = new List<Character>{
            new Character(),
            new Character{ Id=1, Name="Sam"}
        };

        private readonly IMapper _mapper;

        public CharacterService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<ServiceResponse<List<GetCharacterDto>>> Addcharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            Character character = _mapper.Map<Character>(newCharacter);
            character.Id = characters.Max(c => c.Id)+1;
            characters.Add(character);
            serviceResponse.Data=characters.Select(c=> _mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDto>> response = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                Character character = characters.FirstOrDefault(c=>c.Id == id);
                characters.Remove(character);
                return new ServiceResponse<List<GetCharacterDto>>{
                    Data = characters.Select(c=> _mapper.Map<GetCharacterDto>(c)).ToList()
                };
                
            }
            catch (Exception ex)
            {
                response.Success=false;
                response.message=ex.Message;
            }
            
            return response;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            return new ServiceResponse<List<GetCharacterDto>>{Data = characters.Select(c=> _mapper.Map<GetCharacterDto>(c)).ToList()
            };
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var character = characters.FirstOrDefault(c=>c.Id == id);
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updateCharacter)
        {
            ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>();
            try
            {
                Character character = characters.FirstOrDefault(c=>c.Id == updateCharacter.Id);
                _mapper.Map(updateCharacter, character);
                // character.Name = updateCharacter.Name;
                // character.HitPoints = updateCharacter.HitPoints;
                // character.Defense = updateCharacter.Defense;
                // character.Intelligence = updateCharacter.Intelligence;
                // character.Class = updateCharacter.Class;

                response.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception ex)
            {
                response.Success=false;
                response.message=ex.Message;
            }
            
            return response;
        }
    }
}