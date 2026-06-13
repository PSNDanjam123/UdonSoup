
using UdonSharp;

namespace UdonSoup.Component.Data
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public abstract class StateLocal : State
    {
        public override void Sync() => Publish();
    }
}