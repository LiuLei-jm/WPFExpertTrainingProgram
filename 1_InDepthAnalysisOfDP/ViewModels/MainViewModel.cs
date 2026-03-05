using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace InDepthAnalysisOfDP.ViewModels;

public partial class MainViewModel : ObservableObject, IDataErrorInfo
{
    private double _number;
    public double Number
    {
        get => _number;
        set => SetProperty(ref _number, value);
    }
    public string this[string columnName]
    {
        get
        {
            if (columnName == nameof(Number))
            {
                if (Number > 80) return "不要超过80";
            }
            return null!;
        }
    }

    public string Error => null!;
}
