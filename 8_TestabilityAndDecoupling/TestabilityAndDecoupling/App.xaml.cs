using System.Windows;
using TestabilityAndDecoupling.Services;
using TestabilityAndDecoupling.Services.Interfaces;
using TestabilityAndDecoupling.ViewModel;

namespace TestabilityAndDecoupling;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        IDataService dataService = new DataService();
        IUserNotification notification = new UserNotification();
        var vm = new MainViewModel(dataService, notification);
        var window = new MainWindow { DataContext = vm };
        window.Show();
    }
}

