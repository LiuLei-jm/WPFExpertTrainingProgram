using LayoutSystemDemo.Logging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LayoutSystemDemo;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private Random _random = new Random();
    private int _uniformItemsCounter = 0;
    private int _virtualItemsCounter = 0;
    private bool _isLogScrollingPaused = false;
    public MainWindow()
    {
        InitializeComponent();
    }

    private void AddItemsUniformPanel_Click(object sender, RoutedEventArgs e)
    {
        for (int i = 0; i < 5; i++)
        {
            var rect = new Border
            {
                Width = _random.Next(50, 150),
                Height = _random.Next(30, 80),
                Background = new SolidColorBrush(Color.FromRgb(
                   (byte)_random.Next(256), (byte)_random.Next(256), (byte)_random.Next(256)
                    )),
                Child = new TextBlock
                {
                    Text = $"Items {_uniformItemsCounter++}",
                    Foreground = Brushes.White,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                }
            };
            myUniformPanel.Children.Add(rect);
        }
        UpdateUniformPanelItemsCount();
        LogManager.LogInfo($"Added 5 items to uniformSpacingPanel. Total Items: {_uniformItemsCounter}");
    }

    private void UpdateUniformPanelItemsCount()
    {
        UniformPanelItemsCount.Text = VisualTreeHelper.GetChildrenCount(myUniformPanel).ToString();
    }

    private void ClearItemsFromUniformPanel_Click(object sender, RoutedEventArgs e)
    {
        myUniformPanel.Children.Clear();
        _uniformItemsCounter = 0;
        UpdateUniformPanelItemsCount();
        LogManager.LogInfo("Cleared all items from uniformSpacingPanel.");
    }

    private void AddOneThousandItemsToVirtualPanel_Click(object sender, RoutedEventArgs e)
    {
        for (int i = 0; i < 1000; i++)
        {
            myVirtualizingItemsControl.Items.Add($"Log entry {_virtualItemsCounter++}: This is a long virtualized item message to demonstrate scrolling");
        }
        UpdateVirtualPanelItemsCount();
        LogManager.LogInfo($"Added 1000 items to SimpleVirtualizingPanel. Total items: {_virtualItemsCounter}");
    }

    private void UpdateVirtualPanelItemsCount()
    {
        VirtualPanelItemCount.Text = myVirtualizingItemsControl.Items.Count.ToString();
    }

    private void ClearItemsFromVirtualPanel_Click(object sender, RoutedEventArgs e)
    {
        myVirtualizingItemsControl.Items.Clear();
        _virtualItemsCounter = 0;
        UpdateVirtualPanelItemsCount();
        LogManager.LogInfo($"CLeared add items from SimpleVirtualizingPanel.");
    }

    private void LogInfo_Click(object sender, RoutedEventArgs e)
    {
        LogManager.LogInfo("This is an information message.");
    }

    private void LogWarning_Click(object sender, RoutedEventArgs e)
    {
        LogManager.LogWarning("This is a warning message about something potentially problematic.");
    }

    private void LogError_Click(object sender, RoutedEventArgs e)
    {
        LogManager.LogError("A critical error occurred, please check immediately!");
    }

    private void LogDebug_Click(object sender, RoutedEventArgs e)
    {
        LogManager.LogDebug($"A debug message with internal tracing calculation: {_random.NextDouble():F4}");
    }

    private void LogPauseScroll_Click(object sender, RoutedEventArgs e)
    {
        myLogPanel?.PauseScrolling();
        _isLogScrollingPaused = true;
        LogManager.LogInfo("Log scrolling paused.");
    }

    private void LogResumeScroll_Click(object sender, RoutedEventArgs e)
    {
        myLogPanel?.ResumeScrolling();
        _isLogScrollingPaused = false;
        LogManager.LogInfo("Log scrolling resume.");
    }

    private void LogLevelFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (myLogPanel == null || LogLevelFilter.SelectedItem == null) return;
        string selectedLevel = (LogLevelFilter.SelectedItem as ComboBoxItem)?.Content?.ToString();
        myLogPanel.SetFilterLevel((LogLevel)Enum.Parse(typeof(LogLevel), selectedLevel ?? "Info"));
    }

    private void LogTextSearch_TextChanged(object sender, TextChangedEventArgs e)
    {
        myLogPanel?.SetSearchText(LogSearchText.Text);
    }
}