using LayoutSystemDemo.Logging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace LayoutSystemDemo.Controls;

public class SimpleVirtualizingPanel : VirtualizingPanel, IScrollInfo
{
    private const double ItemHeight = 30.0;
    public bool CanHorizontallyScroll { get; set; } = false;
    public bool CanVerticallyScroll { get; set; } = true;

    public double ExtentHeight { get; set; }

    public double ExtentWidth { get; set; }

    public double HorizontalOffset { get; set; }

    public ScrollViewer? ScrollOwner { get; set; }

    public double VerticalOffset { get; set; }

    public double ViewportHeight { get; set; }

    public double ViewportWidth { get; set; }

    protected override Size MeasureOverride(Size availableSize)
    {
        LogManager.LogDebug($"SimpleVirtualizingPanel.MeasureOverride: called with availableSize = {availableSize}");
        ItemsControl itemsControl = ItemsControl.GetItemsOwner(this);
        if (itemsControl == null) return new Size(0, 0);
        IItemContainerGenerator generatorInterface = this.ItemContainerGenerator;
        ItemContainerGenerator concreteGenerator = itemsControl.ItemContainerGenerator;
        if (availableSize.Height == double.PositiveInfinity) return availableSize;
        int itemCount = itemsControl.Items.Count;

        ViewportHeight = availableSize.Height;
        ViewportWidth = availableSize.Width;
        ExtentHeight = itemCount * ItemHeight;
        ExtentWidth = availableSize.Width;

        if (VerticalOffset > ExtentHeight - ViewportHeight)
        {
            VerticalOffset = Math.Max(0, ExtentHeight - ViewportHeight);
        }

        int firstVisibleIndex = (int)Math.Floor(VerticalOffset / ItemHeight);
        int maxVisibleCount = (int)Math.Ceiling(VerticalOffset / ItemHeight) + 1;
        int lastVisibleIndex = Math.Min(itemCount - 1, firstVisibleIndex * maxVisibleCount);

        if (itemCount == 0) return new Size(0, 0);

        for (int i = InternalChildren.Count - 1; i >= 0; i--)
        {
            UIElement child = InternalChildren[i];
            int index = concreteGenerator.IndexFromContainer(child);
            if (index < firstVisibleIndex || index > lastVisibleIndex)
            {
                RemoveInternalChildRange(i, 1);
                GeneratorPosition pos = generatorInterface.GeneratorPositionFromIndex(index);
                generatorInterface.Remove(pos, 1);
            }
        }

        GeneratorPosition startPos = generatorInterface.GeneratorPositionFromIndex(firstVisibleIndex);
        int childIndex = (startPos.Offset == 0) ? startPos.Index : startPos.Index + 1;
        using (generatorInterface.StartAt(startPos, GeneratorDirection.Forward, true))
        {
            UIElement child = generatorInterface.GenerateNext(out bool isNew) as UIElement;
            if (child != null)
            {
                if (isNew)
                {
                    if (childIndex >= InternalChildren.Count)
                    {
                        AddInternalChild(child);
                    }
                    else InsertInternalChild(childIndex, child);
                    generatorInterface.PrepareItemContainer(child);
                }
            }
        }
        ScrollOwner?.InvalidateScrollInfo();
        LogManager.LogDebug($"SimpleVirtualizingPanel.MeasureOverride: returning availableSize = {availableSize}");
        return availableSize;
    }
    protected override Size ArrangeOverride(Size finalSize)
    {
        LogManager.LogDebug($"SimpleVirtualizingPanel.ArrangeOverride: called with finalSize = {finalSize}");
        ItemsControl itemsControl = ItemsControl.GetItemsOwner(this);
        if (itemsControl == null) return finalSize;
        ItemContainerGenerator concreteGenerator = itemsControl.ItemContainerGenerator;
        foreach (UIElement child in InternalChildren)
        {
            int index = concreteGenerator.IndexFromContainer(child);
            if (index != -1)
            {
                double y = (index * ItemHeight) - VerticalOffset;
                child.Arrange(new Rect(0, y, finalSize.Width, ItemHeight));
            }
        }
        LogManager.LogDebug($"SimpleVirtualizingPanel.ArrangeOverride: returning finalSize = {finalSize}");
        return finalSize;
    }
    public void LineDown()
    {
        SetVerticalOffset(VerticalOffset + ItemHeight);
    }

    public void LineLeft()
    {

    }

    public void LineRight()
    {

    }

    public void LineUp()
    {
        SetVerticalOffset(VerticalOffset - ItemHeight);
    }

    public Rect MakeVisible(Visual visual, Rect rectangle)
    {
        return rectangle;
    }

    public void MouseWheelDown()
    {
        SetVerticalOffset(VerticalOffset + 40);
    }

    public void MouseWheelLeft()
    {

    }

    public void MouseWheelRight()
    {

    }

    public void MouseWheelUp()
    {
        SetVerticalOffset(VerticalOffset - 40);
    }

    public void PageDown()
    {
        SetVerticalOffset(VerticalOffset + ViewportHeight);
    }

    public void PageLeft()
    {

    }

    public void PageRight()
    {

    }

    public void PageUp()
    {
        SetVerticalOffset(VerticalOffset - ViewportHeight);
    }

    public void SetHorizontalOffset(double offset)
    {

    }

    public void SetVerticalOffset(double offset)
    {
        double limitOffset = Math.Max(0, Math.Min(offset, ExtentHeight - ViewportHeight));
        if (VerticalOffset != limitOffset)
        {
            VerticalOffset = limitOffset;
            InvalidateMeasure();
        }
    }
}
