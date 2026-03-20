using Microsoft.Win32;
using RoutedEventDemo.Logging;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace RoutedEventDemo;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Info_Click(object sender, RoutedEventArgs e)
    {
        LogManager.Log(LogLevel.Info, "This is an informational message.");
    }

    private void Warning_Click(object sender, RoutedEventArgs e)
    {
        LogManager.Log(LogLevel.Warning, "This is an warning message.");
    }

    private void Error_Click(object sender, RoutedEventArgs e)
    {
        LogManager.Log(LogLevel.Error, "This is an error message.");
    }

    private void Pause_Click(object sender, RoutedEventArgs e)
    {
        logPanel.PauseScrolling();
    }

    private void Resume_Click(object sender, RoutedEventArgs e)
    {
        logPanel.ResumeScrolling();
    }

    private void ClearLog_Click(object sender, RoutedEventArgs e)
    {
        logPanel.Logs.Clear();
    }

    private void ExportLog_Click(object sender, RoutedEventArgs e)
    {
        var saveFileDialog = new SaveFileDialog
        {
            Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
            FileName = "log.txt"
        };
        if (saveFileDialog.ShowDialog() == true)
        {
            File.WriteAllLines(saveFileDialog.FileName, logPanel.Logs.Select(log => log.ToString()));
        }
    }

    private void LevelChanged(object sender, SelectionChangedEventArgs e)
    {
        if (logPanel == null) return;
        var selected = (sender as ComboBox)?.SelectedIndex ?? 0;
        var level = selected switch
        {
            0 => LogLevel.Info,
            1 => LogLevel.Warning,
            2 => LogLevel.Error,
            _ => LogLevel.Info
        };
        logPanel.SetFilterLevel(level);
    }

    private void SearchChanged(object sender, TextChangedEventArgs e)
    {
        logPanel.SetSearchText((sender as TextBox)?.Text ?? "");
    }
}