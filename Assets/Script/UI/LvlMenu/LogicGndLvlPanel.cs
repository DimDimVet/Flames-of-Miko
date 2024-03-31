using Input;
using StatisticPlayer;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class LogicGndLvlPanel : MonoBehaviour
    {
        [Header(" нопка продолжить")]
        [SerializeField] private Button butPanelButton;
        [Header("SliderLevelvery")]
        [SerializeField] private Slider sliderLevelvery;
        [Header("TextClock")]
        [SerializeField] private Text txtClock;
        private bool isTicClock = false;
        //[Header("SliderTime")]
        //[SerializeField] private Slider sliderTime;

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
            butPanelButton.onClick.AddListener(ButtonPanel);
            panels.OnSetSliders += SetSliders;
            panels.OnGetClock += SetClock;
            inputs.OnEventUpdata += InputEventUpdata;
        }
        private void InputEventUpdata(InputData data)
        {
            if (panels.GetActivPanel() == ActivPanel.GndPanel )
            {
                if (data.Menu > 0) { ButtonPanel(); data.Mode = 1; }
            }
        }
        private void SetClock(float secund, float minute, float hour)
        {
            if (isTicClock) { txtClock.text = String.Format("{0:00}:{1:00}.{2:00}", hour, minute, secund); }
            else { txtClock.text = String.Format("{0:00}:{1:00} {2:00}", hour, minute, secund); }
            isTicClock = !isTicClock;
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
        private void ButtonPanel()
        {
            panels.CallButtonPanel();
            panels.AudioPause(true);
            panels.AudioClick();
        }
        private void SetSliders(float sliderLevelveryVol, float sliderTimeVol)
        {
            sliderLevelvery.value = sliderLevelveryVol;
            //sliderTime.value = sliderTimeVol;
        }
        void Update()
        {
            if (isStopClass) { return; }
            if (!isRun) { SetClass(); }
        }
    }
}

