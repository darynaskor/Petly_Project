using Petly.Maui.Models;
using System.Collections.ObjectModel;

namespace Petly.Maui.Services
{
    public interface IAdoptionService
    {
        Task AddRequestAsync(AdoptionRequest request);
        Task<List<AdoptionRequest>> GetAllAsync();
        Task<AdoptionRequest?> GetByIdAsync(string id);
        Task UpdateStatusAsync(string id, AdoptionStatus newStatus);
        Task MarkPetAsAdoptedAsync(string petId); // для адміну

        // Новий метод: отримати заявку користувача
        Task<AdoptionRequest?> GetUserRequestAsync(string userId);
    }

    public class AdoptionService : IAdoptionService
    {
        private readonly string _filePath =
            Path.Combine(FileSystem.Current.AppDataDirectory, "adoption_requests.json");

        private List<AdoptionRequest> _cache = new();

        public AdoptionService()
        {
            Load();
        }

        private void Load()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                _cache = System.Text.Json.JsonSerializer.Deserialize<List<AdoptionRequest>>(json) ?? new();
            }
        }

        private void Save()
        {
            var options = new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true
            };
            string json = System.Text.Json.JsonSerializer.Serialize(_cache, options);
            File.WriteAllText(_filePath, json);
        }

        public Task AddRequestAsync(AdoptionRequest request)
        {
            _cache.Add(request);
            Save();
            return Task.CompletedTask;
        }

        public Task<List<AdoptionRequest>> GetAllAsync()
        {
            return Task.FromResult(_cache.OrderByDescending(x => x.adopt_id).ToList());
        }

        public Task<AdoptionRequest?> GetByIdAsync(string id)
        {
            return Task.FromResult(_cache.FirstOrDefault(x => x.adopt_id == id));
        }

        public Task UpdateStatusAsync(string id, AdoptionStatus newStatus)
        {
            var req = _cache.FirstOrDefault(x => x.adopt_id == id);
            if (req != null)
            {
                req.StatusEnum = newStatus;
                Save();
            }
            return Task.CompletedTask;
        }

        // Адмін позначає тваринку як прилаштовану
        public Task MarkPetAsAdoptedAsync(string petId)
        {
            var req = _cache.FirstOrDefault(x => x.pet_id == petId);
            if (req != null)
            {
                req.IsPetAdopted = true;
                Save();
            }
            return Task.CompletedTask;
        }

        // Отримати заявку конкретного користувача
        public Task<AdoptionRequest?> GetUserRequestAsync(string userId)
        {
            var req = _cache.FirstOrDefault(x => x.user_id == userId);
            return Task.FromResult(req);
        }
    }
}
