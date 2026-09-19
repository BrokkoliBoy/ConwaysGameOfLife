using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gavi
{
    public class Board : MonoBehaviour
    {
        public static Board Instance;

        [SerializeField] private float _aliveChance = 0.5f;
        public float AliveChance { get => _aliveChance; set => _aliveChance = Mathf.Clamp01(value); }

        [Header("- References -")]
        [SerializeField] private Vector2Int _boardSize = new Vector2Int(10, 10);
        [SerializeField] private Cell _cellPrefab;
        [SerializeField] private Transform _parentCells;

        public List<List<Cell>> Cells { get; } = new List<List<Cell>>();
        public List<Cell> Cells1D { get; } = new List<Cell>();

        public bool IsPopulated { get; private set; }
        
        
        #region Mono
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            PopulateBoard();
        }

        #endregion

        
        #region Life Cycle
        public void PopulateBoard()
        {
            if (IsPopulated)
                return;
            IsPopulated = true;
            
            for (int x = 0; x < _boardSize.x; x++)
            {
                Cells.Add(new List<Cell>());
                for (int y = 0; y < _boardSize.y; y++)
                {
                    Cell cell = Instantiate(_cellPrefab, _parentCells);
                    Cells[x].Add(cell);
                    Cells1D.Add(cell);
                }
            }

            for (int x = 0; x < _boardSize.x; x++)
            {
                for (int y = 0; y < _boardSize.y; y++)
                {
                    Cells[x][y].Initialize(x, y);
                }
            }
            
            for (int x = 0; x < _boardSize.x; x++)
            {
                for (int y = 0; y < _boardSize.y; y++)
                {
                    Cells[x][y].PostInitialize();
                }
            }
        }
        #endregion


        #region Cell Control
        public void Randomize()
        {
            foreach (Cell cell in Cells1D)
                cell.IsAlive = Random.Range(0f, 1f) < _aliveChance;
            Simulation.Instance.ResetGenerationCount();
        }

        public void Clear()
        {
            foreach (Cell cell in Cells1D)
                cell.IsAlive = false;
            Simulation.Instance.ResetGenerationCount();
        }
        #endregion


        public List<Cell> GetNeighbours(Cell cell)
        {
            List<Cell> neighbours = new List<Cell>();
            int cellX = cell.PositionX;
            int cellY = cell.PositionY;

            for (int x = cellX - 1; x <= cellX + 1; x++)
            {
                for (int y = cellY - 1; y <= cellY + 1; y++)
                {
                    if (x == cellX && y == cellY)
                        continue;
                    
                    int xUsed = x;
                    if (x < 0) 
                        xUsed = _boardSize.x - 1;
                    if (x >= _boardSize.x) 
                        xUsed = 0;
                    
                    int yUsed = y;
                    if (y < 0) 
                        yUsed = _boardSize.y - 1;
                    if (y >= _boardSize.y) 
                        yUsed = 0;
                    
                    neighbours.Add(Cells[xUsed][yUsed]);
                }
            }
            
            return neighbours;
        }
    }
}
