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

        private void Awake()
        {
            Instance = this;

            PlayBGM();
        }

        public void PlayBGM()
        {
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