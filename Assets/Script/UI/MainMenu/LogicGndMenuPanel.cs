using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UI
{
    public class LogicGndMenuPanel : MonoBehaviour/*, IPointerClickHandler*/
    {
        private bool isStopClass = false, isRun = false;

        private IPanelsExecutor panels;
        [Inject]
        public void Init(IPanelsExecutor _panels)
        {
            panels = _panels;
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
                    ButtonPanel();
                    panels.AudioMuz();
                    isRun = true;
                }
                else { isRun = false; }
            }
        }
        private void ButtonPanel()
        {
            panels.CallButtonPanel();
        }
        //public void OnPointerClick(PointerEventData eventData)
        //{
        //    Debug.Log("click");
        //}
        void Update()
        {
            if (isStopClass) { return; }
            if (!isRun) { SetClass(); }
        }
    }
}

