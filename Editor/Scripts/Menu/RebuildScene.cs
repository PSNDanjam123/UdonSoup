using System.Linq;
using UdonSharpEditor;
using UdonSoup.Editor.Utility;
using UnityEditor;
using UnityEngine;
using UEditor = UnityEditor.Editor;

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
            if (sceneHas("UdonSoup")) return;
            var prefab = Resources.Load("UdonSoup");
            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            instance.transform.SetAsFirstSibling();
            instance.name = "UdonSoup";
        }

        private static void addDatabase()
        {
            if (sceneHas("Database")) return;
            var go = new GameObject("Database");
            go.AddUdonSharpComponent<UdonSoup.Component.Data.Database>();
        }

        private static void addRouter()
        {
            if (sceneHas("Router")) return;
            new GameObject("Router");
        }

        private static void addController()
        {
            if (sceneHas("Controllers")) return;
            new GameObject("Controllers");
        }

        private static bool sceneHas(string name) => SceneFinder.GetRootGameObjects().Any(x => x.name == name);

    }
}