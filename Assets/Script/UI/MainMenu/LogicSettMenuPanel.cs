using Input;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class LogicSettMenuPanel : MonoBehaviour
    {
        [Header("Dropdown разрешений")]
        [SerializeField] private Dropdown screenDropdown;
        [Header("Slider звука")]
        [SerializeField] private Slider muzSlider;
        [SerializeField] private Slider effectSlider;
        [Header(" нопка продолжить")]
        [SerializeField] private Button returnButton;
        private WinAudioSetting winAudioSetting;

        private bool isStopClass = false, isRun = false;

        private IInputPlayerExecutor inputs;
        private IPanelsExecutor panels;
        [Inject]
        public void Init(IPanelsExecutor _panels, IInputPlayerExecutor _inputs)
        {
            panels = _panels;
            inputs = _inputs;
        }
        private void OnEnable()
        {
            panels.OnParametrUI += ParametrUI;
            panels.OnSetResolution += SetResolution;
            inputs.OnEventUpdata += InputEventUpdata;
        }
        private void InputEventUpdata(InputData data)
        {
            if (panels.GetActivPanel() == ActivPanel.SettPanel)
            {
                if (data.Menu > 0) { ButtonPanel(); }
            }
        }
        private void SetResolution(Resolution resolution)
        {
            Screen.SetResolution(resolution.width, resolution.height, true);
        }
        private void ParametrUI(WinAudioSetting _winAudioSetting)
        {
            winAudioSetting = _winAudioSetting;
        }
        void Start()
        {
            SetClass();
        }
        private void SetClass()
        {
            if (!isRun)
            {
                if (panels.SetOk)
                {
                    Resolution currentResolution = Screen.currentResolution;

                    panels.ScreenSet(Screen.resolutions, currentResolution);
                    screenDropdown.ClearOptions();
                    screenDropdown.AddOptions(panels.TextScreen);
                    screenDropdown.value = panels.IndexCurrentScreen;

                    panels.AudioSet();
                    muzSlider.value = winAudioSetting.MuzVol;
                    effectSlider.value = winAudioSetting.EfectVol;

                    isRun = true;
                    SetEventButton();
                }
                else { isRun = false; }
            }
        }
        private void SetEventButton()
        {
            returnButton.onClick.AddListener(ButtonPanel);
            screenDropdown.onValueChanged.AddListener(NewResolution);

            muzSlider.onValueChanged.AddListener(AudioContolValueMuz);
            effectSlider.onValueChanged.AddListener(AudioContolValueEffect);
        }
        private void ButtonPanel()
        {
            panels.CallButtonPanel(true);
            panels.AudioClick();
        }
        private void NewResolution(int indexDrop)
        {
            panels.SetNewResolution(indexDrop);
        }
        private void AudioContolValueMuz(float value)
        {
            AudioControl(value, winAudioSetting.EfectVol);
        }
        private void AudioContolValueEffect(float value)
        {
            AudioControl(winAudioSetting.MuzVol, value);
        }
        private void AudioControl(float valueMuz, float valueEffect)
        {
            winAudioSetting.MuzVol = valueMuz;
            winAudioSetting.EfectVol = valueEffect;
            panels.SetNewAudio(winAudioSetting);
        }
        void Update()
        {
            if (isStopClass) { return; }
            if (!isRun) { SetClass(); }
        }

    }

}