
using System.Linq;
using UdonSoup.Component;
using UdonSoup.Editor.Utility;
using UEditor = UnityEditor.Editor;

namespace UdonSoup.Editor.Menu
{
    public class RebuildRouter : UEditor
    {
        static Router Router;

        public static void Handle()
        {
            if (!init())
            {
                return;
            }
            clear();
            Router.SoupDefineRoutes();
            UnityEditor.EditorUtility.SetDirty(Router);
        }

        private static bool init()
        {
            Router = SceneFinder.FindAllOfType<Router>().FirstOrDefault();
            return Router != null;
        }

        private static void clear()
        {
            Router.ClearRoutes();
        }
    }
}