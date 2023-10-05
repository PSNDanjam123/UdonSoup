
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Data;
using VRC.Udon.Common;

namespace UdonSoup.Component.Data
{
    public class Message : State
    {

        [UdonSynced]
        string networkData = "";

        [UdonSynced]
        int lastRequestId = 0;

        DataList requestPool = new DataList();
        DataList responsePool = new DataList();

        int lastHandledRequestId = 0;

        protected DataDictionary Data = new DataDictionary();

        public override void OnPreSerialization()
        {
            base.OnPreSerialization();
            if (!VRCJson.TrySerializeToJson(requestPool, JsonExportType.Minify, out DataToken res))
            {
                return;
            }
            networkData = res.String;
        }

        public override void OnPostSerialization(SerializationResult result)
        {
            base.OnPostSerialization(result);
            requestPool.Clear();
        }

        public override void OnDeserialization()
        {
            if (!VRCJson.TryDeserializeFromJson(networkData, out DataToken res))
            {
                return;
            }
            responsePool = res.DataList;
            base.OnDeserialization();
        }

        public void Clear()
        {
            SetOwner();
            networkData = "";
            Sync();
        }

        public virtual void Send(DataDictionary message, DataDictionary header = null, bool local = false, bool masterOnly = false)
        {
            if (header == null)
            {
                header = new DataDictionary();
            }
            var data = new DataDictionary();
            data["i"] = ++lastRequestId;
            data["m"] = masterOnly;
            data["h"] = header;
            data["d"] = message;
            if (local || (LocalPlayer.isMaster && masterOnly))
            {
                lastHandledRequestId = lastRequestId;
                publishLocal(data);
                return;
            }
            SetOwner();
            requestPool.Add(data);

            // Send events to self
            responsePool = requestPool.DeepClone();

            // If no other players clear the request pool
            if (VRCPlayerApi.GetPlayerCount() == 1)
            {
                requestPool.Clear();
            }
            Sync();
        }

        protected override void Publish()
        {
            DataToken[] responses = responsePool.ToArray();

            foreach (var response in responses)
            {
                var data = response.DataDictionary;
                var requestId = (int)data["i"].Double;
                if (requestId <= lastHandledRequestId)
                {
                    continue;   // already handled, skip
                }
                lastHandledRequestId = requestId;
                if (data["m"].Boolean && !LocalPlayer.isMaster)
                {
                    continue; // this message is for master only
                }
                for (var i = 0; i < Subscribers.Length; i++)
                {
                    Subscribers[i].OnMessage(this, response.DataDictionary);
                }
            }
        }

        private void publishLocal(DataDictionary data)
        {
            for (var i = 0; i < Subscribers.Length; i++)
            {
                Subscribers[i].OnMessage(this, data);
            }
        }
    }
}