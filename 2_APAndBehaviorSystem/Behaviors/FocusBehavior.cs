using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace APAndBehaviorSystem.Behaviors;

public static class FocusBehavior
{


    public static bool GetAutoSelectAll(DependencyObject obj)
    {
        return (bool)obj.GetValue(AutoSelectAllProperty);
    }

    public static void SetAutoSelectAll(DependencyObject obj, bool value)
    {
        obj.SetValue(AutoSelectAllProperty, value);
    }

    // Using a DependencyProperty as the backing store for AutoSelectAll.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty AutoSelectAllProperty =
        DependencyProperty.RegisterAttached("AutoSelectAll", typeof(bool), typeof(FocusBehavior), new PropertyMetadata(false, OnChanged));

    private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TextBox tb)
        {
            if ((bool)e.NewValue)
            {
                tb.GotKeyboardFocus += OnGotFocus;
                tb.PreviewMouseDown += OnMouseDown;
            }
            else
            {
                tb.GotKeyboardFocus -= OnGotFocus;
                tb.PreviewMouseDown -= OnMouseDown;
            }
        }
    }

    private static void OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        var tb = sender as TextBox;
        if (tb != null)
        {
            tb.Dispatcher.BeginInvoke(new Action(() =>
            {
                tb.SelectAll();
            }));
        }
    }

    private static void OnGotFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        var tb = sender as TextBox;
        if (tb != null)
        {
            tb.Dispatcher.BeginInvoke(new Action(() =>
            {
                tb.SelectAll();
            }));
        }
    }
}
