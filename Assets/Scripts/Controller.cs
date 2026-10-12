using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gavi
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] private KeyCode _keyCodePerformStep = KeyCode.D;
        [SerializeField] private KeyCode _keyCodeGoBackInHistory = KeyCode.A;
        [Space]
        [SerializeField] private KeyCode _keyCodeRandomize = KeyCode.R;
        [SerializeField] private KeyCode _keyCodeKillAllCells = KeyCode.C;
        [Space] 
        [SerializeField] private KeyCode _keyCodeStartSimulation = KeyCode.Space;
        [SerializeField] private KeyCode _keyCodePauseSimulation = KeyCode.Space;

        [Header("- References -")] 
        [SerializeField] private Camera _camera;
        
        
        private void Update()
        {
            if (Input.GetKeyDown(_keyCodePerformStep))
                Simulation.Instance.PerformStep();
                
            else if (Input.GetKeyDown(_keyCodeRandomize))
                Board.Instance.Randomize();
            else if (Input.GetKeyDown(_keyCodeKillAllCells))
                Board.Instance.Clear();
            
            else if (Input.GetKeyDown(_keyCodeStartSimulation) && !Simulation.Instance.IsRunning)
                Simulation.Instance.StartSimulation();
            else if (Input.GetKeyDown(_keyCodePauseSimulation) && Simulation.Instance.IsRunning)
                Simulation.Instance.PauseSimulation();

            bool isLeftClick = Input.GetKey(KeyCode.Mouse0);
            bool isRightClick = Input.GetKey(KeyCode.Mouse1);
            if (isLeftClick)
                PerformMouseClick(true);
            else if (isRightClick)
                PerformMouseClick(false);
        }

        private void PerformMouseClick(bool isLeftClick)
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast (ray, out var hit, 100))
            {
                Cell cell = hit.collider.GetComponent<Cell>();
                if (cell == null)
                    return;
                cell.IsAlive = isLeftClick;
            }    
        }
    }
}
