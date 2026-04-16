using System.Windows;

namespace VisualStateManagerDemo;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void SubmitBtn_Click(object sender, RoutedEventArgs e)
    {
        SubmitBtn.CurrentFormState = FormSubmitButton.FormState.Loading;
        SubmitBtn.IsEnabled = false;
        await Task.Delay(2000);
        bool isSuccess = new System.Random().Next(0, 2) == 0;
        if (isSuccess) SubmitBtn.CurrentFormState = FormSubmitButton.FormState.Success;
        else SubmitBtn.CurrentFormState = FormSubmitButton.FormState.Error;
        await Task.Delay(2000);
        SubmitBtn.CurrentFormState = FormSubmitButton.FormState.Idle;
        SubmitBtn.IsEnabled = true;
    }
}