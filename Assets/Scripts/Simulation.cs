using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gavi
{
    public class Simulation : MonoBehaviour
    {
        public static Simulation Instance;

        [SerializeField][Range(0.0001f, 100f)] private float _stepsPerSecond = 3;
        public float StepsPerSecond { get => _stepsPerSecond; set => _stepsPerSecond = Mathf.Max(0.0001f, value); }

        public GameRule CurrentRule = GameRule.Conway;

        public bool IsRunning { get; private set; }
        private float _lastStepTime;
        public float StepTime => 1 / _stepsPerSecond;
        public int Generation { get; private set; }

        private int _historyDepth;
        private int _historyCurrent;
        
        #region Mono
        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
           if (IsRunning)
                UpdateSimulation();
        }
        #endregion


        #region Life Cycle
        public void StartSimulation()
        {
            if (!Board.Instance.IsPopulated)
                return;
            _lastStepTime = GaviTime.Time;
            IsRunning = true;
        }

        private void UpdateSimulation()
        {
            if (GaviTime.Time - _lastStepTime >= StepTime)
                PerformStep();
        }

        public void PerformStep()
        {
            foreach (Cell cell in Board.Instance.Cells1D)
                cell.PrepareStep();
            foreach (Cell cell in Board.Instance.Cells1D)
                cell.PerformStep();
            _lastStepTime = GaviTime.Time;
            Generation++;
        }

        public void PauseSimulation()
        {
            IsRunning = false;
        }

        public void Resume() => StartSimulation();
        public void Pause() => PauseSimulation();

        public void ToggleSimulation()
        {
            if (IsRunning)
                Pause();
            else
                Resume();
        }

        public void SetRule(GameRule rule)
        {
            CurrentRule = rule;
        }
        #endregion


        #region Generation
        public void ResetGenerationCount()
        {
            Generation = 0;
        }
        #endregion


        #region History

        #endregion
    }
}
