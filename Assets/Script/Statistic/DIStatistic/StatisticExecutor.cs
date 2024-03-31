using Registrator;
using System;
using UI;
using UnityEngine;
using Zenject;

namespace StatisticPlayer
{
    public struct Statistic
    {
        public int ThisHash;
        public int CurrentHour;
        public int CurrentMinute;
        public int CurrentSecund;
        public int TopHour;
        public int TopMinute;
        public int TopSecund;
        public int SpecialMarks;//-1 0 +1
    }
    public class StatisticExecutor : IStatisticExecutor
    {
        public Action<Statistic> OnUpdateStatistic { get { return onUpdateStatistic; } set { onUpdateStatistic = value; } }
        private Action<Statistic> onUpdateStatistic;

        private int thisHash;

        private Statistic statistic;

        private IListDataExecutor dataList;
        private IPanelsExecutor panels;
        [Inject]
        public void Init(IPanelsExecutor _panels, IListDataExecutor _dataList)
        {
            panels = _panels;
            dataList = _dataList;
        }
        public bool InitStatistic()
        {
            thisHash = dataList.GetPlayer().Hash;
            statistic = new Statistic();
            statistic = GetStatistic();

            OnEnable();
            return true;
        }
        private void OnEnable()
        {
            panels.OnSetSliders += ContolStatusa;
            panels.OnGetClock += SetClock;
        }
        private void ContolStatusa(float sliderLevelveryVol, float sliderTimeVol)
        {
            if (sliderLevelveryVol <= 0)
            {
                statistic.SpecialMarks = SpecialMark();
                SetStatistic(statistic);
                OnDisable();
            }
            ////if (sliderLevelveryVol >= 100) { panels.CallVictoryScene(); }
        }
        private int SpecialMark()
        {
            if (statistic.TopHour == 0)
            {
                if (statistic.TopMinute == 0)
                {
                    if (statistic.TopSecund == 0) { return 0; }
                }
            }

            if (statistic.TopHour >= statistic.CurrentHour)
            {
                if (statistic.TopMinute >= statistic.CurrentMinute)
                {
                    if (statistic.TopSecund > statistic.CurrentSecund) { return -1; }
                }
            }

            if (statistic.TopHour <= statistic.CurrentHour)
            {
                if (statistic.TopMinute <= statistic.CurrentMinute)
                {
                    if (statistic.TopSecund < statistic.CurrentSecund) { return 1; }
                }
            }
            return 0;
        }
        private void SetClock(float secund, float minute, float hour)
        {
            statistic.CurrentHour = (int)hour;
            statistic.CurrentMinute = (int)minute;
            statistic.CurrentSecund = (int)secund;

            //if (isTicClock) { txtClock.text = String.Format("{0:00}:{1:00}.{2:00}", hour, minute, secund); }
            //else { txtClock.text = String.Format("{0:00}:{1:00} {2:00}", hour, minute, secund); }
            //isTicClock = !isTicClock;
        }
        private void UpdateStatistic(Statistic statistic)
        {
            onUpdateStatistic?.Invoke(statistic);
        }
        public void SetStatistic(Statistic statistic)
        {

            PlayerPrefs.SetInt("ThisHash", statistic.ThisHash);
            PlayerPrefs.SetInt("CurrentHour", statistic.CurrentHour);
            PlayerPrefs.SetInt("CurrentMinute", statistic.CurrentMinute);
            PlayerPrefs.SetInt("CurrentSecund", statistic.CurrentSecund);
            PlayerPrefs.SetInt("TopHour", statistic.TopHour);
            PlayerPrefs.SetInt("TopMinute", statistic.TopMinute);
            PlayerPrefs.SetInt("TopSecund", statistic.TopSecund);
            UpdateStatistic(statistic);
        }
        public Statistic GetStatistic()
        {
            Statistic statistic = new Statistic();
            statistic.ThisHash = PlayerPrefs.GetInt("ThisHash");
            statistic.CurrentHour = PlayerPrefs.GetInt("CurrentHour");
            statistic.CurrentMinute = PlayerPrefs.GetInt("CurrentMinute");
            statistic.CurrentSecund = PlayerPrefs.GetInt("CurrentSecund");

            statistic.TopHour = PlayerPrefs.GetInt("TopHour");
            statistic.TopMinute = PlayerPrefs.GetInt("TopMinute");
            statistic.TopSecund = PlayerPrefs.GetInt("TopSecund");
            return statistic;
        }
        public void OnDisable()
        {
            panels.OnSetSliders -= ContolStatusa;
            panels.OnGetClock -= SetClock;
        }
    }
}

