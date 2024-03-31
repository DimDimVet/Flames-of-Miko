using FMODUnity;
using UnityEngine;

namespace UI
{
    public class SettingsUIMenu : SettingsUI
    {
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

                if (manualPanel == null) { manualPanel = new GameObject(); }
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

                currentEventFMOD = eventFMODMenu;
                panels.AudioMuz();
                panels.CallButtonPanel(true);
                isRun = true;
            }
        }
    }
}

