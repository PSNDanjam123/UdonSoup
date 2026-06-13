
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UEditor = UnityEditor.Editor;

namespace UdonSoup.Editor.Utility
{

    public class SceneFinder : UEditor
    {
        static Scene Scene;

        public static List<T> FindAllOfType<T>()
        {
            init();
            List<T> output = new List<T>();
            var rootGos = GetRootGameObjects();
            foreach (var rootGo in rootGos) output.AddRange(rootGo.GetComponentsInChildren<T>(true));
            return output;
        }

        static void init() => Scene = SceneManager.GetActiveScene();
        public static GameObject[] GetRootGameObjects()
        {
            init();
            return Scene.GetRootGameObjects();
        }

        public static UdonSoupManager UdonSoupManager
        {
            get
            {
                init();
                var manager = GetRootGameObjects()
                    .Where(x => x.GetComponent<UdonSoupManager>() != null)
                    .Select(x => x.GetComponent<UdonSoupManager>())
                    .FirstOrDefault();
                if (!manager) throw new UnityException("UdonSoupManager is missing from active Scene");
                return manager;
            }
        }
    }
}