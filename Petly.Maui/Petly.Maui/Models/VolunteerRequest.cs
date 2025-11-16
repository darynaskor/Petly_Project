namespace Petly.Maui.Models;

public enum VolunteerStatus
{
    Нова,
    Розглядається,
    Підтверджено,
    Відхилено
}

public class VolunteerRequest
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    // Дані користувача
    public string UserId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    // Дані волонтерства
    public string Shelter { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;

    // Статус заявки
    public VolunteerStatus StatusEnum { get; set; } = VolunteerStatus.Нова;

    public string Status
    {
        get => StatusEnum.ToString();
        set
        {
            if (Enum.TryParse(value, out VolunteerStatus parsed))
                StatusEnum = parsed;
        }
    }
}
