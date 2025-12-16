using Petly.Maui.Models;
using Microsoft.Extensions.Logging; 
using System.Collections.ObjectModel;
using System.Text.Json;

namespace Petly.Maui.Services
{
    public interface IAdoptionService
    {
        Task AddRequestAsync(AdoptionRequest request);
        Task<List<AdoptionRequest>> GetAllAsync();
        Task<AdoptionRequest?> GetByIdAsync(string id);
        Task UpdateStatusAsync(string id, AdoptionStatus newStatus);
        Task MarkPetAsAdoptedAsync(string petId); 
        Task<AdoptionRequest?> GetUserRequestAsync(string userId);
    }

    public class AdoptionService : IAdoptionService
    {
        private readonly string _filePath =
            Path.Combine(FileSystem.Current.AppDataDirectory, "adoption_requests.json");

        private List<AdoptionRequest> _cache = new();
        private readonly ILogger<AdoptionService> _logger; 

        public AdoptionService(ILogger<AdoptionService> logger)
        {
            _logger = logger;
            Load();
        }

        private void Load()
        {
            _logger.LogInformation("Завантаження заявок на адопцію з файлу...");
            try
            {
                if (File.Exists(_filePath))
                {
                    string json = File.ReadAllText(_filePath);
                    _cache = JsonSerializer.Deserialize<List<AdoptionRequest>>(json) ?? new();
                    _logger.LogInformation("Успішно завантажено {Count} заявок.", _cache.Count);
                }
                else
                {
                    _logger.LogWarning("Файл заявок не знайдено. Створено новий список.");
                    _cache = new();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Критична помилка при читанні adoption_requests.json");
                _cache = new(); 
            }
        }

        private void Save()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                string json = JsonSerializer.Serialize(_cache, options);
                File.WriteAllText(_filePath, json);

                _logger.LogDebug("Файл заявок успішно оновлено.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка при збереженні файлу заявок!");
            }
        }

        public Task AddRequestAsync(AdoptionRequest request)
        {
            _logger.LogInformation("Створення нової заявки: User {User} -> Pet {Pet}", request.user_id, request.pet_id);

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
                _logger.LogInformation("Зміна статусу заявки {Id}: {OldStatus} -> {NewStatus}",
                    id, req.StatusEnum, newStatus);

                req.StatusEnum = newStatus;
                Save();
            }
            else
            {
                _logger.LogWarning("Спроба оновити статус неіснуючої заявки: {Id}", id);
            }
            return Task.CompletedTask;
        }

        // Адмін позначає тваринку як прилаштовану
        public Task MarkPetAsAdoptedAsync(string petId)
        {
            var req = _cache.FirstOrDefault(x => x.pet_id == petId);
            if (req != null)
            {
                _logger.LogInformation("Тварина {PetId} офіційно усиновлена через заявку {ReqId}.", petId, req.adopt_id);

                req.IsPetAdopted = true;
                Save();
            }
            else
            {
                _logger.LogWarning("Не знайдено активної заявки для тварини {PetId}, щоб позначити як adopted.", petId);
            }
            return Task.CompletedTask;
        }

        // Отримати заявку конкретного користувача
        public Task<AdoptionRequest?> GetUserRequestAsync(string userId)
        {
            var req = _cache.FirstOrDefault(x => x.user_id == userId);

            if (req == null)
                _logger.LogDebug("Для користувача {UserId} активних заявок не знайдено.", userId);

            return Task.FromResult(req);
        }
    }
}