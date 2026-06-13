
using UdonSharp;
using VRC.SDKBase;

namespace UdonSoup.Core
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public abstract class Network : Base
    {
        public virtual void Sync()
        {
            SetOwner();
            RequestSerialization();
        }

        public void SetOwner()
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
        }

        public bool IsOwner()
        {
            return Networking.IsOwner(Networking.LocalPlayer, gameObject);
        }
    }
}