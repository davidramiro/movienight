using Microsoft.AspNetCore.Identity.UI.Services;
using MovieNight.Data;

namespace MovieNight.Services;

public interface IUserNotifier
{
    public void NotifyUsers();
}