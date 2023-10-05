
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UdonSoup.Attribute;
using UdonSoup.Component.Data;
using VRC.SDK3.Data;

namespace UdonSoup.Utility
{
    [Singleton]
    public class Messenger : UdonSoup.Component.Data.State
    {
        [SerializeField, HideInInspector]
        Message[] messageHandlers = new Message[0];

        [SerializeField, HideInInspector, UdonSynced]
        int[] handlerOwners = new int[0];

        Message messageHandler;

        public void Send(DataDictionary message, bool local = false, bool masterOnly = false)
        {
            getOwnHandler().Send(message, null, local, masterOnly);
        }

        public override void OnMessage(Message messageObject, DataDictionary data)
        {
            var index = 0;
            for (index = 0; index < messageHandlers.Length; index++)
            {
                if (messageObject == messageHandlers[index])
                {
                    break;
                }
            }
            var header = data["h"].DataDictionary;
            var content = data["d"].DataDictionary;
            foreach (var Subscribe in Subscribers)
            {
                Subscribe.OnMessenger(handlerOwners[index], content);
            }
        }

        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            if (!LocalPlayer.isMaster)
            {
                return; // only master can assign
            }
            SetOwner();
            for (var i = 0; i < handlerOwners.Length; i++)
            {
                if (handlerOwners[i] == 0)
                {
                    handlerOwners[i] = player.playerId;
                    messageHandlers[i].Clear();
                    break;
                }
            }
            Sync();
        }

        public override void OnPlayerLeft(VRCPlayerApi player)
        {
            if (!LocalPlayer.isMaster)
            {
                return; // only master can assign
            }
            SetOwner();
            for (var i = 0; i < handlerOwners.Length; i++)
            {
                if (handlerOwners[i] == player.playerId)
                {
                    handlerOwners[i] = 0;
                    messageHandlers[i].Clear();
                    break;
                }
            }
            Sync();
            setOwnHandler();
        }

        public override void OnDeserialization()
        {
            setOwnHandler();
        }

        private void setOwnHandler()
        {
            if (messageHandler != null)
            {
                return;
            }
            for (var i = 0; i < handlerOwners.Length; i++)
            {
                if (handlerOwners[i] == LocalPlayerId)
                {
                    messageHandler = messageHandlers[i];
                    break;
                }
            }
        }

        private Message getOwnHandler()
        {
            if (messageHandler != null)
            {
                return messageHandler;
            }
            for (var i = 0; i < handlerOwners.Length; i++)
            {
                if (handlerOwners[i] == LocalPlayerId)
                {
                    return messageHandlers[i];
                }
            }
            return null;
        }
        protected override void Publish()
        {
            // Not used
        }


    }
}