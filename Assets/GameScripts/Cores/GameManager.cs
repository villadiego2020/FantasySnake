using FS.Asset.Players;
using FS.Cores.Generators;
using FS.Statistics;
using System;
using System.Collections;
using UnityEngine;

namespace FS.Cores
{
    [DefaultExecutionOrder(ScriptOrders.GAME_MANAGER_ORDER)]
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance {  get; private set; }

        public float GameTime;
        public string GameTimerStr => SetTimer();
        public GameState GameState;

        public Action<GameState> OnGameStateUpdateEvent;


        private void Awake()
        {
            instance = this;

            SetState(GameState.Prepare);
        }

        private void Update()
        {
            if (GameState == GameState.Prepare || GameState == GameState.End)
                return;

            IncreaseGameTime();

            if (GameTime >= 30 && GameState != GameState.CrazyTime)
            {
                SetState(GameState.CrazyTime);
            }
        }

        private void IncreaseGameTime()
        {
            GameTime += Time.deltaTime;
        }

        #region State
        public void SetState(GameState state)
        {
            GameState = state;

            switch (GameState)
            {
                case GameState.Prepare:
                    StartCoroutine(nameof(GamePrepare));
                    break;
            }

            OnGameStateUpdateEvent?.Invoke(GameState);
        }

        private IEnumerator GamePrepare()
        {
            GameStatistic.Clear();

            Generator.Instance.Initialize();
            yield return new WaitUntil(() => Generator.Instance.IsDone);
            yield return new WaitForEndOfFrame();

            PlayerController.Instance.Initialize();
            yield return new WaitUntil(() => PlayerController.Instance.IsDone);
            yield return new WaitForEndOfFrame();

            SetState(GameState.Start);
        }
        #endregion

        private string SetTimer()
        {
            float minutes = Mathf.FloorToInt(GameTime / 60);
            float seconds = Mathf.FloorToInt(GameTime % 60);

            string timeStr = string.Format("{0:00}:{1:00}", Math.Clamp(minutes, 0, 59), Math.Clamp(seconds, 0, 59));

            return timeStr;
        }

        #region Testing
        [ContextMenu("Start Game")]
        public void TestStart()
        {
            SetState(GameState.Prepare);
        }
        #endregion
    }
}