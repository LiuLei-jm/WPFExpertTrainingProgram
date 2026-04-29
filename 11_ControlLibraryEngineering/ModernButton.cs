using System.Windows;
using System.Windows.Controls;

namespace ControlLibraryEngineering;

public class ModernButton : Button
{
    static ModernButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(ModernButton),
            new FrameworkPropertyMetadata(typeof(ModernButton))
            );
    }
    public CornerRadius CornerRadius
    {
        get { return (CornerRadius)GetValue(CornerRadiusProperty); }
        set { SetValue(CornerRadiusProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(ModernButton), new PropertyMetadata(new CornerRadius(5)));

}
