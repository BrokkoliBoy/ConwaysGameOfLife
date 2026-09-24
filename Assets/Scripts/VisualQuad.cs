using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gavi
{
    public class VisualQuad : Visual
    {
        [Tooltip("In seconds. Shared by all cells, controllable at runtime via the UI.")]
        public static float FadeTime = 0.2f;
        [SerializeField] private AnimationCurve _fadeCurve = AnimationCurve.Linear(0, 1, 1, 0);

        
        [Header("- References -")]
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private Material _materialAlive;
        [SerializeField] private Material _materialFading;
        [SerializeField] private Material _materialDead;

        private Coroutine _coroutineFadeOut;
        
        public override void SetAlive(bool isAlive)
        {
            if (isAlive)
            {
                if (_coroutineFadeOut != null)
                    StopCoroutine(_coroutineFadeOut);
                _renderer.sharedMaterial = _materialAlive;
            }
            else
                FadeOut(FadeTime);
        }

        private void FadeOut(float time)
        {
            if (_coroutineFadeOut != null)
                StopCoroutine(_coroutineFadeOut);
            _coroutineFadeOut = StartCoroutine(PerformFadeOut(time));
        }

        private IEnumerator PerformFadeOut(float time)
        {
            if (time <= 0)
            {
                _renderer.sharedMaterial = _materialDead;
                yield break;
            }

            _renderer.material = _materialFading;
            float timePassed = 0;
            while (timePassed < time)
            {
                timePassed += GaviTime.DeltaTime;
                float timeRate = timePassed / time;
                Color color = Color.Lerp(_materialAlive.color, _materialDead.color, _fadeCurve.Evaluate(timeRate));
                _renderer.material.color = color;
                yield return null;
            }
            
            _renderer.sharedMaterial = _materialDead;
        }
    }
}
