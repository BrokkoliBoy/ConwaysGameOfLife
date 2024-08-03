using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gavi
{
    public class VisualQuad : Visual
    {
        [SerializeField] private bool _fade = true;
        [Tooltip("In seconds.")]
        [SerializeField] private float _fadeTimeNoSimulation = 0.5f;
        [Tooltip("Tells over how many frames the fading should take place.")]
        [SerializeField] private float _fadeTimeSimulatingMultiplier = 3;
        [SerializeField] private float _fadeTimeSimulatingMin = 0.3f;
        [SerializeField] private float _fadeTimeSimulatingMax = 1f;
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
                FadeOut(Simulation.Instance.IsRunning ? Mathf.Clamp(Simulation.Instance.StepTime * _fadeTimeSimulatingMultiplier, _fadeTimeSimulatingMin, _fadeTimeSimulatingMax) : _fadeTimeNoSimulation);
        }

        private void FadeOut(float time)
        {
            if (_coroutineFadeOut != null)
                StopCoroutine(_coroutineFadeOut);
            _coroutineFadeOut = StartCoroutine(PerformFadeOut(time));
        }

        private IEnumerator PerformFadeOut(float time)
        {
            if (time < 0 || !_fade)
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
