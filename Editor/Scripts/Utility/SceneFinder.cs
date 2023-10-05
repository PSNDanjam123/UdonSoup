
using UnityEngine;
using UnityEngine.SceneManagement;
using UEditor = UnityEditor.Editor;
using System.Collections.Generic;
using VRC.SDKBase.Editor.Api;
using VRC.SDK3.Editor;
using System.Reflection;

namespace UdonSoup.Editor.Utility
{

    public class SceneFinder : UEditor
    {
        static Scene Scene;

        static VRCSdkControlPanel controlPanel;

        public static VRCSdkControlPanel GetVRCControlPanel()
        {
            if (controlPanel == null)
            {
                controlPanel = (VRCSdkControlPanel)UnityEditor.EditorWindow.GetWindow(typeof(VRCSdkControlPanel));
            }
            return controlPanel;
        }

        public static VRCWorld GetVRCWorldData()
        {
            controlPanel = GetVRCControlPanel();
            VRCSdkControlPanel.TryGetBuilder<IVRCSdkWorldBuilderApi>(out IVRCSdkWorldBuilderApi _interface);
            var _builder = (VRCSdkControlPanelWorldBuilder)_interface;
            return (VRCWorld)getInstanceField(_builder, "_worldData");
        }



        public static List<T> FindAllOfType<T>()
        {
            init();
            List<T> output = new List<T>();

            var rootGos = GetRootGameObjects();
            foreach (var rootGo in rootGos)
            {
                output.AddRange(rootGo.GetComponentsInChildren<T>(true));
            }
            return output;
        }

        static void init()
        {
            Scene = SceneManager.GetActiveScene();
        }

        public static GameObject[] GetRootGameObjects()
        {
            init();
            return Scene.GetRootGameObjects();
        }
        private static System.Object getInstanceField<T>(T instance, string fieldName)
        {
            var bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            var field = typeof(T).GetField(fieldName, bindFlags);
            return field.GetValue(instance);
        }
    }
}