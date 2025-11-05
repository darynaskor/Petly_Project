using System.Text.Json;

namespace Petly.Maui.Services
{
    // Простий універсальний репозиторій для збереження будь-якого типу об’єктів у JSON-файл
    public class JsonRepository<T> where T : class, new()
    {
        private readonly string filePath;

        public JsonRepository(string fileName)
        {
            // Зберігаємо файл у локальній папці програми
            string folder = FileSystem.AppDataDirectory;
            filePath = Path.Combine(folder, fileName);
        }

        // 📥 Завантаження списку з файлу
        public async Task<List<T>> LoadAsync()
        {
            if (!File.Exists(filePath))
                return new List<T>();

            using FileStream stream = File.OpenRead(filePath);
            return await JsonSerializer.DeserializeAsync<List<T>>(stream)
                   ?? new List<T>();
        }

        // 💾 Збереження списку у файл
        public async Task SaveAsync(List<T> items)
        {
            using FileStream stream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(stream, items,
                new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
