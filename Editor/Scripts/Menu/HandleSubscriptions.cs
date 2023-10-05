using System.Collections;
using System.Collections.Generic;
using UEditor = UnityEditor.Editor;
using UnityEngine;
using System.Linq;
using UdonSoup.Editor.Utility;
using UdonSoup.Core;
using UdonSoup.Component.Data;

public class HandleSubscriptions : UEditor
{
    public static void Handle()
    {
        SceneFinder.FindAllOfType<State>().ForEach(x => x.ClearSubscriptions());
        SceneFinder.FindAllOfType<UdonSoup.Component.Data.Event>().ForEach(x => x.ClearSubscriptions());

        var rootGos = SceneFinder.GetRootGameObjects();
        handleSetupSubs(rootGos);
    }

    private static void handleSetupSubs(GameObject[] gameObjects)
    {
        foreach (var gameObject in gameObjects)
        {
            var comp = gameObject.GetComponent<Base>();
            if (comp != null)
            {
                comp.SoupSetupSubscriptions();
            }
            var children = gameObject.GetComponentsInChildren<Transform>().Where(x => x.parent == gameObject.transform).Select(x => x.gameObject).ToArray();
            handleSetupSubs(children);
        }
    }
}
