using AnimalShelter.DAL.Repositories;
using AnimalShelter.DAL.Models;

namespace AnimalShelter.BLL.Services
{
    public class ShelterService
    {
        private readonly GenericRepository<Shelter> _repository;


        public ShelterService(GenericRepository<Shelter> repository)
        {
            _repository = repository;
        }


        public async Task<List<Shelter>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<Shelter?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task AddAsync(Shelter shelter) => await _repository.AddAsync(shelter);
        public async Task UpdateAsync(Shelter shelter) => await _repository.UpdateAsync(shelter);
        public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}
