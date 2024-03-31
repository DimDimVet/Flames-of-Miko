using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class ClickerHandleSlider : MonoBehaviour, IPointerClickHandler
    {
        private Slider slider;
        private int hashHandle, sliderEffectHash;

        private IPanelsExecutor panels;
        [Inject]
        public void Init(IPanelsExecutor _panels)
        {
            panels = _panels;
        }
        void Start()
        {
            slider = gameObject.GetComponent<Slider>();
            sliderEffectHash = slider.gameObject.GetHashCode();
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            hashHandle = eventData.pointerPress.GetHashCode();
            if (hashHandle == sliderEffectHash) { panels.AudioClick(); }
        }

    }
}
