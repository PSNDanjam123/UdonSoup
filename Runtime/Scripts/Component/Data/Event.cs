
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Data;
using VRC.Udon.Common;
using VRC.Udon.Common.Interfaces;

namespace UdonSoup.Component.Data
{
    public class Event : DatabaseElement
    {
        [SerializeField]
        bool sendLocalOnly = false;

        [SerializeField]
        bool sendMasterOnly = false;

        [SerializeField, HideInInspector]
        UdonSoup.Core.Base[] subscribers = new UdonSoup.Core.Base[0]; protected UdonSoup.Core.Base[] Subscribers => subscribers;

        public void Send()
        {
            if (sendLocalOnly)
            {
                Receive();
                return;
            }
            SendCustomNetworkEvent(NetworkEventTarget.All, "Receive");
        }

        public void Receive()
        {
            if (sendMasterOnly && !LocalPlayer.isMaster)
            {
                return; // master only
            }
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

        void Publish()
        {
            foreach (var subscriber in subscribers)
            {
                subscriber.OnEvent(this);
            }
        }
    }
}