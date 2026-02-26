using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace PerformanceOptimization.Utilities.Collections;

public class BulkObservableCollection<T> : ObservableCollection<T>
{
    private bool _suppress;
    public void AddRange(IEnumerable<T> items)
    {
        _suppress = true;
        foreach (var item in items)
        {
            Items.Add(item);
        }
        _suppress = false;
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }
    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        if (!_suppress)
            base.OnCollectionChanged(e);
    }
}
