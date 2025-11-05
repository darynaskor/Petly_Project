using AnimalShelter.DAL.Repositories;
using AnimalShelter.DAL.Models;

namespace AnimalShelter.BLL.Services
{
 
    public class AdoptionService
    {
        private readonly GenericRepository<AdoptionRequest> _repository;

        public AdoptionService(GenericRepository<AdoptionRequest> repository)
        {
            _repository = repository;
        }

        public async Task<List<AdoptionRequest>> GetAllAdoptionRequestAsync() => await _repository.GetAllAsync();
        public async Task<AdoptionRequest?> GetAdoptionRequestByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task AddAdoptionRequestAsync(AdoptionRequest adoptionRequest) => await _repository.AddAsync(adoptionRequest);
        public async Task UpdateAdoptionRequestAsync(AdoptionRequest adoptionRequest) => await _repository.UpdateAsync(adoptionRequest);
        public async Task DeleteAdoptionRequestAsync(int id) => await _repository.DeleteAsync(id);
    }
}
