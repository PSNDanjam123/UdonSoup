
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace UdonSoup.Component.Data
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public abstract class StateLocal : State
    {
        public override void Sync()
        {
            Publish();
        }
    }
}