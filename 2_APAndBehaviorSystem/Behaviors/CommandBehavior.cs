using System.Windows;
using System.Windows.Input;

namespace APAndBehaviorSystem.Behaviors;

public static class CommandBehavior
{


    public static ICommand GetEnterKeyCommand(DependencyObject obj)
    {
        return (ICommand)obj.GetValue(EnterKeyCommandProperty);
    }

    public static void SetEnterKeyCommand(DependencyObject obj, ICommand value)
    {
        obj.SetValue(EnterKeyCommandProperty, value);
    }

    // Using a DependencyProperty as the backing store for EnterKeyCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty EnterKeyCommandProperty =
        DependencyProperty.RegisterAttached("EnterKeyCommand", typeof(ICommand), typeof(CommandBehavior), new PropertyMetadata(null, OnChanged));

    private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is UIElement element)
        {
            element.KeyDown -= OnKeyDown;
            if (element != null) element.KeyDown += OnKeyDown;
        }
    }

    private static void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter) return;
        var element = sender as FrameworkElement;
        var command = GetEnterKeyCommand(element);
        if (command == null) return;
        var parameter = element.DataContext;
        if (command.CanExecute(parameter))
        {
            command.Execute(parameter);
            e.Handled = true;
        }
    }
}
