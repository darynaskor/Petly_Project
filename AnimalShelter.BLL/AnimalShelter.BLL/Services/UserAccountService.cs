using AnimalShelter.DAL.Models;
using AnimalShelter.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalShelter.BLL.Services
{
    public class UserAccountService
    {
        private readonly GenericRepository<UserAccount> _repository;


        public UserAccountService(GenericRepository<UserAccount> repository)
        {
            _repository = repository;
        }


        public async Task<List<UserAccount>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<UserAccount?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task AddAsync(UserAccount account) => await _repository.AddAsync(account);
        public async Task UpdateAsync(UserAccount account) => await _repository.UpdateAsync(account);
        public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}
