using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gavi
{
    public class UI : MonoBehaviour
    {
        [Header("- Generation -")]
        [SerializeField] private TextMeshProUGUI _textGenerationCount;

        [Header("- Steps Per Second -")]
        [Tooltip("The slider itself stays a plain 0-1 UI Slider; this curve remaps that normalized " +
                 "position onto the min/max range below, so the front of the slider covers small " +
                 "steps-per-second values at fine resolution and the back covers large values coarsely. " +
                 "Must stay monotonically increasing from (0,0) to (1,1).")]
        [SerializeField] private AnimationCurve _stepsPerSecondCurve = new AnimationCurve(
            new Keyframe(0f, 0f, 0f, 0.6f),
            new Keyframe(1f, 1f, 1.4f, 1.4f));
        [SerializeField] private float _stepsPerSecondMin = 0.1f;
        [SerializeField] private float _stepsPerSecondMax = 50f;
        [SerializeField] private Slider _sliderStepsPerSecond;
        [SerializeField] private TextMeshProUGUI _textStepsPerSecondValue;

        [Header("- Density -")]
        [SerializeField] private Slider _sliderAliveChance;
        [SerializeField] private TextMeshProUGUI _textAliveChanceValue;

        [Header("- Fade Time -")]
        [SerializeField] private Slider _sliderFadeTime;
        [SerializeField] private TextMeshProUGUI _textFadeTimeValue;

        [Header("- Rule -")]
        [SerializeField] private TMP_Dropdown _dropdownRule;

        [Header("- Buttons -")]
        [SerializeField] private Button _buttonPauseResume;
        [SerializeField] private Image _iconPauseResume;
        [SerializeField] private Sprite _iconPause;
        [SerializeField] private Sprite _iconResume;
        [SerializeField] private Button _buttonStepForward;
        [SerializeField] private Button _buttonRandomize;
        [SerializeField] private Button _buttonClear;


        #region Mono
        private void Start()
        {
            SyncFromCurrentState();
            BindListeners();
        }

        private void Update()
        {
            _textGenerationCount.text = "" + Simulation.Instance.Generation;
            _iconPauseResume.sprite = Simulation.Instance.IsRunning ? _iconPause : _iconResume;
        }
        #endregion


        #region Setup
        private void SyncFromCurrentState()
        {
            float stepsPerSecondT = Mathf.InverseLerp(_stepsPerSecondMin, _stepsPerSecondMax, Simulation.Instance.StepsPerSecond);
            _sliderStepsPerSecond.SetValueWithoutNotify(InverseEvaluateMonotonic(_stepsPerSecondCurve, stepsPerSecondT));
            _textStepsPerSecondValue.text = FormatFloat(Simulation.Instance.StepsPerSecond);

            _sliderAliveChance.SetValueWithoutNotify(Board.Instance.AliveChance);
            _textAliveChanceValue.text = FormatFloat(Board.Instance.AliveChance);

            _sliderFadeTime.SetValueWithoutNotify(VisualQuad.FadeTime);
            _textFadeTimeValue.text = FormatFloat(VisualQuad.FadeTime);

            _dropdownRule.SetValueWithoutNotify((int)Simulation.Instance.CurrentRule);
        }

        private void BindListeners()
        {
            _sliderStepsPerSecond.onValueChanged.AddListener(SetStepsPerSecond);
            _sliderAliveChance.onValueChanged.AddListener(SetAliveChance);
            _sliderFadeTime.onValueChanged.AddListener(SetFadeTime);
            _dropdownRule.onValueChanged.AddListener(SetRule);

            _buttonPauseResume.onClick.AddListener(() => { Simulation.Instance.ToggleSimulation(); ClearSelection(); });
            _buttonStepForward.onClick.AddListener(() => { Simulation.Instance.PerformStep(); ClearSelection(); });
            _buttonRandomize.onClick.AddListener(() => { Board.Instance.Randomize(); ClearSelection(); });
            _buttonClear.onClick.AddListener(() => { Board.Instance.Clear(); ClearSelection(); });
        }

        private static string FormatFloat(float value) => value.ToString("0.0");

        // Buttons keep keyboard focus after being clicked, so a later "Submit" keypress
        // (e.g. Space, used for Pause/Resume) would silently re-trigger whichever button
        // was clicked last. Clearing the selection after every click prevents that.
        private static void ClearSelection() => EventSystem.current.SetSelectedGameObject(null);

        // Numerically inverts a monotonically increasing curve from (0,0) to (1,1) via bisection,
        // since AnimationCurve has no built-in inverse.
        private static float InverseEvaluateMonotonic(AnimationCurve curve, float target, int iterations = 24)
        {
            float lo = 0f, hi = 1f;
            for (int i = 0; i < iterations; i++)
            {
                float mid = (lo + hi) * 0.5f;
                if (curve.Evaluate(mid) < target)
                    lo = mid;
                else
                    hi = mid;
            }
            return (lo + hi) * 0.5f;
        }
        #endregion


        #region Setters
        public void SetStepsPerSecond(float sliderValue)
        {
            float t = _stepsPerSecondCurve.Evaluate(sliderValue);
            Simulation.Instance.StepsPerSecond = Mathf.Lerp(_stepsPerSecondMin, _stepsPerSecondMax, t);
            _textStepsPerSecondValue.text = FormatFloat(Simulation.Instance.StepsPerSecond);
        }

        public void SetAliveChance(float value)
        {
            Board.Instance.AliveChance = value;
            _textAliveChanceValue.text = FormatFloat(Board.Instance.AliveChance);
        }

        public void SetFadeTime(float value)
        {
            VisualQuad.FadeTime = value;
            _textFadeTimeValue.text = FormatFloat(VisualQuad.FadeTime);
        }

        public void SetRule(int ruleIndex)
        {
            Simulation.Instance.SetRule((GameRule)ruleIndex);
        }
        #endregion
    }
}
