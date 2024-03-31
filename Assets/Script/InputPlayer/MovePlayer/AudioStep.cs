using FMOD.Studio;
using FMODUnity;
using UI;
using UnityEngine;
using Zenject;

namespace Input
{
    public class AudioStep : MonoBehaviour
    {
        [Header("Установка eventFMOD Step")]
        [SerializeField] private EventReference eventAudioStep;
        private EventInstance audioStep;
        private int thisHash;
        private WinAudioSetting winAudioSetting;

        private IPanelsExecutor panels;
        [Inject]
        public void Init(IPanelsExecutor _panels)
        {
            panels = _panels;
        }
        private void Start()
        {
            SetClass();
            StartEvent();
        }
        private void StartEvent()
        {
            panels.OnParametrUI += ParametrUI;
            panels.OnAudioStep += Steps;
            panels.OnAudioPause += AudioPause;
        }
        private void ParametrUI(WinAudioSetting _winAudioSetting)
        {
            winAudioSetting = _winAudioSetting;
            if (!eventAudioStep.IsNull)
            {
                audioStep.setVolume(winAudioSetting.EfectVol);
            }
        }
        private void SetClass()
        {
            thisHash = gameObject.GetHashCode();
            if (panels != null)
            {
                if (!eventAudioStep.IsNull) { audioStep = RuntimeManager.CreateInstance(eventAudioStep); }
                panels.AudioSet();
            }
        }
        private void AudioPause(bool isPause = false)
        {
            audioStep.setPaused(isPause);
        }
        private void Steps(int hash, bool isActiv)
        {
            if (isActiv) { audioStep.start(); }
            else { audioStep.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); }
        }
        private void OnDisable()
        {
            audioStep.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}

