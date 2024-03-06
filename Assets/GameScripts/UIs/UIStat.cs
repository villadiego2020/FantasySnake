using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FS.UIs
{
    public class UIStat : MonoBehaviour
    {
        [SerializeField] private CanvasGroup m_CanvasGroup;
        [SerializeField] private TextMeshProUGUI m_HPValue;
        [SerializeField] private Image m_HPImage;
        [SerializeField] private TextMeshProUGUI m_AttackValue;

        public void Open()
        {
            m_CanvasGroup.alpha = 1.0f;
        }

        public void Close()
        {
            m_CanvasGroup.alpha = 0.0f;
        }

        public void AdjustStat(int maxHP, int currentHP, int attack)
        {
            AdjustHP(maxHP, currentHP);
            AdjustAttack(attack);
        }

        public void AdjustHP(int maxHP, int currentHP)
        {
            m_HPValue.text = $"{currentHP} / {maxHP}";
            m_HPImage.fillAmount = (float)currentHP / (float)maxHP;
        }

        public void AdjustAttack(int attack)
        {
            m_AttackValue.text = $"{attack}";
        }
    }
}