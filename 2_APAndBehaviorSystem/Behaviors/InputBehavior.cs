using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace APAndBehaviorSystem.Behaviors;

public static class InputBehavior
{


    public static bool GetOnlyNumeric(DependencyObject obj)
    {
        return (bool)obj.GetValue(OnlyNumericProperty);
    }

    public static void SetOnlyNumeric(DependencyObject obj, bool value)
    {
        obj.SetValue(OnlyNumericProperty, value);
    }

    // Using a DependencyProperty as the backing store for OnlyNumeric.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty OnlyNumericProperty =
        DependencyProperty.RegisterAttached("OnlyNumeric", typeof(bool), typeof(InputBehavior), new PropertyMetadata(false, OnOnlyNumericChanged));

    private static void OnOnlyNumericChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TextBox tb)
        {
            if ((bool)e.NewValue)
            {
                tb.PreviewTextInput += OnTextInput;
                tb.PreviewKeyDown += OnKeyDown;
                DataObject.AddPastingHandler(tb, OnPaste);
            }
            else
            {
                tb.PreviewTextInput -= OnTextInput;
                tb.PreviewKeyDown -= OnKeyDown;
                DataObject.RemovePastingHandler(tb, OnPaste);
            }
        }
    }
    private static readonly Regex _regex = new Regex("^[0-9]+$");
    private static void OnPaste(object sender, DataObjectPastingEventArgs e)
    {
        if (e.DataObject.GetDataPresent(typeof(string)))
        {
            var text = e.DataObject.GetData(typeof(string)) as string;
            if (!_regex.IsMatch(text)) e.CancelCommand();
        }
        else
        {
            e.CancelCommand();
        }
    }

    private static void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Space) e.Handled = true;
    }

    private static void OnTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !_regex.IsMatch(e.Text);
    }
}
