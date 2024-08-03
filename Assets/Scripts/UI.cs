using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gavi
{
    public class UI : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI _textStepCount;

        private void Update()
        {
            _textStepCount.text = "" + Simulation.Instance.Generation;
        }
    }
}
