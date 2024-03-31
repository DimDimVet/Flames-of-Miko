using FMODUnity;
using Registrator;
using UnityEngine;

namespace UI
{
    public class SettingsLvlMenu : SettingsUI
    {
        //public int test;
        //[Header("Установка eventFMOD Menu(Pause)")]
        //[SerializeField] protected EventReference eventAudioMenuPause;
        //protected EventInstance audioMenuPause;

        private Construction[] temples;
        private int tempCount;
        private int countTemleTemplesOff = 0;//

        [Header("Имя параметра уровня FMOD (Temples Off)")]
        [SerializeField] private string nameParametrFMODTemplesOff = "Temples Off";
        [Header("Максимальное значение параметра (int)")]
        [SerializeField] private int maxIntParametrFMODTemplesOff = 0;
        protected override void StartEvent()
        {
            panels.OnParametrUI += ParametrUI;
            panels.OnAudioClick += AudioClick;
            panels.OnAudioMuz += AudioMuz;
            panels.OnAudioPause += AudioPause;
            //panels.OnAudioFire += AudioFire;
            templeExecutor.OnFireTemple += FireTemple;
            templeExecutor.OnOffTemples += SetFireTemple;
        }
        protected override void ParametrUI(WinAudioSetting _winAudioSetting)
        {
            winAudioSetting = _winAudioSetting;
            if (!eventAudioFon.IsNull && !eventAudioClick.IsNull)
            {
                audioFon.setVolume(winAudioSetting.MuzVol);
                audioClick.setVolume(winAudioSetting.EfectVol);
            }
        }
        protected override void SetClass()
        {
            if (panels != null)
            {
                winAudioSetting = new WinAudioSetting()
                {
                    MinWidth = minWidth,
                    MinHeight = minHeight
                };

                PanelsLvl panelsLvl = new PanelsLvl()
                {
                    GroundPanel = groundPanel,
                    ButtonPanel = buttonPanel,
                    SettPanel = settPanel,
                    StatisticPanel = statisticPanel,
                    ManualPanel = manualPanel,
                };

                SceneIndex sceneIndex = new SceneIndex()
                {
                    MenuSceneIndex = menuSceneIndex,
                    GameSceneIndex = gameSceneIndex,
                    VictorySceneIndex = victorySceneIndex,
                    OverSceneIndex = overSceneIndex
                };

                panels.Set(winAudioSetting, panelsLvl, sceneIndex);

                if (!eventAudioFon.IsNull && !eventAudioClick.IsNull)
                {
                    audioFon = RuntimeManager.CreateInstance(eventAudioFon);
                    audioClick = RuntimeManager.CreateInstance(eventAudioClick);
                }
                panels.CallGroundPanel();
                panels.CallManualPanel();

                currentEventFMOD = eventFMODLvl;
                audioFon.start();
                panels.AudioMuz();
                isRun = true;
            }
        }
        private void AudioPause(bool isPause = false)
        {
            if (isPause)
            {
                currentEventFMOD = eventFMODPause;
            }
            else
            {
                currentEventFMOD = eventFMODLvl;
            }
            AudioMuz(true);
        }
        protected override void AudioMuz(bool isStart)
        {
            if (isStart)
            {
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName(nameParametrFMODGameState, currentEventFMOD);
            }
        }
        private void Update()
        {
            if (countTemleTemplesOff <= maxIntParametrFMODTemplesOff)
            {
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName(nameParametrFMODTemplesOff, countTemleTemplesOff);
            }
        }

        protected override void OnDisable()
        {
            audioFon.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            audioClick.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        #region AudioEffect
        private void CountUpdate()
        {
            tempCount = 0;
            for (int i = 0; i < temples.Length; i++)
            {
                if (temples[i].StatusTemle == StatusTemple.Null) { tempCount++; }
            }
            if (tempCount >= maxIntParametrFMODTemplesOff) { countTemleTemplesOff = maxIntParametrFMODTemplesOff; }
            else { countTemleTemplesOff = tempCount; }
        }
        private void SetFireTemple(Construction[] _temples)
        {
            temples = _temples;
            CountUpdate();
        }
        private void FireTemple(Construction temple, Construction[] _temples)
        {
            temples = _temples;
            CountUpdate();
        }
        #endregion
    }
}

