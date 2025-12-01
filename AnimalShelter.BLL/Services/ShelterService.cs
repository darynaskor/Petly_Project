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

        public async Task<List<Shelter>> GetAllShelterAsync() => await _repository.GetAllAsync();
        public async Task<Shelter?> GetShelterByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task AddShelterAsync(Shelter shelter) => await _repository.AddAsync(shelter);
        public async Task UpdateShelterAsync(Shelter shelter) => await _repository.UpdateAsync(shelter);
        public async Task DeleteShelterAsync(int id) => await _repository.DeleteAsync(id);
    }
}
