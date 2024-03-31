using Input;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class LogicButtonMenuPanel : MonoBehaviour
    {
        [Header("Кнопка Игра")]
        [SerializeField] private Button gameButton;
        [Header("Кнопка Настройка")]
        [SerializeField] private Button settButton;
        [Header("Кнопка Статистика")]
        [SerializeField] private Button statisticButton;
        [Header("Кнопка Выход")]
        [SerializeField] private Button exitButton;

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
                if (panels != null) { isRun = true; SetEventButton(); }
                else { isRun = false; }
            }
        }
        private void SetEventButton()
        {
            settButton.onClick.AddListener(SettPanel);
            gameButton.onClick.AddListener(GameMenu);
            statisticButton.onClick.AddListener(StatisticPanel);
            exitButton.onClick.AddListener(ExitPanel);
            inputs.OnEventUpdata += InputEventUpdata;
        }
        private void InputEventUpdata(InputData data)
        {
            //if (panels.GetActivPanel() == ActivPanel.ButtonPanel)
            //{
            //    if (data.Menu > 0) { ExitPanel(); }
            //}
        }
        private void SettPanel()
        {
            panels.CallSettPanel();
            panels.AudioClick();
        }
        private void GameMenu()
        {
            panels.CallGameMenu();
            panels.AudioMuz(false);
        }
        private void StatisticPanel()
        {
            panels.CallStatisticaPanel();
            panels.AudioClick();
        }
        private void ExitPanel()
        {
            panels.ExitGame();
            panels.AudioClick();
        }
        void Update()
        {
            if (isStopClass) { return; }
            if (!isRun) { SetClass(); }
        }
    }
}


