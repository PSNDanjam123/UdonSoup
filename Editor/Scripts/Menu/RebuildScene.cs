using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UEditor = UnityEditor.Editor;
using UdonSoup.Editor.Utility;
using UdonSharpEditor;

namespace UdonSoup.Editor.Menu
{
    public class RebuildScene : UEditor
    {
        public static void Handle()
        {
            addUdonSoup();
            addDatabase();
            addRouter();
            addController();
        }

        private static void addUdonSoup()
        {
            if (sceneHas("UdonSoup"))
            {
                return;
            }
            var obj = (GameObject)Resources.Load("UdonSoup");
            var go = Instantiate(obj, new Vector3(0, 0, 0), Quaternion.identity);
            go.name = "UdonSoup";
            go.transform.SetAsFirstSibling();
        }

        private static void addDatabase()
        {
            if (sceneHas("Database"))
            {
                return;
            }
            var go = new GameObject("Database");
            go.AddUdonSharpComponent<UdonSoup.Component.Data.Database>();
        }

        private static void addRouter()
        {
            if (sceneHas("Router"))
            {
                return;
            }
            new GameObject("Router");
        }

        private static void addController()
        {
            if (sceneHas("Controllers"))
            {
                return;
            }
            new GameObject("Controllers");
        }

        private static bool sceneHas(string name)
        {
            var rootGos = SceneFinder.GetRootGameObjects();
            foreach (var rootGo in rootGos)
            {
                if (rootGo.name == name)
                {
                    return true;
                }
            }
            return false;
        }

    }
}