using APAndBehaviorSystem.ViewModels.Commands;
using System.ComponentModel;

namespace APAndBehaviorSystem.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    public RelayCommand SubmitCommand { get; }
    private string _message;
    public string Message
    {
        get => _message;
        set
        {
            _message = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));
        }
    }
    public MainViewModel()
    {
        SubmitCommand = new RelayCommand(OnSubmit);
    }

    private void OnSubmit()
    {
        Message = "Enter pressed!";
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
