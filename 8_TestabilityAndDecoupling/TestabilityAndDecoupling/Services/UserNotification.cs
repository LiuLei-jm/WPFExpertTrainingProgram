using System.Windows;

namespace TestabilityAndDecoupling.Services;

public class UserNotification : IUserNotification
{
    public void ShowMessage(string message)
    {
        MessageBox.Show(message);
    }
}
