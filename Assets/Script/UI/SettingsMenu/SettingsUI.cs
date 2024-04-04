using FMOD.Studio;
using FMODUnity;
using Input;
using TemleLogic;
using UnityEngine;
using Zenject;

namespace UI
{
    public class SettingsUI : MonoBehaviour
    {
        [Header("MenuSceneIndex")]
        [SerializeField] protected int menuSceneIndex = 0;
        [Header("GameSceneIndex")]
        [SerializeField] protected int gameSceneIndex = 1;
        [Header("VictorySceneIndex")]
        [SerializeField] protected int victorySceneIndex = 2;
        [Header("OverSceneIndex")]
        [SerializeField] protected int overSceneIndex = 3;

        [Header("Мин ширина(width)")]
        [SerializeField] protected int minWidth = 1280;
        [Header("Мин высота(height)")]
        [SerializeField] protected int minHeight = 1024;

        [Header("Панели")]
        [SerializeField] protected GameObject groundPanel;
        [SerializeField] protected GameObject buttonPanel;
        [SerializeField] protected GameObject settPanel;
        [SerializeField] protected GameObject statisticPanel;
        [SerializeField] protected GameObject manualPanel;

        [Header("Установка eventFMOD Fon")]
        [SerializeField] protected EventReference eventAudioFon;
        protected EventInstance audioFon;

        [Header("Имя параметра-селектора FMOD (Game State)")]
        [SerializeField] protected string nameParametrFMODGameState = "Game State";
        [Header("Меню (очередь в eventFMOD")]
        [SerializeField] protected int eventFMODMenu = 0;
        [Header("Уровень (очередь в eventFMOD")]
        [SerializeField] protected int eventFMODLvl = 1;
        [Header("Пауза (очередь в eventFMOD")]
        [SerializeField] protected int eventFMODPause = 2;
        [Header("Поражение (очередь в eventFMOD")]
        [SerializeField] protected int eventFMODOver = 3;
        protected int currentEventFMOD;

        [Header("Установка eventFMOD Click")]
        [SerializeField] protected EventReference eventAudioClick;
        protected EventInstance audioClick;

        protected WinAudioSetting winAudioSetting;
        protected bool isStopClass = false, isRun = false;

        protected ITempleExecutor templeExecutor;
        protected IInputPlayerExecutor inputs;
        protected IPanelsExecutor panels;
        [Inject]
        public virtual void Init(IPanelsExecutor _panels, IInputPlayerExecutor _inputs, ITempleExecutor _templeExecutor)
        {
            panels = _panels;
            inputs = _inputs;
            templeExecutor = _templeExecutor;
        }
        void Start()
        {
            StartEvent();
            SetClass();
        }
        protected virtual void StartEvent()
        {
            panels.OnParametrUI += ParametrUI;
            panels.OnAudioClick += AudioClick;
            panels.OnAudioMuz += AudioMuz;
        }
        protected virtual void AudioClick(bool isClick)
        {
            if (!eventAudioClick.IsNull && isClick) { audioClick.start(); }
            else { audioClick.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); }
        }
        protected virtual void AudioMuz(bool isStart)
        {
            if (isStart)
            {
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName(nameParametrFMODGameState, currentEventFMOD);
                audioFon.start();
            }
            else { audioFon.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); }
        }
        protected virtual void ParametrUI(WinAudioSetting _winAudioSetting)
        {
            winAudioSetting = _winAudioSetting;
            if (!eventAudioFon.IsNull && !eventAudioClick.IsNull)
            {
                audioFon.setVolume(winAudioSetting.MuzVol);
                audioClick.setVolume(winAudioSetting.EfectVol);
            }
        }
        protected virtual void SetClass()
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
        protected virtual void OnDisable()
        {
            audioFon.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            audioClick.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

            panels.OnParametrUI -= ParametrUI;
            panels.OnAudioClick -= AudioClick;
            panels.OnAudioMuz -= AudioMuz;
        }
        void Update()
        {
            if (isStopClass) { return; }
            if (!isRun) { SetClass(); }
        }
    }
}

