
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UdonSoup.Attribute;
using VRC.SDK3.Data;
using UdonSoup.Component.Data;

namespace UdonSoup.Component
{
    [Singleton]
    public abstract class Router : UdonSoup.Core.Base
    {
        [SerializeField, HideInInspector]
        string[] routes = new string[0];

        [SerializeField, HideInInspector]
        Controller[] controllers = new Controller[0];

        public override void SoupSetupSubscriptions()
        {
            Messenger.Subscribe(this);
        }

        public override void OnMessenger(int fromPlayerId, DataDictionary data)
        {
            if (!data.ContainsKey("for") || data["for"] != "router")
            {
                return;
            }
            for (var i = 0; i < routes.Length; i++)
            {
                if (routes[i] == data["r"].String)
                {
                    DataDictionary message = null;
                    if (!data["d"].IsNull)
                    {
                        message = data["d"].DataDictionary;
                    }
                    controllers[i].OnRoute(data["r"].String, fromPlayerId, message);
                }
            }
        }

        public abstract void SoupDefineRoutes();

        public void ClearRoutes()
        {
            routes = new string[0];
            controllers = new Controller[0];
        }

        protected void AddRoute(string route, Controller controller)
        {
            var newRoutes = new string[routes.Length + 1];
            var newControllers = new Controller[controllers.Length + 1];

            for (var i = 0; i < routes.Length; i++)
            {
                newRoutes[i] = routes[i];
                newControllers[i] = controllers[i];
            }

            newRoutes[routes.Length] = route;
            newControllers[routes.Length] = controller;

            routes = newRoutes;
            controllers = newControllers;
        }

        public void Send(string route, DataDictionary data = null)
        {
            Messenger.Send(createData(route, data));
        }

        public void SendLocal(string route, DataDictionary data = null)
        {
            Messenger.Send(createData(route, data), true);
        }

        public void SendMaster(string route, DataDictionary data = null)
        {
            Messenger.Send(createData(route, data), false, true);
        }

        private DataDictionary createData(string route, DataDictionary data)
        {

            var output = new DataDictionary();
            output["for"] = "router";
            output["r"] = route;
            output["d"] = data;
            return output;
        }
    }
}