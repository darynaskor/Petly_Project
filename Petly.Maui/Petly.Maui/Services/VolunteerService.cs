using Petly.Maui.Models;
using Microsoft.Maui.Storage;

namespace Petly.Maui.Services;

public interface IVolunteerService
{
    Task AddRequestAsync(VolunteerRequest request);
    Task<List<VolunteerRequest>> GetAllAsync();
    Task<VolunteerRequest?> GetByIdAsync(string id);
    Task<VolunteerRequest?> GetUserRequestAsync(string userId);
    Task UpdateStatusAsync(string id, VolunteerStatus newStatus);
}

public class VolunteerService : IVolunteerService
{
    private readonly string _filePath =
        Path.Combine(FileSystem.Current.AppDataDirectory, "volunteer_requests.json");

    private List<VolunteerRequest> _cache = new();

    public VolunteerService()
    {
        Load();
    }

    private void Load()
    {
        if (File.Exists(_filePath))
        {
            string json = File.ReadAllText(_filePath);
            _cache = System.Text.Json.JsonSerializer.Deserialize<List<VolunteerRequest>>(json) ?? new();
        }
    }

    private void Save()
    {
        var options = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
        string json = System.Text.Json.JsonSerializer.Serialize(_cache, options);
        File.WriteAllText(_filePath, json);
    }

    public Task AddRequestAsync(VolunteerRequest request)
    {
        _cache.Add(request);
        Save();
        return Task.CompletedTask;
    }

    public Task<List<VolunteerRequest>> GetAllAsync()
    {
        return Task.FromResult(_cache.OrderByDescending(x => x.Id).ToList());
    }

    public Task<VolunteerRequest?> GetByIdAsync(string id)
    {
        return Task.FromResult(_cache.FirstOrDefault(x => x.Id == id));
    }

    public Task<VolunteerRequest?> GetUserRequestAsync(string userId)
    {
        var req = _cache.FirstOrDefault(x => x.UserId == userId);
        return Task.FromResult(req);
    }

    public Task UpdateStatusAsync(string id, VolunteerStatus newStatus)
    {
        var req = _cache.FirstOrDefault(x => x.Id == id);
        if (req != null)
        {
            req.StatusEnum = newStatus;
            Save();
        }
        return Task.CompletedTask;
    }
}
