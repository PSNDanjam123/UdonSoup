
using UnityEngine;

namespace UdonSoup.Component.Data
{
    public class StateCollection : DatabaseElement
    {
        [SerializeField, HideInInspector]
        State[] states; public State[] States => states;
    }
}