
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UEditor = UnityEditor.Editor;
using UdonSoup.Editor.Utility;
using UdonSharp;
using UdonSharpEditor;
using System.Linq;
using System.Reflection;

namespace UdonSoup.Editor.Menu
{

    public class HandleAttributes : UEditor
    {
        public static void Handle()
        {

            var udons = SceneFinder.FindAllOfType<UdonSharpBehaviour>();
            foreach (var udon in udons)
            {
                handleSingleton(udon);
            }

        }

        static void handleSingleton(UdonSharpBehaviour udon)
        {

            var type = udon.GetType();
            var attr = type.GetCustomAttributes(true).Where(a => a.GetType() == typeof(UdonSoup.Attribute.Singleton)).FirstOrDefault();
            if (attr == null)
            {
                return; // Not a singleton
            }

            var deps = SceneFinder.FindAllOfType<UdonSharpBehaviour>().Where(u => u.GetType() != type);
            foreach (var dep in deps)
            {
                var depType = dep.GetType();
                var fields = depType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
                foreach (var field in fields)
                {
                    var fieldType = field.FieldType;
                    if (fieldType == type)
                    {
                        field.SetValue(dep, udon);
                        EditorUtility.SetDirty(dep);
                    }
                }
            }
        }


    }
}