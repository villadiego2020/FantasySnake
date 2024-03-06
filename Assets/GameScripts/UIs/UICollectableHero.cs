using UnityEngine;

namespace FS.UIs
{
    public class UICollectableHero : MonoBehaviour
    {
        [SerializeField] private CanvasGroup m_CanvasGroup;

        public void Open()
        {
            m_CanvasGroup.alpha = 1.0f;
        }

        public void Close()
        {
            m_CanvasGroup.alpha = 0.0f;
        }
    }
}