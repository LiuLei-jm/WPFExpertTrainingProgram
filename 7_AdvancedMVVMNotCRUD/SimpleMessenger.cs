namespace AdvancedMVVMNotCRUD;

public record DataDownloadedMessage(string data);

public static class SimpleMessenger
{
    private static Dictionary<Type, List<Delegate>> _subscribers = new();

    public static void Subscribe<TMessage>(Action<TMessage> action)
    {
        var type = typeof(TMessage);
        if (!_subscribers.ContainsKey(type))
            _subscribers[type] = new List<Delegate>();
        _subscribers[type].Add(action);
    }

    public static void Send<TMessage>(TMessage message)
    {
        var type = typeof(TMessage);
        if (_subscribers.TryGetValue(type, out var actions))
        {
            foreach (var action in actions)
            {
                ((Action<TMessage>)action).Invoke(message);
            }
        }
    }
}
