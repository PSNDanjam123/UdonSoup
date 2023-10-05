using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UEditor = UnityEditor.Editor;
using UnityEditor;

namespace UdonSoup.Editor.Menu
{
    public class Rebuild : UEditor
    {
        [MenuItem("UdonSoup/Rebuild")]
        public static void Handle()
        {
            RebuildScene.Handle();
            HandleAttributes.Handle();
            RebuildDatabase.Handle();
            HandleSubscriptions.Handle();
            RebuildMessenger.Handle();
            RebuildRouter.Handle();
        }
    }
}