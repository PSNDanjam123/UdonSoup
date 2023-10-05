
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace UdonSoup.Component.Data
{
    public class DatabaseElement : UdonSoup.Core.Network
    {
        [SerializeField, HideInInspector]
        int databaseIndex; public int DatabaseIndex => databaseIndex;

        [SerializeField, HideInInspector]
        string databaseKey; public string DatabaseKey => databaseKey;

        public DatabaseElement FindChild(string key)
        {
            return Database.FindChild(this, key);
        }

        public DatabaseElement FindParent(int level = 1)
        {
            return Database.FindParent(this, level);
        }

        public virtual void SoupPostDatabaseSetup()
        {
        }
    }
}