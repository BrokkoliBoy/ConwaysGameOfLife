using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gavi
{
    public class GaviTime : MonoBehaviour
    {
        public static GaviTime Instance;
        
        private static float TimeScale;
        [SerializeField] private float _timeScale = 1;
        public static float Time { get { TrySimulate(); return _time; } }
        public static float DeltaTime { get { TrySimulate(); return _deltaTime; } }

        private static float _time;
        private static float _deltaTime;

        private static bool _simulatedThisFrame;

        
        #region Mono
        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            TimeScale = _timeScale;
            TrySimulate();
        }

        private void LateUpdate()
        {
            _simulatedThisFrame = false;
        }
        #endregion


        private static void TrySimulate()
        {
            if (_simulatedThisFrame)
                return;
            _simulatedThisFrame = true;
            Simulate();
        }

        private static void Simulate()
        {
            _deltaTime = UnityEngine.Time.deltaTime * TimeScale;
            _time += _deltaTime;
        }
    }
}
