using FMOD.Studio;
using FMODUnity;
using UI;
using UnityEngine;
using Zenject;

namespace EntityLogic
{
    public class AudioEffectEntity : MonoBehaviour
    {
        [Header("Установка eventFMOD EffectEntity")]
        [SerializeField] private EventReference eventEffectEntity;
        private EventInstance audioEffectEntity;

        private int thisHash;
        private WinAudioSetting winAudioSetting;

        protected IPanelsExecutor panels;
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
            panels.OnAudioPause += AudioPause;
        }
        private void ParametrUI(WinAudioSetting _winAudioSetting)
        {
            winAudioSetting = _winAudioSetting;
            if (!eventEffectEntity.IsNull)
            {
                audioEffectEntity.setVolume(winAudioSetting.EfectVol);
            }
        }
        private void SetClass()
        {
            thisHash = gameObject.GetHashCode();
            if (panels != null)
            {
                panels.AudioSet();
                if (!eventEffectEntity.IsNull)
                {
                    audioEffectEntity = RuntimeManager.CreateInstance(eventEffectEntity);
                    audioEffectEntity.start();
                    //audioTempleOff.start();
                }
            }
        }
        void Update()
        {
            audioEffectEntity.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject));
        }
        private void AudioPause(bool isPause = false)
        {
            audioEffectEntity.setPaused(isPause);
        }
        public void OnDisable()
        {
            audioEffectEntity.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}
