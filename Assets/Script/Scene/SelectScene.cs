using Registrator;
using StatisticPlayer;
using TemleLogic;
using UI;
using UnityEngine;
using Zenject;

namespace SelectScenes
{
    public class SelectScene : MonoBehaviour
    {
        private Construction[] temples;
        private bool isStopClass = false, isRun = false;

        private IStatisticExecutor statistic;
        private IPanelsExecutor panels;
        [Inject]
        public void Init(IPanelsExecutor _panels, ITempleExecutor _templeExecutor, IStatisticExecutor _statistic)
        {
            panels = _panels;
            statistic = _statistic;
        }
        private void OnEnable()
        {
            panels.OnSetSliders += ContolStatusa;
        }
        void Start()
        {
            SetClass();
            statistic.InitStatistic();
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
        private void ContolStatusa(float sliderLevelveryVol, float sliderTimeVol)
        {
            if (sliderLevelveryVol <= 0) { panels.CallOverScene(); }
            //if (sliderLevelveryVol >= 100) { panels.CallVictoryScene(); }
        }
        void Update()
        {
            if (isStopClass) { return; }
            if (!isRun) { SetClass(); }
        }
        private void OnDisable()
        {
            panels.OnSetSliders -= ContolStatusa;
        }
    }
}

