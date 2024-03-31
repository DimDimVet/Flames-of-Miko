using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public struct WinAudioSetting
    {
        public float MuzVol;
        public float EfectVol;

        public int[] MinWidth;
        public int[] MinHeight;
    }
    public struct PanelsLvl
    {
        public GameObject GroundPanel;
        public GameObject ButtonPanel;
        public GameObject SettPanel;
        public GameObject StatisticPanel;
        public GameObject ManualPanel;
    }
    public struct SceneIndex
    {
        public int MenuSceneIndex;
        public int GameSceneIndex;
        public int VictorySceneIndex;
        public int OverSceneIndex;
    }
    public enum ActivPanel
    {
        GndPanel, ButtonPanel, SettPanel, StatisticPanel, ManualPanel,
    }
    public class PanelsExecutor : IPanelsExecutor
    {
        public Action<WinAudioSetting> OnParametrUI { get { return onParametrUI; } set { onParametrUI = value; } }
        private Action<WinAudioSetting> onParametrUI;
        public Action<bool> OnAudioClick { get { return onAudioClick; } set { onAudioClick = value; } }
        private Action<bool> onAudioClick;
        public Action<bool> OnAudioMuz { get { return onAudioMuz; } set { onAudioMuz = value; } }
        private Action<bool> onAudioMuz;
        public Action<bool> OnAudioPause { get { return onAudioPause; } set { onAudioPause = value; } }
        private Action<bool> onAudioPause;
        public Action<int, bool> OnAudioFire { get { return onAudioFire; } set { onAudioFire = value; } }
        private Action<int, bool> onAudioFire;
        public Action<int, bool> OnAudioFireOff { get { return onAudioFireOff; } set { onAudioFireOff = value; } }
        private Action<int, bool> onAudioFireOff;
        public Action<int, bool> OnAudioStep { get { return onAudioStep; } set { onAudioStep = value; } }
        private Action<int, bool> onAudioStep;
        public Action<Resolution> OnSetResolution { get { return onSetResolution; } set { onSetResolution = value; } }
        private Action<Resolution> onSetResolution;
        public Action<float, float> OnSetSliders { get { return onSetSliders; } set { onSetSliders = value; } }
        private Action<float, float> onSetSliders;
        public Action<float, float, float> OnGetClock { get { return onGetClock; } set { onGetClock = value; } }
        private Action<float, float, float> onGetClock;

        private WinAudioSetting winAudioSetting;
        private PanelsLvl panelsLvl;
        private SceneIndex sceneIndex;
        private ActivPanel activPanel;
        //Screen
        private List<string> textScreen;
        public List<string> TextScreen { get { return textScreen; } }
        private int indexCurrentScreen;
        public int IndexCurrentScreen { get { return indexCurrentScreen; } }
        private Resolution currentScreen;
        private Resolution[] resolutions, tempResolutions;
        public bool SetOk { get { return setOk; } }
        private bool setOk = false;

        public void Set(WinAudioSetting _winAudioSetting, PanelsLvl _panelsLvl, SceneIndex _sceneIndex)
        {
            //OnEnable();
            winAudioSetting = new WinAudioSetting();
            winAudioSetting = _winAudioSetting;
            panelsLvl = new PanelsLvl();
            panelsLvl = _panelsLvl;
            sceneIndex = new SceneIndex();
            sceneIndex = _sceneIndex;
            AudioSet();
            setOk = true;
        }

        #region Panels
        public void CallGroundPanel(bool isManualPanel = false)
        {
            if (setOk)
            {
                activPanel = ActivPanel.GndPanel;
                panelsLvl.GroundPanel.SetActive(true);
                panelsLvl.ButtonPanel.SetActive(false);
                panelsLvl.SettPanel.SetActive(false);
                panelsLvl.StatisticPanel.SetActive(false);
                if (isManualPanel) { panelsLvl.ManualPanel.SetActive(isManualPanel); }
                else { panelsLvl.ManualPanel.SetActive(isManualPanel); }
                GameTimer(true);
            }
        }
        public void CallButtonPanel(bool isGroundPanel = false)
        {
            if (setOk)
            {
                activPanel = ActivPanel.ButtonPanel;
                if (isGroundPanel) { panelsLvl.GroundPanel.SetActive(isGroundPanel); }
                else { panelsLvl.GroundPanel.SetActive(isGroundPanel); }
                panelsLvl.ButtonPanel.SetActive(true);
                panelsLvl.SettPanel.SetActive(false);
                panelsLvl.StatisticPanel.SetActive(false);
                panelsLvl.ManualPanel.SetActive(false);
                GameTimer(false);
            }
        }
        public void CallSettPanel(bool isGroundPanel = false)
        {
            if (setOk)
            {
                activPanel = ActivPanel.SettPanel;
                if (isGroundPanel) { panelsLvl.GroundPanel.SetActive(isGroundPanel); }
                else { panelsLvl.GroundPanel.SetActive(isGroundPanel); }
                panelsLvl.ButtonPanel.SetActive(false);
                panelsLvl.SettPanel.SetActive(true);
                panelsLvl.StatisticPanel.SetActive(false);
                panelsLvl.ManualPanel.SetActive(false);
                GameTimer(false);
            }
        }
        public void CallStatisticaPanel(bool isGroundPanel = false)
        {
            if (setOk)
            {
                activPanel = ActivPanel.StatisticPanel;
                if (isGroundPanel) { panelsLvl.GroundPanel.SetActive(isGroundPanel); }
                else { panelsLvl.GroundPanel.SetActive(isGroundPanel); }
                panelsLvl.ButtonPanel.SetActive(false);
                panelsLvl.SettPanel.SetActive(false);
                panelsLvl.StatisticPanel.SetActive(true);
                panelsLvl.ManualPanel.SetActive(false);
                GameTimer(false);
            }
        }
        public void CallManualPanel(bool isGroundPanel = false)
        {
            if (setOk)
            {
                activPanel = ActivPanel.ManualPanel;
                if (isGroundPanel) { panelsLvl.GroundPanel.SetActive(isGroundPanel); }
                else { panelsLvl.GroundPanel.SetActive(isGroundPanel); }
                panelsLvl.ButtonPanel.SetActive(false);
                panelsLvl.SettPanel.SetActive(false);
                panelsLvl.StatisticPanel.SetActive(false);
                panelsLvl.ManualPanel.SetActive(true);
                GameTimer(false);
            }
        }
        public ActivPanel GetActivPanel()
        {
            return activPanel;
        }

        #endregion
        #region Scene
        public void ReBootScene()
        {
            GameTimer(true);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        public void MainMenu()
        {
            GameTimer(true);
            SceneManager.LoadScene(sceneIndex.MenuSceneIndex);
        }
        public void CallGameMenu()
        {
            GameTimer(true);
            SceneManager.LoadScene(sceneIndex.GameSceneIndex);
        }
        public void CallVictoryScene()
        {
            GameTimer(false);
            SceneManager.LoadScene(sceneIndex.VictorySceneIndex);
        }
        public void CallOverScene()
        {
            GameTimer(false);
            SceneManager.LoadScene(sceneIndex.OverSceneIndex);
        }
        public void ExitGame()
        {
            Application.Quit();
        }
        #endregion
        #region Audio
        public void AudioSet()
        {
            GetAudioParametr();
            onParametrUI?.Invoke(winAudioSetting);
        }
        public void SetNewAudio(WinAudioSetting _winAudioSetting)
        {
            SetAudioParametr(_winAudioSetting.MuzVol, _winAudioSetting.EfectVol);
            AudioSet();
        }
        public void AudioClick(bool isStart = true)
        {
            onAudioClick?.Invoke(isStart);
        }
        public void AudioMuz(bool isStart = true)
        {
            onAudioMuz?.Invoke(isStart);
        }
        public void AudioPause(bool isPause = true)
        {
            onAudioPause?.Invoke(isPause);
        }
        public void AudioFire(int hash, bool isActiv)
        {
            onAudioFire?.Invoke(hash, isActiv);
        }
        public void AudioFireOff(int hash, bool isActiv)
        {
            onAudioFireOff?.Invoke(hash, isActiv);
        }
        public void AudioStep(int hash, bool isActiv)
        {
            onAudioStep?.Invoke(hash, isActiv);
        }
        #endregion
        #region Screen
        public void ScreenSet(Resolution[] _resolutions, Resolution currentResolution)
        {
            currentScreen = GetResolution();
            RefreshRate tt = currentScreen.refreshRateRatio;
            SetCurrentResolution(currentScreen);

            textScreen = new List<string>();
            tempResolutions = _resolutions;

            for (int i = 0; i < tempResolutions.Length; i++)
            {
                if (RezultWidth(tempResolutions[i].width) & RezulHeight(tempResolutions[i].height) /*& tempResolutions[i].refreshRateRatio.value == currentResolution.refreshRateRatio.value*/)
                {
                    resolutions = CreatResolution(tempResolutions[i], resolutions);
                    textScreen.Add($"{tempResolutions[i].width}x{tempResolutions[i].height} {(int)tempResolutions[i].refreshRateRatio.value}Ãö");
                }
            }

            for (int i = 0; i < resolutions.Length; i++)
            {
                if (resolutions[i].width == currentScreen.width & resolutions[i].height == currentScreen.height)
                {
                    indexCurrentScreen = i; break;
                }
            }
        }
        private bool RezultWidth(int inWidth)
        {
            for (int i = 0; i < winAudioSetting.MinWidth.Length; i++)
            {
                if (winAudioSetting.MinWidth[i] == inWidth) { return true; }
            }
            return false;
        }
        private bool RezulHeight(int inHeight)
        {
            for (int i = 0; i < winAudioSetting.MinHeight.Length; i++)
            {
                if (winAudioSetting.MinHeight[i] == inHeight) { return true; }
            }
            return false;
        }
        private Resolution[] CreatResolution(Resolution intObject, Resolution[] massivObject)
        {
            if (massivObject != null)
            {
                int newLength = massivObject.Length + 1;
                Array.Resize(ref massivObject, newLength);
                massivObject[newLength - 1] = intObject;
                return massivObject;
            }
            else
            {
                massivObject = new Resolution[] { intObject };
                return massivObject;
            }
        }
        private void SetCurrentResolution(Resolution _currentScreen)
        {
            if (_currentScreen.width == 0 & _currentScreen.height == 0)
            {
                _currentScreen.width = winAudioSetting.MinWidth[0];
                _currentScreen.height = winAudioSetting.MinHeight[0];
            }
            onSetResolution?.Invoke(_currentScreen);
        }
        public void SetNewResolution(int indexDrop)
        {
            currentScreen.width = resolutions[indexDrop].width;
            currentScreen.height = resolutions[indexDrop].height;
            SetResolution(currentScreen);

            SetCurrentResolution(currentScreen);
        }
        #endregion
        #region EPROM
        private void SetResolution(Resolution currentScreen)
        {
            PlayerPrefs.SetInt("CurrentWidth", currentScreen.width);
            PlayerPrefs.SetInt("CurrentHeight", currentScreen.height);
        }
        private Resolution GetResolution()
        {
            Resolution temp = new Resolution();
            temp.width = PlayerPrefs.GetInt("CurrentWidth");
            temp.height = PlayerPrefs.GetInt("CurrentHeight");

            return temp;
        }
        private void SetAudioParametr(float muzVol, float efectVol)
        {
            PlayerPrefs.SetFloat("CurrentMuzVol", muzVol);
            PlayerPrefs.SetFloat("CurrentEfectVol", efectVol);
            winAudioSetting.MuzVol = muzVol;
            winAudioSetting.EfectVol = efectVol;
        }
        private void GetAudioParametr()
        {
            winAudioSetting.MuzVol = PlayerPrefs.GetFloat("CurrentMuzVol");
            winAudioSetting.EfectVol = PlayerPrefs.GetFloat("CurrentEfectVol");
        }
        #endregion
        #region Sliders
        public void SetSliders(float sliderLevelveryVol, float sliderTimeVol)
        {
            onSetSliders?.Invoke(sliderLevelveryVol, sliderTimeVol);
        }
        #endregion
        #region Clock
        public void SetClock(float secund, float minute, float hour)
        {
            onGetClock?.Invoke(secund, minute, hour);
        }
        #endregion
        private void GameTimer(bool isRun)
        {
            if (isRun) { Time.timeScale = 1; }
            else { Time.timeScale = 0; }
        }
    }
}

