using AnimalShelter.DAL.Models;
using AnimalShelter.DAL.Repositories;

namespace AnimalShelter.BLL.Services
{
    public class PetService
    {
        private readonly GenericRepository<Pet> _repository;

        public PetService(GenericRepository<Pet> repository)
        {
            _repository = repository;
        }

        public async Task<List<Pet>> GetAllPetsAsync() => await _repository.GetAllAsync();
        public async Task<Pet?> GetPetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task AddPetAsync(Pet pet) => await _repository.AddAsync(pet);
        public async Task UpdatePetAsync(Pet pet) => await _repository.UpdateAsync(pet);
        public async Task DeletePetAsync(int id) => await _repository.DeleteAsync(id);

    }
}
