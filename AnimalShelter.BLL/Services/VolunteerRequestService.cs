using AnimalShelter.DAL.Models;
using AnimalShelter.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalShelter.BLL.Services
{
    public class VolunteerRequestService
    {
        private readonly GenericRepository<VolunteerRequest> _repository;


        public VolunteerRequestService(GenericRepository<VolunteerRequest> repository)
        {
            _repository = repository;
        }


        public async Task<List<VolunteerRequest>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<VolunteerRequest?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task AddAsync(VolunteerRequest request) => await _repository.AddAsync(request);
        public async Task UpdateAsync(VolunteerRequest request) => await _repository.UpdateAsync(request);
        public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}
