using UnityEngine;

namespace FS.Cores
{
    public class LookAtCamera : MonoBehaviour
    {
        void LateUpdate()
        {
            if (Camera.main == null)
                return;

            transform.forward = Camera.main.transform.forward;
        }
    }
}