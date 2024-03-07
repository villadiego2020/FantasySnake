using FS.Cores;
using System.Collections;
using UnityEngine;

namespace FS.UIs
{
    public class UIFader : MonoBehaviour
    {
        [SerializeField] private float m_Speed;
        [SerializeField] private CanvasGroup m_CanvasGroup;

        private void OnEnable()
        {
            GameManager.instance.OnGameStateUpdateEvent += GameStateChange;
        }
        private void OnDisable()
        {
            GameManager.instance.OnGameStateUpdateEvent -= GameStateChange;
        }

        private void GameStateChange(GameState state)
        {
            if (state == GameState.CrazyTime)
            {
                StartCoroutine(nameof(StartFade));
            }
            else
            {
                m_CanvasGroup.alpha = 0;
                StopCoroutine(nameof(StartFade));
            }
        }

        private IEnumerator StartFade()
        {
            bool isFadeIn = true;

            while(true)
            {
                if(isFadeIn)
                {
                    m_CanvasGroup.alpha = Mathf.Lerp(m_CanvasGroup.alpha, 1, Time.deltaTime * m_Speed);

                    if(m_CanvasGroup.alpha >= 0.9f)
                    {
                        isFadeIn = false;
                    }
                }
                else
                {
                    m_CanvasGroup.alpha = Mathf.Lerp(m_CanvasGroup.alpha, 0, Time.deltaTime * m_Speed);

                    if (m_CanvasGroup.alpha <= 0.1f)
                    {
                        isFadeIn = true;
                    }
                }

                yield return null;
            }
        }
    }
}