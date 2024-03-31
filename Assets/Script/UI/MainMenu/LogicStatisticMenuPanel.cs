using Input;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class LogicStatisticMenuPanel : MonoBehaviour
    {
        [Header("Кнопка Назад")]
        [SerializeField] private Button returnButton;

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
                if (panels != null)
                {
                    isRun = true;
                    SetEventButton();
                }
                else { isRun = false; }
            }
        }
        private void SetEventButton()
        {
            returnButton.onClick.AddListener(ButtonPanel);
            inputs.OnEventUpdata += InputEventUpdata;
        }
        private void InputEventUpdata(InputData data)
        {
            if (panels.GetActivPanel() == ActivPanel.StatisticPanel)
            {
                if (data.Menu > 0) { ButtonPanel(); }
            }
        }
        private void ButtonPanel()
        {
            panels.CallButtonPanel(true);
            panels.AudioClick();
        }
        void Update()
        {
            if (isStopClass) { return; }
            if (!isRun) { SetClass(); }
        }
    }
}


