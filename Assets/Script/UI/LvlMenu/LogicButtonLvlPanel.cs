using Input;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class LogicButtonLvlPanel : MonoBehaviour
    {
        [Header("Кнопка возврат в Игру")]
        [SerializeField] private Button reternGameButton;
        [Header("Кнопка переиграть Игру")]
        [SerializeField] private Button reBootGameButton;
        [Header("Кнопка Настройка")]
        [SerializeField] private Button settButton;
        [Header("Кнопка Статистика")]
        [SerializeField] private Button statisticButton;
        [Header("Кнопка в Меню")]
        [SerializeField] private Button menuButton;

        private bool isStopClass = false, isRun = false;

        private IInputPlayerExecutor inputs;
        private IPanelsExecutor panels;
        [Inject]
        public void Init(IPanelsExecutor _panels, IInputPlayerExecutor _inputs)
        {
            panels = _panels;
            inputs = _inputs;
        }
        void Start()
        {
            SetClass();
        }
        private void SetClass()
        {
            if (!isRun)
            {
                if (panels != null) { isRun = true; SetEventButton();}
                else { isRun = false; }
            }
        }
        private void SetEventButton()
        {
            reternGameButton.onClick.AddListener(ReternGame);
            reBootGameButton.onClick.AddListener(ReBootGame);
            settButton.onClick.AddListener(SettPanel);
            statisticButton.onClick.AddListener(StatisticPanel);
            menuButton.onClick.AddListener(MainMenuPanel);
            inputs.OnEventUpdata += InputEventUpdata;
        }
        private void InputEventUpdata(InputData data)
        {
            //if (panels.GetActivPanel() == ActivPanel.ButtonPanel)
            //{
            //    if (data.Menu > 0) { ReternGame(); }
            //}
            
        }
        private void ReternGame()
        {
            panels.CallGroundPanel();
            panels.AudioPause(false);
        }
        private void ReBootGame()
        {
            panels.CallGameMenu();
            panels.AudioMuz(false);
        }
        private void SettPanel()
        {
            panels.CallSettPanel();
            panels.AudioClick();
        }
        private void StatisticPanel()
        {
            panels.CallStatisticaPanel();
            panels.AudioClick();
        }
        private void MainMenuPanel()
        {
            panels.MainMenu();
            panels.AudioClick();
        }
        void Update()
        {
            if (isStopClass) { return; }
            if (!isRun) { SetClass(); }
        }
    }
}


