using FS.Characters;
using UnityEngine;

namespace FS.Cores
{
    [DefaultExecutionOrder(ScriptOrders.AUDIO_ORDER)]
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource m_BGMSource;
        [SerializeField] private AudioSource m_SFXSource;

        [SerializeField] private AudioClip m_BGM;
        [SerializeField] private AudioClip m_Hit;
        [SerializeField] private AudioClip m_Move;

        public static AudioManager Instance { get; private set; }

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
                PlayBGM(1.5f);
            }
        }

        private void Awake()
        {
            Instance = this;
            PlayBGM(1.0f);
        }

        public void PlayBGM(float pitch)
        {
            m_BGMSource.pitch = pitch;
            m_BGMSource.clip = m_BGM;
            m_BGMSource.Play();
        }

        public void Hit()
        {
            m_SFXSource.PlayOneShot(m_Hit);
        }

        public void Move()
        {
            m_SFXSource.PlayOneShot(m_Move);
        }
    }
}