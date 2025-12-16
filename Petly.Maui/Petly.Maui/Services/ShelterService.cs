using System.Collections.ObjectModel;
using Petly.Maui.Models;

namespace Petly.Maui.Services;

public class ShelterService
{
    private readonly JsonRepository<Shelter> _repo = new("shelters.json");
    private static readonly Random _rng = new();

    public async Task<ObservableCollection<Shelter>> LoadAsync()
    {
        var list = await _repo.LoadAsync();

        // якщо файл порожній — дефолти
        if (list.Count == 0)
        {
            list.Add(new Shelter { shelter_name = "Домівка", address = "вул. Стрийська" });
            list.Add(new Shelter { shelter_name = "ЛКП \"Лев\"", address = "вул. Шевченка" });
            list.Add(new Shelter { shelter_name = "Милосердя", address = "вул. Луганська 7" });
        }

        // дати координати тим, у кого їх немає
        var changed = EnsurePins(list);
        if (changed) await _repo.SaveAsync(list);

        return new ObservableCollection<Shelter>(list);
    }

    public async Task AddAsync(ObservableCollection<Shelter> ui, Shelter s)
    {
        // якщо координати не задані — покладемо рандом
        if (s.Px <= 0 || s.Py <= 0) (s.Px, s.Py) = RandomNearCenter();

        ui.Add(s);
        var all = ui.ToList();
        await _repo.SaveAsync(all);
    }

    public async Task DeleteAsync(ObservableCollection<Shelter> ui, string id)
    {
        var idx = ui.ToList().FindIndex(x => x.Id == id);
        if (idx >= 0)
        {
            ui.RemoveAt(idx);
            await _repo.SaveAsync(ui.ToList());
        }
    }

    /// <summary>Повертай псевдовипадкові координати, зосереджені біля центру.</summary>
    public (double x, double y) RandomNearCenter()
    {
        // трикутний/наближено нормальний розподіл навколо 0.5
        double Centered() =>
            (_rng.NextDouble() + _rng.NextDouble() + _rng.NextDouble()) / 3.0; // ≈N(0.5, ~0.05)
        // трохи звузимо до зони 0.25..0.75
        double clamp(double v) => Math.Clamp(0.25 + (v - 0.5) * 1.0, 0.0, 1.0);

        return (clamp(Centered()), clamp(Centered()));
    }

    /// <summary>Заповнює Px/Py тим, у кого 0; повертає true, якщо були зміни.</summary>
    private bool EnsurePins(IList<Shelter> items)
    {
        bool changed = false;
        foreach (var s in items)
        {
            if (s.Px <= 0 || s.Py <= 0)
            {
                (s.Px, s.Py) = RandomNearCenter();
                changed = true;
            }
        }
        return changed;
    }
}
