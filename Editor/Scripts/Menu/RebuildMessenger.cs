
using UEditor = UnityEditor.Editor;
using UdonSoup.Utility;
using UdonSoup.Editor.Utility;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;
using VRC.SDKBase;
using UnityEngine;
using UdonSharpEditor;


public class RebuildMessenger : UEditor
{
    static int maxCapacity = 32;
    static Messenger Messenger;
    static VRC_SceneDescriptor SceneDescriptor;

    public static void Handle()
    {
        if (!init())
        {
            return;
        }
        setCapacity();
        clearHandlers();
        setHandlers();
        setupEvents();
    }

    private static void setCapacity()
    {
        var worldData = SceneFinder.GetVRCWorldData();
        if (worldData.Capacity == 0)
        {
            Debug.LogWarning("UdonSoup requires the VRC Control Panel is shown with the \"Builder\" tab selected");
            throw new UnityException();
        }
        maxCapacity = worldData.Capacity;
    }

    private static void clearHandlers()
    {
        var children = (UdonSoup.Component.Data.Message[])typeof(Messenger).GetField("messageHandlers", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy).GetValue(Messenger);
        foreach (var child in children)
        {
            if (child == null)
            {
                continue;
            }
            GameObject.DestroyImmediate(child.gameObject);
        }
    }

    private static void setHandlers()
    {

        List<UdonSoup.Component.Data.Message> messages = new List<UdonSoup.Component.Data.Message>();

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
            if (child == null)
            {
                continue;
            }
            child.Subscribe(Messenger);
            UnityEditor.EditorUtility.SetDirty(Messenger);
        }
    }


    private static bool init()
    {
        Messenger = SceneFinder.FindAllOfType<Messenger>().FirstOrDefault();
        SceneDescriptor = SceneFinder.FindAllOfType<VRC_SceneDescriptor>().FirstOrDefault();
        return Messenger != null && SceneDescriptor != null;
    }
}
