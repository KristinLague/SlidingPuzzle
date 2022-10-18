using UnityEngine.UIElements;

public class UITKEventHelper
{
    private event System.Action onUnregisterAll;
    
    public void RegisterCallback<T>(VisualElement root, string handlerName, EventCallback<T> callback) where T : EventBase<T>, new()
    {
        var eventHandler = root.Q<VisualElement>(handlerName);
        eventHandler.RegisterCallback(callback);
        onUnregisterAll += () => eventHandler.UnregisterCallback(callback);
    }
    
    public void RegisterCallback<T>(CallbackEventHandler eventHandler, EventCallback<T> callback) where T : EventBase<T>, new()
    {
        eventHandler.RegisterCallback(callback);
        onUnregisterAll += () => eventHandler.UnregisterCallback(callback);
    }

    public void UnregisterCallback<T>(CallbackEventHandler eventHandler, EventCallback<T> callback) where T : EventBase<T>, new()
    {
        eventHandler.UnregisterCallback(callback);
    }

    public void RegisterValueChangedCallback<T>(INotifyValueChanged<T> control, EventCallback<ChangeEvent<T>> callback)
    {
        CallbackEventHandler eventHandler = control as CallbackEventHandler;
        eventHandler.RegisterCallback(callback);
        onUnregisterAll += () => eventHandler.UnregisterCallback(callback);
    }

    public void UnregisterAllCallbacks()
    {
        onUnregisterAll?.Invoke();
        onUnregisterAll = null;
    }
}
