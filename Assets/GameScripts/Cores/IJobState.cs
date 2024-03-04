using UnityEngine;

namespace FS.Cores
{
    public abstract class IJobState : MonoBehaviour, IInitalize
    {
        public bool IsDone;

        public abstract void Initialize(params object[] objects);
        public abstract void Deinitialize();
        public abstract void Register();
        public abstract void Unregister();
    }
}