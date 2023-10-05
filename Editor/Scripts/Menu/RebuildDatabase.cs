using System.Collections;
using System.Collections.Generic;
using UEditor = UnityEditor.Editor;
using UnityEngine;
using UdonSoup.Editor.Utility;
using System.Linq;
using System.Reflection;
using UdonSoup.Component.Data;

namespace UdonSoup.Editor.Menu
{
    public class RebuildDatabase : UEditor
    {
        static Database database;
        static Dictionary<string, GameObject> routes;
        public static void Handle()
        {
            if (!init())
            {
                return;
            }
            mapRoutes(database.transform, "", true);
            updateDatabaseValues();
            updateStateValues();
            executePostDatabaseUpdate();
        }

        private static bool init()
        {
            database = SceneFinder.FindAllOfType<Database>().FirstOrDefault();
            routes = new Dictionary<string, GameObject>();
            return database != null;
        }

        private static void executePostDatabaseUpdate()
        {
            foreach (var route in routes)
            {
                var dbElem = route.Value.GetComponent<DatabaseElement>();
                if (dbElem == null)
                {
                    continue;
                }
                dbElem.SoupPostDatabaseSetup();
            }
        }

        private static void updateStateValues()
        {
            var keys = routes.Keys.ToArray();
            for (var i = 0; i < keys.Length; i++)
            {
                var key = keys[i];
                var route = routes[keys[i]];

                var dbElem = route.GetComponent<DatabaseElement>();

                if (dbElem == null)
                {
                    continue;
                }

                typeof(DatabaseElement).GetField("databaseKey", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).SetValue(dbElem, key);
                typeof(DatabaseElement).GetField("databaseIndex", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).SetValue(dbElem, i);

                var collection = dbElem.GetComponent<StateCollection>();
                if (collection == null)
                {
                    continue;
                }
                var states = route.transform.GetComponentsInChildren<State>().Where(s => s.transform.parent == route.transform).ToArray();
                typeof(StateCollection).GetField("states", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).SetValue(dbElem, states);
            }
        }

        private static void updateDatabaseValues()
        {
            var type = database.GetType();
            type.GetField("routeKeys", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(database, routes.Keys.ToArray<string>());
            type.GetField("routeObjects", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(database, routes.Values.ToArray<GameObject>());
        }

        private static void mapRoutes(Transform transform, string prefix = "", bool root = false)
        {
            if (!root)
            {
                if (prefix == "")
                {
                    prefix = transform.name;
                }
                else
                {
                    prefix += "." + transform.name;
                }
                routes.Add(prefix, transform.gameObject);
            }
            var childCount = transform.childCount;
            for (var i = 0; i < childCount; i++)
            {
                var child = transform.GetChild(i);
                mapRoutes(child.transform, prefix);
            }
        }
    }
}