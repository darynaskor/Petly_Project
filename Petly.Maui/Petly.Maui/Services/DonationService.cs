using Petly.Maui.Models;

namespace Petly.Maui.Services
{
    public interface IDonationService
    {
        Task AddAsync(Donation donation);
        Task<IReadOnlyList<Donation>> GetAllAsync();
    }

    public class DonationService : IDonationService
    {
        private readonly JsonRepository<Donation> _repo = new("donations.json");

        public async Task AddAsync(Donation donation)
        {
            var list = await _repo.LoadAsync();
            list.Add(donation);
            await _repo.SaveAsync(list);
        }

        public async Task<IReadOnlyList<Donation>> GetAllAsync()
        {
            var list = await _repo.LoadAsync();
            return list
                .OrderByDescending(d => d.CreatedAt)
                .ToList();
        }
    }
}
