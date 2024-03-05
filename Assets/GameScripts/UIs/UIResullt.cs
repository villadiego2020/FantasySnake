using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FS.UIs
{
    public class UIResullt : MonoBehaviour
    {
        [SerializeField] private CanvasGroup m_CanvasGroup;
        [SerializeField] private TextMeshProUGUI m_MonsterDefeat;
        [SerializeField] private Button m_RetryButton;

        private void OnEnable()
        {
            m_RetryButton.onClick.AddListener(OnClickRetry);
        }

        private void OnDisable()
        {
            m_RetryButton.onClick.RemoveListener(OnClickRetry);
        }

        private void OnClickRetry()
        {
            SceneManager.LoadScene("scene_gameplay");
        }

        public void Open()
        {
            m_CanvasGroup.alpha = 1.0f;
        }

        public void SetMonsterDefeat(int amount)
        {
            m_MonsterDefeat.text = $"{amount}";
        }
    }
}