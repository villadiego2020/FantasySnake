using FS.Asset.Players;
using FS.Cores.MapGenerators;
using System;
using System.Collections;
using UnityEngine;

namespace FS.Cores
{
    [DefaultExecutionOrder(ScriptOrders.GAME_MANAGER_ORDER)]
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance {  get; private set; }

        public float GameTime { get; private set; } = 0;
        public GameState GameState { get; private set; } = GameState.Prepare;

        public Action<GameState> OnGameStateUpdateEvent;

        private void Awake()
        {
            instance = this;

            SetState(GameState.Prepare);
        }

        private void Update()
        {
            if (GameState != GameState.Start)
                return;

            IncreaseGameTime();
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

                case GameState.Start:
                    GameStart();
                    break;

                case GameState.Pause:
                    GamePause();
                    break;

                case GameState.End:
                    GameEnd();
                    break;
            }

            OnGameStateUpdateEvent?.Invoke(GameState);
        }

        private IEnumerator GamePrepare()
        {
            Generator.Instance.Initialize();
            yield return new WaitUntil(() => Generator.Instance.IsDone);
            yield return new WaitForEndOfFrame();

            PlayerController.Instance.Initialize();
            yield return new WaitUntil(() => PlayerController.Instance.IsDone);
            yield return new WaitForEndOfFrame();

            SetState(GameState.Start);
        }

        private void GameStart()
        {
        }

        private void GamePause()
        {

        }

        private void GameEnd()
        {

        }
        #endregion

        #region Testing
        [ContextMenu("Start Game")]
        public void TestStart()
        {
            SetState(GameState.Prepare);
        }
        #endregion
    }
}