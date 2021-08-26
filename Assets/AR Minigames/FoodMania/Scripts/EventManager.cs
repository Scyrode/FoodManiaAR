using System;
using UnityEngine;

namespace Scyout.FoodMania
{
    public class EventManager : MonoBehaviour
    {
        public delegate void GameStartAction();
        public static event GameStartAction onGameStart;

        public delegate void GameEndAction();
        public static event GameEndAction onGameEnd;

        public delegate void IncrementScoreAction();
        public static event IncrementScoreAction onIncrementScore;

        private void OnEnable()
        {
            StartButtonBehaviour.onStartButtonPress += CallOnGameStart;
            BasketBehaviour.onCollectFood += CallIncrementScore;
            LoseRectangleBehaviour.onMissedFood += CallOnGameEnd;
        }

        private void OnDisable()
        {
            StartButtonBehaviour.onStartButtonPress -= CallOnGameStart;
            BasketBehaviour.onCollectFood -= CallIncrementScore;
            LoseRectangleBehaviour.onMissedFood -= CallOnGameEnd;
        }

        void CallIncrementScore()
        {
            onIncrementScore?.Invoke();
        }

        void CallOnGameEnd()
        {
            onGameEnd?.Invoke();
        }

        void CallOnGameStart()
        {
            onGameStart?.Invoke();
        }
    }
}
