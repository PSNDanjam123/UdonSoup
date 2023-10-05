
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UdonSoup.Attribute;

namespace UdonSoup.Component.Data
{
    [Singleton]
    public class Database : UdonSoup.Core.Base
    {
        [SerializeField, HideInInspector]
        string[] routeKeys;

        [SerializeField, HideInInspector]
        GameObject[] routeObjects;

        public DatabaseElement Find(string key)
        {
            for (var i = 0; i < routeKeys.Length; i++)
            {
                if (routeKeys[i] == key)
                {
                    return routeObjects[i].GetComponent<DatabaseElement>();
                }
            }
            return null;
        }

        public DatabaseElement FindByIndex(int index)
        {
            return routeObjects[index].GetComponent<DatabaseElement>();
        }

        public DatabaseElement FindChild(DatabaseElement parent, string key)
        {
            return Find(parent.DatabaseKey + "." + key);
        }

        public DatabaseElement FindParent(DatabaseElement child, int level = 1)
        {
            string key = "";
            var keySections = child.DatabaseKey.Split('.');
            for (var i = 0; i < keySections.Length - level; i++)
            {
                if (key == "")
                {
                    key = keySections[i];
                }
                else
                {
                    key += "." + keySections[i];
                }
            }
            if (key == "")
            {
                return null;
            }
            return Find(key);
        }

    }
}