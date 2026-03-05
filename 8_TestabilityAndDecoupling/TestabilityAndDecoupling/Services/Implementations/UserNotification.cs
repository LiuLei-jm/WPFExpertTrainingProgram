using System.Windows;
using TestabilityAndDecoupling.Services.Interfaces;

namespace TestabilityAndDecoupling.Services;

public class UserNotification : IUserNotification
{
    public void ShowMessage(string message)
    {
        MessageBox.Show(message);
    }
}
