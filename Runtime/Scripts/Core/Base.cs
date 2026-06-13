
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;

namespace UdonSoup.Core
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public abstract class Base : UdonSharpBehaviour
    {
        [SerializeField, HideInInspector]
        protected UdonSoup.Component.Data.Database Database;

        [SerializeField, HideInInspector]
        protected UdonSoup.Utility.Messenger Messenger;

        VRC.SDKBase.VRCPlayerApi localPlayer; protected VRC.SDKBase.VRCPlayerApi LocalPlayer
        {
            get
            {
                if (localPlayer == null)
                {
                    localPlayer = VRC.SDKBase.Networking.LocalPlayer;
                }
                return localPlayer;
            }
        }

        int localPlayerId; protected int LocalPlayerId
        {
            get
            {
                if (localPlayerId == 0)
                {
                    localPlayerId = LocalPlayer.playerId;
                }
                return localPlayerId;
            }
        }

        public virtual void SoupSetupSubscriptions()
        {
        }

        public virtual void OnStateChange(UdonSoup.Component.Data.State state)
        {
        }

        public virtual void OnEvent(UdonSoup.Component.Data.Event eventObject)
        {
        }

        public virtual void OnMessage(UdonSoup.Component.Data.Message messageObject, DataDictionary data)
        {
        }

        public virtual void OnMessenger(int fromPlayerId, DataDictionary data)
        {
        }
    }
}