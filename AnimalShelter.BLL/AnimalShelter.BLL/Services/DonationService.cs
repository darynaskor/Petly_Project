using AnimalShelter.DAL.Models;
using AnimalShelter.DAL.Repositories;

namespace AnimalShelter.BLL.Services
{
    public class DonationService
    {
        private readonly GenericRepository<Donation> _repository;

        public DonationService(GenericRepository<Donation> repository)
        {
            _repository = repository;
        }

        public async Task<List<Donation>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<Donation?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task AddAsync(Donation donation) => await _repository.AddAsync(donation);
        public async Task UpdateAsync(Donation donation) => await _repository.UpdateAsync(donation);
        public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}
