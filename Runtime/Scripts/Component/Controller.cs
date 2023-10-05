
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UdonSoup.Attribute;
using VRC.SDK3.Data;

namespace UdonSoup.Component
{
    [Singleton]
    public abstract class Controller : UdonSoup.Core.Base
    {
        public abstract void OnRoute(string route, int fromPlayerId, DataDictionary data);
    }
}