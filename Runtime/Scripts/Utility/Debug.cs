
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UdonSoup.Attribute;

namespace UdonSoup.Utility
{
    [Singleton, UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class Debug : UdonSharpBehaviour
    {
        [SerializeField]
        string headerText = "UdonSoup";

        public void Log(object message)
        {
            UnityEngine.Debug.Log("[<color=lightblue>" + headerText + "</color>]: " + message);
        }
    }
}