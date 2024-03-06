using FS.Cores.Formulas;
using TMPro;
using UnityEngine;

namespace FS.Cores
{
    public class DamagePopup : MonoBehaviour
    {
        private const float DISAPPER_TIMER_MAX = 1f;

        [SerializeField] private TextMeshPro m_Message;
        [SerializeField] private float m_DisapperTimer = 3f;

        private Color m_TextColor;
        private Vector3 m_MoveVector;

        private void Awake()
        {
            m_Message = GetComponent<TextMeshPro>();
        }

        public void Setup(DamageMessage damage)
        {
            switch (damage.DamageHitFilter)
            {
                case DamageHitFilter.Normal:
                    m_Message.text = $"{damage.DamageNet}";
                    m_Message.color = Color.white;
                    m_Message.fontSize = 1;

                    float x = Random.Range(-0.5f, 0.5f);
                    float factor = Random.Range(7f, 15f);
                    m_MoveVector = new Vector3(x, 1f) * factor;
                    break;
            }

            m_TextColor = m_Message.color;
            m_DisapperTimer = DISAPPER_TIMER_MAX;
        }

        private void Update()
        {
            transform.position += m_MoveVector * Time.deltaTime;
            m_MoveVector -= m_MoveVector * 8f * Time.deltaTime;

            if (m_DisapperTimer > DISAPPER_TIMER_MAX * 0.5f)
            {
                float increaseScaleAmout = 1f;
                transform.localScale += Vector3.one * increaseScaleAmout * Time.deltaTime;
            }
            else
            {
                float increaseScaleAmout = 1f;
                transform.localScale -= Vector3.one * increaseScaleAmout * Time.deltaTime;
            }

            m_DisapperTimer -= Time.deltaTime;

            if (m_DisapperTimer < 0)
            {
                float disapperSpeed = 1f;
                m_TextColor.a -= disapperSpeed * Time.deltaTime;
                m_Message.color = m_TextColor;

                if (m_TextColor.a < 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}