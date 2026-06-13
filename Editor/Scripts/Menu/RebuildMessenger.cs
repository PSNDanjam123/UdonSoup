
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UdonSharpEditor;
using UdonSoup.Editor.Utility;
using UdonSoup.Utility;
using UnityEngine;
using UEditor = UnityEditor.Editor;

public class RebuildMessenger : UEditor
{
    static Messenger Messenger;
    public static void Handle()
    {
        Messenger = SceneFinder.FindAllOfType<Messenger>().FirstOrDefault();
        clearHandlers();
        setHandlers();
        setupEvents();
    }
    private static void clearHandlers()
    {
        var children = (UdonSoup.Component.Data.Message[])typeof(Messenger).GetField("messageHandlers", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy).GetValue(Messenger);
        foreach (var child in children)
        {
            if (child == null) continue;
            GameObject.DestroyImmediate(child.gameObject);
        }
    }

    private static void setHandlers()
    {

        List<UdonSoup.Component.Data.Message> messages = new List<UdonSoup.Component.Data.Message>();

        var maxCapacity = SceneFinder.UdonSoupManager.MaxCapacity;

        if (maxCapacity < 1)
        {

        }

        for (var i = 0; i < maxCapacity; i++)
        {
            var obj = new GameObject("Message_" + i);
            obj.transform.SetParent(Messenger.transform);
            var comp = obj.AddUdonSharpComponent<UdonSoup.Component.Data.Message>();
            messages.Add(comp);
        }

        var flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
        typeof(Messenger).GetField("messageHandlers", flags).SetValue(Messenger, messages.ToArray());
        typeof(Messenger).GetField("handlerOwners", flags).SetValue(Messenger, new int[messages.Count]);
        UnityEditor.EditorUtility.SetDirty(Messenger);
    }

    private static void setupEvents()
    {
        var children = (UdonSoup.Component.Data.Message[])typeof(Messenger).GetField("messageHandlers", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy).GetValue(Messenger);
        foreach (var child in children)
        {
            if (child == null) continue;
            child.Subscribe(Messenger);
            UnityEditor.EditorUtility.SetDirty(Messenger);
        }
    }

}
