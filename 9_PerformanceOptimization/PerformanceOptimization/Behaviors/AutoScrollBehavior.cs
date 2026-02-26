using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PerformanceOptimization.Behaviors;

public static class AutoScrollBehavior
{
    public static readonly DependencyProperty EnableProperty =
        DependencyProperty.RegisterAttached(
            "Enable",
            typeof(bool),
            typeof(AutoScrollBehavior),
            new PropertyMetadata(false, OnEnableChanged)
            );

    public static void SetEnable(DependencyObject element, bool value) => element.SetValue(EnableProperty, value);
    public static bool GetEnable(DependencyObject element) => (bool)element.GetValue(EnableProperty);
    private static void OnEnableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ItemsControl itemsControl)
        {
            if ((bool)e.NewValue)
            {
                itemsControl.Loaded += ItemsControl_Loaded;
            }
            else
            {
                itemsControl.Loaded -= ItemsControl_Loaded;
            }
        }
    }

    private static void ItemsControl_Loaded(object sender, RoutedEventArgs e)
    {
        var itemsControl = sender as ItemsControl;
        if (itemsControl == null)
        {
            return;
        }

        var scrollViewer = FindScrollViewer(itemsControl);
        if (scrollViewer == null) return;
        bool autoScroll = true;

        scrollViewer.ScrollChanged += (s, args) =>
        {
            autoScroll = scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight - 1;
        };

        if (itemsControl.ItemsSource is INotifyCollectionChanged collection)
        {
            collection.CollectionChanged += (s, args) =>
            {
                if (!autoScroll) return;
                itemsControl.Dispatcher.BeginInvoke(new Action(() =>
                {
                    scrollViewer.ScrollToEnd();
                }), System.Windows.Threading.DispatcherPriority.Background);
            };
        }
    }

    private static ScrollViewer FindScrollViewer(DependencyObject d)
    {
        if (d is ScrollViewer sv) return sv;
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(d); i++)
        {
            var child = VisualTreeHelper.GetChild(d, i);
            var result = FindScrollViewer(child);
            if (result != null)
            {
                return result;
            }
        }
        return null!;
    }
}
