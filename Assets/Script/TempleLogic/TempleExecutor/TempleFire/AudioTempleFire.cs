using FMOD.Studio;
using FMODUnity;
using UI;
using UnityEngine;
using Zenject;

namespace TemleLogic
{
    public class AudioTempleFire : MonoBehaviour
    {
        [Header("Установка eventFMOD FireTemple")]
        [SerializeField] private EventReference eventFireTemple;
        private EventInstance audioFireTemple;

        [Header("Установка eventFMOD TempleOff")]
        [SerializeField] private EventReference eventTempleOff;
        private EventInstance audioTempleOff;

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
            panels.OnAudioFire += AudioFire;
            panels.OnAudioFireOff += AudioFireOff;
        }
        private void ParametrUI(WinAudioSetting _winAudioSetting)
        {
            winAudioSetting = _winAudioSetting;
            if (!eventFireTemple.IsNull)
            {
                audioFireTemple.setVolume(winAudioSetting.EfectVol);
                audioTempleOff.setVolume(winAudioSetting.EfectVol);
            }
        }
        private void SetClass()
        {
            thisHash = gameObject.GetHashCode();
            if (panels != null)
            {
                panels.AudioSet();
                if (!eventFireTemple.IsNull)
                {
                    audioFireTemple = RuntimeManager.CreateInstance(eventFireTemple);
                    audioTempleOff = RuntimeManager.CreateInstance(eventTempleOff);
                    audioFireTemple.start();
                    //audioTempleOff.start();
                }
            }
        }
        void Update()
        {
            audioFireTemple.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject));
            audioTempleOff.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject));
        }
        private void AudioPause(bool isPause = false)
        {
            audioFireTemple.setPaused(isPause);
            audioTempleOff.setPaused(isPause);
        }
        private void AudioFire(int hash, bool isActiv)
        {
            if (hash == thisHash)
            {
                if (isActiv) { audioFireTemple.start(); }
                else { audioFireTemple.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); }
            }
        }
        private void AudioFireOff(int hash, bool isActiv)
        {
            if (hash == thisHash)
            {
                if (isActiv) { audioTempleOff.start(); }
                else { audioTempleOff.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); }
            }
        }
        public void OnDisable()
        {
            audioFireTemple.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            audioTempleOff.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}
