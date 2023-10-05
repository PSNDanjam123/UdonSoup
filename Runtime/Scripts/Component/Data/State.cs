
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace UdonSoup.Component.Data
{
    public abstract class State : DatabaseElement
    {

        [SerializeField, HideInInspector]
        UdonSoup.Core.Base[] subscribers = new UdonSoup.Core.Base[0]; protected UdonSoup.Core.Base[] Subscribers => subscribers;

        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            Publish();
        }

        public void ClearSubscriptions()
        {
            subscribers = new UdonSoup.Component.View[0];
        }

        public void Subscribe(UdonSoup.Core.Base reciever)
        {
            var newSubs = new UdonSoup.Core.Base[subscribers.Length + 1];
            for (var i = 0; i < subscribers.Length; i++)
            {
                newSubs[i] = subscribers[i];
            }
            newSubs[subscribers.Length] = reciever;
            subscribers = newSubs;
        }

        public override void Sync()
        {
            base.Sync();
            Publish();
        }

        protected virtual void Publish()
        {
            for (var i = 0; i < subscribers.Length; i++)
            {
                subscribers[i].OnStateChange(this);
            }
        }

        public override void OnDeserialization()
        {
            base.OnDeserialization();
            Publish();
        }

    }
}