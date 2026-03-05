using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace InDepthAnalysisOfDP.CustomControls;

[TemplatePart(Name = PART_TextBox, Type = typeof(TextBox))]
[TemplatePart(Name = PART_IncreaseButton, Type = typeof(Button))]
[TemplatePart(Name = PART_DecreaseButton, Type = typeof(Button))]
public class NumericUpDown : Control
{
    private const string PART_TextBox = "PART_TextBox";
    private const string PART_IncreaseButton = "PART_IncreaseButton";
    private const string PART_DecreaseButton = "PART_DecreaseButton";
    private TextBox _textBox;
    private Button _increaseButton;
    private Button _decreaseButton;
    private DispatcherTimer _repeatTimer;
    private bool _isIncreasing;
    #region Commands
    public static readonly RoutedCommand IncreaseCommand = new RoutedCommand(nameof(IncreaseCommand), typeof(NumericUpDown));
    public static readonly RoutedCommand DecreaseCommand = new RoutedCommand(nameof(DecreaseCommand), typeof(NumericUpDown));
    #endregion
    static NumericUpDown()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(typeof(NumericUpDown)));
    }
    public NumericUpDown()
    {
        CommandBindings.Add(new CommandBinding(IncreaseCommand, OnIncrease));
        CommandBindings.Add(new CommandBinding(DecreaseCommand, OnDecrease));
        _repeatTimer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromMilliseconds(100)
        };
        _repeatTimer.Tick += RepeatTimer_Tick;
    }

    #region Dependency Properties


    public double Value
    {
        get { return (double)GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value", typeof(double), typeof(NumericUpDown), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChange, CoerceValue));

    private static void OnValueChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var c = (NumericUpDown)d;
        c.UpdateText();
    }

    private static object CoerceValue(DependencyObject d, object baseValue)
    {
        var c = (NumericUpDown)d;
        var v = (double)baseValue;
        if (v < c.Min) v = c.Min;
        if (v > c.Max) v = c.Max;
        return Math.Round(v, c.Precision);
    }


    public double Min
    {
        get { return (double)GetValue(MinProperty); }
        set { SetValue(MinProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Min.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty MinProperty =
        DependencyProperty.Register("Min", typeof(double), typeof(NumericUpDown), new PropertyMetadata(double.MinValue, OnLimitChanged));


    public double Max
    {
        get { return (double)GetValue(MaxProperty); }
        set { SetValue(MaxProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Max.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty MaxProperty =
        DependencyProperty.Register("Max", typeof(double), typeof(NumericUpDown), new PropertyMetadata(double.MaxValue, OnLimitChanged));


    public double Step
    {
        get { return (double)GetValue(StepProperty); }
        set { SetValue(StepProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Step.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty StepProperty =
        DependencyProperty.Register("Step", typeof(double), typeof(NumericUpDown), new PropertyMetadata(0d));


    public int Precision
    {
        get { return (int)GetValue(PrecisionProperty); }
        set { SetValue(PrecisionProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Precision.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty PrecisionProperty =
        DependencyProperty.Register("Precision", typeof(int), typeof(NumericUpDown), new PropertyMetadata(2, OnLimitChanged));

    private static void OnLimitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var c = (NumericUpDown)d;
        c.CoerceValue(ValueProperty);
    }
    #endregion
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        _textBox = GetTemplateChild(PART_TextBox) as TextBox;
        _increaseButton = GetTemplateChild(PART_IncreaseButton) as Button;
        _decreaseButton = GetTemplateChild(PART_DecreaseButton) as Button;
        if (_textBox != null)
        {
            _textBox.LostFocus += (_, _) =>
            {
                if (double.TryParse(_textBox.Text, out double r))
                {
                    Value = r;
                }
            };
            _textBox.PreviewKeyDown += TextBox_PreviewKeyDown;
        }
        if (_increaseButton != null)
        {
            _increaseButton.Command = IncreaseCommand;
            _increaseButton.PreviewMouseLeftButtonDown += (_, _) => StartRepeat(true);
            _increaseButton.PreviewMouseLeftButtonUp += (_, _) => StopRepeat();
        }
        if (_decreaseButton != null)
        {
            _decreaseButton.Command = DecreaseCommand;
            _decreaseButton.PreviewMouseLeftButtonDown += (_, _) => StartRepeat(false);
            _decreaseButton.PreviewMouseLeftButtonUp += (_, _) => StopRepeat();
        }
        UpdateText();
    }
    #region Repeat
    private void StopRepeat()
    {
        _repeatTimer.Stop();
    }

    private void StartRepeat(bool v)
    {
        _isIncreasing = v;
        _repeatTimer.Start();
    }

    private void RepeatTimer_Tick(object? sender, EventArgs e)
    {
        if (_isIncreasing)
        {
            ExecuteIncrease();
        }
        else
        {
            ExecuteDecrease();
        }
    }
    #endregion
    #region Keyboard
    private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Up)
        {
            ExecuteIncrease();
            e.Handled = true;
        }
        else if (e.Key == Key.Down)
        {
            ExecuteDecrease();
            e.Handled = true;
        }
    }
    #endregion
    #region Command Logic
    private void ExecuteDecrease() => Value -= Step;

    private void ExecuteIncrease() => Value += Step;
    private void OnDecrease(object sender, ExecutedRoutedEventArgs e) => ExecuteDecrease();

    private void OnIncrease(object sender, ExecutedRoutedEventArgs e) => ExecuteIncrease();
    #endregion
    private void UpdateText()
    {
        if (_textBox != null) _textBox.Text = Value.ToString($"F{Precision}", CultureInfo.InvariantCulture);
    }

}
