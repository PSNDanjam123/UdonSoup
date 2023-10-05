
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UdonSoup.Component.Data;

namespace UdonSoup.Component.Data
{
    public class StateCollection : DatabaseElement
    {
        [SerializeField, HideInInspector]
        State[] states; public State[] States => states;
    }
}