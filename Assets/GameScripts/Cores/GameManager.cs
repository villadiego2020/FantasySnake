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

        public GameState GameState { get; private set; } = GameState.Prepare;

        public Action<GameState> OnGameStateUpdateEvent;

        private void Awake()
        {
            instance = this;

            SetState(GameState.Prepare);
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
        }

        private IEnumerator GamePrepare()
        {
            MapGenerator.Instance.Initialize();
            yield return new WaitUntil(() => MapGenerator.Instance.IsDone);
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
            OnGameStateUpdateEvent?.Invoke(GameState);
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