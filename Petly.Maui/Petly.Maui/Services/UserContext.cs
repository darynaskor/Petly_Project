using Petly.Maui.Models;

namespace Petly.Maui.Services;

public class UserContext
{
    private readonly IAuthService _auth;
    private readonly JsonRepository<UserAccount> _repo = new("accounts.json");

    public UserAccount? CurrentUser { get; private set; }

    public UserContext(IAuthService auth) => _auth = auth;

    public async Task LoadCurrentUserAsync()
    {
        var email = _auth.CurrentEmail;
        if (string.IsNullOrWhiteSpace(email))
        {
            CurrentUser = null;
            return;
        }

        var list = await _repo.LoadAsync();
        CurrentUser = list.FirstOrDefault(u =>
            u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }

    public async Task SaveCurrentUserAsync(UserAccount updated)
    {
        var list = await _repo.LoadAsync();
        var idx = list.FindIndex(u => u.Email.Equals(updated.Email, StringComparison.OrdinalIgnoreCase));
        if (idx >= 0) list[idx] = updated;
        else list.Add(updated);
        await _repo.SaveAsync(list);
        CurrentUser = updated;
    }
}
