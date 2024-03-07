using FS.Cores;
using System;
using TMPro;
using UnityEngine;

namespace FS.UIs
{
    public class UIGameScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_Timer;
        [SerializeField] private GameObject m_ScreenRed;

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
            if(state == GameState.CrazyTime)
            {
                m_Timer.color = Color.red;
                m_ScreenRed.SetActive(true);
            }
            else if( state == GameState.End)
            {
                m_ScreenRed.SetActive(false);
                m_Timer.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            SetTime();
        }

        private void SetTime()
        {
            m_Timer.text = $"{GameManager.instance.GameTimerStr}";
        }
    }
}