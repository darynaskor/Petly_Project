using AnimalShelter.DAL.Models;
using AnimalShelter.DAL.Repositories;

namespace AnimalShelter.BLL.Services
{
    public class UserService
    {
        private readonly GenericRepository<User> _repository;


        public UserService(GenericRepository<User> repository)
        {
            _repository = repository;
        }


        public async Task<List<User>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<User?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task AddAsync(User user) => await _repository.AddAsync(user);
        public async Task UpdateAsync(User user) => await _repository.UpdateAsync(user);
        public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}
