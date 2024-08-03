using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gavi
{
    public class Master : MonoBehaviour
    {
        public static Master Instance;

        #region Mono
        private void Awake()
        {
            Instance = this;
        }
        #endregion


        #region Life Cycle
        public void Initialize()
        {
            Board.Instance.PopulateBoard();
        }
        #endregion
    }
}
