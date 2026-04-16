using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VisualStateManagerDemo;

[TemplateVisualState(GroupName = "CommonStates", Name = "Normal")]
[TemplateVisualState(GroupName = "CommonStates", Name = "MouseOver")]
[TemplateVisualState(GroupName = "CommonStates", Name = "Pressed")]
[TemplateVisualState(GroupName = "FormStates", Name = "Idle")]
[TemplateVisualState(GroupName = "FormStates", Name = "Loading")]
[TemplateVisualState(GroupName = "FormStates", Name = "Success")]
[TemplateVisualState(GroupName = "FormStates", Name = "Error")]
public class FormSubmitButton : Button
{
    public enum FormState { Idle, Loading, Success, Error };


    public FormState CurrentFormState
    {
        get { return (FormState)GetValue(CurrentFormStateProperty); }
        set { SetValue(CurrentFormStateProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrentFormState.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentFormStateProperty =
        DependencyProperty.Register(nameof(CurrentFormState), typeof(FormState), typeof(FormSubmitButton), new PropertyMetadata(FormState.Idle, OnFormStateChanged));


    static FormSubmitButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(FormSubmitButton), new FrameworkPropertyMetadata(typeof(FormSubmitButton)));
    }
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        UpdateVisualState(false);
    }

    private void UpdateVisualState(bool v)
    {
        VisualStateManager.GoToState(this, CurrentFormState.ToString(), v);
    }

    private static void OnFormStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is FormSubmitButton btn) btn.UpdateVisualState(true);
    }
    protected override void OnMouseEnter(MouseEventArgs e)
    {
        base.OnMouseEnter(e);
        VisualStateManager.GoToState(this, "MouseOver", true);
    }
    protected override void OnMouseLeave(MouseEventArgs e)
    {
        base.OnMouseLeave(e);
        VisualStateManager.GoToState(this, "Normal", true);
    }
    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);
        VisualStateManager.GoToState(this, "Pressed", true);
    }
    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonUp(e);
        VisualStateManager.GoToState(this, IsMouseOver ? "MouseOver" : "Normal", true);
    }
}
