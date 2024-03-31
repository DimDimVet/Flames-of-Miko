using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public interface IPanelsExecutor
    {
        bool SetOk { get; }
        void Set(WinAudioSetting _winAudioSetting, PanelsLvl _panelsLvl, SceneIndex _sceneIndex);
        void AudioSet();
        void AudioMuz(bool isStart = true);
        void AudioClick(bool isStart = true);
        void AudioPause(bool isPause = true);
        void AudioFire(int hash, bool isActiv);
        void AudioFireOff(int hash, bool isActiv);
        Action<WinAudioSetting> OnParametrUI { get; set; }
        Action<bool> OnAudioClick { get; set; }
        Action<bool> OnAudioMuz { get; set; }
        Action<bool> OnAudioPause { get; set; }
        Action<int, bool> OnAudioFire { get; set; }
        Action<int, bool> OnAudioFireOff { get; set; }
        void SetNewAudio(WinAudioSetting _winAudioSetting);
        void CallButtonPanel(bool isGroundPanel = false);
        void CallGroundPanel(bool isManualPanel = false);
        void CallStatisticaPanel(bool isGroundPanel = false);
        void CallSettPanel(bool isGroundPanel = false);
        void CallGameMenu();
        void CallManualPanel(bool isGroundPanel = false);
        void ExitGame();
        void MainMenu();
        void CallOverScene();
        void CallVictoryScene();
        Action<Resolution> OnSetResolution { get; set; }
        void ScreenSet(Resolution[] _resolutions, Resolution currentResolution);
        List<string> TextScreen { get; }
        int IndexCurrentScreen { get; }
        void SetNewResolution(int indexDrop);
        void SetSliders(float sliderLevelveryVol, float sliderTimeVol);
        Action<float, float> OnSetSliders { get; set; }
        Action<float, float, float> OnGetClock { get; set; }
        void SetClock(float secund, float minute, float hour);
        ActivPanel GetActivPanel();
        Action<int, bool> OnAudioStep { get; set; }
        void AudioStep(int hash, bool isActiv);
    }
}

