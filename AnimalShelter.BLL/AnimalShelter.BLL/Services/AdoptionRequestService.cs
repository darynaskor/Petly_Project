using AnimalShelter.DAL.Repositories;
using AnimalShelter.DAL.Models;

namespace AnimalShelter.BLL.Services
{

    public class AdoptionRequestService
    {
        private readonly GenericRepository<AdoptionRequest> _repository;


        public AdoptionRequestService(GenericRepository<AdoptionRequest> repository)
        {
            _repository = repository;
        }


        public async Task<List<AdoptionRequest>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<AdoptionRequest?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task AddAsync(AdoptionRequest request) => await _repository.AddAsync(request);
        public async Task UpdateAsync(AdoptionRequest request) => await _repository.UpdateAsync(request);
        public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}
