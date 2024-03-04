using FS.Asset.Players;
using UnityEngine;

namespace FS.Cores.Cameras
{
    public class CameraFocus : MonoBehaviour
    {
        [SerializeField] private Vector3 OffsetPosition;

        private void Update()
        {
            if (PlayerController.Instance.Heroes.Count == 0)
                return;

            transform.position = PlayerController.Instance.Heroes[0].transform.position + OffsetPosition;
        }
    }
}