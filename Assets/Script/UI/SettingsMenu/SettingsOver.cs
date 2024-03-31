using FMODUnity;
using Input;
using StatisticPlayer;
using TemleLogic;
using Zenject;

namespace UI
{
    public class SettingsOver : SettingsUI
    {
        private IStatisticExecutor statistic;
        [Inject]
        public override void Init(IPanelsExecutor _panels, IInputPlayerExecutor _inputs, ITempleExecutor _templeExecutor)
        {
            panels = _panels;
            inputs = _inputs;
            templeExecutor = _templeExecutor;
        }
        protected override void SetClass()
        {
            inputs.Enable();
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

                currentEventFMOD = eventFMODOver;
                panels.AudioMuz();
                //panels.CallButtonPanel(true);
                isRun = true;
            }
        }
    }
}

