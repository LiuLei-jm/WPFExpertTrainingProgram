using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ControlTemplateAndStyleSystem;

[TemplatePart(Name = PartThumbName, Type = typeof(Ellipse))]
public class CustomToggleSwitch : ToggleButton
{
    private const string PartThumbName = "PART_Thumb";
    private Ellipse _thumbElement;


    public Brush CheckedBackground
    {
        get { return (Brush)GetValue(CheckedBackgroundProperty); }
        set { SetValue(CheckedBackgroundProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CheckedBackground.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CheckedBackgroundProperty =
        DependencyProperty.Register(nameof(CheckedBackground), typeof(Brush), typeof(CustomToggleSwitch), new PropertyMetadata(Brushes.LimeGreen));


    static CustomToggleSwitch()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomToggleSwitch), new FrameworkPropertyMetadata(typeof(CustomToggleSwitch)));
    }
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        if (_thumbElement != null) _thumbElement.MouseEnter -= Thumb_MouseEnter;
        _thumbElement = GetTemplateChild(PartThumbName) as Ellipse;
        if (_thumbElement != null) _thumbElement.MouseEnter += Thumb_MouseEnter;
    }

    private void Thumb_MouseEnter(object sender, MouseEventArgs e)
    {
    }
}
