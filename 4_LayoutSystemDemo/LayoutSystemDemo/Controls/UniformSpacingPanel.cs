using LayoutSystemDemo.Logging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace LayoutSystemDemo.Controls;

public class UniformSpacingPanel : Panel, IScrollInfo
{
    public double ItemSpacing
    {
        get { return (double)GetValue(ItemSpcingProperty); }
        set { SetValue(ItemSpcingProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ItemSpcing.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ItemSpcingProperty =
        DependencyProperty.Register(nameof(ItemSpacing), typeof(double), typeof(UniformSpacingPanel), new FrameworkPropertyMetadata(5.0, FrameworkPropertyMetadataOptions.AffectsMeasure));


    public bool CanHorizontallyScroll { get; set; } = true;
    public bool CanVerticallyScroll { get; set; } = false;

    public double ExtentHeight { get; private set; }

    public double ExtentWidth { get; private set; }

    public double HorizontalOffset { get; private set; }

    public ScrollViewer? ScrollOwner { get; set; }

    public double VerticalOffset { get; private set; }

    public double ViewportHeight { get; private set; }

    public double ViewportWidth { get; private set; }

    protected override Size MeasureOverride(Size availableSize)
    {
        LogManager.LogDebug($"UniformSpacingPanel.MeasureOverride called with availableSize = {availableSize}");

        double totalWidth = 0;
        double maxHeight = 0;
        int visibleChildrenCount = 0;
        foreach (UIElement child in InternalChildren)
        {
            visibleChildrenCount++;
            child.Measure(new Size(double.PositiveInfinity, availableSize.Height));
            totalWidth += child.DesiredSize.Width;
            maxHeight = Math.Max(maxHeight, child.DesiredSize.Height);
        }
        if (visibleChildrenCount > 1)
        {
            totalWidth += (visibleChildrenCount - 1) * ItemSpacing;
        }

        ExtentWidth = totalWidth;
        ExtentHeight = maxHeight;
        ViewportWidth = availableSize.Width;
        ViewportHeight = availableSize.Height;

        Size desiredSize = new Size(totalWidth, maxHeight);
        LogManager.LogDebug($"UniformSpacingPanel.MeasureOverrid returning desiredSize = {desiredSize}");
        return desiredSize;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        LogManager.LogDebug($"UniformSpacingPanel.ArrangeOverride called with finalSize = {finalSize}");
        double currentX = -HorizontalOffset;
        foreach (UIElement child in InternalChildren)
        {
            if (child.Visibility != Visibility.Collapsed)
            {
                double childWidth = child.DesiredSize.Width;
                double childHeight = Math.Min(child.DesiredSize.Height, finalSize.Height);
                Rect arrangeRect = new Rect(currentX, 0, childWidth, childHeight);
                child.Arrange(arrangeRect);
                currentX += childWidth + ItemSpacing;
            }
        }
        LogManager.LogDebug($"UniformSpacingPanel.ArrangeOverride returning finalSize = {finalSize}");
        return finalSize;
    }
    public void LineDown()
    {

    }

    public void LineLeft()
    {
        SetHorizontalOffset(HorizontalOffset - 10);
    }

    public void LineRight()
    {
        SetHorizontalOffset(HorizontalOffset + 10);
    }

    public void LineUp()
    {

    }

    public Rect MakeVisible(Visual visual, Rect rectangle)
    {
        return rectangle;
    }

    public void MouseWheelDown()
    {
        SetHorizontalOffset(HorizontalOffset + 50);
    }

    public void MouseWheelLeft()
    {
        SetHorizontalOffset(HorizontalOffset - 50);
    }

    public void MouseWheelRight()
    {
        SetHorizontalOffset(HorizontalOffset + 50);
    }

    public void MouseWheelUp()
    {
        SetHorizontalOffset(HorizontalOffset - 50);
    }

    public void PageDown()
    {

    }

    public void PageLeft()
    {
        SetHorizontalOffset(HorizontalOffset - ViewportWidth);
    }

    public void PageRight()
    {
        SetHorizontalOffset(HorizontalOffset + ViewportWidth);
    }

    public void PageUp()
    {

    }

    public void SetHorizontalOffset(double offset)
    {
        double limitOffset = Math.Max(0, Math.Min(offset, ExtentWidth - ViewportWidth));
        if (HorizontalOffset != limitOffset)
        {
            HorizontalOffset = limitOffset;
            InvalidateArrange();
            ScrollOwner?.InvalidateScrollInfo();
        }
    }

    public void SetVerticalOffset(double offset)
    {

    }
}
