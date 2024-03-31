using Input;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class ManualPanel : MonoBehaviour
    {
        [Header(" нопка вызова панели")]
        [SerializeField] private Button manualStartButton;
        [Header(" нопка закрыти€ панели")]
        [SerializeField] private Button manualExitButton;

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
            SetEventButton();
        }
        private void SetEventButton()
        {
            manualStartButton.onClick.AddListener(CallManualPanel);
            manualExitButton.onClick.AddListener(CallGroundPanel);
            inputs.OnEventUpdata += InputEventUpdata;
        }
        private void InputEventUpdata(InputData data)
        {
            //if (panels.GetActivPanel() == ActivPanel.ManualPanel )
            //{
            //    if (data.Menu > 0) { CallGroundPanel(); data.Mode = 1; }
            //}
        }
        private void SetClass()
        {
            if (!isRun)
            {
                if (panels != null)
                {
                    isRun = true;
                }
                else { isRun = false; }
            }
        }
        private void CallManualPanel()
        {
            panels.CallManualPanel();
            panels.AudioPause(true);
            panels.AudioClick();
        }
        private void CallGroundPanel()
        {
            panels.CallGroundPanel();
            panels.AudioPause(false);
            panels.AudioClick();
        }
        void Update()
        {
            if (isStopClass) { return; }
            if (!isRun) { SetClass(); }
        }
    }
}

