// Services/PetService.cs
using System.Collections.ObjectModel;
using Petly.Maui.Models;

namespace Petly.Maui.Services;

public class PetService
{
    private const string File = "pets.json";
    private readonly JsonRepository<Pet> _repo = new(File);

    // Підвантажити (якщо порожньо — додай демо-пети)
    public async Task<ObservableCollection<Pet>> LoadAsync()
    {
        var list = await _repo.LoadAsync();

        if (list.Count == 0)
        {
            list.Add(new Pet { pet_id = 1, pet_name = "Том", type = "Кіт", age = 2, gender = "♂", photourl = null, description = "Лагідний", shelter_id = 1, status = "available" });
            list.Add(new Pet { pet_id = 2, pet_name = "Мона", type = "Собака", age = 3, gender = "♀", photourl = null, description = "Активна", shelter_id = 2, status = "available" });
            await _repo.SaveAsync(list);
        }

        return new ObservableCollection<Pet>(list);
    }

    public async Task AddAsync(ObservableCollection<Pet> collection, Pet pet)
    {
        var list = await _repo.LoadAsync();

        // простий генератор id
        pet.pet_id = (list.Count == 0) ? 1 : list.Max(p => p.pet_id) + 1;

        list.Add(pet);
        await _repo.SaveAsync(list);

        collection.Add(pet); // в UI
    }

    public async Task UpdateAsync(ObservableCollection<Pet> collection, Pet updated)
    {
        var list = await _repo.LoadAsync();
        var idx = list.FindIndex(p => p.pet_id == updated.pet_id);
        if (idx >= 0) list[idx] = updated;
        await _repo.SaveAsync(list);

        // оновити колекцію
        var uiIdx = collection.IndexOf(collection.First(p => p.pet_id == updated.pet_id));
        collection[uiIdx] = updated;
    }

    public async Task DeleteAsync(ObservableCollection<Pet> collection, int petId)
    {
        var list = await _repo.LoadAsync();
        list.RemoveAll(p => p.pet_id == petId);
        await _repo.SaveAsync(list);

        var toRemove = collection.FirstOrDefault(p => p.pet_id == petId);
        if (toRemove != null) collection.Remove(toRemove);
    }
}
