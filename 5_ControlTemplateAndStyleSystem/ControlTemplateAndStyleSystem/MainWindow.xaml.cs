using System.ComponentModel;
using System.Windows;

namespace ControlTemplateAndStyleSystem;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    private bool _isWifiEnabled = false;
    public bool IsWifiEnabled
    {
        get => _isWifiEnabled;
        set
        {
            if (_isWifiEnabled != value)
            {
                _isWifiEnabled = value;
                OnPropertyChanged(nameof(IsWifiEnabled));
            }
        }
    }
    public MainWindow()
    {
        InitializeComponent();
        this.DataContext = this;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}