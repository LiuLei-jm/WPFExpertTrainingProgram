using System.Windows.Media;

namespace PerformanceOptimization.Models;

public class LogItem
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public Brush BackgroundBrush { get; set; }
    private readonly static SolidColorBrush _frozenBrush;
    static LogItem()
    {
        _frozenBrush = new SolidColorBrush(Color.FromRgb(240, 248, 255));
        _frozenBrush.Freeze();
    }
    public static Brush GetOptimizedBrush() => _frozenBrush;
    public static Brush GetBadBrush() => new SolidColorBrush(Color.FromRgb(240, 248, 255));
}
