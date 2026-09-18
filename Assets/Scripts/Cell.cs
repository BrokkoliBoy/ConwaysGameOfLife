using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Gavi
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private Visual _visual;
        
        public bool IsAlive { set => SetAlive(value); get => _isAlive; }
        private bool _isAlive;

        
        public int PositionX { get; private set; }
        public int PositionY { get; private set; }
        private int AliveNeighbours => GetAliveNeighbours();        
        
        
        // state
        private bool _willBeAlive;
        private bool _willBeUnchanged;
        private bool _isInitialized;
        
        // neighbours
        private List<Cell> _neighbors = new List<Cell>();
        
        
        #region Life Cycle
        public void Initialize(int x, int y)
        {
            if (_isInitialized)
                return;
            _isInitialized = true;
            
            PositionX = x;
            PositionY = y;
            _visual.SetPosition(x, y);
        }

        public void PostInitialize()
        {
            _neighbors = Board.Instance.GetNeighbours(this);
            // SetAlive(false);
        }
        #endregion


        #region Update
        public void PrepareStep()
        {
            _willBeUnchanged = false;

            GameRule rule = Simulation.Instance.CurrentRule;
            int aliveNeighbours = AliveNeighbours;

            if (!IsAlive && Rules.ShouldBeBorn(rule, aliveNeighbours))
                _willBeAlive = true;
            else if (IsAlive && Rules.ShouldSurvive(rule, aliveNeighbours))
                _willBeUnchanged = true;
            else if (IsAlive)
                _willBeAlive = false;
            else
                _willBeUnchanged = true;
        }

        public void PerformStep()
        {
            if (_willBeUnchanged)
                return;
            IsAlive = _willBeAlive;
        }
        #endregion
        
        
        #region State
        private void SetAlive(bool isAlive)
        {
            // if (IsEdgeCell)
            //     return;
            
            if (isAlive && IsAlive)
                return;
            if (!isAlive && !IsAlive)
                return;

            _isAlive = isAlive;
            _visual.SetAlive(isAlive);
        }

        private int GetAliveNeighbours()
        {
            int aliveNeighbours = 0;
            foreach (Cell cell in _neighbors)
            {
                if (cell.IsAlive)
                    aliveNeighbours++;
            }

            return aliveNeighbours;
        }
        #endregion
    }
}
